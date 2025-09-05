using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.DTOs.CommentDtos
{
    public class CommentDto
    {
        public int PhoneID { get; set; }
        public int UserID { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
    }
}
