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
    public partial class EditCustomerForm : Form
    {
        private string connectionString;
        private DataRow dataRow;
        public EditCustomerForm(string connectionString, DataRow dataRow)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            this.dataRow = dataRow;

            if (dataRow != null)
            {
                FillFieldsFromDataRow(dataRow);
            }
            else
            {
                dtpDateOfBirth.Value = DateTime.Now.AddYears(-20); // значение по умолчанию
            }
        }

        private void FillFieldsFromDataRow(DataRow row)
        {
            if (row == null) return;

            txtFullName.Text = row["fullname"]?.ToString() ?? "";
            dtpDateOfBirth.Value = row.Table.Columns.Contains("dateofbirth") && !row.IsNull("dateofbirth")
                ? Convert.ToDateTime(row["dateofbirth"])
                : DateTime.Now.AddYears(-20);

            txtNotes.Text = row["notes"]?.ToString() ?? "";
            txtNotes2.Text = row["notes2"]?.ToString() ?? "";
            txtNotes3.Text = row["notes3"]?.ToString() ?? "";
            txtNotes4.Text = row["notes4"]?.ToString() ?? "";
            txtNotes5.Text = row["notes5"]?.ToString() ?? "";
            txtNotes6.Text = row["notes6"]?.ToString() ?? "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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

                    if (dataRow == null)
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
                        // Редактирование существующего клиента
                        int customerId = Convert.ToInt32(dataRow["customerid"]);

                        var cmd = new MySqlCommand(
                            @"UPDATE customers SET 
                        fullname = @fullName, 
                        dateofbirth = @dateOfBirth,
                        notes = @notes,
                        notes2 = @notes2,
                        notes3 = @notes3,
                        notes4 = @notes4,
                        notes5 = @notes5,
                        notes6 = @notes6
                      WHERE customerid = @id",
                            connection);

                        cmd.Parameters.AddWithValue("@fullName", fullName);
                        cmd.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                        cmd.Parameters.AddWithValue("@notes", notes);
                        cmd.Parameters.AddWithValue("@notes2", string.IsNullOrEmpty(notes2) ? (object)DBNull.Value : notes2);
                        cmd.Parameters.AddWithValue("@notes3", string.IsNullOrEmpty(notes3) ? (object)DBNull.Value : notes3);
                        cmd.Parameters.AddWithValue("@notes4", string.IsNullOrEmpty(notes4) ? (object)DBNull.Value : notes4);
                        cmd.Parameters.AddWithValue("@notes5", string.IsNullOrEmpty(notes5) ? (object)DBNull.Value : notes5);
                        cmd.Parameters.AddWithValue("@notes6", string.IsNullOrEmpty(notes6) ? (object)DBNull.Value : notes6);
                        cmd.Parameters.AddWithValue("@id", customerId);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Данные успешно сохранены.");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения: " + ex.Message);
                }
            }
        }
    }
}
