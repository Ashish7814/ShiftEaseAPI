using ShiftEase.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> FindByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
    }
}
