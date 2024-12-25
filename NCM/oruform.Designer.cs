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
            dgv_oru.Location = new Point(12, 12);
            dgv_oru.Name = "dgv_oru";
            dgv_oru.RowHeadersWidth = 62;
            dgv_oru.Size = new Size(1772, 820);
            dgv_oru.TabIndex = 0;
            // 
            // oruform
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1796, 844);
            Controls.Add(dgv_oru);
            MaximumSize = new Size(1818, 900);
            MinimumSize = new Size(1818, 900);
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
