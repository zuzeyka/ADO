using System;
using System.Collections.Generic;

namespace Lesson1.Scaffolder;

public partial class Manager
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Secname { get; set; } = null!;

    public Guid IdMainDep { get; set; }

    public Guid? IdSecDep { get; set; }

    public Guid? IdChief { get; set; }

    public DateTime? DeletedAt { get; set; }
}
