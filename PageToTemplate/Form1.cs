using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;

namespace PageToTemplate
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		delegate void EnableFormCallback(bool enabled);
		private void EnableForm(bool enabled)
		{
			try
			{
				if (this.InvokeRequired)
				{
					EnableFormCallback d = new EnableFormCallback(EnableForm);
					this.Invoke(d, new object[] { enabled });
				}
				else
				{
					this.Enabled = enabled;
					progressBar1.Visible = !enabled;
				}
			}
			catch (ObjectDisposedException)
			{ }
		}

		delegate void SetStatusTextCallback(string text);
		private void SetStatusText(string text)
		{
			try
			{
				if (this.InvokeRequired)
				{
					SetStatusTextCallback d = new SetStatusTextCallback(SetStatusText);
					this.Invoke(d, new object[] { text });
				}
				else
				{
					statusText.Text = text;
				}
			}
			catch (ObjectDisposedException)
			{ }
		}

		delegate string GetNameCallback(string text);
		private string GetName(string text)
		{
			try
			{
				if (this.InvokeRequired)
				{
					GetNameCallback d = new GetNameCallback(GetName);
					return this.Invoke(d, new object[] { text }) as String;
				}
				else
				{
					Name n = new PageToTemplate.Name();
					n.CurrentName = text;
					string result = null;
					if (n.ShowDialog() == System.Windows.Forms.DialogResult.OK)
						result = n.CurrentName;
					return result;
				}
			}
			catch (ObjectDisposedException)
			{ }
			return string.Empty;
		}

		private Dictionary<string, Dictionary<string, string>> GetPageInfo(string path)
		{
			Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
			using (FileStream istream = File.OpenRead(path))
			{
				using (ZipInputStream zip = new ZipInputStream(istream))
				{
					ZipEntry entry;
					while ((entry = zip.GetNextEntry()) != null)
					{
						if (!entry.IsDirectory)
						{
							string[] parts = entry.Name.Split(Path.DirectorySeparatorChar);
							if (parts.Length == 3 && parts[0].ToLower() == "pages")
							{
								string pageid = parts[1];
								if (parts[2] == String.Format("{0}.dat", pageid))
								{
									StringBuilder info = new StringBuilder();
									int size;
									byte[] data = new byte[4096];
									while ((size = zip.Read(data, 0, data.Length)) > 0)
										info.Append(Encoding.ASCII.GetString(data, 0, size));
									string[] lines = info.ToString().Split('\n');
									Dictionary<string, string> d = new Dictionary<string, string>();
									foreach (string l in lines)
									{
										string line = l.Trim();
										int e = line.IndexOf('=');
										if (e != -1)
										{
											string name = line.Substring(0, e);
											string value = line.Substring(e + 1);
											if (d.ContainsKey(name) == false)
												d.Add(name, value);
										}
									}
									// only convert TRMSImages
									if (d.ContainsKey("objecttype") && d["objecttype"] == "TRMS.Components.Imaging.TRMSImage" && d.ContainsKey("description"))
									{
										string name = GetName(d["description"]);
										// a cancel on the name dialog will cause the bulletin to get skipped
										if (String.IsNullOrEmpty(name) == false)
										{
											d.Add("name", name);
											result.Add(pageid, d);
										}
									}
								}
							}
						}
					}
					zip.Close();
				}
			}

			return result;
		}

		private void ImportPackage(string path)
		{
			SetStatusText(String.Format("Importing {0}...", Path.GetFileName(path)));
			try
			{
				Dictionary<string, Dictionary<string, string>> pages = GetPageInfo(path);
				SetStatusText(String.Format("Importing {0}; {1} bulletin{2}...", Path.GetFileName(path), pages.Count, pages.Count == 1 ? String.Empty : "s"));

				string zipFile = Path.Combine(Path.GetDirectoryName(path), String.Format("Templates{0}", Path.GetFileName(path).Replace("Bulletins", String.Empty)));
				using (FileStream os = File.Create(zipFile))
				{
					using (ZipOutputStream zout = new ZipOutputStream(os))
					{
						zout.UseZip64 = UseZip64.Off;
						zout.SetLevel(5);

						using (FileStream istream = File.OpenRead(path))
						{
							// pass to get page details
							using (ZipInputStream zip = new ZipInputStream(istream))
							{
								ZipEntry entry;
								while ((entry = zip.GetNextEntry()) != null)
								{
									if (!entry.IsDirectory)
									{
										string[] parts = entry.Name.Split(Path.DirectorySeparatorChar);
										if (parts.Length == 3 && parts[0].ToLower() == "pages" && pages.ContainsKey(parts[1]))
										{
											string pageid = parts[1];
											string file = parts[2];
											Dictionary<string, string> info = pages[pageid];
											// only pass specific TRMSImage files through (skip the bulletin info .dat and anything else a template doesn't need)
											if (file.ToLower() == "final.jpg" || file.ToLower() == "preview.jpg" || file.ToLower() == "page.dat" || file.ToLower() == "thumbnail.jpg" || file.ToLower() == "tiny.jpg")
											{
												// the most important thing, rename the Page.dat -> Template.dat
												if (file.ToLower() == "page.dat")
													file = "Template.dat";
												ZipEntry e = new ZipEntry(String.Format("Templates\\{0}\\{1}", info["name"], file));
												zout.PutNextEntry(e);
												int size;
												byte[] data = new byte[4096];
												while ((size = zip.Read(data, 0, data.Length)) > 0)
													zout.Write(data, 0, size);
												zout.CloseEntry();
											}
										}
									}
								}
								zip.Close();
							}
						}
						zout.Close();
					}
					os.Close();
				}
			}
			catch (Exception ex)
			{
				SetStatusText(ex.Message);
				Thread.Sleep(1000);
			}
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				new Thread(() =>
				{
					EnableForm(false);
					foreach (string file in files)
					{
						if (Path.GetExtension(file).ToLower() == ".zip" && File.Exists(file))
							ImportPackage(file);
					}
					EnableForm(true);
					SetStatusText(String.Empty);
				}).Start();
			}
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			bool result = true;
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				foreach (string file in files)
				{
					if (Path.GetExtension(file).ToLower() != ".zip")
						result = false;
				}
			}
			else
				result = false;

			if (result)
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}
	}
}
