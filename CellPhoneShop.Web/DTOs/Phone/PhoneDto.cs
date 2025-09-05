namespace CellPhoneShop.Web.DTOs.Phone;

public class PhoneDto
{
    public int PhoneId { get; set; }
    public int? BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string PhoneName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public string? Screen { get; set; }
    public string? Os { get; set; }
    public string? FrontCamera { get; set; }
    public string? RearCamera { get; set; }
    public string? Cpu { get; set; }
    public string? Ram { get; set; }
    public string? Battery { get; set; }
    public string? Sim { get; set; }
    public string? Other { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<PhoneAttributeMappingDto> AttributeMappings { get; set; } = new();
} 