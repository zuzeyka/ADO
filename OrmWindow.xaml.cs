using Lesson1.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lesson1
{
    /// <summary>
    /// Interaction logic for OrmWindow.xaml
    /// </summary>
    public partial class OrmWindow : Window
    {
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Manager> Managers { get; set; }
        public ObservableCollection<Sale> Sales { get; set; }

        private SqlConnection connection;

        public OrmWindow()
        {
            InitializeComponent();

            Departments = new();
            Products = new();
            Managers = new();
            Sales = new();

            DataContext = this;
            connection = new(App.ConnectionString);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                using SqlCommand cmd = new() { Connection = connection };
                cmd.CommandText = "select D.Id, D.Name from Departments D";

                var reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Departments.Add(new(null)
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1)
                    });
                }
                reader.Close();

                cmd.CommandText = "select P.* from Products P";

                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Products.Add(new(reader));
                }
                reader.Close();
                
                cmd.CommandText = "select M.Id, M.Name, M.Surname, M.Secname, M.Id_main_dep, M.Id_sec_dep, M.Id_chief, M.DeletedAt from Managers M";

                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Managers.Add(new(null)
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Surname = reader.GetString(2),
                        Secname = reader.GetString(3),
                        IdMainDep = reader.GetGuid(4),
                        IdSecDep = reader.GetValue(5) == DBNull.Value ? null : reader.GetGuid(5),
                        IdChief = reader.GetValue(6) == DBNull.Value ? null : reader.GetGuid(6),
                        DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7)
                    });
                }
                reader.Close();

                cmd.CommandText = "select S.* from Sales S";

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Sales.Add(new(reader));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(sender is ListViewItem item)
            {
                if(item.Content is Department department)
                {
                    var departmentCrudWindow = new DepartmentCrudWindow();
                    departmentCrudWindow.Department = department;

                    using SqlCommand cmd = new() { Connection = connection };
                    
                    if(departmentCrudWindow.ShowDialog().Value)
                    {
                        var dep = departmentCrudWindow.Department;

                        if (departmentCrudWindow.IsDeleted)
                        {
                            if(MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                            {
                                cmd.CommandText = $"delete from Departments where Id=@id";
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            if(string.IsNullOrWhiteSpace(dep.Name) || dep.Name == department.Name)
                            {
                                return;
                            }

                            cmd.CommandText = @$"update Departments set Name=@name where Id=@id";
                        }

                        cmd.Parameters.AddWithValue("@id", dep.Id);
                        cmd.Parameters.AddWithValue("@name", dep.Name);

                        try
                        {
                            cmd.ExecuteNonQuery();

                            int index = Departments.IndexOf(department);
                            if(departmentCrudWindow.IsDeleted)
                            {
                                Departments.RemoveAt(index);
                            }
                            else
                            {
                                Departments[index] = dep;
                            }

                            MessageBox.Show("Success");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void ListViewItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Product product)
                {
                    var productCrudWindow = new ProductCrudWindow();
                    productCrudWindow.Product = product;

                    using SqlCommand cmd = new() { Connection = connection };

                    if (productCrudWindow.ShowDialog().Value)
                    {
                        var prod = productCrudWindow.Product;

                        if (productCrudWindow.IsDeleted)
                        {
                            if (MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                            {
                                cmd.CommandText = $"delete from Products where Id=@id";
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(prod.Name))
                            {
                                return;
                            }

                            cmd.CommandText = @$"update Products set Name=@name, Price=@price where Id=@id";
                        }

                        cmd.Parameters.AddWithValue("@id", prod.Id);
                        cmd.Parameters.AddWithValue("@name", prod.Name);
                        cmd.Parameters.AddWithValue("@price", prod.Price);

                        try
                        {
                            cmd.ExecuteNonQuery();

                            int index = Products.IndexOf(product);
                            if (productCrudWindow.IsDeleted)
                            {
                                Products.RemoveAt(index);
                            }
                            else
                            {
                                Products[index] = prod;
                            }

                            MessageBox.Show("Success");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void ListViewItem_MouseDoubleClick_2(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Manager manager)
                {
                    ManagerCrudWindow managerCrudWindow = new() { Manager = manager, Departments = Departments, Managers = Managers };

                    using SqlCommand cmd = new() { Connection = connection };

                    if (managerCrudWindow.ShowDialog().Value)
                    {
                        var managerNew = managerCrudWindow.Manager;

                        if (managerCrudWindow.IsDeleted)
                        {
                            if (MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                            {
                                cmd.CommandText = @"update Managers set DeletedAt=@deletedAt where Id=@id";
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            cmd.CommandText = @"update Managers set Name=@name, Surname=@surname, Secname=@secname, Id_main_dep=@idMainDep";

                            if(managerNew.IdSecDep is not null)
                            {
                                cmd.CommandText += ", Id_sec_dep=@idSecDep";
                                cmd.Parameters.AddWithValue("@idSecDep", managerNew.IdSecDep);
                            }
                            if (managerNew.IdChief is not null)
                            {
                                cmd.CommandText += ", Id_chief=@idChief";
                                cmd.Parameters.AddWithValue("@idChief", managerNew.IdChief);
                            }

                            cmd.CommandText += " where Id=@id";
                        }

                        cmd.Parameters.AddWithValue("@id", managerNew.Id);
                        cmd.Parameters.AddWithValue("@name", managerNew.Name);
                        cmd.Parameters.AddWithValue("@surname", managerNew.Surname);
                        cmd.Parameters.AddWithValue("@secname", managerNew.Secname);
                        cmd.Parameters.AddWithValue("@idMainDep", managerNew.IdMainDep);
                        cmd.Parameters.AddWithValue("@deletedAt", DateTime.Now);

                        try
                        {
                            cmd.ExecuteNonQuery();

                            int index = Managers.IndexOf(manager);
                            if (managerCrudWindow.IsDeleted)
                            {
                                Managers.RemoveAt(index);
                            }
                            else
                            {
                                Managers[index] = managerNew;
                            }

                            MessageBox.Show("Success");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void ListViewItem_MouseDoubleClick_3(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Sale sale)
                {
                    var saleCrudWindow = new SaleCrudWindow() { Owner = this };
                    saleCrudWindow.Sale = sale;

                    using SqlCommand cmd = new() { Connection = connection };

                    if (saleCrudWindow.ShowDialog().Value)
                    {
                        var saleNew = saleCrudWindow.Sale;

                        if (saleCrudWindow.IsDeleted)
                        {
                            if (MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                            {
                                cmd.CommandText = $"update Sales set DeletedAt=@deletedAt where Id=@id";
                                cmd.Parameters.AddWithValue("@deletedAt", DateTime.Now);
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            cmd.CommandText = @$"update Sales set Quantity=@quantity, IdProduct=@idProduct, IdManager=@idManager where Id=@id";
                        }

                        cmd.Parameters.AddWithValue("@id", saleNew.Id);
                        cmd.Parameters.AddWithValue("@quantity", saleNew.Quantity);
                        cmd.Parameters.AddWithValue("@idProduct", saleNew.IdProduct);
                        cmd.Parameters.AddWithValue("@idManager", saleNew.IdManager);

                        try
                        {
                            cmd.ExecuteNonQuery();

                            int index = Sales.IndexOf(sale);
                            if (saleCrudWindow.IsDeleted)
                            {
                                Sales.RemoveAt(index);
                            }
                            else
                            {
                                Sales[index] = saleNew;
                            }

                            MessageBox.Show("Success");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void createDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            var departmentCrudWindow = new DepartmentCrudWindow();

            using SqlCommand cmd = new() { Connection = connection };

            if (departmentCrudWindow.ShowDialog().Value)
            {
                var department = departmentCrudWindow.Department;

                if(string.IsNullOrWhiteSpace(department.Name))
                {
                    MessageBox.Show("Name is empty");
                    return;
                }
                
                if(Departments.Any(d => d.Name == department.Name))
                {
                    MessageBox.Show($"{department.Name} already exist");
                    return;
                }

                cmd.CommandText = $@"INSERT INTO Departments ( Id, Name ) VALUES (@id, @name)";
                cmd.Parameters.AddWithValue("@id", department.Id);
                cmd.Parameters.AddWithValue("@name", department.Name);

                try
                {
                    cmd.ExecuteNonQuery();
                    Departments.Add(department);
                    MessageBox.Show("Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void createProductBtn_Click(object sender, RoutedEventArgs e)
        {
            var productCrudWindow = new ProductCrudWindow();

            using SqlCommand cmd = new() { Connection = connection };

            if (productCrudWindow.ShowDialog().Value)
            {
                var product = productCrudWindow.Product;

                if(string.IsNullOrWhiteSpace(product.Name))
                {
                    MessageBox.Show("Name is empty");
                    return;
                }
                
                if(Departments.Any(d => d.Name == product.Name))
                {
                    MessageBox.Show($"{product.Name} already exist");
                    return;
                }

                cmd.CommandText = $@"INSERT INTO Products ( Id, Name, Price ) VALUES (@id, @name, @price)";
                cmd.Parameters.AddWithValue("@id", product.Id);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);

                try
                {
                    cmd.ExecuteNonQuery();
                    Products.Add(product);
                    MessageBox.Show("Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void createManagerBtn_Click(object sender, RoutedEventArgs e)
        {
            var managerCrudWindow = new ManagerCrudWindow() { Manager = null, Departments = Departments, Managers = Managers };

            using SqlCommand cmd = new() { Connection = connection };

            if (managerCrudWindow.ShowDialog().Value)
            {
                var manager = managerCrudWindow.Manager;

                if (Managers.Any(d => d.Name == manager.Name))
                {
                    MessageBox.Show($"{manager.Name} already exist");
                    return;
                }

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

                if(manager.IdSecDep is not null)
                {
                    properties.Add("Id_sec_dep");
                    values.Add("@idSecDep");
                    cmd.Parameters.AddWithValue("@idSecDep", manager.IdSecDep);
                }
                if(manager.IdChief is not null)
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
                    Managers.Add(manager);
                    MessageBox.Show("Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void createSaleBtn_Click(object sender, RoutedEventArgs e)
        {
            var salesCrudWindow = new SaleCrudWindow() { Owner = this };

            using SqlCommand cmd = new() { Connection = connection };

            if (salesCrudWindow.ShowDialog().Value)
            {
                var sale = salesCrudWindow.Sale;

                cmd.CommandText = $@"INSERT INTO Sales ( Id, SellTime, Quantity, IdProduct, IdManager ) VALUES (@id, @sellTime, @quantity, @idProduct, @idManager)";
                cmd.Parameters.AddWithValue("@id", sale.Id);
                cmd.Parameters.AddWithValue("@sellTime", sale.SellTime);
                cmd.Parameters.AddWithValue("@quantity", sale.Quantity);
                cmd.Parameters.AddWithValue("@idProduct", sale.IdProduct);
                cmd.Parameters.AddWithValue("@idManager", sale.IdManager);

                try
                {
                    cmd.ExecuteNonQuery();
                    Sales.Add(sale);
                    MessageBox.Show("Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
