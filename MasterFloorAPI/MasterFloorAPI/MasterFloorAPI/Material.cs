using System;
using System.Collections.Generic;

namespace MasterFloorAPI;

public partial class Material
{
    public int Id { get; set; }

    public decimal DefectRate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
