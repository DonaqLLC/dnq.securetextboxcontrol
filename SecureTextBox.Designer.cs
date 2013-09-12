namespace DNQ.Controls
{
    partial class SecureTextBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textbox
            // 
            this.textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textbox.Location = new System.Drawing.Point(2, 2);
            this.textbox.Name = "textbox";
            this.textbox.Size = new System.Drawing.Size(257, 13);
            this.textbox.TabIndex = 0;
            this.textbox.UseSystemPasswordChar = true;
            this.textbox.WordWrap = false;
            this.textbox.TextChanged += new System.EventHandler(this.textbox_TextChanged);
            this.textbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
            this.textbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textbox_KeyPress);
            this.textbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyUp);
            this.textbox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textbox_PreviewKeyDown);
            // 
            // SecureTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.textbox);
            this.Name = "SecureTextBox";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(261, 60);
            this.Load += new System.EventHandler(this.SecureTextBox_Load);
            this.BackColorChanged += new System.EventHandler(this.SecureTextBox_BackColorChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textbox;
    }
}
