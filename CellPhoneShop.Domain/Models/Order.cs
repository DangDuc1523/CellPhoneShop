using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public int Status { get; set; }

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    public int? WardId { get; set; }

    public string? AddressDetail { get; set; }

    public int PaymentMethod { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual UserAccount? User { get; set; }
}
