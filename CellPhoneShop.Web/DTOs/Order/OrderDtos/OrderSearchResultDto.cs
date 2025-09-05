namespace CellPhoneShop.Web.DTOs.Order.OrderDtos
{
    public class OrderSearchResultDto
    {
        public IEnumerable<OrderDto> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
} 