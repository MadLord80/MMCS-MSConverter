﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiTraConv
{
	public partial class Log : Form
	{
		public Log()
		{
			InitializeComponent();
		}

		public void addLogText(string text)
		{
			logBox.AppendText(Environment.NewLine + text);
		}
	}
}
