using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson1.DAL;

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

        DataContext? dataContext;

        public Department MainDep { get => dataContext.DepartmentApi.GetAll(false).First(d => d.Id == IdMainDep); }
        public Department? SecDep { get => dataContext.DepartmentApi.GetAll(false).FirstOrDefault(d => d.Id == IdSecDep); }
        public Manager? Chief { get => dataContext.ManagerApi.GetAll(false, false).FirstOrDefault(m => m.Id == IdChief); }
        public string ChiefName => $"{Chief?.Name} {Chief?.Surname}";
        public List<Manager> Subordinates { get => dataContext.ManagerApi.GetAll(false, false).Where(m => m.IdChief == Id).ToList(); }

        public Manager(DataContext dataContext)
        {
            this.dataContext = dataContext;
            Id = Guid.NewGuid();
        }

        public Manager(SqlDataReader reader, DataContext dataContext) : this(dataContext)
        {
            Id = reader.GetGuid("Id");
            Name = reader.GetString("Name");
            Surname = reader.GetString("Surname");
            Secname = reader.GetString("Secname");
            IdMainDep = reader.GetGuid("Id_main_dep"); 
            IdSecDep = reader.IsDBNull("Id_sec_dep") ? null : reader.GetGuid("Id_sec_dep"); 
            IdChief = reader.IsDBNull("Id_chief") ? null : reader.GetGuid("Id_chief");
            DeletedAt = reader.IsDBNull("DeletedAt") ? null : reader.GetDateTime("DeletedAt");
        }

        public Manager Clone()
        {
            return new Manager(dataContext)
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
