using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore2_With_Auth.Helpers
{
    public static class ImageExtensions
    {
        //=====================================================================================
        public static Image Resize(this Image current, int maxWidth, int maxHeight)
        {
            int width, height;
            //--------------------------------------------------------------------------------
            if (current.Width > current.Height)
            {
                width = maxWidth;
                height = Convert.ToInt32(current.Height*maxHeight/(double)current.Width);
            }
            else
            {
                width = Convert.ToInt32(current.Width * maxWidth / (double)current.Height); 
                height = maxHeight;
            }
            //--------------------------------------------------------------------------------
            var canvas = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(canvas))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(current, 0, 0, width, height);
            }
            //--------------------------------------------------------------------------------
            return canvas;
        }
        //=====================================================================================
        public static byte[] ToByteArray (this Image current)
        {
            using (var stream = new MemoryStream())
            {
                current.Save(stream, current.RawFormat);
                return stream.ToArray();
            }
        }
        //=====================================================================================
    }
}
