using ShiftEase.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Infrastructure.Interface
{
    public interface IAdminRepository
    {
        Task<List<(string Role, int Count)>> GetUserCountsByRolesAsync();
    }
}
