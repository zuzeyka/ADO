using System;
using System.Collections.Generic;

namespace Lesson1.Scaffolded;

public partial class Sale
{
    public Guid Id { get; set; }

    public DateTime SellTime { get; set; }

    public int Quantity { get; set; }

    public Guid IdProduct { get; set; }

    public Guid IdManager { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Manager IdManagerNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
