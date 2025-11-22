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
        private readonly IConfiguration _configuration;
        private readonly ISignUpService _signUpService;
        private readonly ISignInService _signInService;

        public AuthController(
            IConfiguration configuration,
            ISignUpService signUpService,
            ISignInService signInService)
        {
            _configuration = configuration;
            _signUpService = signUpService;
            _signInService = signInService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var result = await _signInService.LoginAsync(model);
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
                var response = await _signUpService.RegisterAsync(model);
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

        
    }

}