using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Web.DTOs.Phone;

public class PhoneAttributeMappingDto
{
    public int AttributeId { get; set; }
    public string AttributeName { get; set; } = string.Empty;
    public string? Value { get; set; }
} 