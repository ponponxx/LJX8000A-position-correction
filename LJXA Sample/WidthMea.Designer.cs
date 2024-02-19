namespace LJXASample
{
    partial class widthMea
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
            this.dataGridViewWidth = new System.Windows.Forms.DataGridView();
            this.binaryPic = new System.Windows.Forms.PictureBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.addWMArea = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binaryPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewWidth
            // 
            this.dataGridViewWidth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWidth.Location = new System.Drawing.Point(362, 89);
            this.dataGridViewWidth.Name = "dataGridViewWidth";
            this.dataGridViewWidth.RowTemplate.Height = 24;
            this.dataGridViewWidth.Size = new System.Drawing.Size(407, 346);
            this.dataGridViewWidth.TabIndex = 0;
            // 
            // binaryPic
            // 
            this.binaryPic.BackColor = System.Drawing.Color.Transparent;
            this.binaryPic.Location = new System.Drawing.Point(12, 12);
            this.binaryPic.Name = "binaryPic";
            this.binaryPic.Size = new System.Drawing.Size(320, 700);
            this.binaryPic.TabIndex = 12;
            this.binaryPic.TabStop = false;
            this.binaryPic.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.binaryPic_MouseDoubleClick);
            this.binaryPic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.binaryPic_MouseDown);
            this.binaryPic.MouseMove += new System.Windows.Forms.MouseEventHandler(this.binaryPic_MouseMove);
            this.binaryPic.MouseUp += new System.Windows.Forms.MouseEventHandler(this.binaryPic_MouseUp);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(362, 49);
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
            this.numericUpDown1.TabIndex = 14;
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
            this.label1.Location = new System.Drawing.Point(358, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 24);
            this.label1.TabIndex = 15;
            this.label1.Text = "閾值";
            // 
            // addWMArea
            // 
            this.addWMArea.Location = new System.Drawing.Point(362, 441);
            this.addWMArea.Name = "addWMArea";
            this.addWMArea.Size = new System.Drawing.Size(107, 42);
            this.addWMArea.TabIndex = 17;
            this.addWMArea.Text = "ADD";
            this.addWMArea.UseVisualStyleBackColor = true;
            this.addWMArea.Click += new System.EventHandler(this.addWMArea_Click);
            // 
            // widthMea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 724);
            this.Controls.Add(this.addWMArea);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.binaryPic);
            this.Controls.Add(this.dataGridViewWidth);
            this.Name = "widthMea";
            this.Text = "widthMea";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binaryPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewWidth;
        private System.Windows.Forms.PictureBox binaryPic;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addWMArea;
    }
}