using Lesson1.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1.DAL
{
    internal class DepartmentApi
    {
        private readonly SqlConnection connection;

        public DepartmentApi(SqlConnection connection)
        {
            this.connection = connection;
        }

        public List<Department> GetAll()
        {
            var list = new List<Department>();

            using SqlCommand cmd = new() { Connection = connection };
            cmd.CommandText = "select D.* from Departments D";

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new()
                {
                    Id = reader.GetGuid("Id"),
                    Name = reader.GetString("Name")
                });
            }
            reader.Close();

            return list;
        }

        public bool Create(Department department)
        {
            using SqlCommand cmd = new() { Connection = connection };

            cmd.CommandText = $@"insert into Departments ( Id, Name ) values (@id, @name)";
            cmd.Parameters.AddWithValue("@id", department.Id);
            cmd.Parameters.AddWithValue("@name", department.Name);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Department department)
        {
            using SqlCommand cmd = new() { Connection = connection };

            cmd.CommandText = $@"update Departments set Name=@name where Id=@id";
            cmd.Parameters.AddWithValue("@id", department.Id);
            cmd.Parameters.AddWithValue("@name", department.Name);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Guid id)
        {
            using SqlCommand cmd = new() { Connection = connection };

            cmd.CommandText = $@"delete from Departments where Id=@id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
