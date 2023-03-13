using Lesson1.DAL;
using Lesson1.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DalWindow.xaml
    /// </summary>
    public partial class DalWindow : Window
    {
        private DataContext context;

        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Manager> Managers { get; set; }

        public DalWindow()
        {
            InitializeComponent();
            context = new DataContext();
            Departments = new(context.DepartmentApi.GetAll());
            Managers = new(context.ManagerApi.GetAll());
            DataContext = this;
        }

        private void createDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            DepartmentCrudWindow dialog = new();
            if(dialog.ShowDialog() == true)
            {
                bool result = context.DepartmentApi.Create(dialog.Department);

                if(result)
                {
                    MessageBox.Show("Success");
                    Departments.Add(dialog.Department);
                }
            }
        }

        private void createManagerBtn_Click(object sender, RoutedEventArgs e)
        {
            ManagerCrudWindow dialog = new() { Manager = new(context), Departments = Departments, Managers = Managers };
            if (dialog.ShowDialog() == true)
            {
                bool result = context.ManagerApi.Create(dialog.Manager);

                if (result)
                {
                    MessageBox.Show("Success");
                    Managers.Add(dialog.Manager);
                }
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ListViewItem item)
            {
                return;
            }

            if (item.Content is not Department department) {
                return;
            }

            var dialog = new DepartmentCrudWindow() { Department = department };

            if(dialog.ShowDialog() == false)
            {
                return;
            }

            int index = Departments.IndexOf(department);
            if(dialog.IsDeleted)
            {
                if(context.DepartmentApi.Delete(dialog.Department.Id))
                {
                    MessageBox.Show("Success");
                    Departments.RemoveAt(index);
                }
            }
            else
            {
                if (context.DepartmentApi.Update(dialog.Department))
                {
                    MessageBox.Show("Success");
                    Departments[index] = dialog.Department;
                }
            }
        }

        private void ListViewItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ListViewItem item)
            {
                return;
            }

            if (item.Content is not Manager manager)
            {
                return;
            }

            var dialog = new ManagerCrudWindow() { Manager = manager, Departments = Departments, Managers = Managers };

            if (dialog.ShowDialog() == false)
            {
                return;
            }

            int index = Managers.IndexOf(manager);
            if (dialog.IsDeleted)
            {
                if (context.ManagerApi.Delete(dialog.Manager.Id))
                {
                    MessageBox.Show("Success");
                    Departments.RemoveAt(index);
                }
            }
            else
            {
                if (context.ManagerApi.Update(dialog.Manager))
                {
                    MessageBox.Show("Success");
                    Managers[index] = dialog.Manager;
                }
            }
        }
    }
}
