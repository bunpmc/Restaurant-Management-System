﻿using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class OrderItem
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? MenuItemId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual MenuItem? MenuItem { get; set; }

    public virtual Order? Order { get; set; }
}
