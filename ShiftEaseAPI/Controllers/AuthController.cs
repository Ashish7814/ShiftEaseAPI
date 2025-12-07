using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShiftEaseAPI.Service.Account.DTOs;
using ShiftEaseAPI.Service.Account.User;
using ShiftEaseAPI.Service.EmailTemplate;

namespace ShiftEaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public AuthController(IUserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var result = await _userService.LoginAsync(model);
                if (result == null)
                    return Unauthorized();

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                ResponseDto response = await _userService.RegisterAsync(model);
                if (response.Status == "Error")
                {
                   return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                ResponseDto result = await _emailSender.ConfirmEmailAsync(userId, token);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Status);
                }
                return StatusCode(StatusCodes.Status200OK, result.Status);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            } 
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            try
            {
                var response = await _userService.ForgotPasswordAsync(model);
                if (response.Status == "Error")
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
                }
                return StatusCode(StatusCodes.Status200OK, response.Status);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            try
            {
                ResponseDto response = await _userService.ResetPasswordAsync(userId, token);
                if (response.Status == "Error")
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
                }
                return StatusCode(StatusCodes.Status200OK, response.Status);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }
    }

}