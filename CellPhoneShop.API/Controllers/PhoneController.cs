using CellPhoneShop.Business.Services;
using CellPhoneShop.Business.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CellPhoneShop.Domain.Enums;
using System.Security.Claims;
using System.Linq;

namespace CellPhoneShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneController : ControllerBase
    {
        private readonly IPhoneService _phoneService;
        private readonly ILogger<PhoneController> _logger;

        public PhoneController(IPhoneService phoneService, ILogger<PhoneController> logger)
        {
            _phoneService = phoneService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhoneDto>>> GetAll()
        {
            try 
            {
                // Log authentication information
                var authHeader = Request.Headers["Authorization"].ToString();
                _logger.LogInformation("Authorization Header: {AuthHeader}", authHeader);

                // Log user claims
                var claims = User.Claims.Select(c => new { c.Type, c.Value });
                _logger.LogInformation("User Claims: {@Claims}", claims);

                // Log user identity
                _logger.LogInformation("IsAuthenticated: {IsAuthenticated}", User.Identity?.IsAuthenticated);
                _logger.LogInformation("AuthenticationType: {AuthType}", User.Identity?.AuthenticationType);
                _logger.LogInformation("Name: {Name}", User.Identity?.Name);

                // Log roles
                var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                _logger.LogInformation("User Roles: {@Roles}", roles);

                var phones = await _phoneService.GetAllAsync();
                return Ok(phones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting phones");
                return StatusCode(500, "An error occurred while getting the phones");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PhoneDto>> GetById(int id)
        {
            try
            {
                var phone = await _phoneService.GetByIdAsync(id);
                return Ok(phone);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<PhoneSearchResultDto>> Search([FromQuery] PhoneSearchDto searchDto)
        {
            var result = await _phoneService.SearchAsync(searchDto);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<ActionResult<PhoneDto>> Create([FromBody] CreatePhoneDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var createdPhone = await _phoneService.CreateAsync(createDto, userId);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdPhone.PhoneId },
                    createdPhone);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the phone");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePhoneDto updateDto)
        {
            _logger.LogInformation("PUT Update called for Phone ID: {PhoneId}", id);
            _logger.LogInformation("UpdateDto received: {@UpdateDto}", updateDto);
            _logger.LogInformation("ModelState.IsValid: {IsValid}", ModelState.IsValid);
            
            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is invalid: {@Errors}", ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation("User ID claim: {UserIdClaim}", userIdClaim);
                
                var userId = int.Parse(userIdClaim ?? "0");
                _logger.LogInformation("Parsed User ID: {UserId}", userId);
                
                if (userId <= 0)
                {
                    _logger.LogError("Invalid user ID: {UserId}", userId);
                    return BadRequest("Invalid user authentication");
                }
                
                _logger.LogInformation("Calling PhoneService.UpdateAsync with ID: {PhoneId}, UserId: {UserId}", id, userId);
                await _phoneService.UpdateAsync(id, updateDto, userId);
                
                _logger.LogInformation("Phone updated successfully for ID: {PhoneId}", id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Phone not found with ID: {PhoneId}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating phone with ID: {PhoneId}. Full exception: {FullException}", id, ex.ToString());
                
                // Log inner exception details
                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    _logger.LogError("Inner Exception: {InnerException}", innerEx.ToString());
                    innerEx = innerEx.InnerException;
                }
                
                // Return detailed error for debugging
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"An error occurred while updating the phone: {errorMessage}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _phoneService.DeleteAsync(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the phone");
            }
        }

        [HttpGet("search-autocomplete")]
        public async Task<IActionResult> SearchPhones([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<object>());
            var matched = await _phoneService.SearchPhonesAsync(query);
            return Ok(matched.Select(p => new { p.PhoneId, p.PhoneName }));
        }
    }
} 