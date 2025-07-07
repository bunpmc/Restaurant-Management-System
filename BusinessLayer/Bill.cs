using System;
using System.Collections.Generic;

namespace DataAccessLayer;

public partial class Bill
{
    public int BillId { get; set; }

    public int TableId { get; set; }

    public int CustomerId { get; set; }

    public DateTime BillDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
