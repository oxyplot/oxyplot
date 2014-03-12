// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableDocumentFontFamily.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
//   Represents a font family that can be used in a <see cref="PortableDocument" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents a font family that can be used in a <see cref="PortableDocument"/>.
    /// </summary>
    public class PortableDocumentFontFamily
    {
        /// <summary>
        /// Gets or sets the regular font.
        /// </summary>
        public PortableDocumentFont RegularFont { get; set; }

        /// <summary>
        /// Gets or sets the bold font.
        /// </summary>
        public PortableDocumentFont BoldFont { get; set; }

        /// <summary>
        /// Gets or sets the italic font.
        /// </summary>
        public PortableDocumentFont ItalicFont { get; set; }

        /// <summary>
        /// Gets or sets the bold and italic font.
        /// </summary>
        public PortableDocumentFont BoldItalicFont { get; set; }

        /// <summary>
        /// Gets the font with the specified weight and style.
        /// </summary>
        /// <param name="bold">bold font weight.</param>
        /// <param name="italic">italic/oblique font style.</param>
        /// <returns>The font.</returns>
        public PortableDocumentFont GetFont(bool bold, bool italic)
        {
            if (bold && italic && this.BoldItalicFont != null)
            {
                return this.BoldItalicFont;
            }

            if (bold && this.BoldFont != null)
            {
                return this.BoldFont;
            }

            if (italic && this.ItalicFont != null)
            {
                return this.ItalicFont;
            }

            return this.RegularFont;
        }
    }
}