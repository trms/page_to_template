namespace PageToTemplate
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.dropLabel = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.statusText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// dropLabel
			// 
			this.dropLabel.AllowDrop = true;
			this.dropLabel.AutoSize = true;
			this.dropLabel.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dropLabel.Location = new System.Drawing.Point(12, 9);
			this.dropLabel.Name = "dropLabel";
			this.dropLabel.Size = new System.Drawing.Size(350, 30);
			this.dropLabel.TabIndex = 0;
			this.dropLabel.Text = "Drag a Bulletin Package Here";
			this.dropLabel.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
			this.dropLabel.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 16);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(344, 23);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBar1.TabIndex = 1;
			this.progressBar1.Visible = false;
			// 
			// statusText
			// 
			this.statusText.AutoSize = true;
			this.statusText.Location = new System.Drawing.Point(12, 43);
			this.statusText.Name = "statusText";
			this.statusText.Size = new System.Drawing.Size(0, 13);
			this.statusText.TabIndex = 2;
			// 
			// Form1
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(368, 59);
			this.Controls.Add(this.statusText);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.dropLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Page To Template";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label dropLabel;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label statusText;
	}
}

