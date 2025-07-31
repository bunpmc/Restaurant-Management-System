using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Table
{
    public int Id { get; set; }

    public int TableNumber { get; set; }

    public int? Capacity { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
