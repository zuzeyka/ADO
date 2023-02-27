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
    /// Interaction logic for ManagerCrudWindow.xaml
    /// </summary>
    public partial class ManagerCrudWindow : Window
    {
        Manager manager;
        public Manager Manager
        {
            get => manager;
            set
            {
                if (value is not null)
                {
                    manager = value.Clone();
                    idViewLabel.Text = value.Id.ToString();
                    nameViewLabel.Text = value.Name;
                    surnameViewLabel.Text = value.Surname;
                    secnameViewLabel.Text = value.Secname;
                }
                else
                {
                    manager = new() { Id = Guid.NewGuid() };
                    idViewLabel.Text = manager.Id.ToString();
                }
            }
        }
        public bool IsDeleted { get; set; } = false;

        public ManagerCrudWindow()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.Name = nameViewLabel.Text;
            Manager.Surname = surnameViewLabel.Text;
            Manager.Secname = secnameViewLabel.Text;

            Manager.IdMainDep = (mainDepSelect.SelectedItem as Department)?.Id;
            Manager.IdSecDep = (secDepSelect.SelectedItem as Department)?.Id;
            Manager.IdChief = (chiefSelect.SelectedItem as Manager)?.Id;

            if(string.IsNullOrWhiteSpace(Manager.Name))
            {
                MessageBox.Show("Name is empty");
                nameViewLabel.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(Manager.Surname))
            {
                MessageBox.Show("Surname is empty");
                surnameViewLabel.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(Manager.Secname))
            {
                MessageBox.Show("Secname is empty");
                secnameViewLabel.Focus();
                return;
            }
            else if (mainDepSelect.SelectedItem is null)
            {
                MessageBox.Show("MainDep is empty");
                mainDepSelect.Focus();
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
            var mainDep = owner.Departments.FirstOrDefault(d => d.Id == Manager?.IdMainDep);
            var secDep = owner.Departments.FirstOrDefault(d => d.Id == Manager?.IdSecDep);
            var chief = owner.Managers.FirstOrDefault(m => m.Id == Manager?.IdChief);

            mainDepSelect.SelectedItem = mainDep;
            secDepSelect.SelectedItem = secDep;
            chiefSelect.SelectedItem = chief;
        }

        private void clearSecDepBtn_Click(object sender, RoutedEventArgs e)
        {
            secDepSelect.SelectedItem = null;
        }

        private void clearChiefBtn_Click(object sender, RoutedEventArgs e)
        {
            chiefSelect.SelectedItem = null;
        }
    }
}
