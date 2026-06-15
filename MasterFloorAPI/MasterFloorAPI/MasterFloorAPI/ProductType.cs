using System;
using System.Collections.Generic;

namespace MasterFloorAPI;

public partial class ProductType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Coefficient { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
