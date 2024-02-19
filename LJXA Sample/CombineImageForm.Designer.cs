namespace LJXASample
{
    partial class CombineImageForm
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
            this.resultCombinedImage = new System.Windows.Forms.PictureBox();
            this.GetData = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.xPictureNumber = new System.Windows.Forms.NumericUpDown();
            this.yPictureNumber = new System.Windows.Forms.NumericUpDown();
            this.Xnumber = new System.Windows.Forms.Label();
            this.Ynumber = new System.Windows.Forms.Label();
            this.yimagesizeeach = new System.Windows.Forms.NumericUpDown();
            this.ximagesizeeach = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.XCut = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.resultCombinedImage)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xPictureNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yPictureNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yimagesizeeach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ximagesizeeach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XCut)).BeginInit();
            this.SuspendLayout();
            // 
            // resultCombinedImage
            // 
            this.resultCombinedImage.Location = new System.Drawing.Point(0, 0);
            this.resultCombinedImage.Name = "resultCombinedImage";
            this.resultCombinedImage.Size = new System.Drawing.Size(787, 615);
            this.resultCombinedImage.TabIndex = 0;
            this.resultCombinedImage.TabStop = false;
            // 
            // GetData
            // 
            this.GetData.Location = new System.Drawing.Point(696, 651);
            this.GetData.Name = "GetData";
            this.GetData.Size = new System.Drawing.Size(103, 48);
            this.GetData.TabIndex = 1;
            this.GetData.Text = "GetData";
            this.GetData.UseVisualStyleBackColor = true;
            this.GetData.Click += new System.EventHandler(this.GetData_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.resultCombinedImage);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(798, 629);
            this.panel1.TabIndex = 2;
            // 
            // xPictureNumber
            // 
            this.xPictureNumber.Location = new System.Drawing.Point(12, 677);
            this.xPictureNumber.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.xPictureNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.xPictureNumber.Name = "xPictureNumber";
            this.xPictureNumber.Size = new System.Drawing.Size(83, 22);
            this.xPictureNumber.TabIndex = 3;
            this.xPictureNumber.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // yPictureNumber
            // 
            this.yPictureNumber.Location = new System.Drawing.Point(101, 677);
            this.yPictureNumber.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.yPictureNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.yPictureNumber.Name = "yPictureNumber";
            this.yPictureNumber.Size = new System.Drawing.Size(83, 22);
            this.yPictureNumber.TabIndex = 4;
            this.yPictureNumber.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Xnumber
            // 
            this.Xnumber.AutoSize = true;
            this.Xnumber.Location = new System.Drawing.Point(14, 656);
            this.Xnumber.Name = "Xnumber";
            this.Xnumber.Size = new System.Drawing.Size(54, 12);
            this.Xnumber.TabIndex = 5;
            this.Xnumber.Text = "X Number";
            // 
            // Ynumber
            // 
            this.Ynumber.AutoSize = true;
            this.Ynumber.Location = new System.Drawing.Point(99, 656);
            this.Ynumber.Name = "Ynumber";
            this.Ynumber.Size = new System.Drawing.Size(54, 12);
            this.Ynumber.TabIndex = 6;
            this.Ynumber.Text = "Y Number";
            // 
            // yimagesizeeach
            // 
            this.yimagesizeeach.Location = new System.Drawing.Point(326, 677);
            this.yimagesizeeach.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.yimagesizeeach.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.yimagesizeeach.Name = "yimagesizeeach";
            this.yimagesizeeach.Size = new System.Drawing.Size(83, 22);
            this.yimagesizeeach.TabIndex = 8;
            this.yimagesizeeach.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // ximagesizeeach
            // 
            this.ximagesizeeach.Location = new System.Drawing.Point(206, 677);
            this.ximagesizeeach.Maximum = new decimal(new int[] {
            3200,
            0,
            0,
            0});
            this.ximagesizeeach.Minimum = new decimal(new int[] {
            2800,
            0,
            0,
            0});
            this.ximagesizeeach.Name = "ximagesizeeach";
            this.ximagesizeeach.Size = new System.Drawing.Size(83, 22);
            this.ximagesizeeach.TabIndex = 7;
            this.ximagesizeeach.Value = new decimal(new int[] {
            3200,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(324, 656);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "Y Image Size Each";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(204, 656);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "X Image Size Each";
            // 
            // XCut
            // 
            this.XCut.Location = new System.Drawing.Point(444, 677);
            this.XCut.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.XCut.Name = "XCut";
            this.XCut.Size = new System.Drawing.Size(83, 22);
            this.XCut.TabIndex = 11;
            this.XCut.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(442, 656);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "Cut X both side";
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(587, 651);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(103, 48);
            this.search.TabIndex = 13;
            this.search.Text = "search";
            this.search.UseVisualStyleBackColor = true;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // CombineImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 711);
            this.Controls.Add(this.search);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.XCut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.yimagesizeeach);
            this.Controls.Add(this.ximagesizeeach);
            this.Controls.Add(this.Ynumber);
            this.Controls.Add(this.Xnumber);
            this.Controls.Add(this.yPictureNumber);
            this.Controls.Add(this.xPictureNumber);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.GetData);
            this.Name = "CombineImageForm";
            this.Text = "CombineImageForm";
            ((System.ComponentModel.ISupportInitialize)(this.resultCombinedImage)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xPictureNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yPictureNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yimagesizeeach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ximagesizeeach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XCut)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox resultCombinedImage;
        private System.Windows.Forms.Button GetData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown xPictureNumber;
        private System.Windows.Forms.NumericUpDown yPictureNumber;
        private System.Windows.Forms.Label Xnumber;
        private System.Windows.Forms.Label Ynumber;
        private System.Windows.Forms.NumericUpDown yimagesizeeach;
        private System.Windows.Forms.NumericUpDown ximagesizeeach;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown XCut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button search;
    }
}