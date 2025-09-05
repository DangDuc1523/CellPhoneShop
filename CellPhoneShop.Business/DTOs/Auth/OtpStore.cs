using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.DTOs.Auth
{
    public class OtpStore
    {

            public string Email { get; set; }

            public string Otp { get; set; }

            public DateTime CreatedAt { get; set; }

            /// <summary>
            /// Thời gian hiệu lực của mã OTP (ví dụ: 5 phút).
            /// </summary>
            public TimeSpan ValidDuration { get; set; } = TimeSpan.FromMinutes(5);

            /// <summary>
            /// Kiểm tra OTP còn hiệu lực không.
            /// </summary>
            public bool IsExpired => DateTime.UtcNow > CreatedAt + ValidDuration;
        
    }
}
