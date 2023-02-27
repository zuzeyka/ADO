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
    /// Interaction logic for DepartmentCrudWindow.xaml
    /// </summary>
    public partial class DepartmentCrudWindow : Window
    {
        Department department;
        public Department Department
        {
            get => department;
            set
            {
                if(value is not null)
                {
                    department = new() { Id = value.Id, Name = value.Name };
                    idViewLabel.Text = value.Id.ToString();
                    nameViewLabel.Text = value.Name;
                }
                else
                {
                    department = new() { Id = Guid.NewGuid(), Name = "" };
                    idViewLabel.Text = department.Id.ToString();
                }
            }
        }
        public bool IsDeleted { get; set; } = false;

        public DepartmentCrudWindow()
        {
            InitializeComponent();

            Department = null;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Department.Name = nameViewLabel.Text;

            if(string.IsNullOrWhiteSpace(Department.Name))
            {
                MessageBox.Show("Name is empty");
                nameViewLabel.Focus();
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
    }
}
