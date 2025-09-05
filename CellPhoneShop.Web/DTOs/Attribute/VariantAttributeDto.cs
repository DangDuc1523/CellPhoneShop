using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Web.DTOs.Attribute
{
    public class VariantAttributeDto
    {
        public int VariantAttributeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<VariantAttributeValueDto> Values { get; set; } = new();
    }

    public class VariantAttributeValueDto
    {
        public int ValueId { get; set; }
        public int VariantAttributeId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
} 