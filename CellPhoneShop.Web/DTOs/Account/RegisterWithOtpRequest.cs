namespace CellPhoneShop.Web.DTOs.Account
{
    public class RegisterWithOtpRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
