using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class PhoneAttribute
{
    public int AttributeId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeletedBy { get; set; }
    public bool? IsDeleted { get; set; }

    public virtual UserAccount? CreatedByNavigation { get; set; }
    public virtual UserAccount? ModifiedByNavigation { get; set; }
    public virtual UserAccount? DeletedByNavigation { get; set; }
    public virtual ICollection<PhoneAttributeMapping> PhoneAttributeMappings { get; set; } = new List<PhoneAttributeMapping>();
} 