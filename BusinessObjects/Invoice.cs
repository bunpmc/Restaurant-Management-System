using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Invoice
{
    public int Id { get; set; }

    public int? BillId { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public DateTime? IssuedAt { get; set; }

    public bool? Printed { get; set; }

    public bool? Exported { get; set; }

    public virtual Bill? Bill { get; set; }
}
