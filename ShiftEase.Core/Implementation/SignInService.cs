using ShiftEase.Core.Interface;
using ShiftEase.EF.Models;
using ShiftEase.Infrastructure.Interface;
using ShiftEase.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Implementation
{
    public class SignInService : ISignInService
    {
        private readonly ISignInRepository _signInRepository;
        private readonly TokenService _tokenService;
        public SignInService(ISignInRepository signInRepository, TokenService tokenService)
        {
            _signInRepository = signInRepository;
            _tokenService = tokenService;
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
                    user = await _signInRepository.FindByEmailAsync(model.Username);
                }
                else
                {
                    user = await _signInRepository.FindByNameAsync(model.Username);
                }

                if (user == null)
                    return null;

                var passwordOk = await _signInRepository.CheckPasswordAsync(user, model.Password);
                if (!passwordOk)
                    return null;

                var userRoles = await _signInRepository.GetRolesAsync(user);

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
    }
}
