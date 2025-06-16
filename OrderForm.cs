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
    public partial class OrderForm : Form
    {
        private string connectionString = "server=localhost;database=demo;uid=root;pwd=root;";
        public OrderForm()
        {
            InitializeComponent();
            LoadOrders();
            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {
            cmbType.Items.AddRange(new object[] { "Заказ", "Бронь", "Сделка", "ДТП" });
            cmbStatus.Items.AddRange(new object[] { "Новый", "Выполнен", "Отменен" });
            cmbType.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;
            LoadUsers();
            LoadCustomers();
        }

        private void LoadUsers()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT userid, fullname FROM users", conn);
            var reader = cmd.ExecuteReader();
            var users = new Dictionary<int, string>();
            while (reader.Read())
            {
                users.Add(reader.GetInt32("userid"), reader.GetString("fullname"));
            }
            cmbUser.DataSource = new BindingSource(users, null);
            cmbUser.DisplayMember = "Value";
            cmbUser.ValueMember = "Key";
        }

        private void LoadCustomers()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT customerid, fullname FROM customers", conn);
            var reader = cmd.ExecuteReader();
            var customers = new Dictionary<int, string>();
            while (reader.Read())
            {
                customers.Add(reader.GetInt32("customerid"), reader.GetString("fullname"));
            }
            cmbCustomer.DataSource = new BindingSource(customers, null);
            cmbCustomer.DisplayMember = "Value";
            cmbCustomer.ValueMember = "Key";
        }

        private void LoadOrders(string filterType = "")
        {
            var dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT o.orderid, o.object_type, o.object_name, o.description, o.amount, o.status, o.created_at,
                        u.fullname AS user_name, c.fullname AS customer_name
                 FROM orders o
                 LEFT JOIN users u ON o.user_id = u.userid
                 JOIN customers c ON o.customerid = c.customerid";

                if (!string.IsNullOrEmpty(filterType))
                {
                    query += $" WHERE o.object_type = '{filterType}'";
                }

                var adapter = new MySqlDataAdapter(query, conn);
                adapter.Fill(dt);
            }
            dataGridView.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string type = cmbType.SelectedItem?.ToString();
            string name = txtName.Text;
            string desc = txtDescription.Text;
            decimal amount = nudAmount.Value;
            string status = cmbStatus.SelectedItem?.ToString();
            int userId = ((KeyValuePair<int, string>)cmbUser.SelectedItem).Key;
            int customerId = ((KeyValuePair<int, string>)cmbCustomer.SelectedItem).Key;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO orders 
                                (object_type, object_name, description, amount, status, user_id, customerid)
                                 VALUES (@type, @name, @desc, @amount, @status, @user_id, @customerid)";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@customerid", customerId);
                cmd.ExecuteNonQuery();
            }

            LoadOrders();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0) return;
            int orderId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["orderid"].Value);

            string type = cmbType.SelectedItem?.ToString();
            string name = txtName.Text;
            string desc = txtDescription.Text;
            decimal amount = nudAmount.Value;
            string status = cmbStatus.SelectedItem?.ToString();
            int userId = ((KeyValuePair<int, string>)cmbUser.SelectedItem).Key;
            int customerId = ((KeyValuePair<int, string>)cmbCustomer.SelectedItem).Key;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE orders SET 
                                    object_type = @type,
                                    object_name = @name,
                                    description = @desc,
                                    amount = @amount,
                                    status = @status,
                                    user_id = @user_id,
                                    customerid = @customerid
                                    WHERE orderid = @id";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@customerid", customerId);
                cmd.ExecuteNonQuery();
            }

            LoadOrders();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0) return;
            int orderId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["orderid"].Value);

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM orders WHERE orderid = @id";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.ExecuteNonQuery();
            }

            LoadOrders();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadOrders(cmbType.SelectedItem?.ToString());
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0 || dataGridView.SelectedRows[0].IsNewRow) return;

            DataGridViewRow row = dataGridView.SelectedRows[0];
            txtName.Text = row.Cells["object_name"].Value?.ToString() ?? "";
            cmbType.SelectedItem = row.Cells["object_type"].Value?.ToString() ?? "Заказ";
            cmbStatus.SelectedItem = row.Cells["status"].Value?.ToString() ?? "Новый";
            nudAmount.Value = row.Cells["amount"].Value is DBNull ? 0m : Convert.ToDecimal(row.Cells["amount"].Value);
            string userName = row.Cells["user_name"].Value?.ToString() ?? "";
            string customerName = row.Cells["customer_name"].Value?.ToString() ?? "";

            if (!string.IsNullOrEmpty(userName))
            {
                cmbUser.SelectedValue = ((KeyValuePair<int, string>)cmbUser.SelectedItem).Key;
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                cmbCustomer.SelectedValue = ((KeyValuePair<int, string>)cmbCustomer.SelectedItem).Key;
            }
        }
    }
}
