using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace ZlabGrade.Pages.Teacher
{
    public partial class GradesPage : Page
    {
        public GradesPage()
        {
            InitializeComponent();
        }

        List<Student> students = [];
        List<Grade> grades = [];

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStudents();
        }

        private void LoadStudents()
        {
            students.Clear();
            using MySqlConnection mySqlConnection = new("server=sql7.freesqldatabase.com;user=sql7776236;password=rakYbIVDef;database=sql7776236;");
            try
            {
                mySqlConnection.Open();

                // Načtení všech studentů
                string sqlQuery = "SELECT id_uzivatele, jmeno, prijmeni FROM Credentials WHERE role = 'Student'";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Student student = new(
                        Convert.ToInt32(dataReader["id_uzivatele"]),
                        dataReader["jmeno"].ToString(),
                        dataReader["prijmeni"].ToString()
                    );
                    students.Add(student);
                }

                StudentsList.ItemsSource = students;
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
            }
        }

        private void StudentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentsList.SelectedItem != null)
            {
                Student selectedStudent = (Student)StudentsList.SelectedItem;
                StudentNameBlock.Text = $"{selectedStudent.Name} {selectedStudent.Surname}";
                LoadGrades(selectedStudent.Id);
            }
        }

        private void LoadGrades(int studentId)
        {
            grades.Clear();
            using MySqlConnection mySqlConnection = new("server=sql7.freesqldatabase.com;user=sql7776236;password=rakYbIVDef;database=sql7776236;");
            try
            {
                mySqlConnection.Open();

                // Načtení známek studenta
                string sqlQuery = "SELECT id_znamky, znamka, popis, datum FROM Znamky WHERE id_studenta = @studentId AND id_ucitele = @teacherId";
                MySqlCommand command = new(sqlQuery, mySqlConnection);
                command.Parameters.AddWithValue("@studentId", studentId);
                command.Parameters.AddWithValue("@teacherId", LoginWindow.userID);

                using MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Grade grade = new(
                        Convert.ToInt32(dataReader["id_znamky"]),
                        Convert.ToInt32(dataReader["znamka"]),
                        dataReader["popis"].ToString(),
                        Convert.ToDateTime(dataReader["datum"])
                    );
                    grades.Add(grade);
                }

                GradesList.ItemsSource = grades;
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
            }
        }

        private void AddGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsList.SelectedItem == null)
            {
                MessageBox.Show("Nejprve vyberte studenta!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(GradeValueBox.Text, out int gradeValue) || gradeValue < 1 || gradeValue > 5)
            {
                MessageBox.Show("Zadejte platnou známku (1-5)!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(GradeDescriptionBox.Text))
            {
                MessageBox.Show("Zadejte popis známky!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Student selectedStudent = (Student)StudentsList.SelectedItem;

            using MySqlConnection mySqlConnection = new("server=sql7.freesqldatabase.com;user=sql7776236;password=rakYbIVDef;database=sql7776236;");
            try
            {
                mySqlConnection.Open();

                string sqlQuery = "INSERT INTO Znamky (id_studenta, id_ucitele, znamka, popis, datum) VALUES (@studentId, @teacherId, @grade, @description, @date)";
                MySqlCommand command = new(sqlQuery, mySqlConnection);
                command.Parameters.AddWithValue("@studentId", selectedStudent.Id);
                command.Parameters.AddWithValue("@teacherId", LoginWindow.userID);
                command.Parameters.AddWithValue("@grade", gradeValue);
                command.Parameters.AddWithValue("@description", GradeDescriptionBox.Text);
                command.Parameters.AddWithValue("@date", DateTime.Now);

                command.ExecuteNonQuery();

                // Vyčistit formulář
                GradeValueBox.Text = "";
                GradeDescriptionBox.Text = "";

                // Obnovit seznam známek
                LoadGrades(selectedStudent.Id);

                MessageBox.Show("Známka byla úspěšně přidána!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
                MessageBox.Show("Chyba při přidávání známky!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (GradesList.SelectedItem == null)
            {
                MessageBox.Show("Nejprve vyberte známku ke smazání!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Grade selectedGrade = (Grade)GradesList.SelectedItem;

            if (MessageBox.Show("Opravdu chcete smazat tuto známku?", "Potvrzení", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using MySqlConnection mySqlConnection = new("server=sql7.freesqldatabase.com;user=sql7776236;password=rakYbIVDef;database=sql7776236;");
                try
                {
                    mySqlConnection.Open();

                    string sqlQuery = "DELETE FROM Znamky WHERE id_znamky = @gradeId";
                    MySqlCommand command = new(sqlQuery, mySqlConnection);
                    command.Parameters.AddWithValue("@gradeId", selectedGrade.Id);

                    command.ExecuteNonQuery();

                    // Obnovit seznam známek
                    Student selectedStudent = (Student)StudentsList.SelectedItem;
                    LoadGrades(selectedStudent.Id);

                    MessageBox.Show("Známka byla úspěšně smazána!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("ERROR: " + exception.Message);
                    MessageBox.Show("Chyba při mazání známky!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class Student(int id, string name, string surname)
    {
        public int Id = id;
        public string Name = name;
        public string Surname = surname;

        public override string ToString()
        {
            return $"{Name} {Surname}";
        }
    }

    public class Grade(int id, int value, string description, DateTime date)
    {
        public int Id = id;
        public int Value = value;
        public string Description = description;
        public DateTime Date = date;

        public override string ToString()
        {
            return $"{Value} - {Description} ({Date:dd.MM.yyyy})";
        }
    }
}