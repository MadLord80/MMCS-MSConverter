namespace MultiTraConv
{
    partial class MultiTraConv
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
            this.components = new System.ComponentModel.Container();
            this.mp3dir_dialog = new System.Windows.Forms.FolderBrowserDialog();
            this.mp3dir_button = new System.Windows.Forms.Button();
            this.mp3_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.oma_path = new System.Windows.Forms.TextBox();
            this.omadir_button = new System.Windows.Forms.Button();
            this.omadir_dialog = new System.Windows.Forms.FolderBrowserDialog();
            this.FilesTable = new System.Windows.Forms.ListView();
            this.filename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.filestatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.all_count = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.conv_count = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBarBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.user_max_async = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // mp3dir_dialog
            // 
            this.mp3dir_dialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.mp3dir_dialog.ShowNewFolderButton = false;
            // 
            // mp3dir_button
            // 
            this.mp3dir_button.Location = new System.Drawing.Point(926, 14);
            this.mp3dir_button.Name = "mp3dir_button";
            this.mp3dir_button.Size = new System.Drawing.Size(24, 23);
            this.mp3dir_button.TabIndex = 0;
            this.mp3dir_button.Text = "...";
            this.mp3dir_button.UseVisualStyleBackColor = true;
            this.mp3dir_button.Click += new System.EventHandler(this.mp3dir_button_Click);
            // 
            // mp3_path
            // 
            this.mp3_path.Location = new System.Drawing.Point(141, 17);
            this.mp3_path.Name = "mp3_path";
            this.mp3_path.ReadOnly = true;
            this.mp3_path.Size = new System.Drawing.Size(779, 20);
            this.mp3_path.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "MP3 directory (input)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "OMA directory (output)";
            // 
            // oma_path
            // 
            this.oma_path.Location = new System.Drawing.Point(141, 50);
            this.oma_path.Name = "oma_path";
            this.oma_path.ReadOnly = true;
            this.oma_path.Size = new System.Drawing.Size(779, 20);
            this.oma_path.TabIndex = 4;
            // 
            // omadir_button
            // 
            this.omadir_button.Location = new System.Drawing.Point(926, 48);
            this.omadir_button.Name = "omadir_button";
            this.omadir_button.Size = new System.Drawing.Size(24, 23);
            this.omadir_button.TabIndex = 5;
            this.omadir_button.Text = "...";
            this.omadir_button.UseVisualStyleBackColor = true;
            this.omadir_button.Click += new System.EventHandler(this.omadir_button_Click);
            // 
            // omadir_dialog
            // 
            this.omadir_dialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // FilesTable
            // 
            this.FilesTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.filename,
            this.filestatus});
            this.FilesTable.GridLines = true;
            this.FilesTable.Location = new System.Drawing.Point(15, 134);
            this.FilesTable.Name = "FilesTable";
            this.FilesTable.Size = new System.Drawing.Size(935, 363);
            this.FilesTable.TabIndex = 6;
            this.FilesTable.UseCompatibleStateImageBehavior = false;
            this.FilesTable.View = System.Windows.Forms.View.Details;
            // 
            // filename
            // 
            this.filename.Text = "File";
            this.filename.Width = 746;
            // 
            // filestatus
            // 
            this.filestatus.Text = "Status";
            this.filestatus.Width = 185;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Process summary: ";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(113, 115);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(837, 13);
            this.progressBar.TabIndex = 8;
            this.progressBar.Tag = "";
            // 
            // all_count
            // 
            this.all_count.AutoSize = true;
            this.all_count.Location = new System.Drawing.Point(451, 96);
            this.all_count.Name = "all_count";
            this.all_count.Size = new System.Drawing.Size(13, 13);
            this.all_count.TabIndex = 9;
            this.all_count.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(483, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "/";
            // 
            // conv_count
            // 
            this.conv_count.AutoSize = true;
            this.conv_count.Location = new System.Drawing.Point(492, 96);
            this.conv_count.Name = "conv_count";
            this.conv_count.Size = new System.Drawing.Size(13, 13);
            this.conv_count.TabIndex = 11;
            this.conv_count.Text = "0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(833, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 32);
            this.button1.TabIndex = 12;
            this.button1.Text = "Convert";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBarBindingSource
            // 
            this.progressBarBindingSource.DataSource = typeof(System.Windows.Forms.ProgressBar);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Max processes";
            // 
            // user_max_async
            // 
            this.user_max_async.Location = new System.Drawing.Point(141, 80);
            this.user_max_async.Name = "user_max_async";
            this.user_max_async.Size = new System.Drawing.Size(100, 20);
            this.user_max_async.TabIndex = 14;
            // 
            // MultiTraConv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(970, 509);
            this.Controls.Add(this.user_max_async);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.conv_count);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.all_count);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FilesTable);
            this.Controls.Add(this.omadir_button);
            this.Controls.Add(this.oma_path);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mp3_path);
            this.Controls.Add(this.mp3dir_button);
            this.MinimumSize = new System.Drawing.Size(986, 547);
            this.Name = "MultiTraConv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MultiTraConv";
            ((System.ComponentModel.ISupportInitialize)(this.progressBarBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog mp3dir_dialog;
        private System.Windows.Forms.Button mp3dir_button;
        private System.Windows.Forms.TextBox mp3_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox oma_path;
        private System.Windows.Forms.Button omadir_button;
        private System.Windows.Forms.FolderBrowserDialog omadir_dialog;
        private System.Windows.Forms.ListView FilesTable;
        private System.Windows.Forms.ColumnHeader filename;
        private System.Windows.Forms.ColumnHeader filestatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label all_count;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label conv_count;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource progressBarBindingSource;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox user_max_async;
    }
}

