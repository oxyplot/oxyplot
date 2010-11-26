using System;
using System.Runtime.InteropServices;

namespace OxyPlot
{
    public class Gdi32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int cx;
            public int cy;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiObj);

        [DllImport("gdi32.dll")]
        public static extern int GetTextExtentPoint32(IntPtr hdc, string str, int len, ref SIZE siz);

        [DllImport("gdi32.dll")]
        public static extern int DeleteObject(IntPtr hgdiobj);
     
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateFont(int nHeight, int nWidth, int nEscapement, 
            int nOrientation, int fnWeight, uint fdwItalic, uint fdwUnderline, 
            uint fdwStrikeOut, uint fdwCharSet, uint fdwOutputPrecision, uint fdwClipPrecision,
            uint fdwQuality, uint fdwPitchAndFamily, string lpszFace);

        private static OxySize GetTextExtent(IntPtr hdc, string str)
        {
            Gdi32.SIZE sz = default(Gdi32.SIZE);
            sz.cx = 0;
            sz.cy = 0;
            GetTextExtentPoint32(hdc, str, str.Length, ref sz);
            return new OxySize(sz.cx, sz.cy);
        }

        public static OxySize MeasureString(string faceName, int height, int weight, string str)
        {
            var hfont = CreateFont(height, 0, 0, 0, weight, 0, 0, 0, 0, 0, 0, 0, 0, faceName);
            IntPtr hdc = GetDC(IntPtr.Zero);
            IntPtr oldobj = SelectObject(hdc, hfont);
            var result = GetTextExtent(hdc, str);
            SelectObject(hdc, oldobj);
            DeleteObject(hfont);
            DeleteDC(hdc);
            return result;
        }



    }
}