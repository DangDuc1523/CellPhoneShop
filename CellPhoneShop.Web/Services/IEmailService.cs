namespace CellPhoneShop.Web.Services
{
        public interface IEmailService
        {
            Task SendThankYouEmailAsync(string toEmail);
        //DANGDUC
        Task SendOtpEmailAsync(string toEmail, string otp);
        //DANGDUC
    }

}
