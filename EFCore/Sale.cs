using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1.EFCore
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime SellTime { get; set; }
        public int Quantity { get; set; }
        public Guid IdProduct { get; set; }
        public Guid IdManager { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Manager Manager { get; set; }
        //public List<Manager> Managers { get; set; }
        public Product Product { get; set; }
    }
}
