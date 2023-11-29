using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using WinAvtoriz.ViewModels;
using System.Runtime.Remoting.Contexts;

namespace WinAvtoriz.View
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public string ID11;
        public Main()
        {
            InitializeComponent();
            DataContext = new UserVM();
        }
        private string connectionString = "Data Source=localhost;Initial Catalog=RegAvt_BD;Integrated Security=True";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Reg2 rg = new Reg2();
            rg.Show();
        }
        
        private void Fun1()
        {
            SqlConnection connection = new SqlConnection("Data Source = localhost; Initial Catalog = RegAvt_BD; Integrated Security = True");

            connection.Open();
            string cmd = "SELECT * FROM Table_1"; 
            SqlCommand createCommand = new SqlCommand(cmd, connection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("Table_1"); 
            dataAdp.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;
            connection.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Fun1();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            setting.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Fun1();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Editing2 editing = new Editing2();
            editing.Show();

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            DelUser delUser = new DelUser();
            delUser.Show();
        }
    }
}
