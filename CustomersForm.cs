using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace template
{
    public partial class CustomersForm : Form
    {
        private string connectionString;
        private DataTable originalData;

        public CustomersForm() : this(null) { }

        public CustomersForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            LoadData();
        }

        private void LoadData()
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "server=localhost;user=root;database=demo;password=root;";
            }

            originalData = new DataTable();

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM customers";
                    var adapter = new MySqlDataAdapter(query, connection);
                    adapter.Fill(originalData);
                    dataGridView.DataSource = originalData;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                }
            }
        }
        private void ApplyFilters()
        {
            if (originalData == null) return;

            string filter = "";

            // Фильтр по имени
            if (!string.IsNullOrEmpty(txtSearchName.Text))
            {
                filter += $"fullname LIKE '%{txtSearchName.Text}%'";
            }

            // Фильтр по дате рождения
            if (chkFilterDate.Checked)
            {
                if (filter.Length > 0) filter += " AND ";
                filter += $"dateofbirth >= #{dtpFromDate.Value:yyyy-MM-dd}# AND dateofbirth <= #{dtpToDate.Value:yyyy-MM-dd}#";
            }

            try
            {
                DataView dv = originalData.DefaultView;
                dv.RowFilter = filter;
                dataGridView.DataSource = dv;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка фильтрации: " + ex.Message);
            }
        }

        private void ResetFilters()
        {
            txtSearchName.Clear();
            chkFilterDate.Checked = false;
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            dtpFromDate.Value = DateTime.Now.AddYears(-1);
            dtpToDate.Value = DateTime.Now;
            dataGridView.DataSource = originalData;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetFilters();
        }

        private void chkFilterDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFromDate.Enabled = chkFilterDate.Checked;
            dtpToDate.Enabled = chkFilterDate.Checked;

            if (!chkFilterDate.Checked)
            {
                ApplyFilters();
            }
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataRowView selectedRow = (DataRowView)dataGridView.Rows[e.RowIndex].DataBoundItem;
            DataRow row = selectedRow.Row;

            // Передаём выбранную строку в форму редактирования
            EditCustomerForm editForm = new EditCustomerForm(connectionString, row);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadData(); // Перезагружаем данные после изменения
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для удаления.");
                return;
            }

            int customerId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["customerid"].Value);

            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить этого клиента?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                DeleteCustomer(customerId);
            }
        }
        private void DeleteCustomer(int customerId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM customers WHERE customerid = @id";
                    var cmd = new MySqlCommand(deleteQuery, connection);
                    cmd.Parameters.AddWithValue("@id", customerId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Клиент успешно удален.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                }
            }
        }
    }
}
