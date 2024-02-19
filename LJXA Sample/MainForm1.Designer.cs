namespace LJXASample
{
    partial class MainForm1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mousePos = new System.Windows.Forms.Label();
            this.restart = new System.Windows.Forms.Button();
            this.readTIF = new System.Windows.Forms.CheckBox();
            this.callHeight = new System.Windows.Forms.Button();
            this.LoCC = new System.Windows.Forms.Button();
            this.combineImage = new System.Windows.Forms.CheckBox();
            this.widthMea = new System.Windows.Forms.Button();
            this.searchT = new System.Windows.Forms.Button();
            this.TMScore = new System.Windows.Forms.Label();
            this.spotCount = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 700);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(874, 722);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 51);
            this.button1.TabIndex = 1;
            this.button1.Text = "View";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(377, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(320, 700);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(732, 12);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(320, 700);
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox3_MouseDown);
            this.pictureBox3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox3_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 689);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // mousePos
            // 
            this.mousePos.AutoSize = true;
            this.mousePos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.mousePos.Font = new System.Drawing.Font("Arial Black", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.mousePos.Location = new System.Drawing.Point(728, 689);
            this.mousePos.Name = "mousePos";
            this.mousePos.Size = new System.Drawing.Size(80, 23);
            this.mousePos.TabIndex = 6;
            this.mousePos.Text = "( X , X )";
            // 
            // restart
            // 
            this.restart.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.restart.Location = new System.Drawing.Point(966, 723);
            this.restart.Name = "restart";
            this.restart.Size = new System.Drawing.Size(86, 51);
            this.restart.TabIndex = 7;
            this.restart.Text = "Restart";
            this.restart.UseVisualStyleBackColor = true;
            this.restart.Click += new System.EventHandler(this.restart_Click);
            // 
            // readTIF
            // 
            this.readTIF.AutoSize = true;
            this.readTIF.Checked = true;
            this.readTIF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.readTIF.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.readTIF.Location = new System.Drawing.Point(707, 722);
            this.readTIF.Name = "readTIF";
            this.readTIF.Size = new System.Drawing.Size(110, 25);
            this.readTIF.TabIndex = 8;
            this.readTIF.Text = "Read_TIF";
            this.readTIF.UseVisualStyleBackColor = true;
            // 
            // callHeight
            // 
            this.callHeight.Location = new System.Drawing.Point(12, 726);
            this.callHeight.Name = "callHeight";
            this.callHeight.Size = new System.Drawing.Size(127, 50);
            this.callHeight.TabIndex = 9;
            this.callHeight.Text = "Height M";
            this.callHeight.UseVisualStyleBackColor = true;
            this.callHeight.Click += new System.EventHandler(this.callHeight_Click);
            // 
            // LoCC
            // 
            this.LoCC.Location = new System.Drawing.Point(145, 727);
            this.LoCC.Name = "LoCC";
            this.LoCC.Size = new System.Drawing.Size(127, 50);
            this.LoCC.TabIndex = 10;
            this.LoCC.Text = "Loctaion Check";
            this.LoCC.UseVisualStyleBackColor = true;
            this.LoCC.Click += new System.EventHandler(this.LoCC_Click);
            // 
            // combineImage
            // 
            this.combineImage.AutoSize = true;
            this.combineImage.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.combineImage.Location = new System.Drawing.Point(707, 748);
            this.combineImage.Name = "combineImage";
            this.combineImage.Size = new System.Drawing.Size(161, 25);
            this.combineImage.TabIndex = 11;
            this.combineImage.Text = "Combine_Image";
            this.combineImage.UseVisualStyleBackColor = true;
            // 
            // widthMea
            // 
            this.widthMea.Enabled = false;
            this.widthMea.Location = new System.Drawing.Point(278, 726);
            this.widthMea.Name = "widthMea";
            this.widthMea.Size = new System.Drawing.Size(127, 50);
            this.widthMea.TabIndex = 12;
            this.widthMea.Text = "Width M";
            this.widthMea.UseVisualStyleBackColor = true;
            this.widthMea.Click += new System.EventHandler(this.widthMea_Click);
            // 
            // searchT
            // 
            this.searchT.Location = new System.Drawing.Point(544, 727);
            this.searchT.Name = "searchT";
            this.searchT.Size = new System.Drawing.Size(127, 50);
            this.searchT.TabIndex = 13;
            this.searchT.Text = "Search";
            this.searchT.UseVisualStyleBackColor = true;
            this.searchT.Click += new System.EventHandler(this.searchT_Click);
            // 
            // TMScore
            // 
            this.TMScore.AutoSize = true;
            this.TMScore.BackColor = System.Drawing.Color.Transparent;
            this.TMScore.Font = new System.Drawing.Font("Arial Black", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TMScore.Location = new System.Drawing.Point(384, 21);
            this.TMScore.Name = "TMScore";
            this.TMScore.Size = new System.Drawing.Size(21, 23);
            this.TMScore.TabIndex = 14;
            this.TMScore.Text = "0";
            // 
            // spotCount
            // 
            this.spotCount.Enabled = false;
            this.spotCount.Location = new System.Drawing.Point(411, 727);
            this.spotCount.Name = "spotCount";
            this.spotCount.Size = new System.Drawing.Size(127, 50);
            this.spotCount.TabIndex = 15;
            this.spotCount.Text = "Spot Count";
            this.spotCount.UseVisualStyleBackColor = true;
            this.spotCount.Click += new System.EventHandler(this.spotCount_Click);
            // 
            // MainForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 793);
            this.Controls.Add(this.spotCount);
            this.Controls.Add(this.TMScore);
            this.Controls.Add(this.searchT);
            this.Controls.Add(this.widthMea);
            this.Controls.Add(this.combineImage);
            this.Controls.Add(this.LoCC);
            this.Controls.Add(this.callHeight);
            this.Controls.Add(this.readTIF);
            this.Controls.Add(this.restart);
            this.Controls.Add(this.mousePos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainForm1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label mousePos;
        private System.Windows.Forms.Button restart;
        private System.Windows.Forms.CheckBox readTIF;
        private System.Windows.Forms.Button callHeight;
        private System.Windows.Forms.Button LoCC;
        private System.Windows.Forms.CheckBox combineImage;
        private System.Windows.Forms.Button widthMea;
        private System.Windows.Forms.Button searchT;
        private System.Windows.Forms.Label TMScore;
        private System.Windows.Forms.Button spotCount;
    }
}

