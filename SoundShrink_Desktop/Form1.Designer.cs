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

            // Form1 Settings
            this.Text = "SoundShrink Pro - ضغط الملفات الصوتية";
            this.BackColor = Color.FromArgb(30, 30, 35);
            this.Font = new Font("Segoe UI", 9F);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new System.Drawing.Size(900, 680);

            // infoPanel
            this.infoPanel.ColumnCount = 4;
            this.infoPanel.RowCount = 2;
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoPanel.Height = 100;
            this.infoPanel.BackColor = Color.FromArgb(45, 45, 50);
            this.infoPanel.Margin = new Padding(10);
            this.infoPanel.Padding = new Padding(10);
            this.infoPanel.ColumnStyles.Clear();
            this.infoPanel.RowStyles.Clear();

            for (int i = 0; i < 4; i++)
                this.infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            for (int i = 0; i < 2; i++)
                this.infoPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            this.Controls.Add(this.infoPanel);

            // wavePanel
            this.wavePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.wavePanel.Height = 200;
            this.wavePanel.BackColor = Color.FromArgb(15, 15, 20);
            this.wavePanel.Margin = new Padding(10);
            this.Controls.Add(this.wavePanel);

            // spectrumPanel
            this.spectrumPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.spectrumPanel.Height = 200;
            this.spectrumPanel.BackColor = Color.FromArgb(15, 15, 20);
            this.spectrumPanel.Margin = new Padding(10);
            this.Controls.Add(this.spectrumPanel);

            // controlsPanel
            this.controlsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlsPanel.Height = 120;
            this.controlsPanel.BackColor = Color.FromArgb(45, 45, 50);
            this.controlsPanel.Padding = new Padding(20, 10, 20, 10);
            this.controlsPanel.Visible = false;
            this.Controls.Add(this.controlsPanel);

            // progressBar
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
            this.Controls.Add(this.dropPanel);
            this.dropPanel.BringToFront();

            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wavePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumPictureBox)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}