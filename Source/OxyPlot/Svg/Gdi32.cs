//-----------------------------------------------------------------------
// <copyright file="Gdi32.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The gdi 32.
    /// </summary>
    public class Gdi32
    {
        #region Public Methods

        /// <summary>
        /// The delete dc.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <returns>
        /// The delete dc.
        /// </returns>
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// The delete object.
        /// </summary>
        /// <param name="hgdiobj">
        /// The hgdiobj.
        /// </param>
        /// <returns>
        /// The delete object.
        /// </returns>
        [DllImport("gdi32.dll")]
        public static extern int DeleteObject(IntPtr hgdiobj);

        /// <summary>
        /// The get dc.
        /// </summary>
        /// <param name="hWnd">
        /// The h wnd.
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// The get text extent point 32.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <param name="len">
        /// The len.
        /// </param>
        /// <param name="siz">
        /// The siz.
        /// </param>
        /// <returns>
        /// The get text extent point 32.
        /// </returns>
        [DllImport("gdi32.dll")]
        public static extern int GetTextExtentPoint32(IntPtr hdc, string str, int len, ref Size siz);

        /// <summary>
        /// The measure string.
        /// </summary>
        /// <param name="faceName">
        /// The face name.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="weight">
        /// The weight.
        /// </param>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// </returns>
        public static OxySize MeasureString(string faceName, int height, int weight, string str)
        {
            IntPtr hfont = CreateFont(height, 0, 0, 0, weight, 0, 0, 0, 0, 0, 0, 0, 0, faceName);
            IntPtr hdc = GetDC(IntPtr.Zero);
            IntPtr oldobj = SelectObject(hdc, hfont);
            OxySize result = GetTextExtent(hdc, str);
            SelectObject(hdc, oldobj);
            DeleteObject(hfont);
            DeleteDC(hdc);
            return result;
        }

        /// <summary>
        /// The select object.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <param name="hgdiObj">
        /// The hgdi obj.
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiObj);

        #endregion

        #region Methods

        /// <summary>
        /// The create font.
        /// </summary>
        /// <param name="nHeight">
        /// The n height.
        /// </param>
        /// <param name="nWidth">
        /// The n width.
        /// </param>
        /// <param name="nEscapement">
        /// The n escapement.
        /// </param>
        /// <param name="nOrientation">
        /// The n orientation.
        /// </param>
        /// <param name="fnWeight">
        /// The fn weight.
        /// </param>
        /// <param name="fdwItalic">
        /// The fdw italic.
        /// </param>
        /// <param name="fdwUnderline">
        /// The fdw underline.
        /// </param>
        /// <param name="fdwStrikeOut">
        /// The fdw strike out.
        /// </param>
        /// <param name="fdwCharSet">
        /// The fdw char set.
        /// </param>
        /// <param name="fdwOutputPrecision">
        /// The fdw output precision.
        /// </param>
        /// <param name="fdwClipPrecision">
        /// The fdw clip precision.
        /// </param>
        /// <param name="fdwQuality">
        /// The fdw quality.
        /// </param>
        /// <param name="fdwPitchAndFamily">
        /// The fdw pitch and family.
        /// </param>
        /// <param name="lpszFace">
        /// The lpsz face.
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateFont(
            int nHeight, 
            int nWidth, 
            int nEscapement, 
            int nOrientation, 
            int fnWeight, 
            uint fdwItalic, 
            uint fdwUnderline, 
            uint fdwStrikeOut, 
            uint fdwCharSet, 
            uint fdwOutputPrecision, 
            uint fdwClipPrecision, 
            uint fdwQuality, 
            uint fdwPitchAndFamily, 
            string lpszFace);

        /// <summary>
        /// The get text extent.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// </returns>
        private static OxySize GetTextExtent(IntPtr hdc, string str)
        {
            Size sz = default(Size);
            sz.cx = 0;
            sz.cy = 0;
            GetTextExtentPoint32(hdc, str, str.Length, ref sz);
            return new OxySize(sz.cx, sz.cy);
        }

        #endregion

        /// <summary>
        /// The size.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            /// <summary>
            /// The cx.
            /// </summary>
            public int cx;

            /// <summary>
            /// The cy.
            /// </summary>
            public int cy;
        }
    }
}
