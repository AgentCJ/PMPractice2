using System;
using System.Collections.Generic;

namespace MasterFloorAPI;

public partial class SaleItem
{
    public int Id { get; set; }

    public int SaleId { get; set; }

    public string ProductArticle { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Product ProductArticleNavigation { get; set; } = null!;

    public virtual SaleHeader Sale { get; set; } = null!;
}
