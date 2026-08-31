namespace KooliProjekt.WindowsForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            idField = new TextBox();
            titleField = new TextBox();
            yearField = new TextBox();
            authorIdField = new TextBox();
            saveCommand = new Button();
            addCommand = new Button();
            deleteCommand = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(525, 338);
            dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(543, 18);
            label1.Name = "label1";
            label1.Size = new Size(21, 15);
            label1.TabIndex = 1;
            label1.Text = "ID:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(543, 47);
            label2.Name = "label2";
            label2.Size = new Size(33, 15);
            label2.TabIndex = 2;
            label2.Text = "Title:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(543, 76);
            label3.Name = "label3";
            label3.Size = new Size(35, 15);
            label3.TabIndex = 3;
            label3.Text = "Year:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(543, 105);
            label4.Name = "label4";
            label4.Size = new Size(58, 15);
            label4.TabIndex = 4;
            label4.Text = "AuthorId:";
            // 
            // idField
            // 
            idField.Location = new Point(615, 15);
            idField.Name = "idField";
            idField.ReadOnly = true;
            idField.Size = new Size(100, 23);
            idField.TabIndex = 5;
            idField.Text = "0";
            // 
            // titleField
            // 
            titleField.Location = new Point(615, 44);
            titleField.Name = "titleField";
            titleField.Size = new Size(200, 23);
            titleField.TabIndex = 6;
            // 
            // yearField
            // 
            yearField.Location = new Point(615, 73);
            yearField.Name = "yearField";
            yearField.Size = new Size(100, 23);
            yearField.TabIndex = 7;
            // 
            // authorIdField
            // 
            authorIdField.Location = new Point(615, 102);
            authorIdField.Name = "authorIdField";
            authorIdField.Size = new Size(100, 23);
            authorIdField.TabIndex = 8;
            // 
            // saveCommand
            // 
            saveCommand.Location = new Point(615, 149);
            saveCommand.Name = "saveCommand";
            saveCommand.Size = new Size(75, 26);
            saveCommand.TabIndex = 9;
            saveCommand.Text = "Salvesta";
            saveCommand.UseVisualStyleBackColor = true;
            // 
            // addCommand
            // 
            addCommand.Location = new Point(705, 149);
            addCommand.Name = "addCommand";
            addCommand.Size = new Size(75, 26);
            addCommand.TabIndex = 10;
            addCommand.Text = "Lisa uus";
            addCommand.UseVisualStyleBackColor = true;
            // 
            // deleteCommand
            // 
            deleteCommand.Location = new Point(795, 149);
            deleteCommand.Name = "deleteCommand";
            deleteCommand.Size = new Size(75, 26);
            deleteCommand.TabIndex = 11;
            deleteCommand.Text = "Kustuta";
            deleteCommand.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(895, 380);
            Controls.Add(deleteCommand);
            Controls.Add(addCommand);
            Controls.Add(saveCommand);
            Controls.Add(authorIdField);
            Controls.Add(yearField);
            Controls.Add(titleField);
            Controls.Add(idField);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox idField;
        private TextBox titleField;
        private TextBox yearField;
        private TextBox authorIdField;
        private Button saveCommand;
        private Button addCommand;
        private Button deleteCommand;
    }
}
