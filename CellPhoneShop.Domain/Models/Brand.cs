using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandName { get; set; } = null!;

    public string? LogoUrl { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual UserAccount? CreatedByNavigation { get; set; }

    public virtual UserAccount? DeletedByNavigation { get; set; }

    public virtual UserAccount? ModifiedByNavigation { get; set; }

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}
