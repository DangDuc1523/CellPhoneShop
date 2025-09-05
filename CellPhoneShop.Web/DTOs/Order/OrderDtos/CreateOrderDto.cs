using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Web.DTOs.Order.OrderDetailDtos;

namespace CellPhoneShop.Web.DTOs.Order.OrderDtos
{
    public class CreateOrderDto
    {



        [Required]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date time.")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(1000, double.MaxValue, ErrorMessage = "Total amount must be at least 1000.")]
        [DefaultValue(1000)]
        public decimal TotalAmount { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1,4, ErrorMessage = "Status must be a valid value.")]
        public int Status { get; set; }

        public int? ProvinceId { get; set; }

        public int? DistrictId { get; set; }

        public int? WardId { get; set; }

        [StringLength(500, ErrorMessage = "Address detail cannot exceed 500 characters.")]
        public string? AddressDetail { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "Payment method must be a valid value.")]
        public int PaymentMethod { get; set; }

        [StringLength(1000, ErrorMessage = "Note cannot exceed 1000 characters.")]
        public string? Note { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one order detail is required.")]
        public ICollection<CreateOrderDetailDto> OrderDetails { get; set; } = new List<CreateOrderDetailDto>();

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

    }
}
