using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Security.Cryptography;
using System.Runtime.InteropServices.WindowsRuntime;

namespace LJXASample
{
    public partial class LocCheck : Form
    {
        private Mat heightData;
        Mat binaryImage = new Mat();
        Mat binaryImageSmall = new Mat();
        Mat binaryShow = new Mat();
        DataTable AreaTable = new DataTable();

        public LocCheck(Mat height)
        {
            InitializeComponent();
            InitializeDataBinding();
            heightData = height;
            binaryPic.Paint += binaryPic_Paint;

            //資料轉2質化
            //binaryPic貼圖
            binaryImage = new Mat(heightData.Cols,heightData.Rows,MatType.CV_16UC1);
            Cv2.Threshold(heightData, binaryImage, (double)numericUpDown1.Value, 65535, ThresholdTypes.Binary);
            Cv2.Resize(binaryImage, binaryImageSmall, OpenCvSharp.Size.Zero, 0.1f, 0.1f);
            binaryImageSmall.ConvertTo(binaryShow, MatType.CV_8UC1);
            binaryPic.Image = BitmapConverter.ToBitmap(binaryShow);
            if (AreaTable.Columns.Count == 0)
            {
                AreaTable.Columns.Add($"AreaY", typeof(int));
                AreaTable.Columns.Add($"AreaX", typeof(int));
                AreaTable.Columns.Add($"AreaW", typeof(int));
                AreaTable.Columns.Add($"AreaH", typeof(int));
            }
        }

        private System.Drawing.Point startPoint;
        private System.Drawing.Point endPoint;
        private bool drawing = false;
        private bool add = false;

        private void binaryPic_Paint(object sender, PaintEventArgs e)
        {
            if (drawing)
            {
                // Draw a rectangle on the overlayPictureBox
                using (Pen pen = new Pen(Color.MediumVioletRed, 3))
                {
                    Rectangle rect = new Rectangle(startPoint, new System.Drawing.Size(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y));
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        private void binaryPic_MouseDown(object sender, MouseEventArgs e)
        {
            if (add)
            {
                startPoint = e.Location;
                if (startPoint.X < 320 & startPoint.Y < 400)
                {
                    drawing = true;
                }
            }

        }

        private void binaryPic_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing & add)
            {
                endPoint = e.Location;
                if (e.X > 320)
                {
                    endPoint.X = 320;
                }
                if (e.Y > (heightData.Height / 10))
                {
                    endPoint.Y = heightData.Height / 10;
                }
                binaryPic.Invalidate(); // Redraw the overlay
            }
            int x = e.X * 10;
            int y = e.Y * 10;
        }

        private void cv2showimg(Mat img)
        {
            Mat imgshow = new Mat();
            imgshow = img.Clone();
            Cv2.Resize(imgshow, imgshow, OpenCvSharp.Size.Zero, 0.1f,0.1f);
            Cv2.ImShow("mask", imgshow);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

        }

        private bool haveoverlay(Mat source, Mat mask) 
        {
            Mat result = new Mat(source.Rows,source.Cols,MatType.CV_16UC1);
            Cv2.BitwiseAnd(source, mask, result);
            int overlay = 0;
            overlay = Cv2.CountNonZero(result);
            if(overlay == 0) 
            {
                return true; 
            }
            else
            {
                return false;
            }
                

            
        }

        private void binaryPic_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;

                DataRow row = AreaTable.NewRow();
                row[0] = startPoint.X * 10;
                row[1] = startPoint.Y * 10;
                row[2] = (endPoint.X - startPoint.X) * 10;
                row[3] = (endPoint.Y - startPoint.Y) * 10;
                AreaTable.Rows.Add(row);

                add = false;
                //Draw Area
                int area;
                int rowsCount = AreaTable.Rows.Count;
                Mat mask = new Mat(binaryImage.Rows, binaryImage.Cols, MatType.CV_16UC1);
                for (area = 0; area < rowsCount; area++)
                {
                    Rect roi = new Rect((int)AreaTable.Rows[area][0], (int)AreaTable.Rows[area][1], (int)AreaTable.Rows[area][2], (int)AreaTable.Rows[area][3]);
                    Cv2.Rectangle(mask, roi, 65535, -1);
                }
                

                //計算OKNG


                Cv2.BitwiseNot(mask, mask);
                cv2showimg(mask);

                bool result;
                result = haveoverlay(binaryImage,mask);
                if (result)
                {
                    OKNG.BackColor = Color.Green;
                    OKNG.Text = "OK";
                }
                else
                {
                    OKNG.BackColor= Color.Red;
                    OKNG.Text = "NG";
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            binaryImage = new Mat(heightData.Cols, heightData.Rows, MatType.CV_16UC1);
            Cv2.Threshold(heightData, binaryImage, (double)numericUpDown1.Value, 65535, ThresholdTypes.Binary);
            Cv2.Resize(binaryImage, binaryImageSmall, OpenCvSharp.Size.Zero, 0.1f, 0.1f);
            binaryImageSmall.ConvertTo(binaryShow, MatType.CV_8UC1);
            binaryPic.Image = BitmapConverter.ToBitmap(binaryShow);
        }
        private void InitializeDataBinding()
        {
            // Bind the DataTable to the DataGridView
            dataGridView1.DataSource = AreaTable;
        }
        private void addHMArea_Click(object sender, EventArgs e)
        {
            add = true;
        }

        private void clearTable_Click(object sender, EventArgs e)
        {
            
                AreaTable.Clear();

        }
    }
}
