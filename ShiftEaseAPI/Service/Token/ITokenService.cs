using ShiftEaseAPI.Models;
using ShiftEaseAPI.Service.Account.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace ShiftEaseAPI.Service.Token
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> GenerateJwtTokenAsync(ApplicationUser user);
    }
}
