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
    public partial class FondForm : Form
    {
        private string connectionString;
        private DataTable originalData;
        public FondForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            LoadData();
            InitializeComboBoxes();
        }
        private void LoadData()
        {
            originalData = new DataTable();

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM fond";
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

        private void InitializeComboBoxes()
        {
            // Установим значения по умолчанию
            if (txtFloor.Items.Count == 0)
            {
                txtFloor.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });
                txtFloor.SelectedIndex = 0;
            }

            if (txtClass.Items.Count == 0)
            {
                txtClass.Items.AddRange(new object[] { "Эконом", "Стандарт", "Премиум", "Люкс" });
                txtClass.SelectedIndex = 0;
            }

            if (cmbRoomCondition.Items.Count == 0)
            {
                cmbRoomCondition.Items.AddRange(new object[] { "Грязная", "Чистая", "Требует уборки" });
                cmbRoomCondition.SelectedIndex = 0;
            }

            if (cmbStatus.Items.Count == 0)
            {
                cmbStatus.Items.AddRange(new object[] { "Заселена", "Свободна" });
                cmbStatus.SelectedIndex = 0;
            }
        }

        private void ApplyFilters()
        {
            if (originalData == null) return;

            string filter = "";

            if (!string.IsNullOrEmpty(txtFloor.Text))
            {
                filter += $"floor LIKE '{txtFloor.Text}'";
            }

            if (!string.IsNullOrEmpty(txtClass.Text))
            {
                if (filter.Length > 0) filter += " AND ";
                filter += $"class LIKE '{txtClass.Text}'";
            }

            if (cmbRoomCondition.SelectedItem != null && !string.IsNullOrEmpty(cmbRoomCondition.Text))
            {
                if (filter.Length > 0) filter += " AND ";
                filter += $"roomcondition = '{cmbRoomCondition.SelectedItem}'";
            }

            if (cmbStatus.SelectedItem != null && !string.IsNullOrEmpty(cmbStatus.Text))
            {
                if (filter.Length > 0) filter += " AND ";
                filter += $"status = '{cmbStatus.SelectedItem}'";
            }

            DataView dv = originalData.DefaultView;
            dv.RowFilter = filter;
            dataGridView.DataSource = dv;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string floor = txtFloor.Text.Trim();
            string roomClass = txtClass.Text.Trim();
            string condition = cmbRoomCondition.Text.Trim();
            string status = cmbStatus.Text.Trim();

            if (string.IsNullOrEmpty(floor) || string.IsNullOrEmpty(roomClass) ||
                string.IsNullOrEmpty(condition) || string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO fond (floor, class, roomcondition, status) VALUES (@floor, @class, @roomcondition, @status)";
                    var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@floor", floor);
                    cmd.Parameters.AddWithValue("@class", roomClass);
                    cmd.Parameters.AddWithValue("@roomcondition", condition);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Номер успешно добавлен.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления: " + ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите номер для редактирования.");
                return;
            }

            int fondId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["fondid"].Value);
            string floor = txtFloor.Text.Trim();
            string roomClass = txtClass.Text.Trim();
            string condition = cmbRoomCondition.Text.Trim();
            string status = cmbStatus.Text.Trim();

            if (string.IsNullOrEmpty(floor) || string.IsNullOrEmpty(roomClass) ||
                string.IsNullOrEmpty(condition) || string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE fond SET floor = @floor, class = @class, roomcondition = @roomcondition, status = @status WHERE fondid = @fondid";
                    var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@floor", floor);
                    cmd.Parameters.AddWithValue("@class", roomClass);
                    cmd.Parameters.AddWithValue("@roomcondition", condition);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@fondid", fondId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Номер успешно обновлён.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите номер для удаления.");
                return;
            }

            int fondId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["fondid"].Value);

            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить этот номер?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "DELETE FROM fond WHERE fondid = @fondid";
                        var cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@fondid", fondId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Номер успешно удалён.");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка удаления: " + ex.Message);
                    }
                }
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                txtFloor.SelectedIndex = -1;
                txtClass.SelectedIndex = -1;
                cmbRoomCondition.SelectedIndex = -1;
                cmbStatus.SelectedIndex = -1;
                dataGridView.DataSource = originalData;
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                txtFloor.Text = row.Cells["floor"].Value?.ToString();
                txtClass.Text = row.Cells["class"].Value?.ToString();
                cmbRoomCondition.Text = row.Cells["roomcondition"].Value?.ToString();
                cmbStatus.Text = row.Cells["status"].Value?.ToString();
            }
        }
    }
}
