using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class Color
{
    public int ColorId { get; set; }

    public int? PhoneId { get; set; }

    public string ColorName { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<ColorImage> ColorImages { get; set; } = new List<ColorImage>();

    public virtual UserAccount? CreatedByNavigation { get; set; }

    public virtual UserAccount? DeletedByNavigation { get; set; }

    public virtual UserAccount? ModifiedByNavigation { get; set; }

    public virtual Phone? Phone { get; set; }

    public virtual ICollection<PhoneVariant> PhoneVariants { get; set; } = new List<PhoneVariant>();
}
