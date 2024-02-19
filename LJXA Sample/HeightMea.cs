using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace LJXASample
{
    public partial class HeightMea : Form
    {
        private Mat heightData;
        double min, max, ratio;
        bool add = false;
        DataTable HTable = new DataTable();



    public HeightMea(Mat height)
        {
            InitializeComponent();
            InitializeDataBinding();
            overlayDrawBox.BackColor = Color.Transparent;
            overlayDrawBox.Paint += overlayDrawBox_Paint;
            heightData = height;
            int row = heightData.Rows;
            int col = heightData.Cols;
            if (HTable.Columns.Count == 0)
            {
                HTable.Columns.Add($"Area", typeof(string));
                HTable.Columns.Add($"Max", typeof(string));
                HTable.Columns.Add($"Min", typeof(string));
                HTable.Columns.Add($"Average", typeof(string));
            }

            Mat maskedImage = new Mat(row, col, MatType.CV_16UC1);
            Cv2.MinMaxLoc(heightData, out _, out max, out _, out _);//取最大值
            Cv2.Threshold(heightData, maskedImage, 100, 65535, ThresholdTypes.BinaryInv);//利用Threshold, 將沒資料的地方變成65535
            Cv2.BitwiseOr(heightData, maskedImage, heightData);//利用or, 將0與255疊起來變255
            Cv2.MinMaxLoc(heightData, out min, out _, out _, out _);//把最小值求出來,因為0已經被改成255了所以不會抓到0
            Cv2.BitwiseXor(heightData, maskedImage, heightData);//此時0 = 255, 最後上色時會變最大值，利用Xor把Mask = 255的地方削掉
            ratio = 256.0f / (max * 1.1 - min * 1.1);//將最大到最小 壓縮成256階, 上下留一點 buffer

            Mat height_H8bit = new Mat();
            heightData.ConvertTo(height_H8bit, MatType.CV_8UC1, ratio, -min * ratio);
            Cv2.Resize(height_H8bit, height_H8bit, OpenCvSharp.Size.Zero, 0.1f, 0.1f);
            Cv2.ApplyColorMap(height_H8bit, height_H8bit, ColormapTypes.Jet);

            overlayDrawBox.Image = BitmapConverter.ToBitmap(height_H8bit);
            headSet.Items.Add("8020");
            headSet.Items.Add("8060");
            headSet.Items.Add("8080");
            headSet.Items.Add("8200");
        }



        private System.Drawing.Point startPoint;
        private System.Drawing.Point endPoint;
        private bool drawing = false;
        private void overlayDrawBox_MouseDown(object sender, MouseEventArgs e)
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

        private void overlayDrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
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
                overlayDrawBox.Invalidate(); // Redraw the overlay
            }
            int x = e.X * 10;
            int y = e.Y * 10;
            int z = 0;

            if (x < heightData.Cols & y < heightData.Rows & x > 0 & y > 0)
            {
                z = heightData.At<ushort>(y, x);
            }

            XYZ.Text = $"({x},{y},{z})";
        }

        private void overlayDrawBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;

                Rect roi = new Rect(startPoint.X * 10, startPoint.Y * 10, (endPoint.X - startPoint.X) * 10, (endPoint.Y - startPoint.Y) * 10);

                Mat calArea = heightData.Clone(roi);
                double minH, maxH, avgH;
                Mat maskedImage2 = new Mat(calArea.Rows, calArea.Cols, MatType.CV_16UC1);
                Cv2.MinMaxLoc(calArea, out _, out maxH, out _, out _);//取最大最小值
                Cv2.Threshold(calArea, maskedImage2, 100, 65535, ThresholdTypes.BinaryInv);//利用Threshold, 將沒資料的地方變成65535
                Cv2.BitwiseOr(calArea, maskedImage2, calArea);//利用or, 將0與255疊起來變255
                Cv2.MinMaxLoc(calArea, out minH, out _, out _, out _);//把最小值求出來,因為0已經被改成255了所以不會抓到0
                                                                      //計算平均
                                                                      // Initialize variables for sum and count
                double sum = 0;
                int count = 0;

                // Iterate through the pixels in the ROI
                for (int y = 0; y < calArea.Rows; y++)
                {
                    for (int x = 0; x < calArea.Cols; x++)
                    {
                        double pixelValue = calArea.At<ushort>(y, x); // Assuming the ROI is 16-bit

                        // Check if the pixel is not zero
                        if (pixelValue != 0 & pixelValue != 65535)
                        {
                            sum += pixelValue;
                            count++;
                        }
                    }
                }

                // Calculate the mean value
                avgH =sum / count;


                ushort zUnit = 0;
                int lastRowIndex = dataGridView1.Rows.Count - 1;
                int errCode = NativeMethods.LJX8IF_GetZUnitSimpleArray(0, ref zUnit);
                
                DataRow row = HTable.NewRow();
                row[0] = dataGridView1.Rows.Count - 1;
                if (errCode == 0)
                {
                    row[1] = $"{(maxH - 32768) * zUnit / 100 / 1000} mm";
                    row[2] = $"{(minH - 32768) * zUnit / 100 / 1000} mm";
                    row[3] = $"{Math.Round(avgH-32768 * zUnit / 100 / 1000,2)} mm";
                }
                else
                {
                    if (headSet.SelectedItem != null)
                    {
                        string selectedValue = headSet.SelectedItem.ToString();

                        switch (selectedValue)
                        {
                            case "8020":
                                row[1] = $"{(maxH - 32768) * 0.4 / 1000} mm";
                                row[2] = $"{(minH - 32768) * 0.4 / 1000} mm";
                                row[3] = $"{Math.Round((avgH - 32768) * 0.4 / 1000,2)} mm";
                                break;

                            case "8060":
                                row[1] = $"{(maxH - 32768) * 0.8 / 1000} mm";
                                row[2] = $"{(minH - 32768) * 0.8 / 1000} mm";
                                row[3] = $"{Math.Round((avgH - 32768) * 0.8 / 1000, 2)} mm";
                                break;

                            case "8080":
                                row[1] = $"{(maxH - 32768) * 1.6 / 1000} mm";
                                row[2] = $"{(minH - 32768) * 1.6 / 1000} mm";
                                row[3] = $"{Math.Round((avgH - 32768) * 1.6 / 1000, 2)} mm";
                                break;

                            case "8200":
                                row[1] = $"{(maxH - 32768) * 4 / 1000} mm";
                                row[2] = $"{(minH - 32768) * 4 / 1000} mm";
                                row[3] = $"{Math.Round((avgH - 32768) * 4 / 1000, 2)} mm";
                                break;

                            default:
                                row[1] = $"{(maxH - 32768)} * Z unit";
                                row[2] = $"{(minH - 32768)} * Z unit";
                                row[3] = $"{Math.Round(avgH - 32768, 2)}    * Z unit ";
                                break;
                        }
                    }
                    else
                    {
                        row[1] = $"{(maxH - 32768)} * Z unit";
                        row[2] = $"{(minH - 32768)} * Z unit";
                        row[3] = $"{Math.Round(avgH - 32768, 2)}    * Z unit ";
                    }

                }
                
                HTable.Rows.Add(row);
                add = false;

                Mat calArea_H8bit = new Mat();
                calArea.ConvertTo(calArea_H8bit, MatType.CV_8UC1, ratio, -min * ratio);
                Cv2.Resize(calArea_H8bit, calArea_H8bit, OpenCvSharp.Size.Zero, 0.1f, 0.1f);
                Cv2.ApplyColorMap(calArea_H8bit, calArea_H8bit, ColormapTypes.Jet);

                Cv2.ImShow("cut", calArea_H8bit);
                Cv2.MoveWindow("cut", 500, 500);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
                

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            add = true;
        }

        private void overlayDrawBox_Paint(object sender, PaintEventArgs e)
        {
            if (drawing)
            {
                // Draw a rectangle on the overlayPictureBox
                using (Pen pen = new Pen(Color.Black, 3))
                {
                    Rectangle rect = new Rectangle(startPoint, new System.Drawing.Size(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y));
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }


       

        private void InitializeDataBinding()
        {
            // Bind the DataTable to the DataGridView
            dataGridView1.DataSource = HTable;
        }
        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
            HTable.Clear();
        }


    }
}
