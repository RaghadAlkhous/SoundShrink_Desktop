namespace SoundShrink_Desktop
{
    partial class ChartViewerForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();

            // === Main Controls ===
            this._lblTitle = new System.Windows.Forms.Label();
            this._infoPanel = new System.Windows.Forms.Panel();
            this._chartPanel = new DoubleBufferedPanel();
            this._legendPanel = new System.Windows.Forms.Panel();
            this._btnClose = new System.Windows.Forms.Button();

            // === Info Panel Labels ===
            this._lblTime = new System.Windows.Forms.Label();
            this._lblRatio = new System.Windows.Forms.Label();
            this._lblPoints = new System.Windows.Forms.Label();

            // === Legend Panel Controls ===
            this._ratioColorBox = new System.Windows.Forms.Panel();
            this._lblRatioLegend = new System.Windows.Forms.Label();
            this._speedColorBox = new System.Windows.Forms.Panel();
            this._lblSpeedLegend = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // 
            // _lblTitle
            // 
            this._lblTitle.AutoSize = false;
            this._lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this._lblTitle.ForeColor = System.Drawing.Color.White;
            this._lblTitle.Location = new System.Drawing.Point(20, 15);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(860, 35);
            this._lblTitle.TabIndex = 0;
            this._lblTitle.Text = "📊 Compression Analysis";
            this._lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // _infoPanel
            // 
            this._infoPanel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._infoPanel.Location = new System.Drawing.Point(20, 55);
            this._infoPanel.Name = "_infoPanel";
            this._infoPanel.Size = new System.Drawing.Size(860, 50);
            this._infoPanel.TabIndex = 1;
            this._infoPanel.Controls.Add(this._lblTime);
            this._infoPanel.Controls.Add(this._lblRatio);
            this._infoPanel.Controls.Add(this._lblPoints);

            // 
            // _lblTime
            // 
            this._lblTime.AutoSize = true;
            this._lblTime.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this._lblTime.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this._lblTime.Location = new System.Drawing.Point(20, 15);
            this._lblTime.Name = "_lblTime";
            this._lblTime.Size = new System.Drawing.Size(150, 20);
            this._lblTime.TabIndex = 0;
            this._lblTime.Text = "⏱️ Total Time: 0.00s";

            // 
            // _lblRatio
            // 
            this._lblRatio.AutoSize = true;
            this._lblRatio.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this._lblRatio.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this._lblRatio.Location = new System.Drawing.Point(250, 15);
            this._lblRatio.Name = "_lblRatio";
            this._lblRatio.Size = new System.Drawing.Size(150, 20);
            this._lblRatio.TabIndex = 1;
            this._lblRatio.Text = "📈 Final Ratio: 0.00x";

            // 
            // _lblPoints
            // 
            this._lblPoints.AutoSize = true;
            this._lblPoints.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._lblPoints.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this._lblPoints.Location = new System.Drawing.Point(500, 17);
            this._lblPoints.Name = "_lblPoints";
            this._lblPoints.Size = new System.Drawing.Size(150, 19);
            this._lblPoints.TabIndex = 2;
            this._lblPoints.Text = "📊 Data Points: 0";

            // 
            // _chartPanel
            // 
            this._chartPanel.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this._chartPanel.Location = new System.Drawing.Point(20, 115);
            this._chartPanel.Name = "_chartPanel";
            this._chartPanel.Size = new System.Drawing.Size(860, 430);
            this._chartPanel.TabIndex = 2;
            this._chartPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ChartPanel_Paint);

            // 
            // _legendPanel
            // 
            this._legendPanel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._legendPanel.Location = new System.Drawing.Point(20, 555);
            this._legendPanel.Name = "_legendPanel";
            this._legendPanel.Size = new System.Drawing.Size(860, 30);
            this._legendPanel.TabIndex = 3;
            this._legendPanel.Controls.Add(this._ratioColorBox);
            this._legendPanel.Controls.Add(this._lblRatioLegend);
            this._legendPanel.Controls.Add(this._speedColorBox);
            this._legendPanel.Controls.Add(this._lblSpeedLegend);

            // 
            // _ratioColorBox
            // 
            this._ratioColorBox.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this._ratioColorBox.Location = new System.Drawing.Point(20, 7);
            this._ratioColorBox.Name = "_ratioColorBox";
            this._ratioColorBox.Size = new System.Drawing.Size(20, 15);
            this._ratioColorBox.TabIndex = 0;

            // 
            // _lblRatioLegend
            // 
            this._lblRatioLegend.AutoSize = true;
            this._lblRatioLegend.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblRatioLegend.ForeColor = System.Drawing.Color.White;
            this._lblRatioLegend.Location = new System.Drawing.Point(45, 7);
            this._lblRatioLegend.Name = "_lblRatioLegend";
            this._lblRatioLegend.Size = new System.Drawing.Size(120, 15);
            this._lblRatioLegend.TabIndex = 1;
            this._lblRatioLegend.Text = "Compression Ratio";

            // 
            // _speedColorBox
            // 
            this._speedColorBox.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this._speedColorBox.Location = new System.Drawing.Point(220, 7);
            this._speedColorBox.Name = "_speedColorBox";
            this._speedColorBox.Size = new System.Drawing.Size(20, 15);
            this._speedColorBox.TabIndex = 2;

            // 
            // _lblSpeedLegend
            // 
            this._lblSpeedLegend.AutoSize = true;
            this._lblSpeedLegend.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblSpeedLegend.ForeColor = System.Drawing.Color.White;
            this._lblSpeedLegend.Location = new System.Drawing.Point(245, 7);
            this._lblSpeedLegend.Name = "_lblSpeedLegend";
            this._lblSpeedLegend.Size = new System.Drawing.Size(150, 15);
            this._lblSpeedLegend.TabIndex = 3;
            this._lblSpeedLegend.Text = "Processing Speed (MB/s)";

            // 
            // _btnClose
            // 
            this._btnClose.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this._btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnClose.FlatAppearance.BorderSize = 0;
            this._btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(190, 50, 50);
            this._btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(255, 90, 90);
            this._btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._btnClose.ForeColor = System.Drawing.Color.White;
            this._btnClose.Location = new System.Drawing.Point(760, 595);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(120, 35);
            this._btnClose.TabIndex = 4;
            this._btnClose.Text = "✗  Close";
            this._btnClose.UseVisualStyleBackColor = false;
            this._btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // 
            // ChartViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(10, 22, 40);
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._legendPanel);
            this.Controls.Add(this._chartPanel);
            this.Controls.Add(this._infoPanel);
            this.Controls.Add(this._lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChartViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "📊 تحليل عملية الضغط";

            this.ResumeLayout(false);
        }

        #endregion

        // === Fields ===
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Panel _infoPanel;
        private DoubleBufferedPanel _chartPanel;
        private System.Windows.Forms.Panel _legendPanel;
        private System.Windows.Forms.Button _btnClose;

        // === Info Panel Labels ===
        private System.Windows.Forms.Label _lblTime;
        private System.Windows.Forms.Label _lblRatio;
        private System.Windows.Forms.Label _lblPoints;

        // === Legend Panel Controls ===
        private System.Windows.Forms.Panel _ratioColorBox;
        private System.Windows.Forms.Label _lblRatioLegend;
        private System.Windows.Forms.Panel _speedColorBox;
        private System.Windows.Forms.Label _lblSpeedLegend;
    }
}