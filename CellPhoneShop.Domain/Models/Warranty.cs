using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class Warranty
{
    public int WarrantyId { get; set; }

    public int? VariantId { get; set; }

    public int? WarrantyPeriod { get; set; }

    public string? WarrantyCenter { get; set; }

    public string? Contact { get; set; }

    public virtual PhoneVariant? Variant { get; set; }
}
