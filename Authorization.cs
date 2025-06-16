using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace template
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
        }
        private int loginAttempts = 0;
        private const int MaxLoginAttempts = 3;
        private const int InactiveDaysThreshold = 30;
        string ConnectionString = "server=localhost;database=demo;uid=root;pwd=root;";


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
                return;
            }

            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT password, activestatus, role, 
                                    last_login, first_login, password_changed, userid
                                    FROM users WHERE login = @login";
                    var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@login", txtLogin.Text);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Пользователь не найден");
                            return;
                        }

                        reader.Read();
                        string correctPassword = reader.GetString("password");
                        bool isBlocked = reader.GetString("activestatus") == "Заблокирован";
                        string userRole = reader.GetString("role");
                        DateTime? lastLogin = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);
                        bool passwordChanged = reader.GetBoolean("password_changed");
                        int userId = reader.GetInt32("userid");

                        if (isBlocked)
                        {
                            MessageBox.Show("Ваш аккаунт заблокирован. Обратитесь к администратору.");
                            return;
                        }

                        // Проверка неактивности
                        if (lastLogin.HasValue &&
                            (DateTime.Now - lastLogin.Value).TotalDays > InactiveDaysThreshold)
                        {
                            BlockUser(txtLogin.Text);
                            MessageBox.Show("Ваш аккаунт заблокирован из-за неактивности.");
                            return;
                        }

                        if (txtPassword.Text == correctPassword)
                        {
                            // Логируем успешный вход
                            LogAction(userId, "Вход в систему", "System", 0);

                            // Обновляем время последнего входа
                            UpdateLastLogin(txtLogin.Text);

                            if (!passwordChanged)
                            {
                                // Первый вход - требование смены пароля
                                var changePassForm = new ChangePasswordForm(txtLogin.Text);
                                if (changePassForm.ShowDialog() != DialogResult.OK)
                                {
                                    return;
                                }
                            }

                            MessageBox.Show("Вы успешно авторизовались");
                            MainForm mainForm = new MainForm(userRole);
                            mainForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            loginAttempts++;
                            if (loginAttempts >= MaxLoginAttempts)
                            {
                                BlockUser(txtLogin.Text);
                                // Логируем блокировку из-за неудачных попыток входа
                                LogAction(userId, "Блокировка аккаунта (превышены попытки входа)", "User", userId);
                                MessageBox.Show("Вы заблокированы. Обратитесь к администратору.");
                            }
                            else
                            {
                                MessageBox.Show($"Неверный пароль. Осталось попыток: {MaxLoginAttempts - loginAttempts}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void LogAction(int userId, string action, string objectType, int objectId)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO action_log (user_id, action) 
                                   VALUES (@userId, @action)";
                    var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    MySqlParameter mySqlParameter = cmd.Parameters.AddWithValue("@action", action);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Не прерываем работу приложения при ошибке логирования
                Console.WriteLine($"Ошибка логирования: {ex.Message}");
            }
        }

        private void UpdateLastLogin(string login)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = @"UPDATE users 
                               SET last_login = NOW(),
                                   first_login = IFNULL(first_login, NOW())
                               WHERE login = @login";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.ExecuteNonQuery();
            }
        }

        private void BlockUser(string login)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE users SET activestatus = 'Заблокирован' WHERE login = @login";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
