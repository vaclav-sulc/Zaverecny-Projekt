using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;
using MySql.Data.MySqlClient;

namespace Zaverecny_Projekt
{
    public partial class LoginWindow : FluentWindow
    {
        /*                  TO DO LIST
         * 
         *  Upravit hover effect u tlačítka "Login_Button"
         *  Přidat label varování zvlášt pro login a heslo
         *  Šifrovat hesla v databázi
         * 
         */

        public LoginWindow()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------\\
        //                          User Authentication                              \\
        //---------------------------------------------------------------------------\\

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new("server=sql7.freesqldatabase.com;user=sql7776236;password=rakYbIVDef;database=sql7776236;");
            try
            {
                mySqlConnection.Open();

                string sqlQuery = $"SELECT * FROM zlabgrade WHERE login = \"{LoginTextBox.Text.ToLower()}\" AND heslo = \"{PasswordTextBox.Password}\"";
                MySqlCommand command = new(sqlQuery, mySqlConnection);
                
                using MySqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    WarningBar.Visibility = Visibility.Hidden;

                    switch (dataReader["role"])
                    {
                        case "vedeni":
                            break;

                        case "ucitel":

                            ucitelView ucitelView = new();
                            this.Close();
                            ucitelView.Show();
                            break;

                        case "student":

                            Zak_vzhled studentWindow = new();
                            this.Close();
                            studentWindow.Show();
                            break;
                    }
                }
                else
                {
                    WarningBar.IsOpen = true;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
            }
        }
    }
}