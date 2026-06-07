using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;
using SoundShrink_Desktop.Services;
using SoundShrink_Desktop.Algorithms;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace SoundShrink_Desktop
{
    public static class RoundedCornersHelper
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        public static void ApplyRoundedBorder(this Control control, int radius)
        {
            IntPtr ptr = CreateRoundRectRgn(0, 0, control.Width, control.Height, radius, radius);
            System.Drawing.Region region = System.Drawing.Region.FromHrgn(ptr);
            control.Region = region;
        }
    }

    public partial class Form1 : Form
    {
        private AudioFileReader _currentReader;
        private readonly AudioService _audioService;
        private WaveOutEvent _player;
        private AudioFileInfo _currentFile;
        private bool _isPaused;
        private DateTime _playbackStartTime;
        private TimeSpan _playbackOffset;
        private ICompressionAlgorithm _currentAlgorithm;
        private byte[] _compressedData;
        private CompressionResult _lastCompressionResult;
        private Timer _progressTimer;
        private string _originalFilePath;
        private string _decompressedTempFile;
        private bool _isDecompressedMode = false;
        private bool _isUserSeeking = false;
        private bool _isCompressionCancelled = false;
        private CompressionProgressForm _progressForm;
        private Timer _compressionMonitorTimer;
        private DateTime _compressionStartTime;
        private long _totalBytesToProcess;
        private long _processedBytes;

        private volatile int _realProgress;

        private List<float> _savedRatioHistory = new List<float>();
        private List<float> _savedSpeedHistory = new List<float>();
        private string _savedAlgorithmName = "";
        private TimeSpan _savedTotalTime = TimeSpan.Zero;
        private double _savedFinalRatio = 0;

        public Form1()
        {
            InitializeComponent();
            _audioService = new AudioService();
            SetupUI();
            SetupTimers();
            EnableDragDrop();
            InitializeEmptyState();
            ApplyRoundedCorners();
        }

        private void ApplyRoundedCorners()
        {
            panelFileInfo.ApplyRoundedBorder(20);
            panelFileLoad.ApplyRoundedBorder(20);
            panelAudioProperties.ApplyRoundedBorder(20);
            panelOperationReport.ApplyRoundedBorder(20);
            panelCompressionRatio.ApplyRoundedBorder(20);
            panelProcessingSpeed.ApplyRoundedBorder(20);

            btnChooseFile.ApplyRoundedBorder(10);
            btnCompressFile.ApplyRoundedBorder(10);
            btnDecompress.ApplyRoundedBorder(10);
            btnResetWorkspace.ApplyRoundedBorder(10);
            btnPrevious.ApplyRoundedBorder(20);
            btnPlayPause.ApplyRoundedBorder(25);
            btnNext.ApplyRoundedBorder(20);
        }

        private void InitializeEmptyState()
        {
            lblFileLoadText.Text = "Drag or select an audio file";
            lblFileName.Text = "";
            lblPlaybackStatus.Text = "";
            progressBarMain.Value = 0;

            lblFileSizeValue.Text = "";
            lblDurationValue.Text = "";
            lblSampleRatePropValue.Text = "";
            lblChannelsValue.Text = "";
            lblBitRateValue.Text = "";
            lblCodecValue.Text = "";
            lblBitsPerSampleValue.Text = "";

            lblReportContent.Text = "";
            lblCompressionRatioTitle.Text = "";
            lblProcessingSpeedTitle.Text = "";
            progressBarCompression.Value = 0;
            progressBarSpeed.Value = 0;

            lblCurrentTime.Text = "0:00";
            lblRemainingTime.Text = "-0:00";
        }

        private void SetupUI()
        {
            cmbAlgorithm.SelectedIndex = 0;
            cmbSampleRate.SelectedIndex = 0;

            if (cmbQuantLevels.Items.Count > 0)
                cmbQuantLevels.SelectedIndex = 4; 

            if (cmbBitsPerSampleComp.Items.Count > 0)
                cmbBitsPerSampleComp.SelectedIndex = 3;

            UpdateButtonStates(false);
            SetupModernControls();
            UpdateAlgorithmSettings();
        }

        private void SetupModernControls()
        {
            btnPrevious.Click += BtnPrevious_Click;
            btnPlayPause.Click += BtnPlayPause_Click;
            btnNext.Click += BtnNext_Click;

            // ✅ ربط حدث تغيير الخوارزمية
            cmbAlgorithm.SelectedIndexChanged += CmbAlgorithm_SelectedIndexChanged;

            btnPlayPause.Enabled = false;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;

            progressBarMain.MouseDown += ProgressBarMain_MouseDown;
            progressBarMain.MouseUp += ProgressBarMain_MouseUp;
        }

        private void UpdateAlgorithmSettings()
        {
            lblQuantLevels.Visible = false;
            cmbQuantLevels.Visible = false;
            lblDeltaStep.Visible = false;
            numDeltaStep.Visible = false;
            lblBitsPerSampleComp.Visible = false;
            cmbBitsPerSampleComp.Visible = false;
            lblPredictionCoeff.Visible = false;
            trkStepSize.Visible = false;
            lblStepSizeValue.Visible = false;
            lblInitialStep.Visible = false;
            trkInitialStep.Visible = false;          
            lblInitialStepValue.Visible = false;     
            lblStepMultiplier.Visible = false;
            trkStepMultiplier.Visible = false;       
            lblStepMultiplierValue.Visible = false;  

            string algoName = cmbAlgorithm.SelectedItem?.ToString() ?? "";

            switch (algoName)
            {
                case "Nonlinear Quantization":
                    lblQuantLevels.Visible = true;
                    cmbQuantLevels.Visible = true;
                    if (cmbQuantLevels.SelectedIndex == -1)
                        cmbQuantLevels.SelectedIndex = 4;
                    break;

                case "DPCM":
                    lblBitsPerSampleComp.Visible = true;
                    cmbBitsPerSampleComp.Visible = true;
                    if (cmbBitsPerSampleComp.SelectedIndex == -1)
                        cmbBitsPerSampleComp.SelectedIndex = 3;
                    break;

                case "Predictive Differential Coding":
                    lblPredictionCoeff.Visible = true;
                    trkStepSize.Visible = true;
                    lblStepSizeValue.Visible = true;
                    if (trkStepSize.Value == trkStepSize.Minimum)
                        trkStepSize.Value = 10;
                    lblStepSizeValue.Text = (trkStepSize.Value / 100.0).ToString("F3");
                    break;

                case "Delta Modulation":
                    lblDeltaStep.Visible = true;
                    trkStepSize.Visible = true;
                    lblStepSizeValue.Visible = true;
                    if (trkStepSize.Value == trkStepSize.Minimum)
                        trkStepSize.Value = 10;
                    lblStepSizeValue.Text = (trkStepSize.Value / 100.0).ToString("F3");
                    break;

                case "Adaptive Delta Modulation":
                    lblInitialStep.Visible = true;
                    trkInitialStep.Visible = true;          
                    lblInitialStepValue.Visible = true;     
                    lblStepMultiplier.Visible = true;
                    trkStepMultiplier.Visible = true;       
                    lblStepMultiplierValue.Visible = true;  

                    if (trkInitialStep.Value == trkInitialStep.Minimum)
                        trkInitialStep.Value = 10; 
                    lblInitialStepValue.Text = (trkInitialStep.Value / 100.0).ToString("F3");

                    if (trkStepMultiplier.Value == trkStepMultiplier.Minimum)
                        trkStepMultiplier.Value = 15; 
                    lblStepMultiplierValue.Text = (trkStepMultiplier.Value / 10.0).ToString("F2");
                    break;
            }
        }

        private void CmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAlgorithmSettings();
        }

        private void SetupTimers()
        {
            _progressTimer = new Timer { Interval = 200 };
            _progressTimer.Tick += ProgressTimer_Tick;
        }

        private void EnableDragDrop()
        {
            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;
        }

        private void UpdateButtonStates(bool hasFile)
        {
            btnCompressFile.Enabled = hasFile;
            btnPlayPause.Enabled = hasFile;
            btnPrevious.Enabled = hasFile;
            btnNext.Enabled = hasFile;
        }

        private bool IsAudioFile(string path)
        {
            string ext = Path.GetExtension(path).ToLower();
            return ext == ".wav" || ext == ".mp3";
        }

        private void OpenFileBrowser()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.wav;*.mp3",
                Title = "Choose Audio File"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
                HandleFileLoad(dialog.FileName);
        }

        private void HandleFileLoad(string filePath)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string tempFileToKeep = _isDecompressedMode ? _decompressedTempFile : null;

                if (_currentFile != null)
                {
                    if (_player != null)
                    {
                        _player.Stop();
                        _player.Dispose();
                        _player = null;
                    }
                    _progressTimer?.Stop();
                    _audioService.CloseReader();
                    _currentReader = null;
                    _currentFile = null;

                    if (!string.IsNullOrEmpty(_originalFilePath) && File.Exists(_originalFilePath))
                    {
                        if (_originalFilePath != tempFileToKeep)
                        {
                            File.Delete(_originalFilePath);
                        }
                    }
                    _originalFilePath = null;
                }

                if (_isDecompressedMode && !string.IsNullOrEmpty(tempFileToKeep) && tempFileToKeep == filePath)
                {
                    _originalFilePath = filePath;
                }
                else
                {
                    _originalFilePath = Path.Combine(
                        Path.GetTempPath(),
                        $"SoundShrink_Original_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(filePath)}"
                    );
                    File.Copy(filePath, _originalFilePath, true);
                }

                _currentFile = _audioService.LoadAudio(filePath);
                UpdateAudioProperties();

                lblFileLoadText.Text = "Loaded";
                lblFileName.Text = _currentFile.FileName;
                progressBarMain.Value = 0;

                this.Text = _isDecompressedMode
                    ? $"🎵 Decompressed - {Path.GetFileName(filePath)}"
                    : $"Audio Compression Lab - {_currentFile.FileName}";

                lblCurrentTime.Text = "0:00";
                lblRemainingTime.Text = "-" + FormatTime(_currentFile.Duration);

                UpdateButtonStates(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UpdateAudioProperties()
        {
            lblFileSizeValue.Text = $"{_currentFile.FileSizeBytes / (1024.0 * 1024.0):F2} MB";
            lblDurationValue.Text = FormatTime(_currentFile.Duration);
            lblSampleRatePropValue.Text = $"{_currentFile.SampleRate} Hz";
            lblChannelsValue.Text = _currentFile.Channels.ToString();
            lblBitRateValue.Text = $"{_currentFile.AverageBytesPerSecond * 8 / 1000.0:F1} kbps";
            lblCodecValue.Text = Path.GetExtension(_currentFile.FilePath).ToUpper().TrimStart('.');
            lblBitsPerSampleValue.Text = $"{_currentFile.BitsPerSample} bit";
        }

        private string FormatTime(TimeSpan time) => $"{time.Minutes:D2}:{time.Seconds:D2}";

        private void UnloadCurrentFile()
        {
            if (_player != null)
            {
                _player.Stop();
                _player.Dispose();
                _player = null;
            }

            _progressTimer?.Stop();
            _audioService.CloseReader();
            _currentReader = null;

            _currentFile = null;
            _isPaused = false;
            _playbackOffset = TimeSpan.Zero;

            progressBarMain.Value = 0;
            lblPlaybackStatus.Text = "";
            lblFileLoadText.Text = "Drag or select an audio file";
            lblFileName.Text = "";

            lblFileSizeValue.Text = "";
            lblDurationValue.Text = "";
            lblSampleRatePropValue.Text = "";
            lblChannelsValue.Text = "";
            lblBitRateValue.Text = "";
            lblCodecValue.Text = "";
            lblBitsPerSampleValue.Text = "";

            lblReportContent.Text = "";
            lblCompressionRatioTitle.Text = "";
            lblProcessingSpeedTitle.Text = "";
            progressBarCompression.Value = 0;
            progressBarSpeed.Value = 0;

            lblCurrentTime.Text = "0:00";
            lblRemainingTime.Text = "-0:00";

            this.Text = "Audio Compression";
            UpdateButtonStates(false);
            _compressedData = null;
            _lastCompressionResult = null;
            _currentAlgorithm = null;
            _isDecompressedMode = false;

            btnPlayPause.Text = "▶";

            CleanupTempFiles();
        }
        
        #region Playback

        private void PlayAudio()
        {
            if (_currentFile == null) return;

            if (_player == null || _player.PlaybackState == PlaybackState.Stopped)
            {
                var reader = _audioService.GetReader();
                if (reader == null) return;

                reader.Position = 0;
                _currentReader = reader;

                _player?.Dispose();
                _player = new WaveOutEvent();
                _player.Init(reader);

                _player.Play();
                _playbackStartTime = DateTime.Now;
                _playbackOffset = TimeSpan.Zero;
                _progressTimer.Start();

                btnPlayPause.Text = "⏸";
                _isPaused = false;
            }
            else if (_isPaused)
            {
                _player.Play();
                _playbackStartTime = DateTime.Now;
                _progressTimer.Start();

                btnPlayPause.Text = "⏸";
                _isPaused = false;
            }
        }

        private void StopAudio()
        {
            if (_player != null)
            {
                _player.Stop();
                _progressTimer.Stop();

                var reader = _audioService.GetReader();
                if (reader != null) reader.Position = 0;

                progressBarMain.Value = 0;
                lblPlaybackStatus.Text = "";
                lblCurrentTime.Text = "0:00";
                if (_currentFile != null)
                    lblRemainingTime.Text = "-" + FormatTime(_currentFile.Duration);

                _playbackOffset = TimeSpan.Zero;
                _currentReader = null;
                _isPaused = false;
                btnPlayPause.Text = "▶";
            }
        }

        private void TogglePause()
        {
            if (_player != null)
            {
                if (_player.PlaybackState == PlaybackState.Playing)
                {
                    _player.Pause();
                    _playbackOffset += DateTime.Now - _playbackStartTime;
                    _progressTimer.Stop();
                    btnPlayPause.Text = "▶";
                    _isPaused = true;
                }
                else if (_isPaused)
                {
                    PlayAudio();
                }
            }
        }

        private void SeekToPosition(TimeSpan targetTime)
        {
            if (_currentFile == null || _player == null) return;

            var reader = _audioService.GetReader();
            if (reader == null) return;

            bool wasPlaying = _player.PlaybackState == PlaybackState.Playing;

            _player.Pause();
            reader.CurrentTime = targetTime;

            _playbackStartTime = DateTime.Now;
            _playbackOffset = targetTime;

            if (wasPlaying)
            {
                _player.Play();
            }

            progressBarMain.Value = (int)((targetTime.TotalSeconds / _currentFile.Duration.TotalSeconds) * 100);
            lblCurrentTime.Text = FormatTime(targetTime);

            TimeSpan remaining = _currentFile.Duration - targetTime;
            lblRemainingTime.Text = "-" + FormatTime(remaining);
        }

        #endregion

        #region Modern Controls Events

        private void BtnPlayPause_Click(object sender, EventArgs e)
        {
            if (_player == null || _player.PlaybackState == PlaybackState.Stopped)
                PlayAudio();
            else
                TogglePause();
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (_currentFile != null && _player != null)
            {
                TimeSpan currentTime = _playbackOffset;

                if (_player.PlaybackState == PlaybackState.Playing)
                {
                    currentTime = DateTime.Now - _playbackStartTime + _playbackOffset;
                }

                TimeSpan targetTime = TimeSpan.FromSeconds(Math.Max(0, currentTime.TotalSeconds - 10));
                SeekToPosition(targetTime);
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (_currentFile != null && _player != null)
            {
                TimeSpan currentTime = _playbackOffset;

                if (_player.PlaybackState == PlaybackState.Playing)
                {
                    currentTime = DateTime.Now - _playbackStartTime + _playbackOffset;
                }

                TimeSpan targetTime = TimeSpan.FromSeconds(
                    Math.Min(_currentFile.Duration.TotalSeconds, currentTime.TotalSeconds + 10));
                SeekToPosition(targetTime);
            }
        }

        private void ProgressBarMain_MouseDown(object sender, MouseEventArgs e)
        {
            _isUserSeeking = true;
        }

        private void ProgressBarMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isUserSeeking && _currentFile != null && _player != null)
            {
                var progressBar = sender as ProgressBar;
                if (progressBar != null)
                {
                    double progress = (double)e.X / progressBar.Width;
                    progress = Math.Max(0, Math.Min(1, progress));

                    TimeSpan targetTime = TimeSpan.FromSeconds(progress * _currentFile.Duration.TotalSeconds);
                    SeekToPosition(targetTime);
                }
            }
            _isUserSeeking = false;
        }

        #endregion

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            if (_isUserSeeking) return;

            if (_player?.PlaybackState == PlaybackState.Playing && _currentFile != null)
            {
                TimeSpan elapsed = DateTime.Now - _playbackStartTime + _playbackOffset;
                TimeSpan currentTime = elapsed < _currentFile.Duration ? elapsed : _currentFile.Duration;
                TimeSpan remaining = _currentFile.Duration - currentTime;

                if (_currentFile.Duration.TotalSeconds > 0)
                {
                    progressBarMain.Value = (int)((currentTime.TotalSeconds / _currentFile.Duration.TotalSeconds) * 100);
                }

                lblCurrentTime.Text = FormatTime(currentTime);
                lblRemainingTime.Text = "-" + FormatTime(remaining);
            }
        }

        #region Compression

        private float[] LoadAudioSamples(string filePath)
        {
            var samples = new List<float>();
            using (var reader = new AudioFileReader(filePath))
            {
                float[] buffer = new float[4096];
                int read;
                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < read; i++)
                        samples.Add(buffer[i]);
                }
            }
            return samples.ToArray();
        }

        private byte[] FloatsToBytes(float[] samples)
        {
            byte[] bytes = new byte[samples.Length * 4];
            Buffer.BlockCopy(samples, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private int GetSelectedSampleRate()
        {
            string sampleRateText = cmbSampleRate.SelectedItem?.ToString() ?? "Original";
            if (sampleRateText == "Original")
                return _currentFile?.SampleRate ?? 44100;

            if (int.TryParse(sampleRateText, out int rate))
                return rate;

            return _currentFile?.SampleRate ?? 44100;
        }


        private string GenerateReportText(CompressionResult result, string algorithmName)
        {
            string algoName = cmbAlgorithm.SelectedItem?.ToString() ?? "";
            string sampleRateText = cmbSampleRate.SelectedItem?.ToString() ?? "Original";
            int actualSampleRate = GetSelectedSampleRate();

            string settingsText = $"SampleRate: {actualSampleRate}\n";

            switch (algoName)
            {
                case "Nonlinear Quantization":
                    string quantValue = cmbQuantLevels.SelectedItem?.ToString() ?? "256";
                    settingsText += $"QuantizationLevels: {quantValue}\n" +
                                   $"MuLawCoefficient: 255";
                    break;

                case "DPCM":
                    string bitsValue = cmbBitsPerSampleComp.SelectedItem?.ToString() ?? "16";
                    int bits;
                    if (!int.TryParse(bitsValue, out bits))
                        bits = 16;

                    settingsText += $"BitsPerSample: {bits}\n" +
                                   $"QuantStep: {(2.0 / Math.Pow(2, bits)):F6}";
                    break;
                case "Predictive Differential Coding":
                    double stepSizeValue = trkStepSize.Value / 100.0;  
                    settingsText += $"StepSize: {stepSizeValue:F3}\n" +
                                   $"PredictionCoefficient: 0.90";
                    break;
                case "Delta Modulation":
                    double dmStepSize = trkStepSize.Value / 100.0;  
                    settingsText += $"StepSize: {dmStepSize:F3}";
                    break;
                case "Adaptive Delta Modulation":
                    double admInitialStep = trkInitialStep.Value / 100.0;
                    double admMultiplier = trkStepMultiplier.Value / 10.0;
                    settingsText += $"InitialStepSize: {admInitialStep:F3}\n" +
                                   $"StepSizeMultiplier: {admMultiplier:F2}\n" +
                                   $"MinStep: 0.005\n" +
                                   $"MaxStep: 0.5";
                    break;
            }

            double savingPercentage = (1.0 - (double)result.CompressedSize / result.OriginalSize) * 100.0;

            string report = $"OriginalSize: {result.OriginalSize}\n" +
                           $"CompressedSize: {result.CompressedSize}\n" +
                           $"SizeSaving: {savingPercentage:F2}%\n" +
                           $"CompressionRatio: {result.CompressionRatio:F2}x\n" +
                           $"ElapsedTime: {result.ProcessingTime.TotalSeconds:F2} seconds\n" +
                           $"Algorithm: {algorithmName}\n" +
                           $"Channels: {_currentFile?.Channels ?? 1}\n\n" +
                           $"Settings:\n{settingsText}";

            return report;
        }

        private void UpdateOperationReport(CompressionResult result, string algorithmName)
        {
            string report = GenerateReportText(result, algorithmName);

            lblReportContent.Text = report;
            lblCompressionRatioTitle.Text = $"Compression Ratio: {result.CompressionRatio:F2}x";
            lblProcessingSpeedTitle.Text = $"Processing Time: {result.ProcessingTime.TotalSeconds:F2}s";

            if (progressBarCompression.Maximum > 0)
                progressBarCompression.Value = Math.Min(100, (int)(result.CompressionRatio * 100));
        }

        #endregion

        #region Event Handlers

        private void btnShowChart_Click(object sender, EventArgs e)
        {
            if (_savedRatioHistory.Count < 2)
            {
                MessageBox.Show(
                    "Not enough data to display the chart.\n\nPlease perform a compression first.",
                    "No Data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using (var chartForm = new ChartViewerForm(
                _savedRatioHistory,
                _savedSpeedHistory,
                _savedAlgorithmName,
                _savedTotalTime,
                _savedFinalRatio))
            {
                chartForm.ShowDialog(this);
            }
        }
        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileBrowser();
        }

        private void btnCompressFile_Click(object sender, EventArgs e)
        {
            if (_currentFile == null)
            {
                MessageBox.Show("Please choose an audio file first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string algoName = cmbAlgorithm.SelectedItem?.ToString() ?? "";

            ICompressionAlgorithm algorithm = CreateAlgorithmFromSidebar(algoName);

            if (algorithm == null)
            {
                MessageBox.Show("Invalid algorithm selected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ بدء الضغط مع عرض نافذة التقدم
            CompressAudioWithProgress(algorithm);
        }

        private ICompressionAlgorithm CreateAlgorithmFromSidebar(string algoName)
        {
            var settings = GetCurrentCompressionSettings();

            switch (algoName)
            {
                case "Nonlinear Quantization":
                    return new NonlinearQuantization(settings);
                case "DPCM":
                    return new DPCM(settings);
                case "Predictive Differential Coding":
                    return new PredictiveDifferentialCoding(settings);
                case "Delta Modulation":
                    return new DeltaModulation(settings);
                case "Adaptive Delta Modulation":
                    return new AdaptiveDeltaModulation(settings);
                default:
                    return null;
            }
        }

        private void CompressAudioWithProgress(ICompressionAlgorithm algorithm)
        {
            if (_currentFile == null)
            {
                MessageBox.Show("Please choose an audio file first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                float[] samples = LoadAudioSamples(_currentFile.FilePath);
                byte[] audioData = FloatsToBytes(samples);
                int selectedSampleRate = GetSelectedSampleRate();

                _isCompressionCancelled = false;
                _totalBytesToProcess = audioData.Length;
                _processedBytes = 0;
                _compressionStartTime = DateTime.Now;
                _lastCompressionResult = null;
                _realProgress = 0;

                _savedRatioHistory.Clear();
                _savedSpeedHistory.Clear();
                _savedAlgorithmName = algorithm.AlgorithmName;

                _progressForm = new CompressionProgressForm();
                _progressForm.CancelRequested += (s, e) => _isCompressionCancelled = true;

                _compressionMonitorTimer = new Timer { Interval = 50 };
                _compressionMonitorTimer.Tick += CompressionMonitorTimer_Tick;
                _compressionMonitorTimer.Start();

                _progressForm.Show(this);

                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        if (_isCompressionCancelled) return;

                        var progressReporter = new Progress<int>(value =>
                        {
                            _realProgress = value;
                            _processedBytes = (long)(_totalBytesToProcess * value / 100.0);
                        });

                        _compressedData = algorithm.Compress(
                            audioData,
                            selectedSampleRate,
                            _currentFile.BitsPerSample,
                            _currentFile.Channels,
                            progressReporter);

                        _currentAlgorithm = algorithm;
                        _lastCompressionResult = algorithm.GetCompressionStats();

                        _savedFinalRatio = _lastCompressionResult?.CompressionRatio ?? 1.0;
                        _savedTotalTime = _lastCompressionResult?.ProcessingTime ?? TimeSpan.Zero;
                    }
                    catch (Exception ex)
                    {
                        if (!_isCompressionCancelled)
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show($"Compression error: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                        }
                    }
                    finally
                    {
                        var elapsed = DateTime.Now - _compressionStartTime;
                        if (elapsed.TotalMilliseconds < 2000)
                        {
                            System.Threading.Thread.Sleep(2000 - (int)elapsed.TotalMilliseconds);
                        }

                        this.Invoke(new Action(() =>
                        {
                            _compressionMonitorTimer?.Stop();
                            _compressionMonitorTimer?.Dispose();
                            _compressionMonitorTimer = null;

                            if (_progressForm != null)
                            {
                                _progressForm.Close();
                                _progressForm.Dispose();
                                _progressForm = null;
                            }

                            Cursor = Cursors.Default;

                            if (!_isCompressionCancelled && _lastCompressionResult != null)
                            {
                                btnShowChart.Enabled = true;
                                ShowSaveFileDialog(algorithm);
                            }
                            else if (_isCompressionCancelled)
                            {
                                btnShowChart.Enabled = false;
                                MessageBox.Show("Compression process was cancelled.", "Cancelled",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                _compressionMonitorTimer?.Stop();
                _progressForm?.Close();
                MessageBox.Show($"Compression error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CompressionMonitorTimer_Tick(object sender, EventArgs e)
        {
            if (_progressForm == null) return;

            var elapsed = DateTime.Now - _compressionStartTime;

            int progressPercentage = _realProgress;
            double currentRatio = 1.0;

            if (_lastCompressionResult != null)
            {
                progressPercentage = 100;
                currentRatio = _lastCompressionResult.CompressionRatio;
            }
            else
            {
                if (progressPercentage > 0 && progressPercentage < 100)
                {
                    currentRatio = 1.0 + (progressPercentage / 100.0);
                }
            }

            double speedMBps = 0;
            if (elapsed.TotalSeconds > 0.1)
            {
                speedMBps = (_processedBytes / 1024.0 / 1024.0) / elapsed.TotalSeconds;
            }

            TimeSpan estimatedRemaining = TimeSpan.Zero;
            if (progressPercentage > 0 && progressPercentage < 100 && speedMBps > 0)
            {
                double remainingBytes = _totalBytesToProcess - _processedBytes;
                double remainingMB = remainingBytes / 1024.0 / 1024.0;
                estimatedRemaining = TimeSpan.FromSeconds(remainingMB / speedMBps);
            }

            _savedRatioHistory.Add((float)currentRatio);
            _savedSpeedHistory.Add((float)speedMBps);

            if (_savedRatioHistory.Count > 200)
            {
                _savedRatioHistory.RemoveAt(0);
                _savedSpeedHistory.RemoveAt(0);
            }

            var monitor = new CompressionMonitor
            {
                ProgressPercentage = progressPercentage,
                CompressionRatio = currentRatio,
                ProcessingSpeedMBps = speedMBps,
                ProcessedBytes = _processedBytes,
                TotalBytes = _totalBytesToProcess,
                ElapsedTime = elapsed,
                EstimatedRemaining = estimatedRemaining,
                IsCancelled = _isCompressionCancelled
            };

            _progressForm.UpdateProgress(monitor);

            if (_isCompressionCancelled)
            {
                _compressionMonitorTimer.Stop();
                _progressForm.SetCancelled();
            }
        }
        private void ShowSaveFileDialog(ICompressionAlgorithm algorithm)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Compressed Audio Files (*.compressed)|*.compressed";
                saveDialog.Title = "Save Compressed Audio File";

                string originalExt = Path.GetExtension(_currentFile.FilePath).TrimStart('.');
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(_currentFile.FileName);
                saveDialog.FileName = $"{fileNameWithoutExt}_{originalExt}.compressed";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllBytes(saveDialog.FileName, _compressedData);

                        string reportText = GenerateReportText(_lastCompressionResult, algorithm.AlgorithmName);
                        string infoFilePath = saveDialog.FileName + ".info";
                        File.WriteAllText(infoFilePath, reportText);

                        UpdateOperationReport(_lastCompressionResult, algorithm.AlgorithmName);

                        MessageBox.Show(
                            $"Compression completed and saved successfully!\n\n" +
                            $"Compressed File: {Path.GetFileName(saveDialog.FileName)}\n" +
                            $"Info File: {Path.GetFileName(infoFilePath)}\n\n" +
                            $"Compression Ratio: {_lastCompressionResult.CompressionRatio:F2}x",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    UpdateOperationReport(_lastCompressionResult, algorithm.AlgorithmName);
                }
            }
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "Compressed Audio Files (*.compressed)|*.compressed|All Files (*.*)|*.*",
                Title = "Choose a compressed file to decompress"
            };

            if (openDialog.ShowDialog() != DialogResult.OK) return;

            string compressedFilePath = openDialog.FileName;
            string infoFilePath = compressedFilePath + ".info";

            try
            {
                Cursor = Cursors.WaitCursor;

                if (!File.Exists(infoFilePath))
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show(
                        $"❌ The info file (.info) was not found!\n\n" +
                        $"Expected: {Path.GetFileName(infoFilePath)}\n\n" +
                        $"Cannot decompress without algorithm information.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Dictionary<string, string> info;
                try
                {
                    info = ReadInfoFile(infoFilePath);
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show($"❌ Error reading info file:\n{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!info.ContainsKey("Algorithm"))
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("❌ The info file does not contain the algorithm name!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string algorithmName = info["Algorithm"];

                int originalSampleRate = 44100;
                int originalChannels = 1;
                int originalBitsPerSample = 16;

                if (info.ContainsKey("SampleRate") && int.TryParse(info["SampleRate"], out int sr))
                    originalSampleRate = sr;

                if (info.ContainsKey("Channels") && int.TryParse(info["Channels"], out int ch))
                    originalChannels = ch;

                if (info.ContainsKey("BitsPerSample") && int.TryParse(info["BitsPerSample"], out int bps))
                    originalBitsPerSample = bps;

                var originalSettings = ReadAlgorithmSettings(info);

                Cursor = Cursors.Default;

                using (var settingsForm = new DecompressionSettingsForm(
                    algorithmName,
                    originalSampleRate,
                    originalChannels,
                    originalBitsPerSample,
                    originalSettings))
                {
                    if (settingsForm.ShowDialog() != DialogResult.OK)
                        return;

                    Cursor = Cursors.WaitCursor;

                    CompressionSettings finalSettings = settingsForm.AlgorithmSettings;

                    ICompressionAlgorithm algorithm = CreateAlgorithmWithSettings(algorithmName, finalSettings);

                    byte[] compressedData = File.ReadAllBytes(compressedFilePath);

                    byte[] decompressedData = algorithm.Decompress(
                        compressedData,
                        settingsForm.SampleRate,
                        settingsForm.BitsPerSample,
                        settingsForm.Channels);

                    int sampleCount = decompressedData.Length / 4;
                    float[] samples = new float[sampleCount];
                    Buffer.BlockCopy(decompressedData, 0, samples, 0, decompressedData.Length);

                    samples = NormalizeSamples(samples);

                    string originalFileName = Path.GetFileNameWithoutExtension(compressedFilePath);
                    string originalExtension = ".wav"; 

                    if (originalFileName.Contains("_"))
                    {
                        string[] parts = originalFileName.Split('_');
                        if (parts.Length > 0)
                        {
                            string possibleExt = "." + parts[parts.Length - 1].ToLower();
                            if (possibleExt == ".mp3" || possibleExt == ".wav")
                            {
                                originalExtension = possibleExt;
                            }
                        }
                    }

                    string tempWavPath = SaveAsTempWav(
                        samples,
                        settingsForm.SampleRate,
                        settingsForm.Channels,
                        settingsForm.BitsPerSample,
                        originalExtension); 

                    if (!File.Exists(tempWavPath))
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("❌ Failed to create temporary file!",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        HandleFileLoad(tempWavPath);
                        _decompressedTempFile = tempWavPath;
                        _isDecompressedMode = true;
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show($"❌ Failed to load decompressed file:\n\n{ex.Message}",
                            "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        if (File.Exists(tempWavPath))
                        {
                            try { File.Delete(tempWavPath); } catch { }
                        }
                        return;
                    }

                    this.Text = $"🎵 Decompressed - {Path.GetFileName(compressedFilePath)}";

                    Cursor = Cursors.Default;
                    MessageBox.Show(
                        $"✅ Decompression completed successfully!\n\n" +
                        $" Algorithm: {algorithmName}\n" +
                        $" Settings: {(settingsForm.UseOriginalSettings ? "Original" : "Default")}\n" +
                        $" File: {Path.GetFileName(tempWavPath)}\n\n" +
                        $"You can now play the decompressed file.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    try
                    {
                        PlayAudio();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"✅ Decompression was successful, but a playback error occurred:\n\n{ex.Message}",
                            "Playback Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(
                    $"❌ Error during decompression:\n\n{ex.Message}",
                    "Decompression Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private string GetSettingsSummary(string algorithmName, CompressionSettings settings, int sampleRate, int channels)
        {
            string summary = $"Sample Rate: {sampleRate} Hz\nChannels: {(channels == 1 ? "Mono" : "Stereo")}\n";

            switch (algorithmName)
            {
                case "Nonlinear Quantization":
                    summary += $"Quantization Levels: {settings.QuantizationLevels}";
                    break;
                case "DPCM":
                    summary += $"Bits Per Sample: {settings.BitsPerSample}";
                    break;
                case "Predictive Differential Coding":
                    summary += $"Step Size: {settings.StepSize:F3}";  // ✅ تغيير
                    break;
                case "Delta Modulation":
                    summary += $"Step Size: {settings.StepSize:F3}";
                    break;
                case "Adaptive Delta Modulation":
                    summary += $"Initial Step: {settings.InitialStepSize:F3}\n" +
                              $"Step Multiplier: {settings.StepSizeMultiplier:F2}";
                    break;
            }

            return summary;
        }
        private void btnResetWorkspace_Click(object sender, EventArgs e)
        {
            // ✅ 1. إيقاف التشغيل الحالي
            if (_player != null)
            {
                _player.Stop();
                _player.Dispose();
                _player = null;
            }

            _progressTimer?.Stop();
            _audioService.CloseReader();
            _currentReader = null;

            // ✅ 2. مسح معلومات الملف
            _currentFile = null;
            _isPaused = false;
            _playbackOffset = TimeSpan.Zero;

            // ✅ 3. إعادة تعيين واجهة المستخدم
            progressBarMain.Value = 0;
            lblPlaybackStatus.Text = "";
            lblFileLoadText.Text = "Drag or select an audio file";
            lblFileName.Text = "";

            lblFileSizeValue.Text = "";
            lblDurationValue.Text = "";
            lblSampleRatePropValue.Text = "";
            lblChannelsValue.Text = "";
            lblBitRateValue.Text = "";
            lblCodecValue.Text = "";
            lblBitsPerSampleValue.Text = "";

            lblReportContent.Text = "";
            lblCompressionRatioTitle.Text = "";
            lblProcessingSpeedTitle.Text = "";
            progressBarCompression.Value = 0;
            progressBarSpeed.Value = 0;

            lblCurrentTime.Text = "0:00";
            lblRemainingTime.Text = "-0:00";

            this.Text = "Audio Compression";
            UpdateButtonStates(false);
            _compressedData = null;
            _lastCompressionResult = null;
            _currentAlgorithm = null;
            _isDecompressedMode = false;

            btnPlayPause.Text = "▶";

            btnShowChart.Enabled = false;
            _savedRatioHistory.Clear();
            _savedSpeedHistory.Clear();

            ResetAlgorithmSettings();

            CleanupTempFiles();
        }

        private void ResetAlgorithmSettings()
        {
            try
            {
                if (cmbAlgorithm != null && cmbAlgorithm.Items.Count > 0)
                    cmbAlgorithm.SelectedIndex = 0;

                if (cmbSampleRate != null && cmbSampleRate.Items.Count > 0)
                    cmbSampleRate.SelectedIndex = 0;

                if (cmbQuantLevels != null && cmbQuantLevels.Items.Count > 0)
                    cmbQuantLevels.SelectedIndex = 4;

                if (cmbBitsPerSampleComp != null && cmbBitsPerSampleComp.Items.Count > 0)
                    cmbBitsPerSampleComp.SelectedIndex = 3;

                if (trkStepSize != null)
                {
                    trkStepSize.Value = 10;
                    lblStepSizeValue.Text = "0.100";
                }

                if (trkInitialStep != null)
                {
                    trkInitialStep.Value = 10; // 0.10
                    lblInitialStepValue.Text = "0.100";
                }

                if (trkStepMultiplier != null)
                {
                    trkStepMultiplier.Value = 15; // 1.50
                    lblStepMultiplierValue.Text = "1.50";
                }

                if (numDeltaStep != null)
                {
                    decimal deltaStepValue = 0.040m;
                    if (deltaStepValue >= numDeltaStep.Minimum && deltaStepValue <= numDeltaStep.Maximum)
                        numDeltaStep.Value = deltaStepValue;
                    else
                        numDeltaStep.Value = numDeltaStep.Minimum;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ResetAlgorithmSettings: {ex.Message}");
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && IsAudioFile(files[0]))
                    e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0) HandleFileLoad(files[0]);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnloadCurrentFile();
            _progressTimer?.Dispose();
            _audioService?.CloseReader();
            CleanupTempFiles();
            base.OnFormClosing(e);
        }


        #region Decompression

        private void CleanupTempFiles()
        {
            try
            {
                if (!string.IsNullOrEmpty(_originalFilePath) && File.Exists(_originalFilePath))
                {
                    if (_originalFilePath != _decompressedTempFile)
                    {
                        File.Delete(_originalFilePath);
                    }
                    _originalFilePath = null;
                }

                if (!string.IsNullOrEmpty(_decompressedTempFile) && File.Exists(_decompressedTempFile))
                {
                    File.Delete(_decompressedTempFile);
                    _decompressedTempFile = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cleaning temp files: {ex.Message}");
            }
        }
    
        private Dictionary<string, string> ReadInfoFile(string infoFilePath)
        {
            var info = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(infoFilePath))
                throw new FileNotFoundException("Info file (.info) not found", infoFilePath);

            foreach (var line in File.ReadAllLines(infoFilePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.TrimStart().StartsWith("#")) continue;

                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    if (!info.ContainsKey(key))
                        info[key] = value;
                }
            }

            return info;
        }
        private CompressionSettings GetCurrentCompressionSettings()
        {
            int quantLevels = 256;
            if (cmbQuantLevels.SelectedItem != null &&
                int.TryParse(cmbQuantLevels.SelectedItem.ToString(), out int parsedQuant))
            {
                quantLevels = parsedQuant;
            }

            int bitsPerSample = 16;
            if (cmbBitsPerSampleComp.SelectedItem != null &&
                int.TryParse(cmbBitsPerSampleComp.SelectedItem.ToString(), out int parsedBits))
            {
                bitsPerSample = parsedBits;
            }

            double stepSize = trkStepSize.Value / 100.0;

            double initialStepSize = trkInitialStep.Value / 100.0;

            double stepMultiplier = trkStepMultiplier.Value / 10.0;

            return new CompressionSettings
            {
                QuantizationLevels = quantLevels,
                BitsPerSample = bitsPerSample,
                PredictionCoefficient = 0.9,
                StepSize = stepSize,
                InitialStepSize = initialStepSize,
                StepSizeMultiplier = stepMultiplier
            };
        }

        private CompressionSettings ReadAlgorithmSettings(Dictionary<string, string> info)
        {
            var settings = new CompressionSettings();

            try
            {
                if (info.ContainsKey("QuantizationLevels"))
                {
                    string value = info["QuantizationLevels"].Trim();
                    if (int.TryParse(value, out int levels))
                        settings.QuantizationLevels = levels;
                }

                if (info.ContainsKey("BitsPerSample"))
                {
                    string value = info["BitsPerSample"].Trim();
                    if (int.TryParse(value, out int bits))
                        settings.BitsPerSample = bits;
                }

                if (info.ContainsKey("PredictionCoefficient"))
                {
                    string value = info["PredictionCoefficient"].Trim();
                    if (double.TryParse(value,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out double coeff))
                    {
                        settings.PredictionCoefficient = coeff;
                    }
                }

                if (info.ContainsKey("StepSize"))
                {
                    string value = info["StepSize"].Trim();
                    if (double.TryParse(value,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out double step))
                    {
                        settings.StepSize = step;
                    }
                }

                if (info.ContainsKey("InitialStepSize"))
                {
                    string value = info["InitialStepSize"].Trim();
                    if (double.TryParse(value,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out double initStep))
                    {
                        settings.InitialStepSize = initStep;
                    }
                }

                if (info.ContainsKey("StepSizeMultiplier"))
                {
                    string value = info["StepSizeMultiplier"].Trim();
                    if (double.TryParse(value,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out double multiplier))
                    {
                        settings.StepSizeMultiplier = multiplier;
                    }
                }

                if (info.ContainsKey("SampleRate"))
                {
                    string value = info["SampleRate"].Trim().Replace("Hz", "").Trim();
                    if (int.TryParse(value, out int sampleRate))
                        settings.SampleRate = sampleRate;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading algorithm settings: {ex.Message}");
            }

            return settings;
        }
        private ICompressionAlgorithm CreateAlgorithmByName(string algorithmName)
        {
            if (algorithmName.Contains("Nonlinear"))
                return new NonlinearQuantization();
            else if (algorithmName.Contains("DPCM") || algorithmName.Contains("Differential PCM"))
                return new DPCM();
            else if (algorithmName.Contains("Predictive"))
                return new PredictiveDifferentialCoding();
            else if (algorithmName.Contains("Adaptive Delta"))
                return new AdaptiveDeltaModulation();
            else if (algorithmName.Contains("Delta Modulation"))
                return new DeltaModulation();
            else
                throw new NotSupportedException($"Unsupported algorithm: {algorithmName}");
        }

        private ICompressionAlgorithm CreateAlgorithmWithSettings(string algorithmName, CompressionSettings settings)
        {
            if (algorithmName.Contains("Nonlinear"))
                return new NonlinearQuantization(settings);
            else if (algorithmName.Contains("DPCM") || algorithmName.Contains("Differential PCM"))
                return new DPCM(settings);
            else if (algorithmName.Contains("Predictive"))
                return new PredictiveDifferentialCoding(settings);
            else if (algorithmName.Contains("Adaptive Delta"))
                return new AdaptiveDeltaModulation(settings);
            else if (algorithmName.Contains("Delta Modulation"))
                return new DeltaModulation(settings);
            else
                throw new NotSupportedException($"Unsupported algorithm: {algorithmName}");
        }

        private string SaveAsTempWav(float[] samples, int sampleRate, int channels, int bitsPerSample, string originalExtension = ".wav")
        {
            samples = NormalizeSamples(samples);

            string tempPath = Path.Combine(
                Path.GetTempPath(),
                $"SoundShrink_Decompressed_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}{originalExtension.ToLower()}"
            );

            if (channels == 2)
            {
                if (samples.Length % 2 != 0)
                {
                    float[] adjusted = new float[samples.Length - 1];
                    Array.Copy(samples, adjusted, adjusted.Length);
                    samples = adjusted;
                }
            }

            var waveFormat = new WaveFormat(sampleRate, bitsPerSample, channels);

            using (var writer = new WaveFileWriter(tempPath, waveFormat))
            {
                if (bitsPerSample == 16)
                {
                    short[] samples16 = new short[samples.Length];
                    for (int i = 0; i < samples.Length; i++)
                    {
                        samples16[i] = (short)(samples[i] * short.MaxValue);
                    }

                    byte[] bytes = new byte[samples16.Length * 2];
                    Buffer.BlockCopy(samples16, 0, bytes, 0, bytes.Length);
                    writer.Write(bytes, 0, bytes.Length);
                }
                else if (bitsPerSample == 32)
                {
                    byte[] bytes = new byte[samples.Length * 4];
                    Buffer.BlockCopy(samples, 0, bytes, 0, bytes.Length);
                    writer.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    writer.WriteSamples(samples, 0, samples.Length);
                }
            }

            return tempPath;
        }

        private float[] NormalizeSamples(float[] samples)
        {
            if (samples == null || samples.Length == 0) return samples;

            float maxAbs = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                float abs = Math.Abs(samples[i]);
                if (abs > maxAbs) maxAbs = abs;
            }

            if (maxAbs < 0.001f) return samples;

            if (maxAbs > 1.0f)
            {
                float scale = 0.95f / maxAbs; 
                for (int i = 0; i < samples.Length; i++)
                {
                    samples[i] *= scale;
                }
            }
            return samples;
        }


        private void trkInitialStep_ValueChanged(object sender, EventArgs e)
        {
            if (trkInitialStep != null && lblInitialStepValue != null)
            {
                double actualValue = trkInitialStep.Value / 100.0;
                lblInitialStepValue.Text = actualValue.ToString("F3");
            }
        }

        private void trkStepMultiplier_ValueChanged(object sender, EventArgs e)
        {
            if (trkStepMultiplier != null && lblStepMultiplierValue != null)
            {
                double actualValue = trkStepMultiplier.Value / 10.0;
                lblStepMultiplierValue.Text = actualValue.ToString("F2");
            }
        }

        private void trkStepSize_ValueChanged(object sender, EventArgs e)
        {
            if (trkStepSize != null && lblStepSizeValue != null)
            {
                double actualValue = trkStepSize.Value / 100.0;
                lblStepSizeValue.Text = actualValue.ToString("F3");
            }
        }
        #endregion

        #endregion
    }
}