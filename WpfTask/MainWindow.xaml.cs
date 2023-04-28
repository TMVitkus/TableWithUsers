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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CheckingTheDatabase();
        }

        static public string conclusion = @"SELECT [User_Id]
                                  ,[Login]
                                  ,[Password]
                                  ,[Address]
                                  ,[Phone_Number]
                                  ,[Sign_Admin]
                              FROM[UsersDB].[dbo].[UsersInformation]";
        // Строка подключения
        static public string connectionString = @"Data Source=(localdb)\MSSQLLocalDb; Initial Catalog=UsersDB; Integrated Security=True;";
        static public SqlConnection connection = new SqlConnection(connectionString);

        /// <summary>
        /// Проверяем наличие базы данных
        /// </summary>
        public static void CheckingTheDatabase()
        {
            try
            {
                CreatingADatabase();
                connection.Close();
                try
                {
                    CreatingTables();
                    connection.Close();
                }
                catch
                {
                    connection.Close();
                }
            }
            catch
            {
                try
                {
                    CreatingTables();
                    connection.Close();
                }
                catch
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Создание базы данных
        /// </summary>
        public static void CreatingADatabase()
        {
            // Строка подключеня для создания базы данных
            string connectionStringForDB = @"Data Source=(localdb)\MSSQLLocalDb; Initial Catalog=master; Integrated Security=True;";
            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionStringForDB);
            connection.Open();
            SqlCommand command = new SqlCommand();
            // Выполняем команду на создание базы данных
            command.CommandText = "CREATE DATABASE UsersDB;";
            command.Connection = connection;
            command.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// Создание таблиц
        /// </summary>
        public static void CreatingTables()
        {
            SqlCommand command = new SqlCommand();
            connection.Open();
            // Выполняем команду на создание базы данных
            command.CommandText = "CREATE TABLE UsersInformation" +
                                  "(" +
                                     "User_Id INT PRIMARY KEY IDENTITY," +
                                     "Login NVARCHAR(50)," +
                                     "Password NVARCHAR(50)," +
                                     "Address NVARCHAR(50)," +
                                     "Phone_Number BIGINT," +
                                     "Sign_Admin NVARCHAR(5)" +
                                  ");";
            command.Connection = connection;
            command.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// Проверка и запись данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecordingData(object sender, RoutedEventArgs e)
        {
            connection.Open();
            var admin = (ComboBoxItem)adminAttribute.SelectedItem;
            string choiceAdmin;
            if (tbLogin.Text == "" || pass.Password == "" || tbAddress.Text == "" || tbTelephone.Text == "")
            {
                MessageBox.Show("Заполните поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                choiceAdmin = admin.Content.ToString();
                if (!LoginVerification(connectionString, tbLogin.Text))
                {
                    var addingAUser = $"INSERT INTO [UsersInformation] (Login, Password, Address, Phone_Number, Sign_Admin) " +
                    $"VALUES (N'{tbLogin.Text}', N'{pass.Password.GetHashCode()}', N'{tbAddress.Text}', N'{tbTelephone.Text}', N'{choiceAdmin}')";
                    SqlCommand command = new SqlCommand(addingAUser, connection);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Пользователь добавлен.", "Ура.", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Данный логин уже занят.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            connection.Close();
        }

        /// <summary>
        /// Вывод пользователей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserOutput(object sender, RoutedEventArgs e)
        {
            OutputOfTheUserTable(conclusion);
        }

        /// <summary>
        /// Проверка входа в систему
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        private bool LoginVerification(string conStr, string login)
        {
            string queryString = $"SELECT COUNT(*) FROM [dbo].[UsersInformation] WHERE Login = '{login}'";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                command.CommandText = queryString;
                command.Parameters.AddWithValue("Login", login);
                return (int)command.ExecuteScalar() == 1;
            }
        }

        /// <summary>
        /// Метод, который открывает окно, где можно измененить пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditContent(object sender, RoutedEventArgs e)
        {
            EditUsers editUsers = new EditUsers();
            editUsers.ShowDialog();
        }

        /// <summary>
        /// Метод, который открывает окно, где можно добавить пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddContent(object sender, RoutedEventArgs e)
        {
            AddAUser addAUser = new AddAUser();
            addAUser.ShowDialog();
        }

        /// <summary>
        /// Метод, который открывает окно, где можно удалить пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteContent(object sender, RoutedEventArgs e)
        {
            DeleteAUser deleteAUser = new DeleteAUser();
            deleteAUser.ShowDialog();
        }

        /// <summary>
        /// Фильтрация пользоватлей на админство
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilteringAdministrators(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Если хотите вывести администраторов нажмите \"Yes\", " +
                "а если хотите вывести пользователей, которые не являются администраторами нажмите \"No\"", 
                "Фильтрация", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                string cmd = @"SELECT * FROM [dbo].[UsersInformation] WHERE Sign_Admin = 'True'";
                OutputOfTheUserTable(cmd);
            }
            else
            {
                string cmd = @"SELECT * FROM [dbo].[UsersInformation] WHERE Sign_Admin = 'False'";
                OutputOfTheUserTable(cmd);
            }
        }

        /// <summary>
        /// Вывод таблицы
        /// </summary>
        /// <param name="cmd"></param>
        private void OutputOfTheUserTable(string cmd)
        {
            connection.Open();
            SqlCommand createCommand = new SqlCommand(cmd, connection);
            createCommand.ExecuteNonQuery();
            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("UsersInformation");
            dataAdp.Fill(dt);
            TableOutput.ItemsSource = dt.DefaultView;
            connection.Close();
        }
    }
}