namespace CellPhoneShop.Business.DTOs.Attribute;

public class PhoneAttributeDto
{
    public int AttributeId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public bool? IsDeleted { get; set; }
}

public class CreatePhoneAttributeDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdatePhoneAttributeDto
{
    public int AttributeId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
} 