using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Infrastructure.Interface
{
    public interface IManagerRepository
    {
        Task<List<(string Role, int Count)>> getEmployeeByRolesAsync();
    }
}
