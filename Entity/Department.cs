using Lesson1.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1.Entity
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        DataContext dataContext;

        public IEnumerable<Manager> DepManagers => dataContext.ManagerApi.GetAll(false, false).Where(m => m.IdMainDep == Id);

        public Department(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public override string ToString()
        {
            return Id.ToString()[..5] + "... " + Name;
        }
    }
}
