namespace Droid_Image
{
    partial class MessageDisplay
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
            this._textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _textBox
            // 
            this._textBox.BackColor = System.Drawing.Color.Gray;
            this._textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._textBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._textBox.ForeColor = System.Drawing.Color.Yellow;
            this._textBox.Location = new System.Drawing.Point(0, 0);
            this._textBox.Name = "_textBox";
            this._textBox.ReadOnly = true;
            this._textBox.Size = new System.Drawing.Size(200, 20);
            this._textBox.TabIndex = 0;
            this._textBox.Text = "dfqhdht";
            this._textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._textBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this._textBox_MouseClick);
            // 
            // MessageDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._textBox);
            this.Name = "MessageDisplay";
            this.Size = new System.Drawing.Size(200, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _textBox;
    }
}
