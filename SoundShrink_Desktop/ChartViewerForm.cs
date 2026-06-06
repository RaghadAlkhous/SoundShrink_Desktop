using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SoundShrink_Desktop
{
    public partial class ChartViewerForm : Form
    {
        private List<float> _ratioHistory;
        private List<float> _speedHistory;
        private string _algorithmName;
        private TimeSpan _totalTime;
        private double _finalRatio;

        public ChartViewerForm(
            List<float> ratioHistory,
            List<float> speedHistory,
            string algorithmName,
            TimeSpan totalTime,
            double finalRatio)
        {
            _ratioHistory = ratioHistory ?? new List<float>();
            _speedHistory = speedHistory ?? new List<float>();
            _algorithmName = algorithmName ?? "Unknown";
            _totalTime = totalTime;
            _finalRatio = finalRatio;

            InitializeComponent();
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            // تحديث العنوان
            this.Text = $"📊 تحليل عملية الضغط - {_algorithmName}";
            _lblTitle.Text = $"📊 Compression Analysis - {_algorithmName}";

            // تحديث معلومات الأداء
            _lblTime.Text = $"⏱️ Total Time: {_totalTime.TotalSeconds:F2}s";
            _lblRatio.Text = $"📈 Final Ratio: {_finalRatio:F2}x";
            _lblPoints.Text = $"📊 Data Points: {_ratioHistory.Count}";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChartPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.CompositingQuality = CompositingQuality.HighQuality;

            int width = _chartPanel.Width;
            int height = _chartPanel.Height;
            int leftMargin = 70;
            int rightMargin = 70;
            int topMargin = 20;
            int bottomMargin = 50;
            int chartWidth = width - leftMargin - rightMargin;
            int chartHeight = height - topMargin - bottomMargin;

            // الألوان
            Color bgColor = Color.FromArgb(17, 24, 39);
            Color gridColor = Color.FromArgb(60, 70, 90);
            Color axisColor = Color.FromArgb(100, 110, 130);
            Color ratioLineColor = Color.FromArgb(16, 185, 129);
            Color speedLineColor = Color.FromArgb(255, 193, 7);
            Color textColor = Color.FromArgb(200, 200, 200);

            // الخلفية
            using (Brush bgBrush = new SolidBrush(bgColor))
                g.FillRectangle(bgBrush, 0, 0, width, height);

            // الشبكة
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

            // المحاور
            using (Pen axisPen = new Pen(axisColor, 2))
            {
                g.DrawLine(axisPen, leftMargin, topMargin, leftMargin, height - bottomMargin);
                g.DrawLine(axisPen, width - rightMargin, topMargin, width - rightMargin, height - bottomMargin);
                g.DrawLine(axisPen, leftMargin, height - bottomMargin, width - rightMargin, height - bottomMargin);
            }

            // إذا لا توجد بيانات
            if (_ratioHistory.Count < 2 || _speedHistory.Count < 2)
            {
                using (Font font = new Font("Segoe UI", 14F, FontStyle.Italic))
                using (Brush brush = new SolidBrush(Color.FromArgb(100, 110, 130)))
                {
                    g.DrawString("No data available", font, brush, width / 2 - 80, height / 2);
                }
                return;
            }

            // حساب القيم القصوى
            float maxRatio = _ratioHistory.Max();
            float maxSpeed = _speedHistory.Max();
            if (maxRatio < 0.001f) maxRatio = 1.0f;
            if (maxSpeed < 0.001f) maxSpeed = 1.0f;

            // رسم خط نسبة الضغط (أخضر) مع تعبئة
            using (Pen ratioPen = new Pen(ratioLineColor, 3))
            using (Brush ratioFill = new SolidBrush(Color.FromArgb(40, 16, 185, 129)))
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
                {
                    // رسم التعبئة
                    var fillPoints = new PointF[points.Length + 2];
                    Array.Copy(points, fillPoints, points.Length);
                    fillPoints[points.Length] = new PointF(points[points.Length - 1].X, height - bottomMargin);
                    fillPoints[points.Length + 1] = new PointF(points[0].X, height - bottomMargin);
                    g.FillPolygon(ratioFill, fillPoints);

                    // رسم الخط
                    g.DrawLines(ratioPen, points);
                }
            }

            // رسم خط السرعة (أصفر)
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

            // تسميات المحاور
            using (Font axisFont = new Font("Segoe UI", 9F, FontStyle.Bold))
            {
                // المحور الأيسر (نسبة الضغط)
                using (Brush leftBrush = new SolidBrush(ratioLineColor))
                {
                    g.DrawString("Compression Ratio", axisFont, leftBrush, 5, topMargin);
                    g.DrawString($"{maxRatio:F2}x", axisFont, leftBrush, 5, topMargin + 15);
                    g.DrawString("1.0x", axisFont, leftBrush, 5, height - bottomMargin - 20);
                }

                // المحور الأيمن (السرعة)
                using (Brush rightBrush = new SolidBrush(speedLineColor))
                {
                    g.DrawString("Speed (MB/s)", axisFont, rightBrush, width - 65, topMargin);
                    g.DrawString($"{maxSpeed:F2}", axisFont, rightBrush, width - 65, topMargin + 15);
                    g.DrawString("0.00", axisFont, rightBrush, width - 65, height - bottomMargin - 20);
                }

                // المحور السفلي (الوقت)
                using (Brush bottomBrush = new SolidBrush(textColor))
                {
                    g.DrawString("Time →", axisFont, bottomBrush, width / 2 - 20, height - 20);
                }
            }
        }
    }
}