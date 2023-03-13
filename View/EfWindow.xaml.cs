using Lesson1.EFCore;
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
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Lesson1.View
{
    /// <summary>
    /// Interaction logic for EfWindow.xaml
    /// </summary>
    public partial class EfWindow : Window
    {
        EfContext context = new();
        ICollectionView departmentsView { get; set; }
        private static Random rnd = new();

        public EfWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.Departments.Load();
            var departments = context.Departments.Local.ToObservableCollection();

            depList.ItemsSource = departments;

            departmentsView = CollectionViewSource.GetDefaultView(departments);
            departmentsView.Filter = DepartmentFilter;

            UpdateMonitor();
        }

        void UpdateMonitor()
        {
            monitorTextBlock.Text = $"Departments: {context.Departments.Count()}\n";
            monitorTextBlock.Text += $"Products: {context.Products.Count()}\n";
            monitorTextBlock.Text += $"Managers: {context.Managers.Count()}\n";
            monitorTextBlock.Text += $"Sales: {context.Sales.Count()}";
        }

        void UpdateDailyStatistics()
        {
            salesChecks.Content = "0";
            salesPcs.Content = "0";
            bestPcs.Content = "0";
            startMoment.Content = "0";
            finishMoment.Content = "0";
            averagePcs.Content = "0";
        }

        private void addDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            DepartmentCrudWindow dialog = new();

            if (dialog.ShowDialog() == false) return;

            var d = dialog.Department;
            context.Departments.Add(new Department() { Id = d.Id, Name = d.Name });
            context.SaveChanges();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ListViewItem item) return;
            if (item.Content is not Department dep) return;

            DepartmentCrudWindow dialog = new() { Department = new Entity.Department(null) { Id = dep.Id, Name = dep.Name } };

            if (dialog.ShowDialog() == false) return;

            if(dialog.IsDeleted)
            {
                dep.DeletedAt = DateTime.Now;
            }
            else
            {
                dep.Name = dialog.Department.Name;
            }
            
            departmentsView.Refresh();

            context.SaveChanges();
        }

        private void showDeleted_Checked(object sender, RoutedEventArgs e)
        {
            departmentsView.Filter = null;
            (depList.View as GridView).Columns[2].Width = double.NaN;
        }

        private void showDeleted_Unchecked(object sender, RoutedEventArgs e)
        {
            departmentsView.Filter = DepartmentFilter;
            (depList.View as GridView).Columns[2].Width = 0;
        }

        bool DepartmentFilter(object obj)
        {
            if (obj is not Department department) return false;
            return department.DeletedAt is null;
        }

        private void genSalesButton_Click(object sender, RoutedEventArgs e)
        {
            int managersCount = context.Managers.Count();
            int productsCount = context.Products.Count();

            var createSale = () =>
            {
                var indexM = rnd.Next(managersCount);
                Manager manager = context.Managers.Skip(indexM).First();

                var indexP = rnd.Next(productsCount);
                Product product = context.Products.Skip(indexP).First();

                DateTime moment = DateTime.Today.AddSeconds(rnd.Next(0, 86400));

                int quantity = Convert.ToInt32(Math.Ceiling(50 * rnd.Next(5) / product.Price) + 1);

                return new Sale()
                {
                    Id = Guid.NewGuid(),
                    IdManager = manager.Id,
                    IdProduct = product.Id,
                    SellTime = moment,
                    Quantity = quantity
                };
            };

            for (int i = 0; i < 100; i++)
            {
                context.Sales.Add(createSale());
            }

            context.SaveChanges();
            UpdateMonitor();
        }
    }
}
