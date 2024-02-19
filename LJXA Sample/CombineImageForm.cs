using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenCvSharp.Extensions;
using System.Web.UI;

namespace LJXASample
{
    public partial class CombineImageForm : Form
    {
        int deviceId = 0;                   // Set "0" if you use only 1 head.
        int xImageSize = 3200;              // Number of X points.
        int yImageSize = 3000;              // Number of Y lines.
        float yPitchUm = 25.0f;             // Data pitch of Y data. (e.g. your encoder setting)
        int timeoutMs = 30000;              // Timeout value for the acquiring image (in millisecond).
        int useExternalBatchStart = 0;      // Set "1" if you controll the batch start timing externally. (e.g. terminal input)
        Mat dst_luminance = new Mat();
        Mat dst_height = new Mat();


        bool readTIF;
        public CombineImageForm(bool readPic)
        {
            InitializeComponent();
            readTIF = readPic;
        }

        private void GetData_Click(object sender, EventArgs e)
        {
            xImageSize = (int)ximagesizeeach.Value;
            yImageSize = (int)yimagesizeeach.Value;
            List<Mat> imageHeight = new List<Mat>();
            List<Mat> imageLumi = new List<Mat>();
            List<List<ushort>> HeightList = new List<List<ushort>>();
            List<List<ushort>> LumiList = new List<List<ushort>>();
            int RowStart, RowEnd, ColStart, ColEnd;
            int xPicNumber, yPicNumber, imageCount;
            xPicNumber = (int)xPictureNumber.Value;
            yPicNumber = (int)yPictureNumber.Value; ;
            imageCount = xPicNumber* yPicNumber;
            resultCombinedImage.Size =new System.Drawing.Size(xPicNumber* xImageSize/10, yPicNumber* yImageSize/10);

            if (readTIF)
            {   
                for (int i =0; i<imageCount;  i++)
                {
                    // Read File from computer
                    MessageBox.Show(@"Read Lumi Tif file "+ (i+1) +":");
                    OpenFileDialog openFileDialog = new OpenFileDialog();

                    // Set the initial directory
                    //openFileDialog.InitialDirectory = @"C:\";
                    openFileDialog.InitialDirectory = @"C: \Users\KW0158W10\Desktop\"; //Path have to be change
                
                    // Set the filter for the type of files to display
                    openFileDialog.Filter = "tif Files (*.tif)|*.tif|All Files (*.*)|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFilePath = openFileDialog.FileName;
                        //Mat tempImg = new Mat(yImageSize, xImageSize, MatType.CV_16UC1);
                        Mat tempImg = new Mat();
                        tempImg = Cv2.ImRead(selectedFilePath, ImreadModes.Unchanged);
                        imageLumi.Add(tempImg);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else   //read from controller
            {
                LJX8IF_ETHERNET_CONFIG ethernetConfig = new LJX8IF_ETHERNET_CONFIG
                {
                    abyIpAddress = new byte[] { 192, 168, 0, 1 },    // IP address
                    wPortNo = 24691,                                 // Port number
                };
                int HighSpeedPortNo = 24692;         // Port number for high-speed communication

                SetParam setParam = new SetParam
                {
                    YLineNum = yImageSize,
                    YPitchUm = yPitchUm,
                    TimeoutMs = timeoutMs,
                    UseExternalBatchStart = useExternalBatchStart,
                };

                GetParam getParam = new GetParam();
                int errCode = KeyenceLJXAAcq.OpenDevice(deviceId, ethernetConfig, HighSpeedPortNo);
                if (errCode != (int)Rc.Ok)
                {
                    MessageBox.Show(@"Failed to open device.");
                }

                for (int i = 0; i < imageCount; i++)            //Create new List<ushort> and add into a List.
                {
                    List<ushort> temp = new List<ushort>();
                    HeightList.Add(temp);
                    LumiList.Add(temp);
                    MessageBox.Show("Picture "+i, "Picture "+i);
                    errCode = KeyenceLJXAAcq.Acquire(deviceId, HeightList[i], LumiList[i], setParam, ref getParam);
                    if (errCode != (int)Rc.Ok)
                    {
                        MessageBox.Show(@"Failed to acquire picture "+i+"." );
                    }
                }
                KeyenceLJXAAcq.CloseDevice(deviceId);

                for (int i = 0; i < imageCount; i++)
                {
                    Mat blankMat = Mat.Zeros(yImageSize, xImageSize, MatType.CV_16UC1);
                    imageHeight.Add(blankMat);
                    imageLumi.Add(blankMat);
                }
                for (int y = 0; y < yImageSize; y++)
                {
                    for (int x = 0; x < xImageSize; x++)
                    {
                        for (int n = 0; n < imageCount; n++)
                        {
                            imageHeight[n].Set(y, x, HeightList[n][x + y * 3200]);
                            imageLumi[n].Set(y, x, LumiList[n][x + y * 3200]);
                        }
                        
                    }
                }
            }


            //輸出圖片的Mat物件
            //灰度
            
            if (imageLumi.Count == imageCount)
            {
                int X_Cut = (int)XCut.Value;
                dst_luminance = new Mat(yImageSize * yPicNumber, (3200-2*X_Cut) * xPicNumber, MatType.CV_16UC1);

                //copy
                int pictureIndex = 0;
                for (int x=0; x< xPicNumber; x++)
                {
                    for (int y = 0;y < yPicNumber; y++) 
                    {
                        ColStart = x* (xImageSize-2*X_Cut);
                        ColEnd = (x+1)* (xImageSize-2 * X_Cut) -1;
                        RowStart = y*yImageSize;
                        RowEnd = (y+1)*yImageSize-1;
                        //複製圖片
                        imageLumi[pictureIndex].ColRange(0+X_Cut, 3199-X_Cut).RowRange(0, yImageSize-1).CopyTo
                            (dst_luminance.ColRange(ColStart, ColEnd).RowRange(RowStart, RowEnd));
                        showPicture("Picture " + pictureIndex +" ", dst_luminance);
                        pictureIndex +=1;
                    }
                }

                Cv2.Resize(dst_luminance, dst_luminance, OpenCvSharp.Size.Zero, 0.1f, 0.1f);
                dst_luminance.ConvertTo(dst_luminance, MatType.CV_8UC1, 256 / 1024.0f);
                Cv2.ApplyColorMap(dst_luminance, dst_luminance, ColormapTypes.Bone);
                dst_luminance.ConvertTo(dst_luminance, MatType.CV_8U);
                resultCombinedImage.Image = BitmapConverter.ToBitmap(dst_luminance);

                /*
                dst_height = new Mat(yImageSize * yPicNumber, 3200 * xPicNumber, MatType.CV_16UC1);
                imageHeight[0].ColRange(0, 3199).RowRange(0, 2999).CopyTo(dst_height.ColRange(0, 3199).RowRange(0, 2999));
                showPicture("A", dst_height);
                imageHeight[1].ColRange(0, 3199).RowRange(0, 2999).CopyTo(dst_height.ColRange(3200, 6399).RowRange(0, 2999));
                showPicture("B", dst_height);
                imageHeight[2].ColRange(0, 3199).RowRange(0, 2999).CopyTo(dst_height.ColRange(0, 3199).RowRange(3000, 5999));
                showPicture("C", dst_height);
                imageHeight[3].ColRange(0, 3199).RowRange(0, 2999).CopyTo(dst_height.ColRange(3200, 6399).RowRange(3000, 5999));
                showPicture("D", dst_height);

                //img process

                double max, min;
                Mat maskedImage = new Mat(dst_height.Rows, dst_height.Cols, MatType.CV_16UC1);

                Cv2.Resize(dst_height, dst_height, OpenCvSharp.Size.Zero, 0.1f, 0.1f);
                Cv2.MinMaxLoc(dst_height, out _, out max, out _, out _);
                Cv2.Threshold(dst_height, maskedImage, 100, 65535, ThresholdTypes.BinaryInv);
                Cv2.BitwiseOr(dst_height, maskedImage, dst_height);
                Cv2.MinMaxLoc(dst_height, out min, out _, out _, out _);
                Cv2.BitwiseXor(dst_height, maskedImage, dst_height);
                double ratio = 256.0f / (max * 1.1 - min * 1.1);
                dst_height.ConvertTo(dst_height, MatType.CV_8UC1, ratio, -min * ratio);

                Mat mixPic = new Mat();
                */
            }

        }

        private void showPicture(string A,Mat _16UC1)
        {
            Mat temp = _16UC1.Clone();
            temp.ConvertTo(temp, MatType.CV_8UC1, 256 / 1024.0f);
            Cv2.Resize(temp, temp, OpenCvSharp.Size.Zero,0.1f,0.1f);
            Cv2.ApplyColorMap(temp, temp, ColormapTypes.Bone);
            Cv2.ImShow(A, temp);
            Cv2.WaitKey(500);
            Cv2.DestroyAllWindows();
        }

        private void search_Click(object sender, EventArgs e)
        {
            Mat Source = dst_luminance.Clone();
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

                int startX = 50; // X-coordinate of the top-left corner
                int startY = 25;  // Y-coordinate of the top-left corner
                int width = 150;  // Width of the region
                int height = 100; // Height of the region

                // Use slicing to extract the region of interest
                Rect roi = new Rect(startX, startY, width, height);
                Mat regionOfInterest = new Mat(tempImg, roi);

                /*
                //ROTATE TEST
                //旋轉中心
                Point2f center = new Point2f(regionOfInterest.Width / 2.0f, regionOfInterest.Height / 2.0f);
                //旋轉角度
                double angle = 45.0;
                //旋轉矩陣
                Mat rotationMatrix = Cv2.GetRotationMatrix2D(center, angle, scale: 1.0);
                Mat rotatedImage = new Mat();
                Cv2.WarpAffine(regionOfInterest, rotatedImage, rotationMatrix, regionOfInterest.Size());
                // Display the rotated image
                Cv2.ImShow("Rotated Image", rotatedImage);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
                */

                regionOfInterest.ConvertTo(regionOfInterest, MatType.CV_8UC1, 256 / 1024.0f);
                Cv2.ApplyColorMap(regionOfInterest, regionOfInterest, ColormapTypes.Bone);
                regionOfInterest.ConvertTo(regionOfInterest, MatType.CV_8U);

                Cv2.ImShow("regionOfInterest", regionOfInterest);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();

                Cv2.MatchTemplate(Source, regionOfInterest, result, TemplateMatchModes.CCoeffNormed);
                Point minLoc, maxLoc;
                double minVal, maxVal;
                Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);
                Point bottom_right = new Point(maxLoc.X+ width, maxLoc.Y + height);
                Cv2.Rectangle(Source, maxLoc, bottom_right, Scalar.Green, 2);
                resultCombinedImage.Image = BitmapConverter.ToBitmap(Source);

            }
        }
    }
}
