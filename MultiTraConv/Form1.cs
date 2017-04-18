using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MultiTraConv
{
    public partial class MultiTraConv : Form
    {
        private Int32 _start_form_height = 547;
        private Int32 _start_table_height = 363;
        //private FileInfo[] mp3_files;

        public MultiTraConv()
        {
            InitializeComponent();
            this.MaximumSize = new Size(986,800);
            this.SizeChanged += new EventHandler(this.MultiTraConv_SizeChanged);
        }

        private void MultiTraConv_SizeChanged(Object sender, EventArgs e)
        {
            var new_table_height = (this.Height > _start_form_height)
                ? this.Height - _start_form_height
                : _start_form_height - this.Height;
            this.FilesTable.Size = new Size(this.FilesTable.Width, 
                (this.Height > _start_form_height) 
                ? _start_table_height + new_table_height 
                : _start_table_height - new_table_height
            );
        }

        private void mp3dir_button_Click(object sender, EventArgs e)
        {
            if (this.mp3dir_dialog.ShowDialog() == DialogResult.OK)
            {
string temp_dir = "D:\\temp\\Мои документы\\Музыка с дисков";
this.mp3dir_dialog.SelectedPath = temp_dir;

                var mp3_dir = new DirectoryInfo(this.mp3dir_dialog.SelectedPath);
                FileInfo[] mp3_files = mp3_dir.GetFiles("*.mp3", SearchOption.AllDirectories);
                if (mp3_files.Length > 0)
                {
                    this.all_count.Text = mp3_files.Length.ToString();
                    //this.conv_count.Text = mp3_files.Length.ToString();
                    this.mp3_path.Text = this.mp3dir_dialog.SelectedPath;
                    fill_files_table(mp3_files);
                }
                else
                {
                    MessageBox.Show("No MP3 files in directory!");
                }
            }
        }

        private void omadir_button_Click(object sender, EventArgs e)
        {
            if (this.omadir_dialog.ShowDialog() == DialogResult.OK)
            {
string temp_dir = "D:\\tmp";
this.omadir_dialog.SelectedPath = temp_dir;

                var oma_dir = new DirectoryInfo(this.omadir_dialog.SelectedPath);
                if (oma_dir.GetFiles("*").Length > 0)
                {
                    MessageBox.Show("OMA directory is not empty!");
                }
                this.oma_path.Text = this.omadir_dialog.SelectedPath;
            }
        }

        private void fill_files_table(FileInfo[] mp3_files)
        {
            this.FilesTable.Items.Clear();
            foreach (FileInfo file in mp3_files)
            {
                ListViewItem row = new ListViewItem(file.FullName);
                row.SubItems.Add("ready");
                this.FilesTable.Items.Add(row);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.FilesTable.Items.Count < 1) return;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);

            Process traconv = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo(app_dir + "\\TraConv.exe");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            int conv_cnt = 0;
            foreach (ListViewItem row in this.FilesTable.Items)
            {
                string filename = row.Text;
                string add_path = filename.Remove(0, mp3_path.TextLength);
                add_path = add_path.Remove(add_path.Length - 3, 3) + "oma";
                string new_path = oma_path.Text + add_path;
                if (File.Exists(new_path))
                {
                    row.SubItems[1].Text = "skipped (file exists)";
                    this.FilesTable.Refresh();
                    conv_cnt++;
                    this.conv_count.Text = conv_cnt.ToString();
                    this.conv_count.Refresh();
                    continue;
                }
                string new_path_quoted = '"' + new_path + '"';
                row.SubItems[1].Text = "converting...";
                this.FilesTable.Refresh();
                startInfo.Arguments = '"' + filename + '"' + " --Convert --FileType OMA --BitRate 352000 --Output " + new_path_quoted;

                traconv.StartInfo = startInfo;
                traconv.Start();
                string t_out = traconv.StandardOutput.ReadToEnd();
                traconv.WaitForExit();
                if (! t_out.Contains("Location=" + new_path))
                {
                    row.SubItems[1].Text = "Error: " + startInfo.Arguments;
                    this.FilesTable.Refresh();
                    continue;
                }
                row.SubItems[1].Text = "convered";
                this.FilesTable.Refresh();
                conv_cnt++;
                this.conv_count.Text = conv_cnt.ToString();
                this.conv_count.Refresh();
            }            
        }

        private async void get_result (Process proc, ListViewItem item)
        {
            string p_out = await proc.StandardOutput.ReadToEndAsync();
        }
    }
}
