using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using ZlabGrade.Scripts;

namespace ZlabGrade.Pages.Student
{
    public partial class SubjectsPage : Page
    {
        public SubjectsPage()
        {
            InitializeComponent();
        }

        readonly List<Subject> subjects = [];

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                mySqlConnection.Open();

                string sqlQuery = $"SELECT * FROM Subjects WHERE trida = \"{LoginWindow.classroom}\"";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    WarningText.Visibility = Visibility.Hidden;

                    while (dataReader.Read())
                    {
                        Subject subject = new(dataReader["predmet"].ToString(), dataReader["vyucujici"].ToString());
                        subjects.Add(subject);
                    }

                    SubjectList.ItemsSource = subjects;
                }
                else
                {
                    WarningText.Visibility = Visibility.Visible;
                }

                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class Subject(string name, string teachers)
    {
        public string name = name;
        public string teachers = teachers;

        public override string ToString()
        {
            return $"{name}\n{teachers}";
        }
    }
}