using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lesson1.DAL
{
    public class DataContext
    {
        SqlConnection connection;
        public DepartmentApi DepartmentApi { get; set; }
        public ManagerApi ManagerApi { get; set; }

        public DataContext()
        {
            SqlConnection connection = new(App.ConnectionString);
            this.connection = connection;

            try
            {
                this.connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            DepartmentApi = new(this.connection, this);
            ManagerApi = new(this.connection, this);
        }
    }
}
