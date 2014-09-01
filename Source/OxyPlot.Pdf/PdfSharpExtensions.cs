// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfSharpExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods for OxyPlot to PdfSharp type conversion.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf
{
    using System;

    using PdfSharp.Drawing;

    /// <summary>
    /// Provides extension methods for OxyPlot to PdfSharp type conversion.
    /// </summary>
    public static class PdfSharpExtensions
    {
        /// <summary>
        /// Converts an <see cref="OxyRect" /> to an <see cref="XRect" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <returns>The <see cref="XRect" /></returns>
        public static XRect ToXRect(this OxyRect r)
        {
            return new XRect((int)Math.Round(r.Left), (int)Math.Round(r.Top), (int)Math.Round(r.Width), (int)Math.Round(r.Height));
        }
    }
}