using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ShiftEase.Core.Interface;
using ShiftEase.EF.Models;
using ShiftEase.Infrastructure.Implementation;
using ShiftEase.Infrastructure.Interface;
using ShiftEase.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IAuthRepository authRepository, TokenService tokenService, IEmailService emailService, ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Response> RegisterAsync(RegisterModel model)
        {
            try
            {
                var existing = await _authRepository.FindByNameAsync(model.Username);
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
                var result = await _authRepository.CreateUserAsync(user, model.Password);
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
                if (!await _authRepository.RoleExistsAsync(model.Role))
                {
                    var roleResult = await _authRepository.CreateRoleAsync(new ApplicationRole { Name = model.Role });
                    if (!roleResult.Succeeded)
                    {
                        var roleErrors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                        return new Response { Status = "Error", Message = $"Failed to create role: {roleErrors}" };
                    }
                }

                // 5. Add user to role
                var addRoleResult = await _authRepository.AddToRoleAsync(user, model.Role);
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

        public async Task<LoginResponseDto?> LoginAsync(LoginModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model?.Username) || string.IsNullOrWhiteSpace(model?.Password))
                    return null;

                ApplicationUser user = null;

                if (model.Username.Contains("@"))
                {
                    user = await _authRepository.FindByEmailAsync(model.Username);
                }
                else
                {
                    user = await _authRepository.FindByNameAsync(model.Username);
                }

                if (user == null)
                    return null;

                var passwordOk = await _authRepository.CheckPasswordAsync(user, model.Password);
                if (!passwordOk)
                    return null;

                var userRoles = await _authRepository.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var role in userRoles)
                    authClaims.Add(new Claim(ClaimTypes.Role, role));

                var jwtToken = _tokenService.GetToken(authClaims);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                var primaryRole = userRoles.FirstOrDefault() ?? "User";

                return new LoginResponseDto
                {
                    Token = tokenString,
                    Expiration = jwtToken.ValidTo,
                    Role = primaryRole,
                    UserId = user.Id
                };
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }


        public async Task<Response> ForgotPasswordAsync(ForgotPasswordModel model, string frontendResetUrlBase)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model?.Email))
                    return new Response { Status = "Error", Message = "Invalid email." };

                var user = await _authRepository.FindByEmailAsync(model.Email);

                // ALWAYS return success to avoid leaking if email exists or not
                if (user == null)
                {
                    // optionally log attempted email
                    _logger.LogInformation("ForgotPassword requested for non-existing email: {Email}", model.Email);
                    return new Response { Status = "Success", Message = "If a user with that email exists, you will receive a password reset email." };
                }

                var token = await _authRepository.GeneratePasswordResetTokenAsync(user);

                // encode token for URL safety
                var tokenBytes = Encoding.UTF8.GetBytes(token);
                var encodedToken = WebEncoders.Base64UrlEncode(tokenBytes);

                var email = user.Email;
                var resetLink = $"{frontendResetUrlBase}?email={WebUtility.UrlEncode(email)}&token={WebUtility.UrlEncode(encodedToken)}";

                var subject = "Reset your password";
                var body = $@"
                <p>Hello {user.UserName},</p>
                <p>Click the link below to reset your password. This link will expire in the same time as Identity's token expiry (default: depends on your config).</p>
                <p><a href='{resetLink}'>Reset password</a></p>
                <p>If you did not request this, ignore this email.</p>";

                await _emailService.SendAsync(email, subject, body);

                return new Response { Status = "Success", Message = "If a user with that email exists, you will receive a password reset email." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ForgotPasswordAsync");
                return new Response { Status = "Error", Message = "Unable to process request." };
            }
        }

        public async Task<Response> ResetPasswordAsync(ResetPasswordModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model?.Email) ||
                    string.IsNullOrWhiteSpace(model?.Token) ||
                    string.IsNullOrWhiteSpace(model?.NewPassword))
                {
                    return new Response { Status = "Error", Message = "Invalid data." };
                }

                var user = await _authRepository.FindByEmailAsync(model.Email);
                if (user == null)
                    return new Response { Status = "Error", Message = "Invalid request." }; // don't give details

                // decode token
                byte[] tokenBytes;
                try
                {
                    tokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                }
                catch
                {
                    return new Response { Status = "Error", Message = "Invalid token." };
                }

                var decodedToken = Encoding.UTF8.GetString(tokenBytes);

                var result = await _authRepository.ResetPasswordAsync(user, decodedToken, model.NewPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return new Response { Status = "Error", Message = $"Reset failed: {errors}" };
                }

                return new Response { Status = "Success", Message = "Password has been reset successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ResetPasswordAsync");
                return new Response { Status = "Error", Message = "Unable to reset password." };
            }
        }
    }
}
