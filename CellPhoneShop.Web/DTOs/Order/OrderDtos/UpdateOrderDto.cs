using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Web.DTOs.Order.OrderDtos
{
    public class UpdateOrderDto
    {
        [Required]
        [Range(1,4, ErrorMessage = "Status must be a valid value.")]
        public int Status { get; set; }

        public int? ProvinceId { get; set; }

        public int? DistrictId { get; set; }

        public int? WardId { get; set; }

        [StringLength(500, ErrorMessage = "Address detail cannot exceed 500 characters.")]
        public string? AddressDetail { get; set; }

        [Range(1, 2, ErrorMessage = "Payment method must be a valid value.")]
        public int PaymentMethod { get; set; }

        [StringLength(1000, ErrorMessage = "Note cannot exceed 1000 characters.")]
        public string? Note { get; set; }
    }
}
