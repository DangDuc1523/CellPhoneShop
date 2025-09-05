using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Web.DTOs.Cart.CartDtos
{
    public class CreateCartDto
    {
        [Required]
        public int UserId { get; set; }

        public bool? IsActive { get; set; } = true;
    }
}
