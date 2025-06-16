using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace template
{
    public partial class ActionLogForm : Form
    {
        private string connectionString = "server=localhost;database=demo;uid=root;pwd=root;";
        private DataTable originalData;

        public ActionLogForm(string connectionString)
        {
            InitializeComponent();
            LoadUsers();
            LoadActionLogs();
        }

        private void LoadUsers()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand("SELECT login FROM users", connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cmbUser.Items.Add(reader["login"].ToString());
                    }
                }
            }
            cmbUser.SelectedIndex = -1;
        }

        private void LoadActionLogs()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                                u.login AS 'Пользователь',
                                l.action AS 'Действие',
                                l.timestamp AS 'Дата/время'
                        FROM action_log l
                        JOIN users u ON l.user_id = u.userid
                        ORDER BY l.timestamp DESC";

                    var adapter = new MySqlDataAdapter(query, connection);
                    originalData = new DataTable();
                    adapter.Fill(originalData);

                    dataGridView.DataSource = originalData;

                    if (dataGridView.Columns.Count >= 4)
                    {
                        dataGridView.Columns["logid"].HeaderText = "ID";
                        dataGridView.Columns["username"].HeaderText = "Пользователь";
                        dataGridView.Columns["action"].HeaderText = "Действие";
                        dataGridView.Columns["timestamp"].HeaderText = "Дата/время";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки логов: {ex.Message}");
            }
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            LoadActionLogs();
        }

        private void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (originalData == null) return;

            string filter = "";
            if (!string.IsNullOrEmpty(cmbUser.Text))
            {
                filter = $"[Пользователь] = '{cmbUser.Text}'";
            }

            DataView dv = originalData.DefaultView;
            dv.RowFilter = filter;
            dataGridView.DataSource = dv;
        }
    }
}
