using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace GTolk.Util
{
    public static class Imagens
    {
        public static bool IsImage(string fileName)
        {
            try
            {
                using (var bitmap = new System.Drawing.Bitmap(fileName))
                {
                    return !bitmap.Size.IsEmpty;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsRecognisedImageFile(string fileName)
        {
            string targetExtension = System.IO.Path.GetExtension(fileName);
            if (String.IsNullOrEmpty(targetExtension))
            {
                return false;
            }
            else
            {
                targetExtension = "*" + targetExtension.ToLowerInvariant();
            }

            List<string> recognisedImageExtensions = new List<string>();

            foreach (System.Drawing.Imaging.ImageCodecInfo imageCodec in System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders())
            {
                recognisedImageExtensions.AddRange(imageCodec.FilenameExtension.ToLowerInvariant().Split(";".ToCharArray()));
            }

            //bool targetFound = false;
            foreach (string extension in recognisedImageExtensions)
            {
                if (extension.Equals(targetExtension))
                {
                    return true;
                }
            }

            return false;
        }

        public static Image CropToCircle(Image image, int circleDiameter, bool centered = true)
        {
            Bitmap bm = (Bitmap)image;
            Bitmap bt = new Bitmap(bm.Width, bm.Height);
            Graphics g = Graphics.FromImage(bt);
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            //gp.AddEllipse(10, 10, bm.Width - 20, bm.Height - 20);
            if (!centered)
            {
                gp.AddEllipse(0, 0, circleDiameter, circleDiameter);
            }
            else
            {
                float deslocamentoHorizontal = ((float)(circleDiameter - bm.Width)) / 2 * -1;
                float deslocamentoVertical = ((float)(circleDiameter - bm.Height)) / 2 * -1;
                gp.AddEllipse(deslocamentoHorizontal, deslocamentoVertical, circleDiameter, circleDiameter);
            }

            g.Clear(Color.Magenta);
            g.SetClip(gp);
            g.DrawImage(bm, new
            Rectangle(0, 0, bm.Width, bm.Height), 0, 0, bm.Width, bm.Height, GraphicsUnit.Pixel);
            g.Dispose();
            bt.MakeTransparent(Color.Magenta);

            return bt;
        }

        public static Image CropToCircle(Image image, float circleDiameter, float horizontalCenter, float verticalCenter)
        {
            Bitmap bm = (Bitmap)image;
            Bitmap bt = new Bitmap(bm.Width, bm.Height);
            Graphics g = Graphics.FromImage(bt);
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            //gp.AddEllipse(10, 10, bm.Width - 20, bm.Height - 20);

            //float deslocamentoHorizontal = ((float)(circleDiameter - bm.Width)) / 2 * -1;
            //float deslocamentoVertical = ((float)(circleDiameter - bm.Height)) / 2 * -1;
            gp.AddEllipse(horizontalCenter, verticalCenter, circleDiameter, circleDiameter);


            g.Clear(Color.Magenta);
            g.SetClip(gp);
            g.DrawImage(bm, new
            Rectangle(0, 0, bm.Width, bm.Height), 0, 0, bm.Width, bm.Height, GraphicsUnit.Pixel);
            g.Dispose();
            bt.MakeTransparent(Color.Magenta);

            return bt;
        }

        public static Image CropToRetangle(Image image, int pointLeft, int pointTop, int width, int height)
        {
            Bitmap bm = (Bitmap)image;
            Bitmap bt = new Bitmap(bm.Width, bm.Height);
            Graphics g = Graphics.FromImage(bt);
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            //gp.AddEllipse(10, 10, bm.Width - 20, bm.Height - 20);

            //float deslocamentoHorizontal = ((float)(circleDiameter - bm.Width)) / 2 * -1;
            //float deslocamentoVertical = ((float)(circleDiameter - bm.Height)) / 2 * -1;
            //gp.AddEllipse(horizontalCenter, verticalCenter, circleDiameter, circleDiameter);
            gp.AddRectangle(new Rectangle(pointLeft, pointTop, width, height));

            g.Clear(Color.Magenta);
            g.SetClip(gp);
            g.DrawImage(bm, new
            Rectangle(0, 0, bm.Width, bm.Height), 0, 0, bm.Width, bm.Height, GraphicsUnit.Pixel);
            g.Dispose();
            bt.MakeTransparent(Color.Magenta);

            return bt;
        }



        public static Image CutRectangle(Image source, int leftPoint, int upperPoint, int width, int height)
        {
            Rectangle rect = new Rectangle(leftPoint, upperPoint, width, height);
            return CutRectangle(source, rect);
        }

        //public static Bitmap CropImage(Bitmap source, Rectangle section)
        public static Image CutRectangle(Image source, Rectangle section)
        {

            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }


        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        public static Image ScaleImageMinimum(Image image, int minSizeOf)
        {
            int minimumSize = (image.Width < image.Height) ? image.Width : image.Height;

            var ratioX = (double)minSizeOf / minimumSize;
            //var ratioY = (double)minSizeOf / minimumSize;
            var ratio = Math.Min(ratioX, ratioX);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length == 0)
            {
                return null;
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}