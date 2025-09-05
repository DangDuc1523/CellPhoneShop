namespace CellPhoneShop.Web.DTOs.Attribute
{
    public class AttributeDto
    {
        public int AttributeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
} 