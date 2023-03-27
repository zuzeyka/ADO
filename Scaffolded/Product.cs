using System;
using System.Collections.Generic;

namespace Lesson1.Scaffolded;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Sale> Sales { get; } = new List<Sale>();
}
