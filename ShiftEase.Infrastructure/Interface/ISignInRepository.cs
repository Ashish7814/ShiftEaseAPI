using ShiftEase.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Infrastructure.Interface
{
    public interface ISignInRepository
    {
        Task<ApplicationUser> FindByNameAsync(string username);
        Task<ApplicationUser> FindByEmailAsync(string username);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
    }
}
