namespace UitRoom
{
    partial class Main
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
            this.UserNamebt = new System.Windows.Forms.Button();
            this.logout = new System.Windows.Forms.Button();
            this.data = new System.Windows.Forms.DataGridView();
            this.book = new System.Windows.Forms.Button();
            this.mainbtx = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.data)).BeginInit();
            this.SuspendLayout();
            // 
            // UserNamebt
            // 
            this.UserNamebt.Location = new System.Drawing.Point(12, 12);
            this.UserNamebt.Name = "UserNamebt";
            this.UserNamebt.Size = new System.Drawing.Size(137, 50);
            this.UserNamebt.TabIndex = 3;
            this.UserNamebt.Text = " ";
            this.UserNamebt.UseVisualStyleBackColor = true;
            this.UserNamebt.Click += new System.EventHandler(this.UserNamebt_Click);
            // 
            // logout
            // 
            this.logout.Location = new System.Drawing.Point(448, 12);
            this.logout.Name = "logout";
            this.logout.Size = new System.Drawing.Size(150, 50);
            this.logout.TabIndex = 4;
            this.logout.Text = "Đăng Xuất ";
            this.logout.UseVisualStyleBackColor = true;
            this.logout.Click += new System.EventHandler(this.logout_Click);
            // 
            // data
            // 
            this.data.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data.Location = new System.Drawing.Point(12, 77);
            this.data.Name = "data";
            this.data.ReadOnly = true;
            this.data.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.data.Size = new System.Drawing.Size(586, 361);
            this.data.TabIndex = 7;
            this.data.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_CellClick);
            // 
            // book
            // 
            this.book.Location = new System.Drawing.Point(298, 12);
            this.book.Name = "book";
            this.book.Size = new System.Drawing.Size(144, 50);
            this.book.TabIndex = 8;
            this.book.Text = "Return";
            this.book.UseVisualStyleBackColor = true;
            this.book.Click += new System.EventHandler(this.button2_Click);
            // 
            // mainbtx
            // 
            this.mainbtx.Location = new System.Drawing.Point(155, 12);
            this.mainbtx.Name = "mainbtx";
            this.mainbtx.Size = new System.Drawing.Size(137, 50);
            this.mainbtx.TabIndex = 9;
            this.mainbtx.Text = "Main";
            this.mainbtx.UseVisualStyleBackColor = true;
            this.mainbtx.Click += new System.EventHandler(this.mainbtx_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 450);
            this.Controls.Add(this.mainbtx);
            this.Controls.Add(this.book);
            this.Controls.Add(this.data);
            this.Controls.Add(this.logout);
            this.Controls.Add(this.UserNamebt);
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.data)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button UserNamebt;
        private System.Windows.Forms.Button logout;
        private System.Windows.Forms.DataGridView data;
        private System.Windows.Forms.Button book;
        private System.Windows.Forms.Button mainbtx;
    }
}