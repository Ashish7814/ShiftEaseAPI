using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftEase.Core.Interface;

namespace ShiftEaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;
        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("employee-count")]
        public async Task<IActionResult> EmployeeCount()
        {
            try
            {
                var employeeCounts = await _managerService.GetEmployeeByRolesAsync();
                return Ok(employeeCounts.Select(c => new { Role = c.Role, Count = c.Count }));
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
