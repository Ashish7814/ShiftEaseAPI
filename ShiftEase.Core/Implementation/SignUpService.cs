using ShiftEase.Core.Interface;
using ShiftEase.EF.Models;
using ShiftEase.Infrastructure.Interface;
using ShiftEase.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Implementation
{
    public class SignUpService : ISignUpService
    {
        private readonly ISignupRepository _signupRepository;
        public SignUpService(ISignupRepository signupRepository)
        {
            _signupRepository = signupRepository;
        }

        public async Task<Response> RegisterAsync(RegisterModel model)
        {
            try
            {
                var existing = await _signupRepository.FindByNameAsync(model.Username);
                if (existing != null)
                {
                    return new Response { Status = "Error", Message = "User already exists!" };
                }

                // 2. Build user entity
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                // 3. Create user
                var result = await _signupRepository.CreateUserAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return new Response
                    {
                        Status = "Error",
                        Message = $"User creation failed! {errors}"
                    };
                }

                // 4. Ensure role exists
                if (!await _signupRepository.RoleExistsAsync(model.Role))
                {
                    var roleResult = await _signupRepository.CreateRoleAsync(new ApplicationRole { Name = model.Role });
                    if (!roleResult.Succeeded)
                    {
                        var roleErrors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                        return new Response { Status = "Error", Message = $"Failed to create role: {roleErrors}" };
                    }
                }

                // 5. Add user to role
                var addRoleResult = await _signupRepository.AddToRoleAsync(user, model.Role);
                if (!addRoleResult.Succeeded)
                {
                    var roleAddErrors = string.Join("; ", addRoleResult.Errors.Select(e => e.Description));
                    return new Response { Status = "Error", Message = $"Failed to add role to user: {roleAddErrors}" };
                }

                // Success
                return new Response { Status = "Success", Message = "User created successfully!" };
            }

            catch (Exception ex)
            {
                return new Response
                {
                    Status = "Error",
                    Message = ex.Message
                };
            }
        }
    }
}
