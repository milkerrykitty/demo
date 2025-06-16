namespace template
{
    partial class ReportForm
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
            this.ReportGrid = new System.Windows.Forms.DataGridView();
            this.startPeriod = new System.Windows.Forms.DateTimePicker();
            this.endPeriod = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TypeBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NameLbl = new System.Windows.Forms.Label();
            this.GenerateBut = new System.Windows.Forms.Button();
            this.ExportBut = new System.Windows.Forms.Button();
            this.TotalLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ReportGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // ReportGrid
            // 
            this.ReportGrid.BackgroundColor = System.Drawing.Color.White;
            this.ReportGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ReportGrid.Location = new System.Drawing.Point(23, 25);
            this.ReportGrid.Name = "ReportGrid";
            this.ReportGrid.Size = new System.Drawing.Size(621, 403);
            this.ReportGrid.TabIndex = 0;
            // 
            // startPeriod
            // 
            this.startPeriod.Location = new System.Drawing.Point(663, 168);
            this.startPeriod.Name = "startPeriod";
            this.startPeriod.Size = new System.Drawing.Size(232, 20);
            this.startPeriod.TabIndex = 1;
            // 
            // endPeriod
            // 
            this.endPeriod.Location = new System.Drawing.Point(663, 222);
            this.endPeriod.Name = "endPeriod";
            this.endPeriod.Size = new System.Drawing.Size(232, 20);
            this.endPeriod.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(660, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 23);
            this.label5.TabIndex = 26;
            this.label5.Text = "Начало периода";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(660, 196);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 23);
            this.label1.TabIndex = 27;
            this.label1.Text = "Конец периода";
            // 
            // TypeBox
            // 
            this.TypeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TypeBox.FormattingEnabled = true;
            this.TypeBox.Location = new System.Drawing.Point(663, 282);
            this.TypeBox.Name = "TypeBox";
            this.TypeBox.Size = new System.Drawing.Size(232, 24);
            this.TypeBox.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(660, 256);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 23);
            this.label2.TabIndex = 29;
            this.label2.Text = "Тип отчета";
            // 
            // NameLbl
            // 
            this.NameLbl.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NameLbl.ForeColor = System.Drawing.Color.DimGray;
            this.NameLbl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.NameLbl.Location = new System.Drawing.Point(658, 9);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(282, 108);
            this.NameLbl.TabIndex = 30;
            this.NameLbl.Text = "Отчет по ";
            // 
            // GenerateBut
            // 
            this.GenerateBut.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.GenerateBut.BackColor = System.Drawing.Color.DodgerBlue;
            this.GenerateBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GenerateBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.GenerateBut.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.GenerateBut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.GenerateBut.Location = new System.Drawing.Point(729, 319);
            this.GenerateBut.Name = "GenerateBut";
            this.GenerateBut.Size = new System.Drawing.Size(144, 45);
            this.GenerateBut.TabIndex = 31;
            this.GenerateBut.Text = "Сформировать";
            this.GenerateBut.UseVisualStyleBackColor = false;
            this.GenerateBut.Click += new System.EventHandler(this.GenerateBut_Click);
            // 
            // ExportBut
            // 
            this.ExportBut.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ExportBut.BackColor = System.Drawing.Color.DodgerBlue;
            this.ExportBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.ExportBut.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ExportBut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ExportBut.Location = new System.Drawing.Point(729, 383);
            this.ExportBut.Name = "ExportBut";
            this.ExportBut.Size = new System.Drawing.Size(144, 45);
            this.ExportBut.TabIndex = 32;
            this.ExportBut.Text = "Экспорт в Excel";
            this.ExportBut.UseVisualStyleBackColor = false;
            this.ExportBut.Click += new System.EventHandler(this.ExportBut_Click);
            // 
            // TotalLbl
            // 
            this.TotalLbl.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.TotalLbl.ForeColor = System.Drawing.Color.DimGray;
            this.TotalLbl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TotalLbl.Location = new System.Drawing.Point(662, 103);
            this.TotalLbl.Name = "TotalLbl";
            this.TotalLbl.Size = new System.Drawing.Size(154, 23);
            this.TotalLbl.TabIndex = 33;
            this.TotalLbl.Text = "Всего:";
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(952, 458);
            this.Controls.Add(this.TotalLbl);
            this.Controls.Add(this.ExportBut);
            this.Controls.Add(this.GenerateBut);
            this.Controls.Add(this.NameLbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TypeBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.endPeriod);
            this.Controls.Add(this.startPeriod);
            this.Controls.Add(this.ReportGrid);
            this.Name = "ReportForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.ReportGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ReportGrid;
        private System.Windows.Forms.DateTimePicker startPeriod;
        private System.Windows.Forms.DateTimePicker endPeriod;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox TypeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label NameLbl;
        private System.Windows.Forms.Button GenerateBut;
        private System.Windows.Forms.Button ExportBut;
        private System.Windows.Forms.Label TotalLbl;
    }
}