using System;
using System.Collections.Generic;

namespace CellPhoneShop.Domain.Models;

public partial class Master
{
    public int Id { get; set; }

    public int MasterDataId { get; set; }

    public string Category { get; set; } = null!;

    public string Name { get; set; } = null!;
}
