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
                    SubjectDataGrid.Items.Clear();

                    while (dataReader.Read())
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