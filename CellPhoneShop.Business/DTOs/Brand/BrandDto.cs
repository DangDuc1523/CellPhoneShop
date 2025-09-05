namespace CellPhoneShop.API.DTOs.Brand
{
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class CreateBrandDto
    {
        public string BrandName { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateBrandDto
    {
        public string BrandName { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
    }
} 