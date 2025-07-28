using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class PasswordResetToken
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public string? Token { get; set; }

    public DateTime? Expiration { get; set; }

    public bool? IsUsed { get; set; }

    public virtual Employee? Employee { get; set; }
}
