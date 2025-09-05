using CellPhoneShop.Business.DTOs.Attribute;
using CellPhoneShop.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CellPhoneShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhoneAttributeController : ControllerBase
{
    private readonly IPhoneAttributeService _phoneAttributeService;

    public PhoneAttributeController(IPhoneAttributeService phoneAttributeService)
    {
        _phoneAttributeService = phoneAttributeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PhoneAttributeDto>>> GetAll()
    {
        var attributes = await _phoneAttributeService.GetAllAsync();
        return Ok(attributes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PhoneAttributeDto>> GetById(int id)
    {
        var attribute = await _phoneAttributeService.GetByIdAsync(id);
        if (attribute == null)
            return NotFound();

        return Ok(attribute);
    }

    [HttpPost]
    public async Task<ActionResult<PhoneAttributeDto>> Create(CreatePhoneAttributeDto createDto)
    {
        try
        {
            var created = await _phoneAttributeService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.AttributeId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PhoneAttributeDto>> Update(int id, UpdatePhoneAttributeDto updateDto)
    {
        try
        {
            var updated = await _phoneAttributeService.UpdateAsync(id, updateDto);
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _phoneAttributeService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
} 