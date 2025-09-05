using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Web.DTOs.Cart.CartItemDtos
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int? CartId { get; set; }
        public int? VariantId { get; set; }
        public int? Quantity { get; set; }
        public string PhoneName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public List<string> ColorImageUrls { get; set; } = new();

        public decimal Price { get; set; }
    }
}
