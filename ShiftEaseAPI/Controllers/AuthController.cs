using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShiftEase.EF.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShiftEase.Core.Implementation;
using ShiftEase.Core.Interface;
using ShiftEase.Shared.DTOs;

namespace ShiftEaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;

        public AuthController(
            IConfiguration config, IAuthService authService)
        {
            _config = config;
            _authService = authService; 
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var result = await _authService.LoginAsync(model);
                if (result == null)
                    return Unauthorized();

                return Ok(new
                {
                    result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var response = await _authService.RegisterAsync(model);
                if (response.Status == "Error")
                {
                   return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            // Example: read frontend base URL from config
            var frontendUrl = _config["AppSettings:FrontendResetUrl"] ?? "https://yourfrontend.com/reset-password";
            var response = await _authService.ForgotPasswordAsync(model, frontendUrl);
            if (response.Status == "Error")
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var response = await _authService.ResetPasswordAsync(model);
            if (response.Status == "Error")
                return BadRequest(response);
            return Ok(response);
        }
    }

}