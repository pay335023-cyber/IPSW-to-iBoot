namespace IPSW_to_iBoot
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cardLog = new IPSW_to_iBoot.BrandCard();
            this.lblLogHead = new System.Windows.Forms.Label();
            this.sepLogTop = new System.Windows.Forms.Panel();
            this.rtLogView = new System.Windows.Forms.RichTextBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.panelFeature = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSub = new System.Windows.Forms.Label();
            this.lblFiles = new System.Windows.Forms.Label();
            this.txtFiles = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lvPairs = new System.Windows.Forms.ListView();
            this.colFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDevice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIos = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBuild = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRead = new IPSW_to_iBoot.BrandButton();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnClearList = new System.Windows.Forms.Button();
            this.cardLog.SuspendLayout();
            this.panelFeature.SuspendLayout();
            this.SuspendLayout();
            this.cardLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cardLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(11)))), ((int)(((byte)(13)))));
            this.cardLog.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(48)))));
            this.cardLog.Controls.Add(this.lblLogHead);
            this.cardLog.Controls.Add(this.sepLogTop);
            this.cardLog.Controls.Add(this.rtLogView);
            this.cardLog.FaceColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(11)))), ((int)(((byte)(13)))));
            this.cardLog.Location = new System.Drawing.Point(12, 12);
            this.cardLog.Name = "cardLog";
            this.cardLog.Radius = 1;
            this.cardLog.Size = new System.Drawing.Size(380, 556);
            this.cardLog.TabIndex = 0;
            this.lblLogHead.BackColor = System.Drawing.Color.Transparent;
            this.lblLogHead.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblLogHead.ForeColor = System.Drawing.Color.White;
            this.lblLogHead.Location = new System.Drawing.Point(12, 8);
            this.lblLogHead.Name = "lblLogHead";
            this.lblLogHead.Size = new System.Drawing.Size(356, 16);
            this.lblLogHead.TabIndex = 0;
            this.lblLogHead.Text = "LOG";
            this.sepLogTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sepLogTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(48)))));
            this.sepLogTop.Location = new System.Drawing.Point(1, 30);
            this.sepLogTop.Name = "sepLogTop";
            this.sepLogTop.Size = new System.Drawing.Size(378, 1);
            this.sepLogTop.TabIndex = 1;
            this.rtLogView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtLogView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(8)))), ((int)(((byte)(0)))));
            this.rtLogView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtLogView.Font = new System.Drawing.Font("Arial", 9F);
            this.rtLogView.ForeColor = System.Drawing.Color.Lime;
            this.rtLogView.Location = new System.Drawing.Point(8, 38);
            this.rtLogView.Name = "rtLogView";
            this.rtLogView.ReadOnly = true;
            this.rtLogView.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtLogView.Size = new System.Drawing.Size(364, 510);
            this.rtLogView.TabIndex = 2;
            this.rtLogView.Text = "";
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClearLog.Location = new System.Drawing.Point(12, 576);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(110, 26);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "Clear log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.panelFeature.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFeature.BackColor = System.Drawing.Color.White;
            this.panelFeature.Controls.Add(this.lblTitle);
            this.panelFeature.Controls.Add(this.lblSub);
            this.panelFeature.Controls.Add(this.lblFiles);
            this.panelFeature.Controls.Add(this.txtFiles);
            this.panelFeature.Controls.Add(this.btnBrowse);
            this.panelFeature.Controls.Add(this.lvPairs);
            this.panelFeature.Controls.Add(this.btnRead);
            this.panelFeature.Controls.Add(this.btnCopy);
            this.panelFeature.Controls.Add(this.btnClearList);
            this.panelFeature.Location = new System.Drawing.Point(404, 12);
            this.panelFeature.Name = "panelFeature";
            this.panelFeature.Size = new System.Drawing.Size(624, 590);
            this.panelFeature.TabIndex = 2;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.lblTitle.Location = new System.Drawing.Point(4, 2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(616, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "IPSW to iBoot";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(114)))), ((int)(((byte)(124)))));
            this.lblSub.Location = new System.Drawing.Point(6, 32);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(614, 18);
            this.lblSub.TabIndex = 1;
            this.lblSub.Text = "Pick firmware you have already downloaded and read the boot loader build it carries.";
            this.lblFiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.lblFiles.Location = new System.Drawing.Point(6, 62);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(90, 20);
            this.lblFiles.TabIndex = 2;
            this.lblFiles.Text = "Firmware";
            this.lblFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.txtFiles.Location = new System.Drawing.Point(96, 60);
            this.txtFiles.Name = "txtFiles";
            this.txtFiles.ReadOnly = true;
            this.txtFiles.Size = new System.Drawing.Size(408, 23);
            this.txtFiles.TabIndex = 3;
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(510, 59);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(110, 25);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.lvPairs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPairs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFile,
            this.colDevice,
            this.colIos,
            this.colBuild,
            this.colTag,
            this.colSource});
            this.lvPairs.FullRowSelect = true;
            this.lvPairs.GridLines = true;
            this.lvPairs.HideSelection = false;
            this.lvPairs.Location = new System.Drawing.Point(6, 94);
            this.lvPairs.Name = "lvPairs";
            this.lvPairs.Size = new System.Drawing.Size(614, 442);
            this.lvPairs.TabIndex = 5;
            this.lvPairs.UseCompatibleStateImageBehavior = false;
            this.lvPairs.View = System.Windows.Forms.View.Details;
            this.colFile.Text = "Firmware";
            this.colFile.Width = 150;
            this.colDevice.Text = "Device";
            this.colDevice.Width = 90;
            this.colIos.Text = "iOS";
            this.colIos.Width = 70;
            this.colBuild.Text = "Build";
            this.colBuild.Width = 70;
            this.colTag.Text = "Boot loader";
            this.colTag.Width = 160;
            this.colSource.Text = "Read from";
            this.colSource.Width = 70;
            this.btnRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(89)))), ((int)(((byte)(237)))));
            this.btnRead.FlatAppearance.BorderSize = 0;
            this.btnRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRead.ForeColor = System.Drawing.Color.White;
            this.btnRead.Location = new System.Drawing.Point(6, 548);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(190, 30);
            this.btnRead.TabIndex = 6;
            this.btnRead.Text = "Read firmware";
            this.btnRead.UseVisualStyleBackColor = false;
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCopy.Location = new System.Drawing.Point(206, 550);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(130, 26);
            this.btnCopy.TabIndex = 7;
            this.btnCopy.Text = "Copy results";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnClearList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearList.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClearList.Location = new System.Drawing.Point(346, 550);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(110, 26);
            this.btnClearList.TabIndex = 8;
            this.btnClearList.Text = "Clear list";
            this.btnClearList.UseVisualStyleBackColor = true;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1040, 614);
            this.Controls.Add(this.cardLog);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.panelFeature);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IPSW to iBoot";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.cardLog.ResumeLayout(false);
            this.panelFeature.ResumeLayout(false);
            this.panelFeature.PerformLayout();
            this.ResumeLayout(false);
        }

        private IPSW_to_iBoot.BrandCard cardLog;
        private System.Windows.Forms.Label lblLogHead;
        private System.Windows.Forms.Panel sepLogTop;
        private System.Windows.Forms.RichTextBox rtLogView;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Panel panelFeature;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.TextBox txtFiles;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ListView lvPairs;
        private System.Windows.Forms.ColumnHeader colFile;
        private System.Windows.Forms.ColumnHeader colDevice;
        private System.Windows.Forms.ColumnHeader colIos;
        private System.Windows.Forms.ColumnHeader colBuild;
        private System.Windows.Forms.ColumnHeader colTag;
        private System.Windows.Forms.ColumnHeader colSource;
        private IPSW_to_iBoot.BrandButton btnRead;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnClearList;
    }
}
