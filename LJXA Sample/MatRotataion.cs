using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LJXASample
{
    internal class MatRotation
    {
        public static Mat GettransforRotationMatrix2D_AM(Mat Img_In, double Rotation_Angle = 0.0)
        {
            int center_x = Img_In.Width / 2;
            int center_y = Img_In.Height / 2;
            OpenCvSharp.Point center = new OpenCvSharp.Point(center_x, center_y);
            Mat warp_rotate_dst = new Mat();


            double Translation_x = 0.0;
            double Translation_y = 0.0;
            double Scale = 1.0;


            Mat M = Mat.Zeros(2, 3, MatType.CV_64FC1);

            M.At<double>(0, 0) = (double)(Scale * Math.Cos(Rotation_Angle * Math.PI / 180));
            M.At<double>(0, 1) = -(double)(Scale * Math.Sin(Rotation_Angle * Math.PI / 180));
            M.At<double>(1, 0) = (double)(Scale * Math.Sin(Rotation_Angle * Math.PI / 180));
            M.At<double>(1, 1) = (double)(Scale * Math.Cos(Rotation_Angle * Math.PI / 180));

            M.At<double>(0, 2) = (double)((1 - M.At<double>(0, 0)) * center.X - M.At<double>(0, 1) * center.Y + Translation_x);
            M.At<double>(1, 2) = (double)(M.At<double>(0, 1) * center.X + (1 - M.At<double>(0, 0)) * center.Y + Translation_y);

            Cv2.WarpAffine(Img_In, warp_rotate_dst, M, Img_In.Size());
            return warp_rotate_dst;
        }

    }
}
