namespace NCM
{
    partial class editoruform
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            tbnoloader = new TextBox();
            tbmac = new TextBox();
            tbdelay = new TextBox();
            tbleave = new TextBox();
            tbscan = new TextBox();
            tbsignal = new TextBox();
            btnsave = new Button();
            btnclose = new Button();
            tbessid = new TextBox();
            label9 = new Label();
            cbchannel = new ComboBox();
            cbbridging = new ComboBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 15);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(66, 25);
            label1.TabIndex = 0;
            label1.Text = "Loader";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 63);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(115, 25);
            label2.TabIndex = 1;
            label2.Text = "Mac Address";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 112);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(75, 25);
            label3.TabIndex = 2;
            label3.Text = "Channel";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 208);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(131, 25);
            label4.TabIndex = 3;
            label4.Text = "Bridging Mode";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(17, 257);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(56, 25);
            label5.TabIndex = 4;
            label5.Text = "Delay";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(17, 305);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(139, 25);
            label6.TabIndex = 5;
            label6.Text = "Leave Threshold";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(17, 353);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(132, 25);
            label7.TabIndex = 6;
            label7.Text = "Scan Threshold";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(17, 402);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(95, 25);
            label8.TabIndex = 7;
            label8.Text = "Min Signal";
            // 
            // tbnoloader
            // 
            tbnoloader.Location = new Point(206, 10);
            tbnoloader.Margin = new Padding(4, 5, 4, 5);
            tbnoloader.Name = "tbnoloader";
            tbnoloader.ReadOnly = true;
            tbnoloader.Size = new Size(208, 31);
            tbnoloader.TabIndex = 10;
            // 
            // tbmac
            // 
            tbmac.Location = new Point(206, 58);
            tbmac.Margin = new Padding(4, 5, 4, 5);
            tbmac.Name = "tbmac";
            tbmac.ReadOnly = true;
            tbmac.Size = new Size(208, 31);
            tbmac.TabIndex = 11;
            // 
            // tbdelay
            // 
            tbdelay.Location = new Point(206, 252);
            tbdelay.Margin = new Padding(4, 5, 4, 5);
            tbdelay.Name = "tbdelay";
            tbdelay.Size = new Size(208, 31);
            tbdelay.TabIndex = 3;
            // 
            // tbleave
            // 
            tbleave.Location = new Point(206, 300);
            tbleave.Margin = new Padding(4, 5, 4, 5);
            tbleave.Name = "tbleave";
            tbleave.Size = new Size(208, 31);
            tbleave.TabIndex = 4;
            // 
            // tbscan
            // 
            tbscan.Location = new Point(206, 348);
            tbscan.Margin = new Padding(4, 5, 4, 5);
            tbscan.Name = "tbscan";
            tbscan.Size = new Size(208, 31);
            tbscan.TabIndex = 5;
            // 
            // tbsignal
            // 
            tbsignal.Location = new Point(206, 397);
            tbsignal.Margin = new Padding(4, 5, 4, 5);
            tbsignal.Name = "tbsignal";
            tbsignal.Size = new Size(208, 31);
            tbsignal.TabIndex = 6;
            // 
            // btnsave
            // 
            btnsave.Location = new Point(17, 467);
            btnsave.Margin = new Padding(4, 5, 4, 5);
            btnsave.Name = "btnsave";
            btnsave.Size = new Size(210, 38);
            btnsave.TabIndex = 7;
            btnsave.Text = "Save and Reboot";
            btnsave.UseVisualStyleBackColor = true;
            btnsave.Click += btnsave_Click;
            // 
            // btnclose
            // 
            btnclose.Location = new Point(236, 467);
            btnclose.Margin = new Padding(4, 5, 4, 5);
            btnclose.Name = "btnclose";
            btnclose.Size = new Size(180, 38);
            btnclose.TabIndex = 8;
            btnclose.Text = "Close";
            btnclose.UseVisualStyleBackColor = true;
            btnclose.Click += btnclose_Click;
            // 
            // tbessid
            // 
            tbessid.Location = new Point(206, 155);
            tbessid.Margin = new Padding(4, 5, 4, 5);
            tbessid.Name = "tbessid";
            tbessid.Size = new Size(208, 31);
            tbessid.TabIndex = 1;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(17, 160);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(59, 25);
            label9.TabIndex = 11;
            label9.Text = "ESSID";
            // 
            // cbchannel
            // 
            cbchannel.FormattingEnabled = true;
            cbchannel.Location = new Point(206, 107);
            cbchannel.Margin = new Padding(4, 5, 4, 5);
            cbchannel.Name = "cbchannel";
            cbchannel.Size = new Size(208, 33);
            cbchannel.TabIndex = 0;
            // 
            // cbbridging
            // 
            cbbridging.FormattingEnabled = true;
            cbbridging.Location = new Point(206, 203);
            cbbridging.Margin = new Padding(4, 5, 4, 5);
            cbbridging.Name = "cbbridging";
            cbbridging.Size = new Size(208, 33);
            cbbridging.TabIndex = 2;
            // 
            // editoruform
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 535);
            Controls.Add(cbbridging);
            Controls.Add(cbchannel);
            Controls.Add(label9);
            Controls.Add(tbessid);
            Controls.Add(btnclose);
            Controls.Add(btnsave);
            Controls.Add(tbsignal);
            Controls.Add(tbscan);
            Controls.Add(tbleave);
            Controls.Add(tbdelay);
            Controls.Add(tbmac);
            Controls.Add(tbnoloader);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(4, 5, 4, 5);
            MaximumSize = new Size(456, 591);
            MinimumSize = new Size(456, 591);
            Name = "editoruform";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Config ORU";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox tbnoloader;
        private TextBox tbmac;
        private TextBox tbdelay;
        private TextBox tbleave;
        private TextBox tbscan;
        private TextBox tbsignal;
        private Button btnsave;
        private Button btnclose;
        private TextBox tbessid;
        private Label label9;
        private ComboBox cbchannel;
        private ComboBox cbbridging;
    }
}