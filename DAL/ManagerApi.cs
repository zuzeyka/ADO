using Lesson1.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1.DAL
{
    public class ManagerApi
    {
        private readonly SqlConnection connection;
        private readonly DataContext dataContext;

        List<Manager> list = null!;

        public ManagerApi(SqlConnection connection, DataContext dataContext)
        {
            this.connection = connection;
            this.dataContext = dataContext;
        }

        public bool Create(Manager manager)
        {
            using SqlCommand cmd = new() { Connection = connection };
            List<string> properties = new()
            {
                "Id",
                "Name",
                "Surname",
                "Secname",
                "Id_main_dep"
            };
            List<string> values = new()
            {
                "@id",
                "@name",
                "@surname",
                "@secname",
                "@idMainDep"
            };

            if (manager.IdSecDep is not null)
            {
                properties.Add("Id_sec_dep");
                values.Add("@idSecDep");
                cmd.Parameters.AddWithValue("@idSecDep", manager.IdSecDep);
            }
            if (manager.IdChief is not null)
            {
                properties.Add("Id_chief");
                values.Add("@idChief");
                cmd.Parameters.AddWithValue("@idChief", manager.IdChief);
            }

            cmd.CommandText = $@"INSERT INTO Managers ( {string.Join(',', properties)} ) VALUES ({string.Join(',', values)})";
            cmd.Parameters.AddWithValue("@id", manager.Id);
            cmd.Parameters.AddWithValue("@name", manager.Name);
            cmd.Parameters.AddWithValue("@surname", manager.Surname);
            cmd.Parameters.AddWithValue("@secname", manager.Secname);
            cmd.Parameters.AddWithValue("@idMainDep", manager.IdMainDep);

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
            cmd.CommandText = @"update Managers set DeletedAt=@deletedAt where Id=@id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@deletedAt", DateTime.Now);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Manager> GetAll(bool includeDeleted = false, bool reload = false)
        {
            if (list != null && !reload) return list;

            using SqlCommand cmd = new() { Connection = connection };
            cmd.CommandText = "select M.* from Managers M";

            if(!includeDeleted)
            {
                cmd.CommandText += " where M.DeletedAt is null";
            }

            list ??= new();
            list.Clear();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Manager manager = new(reader, dataContext);
                list.Add(manager);
            }
            reader.Close();

            return list;
        }

        public bool Update(Manager manager)
        {
            using SqlCommand cmd = new() { Connection = connection };
            cmd.CommandText = @"update Managers set Name=@name, Surname=@surname, Secname=@secname, Id_main_dep=@idMainDep";

            if (manager.IdSecDep is not null)
            {
                cmd.CommandText += ", Id_sec_dep=@idSecDep";
                cmd.Parameters.AddWithValue("@idSecDep", manager.IdSecDep);
            }
            if (manager.IdChief is not null)
            {
                cmd.CommandText += ", Id_chief=@idChief";
                cmd.Parameters.AddWithValue("@idChief", manager.IdChief);
            }

            cmd.CommandText += " where Id=@id";

            cmd.Parameters.AddWithValue("@id", manager.Id);
            cmd.Parameters.AddWithValue("@name", manager.Name);
            cmd.Parameters.AddWithValue("@surname", manager.Surname);
            cmd.Parameters.AddWithValue("@secname", manager.Secname);
            cmd.Parameters.AddWithValue("@idMainDep", manager.IdMainDep);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
