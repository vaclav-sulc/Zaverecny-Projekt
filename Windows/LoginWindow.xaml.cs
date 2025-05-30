using System.Windows;
using System.Windows.Controls;
using MySqlConnector;
using ZlabGrade.Scripts;

namespace ZlabGrade
{
    public partial class LoginWindow : HandyControl.Controls.Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public static string name = string.Empty;
        public static string surname = string.Empty;
        public static string classroom = string.Empty;
        public static int userID;

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                await mySqlConnection.OpenAsync();

                string sqlQuery = $"SELECT * FROM Credentials WHERE login = \"{LoginTextBox.Text.ToLower()}\" AND heslo = \"{Database.GetStringSha256Hash(PasswordBox.Password)}\"";
                MySqlCommand command = new(sqlQuery, mySqlConnection);
                
                using MySqlDataReader dataReader = await command.ExecuteReaderAsync();
                if (dataReader.HasRows)
                {
                    await dataReader.ReadAsync();

                    WarningLabel.Visibility = Visibility.Hidden;

                    name = dataReader["jmeno"].ToString();
                    surname = dataReader["prijmeni"].ToString();
                    userID = Convert.ToInt32(dataReader["id_uzivatele"]);

                    switch (dataReader["role"])
                    {
                        case "Vedení":

                            VedeniWindow vedeniWindow = new();
                            this.Close();
                            vedeniWindow.Show();
                            break;

                        case "Učitel":

                            UcitelWindow ucitelWindow = new();
                            this.Close();
                            ucitelWindow.Show();
                            break;

                        case "Student":

                            classroom = dataReader["trida"].ToString();

                            StudentWindow studentWindow = new();
                            this.Close();
                            studentWindow.Show();
                            break;
                    }

                    mySqlConnection.Close();
                }
                else
                {
                    WarningLabel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}