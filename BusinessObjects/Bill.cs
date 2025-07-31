using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Bill
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public DateTime? BillTime { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public int? PaidByEmployeeId { get; set; }

    public string? Note { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Employee? PaidByEmployee { get; set; }
}
