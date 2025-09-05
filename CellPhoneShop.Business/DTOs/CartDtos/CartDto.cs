using CellPhoneShop.Business.DTOs.CartItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.DTOs.CartDtos
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsActive { get; set; }
        public ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public decimal TotalAmount { get; set; }
    }
}
