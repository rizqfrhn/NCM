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
            tbchannel = new TextBox();
            tbbridging = new TextBox();
            tbdelay = new TextBox();
            tbleave = new TextBox();
            tbscan = new TextBox();
            tbsignal = new TextBox();
            btnsave = new Button();
            btnclose = new Button();
            tbessid = new TextBox();
            label9 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 0;
            label1.Text = "Loader";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 38);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 1;
            label2.Text = "Mac Address";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 67);
            label3.Name = "label3";
            label3.Size = new Size(51, 15);
            label3.TabIndex = 2;
            label3.Text = "Channel";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 125);
            label4.Name = "label4";
            label4.Size = new Size(86, 15);
            label4.TabIndex = 3;
            label4.Text = "Bridging Mode";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 154);
            label5.Name = "label5";
            label5.Size = new Size(36, 15);
            label5.TabIndex = 4;
            label5.Text = "Delay";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 183);
            label6.Name = "label6";
            label6.Size = new Size(92, 15);
            label6.TabIndex = 5;
            label6.Text = "Leave Threshold";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 212);
            label7.Name = "label7";
            label7.Size = new Size(87, 15);
            label7.TabIndex = 6;
            label7.Text = "Scan Threshold";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 241);
            label8.Name = "label8";
            label8.Size = new Size(63, 15);
            label8.TabIndex = 7;
            label8.Text = "Min Signal";
            // 
            // tbnoloader
            // 
            tbnoloader.Location = new Point(144, 6);
            tbnoloader.Name = "tbnoloader";
            tbnoloader.ReadOnly = true;
            tbnoloader.Size = new Size(147, 23);
            tbnoloader.TabIndex = 0;
            // 
            // tbmac
            // 
            tbmac.Location = new Point(144, 35);
            tbmac.Name = "tbmac";
            tbmac.ReadOnly = true;
            tbmac.Size = new Size(147, 23);
            tbmac.TabIndex = 1;
            // 
            // tbchannel
            // 
            tbchannel.Location = new Point(144, 64);
            tbchannel.Name = "tbchannel";
            tbchannel.Size = new Size(147, 23);
            tbchannel.TabIndex = 2;
            // 
            // tbbridging
            // 
            tbbridging.Location = new Point(144, 122);
            tbbridging.Name = "tbbridging";
            tbbridging.Size = new Size(147, 23);
            tbbridging.TabIndex = 4;
            // 
            // tbdelay
            // 
            tbdelay.Location = new Point(144, 151);
            tbdelay.Name = "tbdelay";
            tbdelay.Size = new Size(147, 23);
            tbdelay.TabIndex = 5;
            // 
            // tbleave
            // 
            tbleave.Location = new Point(144, 180);
            tbleave.Name = "tbleave";
            tbleave.Size = new Size(147, 23);
            tbleave.TabIndex = 6;
            // 
            // tbscan
            // 
            tbscan.Location = new Point(144, 209);
            tbscan.Name = "tbscan";
            tbscan.Size = new Size(147, 23);
            tbscan.TabIndex = 7;
            // 
            // tbsignal
            // 
            tbsignal.Location = new Point(144, 238);
            tbsignal.Name = "tbsignal";
            tbsignal.Size = new Size(147, 23);
            tbsignal.TabIndex = 8;
            // 
            // btnsave
            // 
            btnsave.Location = new Point(12, 280);
            btnsave.Name = "btnsave";
            btnsave.Size = new Size(147, 23);
            btnsave.TabIndex = 9;
            btnsave.Text = "Save and Reboot";
            btnsave.UseVisualStyleBackColor = true;
            btnsave.Click += btnsave_Click;
            // 
            // btnclose
            // 
            btnclose.Location = new Point(165, 280);
            btnclose.Name = "btnclose";
            btnclose.Size = new Size(126, 23);
            btnclose.TabIndex = 10;
            btnclose.Text = "Close";
            btnclose.UseVisualStyleBackColor = true;
            btnclose.Click += btnclose_Click;
            // 
            // tbessid
            // 
            tbessid.Location = new Point(144, 93);
            tbessid.Name = "tbessid";
            tbessid.Size = new Size(147, 23);
            tbessid.TabIndex = 3;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 96);
            label9.Name = "label9";
            label9.Size = new Size(36, 15);
            label9.TabIndex = 11;
            label9.Text = "ESSID";
            // 
            // editoruform
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(304, 321);
            Controls.Add(label9);
            Controls.Add(tbessid);
            Controls.Add(btnclose);
            Controls.Add(btnsave);
            Controls.Add(tbsignal);
            Controls.Add(tbscan);
            Controls.Add(tbleave);
            Controls.Add(tbdelay);
            Controls.Add(tbbridging);
            Controls.Add(tbchannel);
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
            Name = "editoruform";
            Text = "Edit ORU";
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
        private TextBox tbchannel;
        private TextBox tbbridging;
        private TextBox tbdelay;
        private TextBox tbleave;
        private TextBox tbscan;
        private TextBox tbsignal;
        private Button btnsave;
        private Button btnclose;
        private TextBox tbessid;
        private Label label9;
    }
}