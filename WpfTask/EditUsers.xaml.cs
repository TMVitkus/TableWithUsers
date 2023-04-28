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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTask
{
    /// <summary>
    /// Логика взаимодействия для EditUsers.xaml
    /// </summary>
    public partial class EditUsers : Window
    {
        public EditUsers()
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
        /// Метод, который редактирует пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditContent(object sender, RoutedEventArgs e)
        {
            // Сохраняем изменения и конвертируем в строку
            var newLogin = ((DataRowView)Table.SelectedItems[0]).Row["Login"].ToString();
            var newPassword = ((DataRowView)Table.SelectedItems[0]).Row["Password"].ToString();
            var newAddress = ((DataRowView)Table.SelectedItems[0]).Row["Address"].ToString();
            var newPhoneNumber = ((DataRowView)Table.SelectedItems[0]).Row["Phone_Number"].ToString();
            var newSignAdmin = ((DataRowView)Table.SelectedItems[0]).Row["Sign_Admin"].ToString();

            // Проверяем подходит ли строка требованиям
            if (newSignAdmin == "True" || newSignAdmin == "False")
            {
                using (SqlConnection connection = new SqlConnection(MainWindow.connectionString))
                {
                    connection.Open();
                    string queryString = $"SELECT * FROM [dbo].[UsersInformation]";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    // Изменям данные в таблице и сохраняем
                    command.CommandText = @"UPDATE [dbo].[UsersInformation]
                                        SET [Login] = @Login
                                          ,[Password] = @Password
                                          ,[Address] = @Address
                                          ,[Phone_Number] = @Phone_Number
                                          ,[Sign_Admin] = @Sign_Admin
                            WHERE Login = @Login";
                    command.Parameters.AddWithValue("Login", newLogin);
                    command.Parameters.AddWithValue("Password", newPassword);
                    command.Parameters.AddWithValue("Address", newAddress);
                    command.Parameters.AddWithValue("Phone_Number", newPhoneNumber);
                    command.Parameters.AddWithValue("Sign_Admin", newSignAdmin);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Редактирование прошло успешно!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Признак админа может быть только True или False!", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }           
        }
    }
}