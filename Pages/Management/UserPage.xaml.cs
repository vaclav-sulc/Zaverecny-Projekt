using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Controls;
using ZlabGrade.Scripts;

namespace ZlabGrade.Pages.Management
{
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
        }

        List<User> userList = [];

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                mySqlConnection.Open();

                string sqlQuery = $"SELECT * FROM Credentials";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    WarningText.Visibility = Visibility.Hidden;

                    while (dataReader.Read())
                    {
                        User user = new(Convert.ToInt32(dataReader["id_uzivatele"]), dataReader["jmeno"].ToString(), dataReader["prijmeni"].ToString(), dataReader["role"].ToString(), dataReader["trida"].ToString());
                        userList.Add(user);
                    }

                    UserList.ItemsSource = userList;
                }
                else
                {
                    WarningText.Visibility = Visibility.Visible;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
            }
        }

        public class User(int userID, string name, string surname, string role, string classroom)
        {
            public int userID = userID;
            public string name = name;
            public string surname = surname;
            public string role = role;
            public string classroom = classroom;

            public override string ToString()
            {
                return $"{userID}   {name} {surname}   {role}   {classroom}";
            }
        }
    }
}