using System;
using System.Collections.Generic;

namespace DataAccessLayer;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
