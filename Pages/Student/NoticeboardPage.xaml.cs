using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MySqlConnector;
using ZlabGrade.Scripts;

namespace ZlabGrade.Pages.Student
{
    public partial class NoticeboardPage : Page
    {
        public NoticeboardPage()
        {
            InitializeComponent();
        }

        readonly BindingList<NoticeboardMessage> noticeboardMessages = [];
        private bool creatingNewMessage = false;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoginWindow.role == "Vedení")
            {
                ManagementButtons.Visibility = Visibility.Visible;
            }
            else
            {
                ManagementButtons.Visibility = Visibility.Hidden;
            }
            
            noticeboardMessages.Clear();

            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                await mySqlConnection.OpenAsync();

                string sqlQuery = "SELECT * FROM Noticeboard";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = await command.ExecuteReaderAsync();
                if (dataReader.HasRows)
                {
                    WarningText.Visibility = Visibility.Hidden;

                    while (await dataReader.ReadAsync())
                    {
                        NoticeboardMessage noticeboardMessage = new(Convert.ToInt32(dataReader["id_zpravy"]), dataReader["nadpis"].ToString(), dataReader["zprava"].ToString(), dataReader["autor"].ToString(), dataReader.GetDateTime("datum").ToString());
                        noticeboardMessages.Add(noticeboardMessage);
                    }

                    MessageList.ItemsSource = noticeboardMessages;
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

        private void MessageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MessageList.SelectedItem != null)
            {
                HeaderTextBlock.Text = noticeboardMessages[MessageList.SelectedIndex].Header;
                AuthorTextBlock.Text = $"Autor: {noticeboardMessages[MessageList.SelectedIndex].Author}\nVytvořeno: {noticeboardMessages[MessageList.SelectedIndex].DateTime}";
                MessageTextBlock.Text = noticeboardMessages[MessageList.SelectedIndex].Message;
            }
        }

        private void NewMessageButton_Click(object sender, RoutedEventArgs e)
        {
            MessageList.Visibility = Visibility.Hidden;
            HeaderTextBlock.Visibility = Visibility.Hidden;
            AuthorTextBlock.Visibility = Visibility.Hidden;
            MessageTextBlock.Visibility = Visibility.Hidden;
            MessageManagement.Visibility = Visibility.Visible;

            HeaderTextBox.Text = string.Empty;
            MessageTextBox.Text = string.Empty;

            creatingNewMessage = true;
        }

        private void EditMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageList.SelectedItem != null)
            {
                MessageList.Visibility = Visibility.Hidden;
                HeaderTextBlock.Visibility = Visibility.Hidden;
                AuthorTextBlock.Visibility = Visibility.Hidden;
                MessageTextBlock.Visibility = Visibility.Hidden;
                MessageManagement.Visibility = Visibility.Visible;

                HeaderTextBox.Text = noticeboardMessages[MessageList.SelectedIndex].Header;
                MessageTextBox.Text = noticeboardMessages[MessageList.SelectedIndex].Message;

                creatingNewMessage = false;
            }
        }

        private async void DeleteMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageList.SelectedItem != null && MessageList.Visibility == Visibility.Visible)
            {
                if (MessageBox.Show("Opravdu si přejete smazat tuto zprávu? Tato akce je nevratná!", "Smazat zprávu", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using MySqlConnection mySqlConnection = new(Database.loginString);
                    try
                    {
                        await mySqlConnection.OpenAsync();

                        string sqlQuery = "DELETE FROM Noticeboard WHERE id_zpravy = @messageID";
                        MySqlCommand command = new(sqlQuery, mySqlConnection);

                        command.Parameters.AddWithValue("@messageID", noticeboardMessages[MessageList.SelectedIndex].MessageID);

                        await command.ExecuteNonQueryAsync();

                        mySqlConnection.Close();

                        Page_Loaded(null, null);

                        HeaderTextBlock.Text = string.Empty;
                        MessageTextBlock.Text = string.Empty;
                        AuthorTextBlock.Text = string.Empty;
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
            if (!string.IsNullOrWhiteSpace(HeaderTextBox.Text) && !string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                WarningLabel.Visibility = Visibility.Hidden;

                using MySqlConnection mySqlConnection = new(Database.loginString);
                try
                {
                    await mySqlConnection.OpenAsync();

                    string sqlQuery;

                    if (creatingNewMessage)
                    {
                        sqlQuery = "INSERT INTO Noticeboard (nadpis, zprava, autor, datum) VALUES (@header, @message, @author, @dateTime)";
                    }
                    else
                    {
                        sqlQuery = "UPDATE Noticeboard SET nadpis = @header, zprava = @message, autor = @author, datum = @dateTime WHERE id_zpravy = @messageID";
                    }

                    MySqlCommand command = new(sqlQuery, mySqlConnection);

                    command.Parameters.AddWithValue("@header", HeaderTextBox.Text);
                    command.Parameters.AddWithValue("@message", MessageTextBox.Text);
                    command.Parameters.AddWithValue("@author", $"{LoginWindow.name} {LoginWindow.surname}");
                    command.Parameters.AddWithValue("@dateTime", DateTime.Now);

                    if (!creatingNewMessage)
                    {
                        command.Parameters.AddWithValue("@messageID", noticeboardMessages[MessageList.SelectedIndex].MessageID);
                    }

                    await command.ExecuteNonQueryAsync();

                    if (creatingNewMessage)
                    {
                        MessageBox.Show("Zpráva byla úspěšně vytvořena", "Vytvoření zprávy", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Změny byly úspěšně uloženy", "Úprava zprávy", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    mySqlConnection.Close();

                    MessageList.Visibility = Visibility.Visible;
                    HeaderTextBlock.Visibility = Visibility.Visible;
                    AuthorTextBlock.Visibility = Visibility.Visible;
                    MessageTextBlock.Visibility = Visibility.Visible;
                    MessageManagement.Visibility = Visibility.Hidden;

                    HeaderTextBlock.Text = string.Empty;
                    MessageTextBlock.Text = string.Empty;
                    AuthorTextBlock.Text = string.Empty;

                    Page_Loaded(null, null);
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

    public class NoticeboardMessage(int messageID, string header, string message, string author, string dateTime)
    {
        public int MessageID { get; set; } = messageID;
        public string Header { get; set; } = header;
        public string Message { get; set; } = message;
        public string Author { get; set; } = author;
        public string DateTime { get; set; } = dateTime;

        public override string ToString()
        {
            return header;
        }
    }
}