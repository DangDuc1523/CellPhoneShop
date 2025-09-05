using System;

namespace CellPhoneShop.Domain.Models;

public partial class PhoneAttributeMapping
{
    public int PhoneId { get; set; }
    public int AttributeId { get; set; }
    public string Value { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeletedBy { get; set; }
    public bool? IsDeleted { get; set; }

    public virtual Phone Phone { get; set; } = null!;
    public virtual PhoneAttribute Attribute { get; set; } = null!;
    public virtual UserAccount? CreatedByNavigation { get; set; }
    public virtual UserAccount? ModifiedByNavigation { get; set; }
    public virtual UserAccount? DeletedByNavigation { get; set; }
} 