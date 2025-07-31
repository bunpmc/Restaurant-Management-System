using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Order
{
    public int Id { get; set; }

    public int? TableId { get; set; }

    public int? EmployeeId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? OrderTime { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public virtual Bill? Bill { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Table? Table { get; set; }
}
