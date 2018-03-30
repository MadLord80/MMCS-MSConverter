using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MultiTraConv
{
	public partial class MultiTraConv : Form
	{
		private Int32 _start_form_height = 547;
		private Int32 _start_table_height = 363;
		private bool to_stop = false;
		private FileStream log;
		private string traconv_path = Path.GetDirectoryName(Application.ExecutablePath) + "\\TraConv.exe";
		private ProcessStartInfo startInfo;
		private DirectoryInfo root_dir;
		private Dictionary<string, bool[]> converted_dirs = new Dictionary<string, bool[]>();

		private byte[] sc_header = new byte[80] {
			0x52, 0x49, 0x46, 0x46, 0x00, 0x00, 0x00, 0x00, 0x57, 0x41, 0x56, 0x45,
			0x66, 0x6D, 0x74, 0x20, 0x34, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0x02, 0x00,
			0x44, 0xAC, 0x00, 0x00, 0x94, 0x3E, 0x00, 0x00, 0xE8, 0x02, 0x00, 0x00,
			0x22, 0x00, 0x00, 0x08, 0x03, 0x00, 0x00, 0x00, 0xBF, 0xAA, 0x23, 0xE9,
			0x58, 0xCB, 0x71, 0x44, 0xA1, 0x19, 0xFF, 0xFA, 0x01, 0xE4, 0xCE, 0x62,
			0x01, 0x00, 0x28, 0x5C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0x64, 0x61, 0x74, 0x61, 0x00, 0x00, 0x00, 0x00
		};

		//Settings
		public int max_async = 1;
		public int max_bitrate = 128;
		public bool to_sc = true;
		public bool NNNrename = false;
		public bool debug = false;
		public string log_file = "log.txt";

		public MultiTraConv()
		{
			InitializeComponent();

			this.MaximumSize = new Size(986, 800);
			this.SizeChanged += new EventHandler(this.MultiTraConv_SizeChanged);
			this.progressBar.Minimum = 0;
			this.progressBar.Value = 0;
			this.progressBar.Step = 1;
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
				DirectoryInfo mp3_dir = new DirectoryInfo(this.mp3dir_dialog.SelectedPath);
				//FileInfo[] audio_files = mp3_dir.GetFiles("*.mp3", SearchOption.AllDirectories).Where(f => f.Extension==".mp3").ToArray<FileInfo>();
				//FileInfo[] audio2_files = mp3_dir.GetFiles("*.wav", SearchOption.AllDirectories).Where(f => f.Extension == ".wav").ToArray<FileInfo>();
				//int audio_files_length = audio_files.Length;
				//Array.Resize(ref audio_files, audio_files.Length + audio2_files.Length);
				//Array.ConstrainedCopy(audio2_files, 0, audio_files, audio_files_length, audio2_files.Length);
				//audio2_files = null;
				//if (audio_files.Length > 0)
				//{
				//    this.progressBar.Maximum = audio_files.Length;
				//    this.all_count.Text = audio_files.Length.ToString();
				this.root_dir = mp3_dir;
				this.progressBar.Maximum = 0;
				this.mp3_path.Text = this.mp3dir_dialog.SelectedPath;
				//    fill_files_table(audio_files);
				this.FilesTable.Items.Clear();
				fill_files_table(mp3_dir);
				//}
				//else
				//{
				//    MessageBox.Show("No MP3 or WAV files in directory!");
				//}
			}
		}

		private void omadir_button_Click(object sender, EventArgs e)
		{
			if (this.omadir_dialog.ShowDialog() == DialogResult.OK)
			{
				this.oma_path.Text = this.omadir_dialog.SelectedPath;
			}
		}

		//     private void fill_files_table(FileInfo[] mp3_files)
		//     {
		//         this.FilesTable.Items.Clear();
		//foreach (FileInfo file in mp3_files)
		//{
		//	ListViewItem row = new ListViewItem(file.FullName);
		//	row.SubItems.Add("ready");
		//	this.FilesTable.Items.Add(row);
		//}
		//     }

		private void fill_files_table(DirectoryInfo dir)
		{
			FileInfo[] audio_files = Get_tracks_from_dir(dir);
			if (audio_files.Length > 0)
			{
				this.progressBar.Maximum += audio_files.Length;
				this.all_count.Text = this.progressBar.Maximum.ToString();
				foreach (FileInfo track in audio_files)
				{
					ListViewItem row = new ListViewItem(track.FullName);
					row.SubItems.Add("ready");
					this.FilesTable.Items.Add(row);
				}
			}
			foreach (DirectoryInfo subdir in dir.GetDirectories())
			{
				fill_files_table(subdir);
			}
		}

		private FileInfo[] Get_tracks_from_dir(DirectoryInfo dir)
		{
			FileInfo[] audio_files = dir.GetFiles("*.mp3", SearchOption.TopDirectoryOnly).Where(f => f.Extension == ".mp3").ToArray<FileInfo>();
			FileInfo[] audio2_files = dir.GetFiles("*.wav", SearchOption.TopDirectoryOnly).Where(f => f.Extension == ".wav").ToArray<FileInfo>();
			int audio_files_length = audio_files.Length;
			Array.Resize(ref audio_files, audio_files.Length + audio2_files.Length);
			Array.ConstrainedCopy(audio2_files, 0, audio_files, audio_files_length, audio2_files.Length);
			audio2_files = null;
			return audio_files;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (this.FilesTable.Items.Count > 0 && this.oma_path.TextLength > 0)
			{
				foreach (ListViewItem row in this.FilesTable.Items) {
					row.SubItems[1].Text = "ready";
				}
				this.progressBar.Value = 0;

				this.start_convert.Enabled = false;
				this.button_Settings.Enabled = false;
				this.stop_convert.Enabled = true;

				if (debug)
				{
					log = new FileStream(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + log_file, FileMode.Create, FileAccess.Write);
				}

				if (!File.Exists(traconv_path))
				{
					MessageBox.Show("Can`t find " + traconv_path + "!");
					return;
				}

				startInfo = new ProcessStartInfo(traconv_path);
				startInfo.CreateNoWindow = true;
				startInfo.UseShellExecute = false;
				startInfo.RedirectStandardOutput = true;

				//convert(traconv_path);
				ConvertDirs(this.root_dir);
			}
		}

		private async Task ConvertDirs(DirectoryInfo dir)
		{
			this.converted_dirs.Add(dir.FullName, new bool[] { false, false });
			//this.dir_converted = false;
			Convert2(dir);
			while (!this.converted_dirs[dir.FullName][1])
			{
				await Task.Delay(2000);
			}
			Application.DoEvents();
			Console.WriteLine("dir " + dir.Name + " converted");
			foreach (DirectoryInfo subdir in dir.GetDirectories())
			{
				//while (!this.dir_converted)
				//{
				//	await Task.Delay(2000);
				//}
				ConvertDirs(subdir);
			}

			this.start_convert.Enabled = true;
			this.button_Settings.Enabled = true;
			this.stop_convert.Enabled = false;

			if (debug)
			{
				log.Close();
			}
		}

		private async Task Convert2(DirectoryInfo dir)
		{
			int cur_max_async = max_async;
			
			//string prev_dir = "";
			int NNN_cnt = 1;
			//foreach (ListViewItem row in this.FilesTable.Items)
			Console.WriteLine(dir.Name);
			FileInfo[] dir_tracks = Get_tracks_from_dir(dir);
			if (dir_tracks.Length == 0)
			{
				this.converted_dirs[dir.FullName] = new bool[] { true, true };
			}
			foreach (FileInfo track in dir_tracks)
			{
				Console.WriteLine(track.Name + ": " + NNN_cnt);
				if (to_stop)
				{
					to_stop = false;
					break;
				}

				//string filename = row.Text;
				string filename = track.FullName;
				ListViewItem row = this.FilesTable.FindItemWithText(filename);

				//string[] xpath = filename.Split('\\');
				//xpath.SetValue("", xpath.Length - 1);
				//if (prev_dir != String.Join("\\", xpath)) NNN_cnt = 1;
				//prev_dir = String.Join("\\", xpath);

				string add_path = filename.Remove(0, mp3_path.TextLength);
				add_path = add_path.Remove(add_path.Length - 3, 3) + "oma";
				string new_path = oma_path.Text + add_path;
				string new_path_sc = new_path.Remove(new_path.Length - 3, 3) + "sc";
				if (File.Exists(new_path) || File.Exists(new_path_sc))
				{
					if (!File.Exists(new_path_sc) && to_sc)
					{
						change_header(new_path, NNN_cnt);
						row.SubItems[1].Text = "converted";
						NNN_cnt++;
					}
					else
					{
						row.SubItems[1].Text = "skipped (file exists)";
					}
					this.progressBar.PerformStep();
					this.conv_count.Text = this.progressBar.Value.ToString();
					continue;
				}
				string new_path_quoted = '"' + new_path + '"';
				//startInfo.Arguments = '"' + filename + '"' + " --Convert --FileType OMAP --BitRate " + max_bitrate * 1000 + " --Output " + new_path_quoted;
				string Arguments = '"' + filename + '"' + " --Convert --FileType OMAP --BitRate " + max_bitrate * 1000 + " --Output " + new_path_quoted;

				// wait for anyone started processes is finished
				//Console.WriteLine("max async: " + max_async);
				while (max_async == 0)
				{
					await Task.Delay(2000);
				}
				if (to_stop)
				{
					to_stop = false;
					break;
				}
				row.SubItems[1].Text = "converting...";
				max_async--;
				await ConvertFile(Arguments, row, new_path, NNN_cnt, dir.FullName);
				//traconv.Start();
				//get_result(traconv, row, new_path, NNN_cnt, dir.FullName);
				NNN_cnt++;
			}
			// wait for all started processes is finished
			while (max_async < cur_max_async)
			{
				await Task.Delay(2000);
			}
			Application.DoEvents();
			this.converted_dirs[dir.FullName][0] = true;
			//this.dir_converted = true;
			//this.start_convert.Enabled = true;
			//this.button_Settings.Enabled = true;
			//this.stop_convert.Enabled = false;

			//if (debug)
			//{
			//	log.Close();
			//}
		}

		private Task ConvertFile(string Arguments, ListViewItem row, string new_path, int NNN_cnt, string dirFullName)
		{
			return Task.Run(() =>
			{
				Process traconv = new Process();
				startInfo.Arguments = Arguments;
				traconv.StartInfo = startInfo;
				traconv.Start();
				get_result(traconv, row, new_path, NNN_cnt, dirFullName);
			});
		}

		//private async void convert(string traconv_path)
  //      {
  //          int cur_max_async = max_async;
			
		//	ProcessStartInfo startInfo = new ProcessStartInfo(traconv_path);
		//	startInfo.CreateNoWindow = true;
  //          startInfo.UseShellExecute = false;
  //          startInfo.RedirectStandardOutput = true;

  //          string prev_dir = "";
  //          int NNN_cnt = 1;
  //          foreach (ListViewItem row in this.FilesTable.Items)
  //          {
  //              if (to_stop)
  //              {
  //                  to_stop = false;
  //                  break;
  //              }
                
  //              string filename = row.Text;
                
  //              string[] xpath = filename.Split('\\');
  //              xpath.SetValue("", xpath.Length - 1);
  //              if (prev_dir != String.Join("\\", xpath)) NNN_cnt = 1;
  //              prev_dir = String.Join("\\", xpath);

  //              string add_path = filename.Remove(0, mp3_path.TextLength);
  //              add_path = add_path.Remove(add_path.Length - 3, 3) + "oma";
  //              string new_path = oma_path.Text + add_path;
  //              string new_path_sc = new_path.Remove(new_path.Length - 3, 3) + "sc";
  //              if (File.Exists(new_path) || File.Exists(new_path_sc))
  //              {
  //                  if (! File.Exists(new_path_sc) && to_sc)
  //                  {
  //                      change_header(new_path, NNN_cnt);
  //                      row.SubItems[1].Text = "converted";
  //                      NNN_cnt++;
  //                  }
  //                  else
  //                  {
  //                      row.SubItems[1].Text = "skipped (file exists)";
  //                  }
  //                  this.progressBar.PerformStep();
  //                  this.conv_count.Text = this.progressBar.Value.ToString();
  //                  continue;
  //              }
  //              string new_path_quoted = '"' + new_path + '"';
  //              startInfo.Arguments = '"' + filename + '"' + " --Convert --FileType OMAP --BitRate " + max_bitrate * 1000 + " --Output " + new_path_quoted;

  //              while (max_async == 0)
  //              {
  //                  await Task.Delay(2000);
  //              }
  //              if (to_stop)
  //              {
  //                  to_stop = false;
  //                  break;
  //              }
  //              row.SubItems[1].Text = "converting...";
  //              max_async--;
  //              Process traconv = new Process();
  //              traconv.StartInfo = startInfo;
  //              traconv.Start();
  //              get_result(traconv, row, new_path, NNN_cnt);
  //              NNN_cnt++;
  //          }
  //          while (max_async < cur_max_async)
  //          {
  //              await Task.Delay(2000);
  //          }
  //          Application.DoEvents();
  //          this.start_convert.Enabled = true;
  //          this.button_Settings.Enabled = true;
  //          this.stop_convert.Enabled = false;

  //          if (debug)
  //          {
  //              log.Close();
  //          }
  //      }

        private async void get_result (Process proc, ListViewItem item, string new_path, int NNN, string dir)
        {
            string p_out = await proc.StandardOutput.ReadToEndAsync();
            if (!p_out.Contains("Location=" + new_path)) {
                item.SubItems[1].Text = "Error: " + proc.StartInfo.Arguments;
            }
            else
            {
                item.SubItems[1].Text = "converted";
				//if (to_sc) change_header(new_path, NNN);
				change_header(new_path, NNN);
				Console.WriteLine(item.Text + ": converted");
				if (this.converted_dirs[dir][0])
				{
					this.converted_dirs[dir][1] = true;
				}
				this.progressBar.PerformStep();
                this.conv_count.Text = this.progressBar.Value.ToString();
            }
            if (debug)
            {
                byte[] out_text = Encoding.GetEncoding(1251).GetBytes(p_out);
                log.Write(out_text, 0, out_text.Length);
            }
            max_async++;
			//Console.WriteLine(max_async);
        }

        private void change_header(string oma_file, int NNN)
        {
            string sc_file = oma_file.Remove(oma_file.Length - 3, 3) + "sc";

			string[] omas = oma_file.Split('\\');
			string omaf = omas[omas.Length - 1].Remove(omas[omas.Length - 1].Length - 3, 3);

			// NNN - random!!!
			if (NNNrename)
            {
                string NNNst = (NNN.ToString().Length >= 3) ? NNN.ToString() : ((NNN.ToString().Length == 2) ? "0" + NNN.ToString() : "00" + NNN.ToString());
                string[] NNNsc = sc_file.Split('\\');
				NNNsc.SetValue(NNNst + ".sc", NNNsc.Length - 1);
				sc_file = String.Join("\\", NNNsc);
            }
            
            FileStream in_file = new FileStream(oma_file, FileMode.Open, FileAccess.Read);
            FileStream out_file = new FileStream(sc_file, FileMode.Create, FileAccess.Write);

			//OMA files
			//These are the actual MP3 files. The file starts with a tag "ea3";
			//replacing the "ea" with "ID" turns this into an ID3 block, complete
			//with size tag, which can be read with a standard ID3 library.So far
			//all sample files have had 3072 bytes of ID3 data on the device,
			//regardless of the amount in the input files.After the ID3 tag there
			//is a second block starting with either "ea3" or "EA3"(not sure why
			//there's a case difference, nor whether it changes between
			//versions).The next byte, 0x02, is probably part of this
			//signature.The next 16 - bit word is the size of the header including
			//the 4 - byte signature.Immediately after the header is the mp3 data

			byte[] ID3_head = new byte[10];
            byte[] ea3_size = new byte[2];

            in_file.Read(ID3_head, 0, ID3_head.Length);
            ////http://www.developerfusion.com/code/4684/read-mp3-tag-information-id3v1-and-id3v2/
            int[] bytes = new int[4];      // for bit shifting
            bytes[3] = ID3_head[9] | ((ID3_head[8] & 1) << 7);
            bytes[2] = ((ID3_head[8] >> 1) & 63) | ((ID3_head[7] & 3) << 6);
            bytes[1] = ((ID3_head[7] >> 2) & 31) | ((ID3_head[6] & 7) << 5);
            bytes[0] = ((ID3_head[6] >> 3) & 15);

            ulong ID3Size = ((UInt64)10 + (UInt64)bytes[3] |
                ((UInt64)bytes[2] << 8) |
                ((UInt64)bytes[1] << 16) |
                ((UInt64)bytes[0] << 24));

			// 0x100 - ???
			// 4 - second ea3 header tag
			int ea3_offset = (int)(ID3Size + 0x100 + 4);
            in_file.Position = ea3_offset;

			// 2 bytes - second ea3 header length
			in_file.Read(ea3_size, 0, ea3_size.Length);

            Array.Reverse(ea3_size);
            int ea3Size = BitConverter.ToInt16(ea3_size, 0);
            in_file.Position = ea3_offset + ea3Size - 4;

            out_file.Write(sc_header, 0, sc_header.Length);
            in_file.CopyTo(out_file);

            in_file.Close();
            out_file.Close();
            File.Delete(oma_file);
        }

        private void stop_convert_Click(object sender, EventArgs e)
        {
            to_stop = true;
        }

        private void button_Settings_Click(object sender, EventArgs e)
        {
            SettingsPage sp = new SettingsPage(this);
            sp.ShowDialog();
        }

        private void Help_button_Click(object sender, EventArgs e)
        {
            HelpPage hp = new HelpPage();
            hp.ShowDialog();
        }

		private string toUTF8(string text)
		{
			if (text == null || text.Length == 0) return "";
			return new string(text.ToCharArray().
				Select(x => ((x + 848) >= 'А' && (x + 848) <= 'ё') ? (char)(x + 848) : x).
				ToArray()
			);
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			//TagLib.File file = TagLib.File.Create("d:\\ID3V1andV2example.mp3");
			//TagLib.File file = TagLib.File.Create("d:\\ID3V1andV2example.oma");
			//TagLib.File file = TagLib.File.Create("d:\\01-Butch--Не Дали.mp3");
			//TagLib.File file = TagLib.File.Create("d:\\01-Butch--Не Дали.oma");

			//Console.WriteLine("Artist: " + ((file.Tag.Performers.Length > 0) ? toUTF8(file.Tag.Performers[0]) : "Failed!!!"));
			//Console.WriteLine("Title: " + toUTF8(file.Tag.Title));
			//Console.WriteLine("Album: " + toUTF8(file.Tag.Album));

			//string log_file = "d:\\id3vtest\\id3v.log";
			//string log_unkfile = "d:\\id3vtest\\id3v_unk.log";
			//this.progressBar.Minimum = 0;
			//this.progressBar.Maximum = this.FilesTable.Items.Count;
			//this.progressBar.Value = 0;
			//this.progressBar.Step = 1;
			//foreach (ListViewItem row in this.FilesTable.Items)
			//{
			//	string filename = row.Text;
			//	TagLib.File file = TagLib.File.Create(filename);
			//	filename.Replace("\n", "").Replace("\r", "");
			//	string artist = (file.Tag.Performers.Length > 0) ? toUTF8(file.Tag.Performers[0]) : "Empty";
			//	string title = (file.Tag.Title != null && file.Tag.Title.Length > 0) ? toUTF8(file.Tag.Title) : "";
			//	if (title.Length < 1)
			//	{
			//		char delim = '\\';
			//		string[] parts = filename.Split(delim);
			//		title = parts[parts.Length - 1];
			//		title = toUTF8(title.Remove(title.Length - 4, 4));
					
			//		if (title.Contains(" - "))
			//		{
			//			string[] tar = title.Split(new string[] { " - " }, 2, StringSplitOptions.RemoveEmptyEntries);
			//			if (tar.Length == 2)
			//			{
			//				char[] t0 = tar[0].ToCharArray().Where((c) => !char.IsDigit(c)).ToArray();
			//				char[] t1 = tar[1].ToCharArray().Where((c) => !char.IsDigit(c)).ToArray();
			//				if (t0.Length > 1 && t1.Length > 0)
			//				{
			//					File.AppendAllText(log_unkfile, String.Format("{0,64}:{1,64}: ", toUTF8(tar[0]), toUTF8(tar[1])));
			//					File.AppendAllText(log_unkfile, toUTF8(filename) + "\n");
			//					this.progressBar.PerformStep();
			//					continue;
			//				}
			//			}
			//		}
			//	}
			//	// in TITLE for title and artist - by 128 bytes!!!
			//	// ё!!!
			//	// delete start and end spaces!!!!
			//	//string output = String.Format("{0,128}:{1,128}: ", title, artist);
			//	string output = String.Format("{0,64}:{1,64}: ", title, artist);
			//	File.AppendAllText(log_file, output);
			//	File.AppendAllText(log_file, filename + "\n");
			//	this.progressBar.PerformStep();
			//}
			//MessageBox.Show("Done");			
		}
	}
}
