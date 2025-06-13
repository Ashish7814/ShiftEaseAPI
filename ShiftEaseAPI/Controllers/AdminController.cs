using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftEase.Core.Implementation;
using ShiftEase.Core.Interface;

namespace ShiftEaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("count-by-role")]
        public async Task<IActionResult> GetUserCountsByRole()
        {
            var counts = await adminService.GetUserCountsByRolesAsync();
            return Ok(counts.Select(c => new { Role = c.Role, Count = c.Count }));
        }
    }
}
