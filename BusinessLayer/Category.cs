using System;
using System.Collections.Generic;

namespace DataAccessLayer;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
