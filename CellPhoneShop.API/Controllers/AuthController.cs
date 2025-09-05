using CellPhoneShop.Business.DTOs.Auth;
using CellPhoneShop.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace CellPhoneShop.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
		{
			try
			{
				var response = await _authService.RegisterAsync(request);
				return Ok(response);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while registering the user" });
			}
		}

		[HttpPost("login")]
		public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
		{
			try
			{
				var response = await _authService.LoginAsync(request);
				return Ok(response);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while logging in" });
			}
		}
//DANGDUC
        [HttpPost("check-email")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromBody] string email)
        {
            try
            {
                var exists = await _authService.IsEmailExistsAsync(email);
                return Ok(exists);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while checking email" });
            }
        }



    
    }
}
