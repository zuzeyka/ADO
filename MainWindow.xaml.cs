using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lesson1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;

        public MainWindow()
        {
            InitializeComponent();

            connection = new();
            connection.ConnectionString = App.ConnectionString;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                connectionStatusLabel.Content = "Connected";
                connectionStatusLabel.Foreground = Brushes.Green;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                connectionStatusLabel.Content = "Error";
                connectionStatusLabel.Foreground = Brushes.Red;
                Close();
            }

            ShowMonitor();
            ShowData();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private void installDepatrmentsBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new SqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = @"CREATE TABLE Departments (
	            Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	            Name		NVARCHAR(50) NOT NULL
            );";

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmd.Dispose();
        }

        private void fillDepatrmentsBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new SqlCommand(
                @"INSERT INTO Departments 
	            ( Id, Name )
            VALUES 
	            ( 'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',  N'IT отдел'		 	 ), 
	            ( '131EF84B-F06E-494B-848F-BB4BC0604266',  N'Бухгалтерия'		 ), 
	            ( '8DCC3969-1D93-47A9-8B79-A30C738DB9B4',  N'Служба безопасности'), 
	            ( 'D2469412-0E4B-46F7-80EC-8C522364D099',  N'Отдел кадров'		 ),
	            ( '1EF7268C-43A8-488C-B761-90982B31DF4E',  N'Канцелярия'		 ), 
	            ( '415B36D9-2D82-4A92-A313-48312F8E18C6',  N'Отдел продаж'		 ), 
	            ( '624B3BB5-0F2C-42B6-A416-099AAB799546',  N'Юридическая служба' )",
            connection);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmd.Dispose();
            ShowMonitorDepartments();
        }

        private void installProductsBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new SqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = @"CREATE TABLE Products (
	                Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                Name		NVARCHAR(50) NOT NULL,
	                Price		FLOAT  NOT NULL
                );";

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmd.Dispose();
        }

        private void fillProductsBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new SqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = @"INSERT INTO Products
	                ( Id, Name,	Price	)
                VALUES
                    ( 'DA1E17BB-A90D-4C79-B801-5462FB070F57', N'Гвоздь 100мм',			10.50	),
                    ( 'A8E6BE17-5447-4804-AB61-F31ABF5A76D3', N'Шуруп 4х35',			4.25	),
                    ( '21B0F444-2E4F-47D8-80C1-E69BF1C34CA8', N'Гайка М4',				6.50	),
                    ( '2DCA5E44-B06D-4613-BB6A-D3BC91430BFE', N'Гровер М4',			    5.99	),
                    ( '64A4DF8A-0733-4BE9-AABA-C01B4EC3612A', N'Болт 4х60',			    9.98	),
                    ( 'B6D20749-B495-4B1A-BA1C-80B88E78B7CD', N'Гвоздь 80мм',			19.98	),
                    ( '7B08197B-C55F-4389-891F-BF12A575DFFB', N'Отвертка PZ2',			35.50	),
                    ( '870DA1A9-44F4-4018-B7FC-727A2058FAF0', N'Шуруповерт',			799		),
                    ( '8FF90E21-DCDB-4D55-A557-7C6D57DBB029', N'Молоток',				216.50	),
                    ( 'F7F1E576-AF8D-4749-869E-4A794FE69D42', N'Набор ""Новосел""',		52.40	),
                    ('BB29F63D-1261-41F2-89E8-88F44D5EC409', N'Сверло 6х80', 39.98),
                    ('D17A4442-0A71-4673-B450-36929048ADEF', N'Шуруп 5х45',			5.98    ),
                    ('69B125D7-99CC-42D6-A6FA-46687F333749', N'Винт ""потай"" 3х16',		3.98    ),
                    ('94BC671A-A6B6-417A-BC9F-8AE4871A58EC', N'Дюбель 6х60',			5.50    ),
                    ('EFC6578A-00B7-4766-A7E3-79CDBA8C294B', N'Органайзер для шурупов',199     ),
                    ('9654271B-AB52-4225-A30C-D75054B1733F', N'Лазерный дальномер',	1950    ),
                    ('F2585221-1ACA-4EFE-A5E8-C2F4534D1F92', N'Дрель электрическая',	990     ),
                    ('4A550D3B-D1F2-40EF-AE4E-963612C6713A', N'Сварочный аппарат',		2099    ),
                    ('17DB11D1-F50E-4CF4-9C54-CF1BD45802EA', N'Электроды 3мм',			49.98   ),
                    ('7264D33A-16B9-4E22-B3F1-63D6DAE60078', N'Паяльник 40 Вт',		199.98  )";

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmd.Dispose();
            ShowMonitorProducts();
        }

        private void installManagersBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new SqlCommand(@"CREATE TABLE Managers (
	            Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	            Surname		NVARCHAR(50) NOT NULL,
	            Name		NVARCHAR(50) NOT NULL,
	            Secname		NVARCHAR(50) NOT NULL,
	            Id_main_dep UNIQUEIDENTIFIER NOT NULL REFERENCES Departments( Id ),
	            Id_sec_dep	UNIQUEIDENTIFIER REFERENCES Departments( Id ),
	            Id_chief	UNIQUEIDENTIFIER
            );", connection);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmd.Dispose();
        }

        private void fillManagersBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new SqlCommand(File.ReadAllText("Managers.sql"), connection);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmd.Dispose();
            ShowMonitorManagers();
        }

        private void deleteDepatrmentsBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new SqlCommand("drop table Departments", connection);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmd.Dispose();
        }

        private void deleteProductsBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new SqlCommand("drop table Products", connection);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmd.Dispose();
        }

        private void deleteManagersBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new SqlCommand("drop table Managers", connection);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmd.Dispose();
        }

        void ShowMonitorDepartments()
        {
            using var cmd = new SqlCommand(@"select count(*) from Departments", connection);

            try
            {
                var result = cmd.ExecuteScalar();
                departmentsStatusLabel.Content = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                departmentsStatusLabel.Content = "-";
            }

            cmd.Dispose();
        }
        
        void ShowMonitorProducts()
        {
            using var cmd = new SqlCommand(@"select count(*) from Products", connection);

            try
            {
                var result = cmd.ExecuteScalar();
                productsStatusLabel.Content = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                productsStatusLabel.Content = "-";
            }

            cmd.Dispose();
        }
        
        void ShowMonitorManagers()
        {
            using var cmd = new SqlCommand(@"select count(*) from Managers", connection);

            try
            {
                var result = cmd.ExecuteScalar();
                managersStatusLabel.Content = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                managersStatusLabel.Content = "-";
            }

            cmd.Dispose();
        }

        void ShowMonitor()
        {
            ShowMonitorDepartments();
            ShowMonitorProducts();
            ShowMonitorManagers();
        }

        void ShowDepartments()
        {
            using SqlCommand cmd = new("select * from Departments", connection);

            try
            {
                var reader = cmd.ExecuteReader();
                reader.Read();

                while (reader.Read())
                {
                    var str = GetShortGuid(reader.GetGuid(0)) + " " + reader.GetString(1);
                    departmentsListbox.Items.Add(new ListBoxItem() { Content = str });
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        void ShowProducts()
        {
            using SqlCommand cmd = new("select * from Products", connection);

            try
            {
                var reader = cmd.ExecuteReader();
                reader.Read();

                while (reader.Read())
                {
                    var str = GetShortGuid(reader.GetGuid(0)) + " " + reader.GetString(1);
                    productsListbox.Items.Add(new ListBoxItem() { Content = str });
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        void ShowManagers()
        {
            using SqlCommand cmd = new("select * from Managers", connection);

            try
            {
                var reader = cmd.ExecuteReader();
                reader.Read();

                while (reader.Read())
                {
                    var str = $"{GetShortGuid(reader.GetGuid(0))} {reader.GetString(1)} {reader.GetString(2)} {reader.GetString(3)}\n";
                    managersListbox.Items.Add(new ListBoxItem() { Content = str });
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ShowData()
        {
            ShowDepartments();
            ShowProducts();
            ShowManagers();
        }

        string GetShortGuid(Guid guid)
        {
            var str = guid.ToString();
            return $"{str.Substring(0, 3)}...{str.Substring(32)}";
        }
    }
}
