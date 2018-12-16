using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace MultiTraConv
{
	public partial class MultiTraConv : Form
	{
		private Int32 _start_form_height = 547;
		private Int32 _start_table_height = 363;
		private bool to_stop = false;
		//private FileStream log;
		private string traconv_path = Path.GetDirectoryName(Application.ExecutablePath) + "\\TraConv.exe";
		private ProcessStartInfo startInfo;
		private DirectoryInfo root_dir;
		private Dictionary<string, bool[]> converted_dirs = new Dictionary<string, bool[]>();
		private Dictionary<string, int> dirtasks = new Dictionary<string, int>();
		private Dictionary<string, int> dirtrackdone = new Dictionary<string, int>();

		private string codePage = "iso-8859-5";

		private delegate void LVIDelegate(string filename, string Text);
		private delegate void PBDelegate();
		private delegate void SCDelegate();
		private CancellationTokenSource ts = new CancellationTokenSource();
		private CancellationToken ct;

		Log logWindow = new Log();

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
		public int max_async = 3;
		public bool to_sc = true;
		public bool NNNrename = false;
		//public bool debug = false;
		//public string log_file = "log.txt";

		public MultiTraConv()
		{
			InitializeComponent();

			ct = ts.Token;

			this.MaximumSize = new Size(986, 800);
			this.SizeChanged += new EventHandler(this.MultiTraConv_SizeChanged);
			this.user_max_async.Value = max_async;
			this.progressBar.Minimum = 0;
			this.progressBar.Value = 0;
			this.progressBar.Step = 1;

			// test
			//this.mp3_path.Text = "<fullpath>";
			//this.oma_path.Text = "<fullpath>";
			//this.root_dir = new DirectoryInfo(this.mp3_path.Text);
			//this.progressBar.Maximum = 0;
			//this.FilesTable.Items.Clear();
			//fill_files_table(this.root_dir);
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

		private void addLog(string text)
		{
			if (logWindow.Visible)
			{
				logWindow.addLogText(text);
			}
		}

		private void mp3dir_button_Click(object sender, EventArgs e)
		{
			if (this.mp3dir_dialog.ShowDialog() == DialogResult.OK)
			{
				DirectoryInfo mp3_dir = new DirectoryInfo(this.mp3dir_dialog.SelectedPath);
				this.root_dir = mp3_dir;
				this.progressBar.Maximum = 0;
				this.mp3_path.Text = this.mp3dir_dialog.SelectedPath;
				this.FilesTable.Items.Clear();
				fill_files_table(mp3_dir);
			}
		}

		private void omadir_button_Click(object sender, EventArgs e)
		{
			if (this.omadir_dialog.ShowDialog() == DialogResult.OK)
			{
				this.oma_path.Text = this.omadir_dialog.SelectedPath;
			}
		}

		private void fill_files_table(DirectoryInfo dir)
		{
			FileInfo[] audio_files = Get_input_tracks_from_dir(dir);
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

		private FileInfo[] Get_input_tracks_from_dir(DirectoryInfo dir)
		{
			FileInfo[] audio_files = dir.GetFiles("*.mp3", SearchOption.TopDirectoryOnly).Where(f => f.Extension == ".mp3").ToArray<FileInfo>();
			FileInfo[] audio2_files = dir.GetFiles("*.wav", SearchOption.TopDirectoryOnly).Where(f => f.Extension == ".wav").ToArray<FileInfo>();
			int audio_files_length = audio_files.Length;
			Array.Resize(ref audio_files, audio_files.Length + audio2_files.Length);
			Array.ConstrainedCopy(audio2_files, 0, audio_files, audio_files_length, audio2_files.Length);
			audio2_files = null;
			return audio_files;
		}

		private FileInfo[] Get_output_tracks_from_dir(DirectoryInfo dir)
		{
			FileInfo[] audio_files = dir.GetFiles("*.sc", SearchOption.TopDirectoryOnly).Where(f => f.Extension == ".sc").ToArray<FileInfo>();
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
				this.stop_convert.Enabled = true;

				//if (debug)
				//{
				//	log = new FileStream(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + log_file, FileMode.Create, FileAccess.Write);
				//}

				if (!File.Exists(traconv_path))
				{
					MessageBox.Show("Can`t find " + traconv_path + "!");
					this.start_convert.Enabled = true;
					this.stop_convert.Enabled = false;
					return;
				}

				startInfo = new ProcessStartInfo(traconv_path);
				startInfo.CreateNoWindow = true;
				startInfo.UseShellExecute = false;
				startInfo.RedirectStandardOutput = true;

				logWindow.Show();
				ConvertDirs(this.root_dir);
				ConvertDone();
			}
		}

		private async void ConvertDone()
		{
			while (dirtrackdone.Sum(dt => dt.Value) > 0)
			{
				addLog(String.Join(",", 
					dirtrackdone.Where((dir) => dir.Value > 0).ToDictionary(k => k.Key, k => k.Value).Select((dir) => dir.Key + ":" + dir.Value).ToArray()
				));
				await Task.Delay(2000);
			}
			to_stop = false;
			this.start_convert.Enabled = true;
			this.stop_convert.Enabled = false;
			Console.WriteLine(DateTime.Now.ToString("h:mm:ss"));
			MessageBox.Show("DONE!");
		}

		private async void ConvertDirs(DirectoryInfo dir)
		{
			ConvertDir(dir);
			foreach (DirectoryInfo subdir in dir.GetDirectories())
			{
				while (dirtasks.Sum(dt => dt.Value) >= max_async)
				{
					//Console.WriteLine("ConvertDirs: max tasks - wait");
					await Task.Delay(2000);
				}
				if (to_stop)
				{
					//Console.WriteLine("ConvertDirs: stop - return");
					addLog("ConvertDirs: stop - return");
					return;
				}
				ConvertDirs(subdir);
			}
		}

		private async void ConvertDir(DirectoryInfo dir)
		{
			int NNN_cnt = 0;
			//Console.WriteLine("dir #" + dir.Name + "# converting...");
			addLog("dir #" + dir.Name + "# converting...");
			FileInfo[] dir_tracks = Get_input_tracks_from_dir(dir);
			if (dir_tracks.Length == 0)
			{
				//Console.WriteLine("dir empty");
				addLog("dir empty");
				return;
			}
			DirectoryInfo sc_dir = new DirectoryInfo(oma_path.Text + dir.FullName.Remove(0, mp3_path.TextLength));
			Dictionary<string, string[]> titles = new Dictionary<string, string[]>();
			string add_path = null;
			foreach (FileInfo track in dir_tracks)
			{
				if (dir_tracks.Length > 99)
				{
					SetLVIText(track.FullName, "skipped! Max 99 tracks in directory!");
					continue;
				}
				NNN_cnt++;
				//Console.WriteLine("ConvertDir: file - " + track.Name + " -> " + NNN_cnt);
				addLog("ConvertDir: file - " + track.Name + " -> " + NNN_cnt);

				while (dirtasks.Sum(dt => dt.Value) >= max_async)
				{
					//Console.WriteLine("ConvertDir: max tasks - wait");
					await Task.Delay(2000);
				}
				if (to_stop)
				{
					//Console.WriteLine("ConvertDir: stop - break");
					addLog("ConvertDir: stop - break");
					break;
				}
				if (add_path == null)
				{
					add_path = track.FullName.Remove(0, mp3_path.TextLength);
					add_path = add_path.Remove(add_path.Length - track.Name.Length, track.Name.Length);
				}
				if (dirtasks.ContainsKey(add_path))	{
					dirtasks[add_path]++;
					dirtrackdone[add_path]++;
				} else {
					dirtasks.Add(add_path, 1);
					dirtrackdone.Add(add_path, 1);
				}

				string NNN = String.Format("{0,3:D3}", NNN_cnt);
				titles.Add(NNN + ".sc", trackInfo(track));
				ConvertFile(track, NNN);
				Application.DoEvents();
			}

			if (dir_tracks.Length > 99)
			{
				return;
			}

			// WARNING!!!
			// при одновременной конвертации одинаковых файлов (именно по музыке, не по ID3v тегу) 
			// через TraConv файлы могут НЕ сконвертироваться!
			// поэтому можем получить пропуски в списке файлов .sc: 013.sc, 014.sc, 015.sc, 018.sc, 019.sc!
			// и количество файлов *.sc будет меньше исходного количества файлов
			while (dirtasks[add_path] > 0)
			{
				//Console.WriteLine("ConvertDir: dir converted - wait tracks");
				if (to_stop)
				{
					while (dirtasks[add_path] > 0)
					{
						//Console.WriteLine("ConvertDir: stop - wait tracks");
						addLog("ConvertDir: stop - wait tracks");
						//Console.WriteLine("TASKS: " + dirtasks[add_path]);
						addLog("TASKS: " + dirtasks[add_path]);
						await Task.Delay(2000);
					}
					break;
				}
				//Console.WriteLine("TASKS: " + dirtasks[add_path]);
				await Task.Delay(2000);
			}
			//Console.WriteLine("ConvertDir: dir " + dir.Name + " converted");
			addLog("ConvertDir: dir " + dir.Name + " converted");
			CreateTitle(sc_dir, titles);
		}

		private void SetLVIText(string filename, string text)
		{
			if (this.FilesTable.InvokeRequired)
			{
				LVIDelegate LVID = new LVIDelegate(SetLVIText);
				this.FilesTable.BeginInvoke(LVID, new object[] { filename, text });
			}
			else
			{
				this.FilesTable.FindItemWithText(filename).SubItems[1].Text = text;
			}
		}

		private void SetPBStep()
		{
			if (this.progressBar.InvokeRequired)
			{
				PBDelegate D = new PBDelegate(SetPBStep);
				this.progressBar.BeginInvoke(D);
			}
			else
			{
				this.progressBar.PerformStep();
				this.conv_count.Text = this.progressBar.Value.ToString();
			}
		}

		private Task ConvertFile(FileInfo track, string NNN_name)
		{
			var tcs = new TaskCompletionSource<bool>();

			string fullfilename = track.FullName;
			string filename = track.Name;

			ListViewItem row = this.FilesTable.FindItemWithText(fullfilename);

			SetLVIText(fullfilename, "converting...");

			string add_path = fullfilename.Remove(0, mp3_path.TextLength);
			add_path = add_path.Remove(add_path.Length - filename.Length, filename.Length);
			//Console.WriteLine("ConvertFile: add_path #" + add_path + "#");
			string new_path_sc = oma_path.Text + add_path + NNN_name + ".sc";
			if (File.Exists(new_path_sc))
			{
				SetLVIText(fullfilename, "converted");
				//SetPBStep();
				tcs.SetResult(true);
				dirtasks[add_path]--;
				return tcs.Task;
			}
			string new_path_quoted = '"' + new_path_sc + '"';
			string Arguments = '"' + fullfilename + '"' + " --Convert --FileType OMAP --BitRate 128000 --Output " + new_path_quoted;

			//Console.WriteLine(Arguments);
			Process traconv = new Process();
			startInfo.Arguments = Arguments;
			traconv.EnableRaisingEvents = true;
			traconv.StartInfo = startInfo;
			traconv.Exited += (sender, args) =>
			{
				string p_out = traconv.StandardOutput.ReadToEnd();
				if (!p_out.Contains("TargetTrack=" + fullfilename))
				{
					SetLVIText(fullfilename, "Error: " + traconv.StartInfo.Arguments);
				}
				else
				{
					SetLVIText(fullfilename, "converted");
					//SetPBStep();
				}
				traconv.Dispose();
				dirtasks[add_path]--;
				tcs.SetResult(true);
			};
			traconv.Start();
			return tcs.Task;
		}

		//private void CreateTitle(DirectoryInfo dir, Dictionary<string, Dictionary<string, string[]>> titles)
		private void CreateTitle(DirectoryInfo dir, Dictionary<string, string[]> titles)
		{
			FileInfo[] sc_files = Get_output_tracks_from_dir(dir);
			if (sc_files.Length == 0) { return; }
			if (File.Exists(dir + "\\TITLE.lst"))
			{
				string add_path = sc_files[0].FullName.Remove(0, oma_path.TextLength);
				add_path = add_path.Remove(add_path.Length - sc_files[0].Name.Length, sc_files[0].Name.Length);
				dirtrackdone[add_path] = 0;
				return;
			}
			FileStream title_file = new FileStream(dir.FullName + "\\TITLE.lst", FileMode.Create, FileAccess.Write);

			// for 1-st block checksum
			title_file.Write(new byte[] { 0, 0, 0, 0 }, 0, 4);
			// SLJA_TITLE:1.3
			title_file.Write(
				new byte[] { 0x53, 0x4C, 0x4A, 0x41, 0x5F, 0x54, 0x49, 0x54, 0x4C, 0x45, 0x3A, 0x31, 0x2E, 0x33, 0x20 },
				0, 15);
			// for disk id
			title_file.Write(Enumerable.Repeat<byte>(0, 7).ToArray(), 0, 7);
			// 10 null bytes
			title_file.Write(Enumerable.Repeat<byte>(0, 10).ToArray(), 0, 10);
			// data length start from 1b4 offset!!!
			// 28 bytes head + 384 bytes disc title + num of tracks * 384 bytes
			// WARNING!!!
			// при одновременной конвертации одинаковых файлов (именно по музыке, не по ID3v тегу) 
			// через TraConv файлы могут НЕ сконвертироваться!
			// поэтому можем получить пропуски в списке файлов .sc: 013.sc, 014.sc, 015.sc, 018.sc, 019.sc!
			// в TITLE.lst пропуски будут заполнены нулями
			// в итоге количество файлов может быть меньше, чем записано в TITLE.lst
			// поэтому количество файлов = максимальный номер трека
			int num_of_tracks = sc_files.Max(trackfile => Convert.ToInt32(trackfile.Name.Remove(trackfile.Name.Length - 3, 3)));
			byte[] dl = BitConverter.GetBytes(28 + 384 + num_of_tracks * 384);
			Array.Resize(ref dl, 4);
			title_file.Write(dl, 0, 4);
			// 396 null bytes (99 disks titles * 4 bytes)
			title_file.Write(Enumerable.Repeat<byte>(0, 396).ToArray(), 0, 396);

			// HEAD
			// for 12 bytes checksum and disk id
			title_file.Write(Enumerable.Repeat<byte>(0, 8).ToArray(), 0, 8);
			// count of tracks and ...cddb id???
			byte count_files = Convert.ToByte(sc_files.Length);
			title_file.Write(new byte[] { count_files, 1, 0, 0, 0, 0, 0, 0 }, 0, 8);
			// for 12 bytes (checksum and ???)
			title_file.Write(Enumerable.Repeat<byte>(0, 12).ToArray(), 0, 12);
			// TITLES
			//128 - name
			//64 - name on japan
			//128 - artist
			//64 - artist on japan
			// disc title (384 bytes)
			byte[] dirname = stringTo128bytes(dir.Name);
			title_file.Write(dirname, 0, dirname.Length);
			title_file.Write(Enumerable.Repeat<byte>(0, 64).ToArray(), 0, 64);
			// MAY BE ONE ARTIST!!!
			byte[] dirart = stringTo128bytes("Various Artists");
			title_file.Write(dirart, 0, dirart.Length);
			title_file.Write(Enumerable.Repeat<byte>(0, 64).ToArray(), 0, 64);

			// track titles (384 bytes x number of tracks)
			// WARNING!!!
			// при одновременной конвертации одинаковых файлов (именно по музыке, не по ID3v тегу) 
			// через TraConv файлы могут НЕ сконвертироваться!
			// поэтому можем получить пропуски в списке файлов .sc: 013.sc, 014.sc, 015.sc, 018.sc, 019.sc!
			// пропуски повлияют на порядок файлов в TITLE.lst!
			// заполняем описание пропущенных файлов нулями
			int cur_file_num = 1;
			foreach (FileInfo track in sc_files)
			{
				int trackNum = Convert.ToInt32(track.Name.Remove(track.Name.Length - 3, 3));
				if (trackNum > cur_file_num)
				{
					int diff = trackNum - cur_file_num;
					while (diff > 0)
					{
						title_file.Write(Enumerable.Repeat<byte>(0, 128 + 64 + 128 + 64).ToArray(), 0, 128 + 64 + 128 + 64);						
						diff--;
					}
				}
				byte[] name = (titles.ContainsKey(track.Name)) ? stringTo128bytes(titles[track.Name][0]) : stringTo128bytes(trackNum + "");
				title_file.Write(name, 0, name.Length);
				title_file.Write(Enumerable.Repeat<byte>(0, 64).ToArray(), 0, 64);
				byte[] art = (titles.ContainsKey(track.Name)) ? stringTo128bytes(titles[track.Name][1]) : stringTo128bytes("");
				// MAY BE ONE ARTIST!!!
				title_file.Write(art, 0, art.Length);
				title_file.Write(Enumerable.Repeat<byte>(0, 64).ToArray(), 0, 64);
				cur_file_num = (trackNum > cur_file_num) ? trackNum + 1 : cur_file_num + 1;
			}
			title_file.Close();
			change_header(sc_files);
			addLog("TITLE.lst for " + dir.Name + " created");
			//Console.WriteLine(DateTime.Now.ToString("h:mm:ss"));
		}

		private byte[] stringTo128bytes(string str)
		{
			byte[] strbytes = Encoding.GetEncoding(codePage).GetBytes(str);
			// 128 bytes on string
			byte[] lb = Enumerable.Repeat<byte>(0, 128).ToArray();
			if (strbytes.Length > 128)
			{
				Array.Resize(ref strbytes, 128);
			}
			Array.Copy(strbytes, lb, strbytes.Length);
			return lb;
		}

		private string[] trackInfo(FileInfo track)
		{
			TagLib.File file = TagLib.File.Create(track.FullName);
			string artist = (file.Tag.Performers.Length > 0) ? toUTF8(file.Tag.Performers[0]) : "";
			string title = (file.Tag.Title != null && file.Tag.Title.Length > 0) ? toUTF8(file.Tag.Title) : "";
			if (title.Length < 1)
			{
				title = track.Name;
				title = toUTF8(title.Remove(title.Length - 4, 4));

				if (title.Contains(" - "))
				{
					string[] tar = title.Split(new string[] { " - " }, 2, StringSplitOptions.RemoveEmptyEntries);
					if (tar.Length == 2)
					{
						char[] t0 = tar[0].ToCharArray().Where((c) => !char.IsDigit(c)).ToArray();
						char[] t1 = tar[1].ToCharArray().Where((c) => !char.IsDigit(c)).ToArray();
						if (t0.Length > 1 && t1.Length > 0)
						{
							title = toUTF8(tar[0]);
							artist = toUTF8(tar[1]);
						}
					}
				}
			}
			return new string[] {title, artist};
		}

		private void change_header(FileInfo[] sc_files)
		{
			//Console.WriteLine("change_header: dir " + sc_files[0].DirectoryName + " start");
			addLog("change_header: dir " + sc_files[0].DirectoryName + " start");

			string add_path = sc_files[0].FullName.Remove(0, oma_path.TextLength);
			add_path = add_path.Remove(add_path.Length - sc_files[0].Name.Length, sc_files[0].Name.Length);
			foreach (FileInfo track in sc_files)
			{
				FileStream in_file = new FileStream(track.FullName, FileMode.Open, FileAccess.Read);
				FileStream out_file = new FileStream(track.FullName + ".mmcs", FileMode.Create, FileAccess.Write);

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
				string old_name = track.FullName;
				File.Delete(track.FullName);
				File.Move(out_file.Name, old_name);
				//Console.WriteLine("change_header: file: " + old_name + " changed");
				addLog("change_header: file: " + old_name + " changed");
				SetPBStep();
				dirtrackdone[add_path]--;
			}
		}

		private void stop_convert_Click(object sender, EventArgs e)
        {
            to_stop = true;
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

		private void user_max_async_ValueChanged(object sender, EventArgs e)
		{
			max_async = Decimal.ToInt32(this.user_max_async.Value);
		}
	}
}
