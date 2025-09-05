using System;

namespace CellPhoneShop.Domain.Models;

public partial class VariantAttributeMapping
{
    public int VariantId { get; set; }
    public int ValueId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeletedBy { get; set; }
    public bool? IsDeleted { get; set; }

    public virtual PhoneVariant PhoneVariant { get; set; } = null!;
    public virtual VariantAttributeValue AttributeValue { get; set; } = null!;
    public virtual UserAccount? CreatedByNavigation { get; set; }
    public virtual UserAccount? ModifiedByNavigation { get; set; }
    public virtual UserAccount? DeletedByNavigation { get; set; }
} 