using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop
{
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            this.BackColor = Color.FromArgb(20, 20, 25);
        }
    }

    public partial class CompressionProgressForm : Form
    {
        private Timer _chartTimer;
        private System.Collections.Generic.List<float> _ratioHistory;
        private System.Collections.Generic.List<float> _speedHistory;
        private const int MaxChartPoints = 100;

        public event EventHandler CancelRequested;

        public CompressionProgressForm()
        {
            InitializeComponent();

            _ratioHistory = new System.Collections.Generic.List<float>();
            _speedHistory = new System.Collections.Generic.List<float>();

            _chartTimer = new Timer { Interval = 150 };
            _chartTimer.Tick += ChartTimer_Tick;
            _chartTimer.Start();

            InitializeInfoLabels();
        }

        private void InitializeInfoLabels()
        {
            Color titleColor = Color.FromArgb(148, 163, 184);
            Color valueColor = Color.FromArgb(16, 185, 129);

            // Ratio panel
            var lblRatioTitle = new Label
            {
                Text = "Compression Ratio:",
                ForeColor = titleColor,
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            var lblRatioValue = new Label
            {
                Text = "0:1",
                ForeColor = valueColor,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 25)
            };
            _lblRatio.Controls.Add(lblRatioTitle);
            _lblRatio.Controls.Add(lblRatioValue);

            // Speed panel
            var lblSpeedTitle = new Label
            {
                Text = "Processing Speed:",
                ForeColor = titleColor,
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(10, 5)
            };
            var lblSpeedValue = new Label
            {
                Text = "0 MB/s",
                ForeColor = valueColor,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 25)
            };
            _lblSpeed.Controls.Add(lblSpeedTitle);
            _lblSpeed.Controls.Add(lblSpeedValue);

            // Elapsed time panel
            var lblElapsedTitle = new Label
            {
                Text = "Elapsed Time:",
                ForeColor = titleColor,
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(10, 5)
            };
            var lblElapsedValue = new Label
            {
                Text = "00:00",
                ForeColor = valueColor,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 25)
            };
            _lblElapsedTime.Controls.Add(lblElapsedTitle);
            _lblElapsedTime.Controls.Add(lblElapsedValue);

            // Remaining time panel
            var lblRemainingTitle = new Label
            {
                Text = "Remaining Time:",
                ForeColor = titleColor,
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(10, 5)
            };
            var lblRemainingValue = new Label
            {
                Text = "--:--",
                ForeColor = valueColor,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 25)
            };
            _lblRemaining.Controls.Add(lblRemainingTitle);
            _lblRemaining.Controls.Add(lblRemainingValue);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to cancel the compression?", "Confirm cancellation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CancelRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        public void UpdateProgress(CompressionMonitor monitor)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgress(monitor)));
                return;
            }

            _chartPanel.SuspendLayout();

            try
            {
                _progressBar.Value = Math.Min(100, monitor.ProgressPercentage);
                _lblProgress.Text = $"{monitor.ProgressPercentage:F1}%";

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
            }
            finally
            {
                _chartPanel.ResumeLayout();
            }
        }

        private void ChartPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            int width = _chartPanel.Width;
            int height = _chartPanel.Height;
            int leftMargin = 60;
            int rightMargin = 60;
            int topMargin = 30;
            int bottomMargin = 40;
            int chartWidth = width - leftMargin - rightMargin;
            int chartHeight = height - topMargin - bottomMargin;

            Color bgColor = Color.FromArgb(17, 24, 39);
            Color gridColor = Color.FromArgb(60, 70, 90);
            Color axisColor = Color.FromArgb(100, 110, 130);
            Color ratioLineColor = Color.FromArgb(16, 185, 129);
            Color speedLineColor = Color.FromArgb(255, 193, 7);
            Color titleColor = Color.White;
            Color waitingColor = Color.FromArgb(100, 110, 130);

            // Draw background
            using (Brush bgBrush = new SolidBrush(bgColor))
            {
                g.FillRectangle(bgBrush, 0, 0, width, height);
            }

            // Draw grid
            using (Pen gridPen = new Pen(gridColor, 1))
            {
                for (int i = 0; i <= 5; i++)
                {
                    int y = topMargin + (i * chartHeight / 5);
                    g.DrawLine(gridPen, leftMargin, y, width - rightMargin, y);
                }

                for (int i = 0; i <= 10; i++)
                {
                    int x = leftMargin + (i * chartWidth / 10);
                    g.DrawLine(gridPen, x, topMargin, x, height - bottomMargin);
                }
            }

            // Draw axes
            using (Pen axisPen = new Pen(axisColor, 2))
            {
                g.DrawLine(axisPen, leftMargin, topMargin, leftMargin, height - bottomMargin);
                g.DrawLine(axisPen, width - rightMargin, topMargin, width - rightMargin, height - bottomMargin);
                g.DrawLine(axisPen, leftMargin, height - bottomMargin, width - rightMargin, height - bottomMargin);
            }

            // Draw data if available
            if (_ratioHistory.Count > 1 && _speedHistory.Count > 1)
            {
                float maxRatio = _ratioHistory.Max();
                float maxSpeed = _speedHistory.Max();

                if (maxRatio < 0.001f) maxRatio = 1.0f;
                if (maxSpeed < 0.001f) maxSpeed = 1.0f;

                // Draw ratio line (green - same as success color)
                using (Pen ratioPen = new Pen(ratioLineColor, 3))
                {
                    var points = new PointF[_ratioHistory.Count];
                    for (int i = 0; i < _ratioHistory.Count; i++)
                    {
                        float x = leftMargin + (i * (float)chartWidth / Math.Max(1, _ratioHistory.Count - 1));
                        float normalizedRatio = _ratioHistory[i] / maxRatio;
                        float y = (height - bottomMargin) - (normalizedRatio * chartHeight);
                        points[i] = new PointF(x, y);
                    }
                    if (points.Length > 1)
                        g.DrawLines(ratioPen, points);
                }

                // Draw speed line (yellow)
                using (Pen speedPen = new Pen(speedLineColor, 3))
                {
                    var points = new PointF[_speedHistory.Count];
                    for (int i = 0; i < _speedHistory.Count; i++)
                    {
                        float x = leftMargin + (i * (float)chartWidth / Math.Max(1, _speedHistory.Count - 1));
                        float normalizedSpeed = _speedHistory[i] / maxSpeed;
                        float y = (height - bottomMargin) - (normalizedSpeed * chartHeight);
                        points[i] = new PointF(x, y);
                    }
                    if (points.Length > 1)
                        g.DrawLines(speedPen, points);
                }

                // Draw axis labels
                using (Font axisFont = new Font("Segoe UI", 9F, FontStyle.Bold))
                {
                    using (Brush leftBrush = new SolidBrush(ratioLineColor))
                    {
                        g.DrawString("Compression Ratio", axisFont, leftBrush, 5, topMargin);
                        g.DrawString($"{maxRatio:F2}", axisFont, leftBrush, 5, height - bottomMargin - 20);
                        g.DrawString("1.0", axisFont, leftBrush, 5, topMargin + 10);
                    }

                    using (Brush rightBrush = new SolidBrush(speedLineColor))
                    {
                        g.DrawString("Speed MB/s", axisFont, rightBrush, width - 55, topMargin);
                        g.DrawString($"{maxSpeed:F2}", axisFont, rightBrush, width - 55, height - bottomMargin - 20);
                    }
                }
            }
            else
            {
                using (Font font = new Font("Segoe UI", 12F, FontStyle.Italic))
                using (Brush brush = new SolidBrush(waitingColor))
                {
                    g.DrawString("Waiting for data...", font, brush,
                        width / 2 - 100, height / 2);
                }
            }

            // Draw title
            using (Font titleFont = new Font("Segoe UI", 11F, FontStyle.Bold))
            using (Brush titleBrush = new SolidBrush(titleColor))
            {
                g.DrawString(" Compression Analysis", titleFont, titleBrush, width / 2 - 100, 5);
            }
        }

        private void ChartTimer_Tick(object sender, EventArgs e)
        {
            if (_chartPanel.Visible && (_ratioHistory.Count > 0 || _speedHistory.Count > 0))
            {
                _chartPanel.Invalidate();
            }
        }

        public void SetCancelled()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(SetCancelled));
                return;
            }

            _btnCancel.Text = " Cancelled";
            _btnCancel.Enabled = false;
            _btnCancel.BackColor = Color.FromArgb(100, 110, 130);
        }

        public new void Close()
        {
            for (int i = 100; i >= 0; i -= 10)
            {
                this.Opacity = i / 100.0;
                System.Threading.Thread.Sleep(20);
            }

            base.Close();
        }
    }
}