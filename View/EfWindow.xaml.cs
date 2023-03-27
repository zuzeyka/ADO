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

            context.Managers.Load();
            var managers = context.Managers.Local.ToObservableCollection();
            managersList.ItemsSource = managers;
            context.Sales.Load();
            var sales = context.Sales.Local.ToObservableCollection();

            UpdateMonitor();
            UpdateDailyStatistics();
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
            var todaySales = context.Sales.Where(s => s.SellTime >= DateTime.Today).ToList();

            if (todaySales.Count == 0) return;

            var query = context.Products.GroupJoin(
                context.Sales,
                (product) => product.Id,
                (sale) => sale.IdProduct,
                (product, sales) => new { Name = product.Name, Count = sales.Count(), Price = product.Price, Quantity = sales.Sum(s => s.Quantity) }
            ).OrderByDescending(i => i.Count).ToList();


            var countChecks = query.Sum(a => a.Count);
            var sums = query.Select(i => i.Price * i.Quantity);
            var sumPrices = sums.Sum();

            salesChecks.Content = todaySales.Count.ToString();
            salesPcs.Content = sumPrices + " грн";
            maxChecks.Content = todaySales.Max(s => s.Quantity);
            startMoment.Content = todaySales.Min(s => s.SellTime).ToShortTimeString();
            finishMoment.Content = todaySales.Max(s => s.SellTime).ToShortTimeString();
            averagePcs.Content = Math.Round(sumPrices / countChecks, 2) + " грн";
            deletedChecks.Content = todaySales.Where(s => s.DeletedAt is not null).Count();

            foreach (var item in query)
            {
                logBlock.Text += $"{item.Name} - {item.Count}\n";
            }

            var maxQuantity = query.Max(q => q.Quantity);
            var maxSum = sums.Max();

            var bestChecks = query.First();
            var bestCount = query.First(i => i.Quantity == maxQuantity);
            var bestSum = query.First(i => i.Price * i.Quantity == maxSum);

            bestCheckProduct.Content += $"{bestChecks.Name} ({bestChecks.Count})";
            bestCountProduct.Content += $"{bestCount.Name} ({maxQuantity} шт)";
            bestSumProduct.Content += $"{bestSum.Name} - ({maxSum} грн)";

            //////////////////////////////////////////////

            var queryManagers = context.Managers.GroupJoin(
                context.Sales.Where(s => s.DeletedAt == null).Join(
                    context.Products,
                    sale => sale.IdProduct,
                    product => product.Id,
                    (sale, product) => new { IdManager = sale.IdManager, Sum = sale.Quantity * product.Price, Quantity = sale.Quantity }
                ),
                (m) => m.Id,
                (s) => s.IdManager,
                (m, sales) => new { 
                    Manager = m,
                    TotalSum = sales.Sum(s => s.Sum),
                    Count = sales.Count(), 
                    Quantity = sales.Sum(s => s.Quantity)
                }
            );
            var managersCount = queryManagers.OrderByDescending(m => m.Count).ToList();
            var managersQuantity = queryManagers.OrderByDescending(m => m.Quantity).ToList();
            var managersMoney = queryManagers.OrderByDescending(m => m.TotalSum);

            var bestManagerC = managersCount.First();
            var bestManagerM = managersMoney.First();
            var top3 = managersQuantity.Take(3).ToList();

            bestManagerChecks.Content = $"{bestManagerC.Manager.Surname} {bestManagerC.Manager.Name[0]}. {bestManagerC.Manager.Secname[0]}. -- {bestManagerC.Count}";
            bestManagerMoney.Content = $"{bestManagerM.Manager.Surname} {bestManagerM.Manager.Name[0]}. {bestManagerM.Manager.Secname[0]}. -- {bestManagerM.TotalSum} грн";

            for (int i = 0; i < top3.Count; i++)
            {
                var m = top3[i];
                topThreeManager.Content += $"{i + 1}) {m.Manager.Surname} {m.Manager.Name[0]}. {m.Manager.Secname[0]}. -- {m.Quantity}\n";
            }

            //////////////////////////////////////////////

            var queryDepartments = context.Departments.ToList().GroupJoin(
                context.Managers.GroupJoin(
                    context.Sales.Where(s => s.DeletedAt == null),
                    manager => manager.Id,
                    sale => sale.IdManager,
                    (manager, sales) => new { IdMainDep = manager.IdMainDep, Count = sales.Count() }
                ),
                department => department.Id,
                i => i.IdMainDep,
                (department, i) => new { Name = department.Name, Count = i.Sum(s => s.Count) }
            ).OrderByDescending(i => i.Count).ToList();

            foreach (var item in queryDepartments)
            {
                depLogBlock.Text += $"{item.Name} - {item.Count} продаж\n";
            }
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
            UpdateDailyStatistics();
        }
    }
}
