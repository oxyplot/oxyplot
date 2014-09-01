// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BitmapTools.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WpfExamples
{
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public static class BitmapTools
    {
        public static Bitmap Resize(Bitmap bitmap, int newWidth, int newHeight)
        {
            var resizedBitmap = new Bitmap(newWidth, newHeight);
            var g = Graphics.FromImage(resizedBitmap);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(bitmap, 0, 0, newWidth, newHeight);
            g.Dispose();
            return resizedBitmap;
        }
    }
}