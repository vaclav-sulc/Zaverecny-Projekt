using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using ZlabGrade.Scripts;

namespace ZlabGrade.Pages.Student
{
    public partial class NoticeboardPage : Page
    {
        public NoticeboardPage()
        {
            InitializeComponent();
        }

        List<NoticeboardMessage> noticeboardMessages = [];

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                mySqlConnection.Open();

                string sqlQuery = $"SELECT * FROM Noticeboard";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
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
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MessageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MessageList.SelectedItem != null)
            {
                HeaderTextBlock.Text = noticeboardMessages[MessageList.SelectedIndex].header;
                AuthorTextBlock.Text = $"Autor: {noticeboardMessages[MessageList.SelectedIndex].author}";
                MessageTextBlock.Text = noticeboardMessages[MessageList.SelectedIndex].message;
            }
        }
    }

    public class NoticeboardMessage(string header, string message, string author)
    {
        public string header = header;
        public string message = message;
        public string author = author;

        public override string ToString()
        {
            return header;
        }
    }
}