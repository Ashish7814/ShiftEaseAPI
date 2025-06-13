using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Interface
{
    public interface IAdminService
    {
        Task<List<(string Role, int Count)>> GetUserCountsByRolesAsync();
    }
}
