using ShiftEase.Core.Interface;
using ShiftEase.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Implementation
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        public async Task<List<(string Role, int Count)>> GetEmployeeByRolesAsync()
        {
            return await _managerRepository.getEmployeeByRolesAsync();
        }
    }
}
