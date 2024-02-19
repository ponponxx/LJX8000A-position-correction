namespace LJXASample
{
    partial class LocCheck
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
            this.binaryPic = new System.Windows.Forms.PictureBox();
            this.OKNG = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.addHMArea = new System.Windows.Forms.Button();
            this.clearTable = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.binaryPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // binaryPic
            // 
            this.binaryPic.BackColor = System.Drawing.Color.Transparent;
            this.binaryPic.Location = new System.Drawing.Point(12, 12);
            this.binaryPic.Name = "binaryPic";
            this.binaryPic.Size = new System.Drawing.Size(320, 700);
            this.binaryPic.TabIndex = 11;
            this.binaryPic.TabStop = false;
            this.binaryPic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.binaryPic_MouseDown);
            this.binaryPic.MouseMove += new System.Windows.Forms.MouseEventHandler(this.binaryPic_MouseMove);
            this.binaryPic.MouseUp += new System.Windows.Forms.MouseEventHandler(this.binaryPic_MouseUp);
            // 
            // OKNG
            // 
            this.OKNG.Font = new System.Drawing.Font("新細明體", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.OKNG.Location = new System.Drawing.Point(720, 31);
            this.OKNG.Name = "OKNG";
            this.OKNG.Size = new System.Drawing.Size(100, 60);
            this.OKNG.TabIndex = 12;
            this.OKNG.Text = "OK";
            this.OKNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(372, 70);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            40000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(107, 22);
            this.numericUpDown1.TabIndex = 13;
            this.numericUpDown1.Value = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(368, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "閾值";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(372, 108);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(448, 381);
            this.dataGridView1.TabIndex = 15;
            // 
            // addHMArea
            // 
            this.addHMArea.Location = new System.Drawing.Point(372, 509);
            this.addHMArea.Name = "addHMArea";
            this.addHMArea.Size = new System.Drawing.Size(107, 42);
            this.addHMArea.TabIndex = 16;
            this.addHMArea.Text = "ADD";
            this.addHMArea.UseVisualStyleBackColor = true;
            this.addHMArea.Click += new System.EventHandler(this.addHMArea_Click);
            // 
            // clearTable
            // 
            this.clearTable.Location = new System.Drawing.Point(499, 511);
            this.clearTable.Name = "clearTable";
            this.clearTable.Size = new System.Drawing.Size(111, 39);
            this.clearTable.TabIndex = 17;
            this.clearTable.Text = "ClearAll";
            this.clearTable.UseVisualStyleBackColor = true;
            this.clearTable.Click += new System.EventHandler(this.clearTable_Click);
            // 
            // LocCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 760);
            this.Controls.Add(this.clearTable);
            this.Controls.Add(this.addHMArea);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.OKNG);
            this.Controls.Add(this.binaryPic);
            this.Name = "LocCheck";
            this.Text = "LocCheck";
            ((System.ComponentModel.ISupportInitialize)(this.binaryPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox binaryPic;
        private System.Windows.Forms.Label OKNG;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button addHMArea;
        private System.Windows.Forms.Button clearTable;
    }
}