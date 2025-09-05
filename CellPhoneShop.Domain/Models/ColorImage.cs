using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class ColorImage
{
    public int ImageId { get; set; }

    public int? ColorId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Color? Color { get; set; }

    public virtual UserAccount? CreatedByNavigation { get; set; }

    public virtual UserAccount? DeletedByNavigation { get; set; }

    public virtual UserAccount? ModifiedByNavigation { get; set; }
}
