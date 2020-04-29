// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableDocumentExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides OxyPlot extension methods for <see cref="PortableDocument" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides OxyPlot extension methods for <see cref="PortableDocument" />.
    /// </summary>
    public static class PortableDocumentExtensions
    {
        /// <summary>
        /// Sets the stroke color.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="c">The color.</param>
        public static void SetColor(this PortableDocument doc, OxyColor c)
        {
            doc.SetColor(c.R / 255.0, c.G / 255.0, c.B / 255.0);
            doc.SetStrokeAlpha(c.A / 255.0);
        }

        /// <summary>
        /// Sets the fill color.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="c">The color.</param>
        public static void SetFillColor(this PortableDocument doc, OxyColor c)
        {
            doc.SetFillColor(c.R / 255.0, c.G / 255.0, c.B / 255.0);
            doc.SetFillAlpha(c.A / 255.0);
        }
    }
}