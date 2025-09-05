using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int? CartId { get; set; }

    public int? VariantId { get; set; }

    public int? Quantity { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual PhoneVariant? Variant { get; set; }
}
