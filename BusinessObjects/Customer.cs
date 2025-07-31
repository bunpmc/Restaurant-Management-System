using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Customer
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
