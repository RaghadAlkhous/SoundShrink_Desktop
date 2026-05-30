using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using SoundShrink_Desktop.Models;
using SoundShrink_Desktop.Services;
using SoundShrink_Desktop.Analyzers;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

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

        public Form1()
        {
            InitializeComponent();
            _audioService = new AudioService();
            _analyzer = new AudioAnalyzer();

            SetupUI();
            SetupContextMenu();
            SetupTimers();
            EnableDragDrop();
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

            Button btnOptions = new Button
            {
                Text = "⋯",
                Size = new Size(45, 45),
                Location = new Point(480, 12),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnOptions.FlatAppearance.BorderSize = 0;
            btnOptions.Click += (s, e) => _fileContextMenu.Show(btnOptions, new Point(0, btnOptions.Height));
            buttonsPanel.Controls.Add(btnOptions);

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

        private void Form1_Load(object sender, EventArgs e) { }

        #endregion
    }
}