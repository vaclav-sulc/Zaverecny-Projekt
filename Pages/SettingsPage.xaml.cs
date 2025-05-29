using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using ZlabGrade.Scripts;

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
                using MySqlConnection mySqlConnection = new(Database.loginString);
                try
                {
                    mySqlConnection.Open();

                    string sqlQuery = $"UPDATE Credentials SET heslo = @password WHERE id_uzivatele = {LoginWindow.userID}";
                    MySqlCommand command = new(sqlQuery, mySqlConnection);

                    command.Parameters.AddWithValue("@password", Database.GetStringSha256Hash(NewPasswordBox.Password));

                    command.ExecuteNonQuery();

                    WarningLabel.Visibility = Visibility.Hidden;
                    MessageBox.Show("Heslo bylo úspěšně změněno", "Změna hesla", MessageBoxButton.OK, MessageBoxImage.Information);

                    mySqlConnection.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                WarningLabel.Visibility = Visibility.Visible;
            }
        }
    }
}