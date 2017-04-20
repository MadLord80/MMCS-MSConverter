using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MultiTraConv
{
    public partial class MultiTraConv : Form
    {
        private Int32 _start_form_height = 547;
        private Int32 _start_table_height = 363;
        private int max_async = 1;
        private bool to_stop = false;

        public MultiTraConv()
        {
            InitializeComponent();
            this.MaximumSize = new Size(986,800);
            this.SizeChanged += new EventHandler(this.MultiTraConv_SizeChanged);
            this.progressBar.Minimum = 0;
            this.progressBar.Value = 0;
            this.progressBar.Step = 1;
            this.user_max_async.Text = max_async.ToString();
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
                    this.progressBar.Maximum = mp3_files.Length;
                    this.all_count.Text = mp3_files.Length.ToString();
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
            if (this.FilesTable.Items.Count > 0 && this.oma_path.TextLength > 0)
            {
                foreach (ListViewItem row in this.FilesTable.Items) {
                    row.SubItems[1].Text = "ready";
                }
                this.progressBar.Value = 0;
                max_async = Convert.ToInt32(this.user_max_async.Text);

                this.start_convert.Enabled = false;
                this.stop_convert.Enabled = true;
                
                convert();
                //MessageBox.Show("DONE!");
            }
        }

        private async void convert()
        {
            int cur_max_async = max_async;

            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            ProcessStartInfo startInfo = new ProcessStartInfo(app_dir + "\\TraConv.exe");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            
            foreach (ListViewItem row in this.FilesTable.Items)
            {
                if (to_stop)
                {
                    to_stop = false;
                    break;
                }
                string filename = row.Text;
                string add_path = filename.Remove(0, mp3_path.TextLength);
                add_path = add_path.Remove(add_path.Length - 3, 3) + "oma";
                string new_path = oma_path.Text + add_path;
                if (File.Exists(new_path))
                {
                    row.SubItems[1].Text = "skipped (file exists)";
                    this.progressBar.PerformStep();
                    this.conv_count.Text = this.progressBar.Value.ToString();
                    continue;
                }
                string new_path_quoted = '"' + new_path + '"';
                startInfo.Arguments = '"' + filename + '"' + " --Convert --FileType OMA --BitRate 352000 --Output " + new_path_quoted;

                while (max_async == 0)
                {
                    await Task.Delay(2000);
                }
                row.SubItems[1].Text = "converting...";
                max_async--;
                Process traconv = new Process();
                traconv.StartInfo = startInfo;
                traconv.Start();
                get_result(traconv, row, new_path);
            }
            while (max_async < cur_max_async)
            {
                await Task.Delay(2000);
            }
            Application.DoEvents();
            this.start_convert.Enabled = true;
            this.stop_convert.Enabled = false;
        }

        private async void get_result (Process proc, ListViewItem item, string new_path)
        {
            string p_out = await proc.StandardOutput.ReadToEndAsync();
            if (!p_out.Contains("Location=" + new_path)) {
                item.SubItems[1].Text = "Error: " + proc.StartInfo.Arguments;
            }
            else
            {
                item.SubItems[1].Text = "convered";
                this.progressBar.PerformStep();
                this.conv_count.Text = this.progressBar.Value.ToString();
            }
            max_async++;
        }

        private void stop_convert_Click(object sender, EventArgs e)
        {
            to_stop = true;
        }
    }
}
