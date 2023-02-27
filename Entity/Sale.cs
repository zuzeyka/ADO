using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1.Entity
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime SellTime { get; set; }
        public int Quantity { get; set; }
        public Guid? IdProduct { get; set; }
        public Guid? IdManager { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Sale()
        {
            Id = Guid.NewGuid();
            SellTime = DateTime.Now;
            Quantity = 1;
        }

        public Sale(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            SellTime = reader.GetDateTime("SellTime");
            Quantity = reader.GetInt32("Quantity");
            IdProduct = reader.GetGuid("IdProduct");
            IdManager = reader.GetGuid("IdManager");
            DeletedAt = reader.IsDBNull("DeletedAt") ? null : reader.GetDateTime("DeletedAt");
        }

        public Sale Clone()
        {
            return new Sale()
            {
                Id = Id,
                SellTime = SellTime,
                Quantity = Quantity,
                IdProduct = IdProduct,
                IdManager = IdManager,
                DeletedAt = DeletedAt
            };
        }
    }
}
