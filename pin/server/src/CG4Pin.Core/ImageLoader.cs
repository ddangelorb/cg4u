using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace CG4Pin.Core
{
    /// <summary>
    ///     Provides a method to load the image in the specified location and returns a copy with 24 bits per pixel.
    /// </summary>
    public static class ImageLoader
    {
        /// <summary>
        ///     Load the image from the specified location and returns a copy with 24 bits per pixel.
        /// </summary>
        /// <param name="fileName">
        ///     The location of the image to load.
        /// </param>
        /// <returns>
        ///     A copy of the loaded image with 24 bits per pixel.
        /// </returns>
        public static Bitmap LoadImage(string fileName)
        {
            Bitmap returnBitmap;
            Bitmap srcBitmap = new Bitmap(fileName);
            using (srcBitmap)
            {
                PixelFormat pixelFormat;
                switch (srcBitmap.PixelFormat)
                {
                    case PixelFormat.Format8bppIndexed:
                    case PixelFormat.Indexed:
                    case PixelFormat.Format4bppIndexed:
                    case PixelFormat.Format1bppIndexed:
                        pixelFormat = PixelFormat.Format24bppRgb;
                        break;
                    default:
                        pixelFormat = srcBitmap.PixelFormat;
                        break;
                }
                returnBitmap = new Bitmap(srcBitmap.Width, srcBitmap.Height, pixelFormat);
                returnBitmap.SetResolution(srcBitmap.HorizontalResolution, srcBitmap.VerticalResolution);
                Graphics g = Graphics.FromImage(returnBitmap);
                g.DrawImage(srcBitmap, 0, 0);
            }
            return returnBitmap;
        }

        //https://blogs.msdn.microsoft.com/dotnet/2017/01/19/net-core-image-processing/
        public static Bitmap LoadImage2(string inputPath)
        {
            int size = 150;
            int quality = 75;

            using (var image = new Bitmap(System.Drawing.Image.FromFile(inputPath)))
            {
                int width, height;
                if (image.Width > image.Height)
                {
                    width = size;
                    height = Convert.ToInt32(image.Height * size / (double)image.Width);
                }
                else
                {
                    width = Convert.ToInt32(image.Width * size / (double)image.Height);
                    height = size;
                }
                var resized = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(resized))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, width, height);
                }

                return resized;
            }            
        }
    }
}
