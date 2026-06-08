namespace SoundShrink_Desktop
{
    partial class DecompressionSettingsForm
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

            // === Panels ===
            this.topBar = new System.Windows.Forms.Panel();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.settingsBox = new System.Windows.Forms.Panel();

            // === Labels ===
            this.titleLabel = new System.Windows.Forms.Label();
            this.algoLabel = new System.Windows.Forms.Label();
            this.settingsInfo = new System.Windows.Forms.Label();
            this.infoOriginal = new System.Windows.Forms.Label();
            this.infoDefault = new System.Windows.Forms.Label();
            this.optionsTitle = new System.Windows.Forms.Label();

            // === RadioButtons ===
            this.radioOriginal = new System.Windows.Forms.RadioButton();
            this.radioDefault = new System.Windows.Forms.RadioButton();

            // === Buttons ===
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(550, 4);
            this.topBar.TabIndex = 0;

            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(500, 35);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Decompression Options";

            // 
            // algoLabel
            // 
            this.algoLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.algoLabel.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.algoLabel.Location = new System.Drawing.Point(20, 65);
            this.algoLabel.Name = "algoLabel";
            this.algoLabel.Size = new System.Drawing.Size(500, 50);
            this.algoLabel.TabIndex = 2;
            this.algoLabel.Text = "Algorithm: ";

            // 
            // settingsBox
            // 
            this.settingsBox.BackColor = System.Drawing.Color.FromArgb(30, 42, 80);
            this.settingsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.settingsBox.Location = new System.Drawing.Point(20, 100);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new System.Drawing.Size(510, 130);
            this.settingsBox.TabIndex = 9;
            this.settingsBox.Controls.Add(this.settingsInfo);

            // 
            // settingsInfo
            // 
            this.settingsInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.settingsInfo.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.settingsInfo.Location = new System.Drawing.Point(15, 12);
            this.settingsInfo.Name = "settingsInfo";
            this.settingsInfo.Size = new System.Drawing.Size(480, 100);
            this.settingsInfo.TabIndex = 3;
            this.settingsInfo.Text = "Original Settings:\n";

            // 
            // optionsTitle
            // 
            this.optionsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.optionsTitle.ForeColor = System.Drawing.Color.White;
            this.optionsTitle.Location = new System.Drawing.Point(20, 235);
            this.optionsTitle.Name = "optionsTitle";
            this.optionsTitle.Size = new System.Drawing.Size(500, 22);
            this.optionsTitle.TabIndex = 10;
            this.optionsTitle.Text = "Choose Settings Mode";

            // 
            // radioOriginal
            // 
            this.radioOriginal.AutoSize = true;
            this.radioOriginal.Checked = true;
            this.radioOriginal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.radioOriginal.ForeColor = System.Drawing.Color.White;
            this.radioOriginal.Location = new System.Drawing.Point(25, 265);
            this.radioOriginal.Name = "radioOriginal";
            this.radioOriginal.Size = new System.Drawing.Size(300, 24);
            this.radioOriginal.TabIndex = 4;
            this.radioOriginal.TabStop = true;
            this.radioOriginal.Text = "Use Original Settings";
            this.radioOriginal.UseVisualStyleBackColor = true;
            this.radioOriginal.CheckedChanged += new System.EventHandler(this.radioOriginal_CheckedChanged);

            // 
            // infoOriginal
            // 
            this.infoOriginal.AutoSize = true;
            this.infoOriginal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.infoOriginal.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.infoOriginal.Location = new System.Drawing.Point(45, 292);
            this.infoOriginal.Name = "infoOriginal";
            this.infoOriginal.Size = new System.Drawing.Size(400, 15);
            this.infoOriginal.TabIndex = 5;
            this.infoOriginal.Text = "Will use the exact same settings that were used during compression";

            // 
            // radioDefault
            // 
            this.radioDefault.AutoSize = true;
            this.radioDefault.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.radioDefault.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.radioDefault.Location = new System.Drawing.Point(25, 320);
            this.radioDefault.Name = "radioDefault";
            this.radioDefault.Size = new System.Drawing.Size(300, 24);
            this.radioDefault.TabIndex = 6;
            this.radioDefault.Text = "Use Default Algorithm Settings";
            this.radioDefault.UseVisualStyleBackColor = true;
            this.radioDefault.CheckedChanged += new System.EventHandler(this.radioDefault_CheckedChanged);

            // 
            // infoDefault
            // 
            this.infoDefault.AutoSize = true;
            this.infoDefault.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.infoDefault.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.infoDefault.Location = new System.Drawing.Point(45, 347);
            this.infoDefault.Name = "infoDefault";
            this.infoDefault.Size = new System.Drawing.Size(350, 15);
            this.infoDefault.TabIndex = 7;
            this.infoDefault.Text = "Will use default settings (may produce different results)";

            // 
            // buttonsPanel
            // 
            this.buttonsPanel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonsPanel.Location = new System.Drawing.Point(0, 375);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(550, 55);
            this.buttonsPanel.TabIndex = 8;

            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(96, 165, 250);
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(280, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 40);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Continue";
            this.btnOK.UseVisualStyleBackColor = false;

            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 113, 113);
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(410, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;

            // 
            // DecompressionSettingsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(550, 450);
            this.Controls.Add(this.optionsTitle);
            this.Controls.Add(this.settingsBox);
            this.Controls.Add(this.buttonsPanel);
            this.Controls.Add(this.infoDefault);
            this.Controls.Add(this.radioDefault);
            this.Controls.Add(this.infoOriginal);
            this.Controls.Add(this.radioOriginal);
            this.Controls.Add(this.algoLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.topBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DecompressionSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Decompression Options";

            this.buttonsPanel.Controls.Add(this.btnOK);
            this.buttonsPanel.Controls.Add(this.btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // === Fields ===
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Panel settingsBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label algoLabel;
        private System.Windows.Forms.Label settingsInfo;
        private System.Windows.Forms.Label optionsTitle;
        private System.Windows.Forms.Label infoOriginal;
        private System.Windows.Forms.Label infoDefault;
        private System.Windows.Forms.RadioButton radioOriginal;
        private System.Windows.Forms.RadioButton radioDefault;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}