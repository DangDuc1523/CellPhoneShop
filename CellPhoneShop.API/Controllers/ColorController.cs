using CellPhoneShop.Business.DTOs;
using CellPhoneShop.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CellPhoneShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet("phone/{phoneId}")]
        public async Task<ActionResult<IEnumerable<ColorDto>>> GetByPhoneId(int phoneId)
        {
            var colors = await _colorService.GetByPhoneIdAsync(phoneId);
            return Ok(colors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColorDto>> GetById(int id)
        {
            try
            {
                var color = await _colorService.GetByIdAsync(id);
                return Ok(color);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<ColorDto>> Create([FromBody] CreateColorDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId <= 0)
                    return BadRequest("Invalid user authentication");

                var createdColor = await _colorService.CreateAsync(createDto, userId);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdColor.ColorId },
                    createdColor);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while creating the color");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateColorDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId <= 0)
                    return BadRequest("Invalid user authentication");

                await _colorService.UpdateAsync(id, updateDto, userId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while updating the color");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId <= 0)
                    return BadRequest("Invalid user authentication");

                await _colorService.DeleteAsync(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while deleting the color");
            }
        }
    }
} 