namespace DatabaseTesting
{
    partial class DataBaseForm
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
            this.btn_OpenConnection = new System.Windows.Forms.Button();
            this.btn_Insert = new System.Windows.Forms.Button();
            this.btn_Read = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_OpenConnection
            // 
            this.btn_OpenConnection.Location = new System.Drawing.Point(12, 12);
            this.btn_OpenConnection.Name = "btn_OpenConnection";
            this.btn_OpenConnection.Size = new System.Drawing.Size(140, 40);
            this.btn_OpenConnection.TabIndex = 0;
            this.btn_OpenConnection.Text = "Open Connection";
            this.btn_OpenConnection.UseVisualStyleBackColor = true;
            this.btn_OpenConnection.Click += new System.EventHandler(this.btn_OpenConnection_Click);
            // 
            // btn_Insert
            // 
            this.btn_Insert.Location = new System.Drawing.Point(12, 58);
            this.btn_Insert.Name = "btn_Insert";
            this.btn_Insert.Size = new System.Drawing.Size(140, 40);
            this.btn_Insert.TabIndex = 0;
            this.btn_Insert.Text = "Insert";
            this.btn_Insert.UseVisualStyleBackColor = true;
            this.btn_Insert.Click += new System.EventHandler(this.btn_Insert_Click);
            // 
            // btn_Read
            // 
            this.btn_Read.Location = new System.Drawing.Point(12, 104);
            this.btn_Read.Name = "btn_Read";
            this.btn_Read.Size = new System.Drawing.Size(140, 40);
            this.btn_Read.TabIndex = 0;
            this.btn_Read.Text = "Read";
            this.btn_Read.UseVisualStyleBackColor = true;
            // 
            // output
            // 
            this.output.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.output.Location = new System.Drawing.Point(0, 230);
            this.output.Multiline = true;
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(784, 231);
            this.output.TabIndex = 1;
            // 
            // DataBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.output);
            this.Controls.Add(this.btn_Read);
            this.Controls.Add(this.btn_Insert);
            this.Controls.Add(this.btn_OpenConnection);
            this.Name = "DataBaseForm";
            this.Text = "DatabaseForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_OpenConnection;
        private System.Windows.Forms.Button btn_Insert;
        private System.Windows.Forms.Button btn_Read;
        private System.Windows.Forms.TextBox output;
    }
}

