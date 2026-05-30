using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop
{
    public partial class CompressionProgressForm : Form
    {
        private ProgressBar _progressBar;
        private Label _lblProgress;
        private Panel _lblRatio;        // ✅ Panel وليس Label
        private Panel _lblSpeed;        // ✅ Panel وليس Label
        private Panel _lblElapsedTime;  // ✅ Panel وليس Label
        private Panel _lblRemaining;    // ✅ Panel وليس Label
        private Button _btnCancel;
        private Panel _chartPanel;
        private Timer _chartTimer;

        private System.Collections.Generic.List<float> _ratioHistory;
        private System.Collections.Generic.List<float> _speedHistory;
        private const int MaxChartPoints = 100;

        public event EventHandler CancelRequested;

        public CompressionProgressForm()  // ✅ Constructor واحد فقط
        {
            InitializeComponent();
            _ratioHistory = new System.Collections.Generic.List<float>();
            _speedHistory = new System.Collections.Generic.List<float>();
            _chartTimer = new Timer { Interval = 100 };
            _chartTimer.Tick += ChartTimer_Tick;
            _chartTimer.Start();
        }

        private void InitializeComponent()
        {
            this.Text = "مراقبة عملية الضغط";
            this.Size = new Size(700, 500);
            this.BackColor = Color.FromArgb(30, 30, 35);
            this.Font = new Font("Segoe UI", 9F);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            _progressBar = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 30,
                Style = ProgressBarStyle.Continuous,
                ForeColor = Color.FromArgb(0, 255, 128)
            };
            this.Controls.Add(_progressBar);

            _lblProgress = new Label
            {
                Text = "0%",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };
            this.Controls.Add(_lblProgress);

            _chartPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(20, 20, 25),
                Margin = new Padding(10)
            };
            _chartPanel.Paint += ChartPanel_Paint;
            this.Controls.Add(_chartPanel);

            var infoPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 200,
                ColumnCount = 2,
                RowCount = 3,
                BackColor = Color.FromArgb(40, 40, 45),
                Padding = new Padding(10)
            };

            infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            infoPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
            infoPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
            infoPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));

            _lblRatio = CreateInfoLabel("نسبة الضغط:", "0:1");
            _lblSpeed = CreateInfoLabel("سرعة المعالجة:", "0 MB/s");
            _lblElapsedTime = CreateInfoLabel("الوقت المنقضي:", "00:00");
            _lblRemaining = CreateInfoLabel("الوقت المتبقي:", "--:--");

            infoPanel.Controls.Add(_lblRatio, 0, 0);
            infoPanel.Controls.Add(_lblSpeed, 1, 0);
            infoPanel.Controls.Add(_lblElapsedTime, 0, 1);
            infoPanel.Controls.Add(_lblRemaining, 1, 1);

            _btnCancel = new Button
            {
                Text = "❌ إلغاء العملية",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnCancel.FlatAppearance.BorderSize = 0;
            _btnCancel.Click += (s, e) => {
                if (MessageBox.Show("هل تريد إلغاء عملية الضغط؟", "تأكيد الإلغاء",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CancelRequested?.Invoke(this, EventArgs.Empty);
                }
            };
            infoPanel.Controls.Add(_btnCancel, 0, 2);
            infoPanel.SetColumnSpan(_btnCancel, 2);

            this.Controls.Add(infoPanel);
        }

        private Panel CreateInfoLabel(string title, string value)
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            var lblTitle = new Label
            {
                Text = title,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(10, 5)
            };

            var lblValue = new Label
            {
                Text = value,
                ForeColor = Color.FromArgb(0, 191, 255),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 25)
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblValue);
            return panel;
        }

        public void UpdateProgress(CompressionMonitor monitor)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgress(monitor)));
                return;
            }

            _progressBar.Value = Math.Min(100, monitor.ProgressPercentage);
            _lblProgress.Text = $"{monitor.ProgressPercentage:F1}%";

            // تحديث النصوص داخل Panels
            if (_lblRatio.Controls.Count > 1)
                _lblRatio.Controls[1].Text = $"{monitor.CompressionRatio:F2}:1";
            if (_lblSpeed.Controls.Count > 1)
                _lblSpeed.Controls[1].Text = $"{monitor.ProcessingSpeedMBps:F2} MB/s";
            if (_lblElapsedTime.Controls.Count > 1)
                _lblElapsedTime.Controls[1].Text = monitor.ElapsedTime.ToString(@"mm\:ss");
            if (_lblRemaining.Controls.Count > 1)
                _lblRemaining.Controls[1].Text = monitor.EstimatedRemaining.ToString(@"mm\:ss");

            _ratioHistory.Add((float)monitor.CompressionRatio);
            _speedHistory.Add((float)monitor.ProcessingSpeedMBps);

            if (_ratioHistory.Count > MaxChartPoints)
            {
                _ratioHistory.RemoveAt(0);
                _speedHistory.RemoveAt(0);
            }

            _chartPanel.Invalidate();
        }

        private void ChartPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int width = _chartPanel.Width;
            int height = _chartPanel.Height;
            int chartHeight = height / 2;

            // ✅ تمرير جميع المعاملات بما فيها lineColor و fillColor
            DrawChart(g, _ratioHistory, "نسبة الضغط", 0, chartHeight - 10, width, chartHeight - 10,
                Color.FromArgb(0, 255, 128), Color.FromArgb(0, 100, 255));

            DrawChart(g, _speedHistory, "السرعة (MB/s)", 0, chartHeight + 10, width, chartHeight - 10,
                Color.FromArgb(255, 193, 7), Color.FromArgb(255, 100, 0));
        }

        private void DrawChart(Graphics g, System.Collections.Generic.List<float> data,
            string title, int x, int y, int width, int height, Color lineColor, Color fillColor)
        {
            if (data.Count < 2) return;

            using (Font font = new Font("Segoe UI", 9F, FontStyle.Bold))
            {
                g.DrawString(title, font, Brushes.White, x, y - 20);
            }

            using (Brush bgBrush = new SolidBrush(Color.FromArgb(30, 30, 35)))
            {
                g.FillRectangle(bgBrush, x, y, width, height);
            }

            using (Pen pen = new Pen(lineColor, 2))
            {
                var points = new PointF[data.Count];
                float maxVal = data.Count > 0 ? data.Max() : 1;

                for (int i = 0; i < data.Count; i++)
                {
                    float normalizedVal = data[i] / maxVal;
                    points[i] = new PointF(
                        x + (i * (float)width / (data.Count - 1)),
                        y + height - (normalizedVal * (height - 10)) - 5
                    );
                }

                if (points.Length > 1)
                    g.DrawLines(pen, points);
            }

            using (Pen borderPen = new Pen(Color.FromArgb(60, 60, 70), 1))
            {
                g.DrawRectangle(borderPen, x, y, width, height);
            }
        }

        private void ChartTimer_Tick(object sender, EventArgs e)
        {
            _chartPanel.Invalidate();
        }

        public void SetCancelled()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(SetCancelled));
                return;
            }

            _btnCancel.Text = "✅ تم الإلغاء";
            _btnCancel.Enabled = false;
            _btnCancel.BackColor = Color.Gray;
        }
    }
}