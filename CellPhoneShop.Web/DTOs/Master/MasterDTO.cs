using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Web.DTOs.Master
{
    public class MasterDTO
    {
        public int Id { get; set; }
        public int MasterDataId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
