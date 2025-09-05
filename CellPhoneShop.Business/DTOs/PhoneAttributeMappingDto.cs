using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Business.DTOs
{
    public class PhoneAttributeMappingDto
    {
        public int AttributeId { get; set; }
        public string? Value { get; set; }
        public string? AttributeName { get; set; }
    }
} 