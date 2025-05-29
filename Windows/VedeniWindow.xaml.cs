using System.Windows;
using System.Windows.Input;
using ZlabGrade.Pages;
using ZlabGrade.Pages.Management;
using ZlabGrade.Pages.Student;

namespace ZlabGrade
{
    public partial class VedeniWindow : HandyControl.Controls.Window
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

        private void NonClientBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new();
            this.Close();
            loginWindow.Show();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void Close_Click(object sender, RoutedEventArgs e)
            => Close();

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