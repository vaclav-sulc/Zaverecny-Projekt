﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MySqlConnector;
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

        private async void Page_Loaded(object? sender, RoutedEventArgs? e)
        {
            using MySqlConnection mySqlConnection = new(Database.loginString);
            try
            {
                await mySqlConnection.OpenAsync();

                string sqlQuery = "SELECT * FROM Credentials";
                MySqlCommand command = new(sqlQuery, mySqlConnection);

                using MySqlDataReader dataReader = await command.ExecuteReaderAsync();
                if (dataReader.HasRows)
                {
                    WarningText.Visibility = Visibility.Hidden;
                    userList.Clear();

                    while (await dataReader.ReadAsync())
                    {
                        User user = new(Convert.ToInt32(dataReader["id_uzivatele"]), dataReader["jmeno"].ToString(), dataReader["prijmeni"].ToString(), dataReader["login"].ToString(), dataReader["role"].ToString(), dataReader["trida"].ToString());
                        userList.Add(user);
                    }

                    UserDataGrid.ItemsSource = userList;
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
            UserDataGrid.Visibility = Visibility.Hidden;

            NameTextBox.Text = string.Empty;
            SurnameTextBox.Text = string.Empty;
            LoginTextBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            ClassroomTextBox.Text = string.Empty;

            creatingNewUser = true;
        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserDataGrid.SelectedItem != null)
            {
                UserTextBoxes.Visibility = Visibility.Visible;
                UserDataGrid.Visibility = Visibility.Hidden;

                NameTextBox.Text = userList[UserDataGrid.SelectedIndex].Name;
                SurnameTextBox.Text = userList[UserDataGrid.SelectedIndex].Surname;
                LoginTextBox.Text = userList[UserDataGrid.SelectedIndex].Login;
                ClassroomTextBox.Text = userList[UserDataGrid.SelectedIndex].Classroom;

                switch (userList[UserDataGrid.SelectedIndex].Role)
                {
                    case "Student":
                        RoleComboBox.SelectedIndex = 0;
                        break;
                    case "Učitel":
                        RoleComboBox.SelectedIndex = 1;
                        break;
                    case "Vedení":
                        RoleComboBox.SelectedIndex = 2;
                        break;
                }

                creatingNewUser = false;
            }
        }

        private async void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserDataGrid.SelectedItem != null && UserDataGrid.Visibility == Visibility.Visible)
            {
                if (MessageBox.Show("Opravdu si přejete smazat tohoto uživatele? Tato akce je nevratná!", "Smazat uživatele", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using MySqlConnection mySqlConnection = new(Database.loginString);
                    try
                    {
                        await mySqlConnection.OpenAsync();

                        string sqlQuery = "DELETE FROM Credentials WHERE id_uzivatele = @userID";
                        MySqlCommand command = new(sqlQuery, mySqlConnection);

                        command.Parameters.AddWithValue("@userID", userList[UserDataGrid.SelectedIndex].UserID);

                        await command.ExecuteNonQueryAsync();

                        mySqlConnection.Close();

                        Page_Loaded(null, null);
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
            if (RoleComboBox.SelectedIndex == 0 && string.IsNullOrWhiteSpace(ClassroomTextBox.Text))
            {
                WarningLabel.Visibility = Visibility.Visible;
                return;
            }

            if (!string.IsNullOrWhiteSpace(NameTextBox.Text) && !string.IsNullOrWhiteSpace(SurnameTextBox.Text) && !string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                WarningLabel.Visibility = Visibility.Hidden;
                
                using MySqlConnection mySqlConnection = new(Database.loginString);
                try
                {
                    await mySqlConnection.OpenAsync();

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
                            sqlQuery = "UPDATE Credentials SET jmeno = @name, prijmeni = @surname, login = @login, role = @role, trida = @classroom WHERE id_uzivatele = @userID";
                        }
                        else
                        {
                            sqlQuery = "UPDATE Credentials SET jmeno = @name, prijmeni = @surname, login = @login, heslo = @password, role = @role, trida = @classroom WHERE id_uzivatele = @userID";
                        }
                    }

                    MySqlCommand command = new(sqlQuery, mySqlConnection);

                    command.Parameters.AddWithValue("@name", NameTextBox.Text);
                    command.Parameters.AddWithValue("@surname", SurnameTextBox.Text);
                    command.Parameters.AddWithValue("@login", LoginTextBox.Text);
                    command.Parameters.AddWithValue("@password", Database.GetSha256Hash(PasswordBox.Password));
                    command.Parameters.AddWithValue("@role", RoleComboBox.Text);
                    command.Parameters.AddWithValue("@classroom", ClassroomTextBox.Text);

                    if (!creatingNewUser)
                    {
                        command.Parameters.AddWithValue("@userID", userList[UserDataGrid.SelectedIndex].UserID);
                    }

                    await command.ExecuteNonQueryAsync();

                    if (creatingNewUser)
                    {
                        MessageBox.Show("Uživatel byl úspěšně vytvořen", "Založení uživatele", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Změny byly úspěšně uloženy", "Úprava uživatele", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    mySqlConnection.Close();

                    UserTextBoxes.Visibility = Visibility.Hidden;
                    UserDataGrid.Visibility = Visibility.Visible;
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

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassroomTextBox != null)
            {
                if (RoleComboBox.SelectedIndex == 0)
                {
                    ClassroomTextBox.Visibility = Visibility.Visible;
                }
                else
                {
                    ClassroomTextBox.Visibility = Visibility.Hidden;
                    ClassroomTextBox.Text = string.Empty;
                }
            }
        }

        public class User(int userID, string name, string surname, string login, string role, string classroom)
        {
            public int UserID { get; set; } = userID;
            public string Name { get; set; } = name;
            public string Surname { get; set; } = surname;
            public string Login { get; set; } = login;
            public string Role { get; set; } = role;
            public string Classroom { get; set; } = classroom;
        }
    }
}