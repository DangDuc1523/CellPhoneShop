using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class VariantAttributeValue
{
    public int ValueId { get; set; }
    public int VariantAttributeId { get; set; }
    public string Value { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeletedBy { get; set; }
    public bool? IsDeleted { get; set; }

    public virtual VariantAttribute VariantAttribute { get; set; } = null!;
    public virtual UserAccount? CreatedByNavigation { get; set; }
    public virtual UserAccount? ModifiedByNavigation { get; set; }
    public virtual UserAccount? DeletedByNavigation { get; set; }
    public virtual ICollection<VariantAttributeMapping> VariantAttributeMappings { get; set; } = new List<VariantAttributeMapping>();
} 