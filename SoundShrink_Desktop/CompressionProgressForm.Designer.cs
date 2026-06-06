namespace SoundShrink_Desktop
{
    partial class CompressionProgressForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                _chartTimer?.Stop();
                _chartTimer?.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // === Main Controls ===
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._lblProgress = new System.Windows.Forms.Label();
            this._chartPanel = new DoubleBufferedPanel();
            this._btnCancel = new System.Windows.Forms.Button();
            this.infoPanel = new System.Windows.Forms.TableLayoutPanel();

            // === Info Panels ===
            this._lblRatio = new System.Windows.Forms.Panel();
            this._lblSpeed = new System.Windows.Forms.Panel();
            this._lblElapsedTime = new System.Windows.Forms.Panel();
            this._lblRemaining = new System.Windows.Forms.Panel();

            this.SuspendLayout();

            // 
            // _progressBar
            // 
            this._progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._progressBar.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129); // ✅ أخضر النجاح
            this._progressBar.Location = new System.Drawing.Point(0, 0);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(700, 30);
            this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._progressBar.TabIndex = 0;

            // 
            // _lblProgress
            // 
            this._lblProgress.AutoSize = false;
            this._lblProgress.BackColor = System.Drawing.Color.Transparent;
            this._lblProgress.Dock = System.Windows.Forms.DockStyle.Top;
            this._lblProgress.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this._lblProgress.ForeColor = System.Drawing.Color.White;
            this._lblProgress.Location = new System.Drawing.Point(0, 30);
            this._lblProgress.Name = "_lblProgress";
            this._lblProgress.Size = new System.Drawing.Size(700, 40);
            this._lblProgress.TabIndex = 1;
            this._lblProgress.Text = "0%";
            this._lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // _chartPanel
            // 
            this._chartPanel.BackColor = System.Drawing.Color.FromArgb(17, 24, 39); // ✅ مثل Sidebar
            this._chartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chartPanel.Location = new System.Drawing.Point(0, 70);
            this._chartPanel.Margin = new System.Windows.Forms.Padding(10);
            this._chartPanel.Name = "_chartPanel";
            this._chartPanel.Padding = new System.Windows.Forms.Padding(10);
            this._chartPanel.Size = new System.Drawing.Size(700, 230);
            this._chartPanel.TabIndex = 2;
            this._chartPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ChartPanel_Paint);

            // 
            // infoPanel
            // 
            this.infoPanel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59); // ✅ مثل باقي Panels
            this.infoPanel.ColumnCount = 2;
            this.infoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.infoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.infoPanel.Location = new System.Drawing.Point(0, 300);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(10);
            this.infoPanel.RowCount = 3;
            this.infoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.infoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.infoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.infoPanel.Size = new System.Drawing.Size(700, 200);
            this.infoPanel.TabIndex = 3;

            // 
            // _lblRatio
            // 
            this._lblRatio.BackColor = System.Drawing.Color.Transparent;
            this._lblRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblRatio.Location = new System.Drawing.Point(13, 13);
            this._lblRatio.Name = "_lblRatio";
            this._lblRatio.Size = new System.Drawing.Size(337, 59);
            this._lblRatio.TabIndex = 0;

            // 
            // _lblSpeed
            // 
            this._lblSpeed.BackColor = System.Drawing.Color.Transparent;
            this._lblSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblSpeed.Location = new System.Drawing.Point(356, 13);
            this._lblSpeed.Name = "_lblSpeed";
            this._lblSpeed.Size = new System.Drawing.Size(337, 59);
            this._lblSpeed.TabIndex = 1;

            // 
            // _lblElapsedTime
            // 
            this._lblElapsedTime.BackColor = System.Drawing.Color.Transparent;
            this._lblElapsedTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblElapsedTime.Location = new System.Drawing.Point(13, 78);
            this._lblElapsedTime.Name = "_lblElapsedTime";
            this._lblElapsedTime.Size = new System.Drawing.Size(337, 59);
            this._lblElapsedTime.TabIndex = 2;

            // 
            // _lblRemaining
            // 
            this._lblRemaining.BackColor = System.Drawing.Color.Transparent;
            this._lblRemaining.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblRemaining.Location = new System.Drawing.Point(356, 78);
            this._lblRemaining.Name = "_lblRemaining";
            this._lblRemaining.Size = new System.Drawing.Size(337, 59);
            this._lblRemaining.TabIndex = 3;

            // 
            // _btnCancel
            // 
            this._btnCancel.BackColor = System.Drawing.Color.FromArgb(239, 68, 68); // ✅ مثل btnResetWorkspace
            this._btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._btnCancel.FlatAppearance.BorderSize = 0;
            this._btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(190, 50, 50);
            this._btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(255, 90, 90);
            this._btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this._btnCancel.ForeColor = System.Drawing.Color.White;
            this._btnCancel.Location = new System.Drawing.Point(13, 143);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(680, 54);
            this._btnCancel.TabIndex = 4;
            this._btnCancel.Text = "❌ إلغاء العملية";
            this._btnCancel.UseVisualStyleBackColor = false;
            this._btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            // 
            // CompressionProgressForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(10, 22, 40); // ✅ مثل panelMain
            this.ClientSize = new System.Drawing.Size(750, 600);
            this.Controls.Add(this._chartPanel);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this._lblProgress);
            this.Controls.Add(this._progressBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CompressionProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "مراقبة عملية الضغط";

            // Add info panels to table
            this.infoPanel.Controls.Add(this._lblRatio, 0, 0);
            this.infoPanel.Controls.Add(this._lblSpeed, 1, 0);
            this.infoPanel.Controls.Add(this._lblElapsedTime, 0, 1);
            this.infoPanel.Controls.Add(this._lblRemaining, 1, 1);
            this.infoPanel.Controls.Add(this._btnCancel, 0, 2);
            this.infoPanel.SetColumnSpan(this._btnCancel, 2);

            this.ResumeLayout(false);
        }

        #endregion

        // === Fields ===
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Label _lblProgress;
        private System.Windows.Forms.Panel _lblRatio;
        private System.Windows.Forms.Panel _lblSpeed;
        private System.Windows.Forms.Panel _lblElapsedTime;
        private System.Windows.Forms.Panel _lblRemaining;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Panel _chartPanel;
        private System.Windows.Forms.TableLayoutPanel infoPanel;
    }
}