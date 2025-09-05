using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class PhoneVariant
{
    public int VariantId { get; set; }

    public int? PhoneId { get; set; }

    public int? ColorId { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? Sku { get; set; }

    public int Status { get; set; }

    public bool? IsDefault { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Color? Color { get; set; }

    public virtual UserAccount? CreatedByNavigation { get; set; }

    public virtual UserAccount? DeletedByNavigation { get; set; }

    public virtual UserAccount? ModifiedByNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Phone? Phone { get; set; }

    public virtual ICollection<PhonePromotion> PhonePromotions { get; set; } = new List<PhonePromotion>();

    public virtual ICollection<Warranty> Warranties { get; set; } = new List<Warranty>();

    public virtual ICollection<VariantAttributeMapping> VariantAttributeMappings { get; set; } = new List<VariantAttributeMapping>();
}
