// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathRenderingExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to render mathematical expressions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides functionality to render mathematical expressions.
    /// </summary>
    public static class MathRenderingExtensions
    {
        /// <summary>
        /// Initializes static members of the <see cref = "MathRenderingExtensions" /> class.
        /// </summary>
        static MathRenderingExtensions()
        {
            SubAlignment = 0.6;
            SubSize = 0.62;
            SuperAlignment = 0;
            SuperSize = 0.62;
        }

        /// <summary>
        /// Gets or sets the subscript alignment.
        /// </summary>
        private static double SubAlignment { get; set; }

        /// <summary>
        /// Gets or sets the subscript size.
        /// </summary>
        private static double SubSize { get; set; }

        /// <summary>
        /// Gets or sets the superscript alignment.
        /// </summary>
        private static double SuperAlignment { get; set; }

        /// <summary>
        /// Gets or sets the superscript size.
        /// </summary>
        private static double SuperSize { get; set; }

        /// <summary>
        /// Draws or measures text containing sub- and superscript.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="pt">The point.</param>
        /// <param name="text">The text.</param>
        /// <param name="textColor">Color of the text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="ha">The horizontal alignment.</param>
        /// <param name="va">The vertical alignment.</param>
        /// <param name="maxsize">The maximum size of the text.</param>
        /// <param name="measure">Measure the size of the text if set to <c>true</c>.</param>
        /// <returns>The size of the text.</returns>
        /// <example>Subscript: H_{2}O
        /// Superscript: E=mc^{2}
        /// Both: A^{2}_{i,j}</example>
        public static OxySize DrawMathText(
            this IRenderContext rc,
            ScreenPoint pt,
            string text,
            OxyColor textColor,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double angle,
            HorizontalAlignment ha,
            VerticalAlignment va,
            OxySize? maxsize,
            bool measure)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            if (text.Contains("^{") || text.Contains("_{"))
            {
                double x = pt.X;
                double y = pt.Y;

                // Measure
                var size = InternalDrawMathText(rc, x, y, text, textColor, fontFamily, fontSize, fontWeight, true, angle);

                switch (ha)
                {
                    case HorizontalAlignment.Right:
                        x -= size.Width;
                        break;
                    case HorizontalAlignment.Center:
                        x -= size.Width * 0.5;
                        break;
                }

                switch (va)
                {
                    case VerticalAlignment.Bottom:
                        y -= size.Height;
                        break;
                    case VerticalAlignment.Middle:
                        y -= size.Height * 0.5;
                        break;
                }

                InternalDrawMathText(rc, x, y, text, textColor, fontFamily, fontSize, fontWeight, false, angle);
                return measure ? size : OxySize.Empty;
            }

            rc.DrawText(pt, text, textColor, fontFamily, fontSize, fontWeight, angle, ha, va, maxsize);
            if (measure)
            {
                return rc.MeasureText(text, fontFamily, fontSize, fontWeight);
            }

            return OxySize.Empty;
        }

        /// <summary>
        /// Draws text containing sub- and superscript.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="pt">The point.</param>
        /// <param name="text">The text.</param>
        /// <param name="textColor">Color of the text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="ha">The horizontal alignment.</param>
        /// <param name="va">The vertical alignment.</param>
        /// <param name="maxsize">The maximum size of the text.</param>
        /// <example>Subscript: H_{2}O
        /// Superscript: E=mc^{2}
        /// Both: A^{2}_{i,j}</example>
        public static void DrawMathText(
            this IRenderContext rc,
            ScreenPoint pt,
            string text,
            OxyColor textColor,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double angle,
            HorizontalAlignment ha,
            VerticalAlignment va,
            OxySize? maxsize = null)
        {
            DrawMathText(rc, pt, text, textColor, fontFamily, fontSize, fontWeight, angle, ha, va, maxsize, false);
        }

        /// <summary>
        /// The measure math text.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The size of the text.</returns>
        public static OxySize MeasureMathText(
            this IRenderContext rc, string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text.Contains("^{") || text.Contains("_{"))
            {
                return InternalDrawMathText(rc, 0, 0, text, OxyColors.Black, fontFamily, fontSize, fontWeight, true, 0.0);
            }

            return rc.MeasureText(text, fontFamily, fontSize, fontWeight);
        }

        /// <summary>
        /// The internal draw math text.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="s">The s.</param>
        /// <param name="textColor">The text color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="measureOnly">The measure only.</param>
        /// <param name="angle">The angle of the text (degrees).</param>
        /// <returns>The size of the text.</returns>
        private static OxySize InternalDrawMathText(
            IRenderContext rc,
            double x,
            double y,
            string s,
            OxyColor textColor,
            string fontFamily,
            double fontSize,
            double fontWeight,
            bool measureOnly,
            double angle)
        {
            int i = 0;
            double angleRadian = (angle * Math.PI) / 180.0;
            double cosAngle = Math.Round(Math.Cos(angleRadian), 5);
            double sinAngle = Math.Round(Math.Sin(angleRadian), 5);

            double currentX = x, maximumX = x, minimumX = x;
            double currentY = y, maximumY = y, minimumY = y;

            // http://en.wikipedia.org/wiki/Subscript_and_superscript
            double superScriptXDisplacement = sinAngle * fontSize * SuperAlignment;
            double superScriptYDisplacement = cosAngle * fontSize * SuperAlignment;

            double subscriptXDisplacement = sinAngle * fontSize * SubAlignment;
            double subscriptYDisplacement = cosAngle * fontSize * SubAlignment;

            double superscriptFontSize = fontSize * SuperSize;
            double subscriptFontSize = fontSize * SubSize;

            Func<double, double, string, double, OxySize> drawText = (xb, yb, text, fSize) =>
                {
                    if (!measureOnly)
                    {
                        rc.DrawText(new ScreenPoint(xb, yb), text, textColor, fontFamily, fSize, fontWeight, angle);
                    }

                    var flatSize = rc.MeasureText(text, fontFamily, fSize, fontWeight);
                    double width = Math.Abs((flatSize.Width * cosAngle) + (flatSize.Height * sinAngle));
                    double height = Math.Abs((flatSize.Width * sinAngle) + (flatSize.Height * cosAngle));
                    return new OxySize(width, height);
                };

            while (i < s.Length)
            {
                // Superscript
                if (i + 1 < s.Length && s[i] == '^' && s[i + 1] == '{')
                {
                    int i1 = s.IndexOf('}', i);
                    if (i1 != -1)
                    {
                        string supString = s.Substring(i + 2, i1 - i - 2);
                        i = i1 + 1;
                        double sx = currentX + superScriptXDisplacement;
                        double sy = currentY + superScriptYDisplacement;
                        var size = drawText(sx, sy, supString, superscriptFontSize);
                        if (currentX + size.Width > maximumX)
                        {
                            maximumX = currentX + size.Width;
                        }

                        if (currentX + size.Width < minimumX)
                        {
                            minimumX = currentX + size.Width;
                        }

                        if (currentY + size.Height > maximumY)
                        {
                            maximumY = currentY + size.Height;
                        }

                        if (currentY + size.Height < minimumY)
                        {
                            minimumY = currentY + size.Height;
                        }

                        continue;
                    }
                }

                // Subscript
                if (i + 1 < s.Length && s[i] == '_' && s[i + 1] == '{')
                {
                    int i1 = s.IndexOf('}', i);
                    if (i1 != -1)
                    {
                        string subString = s.Substring(i + 2, i1 - i - 2);
                        i = i1 + 1;
                        double sx = currentX - subscriptXDisplacement;
                        double sy = currentY + subscriptYDisplacement;
                        var size = drawText(sx, sy, subString, subscriptFontSize);
                        if (currentX + (size.Width * cosAngle) > maximumX)
                        {
                            maximumX = currentX + (size.Width * cosAngle);
                        }

                        if (currentX + (size.Width * cosAngle) < minimumX)
                        {
                            minimumX = currentX + (size.Width * cosAngle);
                        }

                        if (currentY + (size.Height * sinAngle) > maximumY)
                        {
                            maximumY = currentY + (size.Height * sinAngle);
                        }

                        if (currentY + (size.Height * sinAngle) < minimumY)
                        {
                            minimumY = currentY + (size.Height * sinAngle);
                        }

                        continue;
                    }
                }

                // Regular text
                int i2 = s.IndexOfAny("^_".ToCharArray(), i);
                string regularString;
                if (i2 == -1)
                {
                    regularString = s.Substring(i);
                    i = s.Length;
                }
                else
                {
                    regularString = s.Substring(i, i2 - i);
                    i = i2;
                }

                currentX = maximumX + (2 * cosAngle);
                currentY = maximumY + (2 * sinAngle);
                var size2 = drawText(currentX, currentY, regularString, fontSize);

                currentX += (size2.Width + 2) * cosAngle;
                currentY += (size2.Height + 2) * sinAngle;

                maximumX = Math.Max(currentX, maximumX);
                maximumY = Math.Max(currentY, maximumY);
                minimumX = Math.Min(currentX, minimumX);
                minimumY = Math.Min(currentY, minimumY);
            }

            return new OxySize(maximumX - minimumX, maximumY - minimumY);
        }
    }
}