// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableDocumentExtensions.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides OxyPlot extension methods for <see cref="PortableDocument"/>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides OxyPlot extension methods for <see cref="PortableDocument"/>.
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