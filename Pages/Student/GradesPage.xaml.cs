using System.Windows.Controls;
using MySql.Data.MySqlClient;
using ZlabGrade.Scripts;

namespace ZlabGrade
{
    public partial class GradesPage : Page
    {
        public GradesPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            List<String> Predmety = new List<String>();

            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                mySqlConnection.Open();

                string sqlQuery = $"SELECT predmet FROM Grades";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        string predmet = dataReader["predmet"].ToString();
                        if (!Predmety.Contains(predmet))
                        {
                            Predmety.Add(predmet);
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
            }

            PredmetyComboBox.ItemsSource = Predmety;
            PredmetyComboBox.SelectedIndex = 0;
        }

        private void PredmetyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                mySqlConnection.Open();

                string sqlQuery = @"SELECT popis, datum, znamka, vaha FROM Grades WHERE predmet = @predmet AND id_zaka = @userID";

                MySqlCommand command = new(sqlQuery, mySqlConnection);
                command.Parameters.AddWithValue("@predmet", PredmetyComboBox.SelectedItem.ToString());
                command.Parameters.AddWithValue("@userID", LoginWindow.userID);


                using MySqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    Znamky.Items.Clear();
                    while (dataReader.Read())
                    {
                        Znamky.Items.Add(new
                        {
                            Popis = dataReader["popis"].ToString(),
                            Datum = dataReader.GetDateTime("datum"),
                            Znamka = dataReader["znamka"].ToString(),
                            Vaha = dataReader["vaha"].ToString()
                        });
                    }
                }
                else
                {
                    Znamky.Items.Clear();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
            }
        }
    }
}