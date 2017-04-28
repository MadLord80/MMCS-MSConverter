namespace MultiTraConv
{
    partial class SettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsPage));
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.user_max_async = new System.Windows.Forms.NumericUpDown();
            this.user_max_bitrate = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.is_debug = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.log_file = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.user_max_async)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Max processes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Max bitrate";
            // 
            // user_max_async
            // 
            this.user_max_async.Location = new System.Drawing.Point(96, 40);
            this.user_max_async.Name = "user_max_async";
            this.user_max_async.Size = new System.Drawing.Size(120, 20);
            this.user_max_async.TabIndex = 17;
            this.user_max_async.ValueChanged += new System.EventHandler(this.user_max_async_ValueChanged);
            // 
            // user_max_bitrate
            // 
            this.user_max_bitrate.FormattingEnabled = true;
            this.user_max_bitrate.Items.AddRange(new object[] {
            "64",
            "96",
            "128",
            "132",
            "256",
            "320",
            "352"});
            this.user_max_bitrate.Location = new System.Drawing.Point(95, 68);
            this.user_max_bitrate.Name = "user_max_bitrate";
            this.user_max_bitrate.Size = new System.Drawing.Size(121, 21);
            this.user_max_bitrate.TabIndex = 18;
            this.user_max_bitrate.SelectedIndexChanged += new System.EventHandler(this.user_max_bitrate_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(141, 190);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // is_debug
            // 
            this.is_debug.AutoSize = true;
            this.is_debug.Location = new System.Drawing.Point(15, 112);
            this.is_debug.Name = "is_debug";
            this.is_debug.Size = new System.Drawing.Size(76, 17);
            this.is_debug.TabIndex = 21;
            this.is_debug.Text = "Enable log";
            this.is_debug.UseVisualStyleBackColor = true;
            this.is_debug.CheckedChanged += new System.EventHandler(this.is_debug_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Log-file name";
            // 
            // log_file
            // 
            this.log_file.Location = new System.Drawing.Point(95, 140);
            this.log_file.Name = "log_file";
            this.log_file.ReadOnly = true;
            this.log_file.Size = new System.Drawing.Size(121, 20);
            this.log_file.TabIndex = 23;
            // 
            // SettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 227);
            this.Controls.Add(this.log_file);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.is_debug);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.user_max_bitrate);
            this.Controls.Add(this.user_max_async);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsPage";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.user_max_async)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown user_max_async;
        private System.Windows.Forms.ComboBox user_max_bitrate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox is_debug;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox log_file;
    }
}