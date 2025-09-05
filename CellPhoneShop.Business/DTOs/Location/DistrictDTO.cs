using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.DTOs.Location
{
    public class DistrictDTO
    {
        public int DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public string DistrictName { get; set; } = string.Empty;
    }
}
