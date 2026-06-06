using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;
using SoundShrink_Desktop.Services;
using SoundShrink_Desktop.Algorithms;
using SoundShrink_Desktop.Analyzers;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace SoundShrink_Desktop
{
    public static class RoundedCornersHelper
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
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
            // للـ Panel
            panelFileInfo.ApplyRoundedBorder(20);
            panelFileLoad.ApplyRoundedBorder(20);
            panelAudioProperties.ApplyRoundedBorder(20);
            panelOperationReport.ApplyRoundedBorder(20);
            panelCompressionRatio.ApplyRoundedBorder(20);
            panelProcessingSpeed.ApplyRoundedBorder(20);

            // للأزرار
            btnChooseFile.ApplyRoundedBorder(10);
            btnChangeFile.ApplyRoundedBorder(10);
            btnCompressFile.ApplyRoundedBorder(10);
            btnDecompress.ApplyRoundedBorder(10);
            btnResetWorkspace.ApplyRoundedBorder(10);
            btnPrevious.ApplyRoundedBorder(20); // دائري تماماً
            btnPlayPause.ApplyRoundedBorder(20); // دائري تماماً
            btnNext.ApplyRoundedBorder(20); // دائري تماماً
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
            UpdateButtonStates(false);
            SetupModernControls();
        }

        private void SetupModernControls()
        {
            btnPrevious.Click += BtnPrevious_Click;
            btnPlayPause.Click += BtnPlayPause_Click;
            btnNext.Click += BtnNext_Click;

            btnPlayPause.Enabled = false;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;

            progressBarMain.MouseDown += ProgressBarMain_MouseDown;
            progressBarMain.MouseUp += ProgressBarMain_MouseUp;
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

                if (_currentFile != null)
                    UnloadCurrentFile();

                _originalFilePath = Path.Combine(
                    Path.GetTempPath(),
                    $"SoundShrink_Original_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(filePath)}"
                );
                File.Copy(filePath, _originalFilePath, true);

                _currentFile = _audioService.LoadAudio(filePath);
                UpdateAudioProperties();

                lblFileLoadText.Text = "Loaded";  // ✅ تأكد من وجود النص
                lblFileName.Text = _currentFile.FileName;  // ✅ اسم الملف
                progressBarMain.Value = 0;

                this.Text = $"Audio Compression Lab - {_currentFile.FileName}";

                lblCurrentTime.Text = "0:00";
                lblRemainingTime.Text = "-" + FormatTime(_currentFile.Duration);

                UpdateButtonStates(true);
                _isDecompressedMode = false;
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
            lblFileLoadText.Text = "Drag or select an audio file";  // ✅ إعادة الرسالة الافتراضية
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

            this.Text = "Audio Compression Lab - Multimedia 2026";
            UpdateButtonStates(false);
            _compressedData = null;
            _lastCompressionResult = null;
            _currentAlgorithm = null;
            _isDecompressedMode = false;

            btnPlayPause.Text = "▶";
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

                // ✅ تهيئة المشغل مباشرة بالـ Reader بدون BufferedWaveProvider
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

        /// <summary>
        /// ✅ الدالة الرئيسية للتقديم/التأخير - فورية بدون إعادة تهيئة
        /// </summary>
        private void SeekToPosition(TimeSpan targetTime)
        {
            if (_currentFile == null || _player == null) return;

            var reader = _audioService.GetReader();
            if (reader == null) return;

            bool wasPlaying = _player.PlaybackState == PlaybackState.Playing;

            // ✅ 1. إيقاف التشغيل مؤقتاً لضمان مزامنة الـ buffer الداخلي لـ WaveOutEvent
            _player.Pause();

            // ✅ 2. تغيير موضع القراءة في الملف مباشرة
            reader.CurrentTime = targetTime;

            // ✅ 3. تحديث أوقات التشغيل
            _playbackStartTime = DateTime.Now;
            _playbackOffset = targetTime;

            // ✅ 4. استئناف التشغيل إذا كان يعمل قبل الـ Seek
            if (wasPlaying)
            {
                _player.Play();
            }

            // ✅ 5. تحديث الواجهة
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
            // ✅ الرجوع 10 ثواني للخلف
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
            // ✅ التقديم 10 ثواني للأمام
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

        private void CompressAudio(ICompressionAlgorithm algorithm)
        {
            if (_currentFile == null)
            {
                MessageBox.Show("Please choose an audio file first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                float[] samples = LoadAudioSamples(_currentFile.FilePath);
                byte[] audioData = FloatsToBytes(samples);

                _compressedData = algorithm.Compress(
                    audioData,
                    _currentFile.SampleRate,
                    _currentFile.BitsPerSample,
                    _currentFile.Channels);

                _currentAlgorithm = algorithm;
                _lastCompressionResult = algorithm.GetCompressionStats();

                UpdateOperationReport(_lastCompressionResult, algorithm.AlgorithmName);

                MessageBox.Show($"Compression completed!\nRatio: {_lastCompressionResult.CompressionRatio:F2}:1",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Compression error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateOperationReport(CompressionResult result, string algorithmName)
        {
            string report = $"Original Size: {result.OriginalSize / (1024.0 * 1024.0):F2} MB\n" +
                           $"Compressed Size: {result.CompressedSize / (1024.0 * 1024.0):F2} MB\n" +
                           $"Size Saving: {((1 - result.CompressionRatio) * 100):F2}%\n" +
                           $"Compression Ratio: {result.CompressionRatio:F2}x\n" +
                           $"Elapsed Time: {result.ProcessingTime.TotalSeconds:F2} seconds\n" +
                           $"Algorithm: {algorithmName}\n" +
                           $"Settings: Sample Rate={_currentFile.SampleRate} Hz, " +
                           $"Quantization Levels={(int)numQuantLevels.Value}, " +
                           $"Delta Step={(int)numDeltaStep.Value}";

            lblReportContent.Text = report;
            lblCompressionRatioTitle.Text = $"Compression Ratio: {result.CompressionRatio:F2}x";
            lblProcessingSpeedTitle.Text = $"Processing Speed: {result.ProcessingTime.TotalSeconds:F2}s";

            if (progressBarCompression.Maximum > 0)
                progressBarCompression.Value = (int)(result.CompressionRatio * 100);
        }

        #endregion

        #region Event Handlers

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

            ICompressionAlgorithm algorithm = null;
            string algoName = cmbAlgorithm.SelectedItem?.ToString() ?? "";

            var settings = new CompressionSettings
            {
                QuantizationLevels = (int)numQuantLevels.Value,
                StepSize = (double)numDeltaStep.Value
            };

            switch (algoName)
            {
                case "Nonlinear Quantization":
                    algorithm = new NonlinearQuantization(settings);
                    break;
                case "DPCM":
                    algorithm = new DPCM(settings);
                    break;
                case "Predictive Differential Coding":
                    algorithm = new PredictiveDifferentialCoding(settings);
                    break;
                case "Delta Modulation":
                    algorithm = new DeltaModulation(settings);
                    break;
                case "Adaptive Delta Modulation":
                    algorithm = new AdaptiveDeltaModulation(settings);
                    break;
            }

            if (algorithm != null)
                CompressAudio(algorithm);
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "Compressed Audio Files|*.compressed|All Files|*.*",
                Title = "Choose Compressed File"
            };

            if (openDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                string compressedFile = openDialog.FileName;
                string infoFile = compressedFile + ".info";

                if (!File.Exists(infoFile))
                {
                    MessageBox.Show("Info file not found.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] compressedData = File.ReadAllBytes(compressedFile);
                MessageBox.Show("Decompression completed.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Decompression error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResetWorkspace_Click(object sender, EventArgs e)
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

            this.Text = "Audio Compression Lab - Multimedia 2026";
            UpdateButtonStates(false);
            _compressedData = null;
            _lastCompressionResult = null;
            _currentAlgorithm = null;
            _isDecompressedMode = false;

            btnPlayPause.Text = "▶";
            CleanupTempFiles();
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

        private void CleanupTempFiles()
        {
            try
            {
                if (!string.IsNullOrEmpty(_originalFilePath) && File.Exists(_originalFilePath))
                {
                    File.Delete(_originalFilePath);
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

        #endregion
    }
}