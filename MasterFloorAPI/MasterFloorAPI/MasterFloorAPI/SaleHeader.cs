using System;
using System.Collections.Generic;

namespace MasterFloorAPI;

public partial class SaleHeader
{
    public int Id { get; set; }

    public int PartnerId { get; set; }

    public DateOnly SaleDate { get; set; }

    public virtual Partner Partner { get; set; } = null!;

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
