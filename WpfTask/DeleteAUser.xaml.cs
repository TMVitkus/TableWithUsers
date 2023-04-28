using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace WpfTask
{
    /// <summary>
    /// Логика взаимодействия для DeleteAUser.xaml
    /// </summary>
    public partial class DeleteAUser : Window
    {
        public DeleteAUser()
        {
            InitializeComponent();
            UserOutput();
        }

        /// <summary>
        /// Вывод таблицы
        /// </summary>
        private void UserOutput()
        {
            MainWindow.connection.Open();
            SqlCommand createCommand = new SqlCommand(MainWindow.conclusion, MainWindow.connection);
            createCommand.ExecuteNonQuery();
            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("UsersInformation");
            dataAdp.Fill(dt);
            Table.ItemsSource = dt.DefaultView;
            MainWindow.connection.Close();
        }

        /// <summary>
        /// Метод, который удаляет пользователя 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteContent(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить данного пользователя?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(MainWindow.connectionString))
                {
                    connection.Open();
                    string queryString = $"SELECT * FROM [dbo].[UsersInformation]";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandText = @"DELETE FROM[dbo].[UsersInformation] WHERE Login=@Login";
                    command.Parameters.AddWithValue("Login", ((DataRowView)Table.SelectedItems[0]).Row["Login"].ToString());
                    command.Parameters.AddWithValue("Password", ((DataRowView)Table.SelectedItems[0]).Row["Password"].ToString());
                    command.Parameters.AddWithValue("Address", ((DataRowView)Table.SelectedItems[0]).Row["Address"].ToString());
                    command.Parameters.AddWithValue("Phone_Number", ((DataRowView)Table.SelectedItems[0]).Row["Phone_Number"].ToString());
                    command.Parameters.AddWithValue("Sign_Admin", ((DataRowView)Table.SelectedItems[0]).Row["Sign_Admin"].ToString());
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Пользователь успешно удалён!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}