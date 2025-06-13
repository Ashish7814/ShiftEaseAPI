using ShiftEase.Core.Interface;
using ShiftEase.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
       public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<List<(string Role, int Count)>> GetUserCountsByRolesAsync()
        {
            return await _adminRepository.GetUserCountsByRolesAsync();
        }
    }
}
