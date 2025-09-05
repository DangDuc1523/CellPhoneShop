using CellPhoneShop.Business.DTOs;
using CellPhoneShop.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CellPhoneShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorImageController : ControllerBase
    {
        private readonly IColorImageService _colorImageService;

        public ColorImageController(IColorImageService colorImageService)
        {
            _colorImageService = colorImageService;
        }

        [HttpGet("color/{colorId}")]
        public async Task<ActionResult<IEnumerable<ColorImageDto>>> GetByColorId(int colorId)
        {
            var colorImages = await _colorImageService.GetByColorIdAsync(colorId);
            return Ok(colorImages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColorImageDto>> GetById(int id)
        {
            try
            {
                var colorImage = await _colorImageService.GetByIdAsync(id);
                return Ok(colorImage);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<ColorImageDto>> Create([FromBody] CreateColorImageDto dto)
        {
            var createdColorImage = await _colorImageService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdColorImage.ImageId }, createdColorImage);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateColorImageDto dto)
        {
            try
            {
                await _colorImageService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _colorImageService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
} 