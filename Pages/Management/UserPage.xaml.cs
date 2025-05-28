using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ZlabGrade.Scripts;

namespace ZlabGrade.Pages.Management
{
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
        }

        readonly BindingList<User> userList = [];
        private bool creatingNewUser = false;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                mySqlConnection.Open();

                string sqlQuery = $"SELECT * FROM Credentials";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    WarningText.Visibility = Visibility.Hidden;

                    while (dataReader.Read())
                    {
                        User user = new(Convert.ToInt32(dataReader["id_uzivatele"]), dataReader["jmeno"].ToString(), dataReader["prijmeni"].ToString(), dataReader["login"].ToString(), dataReader["role"].ToString(), dataReader["trida"].ToString());
                        userList.Add(user);
                    }

                    UserList.ItemsSource = userList;
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

        private void NewUserButton_Click(object sender, RoutedEventArgs e)
        {
            UserTextBoxes.Visibility = Visibility.Visible;
            UserList.Visibility = Visibility.Hidden;

            NameTextBox.Text = string.Empty;
            SurnameTextBox.Text = string.Empty;
            LoginTextBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            ClassroomTextBox.Text = string.Empty;

            creatingNewUser = true;
        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserList.SelectedItem != null)
            {
                UserTextBoxes.Visibility = Visibility.Visible;
                UserList.Visibility = Visibility.Hidden;

                NameTextBox.Text = userList[UserList.SelectedIndex].name;
                SurnameTextBox.Text = userList[UserList.SelectedIndex].surname;
                LoginTextBox.Text = userList[UserList.SelectedIndex].login;
                ClassroomTextBox.Text = userList[UserList.SelectedIndex].classroom;

                creatingNewUser = false;
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserList.SelectedItem != null && UserList.Visibility == Visibility.Visible)
            {
                if (MessageBox.Show("Opravdu si přejete smazat tohoto uživatele? Tato akce je nevratná!", "Smazat uživatele", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using MySqlConnection mySqlConnection = new(Database.loginString);
                    try
                    {
                        mySqlConnection.Open();

                        string sqlQuery = $"DELETE FROM Credentials WHERE id_uzivatele = {userList[UserList.SelectedIndex].userID}";
                        MySqlCommand command = new(sqlQuery, mySqlConnection);
                        command.ExecuteNonQuery();

                        userList.RemoveAt(UserList.SelectedIndex);

                        mySqlConnection.Close();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameTextBox.Text) && !string.IsNullOrWhiteSpace(SurnameTextBox.Text) && !string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                WarningLabel.Visibility = Visibility.Hidden;
                
                using MySqlConnection mySqlConnection = new(Database.loginString);
                try
                {
                    mySqlConnection.Open();

                    string sqlQuery;

                    if (creatingNewUser)
                    {
                        if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
                        {
                            sqlQuery = "INSERT INTO Credentials (jmeno, prijmeni, login, heslo, role, trida) VALUES (@name, @surname, @login, @password, @role, @classroom)";
                        }
                        else
                        {
                            WarningLabel.Visibility = Visibility.Visible;
                            mySqlConnection.Close();
                            return;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                        {
                            sqlQuery = $"UPDATE Credentials SET jmeno = @name, prijmeni = @surname, login = @login, role = @role, trida = @classroom WHERE id_uzivatele = {userList[UserList.SelectedIndex].userID}";
                        }
                        else
                        {
                            sqlQuery = $"UPDATE Credentials SET jmeno = @name, prijmeni = @surname, login = @login, heslo = @password, role = @role, trida = @classroom WHERE id_uzivatele = {userList[UserList.SelectedIndex].userID}";
                        }
                    }

                    MySqlCommand command = new(sqlQuery, mySqlConnection);

                    command.Parameters.AddWithValue("@name", NameTextBox.Text);
                    command.Parameters.AddWithValue("@surname", SurnameTextBox.Text);
                    command.Parameters.AddWithValue("@login", LoginTextBox.Text);
                    command.Parameters.AddWithValue("@password", Database.GetStringSha256Hash(PasswordBox.Password));
                    command.Parameters.AddWithValue("@role", RoleComboBox.Text);
                    command.Parameters.AddWithValue("@classroom", ClassroomTextBox.Text);

                    command.ExecuteNonQuery();

                    if (creatingNewUser)
                    {
                        MessageBox.Show("Uživatel byl úspěšně vytvořen", "Založení uživatele", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Změny byly úspěšně uloženy", "Úprava uživatele", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    mySqlConnection.Close();
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

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (UserTextBoxes.Visibility == Visibility.Visible)
            {
                if (RoleComboBox.SelectedItem.ToString() == "Student")
                {
                    ClassroomTextBox.Visibility = Visibility.Visible;
                }
                else
                {
                    ClassroomTextBox.Visibility = Visibility.Hidden;
                }
            }*/
        }

        public class User(int userID, string name, string surname, string login, string role, string classroom)
        {
            public int userID = userID;
            public string name = name;
            public string surname = surname;
            public string login = login;
            public string role = role;
            public string classroom = classroom;

            public override string ToString()
            {
                return $"{userID}   {name} {surname}   {role}   {classroom}";
            }
        }
    }
}