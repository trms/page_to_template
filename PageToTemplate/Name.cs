using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PageToTemplate
{
	public partial class Name : Form
	{
		public Name()
		{
			InitializeComponent();
		}

		public string CurrentName
		{
			get { return textBox1.Text; }
			set { textBox1.Text = value; }
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
			CurrentName = r.Replace(CurrentName, String.Empty);
			if (String.IsNullOrEmpty(CurrentName))
			{
				CurrentName = String.Format("Template {0}", DateTime.Now.ToShortDateString());
				return;
			}
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}
	}
}
