using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace template
{
    public partial class MainForm : Form
    {
        string ConnectionString = "server=localhost;database=demo;uid=root;pwd=root;";
        private string userRole;
        public MainForm(string role)
        {
            InitializeComponent();
            userRole = role;
            btnManageUsers.Visible = false;
            if (userRole == "А")
            {
                btnManageUsers.Visible = true;
            }
        }

        private void btnViewInfo_Click(object sender, EventArgs e)
        {
            using (var informationForm = new InformationForm(ConnectionString, null))
            {
                informationForm.ShowDialog();
            }
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            UserForm userForm = new UserForm(); 
            userForm.Show();
        }

        private void btnChangeUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            Authorization authForm = new Authorization();
            authForm.Show();
        }

        private void btnFond_Click(object sender, EventArgs e)
        {
            using (var form = new FondForm(ConnectionString))
            {
                form.ShowDialog();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            using (ActionLogForm form = new ActionLogForm(ConnectionString))
            {
                form.ShowDialog();
            }
        }

        private void btnShifts_Click(object sender, EventArgs e)
        {
            var shiftForm = new ShiftForm();
            shiftForm.Show();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            var orderForm = new OrderForm();
            orderForm.Show();
        }

        private void btnOpenContract_Click(object sender, EventArgs e)
        {
            var contractForm = new ContractForm();
            contractForm.Show();
        }

        private void btnRep_Click(object sender, EventArgs e)
        {
            var reportForm = new ReportForm();
            reportForm.Show();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            var statisticForm = new StatisticForm();
            statisticForm.Show();
        }
    }
}
