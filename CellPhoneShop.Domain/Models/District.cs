using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class District
{
    public int DistrictId { get; set; }

    public int? ProvinceId { get; set; }

    public string DistrictName { get; set; } = null!;

    public virtual Province? Province { get; set; }

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}
