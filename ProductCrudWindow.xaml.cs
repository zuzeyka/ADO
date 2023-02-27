using Lesson1.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ProductCrudWindow.xaml
    /// </summary>
    public partial class ProductCrudWindow : Window
    {
        Product product;
        public Product Product
        {
            get => product;
            set
            {
                if (value is not null)
                {
                    product = new() { Id = value.Id, Name = value.Name };
                    idViewLabel.Text = value.Id.ToString();
                    nameViewLabel.Text = value.Name;
                    priceViewLabel.Text = value.Price.ToString();
                }
                else
                {
                    product = Product = new() { Id = Guid.NewGuid(), Name = "", Price = 0 };
                }
            }
        }
        public bool IsDeleted { get; set; } = false;

        public ProductCrudWindow()
        {
            InitializeComponent();

            Product = null;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Product.Name = nameViewLabel.Text;
            Product.Price = Convert.ToDouble(priceViewLabel.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
            DialogResult = true;
            Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            IsDeleted = true;
            DialogResult = true;
            Close();
        }
    }
}
