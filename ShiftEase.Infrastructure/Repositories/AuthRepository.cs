using Microsoft.AspNetCore.Identity;
using ShiftEase.Core.Entities;
using ShiftEase.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            try
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }
            catch { 
                // Log exception (not implemented here)
                throw new Exception("An error occurred while checking the user's password.");
            }
        }

        public async Task<ApplicationUser> FindByUsernameAsync(string username)
        {
            try
            {
                return await _userManager.FindByNameAsync(username);
            }
            catch (Exception ex)
            {
                // Log exception (not implemented here)
                throw new Exception("An error occurred while finding the user by username.", ex);
            }
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            try
            {
                return await _userManager.GetRolesAsync(user);
            }
            catch (Exception ex)
            {
                // Log exception (not implemented here)
                throw new Exception("An error occurred while retrieving user roles.", ex);
            }
        }
    }
}
