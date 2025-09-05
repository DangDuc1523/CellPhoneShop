using CellPhoneShop.Business.DTOs;
using CellPhoneShop.Business.Services;
using CellPhoneShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneVariantController : ControllerBase
    {
        private readonly IPhoneVariantService _phoneVariantService;
        private readonly CellPhoneShopContext _context;

        public PhoneVariantController(IPhoneVariantService phoneVariantService, CellPhoneShopContext context)
        {
            _phoneVariantService = phoneVariantService;
            _context = context;
        }

        [HttpGet("phone/{phoneId}")]
        public async Task<ActionResult<IEnumerable<PhoneVariantDto>>> GetByPhoneId(int phoneId)
        {
            var variants = await _phoneVariantService.GetByPhoneIdAsync(phoneId);
            return Ok(variants);
        }

        [HttpGet("color/{colorId}")]
        public async Task<ActionResult<IEnumerable<PhoneVariantDto>>> GetByColorId(int colorId)
        {
            var variants = await _phoneVariantService.GetByColorIdAsync(colorId);
            return Ok(variants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PhoneVariantDto>> GetById(int id)
        {
            try
            {
                var variant = await _phoneVariantService.GetByIdAsync(id);
                return Ok(variant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PhoneVariantDto>> Create([FromBody] CreatePhoneVariantDto dto)
        {
            try
            {
                var createdVariant = await _phoneVariantService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdVariant.VariantId }, createdVariant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePhoneVariantDto dto)
        {
            try
            {
                await _phoneVariantService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _phoneVariantService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // VariantAttributeMapping endpoints
        [HttpPost("mapping")]
        public async Task<IActionResult> CreateVariantAttributeMapping([FromBody] CreateVariantAttributeMappingDto dto)
        {
            try
            {
                Console.WriteLine($"=== Creating VariantAttributeMapping - VariantId: {dto.VariantId}, ValueId: {dto.ValueId} ===");
                
                // Check if variant exists
                var variantExists = await _context.PhoneVariants
                    .AnyAsync(v => v.VariantId == dto.VariantId && v.IsDeleted != true);
                
                if (!variantExists)
                {
                    Console.WriteLine($"Variant {dto.VariantId} not found");
                    return BadRequest(new { Message = $"Variant with ID {dto.VariantId} not found" });
                }
                
                // Check if value exists
                var valueExists = await _context.VariantAttributeValues
                    .AnyAsync(v => v.ValueId == dto.ValueId && v.IsDeleted != true);
                
                if (!valueExists)
                {
                    Console.WriteLine($"VariantAttributeValue {dto.ValueId} not found");
                    return BadRequest(new { Message = $"VariantAttributeValue with ID {dto.ValueId} not found" });
                }
                
                // Check if mapping already exists
                var existingMapping = await _context.VariantAttributeMappings
                    .FirstOrDefaultAsync(m => m.VariantId == dto.VariantId && m.ValueId == dto.ValueId && m.IsDeleted != true);
                
                if (existingMapping != null)
                {
                    Console.WriteLine($"Mapping already exists for VariantId {dto.VariantId} and ValueId {dto.ValueId}");
                    return Ok(new { Message = "Mapping already exists", VariantId = dto.VariantId, ValueId = dto.ValueId });
                }

                var mapping = new VariantAttributeMapping
                {
                    VariantId = dto.VariantId,
                    ValueId = dto.ValueId,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.VariantAttributeMappings.Add(mapping);
                await _context.SaveChangesAsync();
                
                Console.WriteLine($"Mapping created successfully - VariantId: {dto.VariantId}, ValueId: {dto.ValueId}");

                return Ok(new { Message = "Mapping created successfully", VariantId = dto.VariantId, ValueId = dto.ValueId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating mapping: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest(new { Message = "Failed to create mapping", Error = ex.Message });
            }
        }

        [HttpGet("{variantId}/attributes")]
        public async Task<ActionResult<IEnumerable<VariantAttributeMappingDto>>> GetVariantAttributes(int variantId)
        {
            try
            {
                Console.WriteLine($"=== Getting attributes for VariantId: {variantId} ===");
                
                var mappings = await _context.VariantAttributeMappings
                    .Where(m => m.VariantId == variantId && m.IsDeleted != true)
                    .Include(m => m.AttributeValue)
                        .ThenInclude(av => av.VariantAttribute)
                    .Select(m => new VariantAttributeMappingDto
                    {
                        VariantId = m.VariantId,
                        ValueId = m.ValueId,
                        AttributeName = m.AttributeValue.VariantAttribute.Name,
                        AttributeValue = m.AttributeValue.Value
                    })
                    .ToListAsync();

                Console.WriteLine($"Found {mappings.Count} mappings for VariantId {variantId}");
                
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting variant attributes: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest(new { Message = "Failed to get variant attributes", Error = ex.Message });
            }
        }

        [HttpGet("debug/variant-attribute-values")]
        public async Task<IActionResult> GetVariantAttributeValues()
        {
            try
            {
                var variantAttributes = await _context.VariantAttributes
                    .Where(va => va.IsDeleted != true)
                    .Include(va => va.VariantAttributeValues.Where(vav => vav.IsDeleted != true))
                    .Select(va => new 
                    {
                        VariantAttributeId = va.VariantAttributeId,
                        Name = va.Name,
                        Values = va.VariantAttributeValues.Select(vav => new
                        {
                            ValueId = vav.ValueId,
                            Value = vav.Value
                        }).ToList()
                    })
                    .ToListAsync();

                Console.WriteLine($"=== DEBUG: Found {variantAttributes.Count} VariantAttributes ===");
                foreach (var attr in variantAttributes)
                {
                    Console.WriteLine($"Attribute {attr.VariantAttributeId} ({attr.Name}): {attr.Values.Count} values");
                    foreach (var value in attr.Values)
                    {
                        Console.WriteLine($"  - ValueId {value.ValueId}: {value.Value}");
                    }
                }

                return Ok(variantAttributes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting variant attribute values: {ex.Message}");
                return BadRequest(new { Message = "Failed to get variant attribute values", Error = ex.Message });
            }
        }

        [HttpGet("debug/variants")]
        public async Task<IActionResult> GetVariantsDebug()
        {
            try
            {
                var variants = await _context.PhoneVariants
                    .Where(pv => pv.IsDeleted != true)
                    .Take(10)
                    .Select(pv => new
                    {
                        VariantId = pv.VariantId,
                        PhoneId = pv.PhoneId,
                        ColorId = pv.ColorId,
                        Sku = pv.Sku,
                        Status = pv.Status
                    })
                    .ToListAsync();

                Console.WriteLine($"=== DEBUG: Found {variants.Count} PhoneVariants ===");
                foreach (var variant in variants)
                {
                    Console.WriteLine($"VariantId {variant.VariantId}: PhoneId={variant.PhoneId}, ColorId={variant.ColorId}, SKU={variant.Sku}");
                }

                return Ok(variants);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting variants: {ex.Message}");
                return BadRequest(new { Message = "Failed to get variants", Error = ex.Message });
            }
        }
    }

    // DTOs for VariantAttributeMapping
    public class CreateVariantAttributeMappingDto
    {
        public int VariantId { get; set; }
        public int ValueId { get; set; }
    }

    public class VariantAttributeMappingDto
    {
        public int VariantId { get; set; }
        public int ValueId { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public string AttributeValue { get; set; } = string.Empty;
    }
} 