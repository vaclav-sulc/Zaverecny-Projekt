using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace ZlabGrade.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewPasswordBox.Password == ConfirmNewPasswordBox.Password && !string.IsNullOrEmpty(NewPasswordBox.Password))
            {
                using MySqlConnection mySqlConnection = new("server=sql7.freesqldatabase.com;user=sql7776236;password=rakYbIVDef;database=sql7776236;");
                try
                {
                    mySqlConnection.Open();

                    string sqlQuery = $"UPDATE Credentials SET heslo = @NewPassword WHERE id_uzivatele = {LoginWindow.userID}";
                    MySqlCommand command = new(sqlQuery, mySqlConnection);

                    command.Parameters.AddWithValue("@NewPassword", LoginWindow.GetStringSha256Hash(NewPasswordBox.Password));

                    command.ExecuteNonQuery();

                    WarningLabel.Visibility = Visibility.Hidden;
                    SuccessfulLabel.Visibility = Visibility.Visible;
                }
                catch (Exception exception)
                {
                    Console.WriteLine("ERROR: " + exception.Message);
                }
            }
            else
            {
                SuccessfulLabel.Visibility = Visibility.Hidden;
                WarningLabel.Visibility = Visibility.Visible;
            }
        }
    }
}