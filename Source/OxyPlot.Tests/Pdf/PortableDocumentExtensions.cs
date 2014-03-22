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
//   Provides extension methods for <see cref="PortableDocument"/>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Provides extension methods for <see cref="PortableDocument"/>.
    /// </summary>
    public static class PortableDocumentExtensions
    {
        /// <summary>
        /// Saves the document at the specified path.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="path">The path.</param>
        /// <param name="explore">Open Windows explorer on the specified path if set to <c>true</c>.</param>
        public static void Save(this PortableDocument doc, string path, bool explore = false)
        {
            var directory = Path.GetDirectoryName(path);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = File.Create(path))
            {
                doc.Save(stream);
            }

            if (explore)
            {
                Process.Start("explorer.exe", "/select," + path);
            }
        }

        /// <summary>
        /// Draws a cross at the specified position.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="x">The center x-coordinate.</param>
        /// <param name="y">The center y-coordinate.</param>
        /// <param name="size">The size of the cross.</param>
        public static void DrawCross(this PortableDocument doc, double x, double y, double size = 10)
        {
            doc.MoveTo(x - size, y);
            doc.LineTo(x + size, y);
            doc.MoveTo(x, y - size);
            doc.LineTo(x, y + size);
            doc.Stroke(false);
        }
    }
}