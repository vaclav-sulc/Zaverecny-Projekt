using System.Windows;
using ZlabGrade.Pages;
using ZlabGrade.Pages.Teacher;

namespace ZlabGrade
{
    public partial class UcitelWindow : Window
    {
        public UcitelWindow()
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
            ContentFrame.Navigate(new Pages.Teacher.NoticeboardPage());
        }

        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Pages.Teacher.GradesPage());
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Pages.Teacher.SchedulePage());
        }

        private void AbsenceButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Pages.Student.AbsencePage());
        }

        private void SubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Pages.Student.SubjectsPage());
        }
    }
}