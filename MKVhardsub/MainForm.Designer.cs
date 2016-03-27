namespace MKVhardsub
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInputMkv = new System.Windows.Forms.TextBox();
            this.txtSubtitleFile = new System.Windows.Forms.TextBox();
            this.brwInputMkv = new System.Windows.Forms.LinkLabel();
            this.brwSubtitle = new System.Windows.Forms.LinkLabel();
            this.cmdStart = new System.Windows.Forms.Button();
            this.chkEmbedd = new System.Windows.Forms.CheckBox();
            this.cmdStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.prgStatus = new System.Windows.Forms.ProgressBar();
            this.ffmpeg = new System.Diagnostics.Process();
            this.bwFonts = new System.ComponentModel.BackgroundWorker();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input mkv file:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Subtitle file:";
            // 
            // txtInputMkv
            // 
            this.txtInputMkv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputMkv.Location = new System.Drawing.Point(6, 32);
            this.txtInputMkv.Name = "txtInputMkv";
            this.txtInputMkv.ReadOnly = true;
            this.txtInputMkv.Size = new System.Drawing.Size(271, 20);
            this.txtInputMkv.TabIndex = 3;
            // 
            // txtSubtitleFile
            // 
            this.txtSubtitleFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubtitleFile.Location = new System.Drawing.Point(89, 74);
            this.txtSubtitleFile.Name = "txtSubtitleFile";
            this.txtSubtitleFile.ReadOnly = true;
            this.txtSubtitleFile.Size = new System.Drawing.Size(188, 20);
            this.txtSubtitleFile.TabIndex = 4;
            // 
            // brwInputMkv
            // 
            this.brwInputMkv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.brwInputMkv.AutoSize = true;
            this.brwInputMkv.Location = new System.Drawing.Point(283, 35);
            this.brwInputMkv.Name = "brwInputMkv";
            this.brwInputMkv.Size = new System.Drawing.Size(42, 13);
            this.brwInputMkv.TabIndex = 6;
            this.brwInputMkv.TabStop = true;
            this.brwInputMkv.Text = "Browse";
            this.brwInputMkv.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.brwInputMkv_LinkClicked);
            // 
            // brwSubtitle
            // 
            this.brwSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.brwSubtitle.AutoSize = true;
            this.brwSubtitle.Location = new System.Drawing.Point(283, 77);
            this.brwSubtitle.Name = "brwSubtitle";
            this.brwSubtitle.Size = new System.Drawing.Size(42, 13);
            this.brwSubtitle.TabIndex = 7;
            this.brwSubtitle.TabStop = true;
            this.brwSubtitle.Text = "Browse";
            this.brwSubtitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.brwSubtitle_LinkClicked);
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(6, 19);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(152, 31);
            this.cmdStart.TabIndex = 9;
            this.cmdStart.Text = "Start Encode";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // chkEmbedd
            // 
            this.chkEmbedd.AutoSize = true;
            this.chkEmbedd.Checked = true;
            this.chkEmbedd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEmbedd.Location = new System.Drawing.Point(6, 76);
            this.chkEmbedd.Name = "chkEmbedd";
            this.chkEmbedd.Size = new System.Drawing.Size(77, 17);
            this.chkEmbedd.TabIndex = 10;
            this.chkEmbedd.Text = "Embedded";
            this.chkEmbedd.UseVisualStyleBackColor = true;
            // 
            // cmdStop
            // 
            this.cmdStop.Enabled = false;
            this.cmdStop.Location = new System.Drawing.Point(164, 19);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(161, 31);
            this.cmdStop.TabIndex = 11;
            this.cmdStop.Text = "Stop Encode";
            this.cmdStop.UseVisualStyleBackColor = true;
            this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkEmbedd);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtInputMkv);
            this.groupBox1.Controls.Add(this.txtSubtitleFile);
            this.groupBox1.Controls.Add(this.brwSubtitle);
            this.groupBox1.Controls.Add(this.brwInputMkv);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(331, 99);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input MKV";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblStatus);
            this.groupBox2.Controls.Add(this.prgStatus);
            this.groupBox2.Controls.Add(this.cmdStart);
            this.groupBox2.Controls.Add(this.cmdStop);
            this.groupBox2.Location = new System.Drawing.Point(12, 117);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 104);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Process";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(6, 78);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 18);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "Ready.";
            // 
            // prgStatus
            // 
            this.prgStatus.Location = new System.Drawing.Point(6, 58);
            this.prgStatus.Name = "prgStatus";
            this.prgStatus.Size = new System.Drawing.Size(319, 15);
            this.prgStatus.TabIndex = 12;
            // 
            // ffmpeg
            // 
            this.ffmpeg.EnableRaisingEvents = true;
            this.ffmpeg.StartInfo.Domain = "";
            this.ffmpeg.StartInfo.LoadUserProfile = false;
            this.ffmpeg.StartInfo.Password = null;
            this.ffmpeg.StartInfo.StandardErrorEncoding = null;
            this.ffmpeg.StartInfo.StandardOutputEncoding = null;
            this.ffmpeg.StartInfo.UserName = "";
            this.ffmpeg.SynchronizingObject = this;
            this.ffmpeg.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(this.FFMPEG_OuputReceived);
            this.ffmpeg.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(this.FFMPEG_OuputReceived);
            this.ffmpeg.Exited += new System.EventHandler(this.FFMPEG_Exited);
            // 
            // bwFonts
            // 
            this.bwFonts.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwFonts_DoWork);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 231);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "MKVhardsub";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtInputMkv;
        private System.Windows.Forms.TextBox txtSubtitleFile;
        private System.Windows.Forms.LinkLabel brwInputMkv;
        private System.Windows.Forms.LinkLabel brwSubtitle;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.CheckBox chkEmbedd;
        private System.Windows.Forms.Button cmdStop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar prgStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Diagnostics.Process ffmpeg;
        private System.ComponentModel.BackgroundWorker bwFonts;
        private System.Windows.Forms.OpenFileDialog ofd;
    }
}

