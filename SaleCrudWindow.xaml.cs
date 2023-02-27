using Lesson1.Entity;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SaleCrudWindow.xaml
    /// </summary>
    public partial class SaleCrudWindow : Window
    {
        Sale sale;
        public Sale Sale
        {
            get => sale;
            set
            {
                if (value is not null)
                {
                    sale = value.Clone();
                    idViewLabel.Text = value.Id.ToString();
                    sellTimeViewLabel.Text = value.SellTime.ToString();
                    quantityViewLabel.Text = value.Quantity.ToString();
                }
                else
                {
                    sale = new();
                    idViewLabel.Text = sale.Id.ToString();
                    sellTimeViewLabel.Text = sale.SellTime.ToString();
                    quantityViewLabel.Text = sale.Quantity.ToString();
                }
            }
        }
        public bool IsDeleted { get; set; } = false;

        public SaleCrudWindow()
        {
            InitializeComponent();
            Sale = null;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Sale.Quantity = int.Parse(quantityViewLabel.Text);

            Sale.IdProduct = (productSelect.SelectedItem as Product)?.Id;
            Sale.IdManager = (managerSelect.SelectedItem as Manager)?.Id;

            if (Sale.Quantity < 1)
            {
                MessageBox.Show("Quantity is less than 1");
                quantityViewLabel.Focus();
                return;
            }
            else if (sale.IdProduct is null)
            {
                MessageBox.Show("Product is not selected");
                productSelect.Focus();
                return;
            }
            else if (sale.IdManager is null)
            {
                MessageBox.Show("Manager is not selected");
                managerSelect.Focus();
                return;
            }

            DialogResult = true;
            Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            IsDeleted = true;
            DialogResult = true;
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Owner;

            var owner = Owner as OrmWindow;
            var product = owner.Products.FirstOrDefault(p => p.Id == Sale?.IdProduct);
            var manager = owner.Managers.FirstOrDefault(m => m.Id == Sale?.IdManager);

            productSelect.SelectedItem = product;
            managerSelect.SelectedItem = manager;
        }
    }
}
