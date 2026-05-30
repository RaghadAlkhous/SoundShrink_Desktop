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
using System.Drawing.Drawing2D;
using System.IO;
namespace SoundShrink_Desktop
{
    public partial class Form1 : Form
    {
        private bool _isUserSeeking = false;
        private AudioFileReader _currentReader;
        private readonly AudioService _audioService;
        private readonly AudioAnalyzer _analyzer;
        private WaveOutEvent _player;
        private AudioFileInfo _currentFile;
        private Timer _progressTimer;
        private bool _isPaused;
        private DateTime _playbackStartTime;
        private TimeSpan _playbackOffset;
        private BufferedWaveProvider _bufferedProvider;
        private ContextMenuStrip _fileContextMenu;
        private ICompressionAlgorithm _currentAlgorithm;
        private byte[] _compressedData;
        private CompressionResult _lastCompressionResult;
        private Panel _progressOverlay;
        private ProgressBar _compressionProgressBar;
        private Label _progressLabel;

        // متغيرات مراقبة الضغط المتقدمة
        private CompressionProgressForm _progressForm;
        private CompressionMonitor _monitor;
        private System.Threading.CancellationTokenSource _cancelSource;
        private DateTime _compressionStartTime;
        private long _totalBytesToProcess;
        private long _bytesProcessed;
        public Form1()
        {
            InitializeComponent();
            _audioService = new AudioService();
            _analyzer = new AudioAnalyzer();

            SetupUI();
            SetupContextMenu();
            SetupTimers();
            EnableDragDrop();

            SetupProgressOverlay();
        }

        private void SetupUI()
        {
            SetupInfoPanel();
            SetupWavePanel();
            SetupSpectrumPanel();
            SetupControlsPanel();
        }

        private void SetupInfoPanel()
        {
            AddInfoLabel("اسم الملف:", "lblFileName", 0, 0);
            AddInfoLabel("المدة:", "lblDuration", 1, 0);
            AddInfoLabel("معدل العينات:", "lblSampleRate", 2, 0);
            AddInfoLabel("القنوات:", "lblChannels", 3, 0);
            AddInfoLabel("حجم الملف:", "lblFileSize", 0, 1);
            AddInfoLabel("Bit Rate:", "lblBitRate", 1, 1);
            AddInfoLabel("Bits/Sample:", "lblBitsPerSample", 2, 1);
            AddInfoLabel("الترميز:", "lblCodec", 3, 1);
        }

        private void SetupWavePanel()
        {
            lblWaveformTitle.Text = "الشكل الموجي (Waveform)";
            lblWaveformTitle.ForeColor = Color.FromArgb(0, 191, 255);
            lblWaveformTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblWaveformTitle.AutoSize = true;
            lblWaveformTitle.Location = new Point(10, 5);
            wavePanel.Controls.Add(lblWaveformTitle);

            wavePictureBox.BackColor = Color.FromArgb(15, 15, 20);
            wavePictureBox.Visible = false;
            wavePictureBox.Dock = DockStyle.Bottom;
            wavePictureBox.Height = 170;
            wavePictureBox.Paint += WavePictureBox_Paint;
            wavePanel.Controls.Add(wavePictureBox);
        }

        private void SetupSpectrumPanel()
        {
            lblSpectrumTitle.Text = "محلل الطيف الترددي (Spectrum Analyzer)";
            lblSpectrumTitle.ForeColor = Color.FromArgb(0, 255, 128);
            lblSpectrumTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSpectrumTitle.AutoSize = true;
            lblSpectrumTitle.Location = new Point(10, 5);
            spectrumPanel.Controls.Add(lblSpectrumTitle);

            spectrumPictureBox.BackColor = Color.FromArgb(15, 15, 20);
            spectrumPictureBox.Visible = false;
            spectrumPictureBox.Dock = DockStyle.Bottom;
            spectrumPictureBox.Height = 170;
            spectrumPictureBox.Paint += SpectrumPictureBox_Paint;
            spectrumPanel.Controls.Add(spectrumPictureBox);
        }

        private void SetupControlsPanel()
        {
            progressBar.MouseDown += ProgressBar_MouseDown;
            progressBar.MouseUp += ProgressBar_MouseUp;
            progressBar.Scroll += ProgressBar_Scroll;

            var buttonsPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.Transparent
            };

            SetupButton(btnPlay, "▶ تشغيل", Color.FromArgb(0, 191, 255), 50, BtnPlay_Click);
            SetupButton(btnPause, "⏸ إيقاف مؤقت", Color.FromArgb(255, 193, 7), 190, BtnPause_Click);
            btnPause.Enabled = false;
            SetupButton(btnStop, "⏹ إيقاف", Color.FromArgb(220, 53, 69), 340, BtnStop_Click);
            btnStop.Enabled = false;

            // تعريف الـ MenuStrip
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nonlinearQuantizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dpcmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.predictiveCodingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deltaModulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adaptiveDeltaModulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCompressedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            // menuStrip1
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuStrip1.BackColor = Color.FromArgb(45, 45, 50);
            this.menuStrip1.ForeColor = Color.White;
            this.menuStrip1.Font = new Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
    this.fileToolStripMenuItem});

            // fileToolStripMenuItem
            this.fileToolStripMenuItem.Text = "ملف";
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
    this.changeFileToolStripMenuItem,
    this.removeFileToolStripMenuItem,
    this.compressToolStripMenuItem,
    this.saveCompressedToolStripMenuItem});

            // changeFileToolStripMenuItem
            this.changeFileToolStripMenuItem.Text = "🔄 تغيير الملف";
            this.changeFileToolStripMenuItem.Click += new System.EventHandler(this.ChangeFile_Click);

            // removeFileToolStripMenuItem
            this.removeFileToolStripMenuItem.Text = "🗑️ إزالة الملف";
            this.removeFileToolStripMenuItem.Click += new System.EventHandler(this.RemoveFile_Click);

            // compressToolStripMenuItem (قائمة فرعية للخوارزميات)
            this.compressToolStripMenuItem.Text = "🗜️ ضغط الملف";
            this.compressToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
    this.nonlinearQuantizationToolStripMenuItem,
    this.dpcmToolStripMenuItem,
    this.predictiveCodingToolStripMenuItem,
    this.deltaModulationToolStripMenuItem,
    this.adaptiveDeltaModulationToolStripMenuItem});

            // الخوارزميات الخمس
            this.nonlinearQuantizationToolStripMenuItem.Text = "Nonlinear Quantization";
            this.nonlinearQuantizationToolStripMenuItem.Click += new System.EventHandler(this.Compress_NonlinearQuantization_Click);

            this.dpcmToolStripMenuItem.Text = "DPCM";
            this.dpcmToolStripMenuItem.Click += new System.EventHandler(this.Compress_DPCM_Click);

            this.predictiveCodingToolStripMenuItem.Text = "Predictive Differential Coding";
            this.predictiveCodingToolStripMenuItem.Click += new System.EventHandler(this.Compress_PredictiveCoding_Click);

            this.deltaModulationToolStripMenuItem.Text = "Delta Modulation";
            this.deltaModulationToolStripMenuItem.Click += new System.EventHandler(this.Compress_DeltaModulation_Click);

            this.adaptiveDeltaModulationToolStripMenuItem.Text = "Adaptive Delta Modulation";
            this.adaptiveDeltaModulationToolStripMenuItem.Click += new System.EventHandler(this.Compress_AdaptiveDeltaModulation_Click);

            // saveCompressedToolStripMenuItem
            this.saveCompressedToolStripMenuItem.Text = "💾 حفظ الملف المضغوط";
            this.saveCompressedToolStripMenuItem.Click += new System.EventHandler(this.SaveCompressedFile_Click);
            this.saveCompressedToolStripMenuItem.Enabled = false; // مفعل فقط بعد الضغط

            // إضافة الـ MenuStrip للـ Controls
            this.Controls.Add(this.menuStrip1);
            this.menuStrip1.BringToFront();

            buttonsPanel.Controls.AddRange(new Control[] { btnPlay, btnPause, btnStop });

            var timePanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 200,
                BackColor = Color.Transparent
            };

            lblCurrentTime.Text = "00:00";
            lblCurrentTime.ForeColor = Color.FromArgb(0, 191, 255);
            lblCurrentTime.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCurrentTime.AutoSize = true;
            lblCurrentTime.Location = new Point(20, 15);

            lblTotalTime.Text = "00:00";
            lblTotalTime.ForeColor = Color.Gray;
            lblTotalTime.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalTime.AutoSize = true;
            lblTotalTime.Location = new Point(100, 15);

            timePanel.Controls.AddRange(new Control[] {
                lblCurrentTime,
                new Label { Text = "/", ForeColor = Color.Gray, Location = new Point(85, 18), AutoSize = true },
                lblTotalTime
            });
            buttonsPanel.Controls.Add(timePanel);
            controlsPanel.Controls.Add(buttonsPanel);
        }

        private void AddInfoLabel(string text, string name, int col, int row)
        {
            var lblTitle = new Label
            {
                Text = text,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8F),
                AutoSize = true,
                Dock = DockStyle.Fill
            };
            infoPanel.Controls.Add(lblTitle, col, row * 2);

            var lblValue = new Label
            {
                Name = name,
                Text = "-",
                ForeColor = Color.FromArgb(0, 191, 255),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Dock = DockStyle.Fill
            };
            infoPanel.Controls.Add(lblValue, col, row * 2 + 1);
        }

        private void SetupButton(Button btn, string text, Color color, int x, EventHandler clickHandler)
        {
            btn.Text = text;
            btn.Size = new Size(text.Contains("إيقاف مؤقت") ? 130 : 120, 45);
            btn.Location = new Point(x, 12);
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += clickHandler;
        }

        private void SetupContextMenu()
        {
            _fileContextMenu = new ContextMenuStrip();
            _fileContextMenu.Items.Add(new ToolStripMenuItem("🔄 تغيير الملف", null, ChangeFile_Click));
            _fileContextMenu.Items.Add(new ToolStripMenuItem("🗑️ إزالة الملف", null, RemoveFile_Click));
            _fileContextMenu.Font = new Font("Segoe UI", 9F);
            _fileContextMenu.BackColor = Color.FromArgb(45, 45, 50);
            _fileContextMenu.ForeColor = Color.White;
        }

        private void SetupTimers()
        {
            _progressTimer = new Timer { Interval = 200 };
            _progressTimer.Tick += ProgressTimer_Tick;
        }

        private void EnableDragDrop()
        {
            dropPanel.AllowDrop = true;
        }

        #region Helper Methods

        private bool IsAudioFile(string path)
        {
            string ext = System.IO.Path.GetExtension(path).ToLower();
            return ext == ".wav" || ext == ".mp3";
        }

        private void OpenFileBrowser()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.wav;*.mp3",
                Title = "اختر ملف صوتي"
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

                _currentFile = _audioService.LoadAudio(filePath);
                UpdateInfoLabels();
                _analyzer.Clear();

                wavePictureBox.Visible = true;
                spectrumPictureBox.Visible = true;
                controlsPanel.Visible = true;
                dropPanel.Visible = false;

                progressBar.Maximum = (int)_currentFile.Duration.TotalSeconds;
                lblTotalTime.Text = FormatTime(_currentFile.Duration);
                this.Text = $"SoundShrink Pro - {_currentFile.FileName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UpdateInfoLabels()
        {
            SetLabel("lblFileName", _currentFile.FileName);
            SetLabel("lblDuration", FormatTime(_currentFile.Duration));
            SetLabel("lblSampleRate", $"{_currentFile.SampleRate} Hz");
            SetLabel("lblChannels", _currentFile.Channels == 1 ? "Mono" : "Stereo");
            SetLabel("lblFileSize", $"{_currentFile.FileSizeBytes / (1024.0 * 1024.0):F2} MB");
            SetLabel("lblBitRate", $"{_currentFile.AverageBytesPerSecond * 8 / 1000} kbps");
            SetLabel("lblBitsPerSample", $"{_currentFile.BitsPerSample}-bit");
            SetLabel("lblCodec", System.IO.Path.GetExtension(_currentFile.FilePath).ToUpper().TrimStart('.'));
        }

        private void SetLabel(string name, string value)
        {
            var controls = this.Controls.Find(name, true);
            if (controls.Length > 0) controls[0].Text = value;
        }

        private void ResetInfoLabels()
        {
            string[] labels = { "lblFileName", "lblDuration", "lblSampleRate", "lblChannels", "lblFileSize", "lblBitRate", "lblBitsPerSample", "lblCodec" };
            foreach (var label in labels) SetLabel(label, "-");
        }

        private string FormatTime(TimeSpan time) => $"{time.Minutes:D2}:{time.Seconds:D2}";

        #endregion

        #region File Management

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
            _bufferedProvider = null;
            _analyzer.Clear();

            _currentFile = null;
            _isPaused = false;
            _playbackOffset = TimeSpan.Zero;

            progressBar.Value = 0;
            progressBar.Maximum = 100;
            lblCurrentTime.Text = "00:00";
            lblTotalTime.Text = "00:00";

            controlsPanel.Visible = false;
            dropPanel.Visible = true;
            dropPanel.BringToFront();

            ResetInfoLabels();

            wavePictureBox.Invalidate();
            spectrumPictureBox.Invalidate();

            this.Text = "SoundShrink Pro - ضغط الملفات الصوتية";

            btnPlay.Enabled = true;
            btnPause.Enabled = false;
            btnPause.Text = "⏸ إيقاف مؤقت";
            btnStop.Enabled = false;
        }

        private void ChangeFile_Click(object sender, EventArgs e)
        {
            if (_player?.PlaybackState == PlaybackState.Playing)
            {
                if (MessageBox.Show("هل تريد إيقاف التشغيل وتغيير الملف؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            UnloadCurrentFile();
            OpenFileBrowser();
        }

        private void RemoveFile_Click(object sender, EventArgs e)
        {
            if (_player?.PlaybackState == PlaybackState.Playing)
            {
                if (MessageBox.Show("هل تريد إيقاف التشغيل وإزالة الملف؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            UnloadCurrentFile();
        }

        #endregion

        #region Audio Playback

        private void PlayAudio()
        {
            if (_player == null || _player.PlaybackState == PlaybackState.Stopped)
            {
                var reader = _audioService.GetReader();
                if (reader == null) return;

                reader.Position = 0;
                _analyzer.Clear();
                _currentReader = reader;

                _bufferedProvider = new BufferedWaveProvider(reader.WaveFormat)
                {
                    BufferDuration = TimeSpan.FromMinutes(2),
                    DiscardOnBufferOverflow = false
                };

                _player?.Dispose();
                _player = new WaveOutEvent();
                _player.Init(_bufferedProvider);

                Task.Run(() => ReadSamplesInBackground(reader));

                _playbackStartTime = DateTime.Now;
                _playbackOffset = TimeSpan.Zero;

                _player.Play();
                _progressTimer.Start();

                btnPlay.Enabled = false;
                btnPause.Enabled = true;
                btnStop.Enabled = true;
                _isPaused = false;
            }
            else if (_isPaused)
            {
                if (_currentReader != null)
                {
                    Task.Run(() => ReadSamplesInBackground(_currentReader));
                }

                _playbackStartTime = DateTime.Now;
                _player.Play();
                _progressTimer.Start();

                btnPlay.Text = "▶ تشغيل";
                btnPause.Text = "⏸ إيقاف مؤقت";
                _isPaused = false;
            }
        }

        private void ReadSamplesInBackground(AudioFileReader reader)
        {
            float[] buffer = new float[4096];
            int samplesRead;

            try
            {
                while (_player != null && _player.PlaybackState != PlaybackState.Stopped)
                {
                    if (_player.PlaybackState == PlaybackState.Paused)
                    {
                        System.Threading.Thread.Sleep(50);
                        continue;
                    }

                    samplesRead = reader.Read(buffer, 0, buffer.Length);
                    if (samplesRead == 0) break;

                    byte[] byteBuffer = new byte[samplesRead * sizeof(float)];
                    Buffer.BlockCopy(buffer, 0, byteBuffer, 0, byteBuffer.Length);

                    _bufferedProvider?.AddSamples(byteBuffer, 0, byteBuffer.Length);

                    for (int i = 0; i < samplesRead; i++)
                    {
                        _analyzer.AddSample(buffer[i]);
                    }

                    UpdateBoxHandler(wavePictureBox);
                    UpdateBoxHandler(spectrumPictureBox);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ReadSamplesInBackground: {ex.Message}");
            }
        }

        private void UpdateBoxHandler(PictureBox box)
        {
            if (box.Visible)
            {
                if (box.InvokeRequired) box.Invoke(new Action(() => box.Invalidate()));
                else box.Invalidate();
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
                    btnPause.Text = "▶ استئناف";
                    _isPaused = true;
                }
                else if (_isPaused)
                {
                    PlayAudio();
                }
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

                progressBar.Value = 0;
                lblCurrentTime.Text = "00:00";
                _playbackOffset = TimeSpan.Zero;
                _analyzer.Clear();
                _currentReader = null;

                btnPlay.Enabled = true;
                btnPause.Enabled = false;
                btnPause.Text = "⏸ إيقاف مؤقت";
                btnStop.Enabled = false;
                _isPaused = false;

                wavePictureBox.Invalidate();
                spectrumPictureBox.Invalidate();
            }
        }

        #endregion

        #region Progress Bar & Timer

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            if (_isUserSeeking) return;

            if (_player?.PlaybackState == PlaybackState.Playing && _currentFile != null)
            {
                TimeSpan elapsed = DateTime.Now - _playbackStartTime + _playbackOffset;
                TimeSpan currentTime = elapsed < _currentFile.Duration ? elapsed : _currentFile.Duration;

                if (progressBar.Maximum > 0)
                    progressBar.Value = (int)currentTime.TotalSeconds;

                lblCurrentTime.Text = FormatTime(currentTime);
            }
        }

        private void ProgressBar_MouseDown(object sender, MouseEventArgs e) => _isUserSeeking = true;
        private void ProgressBar_MouseUp(object sender, MouseEventArgs e) => _isUserSeeking = false;

        private void ProgressBar_Scroll(object sender, EventArgs e)
        {
            var reader = _audioService.GetReader();
            if (reader != null && _player != null)
            {
                TimeSpan targetTime = TimeSpan.FromSeconds(progressBar.Value);
                lblCurrentTime.Text = FormatTime(targetTime);
                reader.CurrentTime = targetTime;
                _bufferedProvider?.ClearBuffer();

                if (_player.PlaybackState == PlaybackState.Playing)
                {
                    _playbackStartTime = DateTime.Now;
                    _playbackOffset = targetTime;
                    _analyzer.Clear();
                }
            }
        }

        #endregion

        #region Visualization Painting

        private void SpectrumPictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(15, 15, 20));

            var spectrumData = _analyzer.GetSpectrumData();
            if (spectrumData.Count == 0)
            {
                TextRenderer.DrawText(g, "محلل الطيف - بانتظار البيانات...", new Font("Segoe UI", 11F),
                    new Rectangle(0, 0, spectrumPictureBox.Width, spectrumPictureBox.Height),
                    Color.Gray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            int width = spectrumPictureBox.Width;
            int height = spectrumPictureBox.Height;
            int barWidth = width / spectrumData.Count;
            int spacing = 2;

            for (int i = 0; i < spectrumData.Count; i++)
            {
                double intensity = spectrumData[i];
                int barHeight = (int)Math.Min(intensity, height - 10);
                barHeight = Math.Max(barHeight, 4);

                int x = i * barWidth;
                int y = height - barHeight;

                Color topColor = intensity > height * 0.6 ? Color.FromArgb(255, 0, 100) : Color.FromArgb(0, 255, 128);
                Color bottomColor = Color.FromArgb(0, 100, 255);

                using (var brush = new LinearGradientBrush(new Point(x, y), new Point(x, height), topColor, bottomColor))
                {
                    g.FillRectangle(brush, x + spacing / 2, y, barWidth - spacing, barHeight);
                }
            }

            using (Pen pen = new Pen(Color.FromArgb(0, 255, 128), 2))
                g.DrawLine(pen, 0, height - 1, width, height - 1);
        }

        private void WavePictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(15, 15, 20));

            var samples = _analyzer.GetWaveSamples();
            if (samples.Count < 2)
            {
                TextRenderer.DrawText(g, "بانتظار البيانات...", new Font("Segoe UI", 12F),
                    new Rectangle(0, 0, wavePictureBox.Width, wavePictureBox.Height),
                    Color.Gray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            int width = wavePictureBox.Width;
            int height = wavePictureBox.Height;
            int centerY = height / 2;

            using (Pen gridPen = new Pen(Color.FromArgb(40, 40, 50), 1))
                g.DrawLine(gridPen, 0, centerY, width, centerY);

            using (Pen wavePen = new Pen(Color.FromArgb(0, 191, 255), 1.5f))
            {
                float step = (float)width / Math.Min(samples.Count, _analyzer.MaxSamples);
                var points = new System.Collections.Generic.List<Point>();
                int startIndex = Math.Max(0, samples.Count - _analyzer.MaxSamples);

                for (int i = startIndex; i < samples.Count; i++)
                {
                    int x = (int)((i - startIndex) * step);
                    int y = centerY - (int)(samples[i] * (centerY - 15));
                    points.Add(new Point(x, y));
                }

                if (points.Count > 1) g.DrawLines(wavePen, points.ToArray());
            }

            using (Pen pen = new Pen(Color.FromArgb(0, 120, 215), 1))
                g.DrawRectangle(pen, 0, 0, width - 1, height - 1);
        }

        #endregion

        #region Drop Zone Events 

        private void DropZone_Click(object sender, EventArgs e)
        {
            OpenFileBrowser(); 
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

        #endregion

        #region Event Handlers & Cleanup

        private void BtnPlay_Click(object sender, EventArgs e) => PlayAudio();
        private void BtnPause_Click(object sender, EventArgs e) => TogglePause();
        private void BtnStop_Click(object sender, EventArgs e) => StopAudio();

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnloadCurrentFile();
            _progressTimer?.Dispose();
            _audioService?.CloseReader();
            base.OnFormClosing(e);
        }

        #region Compression Event Handlers

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
                    {
                        samples.Add(buffer[i]);
                    }
                }
            }

            return samples.ToArray();
        }

        // ✅ دالة مساعدة لتحويل float[] إلى byte[] (Little-Endian)
        private byte[] FloatsToBytes(float[] samples)
        {
            byte[] bytes = new byte[samples.Length * 4];
            Buffer.BlockCopy(samples, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void Compress_NonlinearQuantization_Click(object sender, EventArgs e)
        {
            CompressAudio(new NonlinearQuantization());
        }

        private void Compress_DPCM_Click(object sender, EventArgs e)
        {
            CompressAudio(new DPCM());
        }

        private void Compress_PredictiveCoding_Click(object sender, EventArgs e)
        {
            CompressAudio(new PredictiveDifferentialCoding());
        }

        private void Compress_DeltaModulation_Click(object sender, EventArgs e)
        {
            CompressAudio(new DeltaModulation());
        }

        private void Compress_AdaptiveDeltaModulation_Click(object sender, EventArgs e)
        {
            CompressAudio(new AdaptiveDeltaModulation());
        }

        private void CompressAudio(ICompressionAlgorithm algorithm)
        {
            if (_currentFile == null)
            {
                MessageBox.Show("الرجاء اختيار ملف صوتي أولاً", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // تهيئة المراقبة
            _monitor = new CompressionMonitor();
            _cancelSource = new System.Threading.CancellationTokenSource();
            _compressionStartTime = DateTime.Now;
            _totalBytesToProcess = _currentFile.FileSizeBytes;
            _bytesProcessed = 0;

            // إنشاء وعرض نموذج المراقبة المتقدم
            _progressForm = new CompressionProgressForm();
            _progressForm.CancelRequested += (s, e) => {
                _cancelSource.Cancel();
            };

            _progressForm.Show(this);
            _progressForm.BringToFront();

            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        // ✅ قراءة العينات الصوتية الحقيقية بدلاً من بايتات الملف الخام
                        float[] samples = LoadAudioSamples(_currentFile.FilePath);
                        byte[] audioData = FloatsToBytes(samples);

                        // محاكاة التقدم مع التحديث المستمر
                        int chunkSize = audioData.Length / 100;
                        for (int i = 0; i <= 100; i++)
                        {
                            if (_cancelSource.Token.IsCancellationRequested)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    _progressForm.SetCancelled();
                                }));

                                System.Threading.Thread.Sleep(1000);

                                this.Invoke(new Action(() =>
                                {
                                    _progressForm.Close();
                                }));

                                return;
                            }

                            _bytesProcessed = (long)(i * chunkSize);
                            UpdateMonitorProgress(i, algorithm);
                            System.Threading.Thread.Sleep(50);
                        }

                        // الضغط الفعلي
                        _compressedData = algorithm.Compress(
                            audioData,
                            _currentFile.SampleRate,
                            _currentFile.BitsPerSample,
                            _currentFile.Channels);


                        byte[] decompressed =
                            algorithm.Decompress(
                                _compressedData,
                                _currentFile.SampleRate,
                                _currentFile.BitsPerSample,
                                _currentFile.Channels);

                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(
                                $"Original Samples = {audioData.Length / 4}\n" +
                                $"Decompressed Samples = {decompressed.Length / 4}",
                                "Decompression Check");
                        }));

                        _currentAlgorithm = algorithm;
                        _lastCompressionResult = algorithm.GetCompressionStats();


                        // التحديث النهائي
                        this.Invoke(new Action(() =>
                        {
                            _progressForm.Close();
                            saveCompressedToolStripMenuItem.Enabled = true;
                            ShowCompressionReport(_lastCompressionResult, algorithm.AlgorithmName);
                        }));
                    }
                    catch (OperationCanceledException)
                    {
                        // تم الإلغاء بنجاح
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() =>
                        {
                            _progressForm?.Close();
                            MessageBox.Show($"خطأ في الضغط: {ex.Message}", "خطأ",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                }, _cancelSource.Token);
            }
            catch (Exception ex)
            {
                _progressForm?.Close();
                MessageBox.Show($"خطأ في بدء الضغط: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateMonitorProgress(int percentage, ICompressionAlgorithm algorithm)
        {
            TimeSpan elapsed = DateTime.Now - _compressionStartTime;
            double speedMBps = (_bytesProcessed / (1024.0 * 1024.0)) / elapsed.TotalSeconds;
            double remainingBytes = _totalBytesToProcess - _bytesProcessed;
            double remainingSeconds = speedMBps > 0 ? remainingBytes / (speedMBps * 1024 * 1024) : 0;
            double estimatedRatio = _totalBytesToProcess > 0 ?
                (double)_totalBytesToProcess / Math.Max(_bytesProcessed, 1) : 1;

            _monitor.ProgressPercentage = percentage;
            _monitor.CompressionRatio = estimatedRatio;
            _monitor.ProcessingSpeedMBps = speedMBps;
            _monitor.ElapsedTime = elapsed;
            _monitor.EstimatedRemaining = TimeSpan.FromSeconds(remainingSeconds);
            _monitor.ProcessedBytes = _bytesProcessed;
            _monitor.TotalBytes = _totalBytesToProcess;

            _progressForm?.UpdateProgress(_monitor);
        }
        private void SetupProgressOverlay()
        {
            _progressOverlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 35, 230), // شبه شفاف
                Visible = false,
                Enabled = false
            };

            var progressPanel = new Panel
            {
                Size = new Size(400, 120),
                BackColor = Color.FromArgb(45, 45, 50),
                Location = new Point((this.ClientSize.Width - 400) / 2, (this.ClientSize.Height - 120) / 2),
                Anchor = AnchorStyles.None
            };
            progressPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(0, 255, 128), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, progressPanel.Width - 1, progressPanel.Height - 1);
                }
            };

            _progressLabel = new Label
            {
                Text = "جاري ضغط الملف...",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };

            _compressionProgressBar = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee, // شريط متحرك غير محدد
                ForeColor = Color.FromArgb(0, 255, 128), // ✅ أخضر
                Height = 20,
                Dock = DockStyle.Top,
                Margin = new Padding(20, 5, 20, 10)
            };

            var statusLabel = new Label
            {
                Text = "يرجى الانتظار...",
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 25
            };

            progressPanel.Controls.Add(statusLabel);
            progressPanel.Controls.Add(_compressionProgressBar);
            progressPanel.Controls.Add(_progressLabel);
            _progressOverlay.Controls.Add(progressPanel);

            this.Controls.Add(_progressOverlay);
            _progressOverlay.BringToFront();
        }

        private void ShowCompressionProgress(string message)
        {
            _progressLabel.Text = message;
            _compressionProgressBar.Style = ProgressBarStyle.Marquee;
            _progressOverlay.Visible = true;
            _progressOverlay.Enabled = true;
            _progressOverlay.BringToFront();
            this.Refresh();
            Application.DoEvents(); // تحديث الواجهة فوراً
        }

        private void HideCompressionProgress()
        {
            _progressOverlay.Visible = false;
            _progressOverlay.Enabled = false;
            this.Refresh();
        }

        private void UpdateProgressPercentage(int percentage, string message)
        {
            // 🔍 التحقق مما إذا كنا في خيط خلفي
            if (this.InvokeRequired)
            {
                // إذا نعم، نرسل الأمر للخيط الرئيسي لتنفيذه
                this.Invoke(new Action<int, string>(UpdateProgressPercentage), percentage, message);
                return;
            }

            // 🔒 الآن نحن في الخيط الرئيسي، يمكننا تحديث الواجهة بأمان
            _compressionProgressBar.Style = ProgressBarStyle.Continuous;
            _compressionProgressBar.Value = percentage;
            _progressLabel.Text = message;

            // تحديث الواجهة فوراً
            this.Refresh();
            Application.DoEvents();
        }

        private void ShowCompressionReport(CompressionResult result, string algorithmName)
        {
            string report = $@"
خوارزمية الضغط: {algorithmName}

حجم الملف الأصلي: {result.OriginalSize / 1024.0:F2} KB
حجم الملف المضغوط: {result.CompressedSize / 1024.0:F2} KB
نسبة الضغط: {result.CompressionRatio:F2}:1
الوقت المستغرق: {result.ProcessingTime.TotalMilliseconds:F2} ms
";

            MessageBox.Show(report, "تقرير الضغط",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveCompressedFile_Click(object sender, EventArgs e)
        {
            if (_compressedData == null || _currentAlgorithm == null)
            {
                MessageBox.Show("لا توجد بيانات مضغوطة للحفظ", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Compressed Audio Files|*.compressed|All Files|*.*",
                Title = "حفظ الملف المضغوط",
                // ✅ صحيح:
                FileName = $"{Path.GetFileNameWithoutExtension(_currentFile.FilePath)}_compressed.compressed"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // حفظ البيانات المضغوطة
                    File.WriteAllBytes(saveDialog.FileName, _compressedData);

                    // حفظ معلومات إضافية عن الضغط في ملف منفصل
                    string infoFile = saveDialog.FileName + ".info";
                    string info = $@"
OriginalFile: {_currentFile.FilePath}
Algorithm: {_currentAlgorithm.AlgorithmName}
SampleRate: {_currentFile.SampleRate}
Channels: {_currentFile.Channels}
BitsPerSample: {_currentFile.BitsPerSample}
OriginalSize: {_currentFile.FileSizeBytes}
CompressedSize: {_compressedData.Length}
CompressionRatio: {_lastCompressionResult.CompressionRatio}
";
                    File.WriteAllText(infoFile, info);

                    MessageBox.Show($"تم حفظ الملف المضغوط بنجاح!\n\nالملف: {saveDialog.FileName}\nمعلومات إضافية: {infoFile}",
                        "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"خطأ في الحفظ: {ex.Message}", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e) { }

        #endregion
    }
}