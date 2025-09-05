using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.DTOs
{
	public class UpdateUserProfileDto
{
    public string FullName { get; set; }
    public string Phone { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }
    public string AddressDetail { get; set; }
}
}
