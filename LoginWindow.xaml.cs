using System.Windows;

namespace Zaverecny_Projekt
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Login_TextBox.Text == "Uživatelské jméno")
            {
                Login_TextBox.Text = string.Empty;
            }
        }

        private void Login_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Login_TextBox.Text == string.Empty)
            {
                Login_TextBox.Text = "Uživatelské jméno";
            }
        }

        private void Password_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password_TextBox.Text == "Heslo")
            {
                Password_TextBox.Text = string.Empty;
            }
        }

        private void Password_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Password_TextBox.Text == string.Empty)
            {
                Password_TextBox.Text = "Heslo";
            }
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Login_TextBox.Text.ToLower() != "rothbad" || Password_TextBox.Text != "heslo123")
            {
                Warning_Label.Visibility = Visibility.Visible;

                Login_TextBox.Text = "Uživatelské jméno";
                Password_TextBox.Text = "Heslo";
            }
            else
            {
                Warning_Label.Visibility = Visibility.Hidden;
            }
        }
    }
}
