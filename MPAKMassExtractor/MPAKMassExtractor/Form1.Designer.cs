namespace MPAKMassExtractor
{
    partial class Form1
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
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.extractButton = new System.Windows.Forms.Button();
            this.mpkFolderTextBox = new System.Windows.Forms.TextBox();
            this.extractFolderTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "MPK Folder:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(311, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Extract all MPK files within a folder, and place it in another folder.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Extract Folder:";
            // 
            // extractButton
            // 
            this.extractButton.Location = new System.Drawing.Point(281, 90);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(75, 23);
            this.extractButton.TabIndex = 3;
            this.extractButton.Text = "Extract";
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.extractButton_Click);
            // 
            // mpkFolderTextBox
            // 
            this.mpkFolderTextBox.Location = new System.Drawing.Point(102, 40);
            this.mpkFolderTextBox.Name = "mpkFolderTextBox";
            this.mpkFolderTextBox.Size = new System.Drawing.Size(254, 20);
            this.mpkFolderTextBox.TabIndex = 4;
            this.mpkFolderTextBox.Click += new System.EventHandler(this.mpkFolderTextBox_Click);
            // 
            // extractFolderTextbox
            // 
            this.extractFolderTextbox.Location = new System.Drawing.Point(102, 64);
            this.extractFolderTextbox.Name = "extractFolderTextbox";
            this.extractFolderTextbox.Size = new System.Drawing.Size(254, 20);
            this.extractFolderTextbox.TabIndex = 5;
            this.extractFolderTextbox.Click += new System.EventHandler(this.extractFolderTextbox_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 119);
            this.Controls.Add(this.extractFolderTextbox);
            this.Controls.Add(this.mpkFolderTextBox);
            this.Controls.Add(this.extractButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button extractButton;
        private System.Windows.Forms.TextBox mpkFolderTextBox;
        private System.Windows.Forms.TextBox extractFolderTextbox;
    }
}

