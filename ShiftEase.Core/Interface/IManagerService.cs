using ShiftEase.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Interface
{
    public interface IManagerService
    {
        Task<List<(string Role, int Count)>> GetEmployeeByRolesAsync();
    }
}
