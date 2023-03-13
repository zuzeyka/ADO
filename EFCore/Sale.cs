using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1.EFCore
{
    internal class Sale
    {
        public Guid Id { get; set; }
        public DateTime SellTime { get; set; }
        public int Quantity { get; set; }
        public Guid IdProduct { get; set; }
        public Guid IdManager { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
