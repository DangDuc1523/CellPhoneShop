namespace CellPhoneShop.Web.DTOs.Phone
{
    public class PhoneSearchResultDto
    {
        public IEnumerable<PhoneDto> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
} 