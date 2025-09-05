using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Web.DTOs.Order.OrderDetailDtos
{
    public class CreateOrderDetailDto
    {
        [Required]
        public int VariantId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(1000, double.MaxValue, ErrorMessage = "Price must be at least 1000.")]
        public decimal Price { get; set; }
    }
}
