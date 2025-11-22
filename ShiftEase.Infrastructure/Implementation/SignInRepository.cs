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
    public class SignInRepository : ISignInRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public SignInRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public Task<ApplicationUser> FindByNameAsync(string username)
        => _userManager.FindByNameAsync(username);
        public Task<ApplicationUser> FindByEmailAsync(string username)
            => _userManager.FindByEmailAsync(username);

        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
            => _userManager.CheckPasswordAsync(user, password);

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
            => _userManager.GetRolesAsync(user);
    }
}
