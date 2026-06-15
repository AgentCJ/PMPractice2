using System;
using System.Collections.Generic;

namespace MasterFloorAPI;

public partial class PartnerType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Partner> Partners { get; set; } = new List<Partner>();
}
