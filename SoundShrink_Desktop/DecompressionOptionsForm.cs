using System;
using System.Drawing;
using System.Windows.Forms;

namespace SoundShrink_Desktop
{
    public partial class DecompressionOptionsForm : Form
    {
        private bool _useOriginalSettings = true;
        private Label lblAlgorithmName;

        public bool UseOriginalSettings => _useOriginalSettings;

        public DecompressionOptionsForm(string algorithmName)
        {
            InitializeComponent();
            BuildUI(algorithmName);
        }

        private void BuildUI(string algorithmName)
        {
            this.SuspendLayout();

            // إعدادات النافذة
            this.Text = $"⚙️ خيارات فك الضغط - {algorithmName}";
            this.Size = new Size(520, 320); // تصغير الحجم
            this.BackColor = Color.FromArgb(30, 30, 35);
            this.Font = new Font("Segoe UI", 9F);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // === شريط علوي ملوّن ===
            var topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 4,
                BackColor = Color.FromArgb(0, 191, 255)
            };
            this.Controls.Add(topBar);

            // === العنوان الرئيسي ===
            var titleLabel = new Label
            {
                Text = " خيارات فك الضغط",
                ForeColor = Color.FromArgb(0, 191, 255),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = false,
                Size = new Size(480, 35),
                Location = new Point(20, 15),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(titleLabel);

            // === خط فاصل ===
            var separator0 = new Panel
            {
                Size = new Size(470, 1),
                Location = new Point(20, 55),
                BackColor = Color.FromArgb(60, 60, 70)
            };
            this.Controls.Add(separator0);

            // === عنوان قسم الخوارزمية ===
            var algoLabel = new Label
            {
                Text = "🔹 الخوارزمية المستخدمة:",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(20, 65),
                AutoSize = true
            };
            this.Controls.Add(algoLabel);

            // === اسم الخوارزمية ===
            lblAlgorithmName = new Label
            {
                Text = algorithmName,
                ForeColor = Color.FromArgb(0, 255, 128),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(40, 92),
                AutoSize = true
            };
            this.Controls.Add(lblAlgorithmName);

            // === خط فاصل ===
            var separator1 = new Panel
            {
                Size = new Size(470, 1),
                Location = new Point(20, 125),
                BackColor = Color.FromArgb(60, 60, 70)
            };
            this.Controls.Add(separator1);

            // === عنوان قسم الخيارات ===
            var optionsTitle = new Label
            {
                Text = "🎛️ اختر طريقة فك الضغط:",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(20, 135),
                AutoSize = true
            };
            this.Controls.Add(optionsTitle);

            // === الخيار 1: الإعدادات الأصلية ===
            var panelOriginal = new Panel
            {
                Size = new Size(470, 50),
                Location = new Point(20, 160),
                BackColor = Color.FromArgb(40, 40, 45)
            };
            panelOriginal.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(0, 191, 255), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, panelOriginal.Width - 1, panelOriginal.Height - 1);
            };

            var radioOriginal = new RadioButton
            {
                Text = "📋 فك الضغط بالإعدادات الأصلية (الموصى به)",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, 5),
                AutoSize = true,
                Checked = true,
                BackColor = Color.Transparent
            };
            radioOriginal.CheckedChanged += (s, e) => {
                if (radioOriginal.Checked)
                    _useOriginalSettings = true;
            };
            panelOriginal.Controls.Add(radioOriginal);

            var infoOriginal = new Label
            {
                Text = "💡 سيتم استخدام نفس الإعدادات التي استُخدمت عند ضغط الملف",
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                Location = new Point(30, 28),
                AutoSize = true
            };
            panelOriginal.Controls.Add(infoOriginal);

            this.Controls.Add(panelOriginal);

            // === الخيار 2: الإعدادات الافتراضية ===
            var panelDefault = new Panel
            {
                Size = new Size(470, 50),
                Location = new Point(20, 220),
                BackColor = Color.FromArgb(40, 40, 45)
            };
            panelDefault.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(255, 193, 7), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, panelDefault.Width - 1, panelDefault.Height - 1);
            };

            var radioDefault = new RadioButton
            {
                Text = "🔧 فك الضغط بالإعدادات الافتراضية للخوارزمية",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, 5),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            radioDefault.CheckedChanged += (s, e) => {
                if (radioDefault.Checked)
                    _useOriginalSettings = false;
            };
            panelDefault.Controls.Add(radioDefault);

            var infoDefault = new Label
            {
                Text = "⚠️ سيتم استخدام الإعدادات الافتراضية (قد تختلف عن الأصلية)",
                ForeColor = Color.FromArgb(255, 193, 7),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                Location = new Point(30, 28),
                AutoSize = true
            };
            panelDefault.Controls.Add(infoDefault);

            this.Controls.Add(panelDefault);

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
            this.Controls.Add(buttonsPanel);

            // === زر متابعة ===
            var btnOK = CreateStyledButton("✓  متابعة", Color.FromArgb(0, 191, 255), DialogResult.OK);
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
    }
}