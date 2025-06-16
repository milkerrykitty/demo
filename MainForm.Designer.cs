namespace template
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnViewInfo = new System.Windows.Forms.Button();
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.btnChangeUser = new System.Windows.Forms.Button();
            this.btnFond = new System.Windows.Forms.Button();
            this.btnAct = new System.Windows.Forms.Button();
            this.btnShifts = new System.Windows.Forms.Button();
            this.btnOrders = new System.Windows.Forms.Button();
            this.btnOpenContract = new System.Windows.Forms.Button();
            this.btnRep = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnViewInfo
            // 
            this.btnViewInfo.Location = new System.Drawing.Point(310, 27);
            this.btnViewInfo.Name = "btnViewInfo";
            this.btnViewInfo.Size = new System.Drawing.Size(178, 75);
            this.btnViewInfo.TabIndex = 0;
            this.btnViewInfo.Text = "Посмотреть какую то инфу о клиенте";
            this.btnViewInfo.UseVisualStyleBackColor = true;
            this.btnViewInfo.Click += new System.EventHandler(this.btnViewInfo_Click);
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.Location = new System.Drawing.Point(310, 108);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(178, 45);
            this.btnManageUsers.TabIndex = 1;
            this.btnManageUsers.Text = "Пользователи";
            this.btnManageUsers.UseVisualStyleBackColor = true;
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click);
            // 
            // btnChangeUser
            // 
            this.btnChangeUser.Location = new System.Drawing.Point(310, 321);
            this.btnChangeUser.Name = "btnChangeUser";
            this.btnChangeUser.Size = new System.Drawing.Size(178, 51);
            this.btnChangeUser.TabIndex = 2;
            this.btnChangeUser.Text = "Сменить пользователя";
            this.btnChangeUser.UseVisualStyleBackColor = true;
            this.btnChangeUser.Click += new System.EventHandler(this.btnChangeUser_Click);
            // 
            // btnFond
            // 
            this.btnFond.Location = new System.Drawing.Point(310, 159);
            this.btnFond.Name = "btnFond";
            this.btnFond.Size = new System.Drawing.Size(178, 46);
            this.btnFond.TabIndex = 3;
            this.btnFond.Text = "Номерной фонд";
            this.btnFond.UseVisualStyleBackColor = true;
            this.btnFond.Click += new System.EventHandler(this.btnFond_Click);
            // 
            // btnAct
            // 
            this.btnAct.Location = new System.Drawing.Point(310, 211);
            this.btnAct.Name = "btnAct";
            this.btnAct.Size = new System.Drawing.Size(178, 46);
            this.btnAct.TabIndex = 4;
            this.btnAct.Text = "Журнал активности";
            this.btnAct.UseVisualStyleBackColor = true;
            this.btnAct.Click += new System.EventHandler(this.btnAct_Click);
            // 
            // btnShifts
            // 
            this.btnShifts.Location = new System.Drawing.Point(310, 263);
            this.btnShifts.Name = "btnShifts";
            this.btnShifts.Size = new System.Drawing.Size(178, 46);
            this.btnShifts.TabIndex = 5;
            this.btnShifts.Text = "Смены";
            this.btnShifts.UseVisualStyleBackColor = true;
            this.btnShifts.Click += new System.EventHandler(this.btnShifts_Click);
            // 
            // btnOrders
            // 
            this.btnOrders.Location = new System.Drawing.Point(494, 163);
            this.btnOrders.Name = "btnOrders";
            this.btnOrders.Size = new System.Drawing.Size(72, 146);
            this.btnOrders.TabIndex = 6;
            this.btnOrders.Text = "Операции";
            this.btnOrders.UseVisualStyleBackColor = true;
            this.btnOrders.Click += new System.EventHandler(this.btnOrders_Click);
            // 
            // btnOpenContract
            // 
            this.btnOpenContract.Location = new System.Drawing.Point(232, 161);
            this.btnOpenContract.Name = "btnOpenContract";
            this.btnOpenContract.Size = new System.Drawing.Size(72, 146);
            this.btnOpenContract.TabIndex = 7;
            this.btnOpenContract.Text = "Чек";
            this.btnOpenContract.UseVisualStyleBackColor = true;
            this.btnOpenContract.Click += new System.EventHandler(this.btnOpenContract_Click);
            // 
            // btnRep
            // 
            this.btnRep.Location = new System.Drawing.Point(494, 27);
            this.btnRep.Name = "btnRep";
            this.btnRep.Size = new System.Drawing.Size(72, 130);
            this.btnRep.TabIndex = 8;
            this.btnRep.Text = "Отчет";
            this.btnRep.UseVisualStyleBackColor = true;
            this.btnRep.Click += new System.EventHandler(this.btnRep_Click);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(232, 27);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(72, 130);
            this.btnReport.TabIndex = 9;
            this.btnReport.Text = "Статистика";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnRep);
            this.Controls.Add(this.btnOpenContract);
            this.Controls.Add(this.btnOrders);
            this.Controls.Add(this.btnShifts);
            this.Controls.Add(this.btnAct);
            this.Controls.Add(this.btnFond);
            this.Controls.Add(this.btnChangeUser);
            this.Controls.Add(this.btnManageUsers);
            this.Controls.Add(this.btnViewInfo);
            this.Name = "MainForm";
            this.Text = "Главное меню";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnViewInfo;
        private System.Windows.Forms.Button btnManageUsers;
        private System.Windows.Forms.Button btnChangeUser;
        private System.Windows.Forms.Button btnFond;
        private System.Windows.Forms.Button btnAct;
        private System.Windows.Forms.Button btnShifts;
        private System.Windows.Forms.Button btnOrders;
        private System.Windows.Forms.Button btnOpenContract;
        private System.Windows.Forms.Button btnRep;
        private System.Windows.Forms.Button btnReport;
    }
}