using System;
using System.Collections.Generic;

namespace DataAccessLayer;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int MenuItemId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual MenuItem MenuItem { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
