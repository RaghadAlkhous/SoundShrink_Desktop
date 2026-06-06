using System;
using System.Drawing;
using System.Windows.Forms;

namespace SoundShrink_Desktop
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Sidebar elements
        private Panel panelSidebar;
        private Label lblAudioLab;
        private Label lblAudioLabSub;
        private Button btnChooseFile;
        private Button btnCompressFile;
        private Button btnShowChart;
        private Button btnDecompress;
        private Button btnResetWorkspace;
        private Label lblCompressionSettings;
        private ComboBox cmbAlgorithm;
        private Label lblSampleRateLabel;
        private ComboBox cmbSampleRate;

        // Common compression controls
        private Label lblQuantLevels;
        private ComboBox cmbQuantLevels;
        private Label lblDeltaStep;
        private NumericUpDown numDeltaStep;

        // DPCM controls
        private Label lblBitsPerSampleComp;
        private ComboBox cmbBitsPerSampleComp;

        // Predictive Differential Coding & Delta Modulation controls
        private Label lblPredictionCoeff;
        private TrackBar trkStepSize;
        private Label lblStepSizeValue;

        private Label lblInitialStep;
        private TrackBar trkInitialStep;
        private Label lblInitialStepValue;
        private Label lblStepMultiplier;
        private TrackBar trkStepMultiplier;
        private Label lblStepMultiplierValue;

        // Main area elements
        private Panel panelMain;
        private Label lblMainTitle;
        private Label lblMainSubtitle;
        private Panel panelFileLoad;
        private Panel panelFileInfo;
        private Label lblFileLoadText;
        private Label lblFileName;
        private ProgressBar progressBarMain;
        private Label lblPlaybackStatus;
        private Panel panelAudioProperties;
        private Label lblAudioPropertiesTitle;
        private Label lblFileSizeLabel;
        private Label lblFileSizeValue;
        private Label lblDurationLabel;
        private Label lblDurationValue;
        private Label lblSampleRatePropLabel;
        private Label lblSampleRatePropValue;
        private Label lblChannelsLabel;
        private Label lblChannelsValue;
        private Label lblBitRateLabel;
        private Label lblBitRateValue;
        private Label lblCodecLabel;
        private Label lblCodecValue;
        private Label lblBitsPerSampleLabel;
        private Label lblBitsPerSampleValue;
        private Panel panelOperationReport;
        private Label lblOperationReportTitle;
        private Label lblReportContent;
        private Panel panelCompressionRatio;
        private Label lblCompressionRatioTitle;
        private ProgressBar progressBarCompression;
        private Panel panelProcessingSpeed;
        private Label lblProcessingSpeedTitle;
        private ProgressBar progressBarSpeed;
        private Button btnChangeFile;

        // Modern player controls
        private Panel pnlTimeDisplay;
        private Panel pnlPlayerControls;
        private Button btnPrevious;
        private Button btnPlayPause;
        private Button btnNext;
        private Label lblCurrentTime;
        private Label lblRemainingTime;

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
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.lblAudioLab = new System.Windows.Forms.Label();
            this.lblAudioLabSub = new System.Windows.Forms.Label();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.btnChangeFile = new System.Windows.Forms.Button();
            this.btnCompressFile = new System.Windows.Forms.Button();
            this.btnShowChart = new System.Windows.Forms.Button();
            this.btnDecompress = new System.Windows.Forms.Button();
            this.btnResetWorkspace = new System.Windows.Forms.Button();
            this.lblCompressionSettings = new System.Windows.Forms.Label();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.lblSampleRateLabel = new System.Windows.Forms.Label();
            this.cmbSampleRate = new System.Windows.Forms.ComboBox();
            this.lblQuantLevels = new System.Windows.Forms.Label();
            this.cmbQuantLevels = new System.Windows.Forms.ComboBox();
            this.lblDeltaStep = new System.Windows.Forms.Label();
            this.numDeltaStep = new System.Windows.Forms.NumericUpDown();
            this.lblBitsPerSampleComp = new System.Windows.Forms.Label();
            this.cmbBitsPerSampleComp = new System.Windows.Forms.ComboBox();
            this.lblPredictionCoeff = new System.Windows.Forms.Label();
            this.trkStepSize = new System.Windows.Forms.TrackBar();
            this.lblStepSizeValue = new System.Windows.Forms.Label();
            this.lblInitialStep = new System.Windows.Forms.Label();
            this.trkInitialStep = new System.Windows.Forms.TrackBar();
            this.lblInitialStepValue = new System.Windows.Forms.Label();
            this.lblStepMultiplier = new System.Windows.Forms.Label();
            this.trkStepMultiplier = new System.Windows.Forms.TrackBar();
            this.lblStepMultiplierValue = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.lblMainTitle = new System.Windows.Forms.Label();
            this.lblMainSubtitle = new System.Windows.Forms.Label();
            this.panelFileLoad = new System.Windows.Forms.Panel();
            this.panelFileInfo = new System.Windows.Forms.Panel();
            this.lblFileLoadText = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.progressBarMain = new System.Windows.Forms.ProgressBar();
            this.lblPlaybackStatus = new System.Windows.Forms.Label();
            this.pnlTimeDisplay = new System.Windows.Forms.Panel();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblRemainingTime = new System.Windows.Forms.Label();
            this.pnlPlayerControls = new System.Windows.Forms.Panel();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.panelAudioProperties = new System.Windows.Forms.Panel();
            this.lblAudioPropertiesTitle = new System.Windows.Forms.Label();
            this.lblFileSizeLabel = new System.Windows.Forms.Label();
            this.lblFileSizeValue = new System.Windows.Forms.Label();
            this.lblDurationLabel = new System.Windows.Forms.Label();
            this.lblDurationValue = new System.Windows.Forms.Label();
            this.lblSampleRatePropLabel = new System.Windows.Forms.Label();
            this.lblSampleRatePropValue = new System.Windows.Forms.Label();
            this.lblChannelsLabel = new System.Windows.Forms.Label();
            this.lblChannelsValue = new System.Windows.Forms.Label();
            this.lblBitRateLabel = new System.Windows.Forms.Label();
            this.lblBitRateValue = new System.Windows.Forms.Label();
            this.lblCodecLabel = new System.Windows.Forms.Label();
            this.lblCodecValue = new System.Windows.Forms.Label();
            this.lblBitsPerSampleLabel = new System.Windows.Forms.Label();
            this.lblBitsPerSampleValue = new System.Windows.Forms.Label();
            this.panelOperationReport = new System.Windows.Forms.Panel();
            this.lblOperationReportTitle = new System.Windows.Forms.Label();
            this.lblReportContent = new System.Windows.Forms.Label();
            this.panelCompressionRatio = new System.Windows.Forms.Panel();
            this.lblCompressionRatioTitle = new System.Windows.Forms.Label();
            this.progressBarCompression = new System.Windows.Forms.ProgressBar();
            this.panelProcessingSpeed = new System.Windows.Forms.Panel();
            this.lblProcessingSpeedTitle = new System.Windows.Forms.Label();
            this.progressBarSpeed = new System.Windows.Forms.ProgressBar();

            ((System.ComponentModel.ISupportInitialize)(this.numDeltaStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkStepSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkInitialStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkStepMultiplier)).BeginInit();

            this.panelSidebar.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelFileLoad.SuspendLayout();
            this.pnlTimeDisplay.SuspendLayout();
            this.pnlPlayerControls.SuspendLayout();
            this.panelAudioProperties.SuspendLayout();
            this.panelOperationReport.SuspendLayout();
            this.panelCompressionRatio.SuspendLayout();
            this.panelProcessingSpeed.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.panelSidebar.AutoScroll = true;
            this.panelSidebar.Controls.Add(this.lblAudioLab);
            this.panelSidebar.Controls.Add(this.lblAudioLabSub);
            this.panelSidebar.Controls.Add(this.btnChooseFile);
            this.panelSidebar.Controls.Add(this.btnResetWorkspace);
            this.panelSidebar.Controls.Add(this.lblCompressionSettings);
            this.panelSidebar.Controls.Add(this.cmbAlgorithm);
            this.panelSidebar.Controls.Add(this.lblSampleRateLabel);
            this.panelSidebar.Controls.Add(this.cmbSampleRate);
            this.panelSidebar.Controls.Add(this.lblQuantLevels);
            this.panelSidebar.Controls.Add(this.cmbQuantLevels);
            this.panelSidebar.Controls.Add(this.lblDeltaStep);
            this.panelSidebar.Controls.Add(this.numDeltaStep);
            this.panelSidebar.Controls.Add(this.lblBitsPerSampleComp);
            this.panelSidebar.Controls.Add(this.cmbBitsPerSampleComp);
            this.panelSidebar.Controls.Add(this.lblPredictionCoeff);
            this.panelSidebar.Controls.Add(this.trkStepSize);
            this.panelSidebar.Controls.Add(this.lblStepSizeValue);
            this.panelSidebar.Controls.Add(this.lblInitialStep);
            this.panelSidebar.Controls.Add(this.trkInitialStep);
            this.panelSidebar.Controls.Add(this.lblInitialStepValue);
            this.panelSidebar.Controls.Add(this.lblStepMultiplier);
            this.panelSidebar.Controls.Add(this.trkStepMultiplier);
            this.panelSidebar.Controls.Add(this.lblStepMultiplierValue);
            this.panelSidebar.Controls.Add(this.btnCompressFile);
            this.panelSidebar.Controls.Add(this.btnShowChart);
            this.panelSidebar.Controls.Add(this.btnDecompress);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(230, 780);
            this.panelSidebar.TabIndex = 0;
            // 
            // lblAudioLab
            // 
            this.lblAudioLab.AutoSize = false;
            this.lblAudioLab.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblAudioLab.ForeColor = System.Drawing.Color.White;
            this.lblAudioLab.Location = new System.Drawing.Point(35, 8);
            this.lblAudioLab.Name = "lblAudioLab";
            this.lblAudioLab.Size = new System.Drawing.Size(180, 40);
            this.lblAudioLab.TabIndex = 0;
            this.lblAudioLab.Text = "AudioLab";
            // 
            // lblAudioLabSub
            // 
            this.lblAudioLabSub.AutoSize = false;
            this.lblAudioLabSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAudioLabSub.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblAudioLabSub.Location = new System.Drawing.Point(35, 60);
            this.lblAudioLabSub.Name = "lblAudioLabSub";
            this.lblAudioLabSub.Size = new System.Drawing.Size(180, 20);
            this.lblAudioLabSub.TabIndex = 1;
            this.lblAudioLabSub.Text = "Audio Compression Desktop App";
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnChooseFile.FlatAppearance.BorderSize = 0;
            this.btnChooseFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseFile.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnChooseFile.ForeColor = System.Drawing.Color.White;
            this.btnChooseFile.Location = new System.Drawing.Point(20, 110);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(190, 45);
            this.btnChooseFile.TabIndex = 2;
            this.btnChooseFile.Text = "Choose Audio File";
            this.btnChooseFile.UseVisualStyleBackColor = false;
            this.btnChooseFile.Click += new System.EventHandler(this.btnChooseFile_Click);
            // 
            // btnResetWorkspace
            // 
            this.btnResetWorkspace.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnResetWorkspace.FlatAppearance.BorderSize = 0;
            this.btnResetWorkspace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetWorkspace.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnResetWorkspace.ForeColor = System.Drawing.Color.White;
            this.btnResetWorkspace.Location = new System.Drawing.Point(20, 165);
            this.btnResetWorkspace.Name = "btnResetWorkspace";
            this.btnResetWorkspace.Size = new System.Drawing.Size(190, 45);
            this.btnResetWorkspace.TabIndex = 15;
            this.btnResetWorkspace.Text = "Reset Workspace";
            this.btnResetWorkspace.UseVisualStyleBackColor = false;
            this.btnResetWorkspace.Click += new System.EventHandler(this.btnResetWorkspace_Click);
            // 
            // lblCompressionSettings
            // 
            this.lblCompressionSettings.AutoSize = false;
            this.lblCompressionSettings.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblCompressionSettings.ForeColor = System.Drawing.Color.White;
            this.lblCompressionSettings.Location = new System.Drawing.Point(20, 240);
            this.lblCompressionSettings.Name = "lblCompressionSettings";
            this.lblCompressionSettings.Size = new System.Drawing.Size(190, 25);
            this.lblCompressionSettings.TabIndex = 5;
            this.lblCompressionSettings.Text = "Compression Settings";
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.cmbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlgorithm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbAlgorithm.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbAlgorithm.ForeColor = System.Drawing.Color.White;
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Items.AddRange(new object[] {
            "Nonlinear Quantization",
            "DPCM",
            "Predictive Differential Coding",
            "Delta Modulation",
            "Adaptive Delta Modulation"});
            this.cmbAlgorithm.Location = new System.Drawing.Point(20, 275);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(190, 31);
            this.cmbAlgorithm.TabIndex = 6;
            // 
            // lblSampleRateLabel
            // 
            this.lblSampleRateLabel.AutoSize = false;
            this.lblSampleRateLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSampleRateLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblSampleRateLabel.Location = new System.Drawing.Point(20, 315);
            this.lblSampleRateLabel.Name = "lblSampleRateLabel";
            this.lblSampleRateLabel.Size = new System.Drawing.Size(190, 18);
            this.lblSampleRateLabel.TabIndex = 7;
            this.lblSampleRateLabel.Text = "Sample Rate";
            // 
            // cmbSampleRate
            // 
            this.cmbSampleRate.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.cmbSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSampleRate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSampleRate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbSampleRate.ForeColor = System.Drawing.Color.White;
            this.cmbSampleRate.FormattingEnabled = true;
            this.cmbSampleRate.Items.AddRange(new object[] {
            "Original",
            "8000",
            "16000",
            "22050",
            "44100",
            "48000"});
            this.cmbSampleRate.Location = new System.Drawing.Point(20, 337);
            this.cmbSampleRate.Name = "cmbSampleRate";
            this.cmbSampleRate.Size = new System.Drawing.Size(190, 31);
            this.cmbSampleRate.TabIndex = 8;
            // 
            // lblQuantLevels
            // 
            this.lblQuantLevels.AutoSize = false;
            this.lblQuantLevels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblQuantLevels.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblQuantLevels.Location = new System.Drawing.Point(20, 380);
            this.lblQuantLevels.Name = "lblQuantLevels";
            this.lblQuantLevels.Size = new System.Drawing.Size(190, 18);
            this.lblQuantLevels.TabIndex = 9;
            this.lblQuantLevels.Text = "Quantization Levels";
            // 
            // cmbQuantLevels
            // 
            this.cmbQuantLevels.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.cmbQuantLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQuantLevels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbQuantLevels.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbQuantLevels.ForeColor = System.Drawing.Color.White;
            this.cmbQuantLevels.FormattingEnabled = true;
            this.cmbQuantLevels.Items.AddRange(new object[] {
            "16",
            "32",
            "64",
            "128",
            "256"});
            this.cmbQuantLevels.Location = new System.Drawing.Point(20, 402);
            this.cmbQuantLevels.Name = "cmbQuantLevels";
            this.cmbQuantLevels.Size = new System.Drawing.Size(190, 31);
            this.cmbQuantLevels.TabIndex = 10;
            // 
            // lblDeltaStep
            // 
            this.lblDeltaStep.AutoSize = false;
            this.lblDeltaStep.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDeltaStep.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblDeltaStep.Location = new System.Drawing.Point(20, 380);
            this.lblDeltaStep.Name = "lblDeltaStep";
            this.lblDeltaStep.Size = new System.Drawing.Size(190, 18);
            this.lblDeltaStep.TabIndex = 11;
            this.lblDeltaStep.Text = "Step Size (0.01 - 1.00)";
            this.lblDeltaStep.Visible = false;
            // 
            // numDeltaStep
            // 
            this.numDeltaStep.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.numDeltaStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numDeltaStep.DecimalPlaces = 3;
            this.numDeltaStep.ForeColor = System.Drawing.Color.White;
            this.numDeltaStep.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            this.numDeltaStep.Location = new System.Drawing.Point(20, 402);
            this.numDeltaStep.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numDeltaStep.Minimum = new decimal(new int[] { 1, 0, 0, 196608 });
            this.numDeltaStep.Name = "numDeltaStep";
            this.numDeltaStep.Size = new System.Drawing.Size(190, 27);
            this.numDeltaStep.TabIndex = 12;
            this.numDeltaStep.Value = new decimal(new int[] { 5, 0, 0, 131072 });
            this.numDeltaStep.Visible = false;
            // 
            // lblBitsPerSampleComp
            // 
            this.lblBitsPerSampleComp.AutoSize = false;
            this.lblBitsPerSampleComp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBitsPerSampleComp.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblBitsPerSampleComp.Location = new System.Drawing.Point(20, 380);
            this.lblBitsPerSampleComp.Name = "lblBitsPerSampleComp";
            this.lblBitsPerSampleComp.Size = new System.Drawing.Size(190, 18);
            this.lblBitsPerSampleComp.TabIndex = 16;
            this.lblBitsPerSampleComp.Text = "Bits Per Sample";
            this.lblBitsPerSampleComp.Visible = false;
            // 
            // cmbBitsPerSampleComp
            // 
            this.cmbBitsPerSampleComp.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.cmbBitsPerSampleComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBitsPerSampleComp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBitsPerSampleComp.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbBitsPerSampleComp.ForeColor = System.Drawing.Color.White;
            this.cmbBitsPerSampleComp.FormattingEnabled = true;
            this.cmbBitsPerSampleComp.Items.AddRange(new object[] {
            "4",
            "8",
            "12",
            "16",
            "24",
            "32"});
            this.cmbBitsPerSampleComp.Location = new System.Drawing.Point(20, 402);
            this.cmbBitsPerSampleComp.Name = "cmbBitsPerSampleComp";
            this.cmbBitsPerSampleComp.Size = new System.Drawing.Size(190, 31);
            this.cmbBitsPerSampleComp.TabIndex = 17;
            this.cmbBitsPerSampleComp.Visible = false;
            // 
            // lblPredictionCoeff
            // 
            this.lblPredictionCoeff.AutoSize = false;
            this.lblPredictionCoeff.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPredictionCoeff.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblPredictionCoeff.Location = new System.Drawing.Point(20, 380);
            this.lblPredictionCoeff.Name = "lblPredictionCoeff";
            this.lblPredictionCoeff.Size = new System.Drawing.Size(190, 18);
            this.lblPredictionCoeff.TabIndex = 18;
            this.lblPredictionCoeff.Text = "Step Size (0.01 - 1.00)";
            this.lblPredictionCoeff.Visible = false;
            // 
            // trkStepSize
            // 
            this.trkStepSize.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.trkStepSize.LargeChange = 10;
            this.trkStepSize.Location = new System.Drawing.Point(20, 402);
            this.trkStepSize.Maximum = 100;
            this.trkStepSize.Minimum = 1;
            this.trkStepSize.Name = "trkStepSize";
            this.trkStepSize.Size = new System.Drawing.Size(190, 45);
            this.trkStepSize.SmallChange = 1;
            this.trkStepSize.TabIndex = 19;
            this.trkStepSize.TickFrequency = 10;
            this.trkStepSize.Value = 10;
            this.trkStepSize.Visible = false;
            this.trkStepSize.ValueChanged += new System.EventHandler(this.trkStepSize_ValueChanged);
            // 
            // lblStepSizeValue
            // 
            this.lblStepSizeValue.AutoSize = false;
            this.lblStepSizeValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStepSizeValue.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.lblStepSizeValue.Location = new System.Drawing.Point(20, 445);
            this.lblStepSizeValue.Name = "lblStepSizeValue";
            this.lblStepSizeValue.Size = new System.Drawing.Size(190, 20);
            this.lblStepSizeValue.TabIndex = 25;
            this.lblStepSizeValue.Text = "0.100";
            this.lblStepSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStepSizeValue.Visible = false;
            // 
            // lblInitialStep
            // 
            this.lblInitialStep.AutoSize = false;
            this.lblInitialStep.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInitialStep.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblInitialStep.Location = new System.Drawing.Point(20, 380);
            this.lblInitialStep.Name = "lblInitialStep";
            this.lblInitialStep.Size = new System.Drawing.Size(190, 18);
            this.lblInitialStep.TabIndex = 20;
            this.lblInitialStep.Text = "Initial Step Size (0.01 - 1.00)";
            this.lblInitialStep.Visible = false;
            // 
            // trkInitialStep
            // 
            this.trkInitialStep.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.trkInitialStep.LargeChange = 10;
            this.trkInitialStep.Location = new System.Drawing.Point(20, 402);
            this.trkInitialStep.Maximum = 100;
            this.trkInitialStep.Minimum = 1;
            this.trkInitialStep.Name = "trkInitialStep";
            this.trkInitialStep.Size = new System.Drawing.Size(190, 45);
            this.trkInitialStep.SmallChange = 1;
            this.trkInitialStep.TabIndex = 21;
            this.trkInitialStep.TickFrequency = 10;
            this.trkInitialStep.Value = 10;
            this.trkInitialStep.Visible = false;
            this.trkInitialStep.ValueChanged += new System.EventHandler(this.trkInitialStep_ValueChanged);
            // 
            // lblInitialStepValue
            // 
            this.lblInitialStepValue.AutoSize = false;
            this.lblInitialStepValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblInitialStepValue.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.lblInitialStepValue.Location = new System.Drawing.Point(20, 445);
            this.lblInitialStepValue.Name = "lblInitialStepValue";
            this.lblInitialStepValue.Size = new System.Drawing.Size(190, 20);
            this.lblInitialStepValue.TabIndex = 22;
            this.lblInitialStepValue.Text = "0.100";
            this.lblInitialStepValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblInitialStepValue.Visible = false;
            // 
            // lblStepMultiplier
            // 
            this.lblStepMultiplier.AutoSize = false;
            this.lblStepMultiplier.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStepMultiplier.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblStepMultiplier.Location = new System.Drawing.Point(20, 470);
            this.lblStepMultiplier.Name = "lblStepMultiplier";
            this.lblStepMultiplier.Size = new System.Drawing.Size(190, 18);
            this.lblStepMultiplier.TabIndex = 23;
            this.lblStepMultiplier.Text = "Step Multiplier (1.1 - 2.0)";
            this.lblStepMultiplier.Visible = false;
            // 
            // trkStepMultiplier
            // 
            this.trkStepMultiplier.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.trkStepMultiplier.LargeChange = 5;
            this.trkStepMultiplier.Location = new System.Drawing.Point(20, 492);
            this.trkStepMultiplier.Maximum = 20;
            this.trkStepMultiplier.Minimum = 11;
            this.trkStepMultiplier.Name = "trkStepMultiplier";
            this.trkStepMultiplier.Size = new System.Drawing.Size(190, 45);
            this.trkStepMultiplier.SmallChange = 1;
            this.trkStepMultiplier.TabIndex = 24;
            this.trkStepMultiplier.TickFrequency = 1;
            this.trkStepMultiplier.Value = 15;
            this.trkStepMultiplier.Visible = false;
            this.trkStepMultiplier.ValueChanged += new System.EventHandler(this.trkStepMultiplier_ValueChanged);
            // 
            // lblStepMultiplierValue
            // 
            this.lblStepMultiplierValue.AutoSize = false;
            this.lblStepMultiplierValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStepMultiplierValue.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.lblStepMultiplierValue.Location = new System.Drawing.Point(20, 535);
            this.lblStepMultiplierValue.Name = "lblStepMultiplierValue";
            this.lblStepMultiplierValue.Size = new System.Drawing.Size(190, 20);
            this.lblStepMultiplierValue.TabIndex = 26;
            this.lblStepMultiplierValue.Text = "1.50";
            this.lblStepMultiplierValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStepMultiplierValue.Visible = false;
            // 
            // btnCompressFile
            // 
            this.btnCompressFile.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnCompressFile.FlatAppearance.BorderSize = 0;
            this.btnCompressFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompressFile.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCompressFile.ForeColor = System.Drawing.Color.White;
            this.btnCompressFile.Location = new System.Drawing.Point(20, 580);
            this.btnCompressFile.Name = "btnCompressFile";
            this.btnCompressFile.Size = new System.Drawing.Size(190, 45);
            this.btnCompressFile.TabIndex = 13;
            this.btnCompressFile.Text = "Compress File";
            this.btnCompressFile.UseVisualStyleBackColor = false;
            this.btnCompressFile.Click += new System.EventHandler(this.btnCompressFile_Click);
            // 
            // btnShowChart
            // 
            this.btnShowChart.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnShowChart.Enabled = false;
            this.btnShowChart.FlatAppearance.BorderSize = 0;
            this.btnShowChart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(10, 150, 100);
            this.btnShowChart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(20, 210, 150);
            this.btnShowChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowChart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnShowChart.ForeColor = System.Drawing.Color.White;
            this.btnShowChart.Location = new System.Drawing.Point(20, 635);
            this.btnShowChart.Name = "btnShowChart";
            this.btnShowChart.Size = new System.Drawing.Size(190, 40);
            this.btnShowChart.TabIndex = 27;
            this.btnShowChart.Text = "📊 Show Chart";
            this.btnShowChart.UseVisualStyleBackColor = false;
            this.btnShowChart.Click += new System.EventHandler(this.btnShowChart_Click);
            // 
            // btnDecompress
            // 
            this.btnDecompress.BackColor = System.Drawing.Color.FromArgb(6, 182, 212);
            this.btnDecompress.FlatAppearance.BorderSize = 0;
            this.btnDecompress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDecompress.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnDecompress.ForeColor = System.Drawing.Color.White;
            this.btnDecompress.Location = new System.Drawing.Point(20, 685);
            this.btnDecompress.Name = "btnDecompress";
            this.btnDecompress.Size = new System.Drawing.Size(190, 45);
            this.btnDecompress.TabIndex = 14;
            this.btnDecompress.Text = "Decompress ACMP";
            this.btnDecompress.UseVisualStyleBackColor = false;
            this.btnDecompress.Click += new System.EventHandler(this.btnDecompress_Click);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(10, 22, 40);
            this.panelMain.Controls.Add(this.lblMainTitle);
            this.panelMain.Controls.Add(this.lblMainSubtitle);
            this.panelMain.Controls.Add(this.panelFileLoad);
            this.panelMain.Controls.Add(this.panelAudioProperties);
            this.panelMain.Controls.Add(this.panelOperationReport);
            this.panelMain.Controls.Add(this.panelCompressionRatio);
            this.panelMain.Controls.Add(this.panelProcessingSpeed);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(230, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1180, 780);
            this.panelMain.TabIndex = 1;
            // 
            // lblMainTitle
            // 
            this.lblMainTitle.AutoSize = false;
            this.lblMainTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblMainTitle.ForeColor = System.Drawing.Color.White;
            this.lblMainTitle.Location = new System.Drawing.Point(20, 10);
            this.lblMainTitle.Name = "lblMainTitle";
            this.lblMainTitle.Size = new System.Drawing.Size(800, 40);
            this.lblMainTitle.TabIndex = 0;
            this.lblMainTitle.Text = "Universal Audio Compression Workspace";
            // 
            // lblMainSubtitle
            // 
            this.lblMainSubtitle.AutoSize = false;
            this.lblMainSubtitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblMainSubtitle.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblMainSubtitle.Location = new System.Drawing.Point(20, 60);
            this.lblMainSubtitle.Name = "lblMainSubtitle";
            this.lblMainSubtitle.Size = new System.Drawing.Size(800, 25);
            this.lblMainSubtitle.TabIndex = 1;
            this.lblMainSubtitle.Text = "Load, preview, configure, compress, monitor, cancel, decompress, reset, and save audio files.";
            // 
            // panelFileLoad
            // 
            this.panelFileLoad.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.panelFileLoad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFileLoad.Controls.Add(this.panelFileInfo);
            this.panelFileLoad.Controls.Add(this.lblPlaybackStatus);
            this.panelFileLoad.Controls.Add(this.pnlTimeDisplay);
            this.panelFileLoad.Controls.Add(this.progressBarMain);
            this.panelFileLoad.Controls.Add(this.pnlPlayerControls);
            this.panelFileLoad.Location = new System.Drawing.Point(20, 110);
            this.panelFileLoad.Name = "panelFileLoad";
            this.panelFileLoad.Size = new System.Drawing.Size(800, 320);
            this.panelFileLoad.TabIndex = 2;
            // 
            // panelFileInfo
            // 
            this.panelFileInfo.BackColor = System.Drawing.Color.FromArgb(30, 42, 80);
            this.panelFileInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFileInfo.Controls.Add(this.lblFileLoadText);
            this.panelFileInfo.Controls.Add(this.lblFileName);
            this.panelFileInfo.Location = new System.Drawing.Point(40, 20);
            this.panelFileInfo.Name = "panelFileInfo";
            this.panelFileInfo.Size = new System.Drawing.Size(720, 150);
            this.panelFileInfo.TabIndex = 0;
            // 
            // lblFileLoadText
            // 
            this.lblFileLoadText.AutoSize = false;
            this.lblFileLoadText.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblFileLoadText.ForeColor = System.Drawing.Color.White;
            this.lblFileLoadText.Location = new System.Drawing.Point(0, 20);
            this.lblFileLoadText.Name = "lblFileLoadText";
            this.lblFileLoadText.Size = new System.Drawing.Size(720, 50);
            this.lblFileLoadText.TabIndex = 0;
            this.lblFileLoadText.Text = "Drag or select an audio file";
            this.lblFileLoadText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = false;
            this.lblFileName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblFileName.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.lblFileName.Location = new System.Drawing.Point(0, 75);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(720, 35);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPlaybackStatus
            // 
            this.lblPlaybackStatus.AutoSize = false;
            this.lblPlaybackStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPlaybackStatus.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblPlaybackStatus.Location = new System.Drawing.Point(20, 290);
            this.lblPlaybackStatus.Name = "lblPlaybackStatus";
            this.lblPlaybackStatus.Size = new System.Drawing.Size(740, 20);
            this.lblPlaybackStatus.TabIndex = 2;
            this.lblPlaybackStatus.Text = "";
            this.lblPlaybackStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTimeDisplay
            // 
            this.pnlTimeDisplay.BackColor = System.Drawing.Color.Transparent;
            this.pnlTimeDisplay.Controls.Add(this.lblCurrentTime);
            this.pnlTimeDisplay.Controls.Add(this.lblRemainingTime);
            this.pnlTimeDisplay.Location = new System.Drawing.Point(20, 190);
            this.pnlTimeDisplay.Name = "pnlTimeDisplay";
            this.pnlTimeDisplay.Size = new System.Drawing.Size(770, 25);
            this.pnlTimeDisplay.TabIndex = 3;
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCurrentTime.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblCurrentTime.Location = new System.Drawing.Point(0, 3);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(35, 19);
            this.lblCurrentTime.TabIndex = 0;
            this.lblCurrentTime.Text = "0:00";
            // 
            // lblRemainingTime
            // 
            this.lblRemainingTime.AutoSize = true;
            this.lblRemainingTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRemainingTime.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblRemainingTime.Location = new System.Drawing.Point(715, 3);
            this.lblRemainingTime.Name = "lblRemainingTime";
            this.lblRemainingTime.Size = new System.Drawing.Size(45, 19);
            this.lblRemainingTime.TabIndex = 1;
            this.lblRemainingTime.Text = "-0:00";
            // 
            // progressBarMain
            // 
            this.progressBarMain.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.progressBarMain.Location = new System.Drawing.Point(20, 220);
            this.progressBarMain.Maximum = 100;
            this.progressBarMain.Name = "progressBarMain";
            this.progressBarMain.Size = new System.Drawing.Size(760, 8);
            this.progressBarMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarMain.TabIndex = 4;
            this.progressBarMain.Value = 0;
            // 
            // pnlPlayerControls
            // 
            this.pnlPlayerControls.BackColor = System.Drawing.Color.Transparent;
            this.pnlPlayerControls.Controls.Add(this.btnPrevious);
            this.pnlPlayerControls.Controls.Add(this.btnPlayPause);
            this.pnlPlayerControls.Controls.Add(this.btnNext);
            this.pnlPlayerControls.Location = new System.Drawing.Point(0, 220);
            this.pnlPlayerControls.Name = "pnlPlayerControls";
            this.pnlPlayerControls.Size = new System.Drawing.Size(800, 90);
            this.pnlPlayerControls.TabIndex = 5;
            // 
            // btnPrevious
            // 
            this.btnPrevious.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevious.FlatAppearance.BorderSize = 0;
            this.btnPrevious.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(40, 45, 55);
            this.btnPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(50, 55, 65);
            this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevious.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.btnPrevious.ForeColor = System.Drawing.Color.White;
            this.btnPrevious.Location = new System.Drawing.Point(305, 25);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(40, 40);
            this.btnPrevious.TabIndex = 1;
            this.btnPrevious.Text = "⏮";
            this.btnPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrevious.UseVisualStyleBackColor = false;
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.BackColor = System.Drawing.Color.Transparent;
            this.btnPlayPause.FlatAppearance.BorderSize = 0;
            this.btnPlayPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(40, 45, 55);
            this.btnPlayPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(50, 55, 65);
            this.btnPlayPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlayPause.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.btnPlayPause.ForeColor = System.Drawing.Color.White;
            this.btnPlayPause.Location = new System.Drawing.Point(385, 20);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(50, 50);
            this.btnPlayPause.TabIndex = 2;
            this.btnPlayPause.Text = "▶";
            this.btnPlayPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPlayPause.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(40, 45, 55);
            this.btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(50, 55, 65);
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Location = new System.Drawing.Point(465, 25);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(40, 40);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "⏭";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNext.UseVisualStyleBackColor = false;
            // 
            // panelAudioProperties
            // 
            this.panelAudioProperties.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.panelAudioProperties.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAudioProperties.Controls.Add(this.lblAudioPropertiesTitle);
            this.panelAudioProperties.Controls.Add(this.lblFileSizeLabel);
            this.panelAudioProperties.Controls.Add(this.lblFileSizeValue);
            this.panelAudioProperties.Controls.Add(this.lblDurationLabel);
            this.panelAudioProperties.Controls.Add(this.lblDurationValue);
            this.panelAudioProperties.Controls.Add(this.lblSampleRatePropLabel);
            this.panelAudioProperties.Controls.Add(this.lblSampleRatePropValue);
            this.panelAudioProperties.Controls.Add(this.lblChannelsLabel);
            this.panelAudioProperties.Controls.Add(this.lblChannelsValue);
            this.panelAudioProperties.Controls.Add(this.lblBitRateLabel);
            this.panelAudioProperties.Controls.Add(this.lblBitRateValue);
            this.panelAudioProperties.Controls.Add(this.lblCodecLabel);
            this.panelAudioProperties.Controls.Add(this.lblCodecValue);
            this.panelAudioProperties.Controls.Add(this.lblBitsPerSampleLabel);
            this.panelAudioProperties.Controls.Add(this.lblBitsPerSampleValue);
            this.panelAudioProperties.Location = new System.Drawing.Point(840, 110);
            this.panelAudioProperties.Name = "panelAudioProperties";
            this.panelAudioProperties.Size = new System.Drawing.Size(320, 320);
            this.panelAudioProperties.TabIndex = 3;
            // 
            // lblAudioPropertiesTitle
            // 
            this.lblAudioPropertiesTitle.AutoSize = false;
            this.lblAudioPropertiesTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblAudioPropertiesTitle.ForeColor = System.Drawing.Color.White;
            this.lblAudioPropertiesTitle.Location = new System.Drawing.Point(20, 15);
            this.lblAudioPropertiesTitle.Name = "lblAudioPropertiesTitle";
            this.lblAudioPropertiesTitle.Size = new System.Drawing.Size(280, 30);
            this.lblAudioPropertiesTitle.TabIndex = 0;
            this.lblAudioPropertiesTitle.Text = "Audio Properties";
            // 
            // lblFileSizeLabel
            // 
            this.lblFileSizeLabel.AutoSize = false;
            this.lblFileSizeLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFileSizeLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblFileSizeLabel.Location = new System.Drawing.Point(15, 60);
            this.lblFileSizeLabel.Name = "lblFileSizeLabel";
            this.lblFileSizeLabel.Size = new System.Drawing.Size(145, 20);
            this.lblFileSizeLabel.TabIndex = 1;
            this.lblFileSizeLabel.Text = "File Size";
            // 
            // lblFileSizeValue
            // 
            this.lblFileSizeValue.AutoSize = false;
            this.lblFileSizeValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFileSizeValue.ForeColor = System.Drawing.Color.White;
            this.lblFileSizeValue.Location = new System.Drawing.Point(165, 60);
            this.lblFileSizeValue.Name = "lblFileSizeValue";
            this.lblFileSizeValue.Size = new System.Drawing.Size(140, 20);
            this.lblFileSizeValue.TabIndex = 2;
            this.lblFileSizeValue.Text = "";
            this.lblFileSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDurationLabel
            // 
            this.lblDurationLabel.AutoSize = false;
            this.lblDurationLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDurationLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblDurationLabel.Location = new System.Drawing.Point(15, 95);
            this.lblDurationLabel.Name = "lblDurationLabel";
            this.lblDurationLabel.Size = new System.Drawing.Size(145, 20);
            this.lblDurationLabel.TabIndex = 3;
            this.lblDurationLabel.Text = "Duration";
            // 
            // lblDurationValue
            // 
            this.lblDurationValue.AutoSize = false;
            this.lblDurationValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDurationValue.ForeColor = System.Drawing.Color.White;
            this.lblDurationValue.Location = new System.Drawing.Point(165, 95);
            this.lblDurationValue.Name = "lblDurationValue";
            this.lblDurationValue.Size = new System.Drawing.Size(140, 20);
            this.lblDurationValue.TabIndex = 4;
            this.lblDurationValue.Text = "";
            this.lblDurationValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSampleRatePropLabel
            // 
            this.lblSampleRatePropLabel.AutoSize = false;
            this.lblSampleRatePropLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSampleRatePropLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblSampleRatePropLabel.Location = new System.Drawing.Point(15, 130);
            this.lblSampleRatePropLabel.Name = "lblSampleRatePropLabel";
            this.lblSampleRatePropLabel.Size = new System.Drawing.Size(145, 20);
            this.lblSampleRatePropLabel.TabIndex = 5;
            this.lblSampleRatePropLabel.Text = "Sample Rate";
            // 
            // lblSampleRatePropValue
            // 
            this.lblSampleRatePropValue.AutoSize = false;
            this.lblSampleRatePropValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSampleRatePropValue.ForeColor = System.Drawing.Color.White;
            this.lblSampleRatePropValue.Location = new System.Drawing.Point(165, 130);
            this.lblSampleRatePropValue.Name = "lblSampleRatePropValue";
            this.lblSampleRatePropValue.Size = new System.Drawing.Size(140, 20);
            this.lblSampleRatePropValue.TabIndex = 6;
            this.lblSampleRatePropValue.Text = "";
            this.lblSampleRatePropValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblChannelsLabel
            // 
            this.lblChannelsLabel.AutoSize = false;
            this.lblChannelsLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblChannelsLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblChannelsLabel.Location = new System.Drawing.Point(15, 165);
            this.lblChannelsLabel.Name = "lblChannelsLabel";
            this.lblChannelsLabel.Size = new System.Drawing.Size(145, 20);
            this.lblChannelsLabel.TabIndex = 7;
            this.lblChannelsLabel.Text = "Channels";
            // 
            // lblChannelsValue
            // 
            this.lblChannelsValue.AutoSize = false;
            this.lblChannelsValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChannelsValue.ForeColor = System.Drawing.Color.White;
            this.lblChannelsValue.Location = new System.Drawing.Point(165, 165);
            this.lblChannelsValue.Name = "lblChannelsValue";
            this.lblChannelsValue.Size = new System.Drawing.Size(140, 20);
            this.lblChannelsValue.TabIndex = 8;
            this.lblChannelsValue.Text = "";
            this.lblChannelsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBitRateLabel
            // 
            this.lblBitRateLabel.AutoSize = false;
            this.lblBitRateLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBitRateLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblBitRateLabel.Location = new System.Drawing.Point(15, 200);
            this.lblBitRateLabel.Name = "lblBitRateLabel";
            this.lblBitRateLabel.Size = new System.Drawing.Size(145, 20);
            this.lblBitRateLabel.TabIndex = 9;
            this.lblBitRateLabel.Text = "Bit Rate";
            // 
            // lblBitRateValue
            // 
            this.lblBitRateValue.AutoSize = false;
            this.lblBitRateValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBitRateValue.ForeColor = System.Drawing.Color.White;
            this.lblBitRateValue.Location = new System.Drawing.Point(165, 200);
            this.lblBitRateValue.Name = "lblBitRateValue";
            this.lblBitRateValue.Size = new System.Drawing.Size(140, 20);
            this.lblBitRateValue.TabIndex = 10;
            this.lblBitRateValue.Text = "";
            this.lblBitRateValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCodecLabel
            // 
            this.lblCodecLabel.AutoSize = false;
            this.lblCodecLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCodecLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblCodecLabel.Location = new System.Drawing.Point(15, 235);
            this.lblCodecLabel.Name = "lblCodecLabel";
            this.lblCodecLabel.Size = new System.Drawing.Size(145, 20);
            this.lblCodecLabel.TabIndex = 11;
            this.lblCodecLabel.Text = "Codec";
            // 
            // lblCodecValue
            // 
            this.lblCodecValue.AutoSize = false;
            this.lblCodecValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCodecValue.ForeColor = System.Drawing.Color.White;
            this.lblCodecValue.Location = new System.Drawing.Point(165, 235);
            this.lblCodecValue.Name = "lblCodecValue";
            this.lblCodecValue.Size = new System.Drawing.Size(140, 20);
            this.lblCodecValue.TabIndex = 12;
            this.lblCodecValue.Text = "";
            this.lblCodecValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBitsPerSampleLabel
            // 
            this.lblBitsPerSampleLabel.AutoSize = false;
            this.lblBitsPerSampleLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBitsPerSampleLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblBitsPerSampleLabel.Location = new System.Drawing.Point(15, 270);
            this.lblBitsPerSampleLabel.Name = "lblBitsPerSampleLabel";
            this.lblBitsPerSampleLabel.Size = new System.Drawing.Size(145, 20);
            this.lblBitsPerSampleLabel.TabIndex = 13;
            this.lblBitsPerSampleLabel.Text = "Bits/Sample";
            // 
            // lblBitsPerSampleValue
            // 
            this.lblBitsPerSampleValue.AutoSize = false;
            this.lblBitsPerSampleValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBitsPerSampleValue.ForeColor = System.Drawing.Color.White;
            this.lblBitsPerSampleValue.Location = new System.Drawing.Point(165, 270);
            this.lblBitsPerSampleValue.Name = "lblBitsPerSampleValue";
            this.lblBitsPerSampleValue.Size = new System.Drawing.Size(140, 20);
            this.lblBitsPerSampleValue.TabIndex = 14;
            this.lblBitsPerSampleValue.Text = "";
            this.lblBitsPerSampleValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelOperationReport
            // 
            this.panelOperationReport.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.panelOperationReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOperationReport.Controls.Add(this.lblOperationReportTitle);
            this.panelOperationReport.Controls.Add(this.lblReportContent);
            this.panelOperationReport.Location = new System.Drawing.Point(20, 450);
            this.panelOperationReport.Name = "panelOperationReport";
            this.panelOperationReport.Size = new System.Drawing.Size(800, 220);
            this.panelOperationReport.TabIndex = 4;
            // 
            // lblOperationReportTitle
            // 
            this.lblOperationReportTitle.AutoSize = false;
            this.lblOperationReportTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblOperationReportTitle.ForeColor = System.Drawing.Color.White;
            this.lblOperationReportTitle.Location = new System.Drawing.Point(20, 15);
            this.lblOperationReportTitle.Name = "lblOperationReportTitle";
            this.lblOperationReportTitle.Size = new System.Drawing.Size(300, 30);
            this.lblOperationReportTitle.TabIndex = 0;
            this.lblOperationReportTitle.Text = "Operation Report";
            // 
            // lblReportContent
            // 
            this.lblReportContent.AutoSize = false;
            this.lblReportContent.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblReportContent.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblReportContent.Location = new System.Drawing.Point(20, 55);
            this.lblReportContent.Name = "lblReportContent";
            this.lblReportContent.Size = new System.Drawing.Size(760, 150);
            this.lblReportContent.TabIndex = 1;
            this.lblReportContent.Text = "";
            // 
            // panelCompressionRatio
            // 
            this.panelCompressionRatio.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.panelCompressionRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCompressionRatio.Controls.Add(this.lblCompressionRatioTitle);
            this.panelCompressionRatio.Controls.Add(this.progressBarCompression);
            this.panelCompressionRatio.Location = new System.Drawing.Point(840, 450);
            this.panelCompressionRatio.Name = "panelCompressionRatio";
            this.panelCompressionRatio.Size = new System.Drawing.Size(320, 80);
            this.panelCompressionRatio.TabIndex = 5;
            // 
            // lblCompressionRatioTitle
            // 
            this.lblCompressionRatioTitle.AutoSize = false;
            this.lblCompressionRatioTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCompressionRatioTitle.ForeColor = System.Drawing.Color.White;
            this.lblCompressionRatioTitle.Location = new System.Drawing.Point(20, 15);
            this.lblCompressionRatioTitle.Name = "lblCompressionRatioTitle";
            this.lblCompressionRatioTitle.Size = new System.Drawing.Size(300, 25);
            this.lblCompressionRatioTitle.TabIndex = 0;
            this.lblCompressionRatioTitle.Text = "";
            // 
            // progressBarCompression
            // 
            this.progressBarCompression.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.progressBarCompression.Location = new System.Drawing.Point(20, 35);
            this.progressBarCompression.Name = "progressBarCompression";
            this.progressBarCompression.Size = new System.Drawing.Size(280, 25);
            this.progressBarCompression.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCompression.TabIndex = 1;
            // 
            // panelProcessingSpeed
            // 
            this.panelProcessingSpeed.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.panelProcessingSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProcessingSpeed.Controls.Add(this.lblProcessingSpeedTitle);
            this.panelProcessingSpeed.Controls.Add(this.progressBarSpeed);
            this.panelProcessingSpeed.Location = new System.Drawing.Point(840, 550);
            this.panelProcessingSpeed.Name = "panelProcessingSpeed";
            this.panelProcessingSpeed.Size = new System.Drawing.Size(320, 80);
            this.panelProcessingSpeed.TabIndex = 6;
            // 
            // lblProcessingSpeedTitle
            // 
            this.lblProcessingSpeedTitle.AutoSize = false;
            this.lblProcessingSpeedTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblProcessingSpeedTitle.ForeColor = System.Drawing.Color.White;
            this.lblProcessingSpeedTitle.Location = new System.Drawing.Point(20, 10);
            this.lblProcessingSpeedTitle.Name = "lblProcessingSpeedTitle";
            this.lblProcessingSpeedTitle.Size = new System.Drawing.Size(300, 25);
            this.lblProcessingSpeedTitle.TabIndex = 0;
            this.lblProcessingSpeedTitle.Text = "";
            // 
            // progressBarSpeed
            // 
            this.progressBarSpeed.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.progressBarSpeed.Location = new System.Drawing.Point(20, 40);
            this.progressBarSpeed.Name = "progressBarSpeed";
            this.progressBarSpeed.Size = new System.Drawing.Size(280, 20);
            this.progressBarSpeed.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarSpeed.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(10, 22, 40);
            this.ClientSize = new System.Drawing.Size(1410, 780);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Compression";

            ((System.ComponentModel.ISupportInitialize)(this.numDeltaStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkStepSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkInitialStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkStepMultiplier)).EndInit();

            this.panelSidebar.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelFileLoad.ResumeLayout(false);
            this.pnlTimeDisplay.ResumeLayout(false);
            this.pnlTimeDisplay.PerformLayout();
            this.pnlPlayerControls.ResumeLayout(false);
            this.panelAudioProperties.ResumeLayout(false);
            this.panelOperationReport.ResumeLayout(false);
            this.panelCompressionRatio.ResumeLayout(false);
            this.panelProcessingSpeed.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}