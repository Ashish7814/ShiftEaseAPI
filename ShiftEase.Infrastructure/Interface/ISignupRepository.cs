using Microsoft.AspNetCore.Identity;
using ShiftEase.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Infrastructure.Interface
{
    public interface ISignupRepository
    {
        Task<ApplicationUser> FindByNameAsync(string username);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(ApplicationRole role);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    }
}
