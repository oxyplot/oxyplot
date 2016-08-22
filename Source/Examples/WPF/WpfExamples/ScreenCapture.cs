// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenCapture.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WpfExamples
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public static class ScreenCapture
    {

        public static Bitmap Capture(int left, int top, int width, int height)
        {
            var hDesk = NativeMethods.GetDesktopWindow();
            var hSrce = NativeMethods.GetWindowDC(hDesk);
            var hDest = NativeMethods.CreateCompatibleDC(hSrce);
            var hBmp = NativeMethods.CreateCompatibleBitmap(hSrce, width, height);
            var hOldBmp = NativeMethods.SelectObject(hDest, hBmp);
            NativeMethods.BitBlt(hDest, 0, 0, width, height, hSrce, left, top, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);
            var bmp = Image.FromHbitmap(hBmp);
            NativeMethods.SelectObject(hDest, hOldBmp);
            NativeMethods.DeleteObject(hBmp);
            NativeMethods.DeleteDC(hDest);
            NativeMethods.ReleaseDC(hDesk, hSrce);
            return bmp;
        }

        private static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, CopyPixelOperation rop);
            [DllImport("user32.dll")]
            public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDc);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hDc);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr ptr);
        }
    }
}