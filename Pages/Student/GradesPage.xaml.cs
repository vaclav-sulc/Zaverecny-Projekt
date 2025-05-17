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

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                mySqlConnection.Open();

                string sqlQuery = $"SELECT * FROM Grades";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = command.ExecuteReader();
                /*if (dataReader.HasRows)
                {
                    WarningText.Visibility = Visibility.Hidden;

                    while (dataReader.Read())
                    {
                        NoticeboardMessage noticeboardMessage = new(dataReader["nadpis"].ToString(), dataReader["zprava"].ToString(), dataReader["autor"].ToString());
                        noticeboardMessages.Add(noticeboardMessage);
                    }

                    MessageList.ItemsSource = noticeboardMessages;
                }
                else
                {
                    WarningText.Visibility = Visibility.Visible;
                }*/
            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR: " + exception.Message);
            }
        }
    }
}