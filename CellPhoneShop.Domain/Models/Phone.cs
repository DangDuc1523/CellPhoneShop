using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class Phone
{
    public int PhoneId { get; set; }

    public int? BrandId { get; set; }

    public string PhoneName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal BasePrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<Color> Colors { get; set; } = new List<Color>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual UserAccount? CreatedByNavigation { get; set; }

    public virtual UserAccount? DeletedByNavigation { get; set; }

    public virtual UserAccount? ModifiedByNavigation { get; set; }

    public virtual ICollection<PhoneVariant> PhoneVariants { get; set; } = new List<PhoneVariant>();

    public virtual ICollection<PhoneAttributeMapping> PhoneAttributeMappings { get; set; } = new List<PhoneAttributeMapping>();
}
