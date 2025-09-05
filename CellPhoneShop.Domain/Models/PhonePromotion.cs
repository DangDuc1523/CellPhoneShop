using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class PhonePromotion
{
    public int Id { get; set; }

    public int? VariantId { get; set; }

    public int? PromotionId { get; set; }

    public virtual Promotion? Promotion { get; set; }

    public virtual PhoneVariant? Variant { get; set; }
}
