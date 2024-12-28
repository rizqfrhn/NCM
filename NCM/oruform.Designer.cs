namespace NCM
{
    partial class oruform
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgv_oru = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgv_oru).BeginInit();
            SuspendLayout();
            // 
            // dgv_oru
            // 
            dgv_oru.AllowUserToAddRows = false;
            dgv_oru.AllowUserToDeleteRows = false;
            dgv_oru.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv_oru.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_oru.Location = new Point(8, 7);
            dgv_oru.Margin = new Padding(2);
            dgv_oru.Name = "dgv_oru";
            dgv_oru.RowHeadersWidth = 62;
            dgv_oru.Size = new Size(1141, 494);
            dgv_oru.TabIndex = 0;
            // 
            // oruform
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1160, 512);
            Controls.Add(dgv_oru);
            Margin = new Padding(2);
            MaximumSize = new Size(1277, 556);
            MinimumSize = new Size(960, 451);
            Name = "oruform";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ORU Management";
            Shown += oruform_Shown;
            ((System.ComponentModel.ISupportInitialize)dgv_oru).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgv_oru;
    }
}
