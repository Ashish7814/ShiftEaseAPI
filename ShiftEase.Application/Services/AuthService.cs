using ShiftEase.Application.Interfaces;
using ShiftEase.Core.Interfaces;
using ShiftEase.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly TokenService _tokenService;

        public AuthService(IAuthRepository authRepository, TokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto model)
        {
            try
            {
                var user = await _authRepository.FindByUsernameAsync(model.Username);
                if (user == null || !await _authRepository.CheckPasswordAsync(user, model.Password))
                    return null;

                var roles = await _authRepository.GetUserRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var role in roles)
                    authClaims.Add(new Claim(ClaimTypes.Role, role));

                var token = _tokenService.GenerateToken(authClaims);

                return new LoginResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    Role = roles.FirstOrDefault() ?? "User"
                };
            }
            catch (Exception ex)
            {
                // Log exception (not implemented here)
                throw new Exception("An error occurred during login.", ex);
            }
        }
    }
}
