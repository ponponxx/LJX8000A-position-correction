using OpenCvSharp;
using OpenCvSharp.Extensions;
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

namespace LJXASample
{
    public partial class spotCount : Form
    {
        private Mat heightData;
        Mat binaryImage = new Mat();
        Mat binaryImageSmall = new Mat();
        Mat binaryShowFull = new Mat();
        Mat binaryImagePartial = new Mat();
        Mat binaryImageShowPartial = new Mat();
        DataTable spotTable = new DataTable();
        public spotCount(Mat height)
        {
            InitializeComponent();
            InitializeDataBinding();

            heightData = height;
            binaryPic.Paint += binaryPic_Paint;

            //資料轉2質化
            //binaryPic貼圖
            binaryImage = new Mat(heightData.Cols, heightData.Rows, MatType.CV_16UC1);
            Cv2.Threshold(heightData, binaryImage, (double)numericUpDown1.Value, 65535, ThresholdTypes.Binary);
            Cv2.Resize(binaryImage, binaryImageSmall, OpenCvSharp.Size.Zero, 0.1f, 0.1f);
            binaryImageSmall.ConvertTo(binaryShowFull, MatType.CV_8UC1);
            binaryPic.Image = BitmapConverter.ToBitmap(binaryShowFull);
            if (spotTable.Columns.Count == 0)
            {
                spotTable.Columns.Add($"Spot Count", typeof(int));
                spotTable.Columns.Add($"Max Spot Size", typeof(int));
                spotTable.Columns.Add($"Min Spot Size", typeof(int));
                /*
                spotTable.Columns.Add($"AreaY", typeof(int));
                spotTable.Columns.Add($"AreaX", typeof(int));
                spotTable.Columns.Add($"AreaW", typeof(int));
                spotTable.Columns.Add($"AreaH", typeof(int));
                */
            }

        }


        private System.Drawing.Point startPoint;
        private System.Drawing.Point endPoint;
        private System.Drawing.Point centerPoint;
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

        }

        private void binaryPic_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                DataRow row = spotTable.NewRow();
                if (!isZoom) //縮小狀態
                {
                    drawing = false;
                    /*
                    row[2] = startPoint.X * 10;
                    row[3] = startPoint.Y * 10;
                    row[4] = (endPoint.X - startPoint.X) * 10;
                    row[5] = (endPoint.Y - startPoint.Y) * 10;
                    */

                    add = false;
                    //Show Area
                    Rect roi = new Rect(startPoint.X, startPoint.Y, (endPoint.X - startPoint.X), (endPoint.Y - startPoint.Y));
                    Mat subMat = new Mat(binaryShowFull, roi);
                    //Cut the data out
                    Rect roiActual =new Rect(startPoint.X * 10, startPoint.Y * 10, (endPoint.X - startPoint.X) * 10, (endPoint.Y - startPoint.Y) * 10);
                    Mat calMat = new Mat(binaryImage, roiActual);
                    calMat.ConvertTo(calMat,MatType.CV_8UC1);
                    cv2showimg(calMat);

                    // Define the kernel size
                    Mat elementClose = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5));
                    Mat elementOpen = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(15, 15));
                    

                    // Apply opening
                    Cv2.MorphologyEx(calMat, calMat, MorphTypes.Close, elementClose);
                    Cv2.MorphologyEx(calMat, calMat, MorphTypes.Open, elementOpen);
                    Rect rectEdge = new Rect(0,0,calMat.Cols,calMat.Rows);
                    Cv2.Rectangle(calMat, rectEdge, new Scalar(0,0,0), 1);
                    cv2showimg(calMat);

                    Mat cannyEdges = new Mat();
                    double threshold1 = 50; // These values can be adjusted
                    double threshold2 = 150;
                    Cv2.Canny(calMat, cannyEdges, threshold1, threshold2);
                    cv2showimg(cannyEdges);


                    OpenCvSharp.Point[][] contours;
                    HierarchyIndex[] hierarchy;
                    Cv2.FindContours(cannyEdges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                    Cv2.CvtColor(calMat, calMat, ColorConversionCodes.GRAY2BGR);
                    Cv2.DrawContours(calMat, contours, -1, new Scalar(0, 0, 255), 2);

                    cv2showimg(calMat);

                    var sortedContours = contours
                        .OrderByDescending(c => Cv2.ContourArea(c))
                        .Where(c => Cv2.ContourArea(c) > 500)
                        .ToArray();
                    for (int count = 0; count < sortedContours.Length; count++)
                    {
                        OpenCvSharp.Point[] currentContours = sortedContours[count];
                        /*
                        RotatedRect rect = Cv2.MinAreaRect(currentContours);
                        Point2f[] rectPoints = Cv2.BoxPoints(rect);
                        OpenCvSharp.Point[] points = rectPoints.Select(p => new OpenCvSharp.Point((int)p.X, (int)p.Y)).ToArray();
                        Cv2.Polylines(calMat, new OpenCvSharp.Point[][] { points }, true, new Scalar(0, 255, 0), 2);
                        //MessageBox.Show("Size" + Cv2.ContourArea(currentContours));
                        */

                        if (count == 0)
                        {
                            row[1] = (int)Cv2.ContourArea(currentContours);
                        }
                        else if(count == sortedContours.Length-1)
                        {
                            row[2] = (int)Cv2.ContourArea(currentContours);
                        }
                    }

                    row[0] = sortedContours.Length;
                    spotTable.Rows.Add(row);

                }
                else //放大狀態
                {
                    drawing = false;
                    /*
                    row[2] = startPoint.X+centerPoint.X*10;
                    row[3] = startPoint.Y+centerPoint.Y*10;
                    row[4] = (endPoint.X - startPoint.X);
                    row[5] = (endPoint.Y - startPoint.Y); 
                    */
                    add = false;
                    //Show Area
                    Rect roi = new Rect(startPoint.X, startPoint.Y , (endPoint.X - startPoint.X), (endPoint.Y - startPoint.Y));
                    Mat subMat = new Mat(binaryImageShowPartial, roi);
                    cv2showimg(subMat);
                    //Cut the data out
                    Rect roiActual = new Rect(startPoint.X, startPoint.Y, (endPoint.X - startPoint.X), (endPoint.Y - startPoint.Y));
                    Mat calMat = new Mat(binaryImagePartial, roiActual);
                    cv2showimg(calMat);


                    calMat.ConvertTo(calMat, MatType.CV_8UC1);
                    // Define the kernel size
                    Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(15, 15));

                    // Apply opening
                    Cv2.MorphologyEx(calMat, calMat, MorphTypes.Open, element);
                    Cv2.MorphologyEx(calMat, calMat, MorphTypes.Close, element);
                    Rect rectEdge = new Rect(0, 0, calMat.Cols, calMat.Rows);
                    Cv2.Rectangle(calMat, rectEdge, new Scalar(0, 0, 0), 1);
                    cv2showimg(calMat);

                    Mat cannyEdges = new Mat();
                    double threshold1 = 50; // These values can be adjusted
                    double threshold2 = 150;
                    Cv2.Canny(calMat, cannyEdges, threshold1, threshold2);
                    cv2showimg(cannyEdges);
                    OpenCvSharp.Point[][] contours;
                    HierarchyIndex[] hierarchy;
                    Cv2.FindContours(cannyEdges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                    Cv2.CvtColor(calMat, calMat, ColorConversionCodes.GRAY2BGR);
                    Cv2.DrawContours(calMat, contours, -1, new Scalar(0, 0, 255), 2);
                    cv2showimg(calMat);

                    var sortedContours = contours
                        .OrderByDescending(c => Cv2.ContourArea(c))
                        .Where(c=> Cv2.ContourArea(c)>500)
                        .ToArray();

                    for (int count = 0; count < sortedContours.Length; count++)
                    {
                        OpenCvSharp.Point[] currentContours = sortedContours[count];
                        /*
                        RotatedRect rect = Cv2.MinAreaRect(currentContours);
                        Point2f[] rectPoints = Cv2.BoxPoints(rect);
                        OpenCvSharp.Point[] points = rectPoints.Select(p => new OpenCvSharp.Point((int)p.X, (int)p.Y)).ToArray();
                        Cv2.Polylines(calMat, new OpenCvSharp.Point[][] { points }, true, new Scalar(0, 255, 0), 2);
                        //MessageBox.Show("Size" + Cv2.ContourArea(currentContours));
                        */

                        if (count == 0)
                        {
                            row[1] = (int)Cv2.ContourArea(currentContours);
                        }
                        else if (count == sortedContours.Length - 1)
                        {
                            row[2] = (int)Cv2.ContourArea(currentContours);
                        }
                    }

                    row[0] = sortedContours.Length;

                    row[0] = sortedContours.Length;
                    spotTable.Rows.Add(row);
                }
                
            }
        }

        bool isZoom = false;
        private void binaryPic_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            centerPoint = e.Location;
            if (!isZoom)
            {
                //圖片截圖重畫

                Rect roi = new Rect((e.Location.X*10)-160, (e.Location.Y*10)-200, 320, 400);
                Mat subMat = new Mat(binaryImage, roi);
                subMat.CopyTo(binaryImagePartial);
                binaryImagePartial.ConvertTo(binaryImageShowPartial, MatType.CV_8UC1);
                binaryPic.Image = BitmapConverter.ToBitmap(binaryImageShowPartial);
                isZoom = true;
            }
            else
            {
                binaryPic.Image = BitmapConverter.ToBitmap(binaryShowFull);
                isZoom= false;
            }


        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            binaryImage = new Mat(heightData.Cols, heightData.Rows, MatType.CV_16UC1);
            Cv2.Threshold(heightData, binaryImage, (double)numericUpDown1.Value, 65535, ThresholdTypes.Binary);
            Cv2.Resize(binaryImage, binaryImageSmall, OpenCvSharp.Size.Zero, 0.1f, 0.1f);
            binaryImageSmall.ConvertTo(binaryShowFull, MatType.CV_8UC1);
            binaryPic.Image = BitmapConverter.ToBitmap(binaryShowFull);
        }



        private void cv2showimg(Mat img)
        {
            Mat imgshow = new Mat();
            imgshow = img.Clone();
            Cv2.ImShow("mask", imgshow);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private void addArea_Click(object sender, EventArgs e)
        {
            add = true;
        }
        public void InitializeDataBinding()
        {
            dataGridViewSpot.DataSource = spotTable;
        }
    }
}
