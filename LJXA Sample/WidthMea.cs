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
    public partial class widthMea : Form
    {
        private Mat heightData;
        Mat binaryImage = new Mat();
        Mat binaryImageSmall = new Mat();
        Mat binaryShowFull = new Mat();
        Mat binaryImagePartial = new Mat();
        Mat binaryImageShowPartial = new Mat();
        DataTable widthTable = new DataTable();
        public widthMea(Mat height)
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

            if (widthTable.Columns.Count == 0)
            {
                widthTable.Columns.Add($"Width", typeof(double));
                widthTable.Columns.Add($"AreaY", typeof(int));
                widthTable.Columns.Add($"AreaX", typeof(int));
                widthTable.Columns.Add($"AreaW", typeof(int));
                widthTable.Columns.Add($"AreaH", typeof(int));
            }

        }


        public void InitializeDataBinding()
        {
            dataGridViewWidth.DataSource = widthTable;
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
                DataRow row = widthTable.NewRow();
                if (!isZoom)
                {
                    drawing = false;

                    
                    row[1] = startPoint.X * 10;
                    row[2] = startPoint.Y * 10;
                    row[3] = (endPoint.X - startPoint.X) * 10;
                    row[4] = (endPoint.Y - startPoint.Y) * 10;
                    

                    add = false;
                    //Show Area
                    Rect roi = new Rect(startPoint.X, startPoint.Y, (endPoint.X - startPoint.X), (endPoint.Y - startPoint.Y));
                    Mat subMat = new Mat(binaryShowFull, roi);
                    //Count Width
                    Rect roiActual =new Rect(startPoint.X * 10, startPoint.Y * 10, (endPoint.X - startPoint.X) * 10, (endPoint.Y - startPoint.Y) * 10);
                    Mat calMat = new Mat(binaryImage, roiActual);
                    calMat.ConvertTo(calMat,MatType.CV_8UC1);
                    cv2showimg(calMat);
                    // Define the kernel size
                    Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(15, 15));

                    // Apply opening
                    Cv2.MorphologyEx(calMat, calMat, MorphTypes.Open, element);

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

                    Point2f center1 = new OpenCvSharp.Point();
                    Point2f center2 = new OpenCvSharp.Point();

                    OpenCvSharp.Point[] largestContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).FirstOrDefault();
                    if (largestContour != null)
                    {
                        RotatedRect rect1 = Cv2.MinAreaRect(largestContour);

                        Point2f[] rectPoints = Cv2.BoxPoints(rect1);
                        OpenCvSharp.Point[] points = rectPoints.Select(p => new OpenCvSharp.Point((int)p.X, (int)p.Y)).ToArray();

                        Cv2.Polylines(calMat, new OpenCvSharp.Point[][] { points }, true, new Scalar(0, 255, 0), 2);
                        center1 = rect1.Center;
                        cv2showimg(calMat);
                    }
                    OpenCvSharp.Point[] secondLargestContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).ElementAtOrDefault(1);
                    if (secondLargestContour != null)
                    {
                        RotatedRect rect2 = Cv2.MinAreaRect(secondLargestContour);

                        Point2f[] rectPoints2 = Cv2.BoxPoints(rect2);
                        OpenCvSharp.Point[] points2 = rectPoints2.Select(p => new OpenCvSharp.Point((int)p.X, (int)p.Y)).ToArray();

                        Cv2.Polylines(calMat, new OpenCvSharp.Point[][] { points2 }, true, new Scalar(0, 255, 0), 2);
                        center2 = rect2.Center;
                        cv2showimg(calMat);
                    }
                    Cv2.Line(calMat, new OpenCvSharp.Point((int)center1.X, (int)center1.Y), new OpenCvSharp.Point((int)center2.X, (int)center2.Y), new Scalar(255, 0, 0), 2);
                    cv2showimg(calMat);
                    double distance = Math.Sqrt(Math.Pow(center2.X - center1.X, 2) + Math.Pow(center2.Y - center1.Y, 2));
                    row[0] = distance;
                    widthTable.Rows.Add(row);
                    cv2showimg(calMat);

                }
                else
                {
                    drawing = false;

                    row[1] = startPoint.X+centerPoint.X*10;
                    row[2] = startPoint.Y+centerPoint.Y*10;
                    row[3] = (endPoint.X - startPoint.X);
                    row[4] = (endPoint.Y - startPoint.Y); 

                    add = false;
                    //Show Area
                    Rect roi = new Rect(startPoint.X, startPoint.Y , (endPoint.X - startPoint.X), (endPoint.Y - startPoint.Y));
                    Mat subMat = new Mat(binaryImageShowPartial, roi);
                    cv2showimg(subMat);
                    //Count Width
                    Rect roiActual = new Rect(startPoint.X, startPoint.Y, (endPoint.X - startPoint.X), (endPoint.Y - startPoint.Y));
                    Mat calMat = new Mat(binaryImagePartial, roiActual);
                    cv2showimg(calMat);


                    calMat.ConvertTo(calMat, MatType.CV_8UC1);
                    // Define the kernel size
                    Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(15, 15));

                    // Apply opening
                    Cv2.MorphologyEx(calMat, calMat, MorphTypes.Open, element);

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

                    Point2f center1 = new OpenCvSharp.Point();
                    Point2f center2 = new OpenCvSharp.Point();

                    OpenCvSharp.Point[] largestContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).FirstOrDefault();
                    if (largestContour != null)
                    {
                        RotatedRect rect1 = Cv2.MinAreaRect(largestContour);

                        Point2f[] rectPoints = Cv2.BoxPoints(rect1);
                        OpenCvSharp.Point[] points = rectPoints.Select(p => new OpenCvSharp.Point((int)p.X, (int)p.Y)).ToArray();

                        Cv2.Polylines(calMat, new OpenCvSharp.Point[][] { points }, true, new Scalar(0, 255, 0), 2);
                        center1 = rect1.Center;
                        cv2showimg(calMat);
                    }

                    OpenCvSharp.Point[] secondLargestContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).ElementAtOrDefault(1);
                    if (secondLargestContour != null)
                    {
                        RotatedRect rect2 = Cv2.MinAreaRect(secondLargestContour);

                        Point2f[] rectPoints2 = Cv2.BoxPoints(rect2);
                        OpenCvSharp.Point[] points2 = rectPoints2.Select(p => new OpenCvSharp.Point((int)p.X, (int)p.Y)).ToArray();

                        Cv2.Polylines(calMat, new OpenCvSharp.Point[][] { points2 }, true, new Scalar(0, 255, 0), 2);
                        center2 = rect2.Center;
                        cv2showimg(calMat);
                    }


                    Cv2.Line(calMat, new OpenCvSharp.Point((int)center1.X, (int)center1.Y), new OpenCvSharp.Point((int)center2.X, (int)center2.Y), new Scalar(255, 0, 0), 2);
                    
                    double distance = Math.Sqrt(Math.Pow(center2.X - center1.X, 2) + Math.Pow(center2.Y - center1.Y, 2));
                    row[0] = distance;
                    widthTable.Rows.Add(row);
                    cv2showimg(calMat);
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

        private void addWMArea_Click(object sender, EventArgs e)
        {
            add = true;
        }
    }
}
