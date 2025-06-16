using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;

namespace template
{
    public partial class UserForm : Form
    {
        private string connectionString = "server=localhost;user=root;database=demo;password=root;";
        public UserForm()
        {
            InitializeComponent();
            cmbRole.Items.AddRange(new object[] { "А", "П" });
            if (cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = 0;
            cmbStatus.Items.AddRange(new object[] { "Работает", "Уволен" });
            cmbStatus.SelectedIndex = 0;
            cmbAccStat.Items.AddRange(new object[] { "Активный", "Заблокирован" });
            cmbAccStat.SelectedIndex = 0;
            LoadUsers();
        }
        private void LoadUsers()
        {
            dataGridView.Columns.Clear();
            dataGridView.Rows.Clear();

            // Добавляем колонки
            dataGridView.Columns.Add("userid", "ID");
            dataGridView.Columns.Add("login", "Логин");
            dataGridView.Columns.Add("password", "Пароль");
            dataGridView.Columns.Add("fullname", "ФИО");
            dataGridView.Columns.Add("role", "Роль");
            dataGridView.Columns.Add("activestatus", "Статус аккаунта");
            dataGridView.Columns.Add("status", "Статус работы");
            dataGridView.Columns.Add("last_login", "Последний вход");
            dataGridView.Columns.Add("first_login", "Первый вход");
            dataGridView.Columns.Add("password_changed", "Пароль изменён");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM users";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        dataGridView.Rows.Add(
                            reader["userid"],
                            reader["login"],
                            reader["password"],
                            reader["fullname"],
                            reader["role"],
                            reader["activestatus"],
                            reader["status"],
                            (reader["last_login"] == DBNull.Value ? "Никогда" : Convert.ToDateTime(reader["last_login"]).ToString("yyyy-MM-dd HH:mm")),
                            (reader["first_login"] == DBNull.Value ? "Никогда" : Convert.ToDateTime(reader["first_login"]).ToString("yyyy-MM-dd HH:mm")),
                            (reader["password_changed"] == DBNull.Value ? "Нет" :
                                Convert.ToBoolean(reader["password_changed"]) ? "Да" : "Нет")
                        );
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                }
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Text;
            string role = cmbRole.SelectedItem?.ToString();
            string fullname = txtFullName.Text; 
            string status = cmbStatus.SelectedItem?.ToString();
            string accStatus = cmbAccStat.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            string connectionString = "server=localhost;user=root;database=demo;password=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkUserQuery = "SELECT COUNT(*) FROM users WHERE login = @login";
                    MySqlCommand checkUserCommand = new MySqlCommand(checkUserQuery, connection);
                    checkUserCommand.Parameters.AddWithValue("@login", login);
                    int userCount = Convert.ToInt32(checkUserCommand.ExecuteScalar());

                    if (userCount > 0)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует.");
                        return;
                    }

                    string addUserQuery = "INSERT INTO users (login, password, role, activestatus, fullname, status) " + 
                        "VALUES (@login, @password, @role, @activestatus, @fullname, @status)";
                    MySqlCommand addUserCommand = new MySqlCommand(addUserQuery, connection);
                    addUserCommand.Parameters.AddWithValue("@login", login);
                    addUserCommand.Parameters.AddWithValue("@password", password);
                    addUserCommand.Parameters.AddWithValue("@role", role);
                    addUserCommand.Parameters.AddWithValue("@activestatus", accStatus);
                    addUserCommand.Parameters.AddWithValue("@fullname", fullname); 
                    addUserCommand.Parameters.AddWithValue("@status", status);

                    addUserCommand.ExecuteNonQuery();

                    MessageBox.Show("Пользователь успешно добавлен.");
                    LoadUsers();
                    txtLogin.Clear();
                    txtPassword.Clear();
                    cmbRole.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для редактирования.");
                return;
            }

            int userid = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["userid"].Value);

            // Получаем новые значения из полей формы
            string newLogin = txtLogin.Text;
            string newPassword = txtPassword.Text;
            string newRole = cmbRole.SelectedItem?.ToString();
            string newWorkStatus = cmbStatus.SelectedItem?.ToString();

            // Получаем текущие значения из строки
            string oldLogin = dataGridView.SelectedRows[0].Cells["login"].Value.ToString();
            string oldPassword = dataGridView.SelectedRows[0].Cells["password"].Value.ToString();
            string oldRole = dataGridView.SelectedRows[0].Cells["role"].Value.ToString();
            string oldFullname = dataGridView.SelectedRows[0].Cells["fullname"].Value.ToString();
            string oldAccountStatus = dataGridView.SelectedRows[0].Cells["activestatus"].Value.ToString();
            string oldWorkStatus = dataGridView.SelectedRows[0].Cells["status"].Value.ToString();

            // Если поле не изменено — оставляем старое значение
            string login = string.IsNullOrEmpty(newLogin) ? oldLogin : newLogin;
            string password = string.IsNullOrEmpty(newPassword) ? oldPassword : newPassword;
            string role = newRole ?? oldRole;
            string fullname = txtFullName.Text;
            string accountStatus = oldAccountStatus; 
            string workStatus = newWorkStatus ?? oldWorkStatus;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля.");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string updateUserQuery = "UPDATE users SET login = @login, password = @password, " +
                                             "role = @role, fullname = @fullname, status = @workStatus WHERE userid = @userid";

                    MySqlCommand updateUserCommand = new MySqlCommand(updateUserQuery, connection);
                    updateUserCommand.Parameters.AddWithValue("@login", login);
                    updateUserCommand.Parameters.AddWithValue("@password", password);
                    updateUserCommand.Parameters.AddWithValue("@role", role);
                    updateUserCommand.Parameters.AddWithValue("@fullname", fullname);
                    updateUserCommand.Parameters.AddWithValue("@workStatus", workStatus);
                    updateUserCommand.Parameters.AddWithValue("@userid", userid);
                    updateUserCommand.ExecuteNonQuery();

                    MessageBox.Show("Данные пользователя успешно обновлены.");
                    LoadUsers(); // Перезагружаем данные
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void btnUnblockUser_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для снятия блокировки.");
                return;
            }

            int userid = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["userid"].Value);

            string connectionString = "server=localhost;user=root;database=demo;password=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string unblockUserQuery = "UPDATE users SET activestatus = 'Активный' WHERE userid = @userid";
                    MySqlCommand unblockUserCommand = new MySqlCommand(unblockUserQuery, connection);
                    unblockUserCommand.Parameters.AddWithValue("@userid", userid);
                    unblockUserCommand.ExecuteNonQuery();

                    MessageBox.Show("Пользователь успешно разблокирован.");
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления.");
                return;
            }

            int userid = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["userid"].Value);

            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить этого пользователя?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                string connectionString = "server=localhost;user=root;database=demo;password=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string deleteUserQuery = "DELETE FROM users WHERE userid = @userid";
                        MySqlCommand deleteUserCommand = new MySqlCommand(deleteUserQuery, connection);
                        deleteUserCommand.Parameters.AddWithValue("@userid", userid);
                        deleteUserCommand.ExecuteNonQuery();

                        MessageBox.Show("Пользователь успешно удален.");
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                    }
                }
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                txtLogin.Text = row.Cells["login"].Value?.ToString();
                txtPassword.Text = row.Cells["password"].Value?.ToString();
                txtFullName.Text = row.Cells["fullname"].Value?.ToString();
                cmbRole.SelectedItem = row.Cells["role"].Value?.ToString();

                var statusValue = row.Cells["status"].Value?.ToString();
                cmbStatus.SelectedItem = string.IsNullOrEmpty(statusValue) ? "Работает" : statusValue;
            }
        }
    }
}
