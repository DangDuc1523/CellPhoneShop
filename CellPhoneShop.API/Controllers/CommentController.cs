using CellPhoneShop.Business.DTOs.CommentDtos;
using CellPhoneShop.Business.Services;
using CellPhoneShop.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CellPhoneShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }
        [HttpGet("phone/{phoneId}")]
        public async Task<IActionResult> GetComments(int phoneId)
        {
            var result = await _service.GetByPhoneIdAsync(phoneId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentDto dto)
        {
            var comment = new Comment
            {
                PhoneId = dto.PhoneID,
                UserId = dto.UserID,
                Rating = dto.Rating,
                Content = dto.Content
            };

            await _service.AddAsync(comment);
            return Ok(comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto dto)
        {
            var comment = new Comment
            {
                Rating = dto.Rating,
                Content = dto.Content
            };

            await _service.UpdateAsync(id, comment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id, [FromQuery] int deletedBy)
        {
            await _service.DeleteAsync(id, deletedBy);
            return NoContent();
        }

    }
}
