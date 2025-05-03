using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using HandyControl.Tools;

namespace ZlabGrade
{
    public partial class LoginWindow : Window
    {
        /*                  TO DO LIST
         * 
         *  Přidat label varování zvlášt pro login a heslo
         *  
         */

        public LoginWindow()
        {
            InitializeComponent();
            ConfigHelper.Instance.SetLang("cz");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Template.FindName("PART_TextBox", PasswordBox) is TextBox textBox)
            {
                textBox.Foreground = Brushes.White;
                textBox.CaretBrush = Brushes.White;
            }
        }

        //---------------------------------------------------------------------------\\
        //                          User Authentication                              \\
        //---------------------------------------------------------------------------\\

        public static string name = string.Empty;
        public static string surname = string.Empty;

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new("server=sql7.freesqldatabase.com;user=sql7776236;password=rakYbIVDef;database=sql7776236;");
            try
            {
                mySqlConnection.Open();

                string sqlQuery = $"SELECT * FROM Credentials WHERE login = \"{LoginTextBox.Text.ToLower()}\" AND heslo = \"{GetStringSha256Hash(PasswordBox.Password)}\"";
                MySqlCommand command = new(sqlQuery, mySqlConnection);
                
                using MySqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    WarningLabel.Visibility = Visibility.Hidden;

                    name = dataReader["jmeno"].ToString();
                    surname = dataReader["prijmeni"].ToString();

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

                            StudentWindow studentWindow = new();
                            this.Close();
                            studentWindow.Show();
                            break;
                    }
                }
                else
                {
                    WarningLabel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
            }
        }

        private static string GetStringSha256Hash(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            byte[] hash = SHA256.HashData(buffer);

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}