using System.Windows;
using System.Windows.Controls;
using MySqlConnector;
using ZlabGrade.Scripts;

namespace ZlabGrade
{
    public partial class GradesPage : Page
    {
        public GradesPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> Predmety = [];

            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                await mySqlConnection.OpenAsync();

                string sqlQuery = $"SELECT predmet FROM Grades";
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

            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                await mySqlConnection.OpenAsync();
                string sqlQuery = @"SELECT popis, datum, znamka, vaha FROM Grades WHERE predmet = @predmet AND id_zaka = @userID";
                MySqlCommand command = new(sqlQuery, mySqlConnection);
                command.Parameters.AddWithValue("@predmet", PredmetyComboBox.SelectedItem.ToString());
                command.Parameters.AddWithValue("@userID", LoginWindow.userID);
                using MySqlDataReader dataReader = await command.ExecuteReaderAsync();

                if (dataReader.HasRows)
                {
                    Znamky.Items.Clear();
                    while (await dataReader.ReadAsync())
                    {
                        Znamky.Items.Add(new
                        {
                            Popis = dataReader["popis"].ToString(),
                            Datum = dataReader.GetDateTime("datum"),
                            Znamka = dataReader["znamka"].ToString(),
                            Vaha = dataReader["vaha"].ToString()
                        });

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
                }
                else
                {
                    Znamky.Items.Clear();
                }
                mySqlConnection.Close();

                
                if (soucetVah > 0)
                {
                    double vazenyPrumer = soucetVazenychZnamek / soucetVah;
                    txtAvg.Text = vazenyPrumer.ToString("F2");
                }
                else
                {
                    txtAvg.Text = "N/A";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                txtAvg.Text = "Chyba";
            }
        }
    }
}