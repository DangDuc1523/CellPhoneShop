using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CellPhoneShop.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        // Gửi email cảm ơn
        public async Task SendThankYouEmailAsync(string toEmail)
        {
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]);
            var smtpUser = _config["EmailSettings:SmtpUser"];
            var smtpPass = _config["EmailSettings:SmtpPass"];

            var message = new MailMessage
            {
                From = new MailAddress(smtpUser),
                Subject = "Thank you for subscribing!",
                Body = "Cảm ơn bạn đã đăng ký nhận tin từ CellPhoneShop!",
                IsBodyHtml = false
            };
            message.To.Add(toEmail);

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }

        // Gửi email OTP lấy lại mật khẩu
        //DANGDUC
        public async Task SendOtpEmailAsync(string toEmail, string otp)
        {
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]);
            var smtpUser = _config["EmailSettings:SmtpUser"];
            var smtpPass = _config["EmailSettings:SmtpPass"];

            var message = new MailMessage
            {
                From = new MailAddress(smtpUser),
                Subject = "Mã OTP xác thực",
                Body = $"Mã OTP của bạn là: {otp}",
                IsBodyHtml = false
            };
            message.To.Add(toEmail);

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                UseDefaultCredentials = false, 
                Credentials = new NetworkCredential(smtpUser, smtpPass)
            };

            await client.SendMailAsync(message);
        }
        //DANGDUC
    }
}
