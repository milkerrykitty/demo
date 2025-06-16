using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace template
{
    public partial class InformationForm : Form
    {
        private int? customerId = null;
        string connString = "server=localhost;database=demo;uid=root;pwd=root;";
        private string connectionString;

        // Конструктор: принимает строку подключения и (опционально) ID клиента для редактирования
        public InformationForm(string connectionString, int? customerId = null)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            this.customerId = customerId;

            this.Text = customerId == null ? "Добавление клиента" : "Редактирование клиента";
            LoadData();
        }

        private void LoadData()
        {
            if (customerId.HasValue)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        var cmd = new MySqlCommand("SELECT * FROM customers WHERE customerid = @id", connection);
                        cmd.Parameters.AddWithValue("@id", customerId.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtFullName.Text = reader["fullname"].ToString();
                                dtpDateOfBirth.Value = Convert.ToDateTime(reader["dateofbirth"]);
                                txtNotes.Text = reader["notes"].ToString();
                                txtNotes2.Text = reader["notes2"] as string ?? "";
                                txtNotes3.Text = reader["notes3"] as string ?? "";
                                txtNotes4.Text = reader["notes4"] as string ?? "";
                                txtNotes5.Text = reader["notes5"] as string ?? "";
                                txtNotes6.Text = reader["notes6"] as string ?? "";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            DateTime dateOfBirth = dtpDateOfBirth.Value;
            string notes = txtNotes.Text.Trim();
            string notes2 = txtNotes2.Text.Trim();
            string notes3 = txtNotes3.Text.Trim();
            string notes4 = txtNotes4.Text.Trim();
            string notes5 = txtNotes5.Text.Trim();
            string notes6 = txtNotes6.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(notes))
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля.");
                return;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if (customerId == null)
                    {
                        // Добавление нового клиента
                        var cmd = new MySqlCommand(
                            @"INSERT INTO customers 
                                (fullname, dateofbirth, notes, notes2, notes3, notes4, notes5, notes6) 
                              VALUES 
                                (@fullName, @dateOfBirth, @notes, @notes2, @notes3, @notes4, @notes5, @notes6)",
                            connection);
                        cmd.Parameters.AddWithValue("@fullName", fullName);
                        cmd.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                        cmd.Parameters.AddWithValue("@notes", notes);
                        cmd.Parameters.AddWithValue("@notes2", string.IsNullOrEmpty(notes2) ? (object)DBNull.Value : notes2);
                        cmd.Parameters.AddWithValue("@notes3", string.IsNullOrEmpty(notes3) ? (object)DBNull.Value : notes3);
                        cmd.Parameters.AddWithValue("@notes4", string.IsNullOrEmpty(notes4) ? (object)DBNull.Value : notes4);
                        cmd.Parameters.AddWithValue("@notes5", string.IsNullOrEmpty(notes5) ? (object)DBNull.Value : notes5);
                        cmd.Parameters.AddWithValue("@notes6", string.IsNullOrEmpty(notes6) ? (object)DBNull.Value : notes6);

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // Обновление существующего клиента
                        var cmd = new MySqlCommand(
                            "UPDATE customers SET fullname = @fullName, dateofbirth = @dateOfBirth, notes = @notes, notes2 = @notes2 WHERE customerid = @id",
                            connection);
                        cmd.Parameters.AddWithValue("@fullName", fullName);
                        cmd.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                        cmd.Parameters.AddWithValue("@notes", notes);
                        cmd.Parameters.AddWithValue("@notes2", string.IsNullOrEmpty(notes2) ? (object)DBNull.Value : notes2);
                        cmd.Parameters.AddWithValue("@notes3", string.IsNullOrEmpty(notes3) ? (object)DBNull.Value : notes3);
                        cmd.Parameters.AddWithValue("@notes4", string.IsNullOrEmpty(notes4) ? (object)DBNull.Value : notes4);
                        cmd.Parameters.AddWithValue("@notes5", string.IsNullOrEmpty(notes5) ? (object)DBNull.Value : notes5);
                        cmd.Parameters.AddWithValue("@notes6", string.IsNullOrEmpty(notes6) ? (object)DBNull.Value : notes6);
                        cmd.Parameters.AddWithValue("@id", customerId.Value);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Данные успешно сохранены.");
                    DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения: " + ex.Message);
                }
            }
        }

        private void btnShowCustomers_Click(object sender, EventArgs e)
        {
            CustomersForm customersForm = new CustomersForm(connectionString);
            customersForm.Show();
        }
    }
}
