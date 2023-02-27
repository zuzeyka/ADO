using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1.Entity
{
    public class Manager
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Secname { get; set; }
        public Guid? IdMainDep { get; set; }
        public Guid? IdSecDep { get; set; }
        public Guid? IdChief { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Manager Clone()
        {
            return new Manager()
            {
                Id = Id,
                Name = Name,
                Surname = Surname,
                Secname = Secname,
                IdMainDep = IdMainDep,
                IdSecDep = IdSecDep,
                IdChief = IdChief,
                DeletedAt = DeletedAt
            };
        }
    }
}
