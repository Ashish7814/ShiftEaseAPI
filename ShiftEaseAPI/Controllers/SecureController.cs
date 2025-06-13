using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShiftEaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SecureController : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndpoint()
        {
            return Ok(new { message = $"Hello {User.Identity.Name}. This is Admin endpoint." });
        }

        [HttpGet("manager")]
        [Authorize(Roles = "Manager")]
        public IActionResult ManagerEndpoint()
        {
            return Ok(new { message = $"Hello {User.Identity.Name}. This is Manager endpoint." });
        }

        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public IActionResult UserEndpoint()
        {
            return Ok(new { message = $"Hello {User.Identity.Name}. This is User endpoint." });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Manager,User")]
        public IActionResult AllRolesEndpoint()
        {
            return Ok(new { message = $"Hello {User.Identity.Name}. This endpoint accepts all authenticated users." });
        }
    }
}