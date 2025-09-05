using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Web.DTOs.Account
{
    public class AccountDto
    {

        [Key]
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string? PasswordH { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string AddressDetail { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
    }
}
