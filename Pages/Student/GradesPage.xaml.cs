using MySqlConnector;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ZlabGrade.Pages.Student;
using ZlabGrade.Pages.Teacher;
using ZlabGrade.Scripts;

namespace ZlabGrade
{
    public partial class GradesPage : Page
    {
        public GradesPage()
        {
            InitializeComponent();
        }

        readonly List<String> Predmety = [];
        readonly BindingList<Grade> gradesList = [];
        private bool creatingNewGrade = false;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoginWindow.role == "Učitel")
            {
                Thickness margin = Znamky.Margin;
                margin.Bottom = 60;
                Znamky.Margin = margin;

                TeacherButtons.Visibility = Visibility.Visible;
            }
            else
            {
                Thickness margin = Znamky.Margin;
                margin.Bottom = 10;
                Znamky.Margin = margin;

                TeacherButtons.Visibility = Visibility.Hidden;
            }

            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                await mySqlConnection.OpenAsync();

                string sqlQuery = "SELECT * FROM Grades ORDER BY predmet";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = await command.ExecuteReaderAsync();
                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        string predmet = dataReader["predmet"].ToString();
                        if (!Predmety.Contains(predmet))
                        {
                            Predmety.Add(predmet);
                        }
                    }
                }

                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            PredmetyComboBox.ItemsSource = Predmety;
            PredmetyComboBox.SelectedIndex = 0;
        }

        private async void PredmetyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PredmetyComboBox.SelectedItem == null) return;

            double soucetVazenychZnamek = 0;
            double soucetVah = 0;

            gradesList.Clear();

            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                await mySqlConnection.OpenAsync();
                
                string sqlQuery = "SELECT * FROM Grades WHERE predmet = @predmet AND id_zaka = @userID";
                
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                command.Parameters.AddWithValue("@predmet", PredmetyComboBox.SelectedItem.ToString());

                if (LoginWindow.role == "Učitel")
                {
                    command.Parameters.AddWithValue("@userID", GradingPage.studentUserID);
                }
                else
                {
                    command.Parameters.AddWithValue("@userID", LoginWindow.userID);
                }

                using MySqlDataReader dataReader = await command.ExecuteReaderAsync();
                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        Grade grade = new(Convert.ToInt32(dataReader["id_znamky"]), Convert.ToInt32(dataReader["id_zaka"]), dataReader["predmet"].ToString(), Convert.ToInt32(dataReader["znamka"]), Convert.ToInt32(dataReader["vaha"]), dataReader["popis"].ToString(), dataReader.GetDateTime("datum"));
                        gradesList.Add(grade);

                        //vypocet prumeru
                        try
                        {
                            int znamka = Convert.ToInt32(dataReader["znamka"]);
                            double vaha = Convert.ToDouble(dataReader["vaha"]);
                            soucetVazenychZnamek += znamka * vaha;
                            soucetVah += vaha;
                        }
                        catch
                        {

                        }
                    }

                    Znamky.ItemsSource = gradesList;
                }

                mySqlConnection.Close();

                if (soucetVah > 0)
                {
                    double vazenyPrumer = soucetVazenychZnamek / soucetVah;
                    SubjectAverageTextBlock.Text = $"Průměr: {vazenyPrumer:F2}";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewGradeButton_Click(object sender, RoutedEventArgs e)
        {
            Znamky.Visibility = Visibility.Hidden;
            PredmetyComboBox.Visibility = Visibility.Hidden;
            SubjectAverageTextBlock.Visibility = Visibility.Hidden;
            TeacherButtons.Visibility = Visibility.Hidden;
            TeacherGrades.Visibility = Visibility.Visible;

            GradeNumeric.Value = 1;
            WeightNumeric.Value = 1;
            DescriptionTextBox.Text = string.Empty;

            creatingNewGrade = true;
        }

        private void EditGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Znamky.SelectedItem == null) return;

            Znamky.Visibility = Visibility.Hidden;
            PredmetyComboBox.Visibility = Visibility.Hidden;
            SubjectAverageTextBlock.Visibility = Visibility.Hidden;
            TeacherButtons.Visibility = Visibility.Hidden;
            TeacherGrades.Visibility = Visibility.Visible;

            GradeNumeric.Value = gradesList[Znamky.SelectedIndex].Mark;
            WeightNumeric.Value = gradesList[Znamky.SelectedIndex].Weight;
            DescriptionTextBox.Text = gradesList[Znamky.SelectedIndex].Description;

            creatingNewGrade = false;
        }

        private async void DeleteGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Znamky.SelectedItem != null && Znamky.Visibility == Visibility.Visible)
            {
                if (MessageBox.Show("Opravdu si přejete smazat tuto známku? Tato akce je nevratná!", "Smazat známku", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using MySqlConnection mySqlConnection = new(Database.loginString);
                    try
                    {
                        await mySqlConnection.OpenAsync();

                        string sqlQuery = "DELETE FROM Grades WHERE id_znamky = @gradeID";
                        MySqlCommand command = new(sqlQuery, mySqlConnection);

                        command.Parameters.AddWithValue("@gradeID", gradesList[Znamky.SelectedIndex].GradeID);

                        await command.ExecuteNonQueryAsync();

                        mySqlConnection.Close();

                        PredmetyComboBox_SelectionChanged(null, null);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                WarningLabel.Visibility = Visibility.Hidden;

                using MySqlConnection mySqlConnection = new(Database.loginString);
                try
                {
                    await mySqlConnection.OpenAsync();

                    string sqlQuery;

                    if (creatingNewGrade)
                    {
                        sqlQuery = "INSERT INTO Grades (id_zaka, predmet, znamka, vaha, popis, datum) VALUES (@studentUserID, @subject, @grade, @weight, @description, @date)";
                    }
                    else
                    {
                        sqlQuery = "UPDATE Grades SET znamka = @grade, vaha = @weight, popis = @description, datum = @date WHERE id_znamky = @gradeID";
                    }

                    MySqlCommand command = new(sqlQuery, mySqlConnection);

                    if (creatingNewGrade)
                    {
                        command.Parameters.AddWithValue("@studentUserID", GradingPage.studentUserID);
                        command.Parameters.AddWithValue("@subject", PredmetyComboBox.SelectedItem.ToString());
                    }

                    command.Parameters.AddWithValue("@grade", GradeNumeric.Value);
                    command.Parameters.AddWithValue("@weight", WeightNumeric.Value);
                    command.Parameters.AddWithValue("@description", DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@date", DateTime.Now.Date);

                    if (!creatingNewGrade)
                    {
                        command.Parameters.AddWithValue("@gradeID", gradesList[Znamky.SelectedIndex].GradeID);
                    }

                    await command.ExecuteNonQueryAsync();

                    if (creatingNewGrade)
                    {
                        MessageBox.Show("Známka byla úspěšně zapsána", "Zapsání známky", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Změny byly úspěšně uloženy", "Úprava známky", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    mySqlConnection.Close();

                    PredmetyComboBox_SelectionChanged(null, null);

                    Znamky.Visibility = Visibility.Visible;
                    PredmetyComboBox.Visibility = Visibility.Visible;
                    SubjectAverageTextBlock.Visibility = Visibility.Visible;
                    TeacherButtons.Visibility = Visibility.Visible;
                    TeacherGrades.Visibility = Visibility.Hidden;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                WarningLabel.Visibility = Visibility.Visible;
            }
        }
    }

    public class Grade(int gradeID, int studentUserID, string subject, int mark, int weight, string description, DateTime date)
    {
        public int GradeID { get; set; } = gradeID;
        public int StudentUserID { get; set; } = studentUserID;
        public string Subject { get; set; } = subject;
        public int Mark { get; set; } = mark;
        public int Weight { get; set; } = weight;
        public string Description { get; set; } = description;
        public DateTime Date { get; set; } = date;
    }
}