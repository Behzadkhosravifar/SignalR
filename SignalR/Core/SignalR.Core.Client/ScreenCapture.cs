﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SignalR.Core
{
    /// <summary>
    /// Provides functions to capture the entire screen, or a particular window.
    /// </summary>
    public static class ScreenCapture
    {
        public static Rectangle AllScreenSize;

        public static Size ReSizeAspectRatio = new Size(1024, 768); // set to aspect 1024×768

        static ScreenCapture()
        {
            AllScreenSize = Screen.AllScreens.Aggregate(Rectangle.Empty, (current, s) => Rectangle.Union(current, s.Bounds));
        }


        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns>Screen Captured Image</returns>
        public static Image Capture()
        {
            var screenShotBmp = new Bitmap(AllScreenSize.Width, AllScreenSize.Height, PixelFormat.Format32bppArgb);
            using (var screenShotGraphics = Graphics.FromImage(screenShotBmp))
            {
                if (AllScreenSize == Rectangle.Empty || AllScreenSize.Width <= 200 || AllScreenSize.Height <= 200)
                    return null;

                screenShotGraphics.CopyFromScreen(AllScreenSize.X, AllScreenSize.Y,
                    0, 0, AllScreenSize.Size, CopyPixelOperation.SourceCopy);

                return screenShotBmp;
            }
        }

        public static Image FromFile(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Image result = Image.FromStream(fs);
                fs.Close();
                // We MUST call the constructor here, 
                // otherwise the bitmap will still be linked to the original file 
                return new Bitmap(result);
            }
        }

        /// <summary>
        /// Convert Image object to bytes array
        /// </summary>
        /// <param name="img">Image object to convert.</param>
        /// <returns>Converted image in frame of bytes array.</returns>
        public static byte[] ToBytes(this Image img)
        {
            if (img == null) return null;

            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Convert image bytes array to image object.
        /// </summary>
        /// <param name="imgBytes">Image bytes array to convert image.</param>
        /// <returns>Converted Image Object's.</returns>
        public static Image ToImage(this byte[] imgBytes)
        {
            if (imgBytes == null) return null;

            using (var ms = new MemoryStream(imgBytes))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="desWidth">Width of the DES.</param>
        /// <param name="desHeight">Height of the DES.</param>
        /// <returns></returns>
        public static Image ResizeImage(this Image image, int desWidth, int desHeight)
        {
            if (image == null) return null;

            int w, h;

            if (image.Height > image.Width)
            {
                w = (image.Width * desHeight) / image.Height;
                h = desHeight;
            }
            else // Wide Image
            {
                w = desWidth;
                h = (image.Height * desWidth) / image.Width;
            }

            var bmp = new Bitmap(w, h);

            using (var g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.InterpolationMode = InterpolationMode.Low;
                g.DrawImage(image, 0, 0, w, h);
            }

            return bmp;
        }

        public static string ToBase64String(this Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static Image Base64ToImage(this string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }
    }
}
