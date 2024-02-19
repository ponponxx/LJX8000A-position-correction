using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace LJXASample
{
    public partial class MainForm1 : Form
    {
        public MainForm1()
        {
            InitializeComponent();
            //MouseLoc();
            callHeight.Enabled = false;
            LoCC.Enabled = false;
            pictureBox1.Enabled = false;
            pictureBox2.Enabled = false;
            pictureBox3.Enabled = false;
        }
        
        private void MouseLoc()
        {
            this.MouseMove += (sender, e) =>
            {
                mousePos.Text = $"({e.X} , {e.Y})";
            };

        }


        int deviceId = 0;                   // Set "0" if you use only 1 head.
        int xImageSize = 3200;              // Number of X points.
        int yImageSize = 4000;              // Number of Y lines.
        float yPitchUm = 12.5f;             // Data pitch of Y data. (e.g. your encoder setting)
        int timeoutMs = 30000;               // Timeout value for the acquiring image (in millisecond).
        int useExternalBatchStart = 0;      // Set "1" if you controll the batch start timing externally. (e.g. terminal input)

        Mat src_H = new Mat();
        Mat src_L = new Mat();
        Mat mixPic = new Mat();
        Mat src_H8bit = new Mat();
        Mat src_L8bit = new Mat();

        bool tilted = false;


        private void button1_Click(object sender, EventArgs e)
        {
            src_H = new Mat(yImageSize, xImageSize, MatType.CV_16UC1);
            src_L = new Mat(yImageSize, xImageSize, MatType.CV_16UC1);
            if (readTIF.Checked && !combineImage.Checked )
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                // Set the initial directory (optional)
                openFileDialog.InitialDirectory = @"C:\";

                // Set the filter for the type of files to display
                openFileDialog.Filter = "tif Files (*.tif)|*.tif|All Files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    src_H = Cv2.ImRead(selectedFilePath, ImreadModes.Unchanged);
                }

                OpenFileDialog openFileDialog2 = new OpenFileDialog();

                // Set the initial directory (optional)
                openFileDialog2.InitialDirectory = @"C:\";

                // Set the filter for the type of files to display
                openFileDialog2.Filter = "tif Files (*.tif)|*.tif|All Files (*.*)|*.*";

                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog2.FileName;
                    src_L = Cv2.ImRead(selectedFilePath, ImreadModes.Unchanged);
                }

            }
            else if(combineImage.Checked)
            {
                //open Form
                CombineImageForm combineImage = new CombineImageForm(readTIF.Checked);
                combineImage.Show();
            }
            else
            {

                LJX8IF_ETHERNET_CONFIG ethernetConfig = new LJX8IF_ETHERNET_CONFIG
                {
                    abyIpAddress = new byte[] { 192, 168, 0, 1 },    // IP address
                    wPortNo = 24691,                                 // Port number
                };

                int HighSpeedPortNo = 24692;         // Port number for high-speed communication


                List<ushort> HeightImage = new List<ushort>();
                List<ushort> LuminanceImage = new List<ushort>();

                // Prepare setting parameter
                SetParam setParam = new SetParam
                {
                    YLineNum = yImageSize,
                    YPitchUm = yPitchUm,
                    TimeoutMs = timeoutMs,
                    UseExternalBatchStart = useExternalBatchStart,
                };

                // Variable to store information of the acquired image
                GetParam getParam = new GetParam();

                //------------------------------------------------------------
                // Step1. 連線
                //------------------------------------------------------------
                int errCode = KeyenceLJXAAcq.OpenDevice(deviceId, ethernetConfig, HighSpeedPortNo);
                if (errCode != (int)Rc.Ok)
                {
                    Console.WriteLine(@"Failed to open device.");
                }

                //------------------------------------------------------------
                // Step2. Acquire image
                //------------------------------------------------------------

                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();
                errCode = KeyenceLJXAAcq.Acquire(deviceId, HeightImage, LuminanceImage, setParam, ref getParam);
                if (errCode != (int)Rc.Ok)
                {
                    Console.WriteLine(@"Failed to acquire image.");

                }
                //stopwatch.Stop();
                //TimeSpan time = stopwatch.Elapsed;
                //Console.WriteLine(time.TotalMilliseconds);


                //------------------------------------------------------------
                // Step3. Close device
                //------------------------------------------------------------
                KeyenceLJXAAcq.CloseDevice(deviceId);

                for (int y = 0; y < yImageSize; y++)
                {
                    for (int x = 0; x < xImageSize; x++)
                    {
                          src_H.Set(y, x, HeightImage[x + y * 3200]);
                        src_L.Set(y, x, LuminanceImage[x + y * 3200]);
                    }
                }
            }
            
            if (combineImage.Checked)
            {
                //不要執行MainForm畫圖
            }
            else
            {
                pictureBox1.Enabled = true;
                pictureBox2.Enabled = true;
                pictureBox3.Enabled = true;



                int row = src_H.Rows;
                int col = src_H.Cols;

                double min, max;
                Mat maskedImage = new Mat(row, col, MatType.CV_16UC1);


                //取最大值
                Cv2.MinMaxLoc(src_H, out _, out max, out _, out _);

                //利用Threshold, 將沒資料的地方變成65535
                Cv2.Threshold(src_H, maskedImage, 100, 65535, ThresholdTypes.BinaryInv);
                //利用or, 將0與65535疊起來變65535
                Cv2.BitwiseOr(src_H, maskedImage, src_H);

                //把最小值求出來,因為0已經被改成65535了所以不會抓到0
                Cv2.MinMaxLoc(src_H, out min, out _, out _, out _);

                //此時0 = 65535, 最後上色時會變最大值，利用Xor把Mask = 255的地方削掉
                Cv2.BitwiseXor(src_H, maskedImage, src_H);

                //輸出最大最小
                string minMaxNumber = "最大:" + max + ", 最小:" + min;
                label1.Text = minMaxNumber;

                //將最大到最小 壓縮成256階, 上下留一點buffer
                double ratio = 256.0f / (max * 1.1 - min * 1.1);

                //dst = src * alpha + beta
                //如果max=35000,  min = 31000 ,  31000* (256/(35000-31000)) = 1984 , 故還要減掉 31000*0.064 才會變成0
                //所以beta = -min*alpha

                src_H.ConvertTo(src_H8bit, MatType.CV_8UC1, ratio, -min * ratio);
                src_L.ConvertTo(src_L8bit, MatType.CV_8UC1, 256 / 1024.0f);

                Cv2.Resize(src_H8bit, src_H8bit, OpenCvSharp.Size.Zero, 0.1f, 0.1f);
                Cv2.Resize(src_L8bit, src_L8bit, OpenCvSharp.Size.Zero, 0.1f, 0.1f);

                Cv2.ApplyColorMap(src_H8bit, src_H8bit, ColormapTypes.Jet);
                Cv2.ApplyColorMap(src_L8bit, src_L8bit, ColormapTypes.Bone);


                Cv2.AddWeighted(src_H8bit, 0.5, src_L8bit, 0.5, 0, mixPic);

                pictureBox1.Image = BitmapConverter.ToBitmap(src_H8bit);
                pictureBox2.Image = BitmapConverter.ToBitmap(src_L8bit);
                pictureBox3.Image = BitmapConverter.ToBitmap(mixPic);

                pictureBox1.Refresh();
                pictureBox2.Refresh();
                pictureBox3.Refresh();
                callHeight.Enabled = true;
                LoCC.Enabled = true;
                widthMea.Enabled = true;
                spotCount.Enabled = true;
            }
        }

        public static Mat TiltCorrect(in List<OpenCvSharp.Point> points, in Mat src)
        {
            //----------------------------------------
            // Calculate the tilt coefficient(a,b,c)
            // by solving the simultaneous equations.
            // Z = a * X + b * Y + c
            //----------------------------------------
            // Calculate
            double a, b, c;

            double sumX = 0.0;
            double sumY = 0.0;
            double sumZ = 0.0;
            double sumXX = 0.0;
            double sumYY = 0.0;
            double sumXY = 0.0;
            double sumXZ = 0.0;
            double sumYZ = 0.0;
            double sumN = 0.0;

            for (var i = 0; i < points.Count; ++i)
            {
                double z = src.At<ushort>(points[i].Y, points[i].X);
                double x = points[i].X;
                double y = points[i].Y;
                if (z != 0.0)
                {
                    sumX += x;
                    sumY += y;
                    sumZ += z;
                    sumXX += x * x;
                    sumYY += y * y;
                    sumXY += x * y;
                    sumXZ += x * z;
                    sumYZ += y * z;
                    sumN += 1.0;
                }
            }

            using (Mat org = new Mat(3, 3, MatType.CV_64F))
            using (Mat rightV = new Mat(3, 1, MatType.CV_64F))
            {
                org.At<double>(0, 0) = sumXX;
                org.At<double>(0, 1) = sumXY;
                org.At<double>(0, 2) = sumX;

                org.At<double>(1, 0) = sumXY;
                org.At<double>(1, 1) = sumYY;
                org.At<double>(1, 2) = sumY;

                org.At<double>(2, 0) = sumX;
                org.At<double>(2, 1) = sumY;
                org.At<double>(2, 2) = sumN;

                rightV.At<double>(0, 0) = sumXZ;
                rightV.At<double>(1, 0) = sumYZ;
                rightV.At<double>(2, 0) = sumZ;

                // Solve the simultaneous equations by multiplying inverse matrix.
                using (Mat inv = org.Inv(DecompTypes.SVD))
                using (Mat resV = inv * rightV)
                {
                    a = resV.At<double>(0, 0);
                    b = resV.At<double>(1, 0);
                    c = resV.At<double>(2, 0);
                }
            }

            //----------------------------------------
            // Generate the zero plane
            //----------------------------------------
            Mat tiltCorrected = new Mat();
            using (Mat zeroPlane_DBL = Mat.Zeros(src.Size().Height, src.Size().Width, MatType.CV_64FC1))
            {
                var dblArray = new double[src.Size().Height * src.Size().Width];

                int stride = src.Size().Width;
                double a_x = 0.0;
                double b_y = 0.0;
                int x = 0;
                for (var i = 0; i < dblArray.Length; ++i)
                {
                    dblArray[i] = a_x + b_y + c;

                    ++x;
                    a_x += a;
                    if (x == stride)
                    {
                        x = 0;
                        a_x = 0.0;
                        b_y += b;
                    }
                }
                // To speed up pixel data referencing, do not use Mat.At or Mat.Set inside the loop. 
                Marshal.Copy(dblArray, 0, zeroPlane_DBL.Data, dblArray.Length);

                //----------------------------------------
                // Subtract the tilt data.
                // Use mask not to calculate the invalid data.
                //----------------------------------------
                using (Mat nonZeroMask = new Mat())
                using (Mat tiltCorrected_DBL = new Mat())
                using (Mat srcH_DBL = new Mat())
                {
                    // Prepare mask.
                    src.ConvertTo(nonZeroMask, MatType.CV_8U, 1 / 256.0f, 0);

                    // Convert 16-bit data to double data.
                    src.ConvertTo(srcH_DBL, MatType.CV_64FC1, 1, 0);

                    // Subtract the tilt data.
                    // '32768' is added to offset signed data and convert it unsigned data.
                    Cv2.Subtract(srcH_DBL, zeroPlane_DBL, tiltCorrected_DBL, nonZeroMask);
                    Cv2.Add(tiltCorrected_DBL, 32768, tiltCorrected_DBL, nonZeroMask);

                    // Return 16-bit data.
                    tiltCorrected_DBL.ConvertTo(tiltCorrected, MatType.CV_16UC1, 1, 0);
                }
            }

            return tiltCorrected;
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos.Text = $"({e.X*10},{e.Y*10})";
        }

        Mat mixDraw = new Mat();
        Mat newH = new Mat();
        Mat tiltedH = new Mat();
        private List<OpenCvSharp.Point> mouseDownPoints = new List<OpenCvSharp.Point>();

        private List<OpenCvSharp.Point> mouseDownPointsDraw = new List<OpenCvSharp.Point>();
        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mouseDownPoints.Count == 0) 
                { 
                    mixDraw = mixPic.Clone();
                    mouseDownPointsDraw.Add(new OpenCvSharp.Point(e.X, e.Y));
                    mouseDownPoints.Add(new OpenCvSharp.Point(e.X*10, e.Y*10));
                }
                else
                {
                    mouseDownPointsDraw.Add(new OpenCvSharp.Point(e.X, e.Y));
                    mouseDownPoints.Add(new OpenCvSharp.Point(e.X * 10, e.Y * 10));
                }
                Cv2.Circle(mixDraw, mouseDownPointsDraw[mouseDownPointsDraw.Count - 1], 5, Scalar.Red, 5);
                pictureBox3.Image = BitmapConverter.ToBitmap(mixDraw);
                pictureBox3.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                newH = src_H.Clone();                                //Call the "correction" function with recorded points
                tiltedH = TiltCorrect(mouseDownPoints, newH);
                tilted = true;
                mouseDownPoints.Clear();                             // Clear the recorded points
                mouseDownPointsDraw.Clear();                         // Clear the recorded points

                int row = tiltedH.Rows;
                int col = tiltedH.Cols;
                double min, max;
                Mat maskedImage = new Mat(row, col, MatType.CV_16UC1);
                Cv2.MinMaxLoc(tiltedH, out _, out max, out _, out _);//取最大值
                Cv2.Threshold(tiltedH, maskedImage, 100, 65535, ThresholdTypes.BinaryInv);//利用Threshold, 將沒資料的地方變成65535
                Cv2.BitwiseOr(tiltedH, maskedImage, tiltedH);//利用or, 將0與255疊起來變255
                Cv2.MinMaxLoc(tiltedH, out min, out _, out _, out _);//把最小值求出來,因為0已經被改成255了所以不會抓到0
                Cv2.BitwiseXor(tiltedH, maskedImage, tiltedH);//此時0 = 255, 最後上色時會變最大值，利用Xor把Mask = 255的地方削掉
                double ratio = 256.0f / (max * 1.1 - min * 1.1);//將最大到最小 壓縮成256階, 上下留一點 buffer
                string minMaxNumber = "最大:" + max + ", 最小:" + min;
                label1.Text = minMaxNumber;

                Mat tilt_H8bit = new Mat();
                tiltedH.ConvertTo(tilt_H8bit, MatType.CV_8UC1, ratio, -min * ratio);
                Cv2.Resize(tilt_H8bit, tilt_H8bit, OpenCvSharp.Size.Zero, 0.1f, 0.1f);
                Cv2.ApplyColorMap(tilt_H8bit, tilt_H8bit, ColormapTypes.Jet);
                
                pictureBox1.Image = BitmapConverter.ToBitmap(tilt_H8bit);
                pictureBox1.Refresh();
                pictureBox3.Image = BitmapConverter.ToBitmap(mixPic);
                pictureBox3.Refresh();
            }
        }

        private void restart_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Restart();
        }

        private void callHeight_Click(object sender, EventArgs e)
        {
            if (tilted)
            {
                HeightMea heightMea = new HeightMea(tiltedH);
                heightMea.Show();
            }
            else
            {
                HeightMea heightMea = new HeightMea(src_H);
                heightMea.Show();
            }
        }

        private void LoCC_Click(object sender, EventArgs e)
        {
            if (tilted)
            {
                LocCheck locCheck = new LocCheck(tiltedH);
                locCheck.Show();
            }
            else
            {
                LocCheck locCheck = new LocCheck(src_H);
                locCheck.Show();
            }
        }

        private void widthMea_Click(object sender, EventArgs e)
        {
            if (tilted)
            {
                widthMea widthMea = new widthMea(tiltedH);
                widthMea.Show();
            }
            else
            {
                widthMea widthMea = new widthMea(src_H);
                widthMea.Show();
            }
        }

        private async void searchT_Click(object sender, EventArgs e)
        {
            Mat Source = src_L8bit.Clone();
            Mat result = new Mat();
            MessageBox.Show(@"Select search target");
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the initial directory
            //openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.InitialDirectory = @"C: \Users\KW0158W10\Desktop\"; //Path have to be change

            // Set the filter for the type of files to display
            openFileDialog.Filter = "tif Files (*.tif)|*.tif|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                Mat tempImg = new Mat();
                tempImg = Cv2.ImRead(selectedFilePath, ImreadModes.Unchanged);
                Cv2.Resize(tempImg, tempImg, OpenCvSharp.Size.Zero, 0.1f, 0.1f);

                int startX = 75; // X-coordinate of the top-left corner
                int startY = 160;  // Y-coordinate of the top-left corner
                int width = 150;  // Width of the region
                int height = 80; // Height of the region

                // Use slicing to extract the region of interest
                Rect roi = new Rect(startX, startY, width, height);
                Mat regionOfInterest = new Mat(tempImg, roi);

                regionOfInterest.ConvertTo(regionOfInterest, MatType.CV_8UC1, 256 / 1024.0f);
                Cv2.ApplyColorMap(regionOfInterest, regionOfInterest, ColormapTypes.Bone);
                regionOfInterest.ConvertTo(regionOfInterest, MatType.CV_8U);

                Cv2.ImShow("regionOfInterest", regionOfInterest);
                OpenCvSharp.Point minLoc, maxLoc;
                double maxVal,maxMaxVal = 0,maxAngle = 0;
                TMScore.Visible = true;
                Mat rotatedSource = new Mat();
                OpenCvSharp.Scalar okng = new OpenCvSharp.Scalar();
                System.Drawing.Point scoreLoc = new System.Drawing.Point();
                OpenCvSharp.Point bottom_right = new OpenCvSharp.Point();

                if (src_L8bit == null | src_L8bit.Empty() | src_H8bit.Width == 0)
                {
                    button1_Click(null, EventArgs.Empty);
                    Source = src_L8bit.Clone();
                    for (double angle = -5.0; angle < 5.0; angle += 0.1)
                    {
                        rotatedSource = new Mat(MatRotation.GettransforRotationMatrix2D_AM(Source,angle));
                        Cv2.MatchTemplate(rotatedSource, regionOfInterest, result, TemplateMatchModes.CCoeffNormed);
                        Cv2.MinMaxLoc(result, out _, out maxVal, out minLoc, out maxLoc);
                        if (maxVal > 0.5)
                        {
                            okng = Scalar.LightGreen;
                            TMScore.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            okng = Scalar.LightPink;
                            TMScore.BackColor = Color.LightPink;
                        }
                        maxVal = maxVal * 100;
                        scoreLoc = new System.Drawing.Point(maxLoc.X + 377, maxLoc.Y + 12);
                        TMScore.Location = scoreLoc;
                        TMScore.Text = maxVal.ToString("F2") + angle.ToString("F1");
                        TMScore.Update();
                        bottom_right = new OpenCvSharp.Point(maxLoc.X + width, maxLoc.Y + height);
                        Cv2.Rectangle(rotatedSource, maxLoc, bottom_right, okng, 2);
                        pictureBox2.Image = BitmapConverter.ToBitmap(rotatedSource);
                        pictureBox2.Refresh();
                        if (maxVal > maxMaxVal)
                        {
                            maxMaxVal = maxVal;
                            maxAngle = angle;
                        }
                        
                    }
                    
                }
                else
                {
                    for (double angle = -5.0; angle < 5.0; angle += 0.1)
                    {
                        
                        rotatedSource = MatRotation.GettransforRotationMatrix2D_AM(Source, angle);
                        Cv2.MatchTemplate(rotatedSource, regionOfInterest, result, TemplateMatchModes.CCoeffNormed);
                        Cv2.MinMaxLoc(result, out _, out maxVal, out minLoc, out maxLoc);
                        
                        if (maxVal > 0.5)
                        {
                            okng = Scalar.LightGreen;
                            TMScore.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            okng = Scalar.LightPink;
                            TMScore.BackColor = Color.LightPink;
                        }
                        maxVal = maxVal * 100;
                        scoreLoc = new System.Drawing.Point(maxLoc.X + 377, maxLoc.Y + 12);
                        TMScore.Location = scoreLoc;
                        TMScore.Text = maxVal.ToString("F2") + angle.ToString("F1");
                        TMScore.Update();
                        bottom_right = new OpenCvSharp.Point(maxLoc.X + width, maxLoc.Y + height);
                        Cv2.Rectangle(rotatedSource, maxLoc, bottom_right, okng, 2);
                        pictureBox2.Image = BitmapConverter.ToBitmap(rotatedSource);
                        pictureBox2.Refresh();
                        if (maxVal > maxMaxVal)
                        {
                            maxMaxVal = maxVal;
                            maxAngle = angle;
                        }

                    }
                }

                rotatedSource = MatRotation.GettransforRotationMatrix2D_AM(Source, maxAngle);
                Cv2.MatchTemplate(rotatedSource, regionOfInterest, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out _, out maxVal, out minLoc, out maxLoc);
                if (maxVal > 0.5)
                {
                    okng = Scalar.LightGreen;
                    TMScore.BackColor = Color.LightGreen;
                }
                else
                {
                    okng = Scalar.LightPink;
                    TMScore.BackColor = Color.LightPink;
                }
                maxVal = maxVal * 100;
                scoreLoc = new System.Drawing.Point(maxLoc.X + 377, maxLoc.Y + 12);
                TMScore.Location = scoreLoc;
                TMScore.Text = "相似度:"+maxVal.ToString("F2") + "角度: "+maxAngle.ToString("F1");
                TMScore.Update();
                bottom_right = new OpenCvSharp.Point(maxLoc.X + width, maxLoc.Y + height);
                Cv2.Rectangle(rotatedSource, maxLoc, bottom_right, okng, 2);
                pictureBox2.Image = BitmapConverter.ToBitmap(rotatedSource);
                pictureBox2.Refresh();

                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();

            }
        }

        private void spotCount_Click(object sender, EventArgs e)
        {
            if (tilted)
            {
                spotCount spotCount = new spotCount(tiltedH);
                spotCount.Show();
            }
            else
            {
                spotCount spotCount = new spotCount(src_H);
                spotCount.Show();
            }
        }
        
    }
}
