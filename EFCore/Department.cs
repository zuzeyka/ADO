﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1.EFCore
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<Manager> Workers { get; set; }
        public List<Manager> SubWorkers { get; set; }
    }
}
