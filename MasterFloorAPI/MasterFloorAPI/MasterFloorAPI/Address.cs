using System;
using System.Collections.Generic;

namespace MasterFloorAPI;

public partial class Address
{
    public int Id { get; set; }

    public string? PostalCode { get; set; }

    public string? Area { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public string? House { get; set; }

    public virtual ICollection<Partner> Partners { get; set; } = new List<Partner>();
}
