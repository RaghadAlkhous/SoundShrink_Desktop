using System;
using System.Drawing;
using System.Windows.Forms;

namespace SoundShrink_Desktop
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel infoPanel;
        private System.Windows.Forms.Label lblSpectrumTitle;
        private System.Windows.Forms.Label lblWaveformTitle;
        private System.Windows.Forms.Panel wavePanel;
        private System.Windows.Forms.Panel spectrumPanel;
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.Panel dropPanel;

        private System.Windows.Forms.Label lblDropZoneMain;
        private System.Windows.Forms.Label lblDropZoneSub;

        private System.Windows.Forms.TrackBar progressBar;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblTotalTime;
        private System.Windows.Forms.PictureBox wavePictureBox;
        private System.Windows.Forms.PictureBox spectrumPictureBox;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.infoPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblSpectrumTitle = new System.Windows.Forms.Label();
            this.lblWaveformTitle = new System.Windows.Forms.Label();
            this.wavePanel = new System.Windows.Forms.Panel();
            this.spectrumPanel = new System.Windows.Forms.Panel();
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.dropPanel = new System.Windows.Forms.Panel();

            this.lblDropZoneMain = new System.Windows.Forms.Label();
            this.lblDropZoneSub = new System.Windows.Forms.Label();

            this.progressBar = new System.Windows.Forms.TrackBar();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.wavePictureBox = new System.Windows.Forms.PictureBox();
            this.spectrumPictureBox = new System.Windows.Forms.PictureBox();

            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wavePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumPictureBox)).BeginInit();
            this.SuspendLayout();

            this.Text = "SoundShrink Pro - ضغط الملفات الصوتية";
            this.BackColor = Color.FromArgb(30, 30, 35);
            this.Font = new Font("Segoe UI", 9F);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new System.Drawing.Size(900, 680);

            this.infoPanel.ColumnCount = 4;
            this.infoPanel.RowCount = 2;
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoPanel.Height = 100;
            this.infoPanel.BackColor = Color.FromArgb(45, 45, 50);
            this.infoPanel.Margin = new Padding(10);
            this.infoPanel.Padding = new Padding(10);
            this.infoPanel.ColumnStyles.Clear();
            this.infoPanel.RowStyles.Clear();
            this.infoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.infoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.infoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.infoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.infoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.infoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Controls.Add(this.infoPanel);

            this.wavePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.wavePanel.Height = 200;
            this.wavePanel.BackColor = Color.FromArgb(15, 15, 20);
            this.wavePanel.Margin = new Padding(10);
            this.Controls.Add(this.wavePanel);

            this.spectrumPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.spectrumPanel.Height = 200;
            this.spectrumPanel.BackColor = Color.FromArgb(15, 15, 20);
            this.spectrumPanel.Margin = new Padding(10);
            this.Controls.Add(this.spectrumPanel);

            this.controlsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlsPanel.Height = 120;
            this.controlsPanel.BackColor = Color.FromArgb(45, 45, 50);
            this.controlsPanel.Padding = new Padding(20, 10, 20, 10);
            this.controlsPanel.Visible = false;
            this.Controls.Add(this.controlsPanel);

            this.progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar.Height = 40;
            this.progressBar.Minimum = 0;
            this.progressBar.Maximum = 100;
            this.progressBar.TickFrequency = 10;
            this.progressBar.BackColor = Color.FromArgb(45, 45, 50);
            this.controlsPanel.Controls.Add(this.progressBar);


            // dropPanel 
            this.dropPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dropPanel.BackColor = Color.FromArgb(40, 40, 45);
            this.dropPanel.Margin = new Padding(10);
        
            this.dropPanel.Click += new System.EventHandler(this.DropZone_Click); 
            this.dropPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.dropPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.Controls.Add(this.dropPanel);

            this.lblDropZoneMain.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblDropZoneMain.ForeColor = Color.White;
            this.lblDropZoneMain.AutoSize = false;
            this.lblDropZoneMain.Size = new Size(400, 40);
            this.lblDropZoneMain.TextAlign = ContentAlignment.MiddleCenter;
            this.lblDropZoneMain.Dock = DockStyle.Top;
            this.lblDropZoneMain.Margin = new Padding(0, 10, 0, 5);
            this.lblDropZoneMain.Cursor = Cursors.Hand; 
            this.lblDropZoneMain.Click += new System.EventHandler(this.DropZone_Click);  
            this.dropPanel.Controls.Add(this.lblDropZoneMain);

            this.lblDropZoneSub.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.lblDropZoneSub.ForeColor = Color.Gray;
            this.lblDropZoneSub.AutoSize = false;
            this.lblDropZoneSub.Size = new Size(300, 25);
            this.lblDropZoneSub.TextAlign = ContentAlignment.MiddleCenter;
            this.lblDropZoneSub.Dock = DockStyle.Top;
            this.lblDropZoneSub.Margin = new Padding(0, 5, 0, 0);
            this.lblDropZoneSub.Cursor = Cursors.Hand;  
            this.lblDropZoneSub.Click += new System.EventHandler(this.DropZone_Click); 
            this.dropPanel.Controls.Add(this.lblDropZoneSub);

            this.lblDropZoneMain.Text = "اسحب الملف الصوتي هنا أو انقر للاختيار";
            this.lblDropZoneMain.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblDropZoneMain.ForeColor = Color.White;
            this.lblDropZoneMain.AutoSize = false;
            this.lblDropZoneMain.Size = new Size(400, 40);
            this.lblDropZoneMain.TextAlign = ContentAlignment.MiddleCenter;
            this.lblDropZoneMain.Dock = DockStyle.Top;
            this.lblDropZoneMain.Margin = new Padding(0, 10, 0, 5);
            this.dropPanel.Controls.Add(this.lblDropZoneMain);

            this.lblDropZoneSub.Margin = new Padding(0, 5, 0, 0);
            this.dropPanel.Controls.Add(this.lblDropZoneSub);

            this.dropPanel.BringToFront();

            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wavePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumPictureBox)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}