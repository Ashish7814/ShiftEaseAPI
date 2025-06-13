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
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<(string Role, int Count)>> GetUserCountsByRolesAsync()
        {
            var result = new List<(string Role, int Count)>();

            if (_userManager == null || _roleManager == null)
                throw new InvalidOperationException("UserManager or RoleManager is not initialized.");

            var roles = _roleManager.Roles
                .Where(r => r.Name != "Admin")
                .Select(r => r.Name)
                .ToList();

            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);

                // Optional: Skip roles with 0 users if desired
                if (usersInRole != null && usersInRole.Any())
                {
                    result.Add((role, usersInRole.Count));
                }
            }

            return result;
        }
    }
}
