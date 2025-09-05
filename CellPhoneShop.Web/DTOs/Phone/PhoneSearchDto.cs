namespace CellPhoneShop.Web.DTOs.Phone
{
    public class PhoneSearchDto
    {
        public string? SearchTerm { get; set; }
        public int? BrandId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Ram { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
} 