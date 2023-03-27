using System;
using System.Collections.Generic;

namespace Lesson1.Scaffolder;

public partial class Department
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? DeletedAt { get; set; }
}
