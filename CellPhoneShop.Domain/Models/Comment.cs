using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? PhoneId { get; set; }

    public int? UserId { get; set; }

    public int? Rating { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Phone? Phone { get; set; }

    public virtual UserAccount? User { get; set; }
}
