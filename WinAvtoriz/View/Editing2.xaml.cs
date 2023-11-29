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

namespace WinAvtoriz.View
{
    /// <summary>
    /// Логика взаимодействия для Editing2.xaml
    /// </summary>
    public partial class Editing2 : Window
    {
        public Editing2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main main = new Main();
            if (main.dataGrid.SelectedItem != null)
            {
                DataRowView selrow = (DataRowView)main.dataGrid.SelectedItem;
                string uName = selrow["id"].ToString();
                try
                {
                    using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=RegAvt_BD;Integrated Security=True"))
                    {
                        connection.Open();

                        string del = $"DELETE FROM Table_1 WHERE id = '{uName}'";

                        using (SqlCommand command = new SqlCommand(del, connection))
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Пользователь был удачно удалён");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удаление Users: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Не был выбран пользователь!");
            }
        }
    }
}
