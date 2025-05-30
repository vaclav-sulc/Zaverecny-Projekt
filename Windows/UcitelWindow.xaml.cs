using System.Windows;
using System.Windows.Input;
using ZlabGrade.Pages;
using ZlabGrade.Pages.Student;

namespace ZlabGrade
{
    public partial class UcitelWindow : HandyControl.Controls.Window
    {
        public UcitelWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           Info.Text = $"{LoginWindow.name} {LoginWindow.surname}";
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
        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new GradesPage());
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new SchedulePage());
        }

        private void AbsenceButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new AbsencePage());
        }

        private void SubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new SubjectsPage());
        }
    }
}