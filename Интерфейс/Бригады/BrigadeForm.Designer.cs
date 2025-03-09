namespace КурсоваяПрог.Интерфейс.Бригады
{
    partial class BrigadeForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddBrigade = new System.Windows.Forms.Button();
            this.btnDeleteBrigade = new System.Windows.Forms.Button();
            this.btnEditBrigade = new System.Windows.Forms.Button();
            this.btnAddWorker = new System.Windows.Forms.Button();
            this.btnDeleteWorker = new System.Windows.Forms.Button();
            this.btnEditWorker = new System.Windows.Forms.Button();
            this.dataGridViewWorkers = new System.Windows.Forms.DataGridView();
            this.listBoxWorkers = new System.Windows.Forms.ListBox();
            this.dataGridViewBrigades = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxBrigade = new System.Windows.Forms.ComboBox();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWorkers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBrigades)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(620, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Бригады";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(513, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Работники";
            // 
            // btnAddBrigade
            // 
            this.btnAddBrigade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddBrigade.Location = new System.Drawing.Point(264, 30);
            this.btnAddBrigade.Name = "btnAddBrigade";
            this.btnAddBrigade.Size = new System.Drawing.Size(159, 59);
            this.btnAddBrigade.TabIndex = 4;
            this.btnAddBrigade.Text = "Добавить бригаду";
            this.btnAddBrigade.UseVisualStyleBackColor = true;
            this.btnAddBrigade.Click += new System.EventHandler(this.btnAddBrigade_Click);
            // 
            // btnDeleteBrigade
            // 
            this.btnDeleteBrigade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDeleteBrigade.Location = new System.Drawing.Point(264, 159);
            this.btnDeleteBrigade.Name = "btnDeleteBrigade";
            this.btnDeleteBrigade.Size = new System.Drawing.Size(159, 59);
            this.btnDeleteBrigade.TabIndex = 5;
            this.btnDeleteBrigade.Text = "Удалить бригаду";
            this.btnDeleteBrigade.UseVisualStyleBackColor = true;
            this.btnDeleteBrigade.Click += new System.EventHandler(this.btnDeleteBrigade_Click);
            // 
            // btnEditBrigade
            // 
            this.btnEditBrigade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnEditBrigade.Location = new System.Drawing.Point(264, 94);
            this.btnEditBrigade.Name = "btnEditBrigade";
            this.btnEditBrigade.Size = new System.Drawing.Size(159, 59);
            this.btnEditBrigade.TabIndex = 5;
            this.btnEditBrigade.Text = "Редактировать бригаду";
            this.btnEditBrigade.UseVisualStyleBackColor = true;
            this.btnEditBrigade.Click += new System.EventHandler(this.btnEditBrigade_Click);
            // 
            // btnAddWorker
            // 
            this.btnAddWorker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddWorker.Location = new System.Drawing.Point(12, 264);
            this.btnAddWorker.Name = "btnAddWorker";
            this.btnAddWorker.Size = new System.Drawing.Size(149, 60);
            this.btnAddWorker.TabIndex = 6;
            this.btnAddWorker.Text = "Добавить работника";
            this.btnAddWorker.UseVisualStyleBackColor = true;
            this.btnAddWorker.Click += new System.EventHandler(this.btnAddWorker_Click);
            // 
            // btnDeleteWorker
            // 
            this.btnDeleteWorker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDeleteWorker.Location = new System.Drawing.Point(12, 395);
            this.btnDeleteWorker.Name = "btnDeleteWorker";
            this.btnDeleteWorker.Size = new System.Drawing.Size(149, 60);
            this.btnDeleteWorker.TabIndex = 7;
            this.btnDeleteWorker.Text = "Удалить работника";
            this.btnDeleteWorker.UseVisualStyleBackColor = true;
            this.btnDeleteWorker.Click += new System.EventHandler(this.btnDeleteWorker_Click);
            // 
            // btnEditWorker
            // 
            this.btnEditWorker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnEditWorker.Location = new System.Drawing.Point(12, 329);
            this.btnEditWorker.Name = "btnEditWorker";
            this.btnEditWorker.Size = new System.Drawing.Size(149, 60);
            this.btnEditWorker.TabIndex = 7;
            this.btnEditWorker.Text = "Редактировать работника";
            this.btnEditWorker.UseVisualStyleBackColor = true;
            this.btnEditWorker.Click += new System.EventHandler(this.btnEditWorker_Click);
            // 
            // dataGridViewWorkers
            // 
            this.dataGridViewWorkers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWorkers.Location = new System.Drawing.Point(289, 259);
            this.dataGridViewWorkers.Name = "dataGridViewWorkers";
            this.dataGridViewWorkers.Size = new System.Drawing.Size(581, 202);
            this.dataGridViewWorkers.TabIndex = 8;
            // 
            // listBoxWorkers
            // 
            this.listBoxWorkers.FormattingEnabled = true;
            this.listBoxWorkers.Location = new System.Drawing.Point(175, 302);
            this.listBoxWorkers.Name = "listBoxWorkers";
            this.listBoxWorkers.Size = new System.Drawing.Size(104, 121);
            this.listBoxWorkers.TabIndex = 9;
            // 
            // dataGridViewBrigades
            // 
            this.dataGridViewBrigades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBrigades.Location = new System.Drawing.Point(442, 41);
            this.dataGridViewBrigades.Name = "dataGridViewBrigades";
            this.dataGridViewBrigades.Size = new System.Drawing.Size(428, 169);
            this.dataGridViewBrigades.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(33, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 24);
            this.label3.TabIndex = 11;
            this.label3.Text = "Выбор обьекта ";
            // 
            // comboBoxBrigade
            // 
            this.comboBoxBrigade.FormattingEnabled = true;
            this.comboBoxBrigade.Location = new System.Drawing.Point(37, 148);
            this.comboBoxBrigade.Name = "comboBoxBrigade";
            this.comboBoxBrigade.Size = new System.Drawing.Size(151, 21);
            this.comboBoxBrigade.TabIndex = 12;
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(37, 85);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(147, 21);
            this.comboBoxProject.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(33, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 24);
            this.label4.TabIndex = 14;
            this.label4.Text = "Выбор бригады";
            // 
            // BrigadeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 478);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxProject);
            this.Controls.Add(this.comboBoxBrigade);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridViewBrigades);
            this.Controls.Add(this.listBoxWorkers);
            this.Controls.Add(this.dataGridViewWorkers);
            this.Controls.Add(this.btnEditWorker);
            this.Controls.Add(this.btnDeleteWorker);
            this.Controls.Add(this.btnAddWorker);
            this.Controls.Add(this.btnEditBrigade);
            this.Controls.Add(this.btnDeleteBrigade);
            this.Controls.Add(this.btnAddBrigade);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BrigadeForm";
            this.Text = "BrigadeForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWorkers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBrigades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddBrigade;
        private System.Windows.Forms.Button btnDeleteBrigade;
        private System.Windows.Forms.Button btnEditBrigade;
        private System.Windows.Forms.Button btnAddWorker;
        private System.Windows.Forms.Button btnDeleteWorker;
        private System.Windows.Forms.Button btnEditWorker;
        private System.Windows.Forms.DataGridView dataGridViewWorkers;
        private System.Windows.Forms.ListBox listBoxWorkers;
        private System.Windows.Forms.DataGridView dataGridViewBrigades;
        private System.Windows.Forms.ComboBox comboBoxProjects;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxBrigade;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Label label4;
    }
}