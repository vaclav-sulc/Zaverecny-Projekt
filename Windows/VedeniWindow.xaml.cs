using System.Windows;
using ZlabGrade.Pages;
using ZlabGrade.Pages.Student;
using ZlabGrade.Pages.Management;

namespace ZlabGrade
{
    public partial class VedeniWindow : Window
    {
        public VedeniWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NameButton.Content = $"{LoginWindow.name} {LoginWindow.surname}";
        }

        private void NameButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new();
            this.Close();
            loginWindow.Show();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new SettingsPage());
        }

        private void NoticeboardButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new NoticeboardPage());
        }

        private void SchedulesButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new SchedulePage());
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new UserPage());
        }
    }
}