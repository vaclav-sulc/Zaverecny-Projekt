using System.Windows;
using System.Windows.Controls;
using MySqlConnector;
using ZlabGrade.Scripts;

namespace ZlabGrade.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewPasswordBox.Password == ConfirmNewPasswordBox.Password)
            {
                if (!string.IsNullOrEmpty(NewPasswordBox.Password) && !string.IsNullOrEmpty(ConfirmNewPasswordBox.Password))
                {
                    using MySqlConnection mySqlConnection = new(Database.loginString);
                    try
                    {
                        await mySqlConnection.OpenAsync();

                        string sqlQuery = "UPDATE Credentials SET heslo = @password WHERE id_uzivatele = @userID";
                        MySqlCommand command = new(sqlQuery, mySqlConnection);

                        command.Parameters.AddWithValue("@password", Database.GetSha256Hash(NewPasswordBox.Password));
                        command.Parameters.AddWithValue("@userID", LoginWindow.userID);

                        await command.ExecuteNonQueryAsync();

                        WarningLabel.Visibility = Visibility.Hidden;
                        MessageBox.Show("Heslo bylo úspěšně změněno", "Změna hesla", MessageBoxButton.OK, MessageBoxImage.Information);

                        mySqlConnection.Close();

                        NewPasswordBox.ShowPassword = false;
                        ConfirmNewPasswordBox.ShowPassword = false;
                        NewPasswordBox.Password = string.Empty;
                        ConfirmNewPasswordBox.Password = string.Empty;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                WarningLabel.Visibility = Visibility.Visible;
            }
        }
    }
}