using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop
{
    public partial class CompressionSettingsForm : Form
    {
        private AlgorithmType _algorithmType;
        private CompressionSettings _settings;

        // عناصر التحكم الديناميكية
        private TrackBar _trackBar1;
        private TrackBar _trackBar2;
        private ComboBox _comboBox1;
        private Label _valueLabel1;
        private Label _valueLabel2;
        private Panel _mainPanel;

        public CompressionSettingsForm(AlgorithmType algorithmType, CompressionSettings currentSettings = null)
        {
            _algorithmType = algorithmType;
            _settings = currentSettings ?? new CompressionSettings();

            InitializeComponent();
            BuildUI();
        }

        /// <summary>
        /// بناء الواجهة بالكامل برمجياً
        /// </summary>
        private void BuildUI()
        {
            // === إعدادات النافذة الرئيسية ===
            this.SuspendLayout();
            this.Text = $"⚙️ إعدادات - {GetAlgorithmName()}";
            this.Size = new Size(550, 450);
            this.BackColor = Color.FromArgb(30, 30, 35);
            this.Font = new Font("Segoe UI", 9F);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // === اللوحة الرئيسية (مع حدود جميلة) ===
            _mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 35),
                Padding = new Padding(25)
            };
            _mainPanel.Paint += (s, e) =>
            {
                // رسم خط فاصل علوي ملوّن
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Point(0, 0), new Point(this.Width, 0),
                    Color.FromArgb(0, 191, 255), Color.FromArgb(0, 255, 128)))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, this.Width, 4);
                }
            };
            this.Controls.Add(_mainPanel);

            // === عنوان الخوارزمية ===
            var titleLabel = new Label
            {
                Text = GetAlgorithmName(),
                ForeColor = Color.FromArgb(0, 191, 255),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = false,
                Size = new Size(500, 35),
                Location = new Point(25, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            _mainPanel.Controls.Add(titleLabel);

            // === خط فاصل تحت العنوان ===
            var separator = new Panel
            {
                Size = new Size(480, 2),
                Location = new Point(25, 60),
                BackColor = Color.FromArgb(60, 60, 70)
            };
            _mainPanel.Controls.Add(separator);

            // === إنشاء عناصر التحكم حسب نوع الخوارزمية ===
            int yPos = 85;

            switch (_algorithmType)
            {
                case AlgorithmType.NonlinearQuantization:
                    SetupNonlinearQuantizationControls(ref yPos);
                    break;
                case AlgorithmType.DPCM:
                    SetupDPCMControls(ref yPos);
                    break;
                case AlgorithmType.PredictiveDifferentialCoding:
                    SetupPredictiveCodingControls(ref yPos);
                    break;
                case AlgorithmType.DeltaModulation:
                    SetupDeltaModulationControls(ref yPos);
                    break;
                case AlgorithmType.AdaptiveDeltaModulation:
                    SetupAdaptiveDeltaModulationControls(ref yPos);
                    break;
            }

            // === لوحة الأزرار السفلية ===
            var buttonsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.FromArgb(40, 40, 45)
            };
            buttonsPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(60, 60, 70), 1))
                    e.Graphics.DrawLine(pen, 0, 0, buttonsPanel.Width, 0);
            };
            _mainPanel.Controls.Add(buttonsPanel);

            // === زر موافق ===
            var btnOK = CreateStyledButton("✓  موافق", Color.FromArgb(0, 191, 255), DialogResult.OK);
            btnOK.Location = new Point(buttonsPanel.Width - 240, 18);
            buttonsPanel.Controls.Add(btnOK);

            // === زر إلغاء ===
            var btnCancel = CreateStyledButton("✗  إلغاء", Color.FromArgb(220, 53, 69), DialogResult.Cancel);
            btnCancel.Location = new Point(buttonsPanel.Width - 120, 18);
            buttonsPanel.Controls.Add(btnCancel);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            this.ResumeLayout(false);
        }

        /// <summary>
        /// إنشاء زر مصمم بشكل احترافي
        /// </summary>
        private Button CreateStyledButton(string text, Color backColor, DialogResult result)
        {
            var btn = new Button
            {
                Text = text,
                DialogResult = result,
                Size = new Size(105, 38),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.2f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.2f);
            return btn;
        }

        /// <summary>
        /// إنشاء Label عنوان لقسم الإعداد
        /// </summary>
        private Label CreateSectionLabel(string text, int x, int y)
        {
            var label = new Label
            {
                Text = text,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(x, y),
                AutoSize = true
            };
            _mainPanel.Controls.Add(label);
            return label;
        }

        /// <summary>
        /// إنشاء Label معلومات إضافية
        /// </summary>
        private Label CreateInfoLabel(string text, int x, int y)
        {
            var label = new Label
            {
                Text = "💡 " + text,
                ForeColor = Color.FromArgb(150, 150, 160),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                Location = new Point(x, y),
                AutoSize = true
            };
            _mainPanel.Controls.Add(label);
            return label;
        }

        // ============================================
        // إعدادات الخوارزميات المختلفة
        // ============================================

        private void SetupNonlinearQuantizationControls(ref int yPos)
        {
            CreateSectionLabel("📊 مستويات التكميم (Quantization Levels)", 25, yPos);
            yPos += 35;

            _comboBox1 = CreateStyledComboBox();
            _comboBox1.Location = new Point(25, yPos);

            int[] levels = { 16, 32, 64, 128, 256, 512, 1024 };
            int selectedIndex = -1;
            for (int i = 0; i < levels.Length; i++)
            {
                _comboBox1.Items.Add($"{levels[i]} مستوى");
                if (levels[i] == _settings.QuantizationLevels)
                    selectedIndex = i;
            }
            _comboBox1.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 4;
            _mainPanel.Controls.Add(_comboBox1);

            yPos += 50;
            CreateInfoLabel("كلما زادت المستويات = جودة أعلى ولكن ضغط أقل", 25, yPos);
        }

        private void SetupDPCMControls(ref int yPos)
        {
            CreateSectionLabel("🔢 عدد البتات لكل عينة (Bits Per Sample)", 25, yPos);
            yPos += 35;

            _comboBox1 = CreateStyledComboBox();
            _comboBox1.Location = new Point(25, yPos);

            int[] bits = { 4, 8, 12, 16, 24, 32 };
            int selectedIndex = -1;
            for (int i = 0; i < bits.Length; i++)
            {
                _comboBox1.Items.Add($"{bits[i]}-bit");
                if (bits[i] == _settings.BitsPerSample)
                    selectedIndex = i;
            }
            _comboBox1.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 3;
            _mainPanel.Controls.Add(_comboBox1);

            yPos += 50;
            CreateInfoLabel("بتات أقل = ضغط أعلى ولكن جودة أقل", 25, yPos);
        }

        private void SetupPredictiveCodingControls(ref int yPos)
        {
            CreateSectionLabel("🎯 معامل التنبؤ (Prediction Coefficient)", 25, yPos);
            yPos += 35;

            _trackBar1 = CreateStyledTrackBar(1, 100, (int)(_settings.PredictionCoefficient * 100), 10);
            _trackBar1.Location = new Point(25, yPos);
            _trackBar1.ValueChanged += (s, e) =>
            {
                _valueLabel1.Text = $"{_trackBar1.Value / 100.0:F2}";
            };
            _mainPanel.Controls.Add(_trackBar1);

            yPos += 50;

            _valueLabel1 = CreateValueLabel($"{_settings.PredictionCoefficient:F2}");
            _valueLabel1.Location = new Point(25, yPos);
            _mainPanel.Controls.Add(_valueLabel1);

            yPos += 40;
            CreateInfoLabel("0.9 = اعتماد عالي على العينة السابقة  |  0.5 = اعتماد متوسط", 25, yPos);
        }

        private void SetupDeltaModulationControls(ref int yPos)
        {
            CreateSectionLabel("📏 حجم الخطوة (Step Size)", 25, yPos);
            yPos += 35;

            _trackBar1 = CreateStyledTrackBar(1, 100, (int)(_settings.StepSize * 100), 10);
            _trackBar1.Location = new Point(25, yPos);
            _trackBar1.ValueChanged += (s, e) =>
            {
                _valueLabel1.Text = $"{_trackBar1.Value / 100.0:F3}";
            };
            _mainPanel.Controls.Add(_trackBar1);

            yPos += 50;

            _valueLabel1 = CreateValueLabel($"{_settings.StepSize:F3}");
            _valueLabel1.Location = new Point(25, yPos);
            _mainPanel.Controls.Add(_valueLabel1);

            yPos += 40;
            CreateInfoLabel("خطوات صغيرة = دقة عالية  |  خطوات كبيرة = تكيف سريع", 25, yPos);
        }

        private void SetupAdaptiveDeltaModulationControls(ref int yPos)
        {
            // === حجم الخطوة الأولي ===
            CreateSectionLabel("📏 حجم الخطوة الأولي (Initial Step Size)", 25, yPos);
            yPos += 35;

            _trackBar1 = CreateStyledTrackBar(1, 100, (int)(_settings.InitialStepSize * 100), 10);
            _trackBar1.Location = new Point(25, yPos);
            _trackBar1.ValueChanged += (s, e) =>
            {
                _valueLabel1.Text = $"{_trackBar1.Value / 100.0:F3}";
            };
            _mainPanel.Controls.Add(_trackBar1);

            yPos += 50;

            _valueLabel1 = CreateValueLabel($"{_settings.InitialStepSize:F3}");
            _valueLabel1.Location = new Point(25, yPos);
            _mainPanel.Controls.Add(_valueLabel1);

            yPos += 50;

            // === معامل المضاعف ===
            CreateSectionLabel("⚡ معامل المضاعف (Step Size Multiplier)", 25, yPos);
            yPos += 35;

            _trackBar2 = CreateStyledTrackBar(11, 20, (int)(_settings.StepSizeMultiplier * 10), 1);
            _trackBar2.Location = new Point(25, yPos);
            _trackBar2.ValueChanged += (s, e) =>
            {
                _valueLabel2.Text = $"{_trackBar2.Value / 10.0:F1}";
            };
            _mainPanel.Controls.Add(_trackBar2);

            yPos += 50;

            _valueLabel2 = CreateValueLabel($"{_settings.StepSizeMultiplier:F1}");
            _valueLabel2.Location = new Point(25, yPos);
            _mainPanel.Controls.Add(_valueLabel2);
        }

        // ============================================
        // دوال مساعدة لإنشاء عناصر تحكم مصممة
        // ============================================

        private ComboBox CreateStyledComboBox()
        {
            var combo = new ComboBox
            {
                Size = new Size(250, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(50, 50, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            return combo;
        }

        private TrackBar CreateStyledTrackBar(int min, int max, int value, int tickFreq)
        {
            var track = new TrackBar
            {
                Size = new Size(450, 50),
                Minimum = min,
                Maximum = max,
                Value = value,
                TickFrequency = tickFreq,
                BackColor = Color.FromArgb(30, 30, 35)
            };
            return track;
        }

        private Label CreateValueLabel(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Color.FromArgb(0, 255, 128),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize = true
            };
        }

        private string GetAlgorithmName()
        {
            switch (_algorithmType)
            {
                case AlgorithmType.NonlinearQuantization:
                    return "Nonlinear Quantization";
                case AlgorithmType.DPCM:
                    return "DPCM (Differential Pulse Code Modulation)";
                case AlgorithmType.PredictiveDifferentialCoding:
                    return "Predictive Differential Coding";
                case AlgorithmType.DeltaModulation:
                    return "Delta Modulation";
                case AlgorithmType.AdaptiveDeltaModulation:
                    return "Adaptive Delta Modulation";
                default:
                    return "";
            }
        }

        /// <summary>
        /// الحصول على الإعدادات بعد تعديلها
        /// </summary>
        public CompressionSettings GetSettings()
        {
            switch (_algorithmType)
            {
                case AlgorithmType.NonlinearQuantization:
                    if (_comboBox1?.SelectedItem != null)
                    {
                        string text = _comboBox1.SelectedItem.ToString().Replace(" مستوى", "");
                        _settings.QuantizationLevels = int.Parse(text);
                    }
                    break;

                case AlgorithmType.DPCM:
                    if (_comboBox1?.SelectedItem != null)
                    {
                        string bitsText = _comboBox1.SelectedItem.ToString().Replace("-bit", "");
                        _settings.BitsPerSample = int.Parse(bitsText);
                    }
                    break;

                case AlgorithmType.PredictiveDifferentialCoding:
                    if (_trackBar1 != null)
                        _settings.PredictionCoefficient = _trackBar1.Value / 100.0;
                    break;

                case AlgorithmType.DeltaModulation:
                    if (_trackBar1 != null)
                        _settings.StepSize = _trackBar1.Value / 100.0;
                    break;

                case AlgorithmType.AdaptiveDeltaModulation:
                    if (_trackBar1 != null)
                        _settings.InitialStepSize = _trackBar1.Value / 100.0;
                    if (_trackBar2 != null)
                        _settings.StepSizeMultiplier = _trackBar2.Value / 10.0;
                    break;
            }

            return _settings;
        }
    }
}