using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.DTOs.Location
{
    public class WardDTO
    {
        public int WardId { get; set; }
        public int? DistrictId { get; set; }
        public string WardName { get; set; } = string.Empty;
    }
}
