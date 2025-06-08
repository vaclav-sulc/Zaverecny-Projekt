using System.Windows.Controls;
using ZlabGrade.Scripts;
using MySqlConnector;
using System.Collections.ObjectModel;

namespace ZlabGrade.Pages.Student
{
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            NactiRozvrhAsync(LoginWindow.classroom);
        }

        public ObservableCollection<HodinaRozvrhu> Hodiny { get; set; } = new ObservableCollection<HodinaRozvrhu>();
        public async void NactiRozvrhAsync(string trida)
        {
            string sql = "SELECT ZkratkaDne, Hodina, ZkratkaPredmetu, ZkratkaUcitele, Mistnost FROM Schedules WHERE Trida = @trida";
            using MySqlConnection mySqlConnection = new(Database.loginString);
            using var command = new MySqlCommand(sql, mySqlConnection);
            var rozvrhDict = new Dictionary<(string, int), HodinaRozvrhu>();

            command.Parameters.AddWithValue("@trida", trida);
            try
            {
                await mySqlConnection.OpenAsync();
                using MySqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    string den = reader.GetString(0);
                    int hodina = reader.GetInt32(1);
                    string predmet = reader.GetString(2);
                    string ucitel = reader.GetString(3);
                    string mistnost = reader.GetString(4);

                    rozvrhDict[(den, hodina)] = new HodinaRozvrhu(den, hodina, predmet, ucitel, mistnost);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading schedule: {ex.Message}");
            }

            string[] dny = { "po", "út", "st", "čt", "pá" };
            Hodiny.Clear();
            foreach (string den in dny)
                for (int hod = 1; hod <= 10; hod++)
                    if (rozvrhDict.TryGetValue((den, hod), out var hodina))
                        Hodiny.Add(hodina);
                    else
                        Hodiny.Add(new HodinaRozvrhu(den, hod, "", "", ""));
        }
    }
}