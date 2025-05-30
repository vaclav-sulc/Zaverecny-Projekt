using System.Windows;
using System.Windows.Controls;
using MySqlConnector;
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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                await mySqlConnection.OpenAsync();

                string sqlQuery = $"SELECT * FROM Subjects WHERE trida = \"{LoginWindow.classroom}\"";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = await command.ExecuteReaderAsync();
                if (dataReader.HasRows)
                {
                    WarningText.Visibility = Visibility.Hidden;
                    SubjectDataGrid.Items.Clear();

                    while (await dataReader.ReadAsync())
                    {
                        SubjectDataGrid.Items.Add(new
                        {
                            Subject = dataReader["predmet"].ToString(),
                            Teacher = dataReader["vyucujici"].ToString()
                        });
                    }
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
}