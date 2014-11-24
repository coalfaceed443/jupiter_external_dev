using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace CRM.Code.Utils
{
    public static class Imaging
    {
        public static bool ResizeImage(string strPath, int maxWidth, int maxHeight, int quality)
        {
            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(strPath))
                {
                    if (image.Height > maxHeight || image.Width > maxWidth)
                    {

                        int mxW = maxWidth;
                        int mxH = maxHeight;
                        int asW = (image.Width * mxH) / image.Height;
                        int asH = (image.Height * mxW) / image.Width;
                        int newwidth;
                        int newheight;

                        if (asW <= mxW)
                        {
                            newwidth = asW;
                            newheight = mxH;
                        }
                        else
                        {
                            newwidth = mxW;
                            newheight = asH;
                        }

                        Image thumbnail = new Bitmap(newwidth, newheight);

                        using (Graphics graphic = Graphics.FromImage(thumbnail))
                        {

                            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphic.SmoothingMode = SmoothingMode.HighQuality;
                            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphic.CompositingQuality = CompositingQuality.HighQuality;

                            graphic.DrawImage(image, -1, -1, newwidth + 1, newheight + 1);

                            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                            EncoderParameters encoderParameters;

                            encoderParameters = new EncoderParameters(1);
                            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                            image.Dispose();

                            thumbnail.Save(strPath, info[1], encoderParameters);
                            thumbnail.Dispose();
                        }
                    }
                }
            }
            catch (Exception e) { return false; }

            return true;
        }

        public static bool ResizeImage(string strPath, int maxWidth, int maxHeight)
        {
            return ResizeImage(strPath, maxWidth, maxHeight, 95);
        }

        public static bool ResizeImageFixed(string strPath, int fixedWidth, int fixedHeight)
        {
            try
            {
                using (System.Drawing.Image oImg = System.Drawing.Image.FromFile(strPath))
                {
                    int newwidth;
                    int newheight;

                    if (oImg.Height > oImg.Width)
                    {
                        newwidth = fixedWidth;
                        newheight = Convert.ToInt32(Convert.ToDouble(oImg.Height) / (Convert.ToDouble(oImg.Width) / Convert.ToDouble(newwidth)));
                    }
                    else
                    {
                        newheight = fixedHeight;
                        newwidth = Convert.ToInt32(Convert.ToDouble(oImg.Width) / (Convert.ToDouble(oImg.Height) / Convert.ToDouble(newheight)));
                    }

                    System.Drawing.Image oThumbNail = new Bitmap(fixedWidth, fixedHeight, oImg.PixelFormat);

                    using (Graphics oGraphic = Graphics.FromImage(oThumbNail))
                    {
                        oGraphic.CompositingQuality = CompositingQuality.HighQuality;
                        oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                        oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        Rectangle oRectangle = new Rectangle(-1, -1, newwidth + 1, newheight + 1);

                        oGraphic.DrawImage(oImg, oRectangle);

                        oImg.Dispose();

                        oThumbNail.Save(strPath, ImageFormat.Jpeg);

                        oThumbNail.Dispose();
                    }

                }
            }

            catch (Exception e) { return false; }

            return true;
        }
    }
}