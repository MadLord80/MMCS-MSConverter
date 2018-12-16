namespace MultiTraConv
{
	partial class Log
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
			this.logBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// logBox
			// 
			this.logBox.BackColor = System.Drawing.Color.Silver;
			this.logBox.Location = new System.Drawing.Point(12, 12);
			this.logBox.Multiline = true;
			this.logBox.Name = "logBox";
			this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.logBox.Size = new System.Drawing.Size(776, 426);
			this.logBox.TabIndex = 0;
			// 
			// Log
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.logBox);
			this.Name = "Log";
			this.Text = "Log";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox logBox;
	}
}