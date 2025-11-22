using Microsoft.AspNetCore.Identity;
using ShiftEase.EF.Models;
using ShiftEase.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Infrastructure.Implementation
{
    public class SignupRepository : ISignupRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public SignupRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public Task<ApplicationUser> FindByNameAsync(string username)
        => _userManager.FindByNameAsync(username);

        public Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
            => _userManager.CreateAsync(user, password);

        public Task<bool> RoleExistsAsync(string roleName)
            => _roleManager.RoleExistsAsync(roleName);

        public Task<IdentityResult> CreateRoleAsync(ApplicationRole role)
            => _roleManager.CreateAsync(role);

        public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
            => _userManager.AddToRoleAsync(user, role);
    }
}
