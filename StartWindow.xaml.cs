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
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void basicButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new MainWindow().ShowDialog();
            Show();
        }

        private void ormButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new OrmWindow().ShowDialog();
            Show();
        }

        private void dalButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new DalWindow().ShowDialog();
            Show();
        }
    }
}
