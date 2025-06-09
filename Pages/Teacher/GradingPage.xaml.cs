using MySqlConnector;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ZlabGrade.Scripts;

namespace ZlabGrade.Pages.Teacher
{
    public partial class GradingPage : Page
    {
        public GradingPage()
        {
            InitializeComponent();
        }

        readonly BindingList<Student> studentList = [];
        public static int studentUserID;

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            studentList.Clear();

            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                await mySqlConnection.OpenAsync();

                string sqlQuery;

                if (string.IsNullOrEmpty(StudentNameTextBox.Text))
                {
                    sqlQuery = "SELECT * FROM Credentials WHERE role = \"Student\" AND trida LIKE @classroom";
                }
                else if (string.IsNullOrEmpty(ClassroomTextBox.Text))
                {
                    sqlQuery = "SELECT * FROM Credentials WHERE role = \"Student\" AND (jmeno LIKE @fullName OR prijmeni LIKE @fullName)";
                }
                else
                {
                    sqlQuery = "SELECT * FROM Credentials WHERE role = \"Student\" AND (jmeno LIKE @fullName OR prijmeni LIKE @fullName) AND trida LIKE @classroom";
                }

                MySqlCommand command = new(sqlQuery, mySqlConnection);

                command.Parameters.AddWithValue("@fullName", $"%{StudentNameTextBox.Text}%");
                command.Parameters.AddWithValue("@classroom", $"%{ClassroomTextBox.Text}%");

                using MySqlDataReader dataReader = await command.ExecuteReaderAsync();
                if (dataReader.HasRows)
                {
                    WarningLabel.Visibility = Visibility.Hidden;

                    while (await dataReader.ReadAsync())
                    {
                        Student student = new(Convert.ToInt32(dataReader["id_uzivatele"]), dataReader["jmeno"].ToString(), dataReader["prijmeni"].ToString(), dataReader["trida"].ToString());
                        studentList.Add(student);
                    }

                    StudentDataGrid.ItemsSource = studentList;
                }
                else
                {
                    WarningLabel.Visibility = Visibility.Visible;
                }

                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem != null)
            {
                studentUserID = studentList[StudentDataGrid.SelectedIndex].UserID;

                UcitelWindow ucitelWindow = Window.GetWindow(this) as UcitelWindow;
                ucitelWindow.ContentFrame.Navigate(new GradesPage());
            }
        }
    }

    public class Student(int userID, string name, string surname, string classroom)
    {
        public int UserID { get; set; } = userID;
        public string Name { get; set; } = name;
        public string Surname { get; set; } = surname;
        public string Classroom { get; set; } = classroom;
    }
}