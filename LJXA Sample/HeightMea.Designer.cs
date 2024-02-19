namespace LJXASample
{
    partial class HeightMea
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
            this.close = new System.Windows.Forms.Button();
            this.XYZ = new System.Windows.Forms.Label();
            this.overlayDrawBox = new System.Windows.Forms.PictureBox();
            this.headSet = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.addHMArea = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.overlayDrawBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(694, 572);
            this.close.Name = "close";
            this.close.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.close.Size = new System.Drawing.Size(108, 40);
            this.close.TabIndex = 2;
            this.close.Text = "CLOSE";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // XYZ
            // 
            this.XYZ.AutoSize = true;
            this.XYZ.Location = new System.Drawing.Point(352, 63);
            this.XYZ.Name = "XYZ";
            this.XYZ.Size = new System.Drawing.Size(42, 12);
            this.XYZ.TabIndex = 9;
            this.XYZ.Text = "(X,Y,Z)";
            // 
            // overlayDrawBox
            // 
            this.overlayDrawBox.BackColor = System.Drawing.Color.Transparent;
            this.overlayDrawBox.Location = new System.Drawing.Point(12, 12);
            this.overlayDrawBox.Name = "overlayDrawBox";
            this.overlayDrawBox.Size = new System.Drawing.Size(320, 700);
            this.overlayDrawBox.TabIndex = 10;
            this.overlayDrawBox.TabStop = false;
            this.overlayDrawBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.overlayDrawBox_MouseDown);
            this.overlayDrawBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.overlayDrawBox_MouseMove);
            this.overlayDrawBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.overlayDrawBox_MouseUp);
            // 
            // headSet
            // 
            this.headSet.FormattingEnabled = true;
            this.headSet.Location = new System.Drawing.Point(370, 29);
            this.headSet.Name = "headSet";
            this.headSet.Size = new System.Drawing.Size(122, 20);
            this.headSet.TabIndex = 11;
            this.headSet.Text = "HeadManualPick";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(354, 98);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(448, 381);
            this.dataGridView1.TabIndex = 12;
            // 
            // addHMArea
            // 
            this.addHMArea.Location = new System.Drawing.Point(695, 485);
            this.addHMArea.Name = "addHMArea";
            this.addHMArea.Size = new System.Drawing.Size(107, 42);
            this.addHMArea.TabIndex = 13;
            this.addHMArea.Text = "ADD";
            this.addHMArea.UseVisualStyleBackColor = true;
            this.addHMArea.Click += new System.EventHandler(this.button1_Click);
            // 
            // HeightMea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 751);
            this.Controls.Add(this.addHMArea);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.headSet);
            this.Controls.Add(this.overlayDrawBox);
            this.Controls.Add(this.XYZ);
            this.Controls.Add(this.close);
            this.Name = "HeightMea";
            this.Text = "HeightMea";
            ((System.ComponentModel.ISupportInitialize)(this.overlayDrawBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Label XYZ;
        private System.Windows.Forms.PictureBox overlayDrawBox;
        private System.Windows.Forms.ComboBox headSet;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button addHMArea;
    }
}