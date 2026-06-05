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
            this.progressBar = new System.Windows.Forms.TrackBar();
            this.dropPanel = new System.Windows.Forms.Panel();
            this.lblDropZoneMain = new System.Windows.Forms.Label();
            this.lblDropZoneSub = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.wavePictureBox = new System.Windows.Forms.PictureBox();
            this.spectrumPictureBox = new System.Windows.Forms.PictureBox();
            this.controlsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).BeginInit();
            this.dropPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wavePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.infoPanel.ColumnCount = 4;
            this.infoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.infoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.infoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.infoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoPanel.Location = new System.Drawing.Point(0, 400);
            this.infoPanel.Margin = new System.Windows.Forms.Padding(10);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(10);
            this.infoPanel.RowCount = 2;
            this.infoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.infoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.infoPanel.Size = new System.Drawing.Size(900, 100);
            this.infoPanel.TabIndex = 0;
            // 
            // lblSpectrumTitle
            // 
            this.lblSpectrumTitle.Location = new System.Drawing.Point(0, 0);
            this.lblSpectrumTitle.Name = "lblSpectrumTitle";
            this.lblSpectrumTitle.Size = new System.Drawing.Size(100, 23);
            this.lblSpectrumTitle.TabIndex = 0;
            // 
            // lblWaveformTitle
            // 
            this.lblWaveformTitle.Location = new System.Drawing.Point(0, 0);
            this.lblWaveformTitle.Name = "lblWaveformTitle";
            this.lblWaveformTitle.Size = new System.Drawing.Size(100, 23);
            this.lblWaveformTitle.TabIndex = 0;
            // 
            // wavePanel
            // 
            this.wavePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.wavePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.wavePanel.Location = new System.Drawing.Point(0, 200);
            this.wavePanel.Margin = new System.Windows.Forms.Padding(10);
            this.wavePanel.Name = "wavePanel";
            this.wavePanel.Size = new System.Drawing.Size(900, 200);
            this.wavePanel.TabIndex = 1;
            // 
            // spectrumPanel
            // 
            this.spectrumPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.spectrumPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.spectrumPanel.Location = new System.Drawing.Point(0, 0);
            this.spectrumPanel.Margin = new System.Windows.Forms.Padding(10);
            this.spectrumPanel.Name = "spectrumPanel";
            this.spectrumPanel.Size = new System.Drawing.Size(900, 200);
            this.spectrumPanel.TabIndex = 2;
            
            // 
            // controlsPanel
            // 
            this.controlsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.controlsPanel.Controls.Add(this.progressBar);
            this.controlsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlsPanel.Location = new System.Drawing.Point(0, 560);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.controlsPanel.Size = new System.Drawing.Size(900, 120);
            this.controlsPanel.TabIndex = 3;
            this.controlsPanel.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar.Location = new System.Drawing.Point(20, 10);
            this.progressBar.Maximum = 100;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(860, 56);
            this.progressBar.TabIndex = 0;
            this.progressBar.TickFrequency = 10;
            // 
            // dropPanel
            // 
            this.dropPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.dropPanel.Controls.Add(this.lblDropZoneMain);
            this.dropPanel.Controls.Add(this.lblDropZoneSub);
            this.dropPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dropPanel.Location = new System.Drawing.Point(0, 0);
            this.dropPanel.Margin = new System.Windows.Forms.Padding(10);
            this.dropPanel.Name = "dropPanel";
            this.dropPanel.Size = new System.Drawing.Size(900, 680);
            this.dropPanel.TabIndex = 4;
            this.dropPanel.Click += new System.EventHandler(this.DropZone_Click);
            this.dropPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.dropPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            // 
            // lblDropZoneMain
            // 
            this.lblDropZoneMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDropZoneMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDropZoneMain.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblDropZoneMain.ForeColor = System.Drawing.Color.White;
            this.lblDropZoneMain.Location = new System.Drawing.Point(0, 25);
            this.lblDropZoneMain.Margin = new System.Windows.Forms.Padding(0, 10, 0, 5);
            this.lblDropZoneMain.Name = "lblDropZoneMain";
            this.lblDropZoneMain.Size = new System.Drawing.Size(900, 40);
            this.lblDropZoneMain.TabIndex = 0;
            this.lblDropZoneMain.Text = "اسحب الملف الصوتي هنا أو انقر للاختيار";
            this.lblDropZoneMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDropZoneMain.Click += new System.EventHandler(this.DropZone_Click);
            // 
            // lblDropZoneSub
            // 
            this.lblDropZoneSub.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDropZoneSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDropZoneSub.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblDropZoneSub.ForeColor = System.Drawing.Color.Gray;
            this.lblDropZoneSub.Location = new System.Drawing.Point(0, 0);
            this.lblDropZoneSub.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblDropZoneSub.Name = "lblDropZoneSub";
            this.lblDropZoneSub.Size = new System.Drawing.Size(900, 25);
            this.lblDropZoneSub.TabIndex = 1;
            this.lblDropZoneSub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDropZoneSub.Click += new System.EventHandler(this.DropZone_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(0, 0);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 23);
            this.btnPlay.TabIndex = 0;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(0, 0);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(0, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 0;
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.Location = new System.Drawing.Point(0, 0);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(100, 23);
            this.lblCurrentTime.TabIndex = 0;
            // 
            // lblTotalTime
            // 
            this.lblTotalTime.Location = new System.Drawing.Point(0, 0);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(100, 23);
            this.lblTotalTime.TabIndex = 0;
            // 
            // wavePictureBox
            // 
            this.wavePictureBox.Location = new System.Drawing.Point(0, 0);
            this.wavePictureBox.Name = "wavePictureBox";
            this.wavePictureBox.Size = new System.Drawing.Size(100, 50);
            this.wavePictureBox.TabIndex = 0;
            this.wavePictureBox.TabStop = false;
            // 
            // spectrumPictureBox
            // 
            this.spectrumPictureBox.Location = new System.Drawing.Point(0, 0);
            this.spectrumPictureBox.Name = "spectrumPictureBox";
            this.spectrumPictureBox.Size = new System.Drawing.Size(100, 50);
            this.spectrumPictureBox.TabIndex = 0;
            this.spectrumPictureBox.TabStop = false;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.ClientSize = new System.Drawing.Size(900, 680);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this.wavePanel);
            this.Controls.Add(this.spectrumPanel);
            this.Controls.Add(this.controlsPanel);
            this.Controls.Add(this.dropPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SoundShrink Pro - ضغط الملفات الصوتية";
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).EndInit();
            this.dropPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.wavePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        // ✅ أضف هذه التعريفات في نهاية partial class Form1 في Form1.Designer.cs:

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nonlinearQuantizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dpcmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem predictiveCodingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deltaModulationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adaptiveDeltaModulationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCompressedToolStripMenuItem;


        private System.Windows.Forms.ToolStripMenuItem decompressToolStripMenuItem;
        #endregion
    }
}