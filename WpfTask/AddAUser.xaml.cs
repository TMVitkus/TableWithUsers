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
    /// Логика взаимодействия для AddAUser.xaml
    /// </summary>
    public partial class AddAUser : Window
    {
        public AddAUser()
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
        /// Метод, который добавляет пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddContent(object sender, RoutedEventArgs e)
        {
            try
            {
                // Сохраняем изменения и конвертируем в строку
                var newLogin = ((DataRowView)Table.SelectedItems[0]).Row["Login"].ToString();
                var newPassword = ((DataRowView)Table.SelectedItems[0]).Row["Password"].ToString();
                var newAddress = ((DataRowView)Table.SelectedItems[0]).Row["Address"].ToString();
                var newPhoneNumber = ((DataRowView)Table.SelectedItems[0]).Row["Phone_Number"].ToString();
                var newSignAdmin = ((DataRowView)Table.SelectedItems[0]).Row["Sign_Admin"].ToString();

                if (newSignAdmin == "True" || newSignAdmin == "False")
                {
                    using (SqlConnection connection = new SqlConnection(MainWindow.connectionString))
                    {
                        connection.Open();
                        string queryString = $"SELECT * FROM [dbo].[UsersInformation]";
                        SqlCommand command = new SqlCommand(queryString, connection);
                        // Добавляем пользователя в таблицу
                        command.CommandText = @"INSERT INTO[UsersInformation] (Login, Password, Address, Phone_Number, Sign_Admin) " +
                        $"VALUES (N'{newLogin}', N'{newPassword}', N'{newAddress}', N'{newPhoneNumber}', N'{newSignAdmin}')";
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Пользователь успешно добавлен!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Признак админа может быть только True или False!", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}