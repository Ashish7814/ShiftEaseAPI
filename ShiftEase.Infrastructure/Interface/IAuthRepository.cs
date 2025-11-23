using Microsoft.AspNetCore.Identity;
using ShiftEase.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Infrastructure.Interface
{
    public interface IAuthRepository
    {
        //SignUp 
        Task<ApplicationUser> FindByNameAsync(string username);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(ApplicationRole role);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        //SignIn
        Task<ApplicationUser> FindByEmailAsync(string username);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        //Forgot Password
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
    }
}
