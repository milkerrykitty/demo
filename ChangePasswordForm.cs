using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace template
{
    public partial class ChangePasswordForm : Form
    {
        private string login;
        private string connectionString = "server=localhost;database=demo;uid=root;pwd=root;";

        public ChangePasswordForm()
        {
            InitializeComponent();
        }
        public ChangePasswordForm(string login) : this()
        {
            this.login = login;
            this.Text = $"Смена пароля для пользователя: {login}";
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrent.Text) ||
                string.IsNullOrEmpty(txtNew.Text) ||
                string.IsNullOrEmpty(txtConfirm.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            if (txtNew.Text != txtConfirm.Text)
            {
                MessageBox.Show("Новый пароль и подтверждение не совпадают");
                return;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Проверка текущего пароля
                    string checkQuery = "SELECT password FROM users WHERE login = @login";
                    var checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@login", login);
                    string correctPassword = checkCmd.ExecuteScalar()?.ToString();

                    if (correctPassword != txtCurrent.Text)
                    {
                        MessageBox.Show("Текущий пароль введен неверно");
                        return;
                    }

                    // Обновление пароля
                    string updateQuery = @"UPDATE users 
                                         SET password = @newPassword, 
                                             password_changed = TRUE,
                                             first_login = NOW()
                                         WHERE login = @login";
                    var updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@newPassword", txtNew.Text);
                    updateCmd.Parameters.AddWithValue("@login", login);
                    updateCmd.ExecuteNonQuery();

                    MessageBox.Show("Пароль успешно изменен");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
