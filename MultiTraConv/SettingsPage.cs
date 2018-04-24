using System;
using System.Windows.Forms;

namespace MultiTraConv
{
    public partial class SettingsPage : Form
    {
        MultiTraConv mform;

        public SettingsPage(MultiTraConv MainForm)
        {
            InitializeComponent();
            mform = MainForm;

            this.is_debug.Checked = mform.debug;
            this.log_file.Text = mform.log_file;
            this.to_sc.Checked = mform.to_sc;
            this.NNNrename.Checked = (mform.to_sc) ? mform.NNNrename : false;
            this.NNNrename.Enabled = (mform.to_sc) ? true : false;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.user_max_async.Value = mform.max_async;
            //this.user_max_bitrate.SelectedItem = mform.max_bitrate.ToString();
        }

        private void user_max_async_ValueChanged(object sender, EventArgs e)
        {
            mform.max_async = Decimal.ToInt32(this.user_max_async.Value);
        }

        private void user_max_bitrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //mform.max_bitrate = Convert.ToInt32(this.user_max_bitrate.SelectedItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mform.log_file = this.log_file.Text;
            this.Close();
        }

        private void is_debug_CheckedChanged(object sender, EventArgs e)
        {
            mform.debug = this.is_debug.Checked;
            this.log_file.ReadOnly = (this.is_debug.Checked) ? false : true;
        }

        private void to_sc_CheckedChanged(object sender, EventArgs e)
        {
            mform.to_sc = this.to_sc.Checked;
            this.NNNrename.Checked = (mform.to_sc) ? mform.NNNrename : false;
            this.NNNrename.Enabled = (mform.to_sc) ? true : false;
        }

        private void NNNrename_CheckedChanged(object sender, EventArgs e)
        {
            mform.NNNrename = this.NNNrename.Checked;
        }
    }
}
