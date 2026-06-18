using System;
using System.Collections.Generic;

namespace MasterFloorAPI.Models;

public partial class PartnerRatingHistory
{
    public int Id { get; set; }

    public int PartnerId { get; set; }

    public int? OldRating { get; set; }

    public int NewRating { get; set; }

    public DateTime? ChangedAt { get; set; }

    public string? ChangedBy { get; set; }

    public virtual Partner Partner { get; set; } = null!;
}
