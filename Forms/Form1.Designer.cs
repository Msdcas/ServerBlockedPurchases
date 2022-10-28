namespace ServerBlockedPurchases
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainGrid = new System.Windows.Forms.DataGridView();
            this.Sql_row_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Инициатор = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.comboBoxIpList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // mainGrid
            // 
            this.mainGrid.AllowUserToAddRows = false;
            this.mainGrid.AllowUserToDeleteRows = false;
            this.mainGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mainGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sql_row_id,
            this.Column2,
            this.Инициатор,
            this.Column3,
            this.Column1});
            this.mainGrid.Location = new System.Drawing.Point(9, 141);
            this.mainGrid.Name = "mainGrid";
            this.mainGrid.ReadOnly = true;
            this.mainGrid.RowHeadersVisible = false;
            this.mainGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mainGrid.Size = new System.Drawing.Size(585, 181);
            this.mainGrid.TabIndex = 0;
            // 
            // Sql_row_id
            // 
            this.Sql_row_id.HeaderText = "SQL row id";
            this.Sql_row_id.Name = "Sql_row_id";
            this.Sql_row_id.ReadOnly = true;
            this.Sql_row_id.Width = 90;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Время начала блокировки";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // Инициатор
            // 
            this.Инициатор.HeaderText = "Инициатор";
            this.Инициатор.Name = "Инициатор";
            this.Инициатор.ReadOnly = true;
            this.Инициатор.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Отдел";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Host";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 90;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(9, 48);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(350, 87);
            this.richTextBoxLog.TabIndex = 6;
            this.richTextBoxLog.Text = "";
            // 
            // comboBoxIpList
            // 
            this.comboBoxIpList.FormattingEnabled = true;
            this.comboBoxIpList.Location = new System.Drawing.Point(365, 6);
            this.comboBoxIpList.Name = "comboBoxIpList";
            this.comboBoxIpList.Size = new System.Drawing.Size(226, 21);
            this.comboBoxIpList.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Укажите IP адрес, на котором будет прослушиваться порт 9690";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(446, 91);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 44);
            this.button1.TabIndex = 9;
            this.button1.Text = "S T A R T";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 327);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxIpList);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.mainGrid);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(619, 4096);
            this.MinimumSize = new System.Drawing.Size(619, 177);
            this.Name = "Form1";
            this.Text = "Сервер учета блокированных заявок";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView mainGrid;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sql_row_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Инициатор;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.ComboBox comboBoxIpList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}

