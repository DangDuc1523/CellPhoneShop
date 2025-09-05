namespace CellPhoneShop.Web.DTOs.Color;

public class ColorDto
{
    public int ColorId { get; set; }
    public int PhoneId { get; set; }
    public string ColorName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

public class CreateColorDto
{
    public int PhoneId { get; set; }
    public string ColorName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
} 