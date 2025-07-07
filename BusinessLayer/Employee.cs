using System;
using System.Collections.Generic;

namespace DataAccessLayer;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? JobTitle { get; set; }

    public DateOnly BirthDate { get; set; }

    public DateOnly HireDate { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
