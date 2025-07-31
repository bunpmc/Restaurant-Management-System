using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class ExportLog
{
    public int Id { get; set; }

    public int? ExportedByEmployeeId { get; set; }

    public string? ExportType { get; set; }

    public DateTime? ExportedAt { get; set; }

    public virtual Employee? ExportedByEmployee { get; set; }
}
