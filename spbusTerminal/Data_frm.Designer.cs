namespace spbusTerminal
{
    partial class Data_frm
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
            this.Dates_dataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.Dates_dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // Dates_dataGridView
            // 
            this.Dates_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dates_dataGridView.Location = new System.Drawing.Point(12, 12);
            this.Dates_dataGridView.Name = "Dates_dataGridView";
            this.Dates_dataGridView.Size = new System.Drawing.Size(776, 426);
            this.Dates_dataGridView.TabIndex = 0;
            // 
            // Data_frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Dates_dataGridView);
            this.Name = "Data_frm";
            this.Text = "Данные";
            ((System.ComponentModel.ISupportInitialize)(this.Dates_dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Dates_dataGridView;
    }
}