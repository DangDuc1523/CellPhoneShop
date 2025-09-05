using CellPhoneShop.Business.DTOs.Attribute;
using CellPhoneShop.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CellPhoneShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VariantAttributeController : ControllerBase
{
    private readonly IVariantAttributeService _variantAttributeService;
    private readonly IVariantAttributeValueService _valueService;

    public VariantAttributeController(
        IVariantAttributeService variantAttributeService,
        IVariantAttributeValueService valueService)
    {
        _variantAttributeService = variantAttributeService;
        _valueService = valueService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VariantAttributeDto>>> GetAll()
    {
        var attributes = await _variantAttributeService.GetAllAsync();
        return Ok(attributes);
    }

    [HttpGet("with-values")]
    public async Task<ActionResult<IEnumerable<VariantAttributeWithValuesDto>>> GetAllWithValues()
    {
        try
        {
            var attributes = await _variantAttributeService.GetAllAsync();
            var result = new List<VariantAttributeWithValuesDto>();

            foreach (var attribute in attributes)
            {
                var values = await _valueService.GetByAttributeIdAsync(attribute.VariantAttributeId);
                result.Add(new VariantAttributeWithValuesDto
                {
                    VariantAttributeId = attribute.VariantAttributeId,
                    Name = attribute.Name,
                    Description = attribute.Description,
                    Values = values.ToList()
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Failed to get attributes with values", Error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VariantAttributeDto>> GetById(int id)
    {
        var attribute = await _variantAttributeService.GetByIdAsync(id);
        if (attribute == null)
            return NotFound();

        return Ok(attribute);
    }

    [HttpPost]
    public async Task<ActionResult<VariantAttributeDto>> Create(CreateVariantAttributeDto createDto)
    {
        try
        {
            var created = await _variantAttributeService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.VariantAttributeId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VariantAttributeDto>> Update(int id, UpdateVariantAttributeDto updateDto)
    {
        try
        {
            var updated = await _variantAttributeService.UpdateAsync(id, updateDto);
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
        var deleted = await _variantAttributeService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    // Value endpoints
    [HttpGet("{attributeId}/values")]
    public async Task<ActionResult<IEnumerable<VariantAttributeValueDto>>> GetValues(int attributeId)
    {
        var values = await _valueService.GetByAttributeIdAsync(attributeId);
        return Ok(values);
    }

    [HttpPost("values")]
    public async Task<ActionResult<VariantAttributeValueDto>> CreateValue(CreateVariantAttributeValueDto createDto)
    {
        try
        {
            var created = await _valueService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetValueById), new { id = created.ValueId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("values/{id}")]
    public async Task<ActionResult<VariantAttributeValueDto>> GetValueById(int id)
    {
        var value = await _valueService.GetByIdAsync(id);
        if (value == null)
            return NotFound();

        return Ok(value);
    }

    [HttpPut("values/{id}")]
    public async Task<ActionResult<VariantAttributeValueDto>> UpdateValue(int id, UpdateVariantAttributeValueDto updateDto)
    {
        try
        {
            var updated = await _valueService.UpdateAsync(id, updateDto);
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("values/{id}")]
    public async Task<ActionResult> DeleteValue(int id)
    {
        var deleted = await _valueService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

// DTO for VariantAttribute with Values included
public class VariantAttributeWithValuesDto
{
    public int VariantAttributeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<VariantAttributeValueDto> Values { get; set; } = new();
} 