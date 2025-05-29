using HandyControl.Controls;
using System.Windows;
using System.Windows.Input;
using ZlabGrade.Pages;
using ZlabGrade.Pages.Student;

namespace ZlabGrade
{
    public partial class StudentWindow : HandyControl.Controls.Window
    {
        public StudentWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Info.Text = $"{LoginWindow.name} {LoginWindow.surname}, {LoginWindow.classroom}";
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

        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new GradesPage());
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new SchedulePage());
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
            loginWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
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