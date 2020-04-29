// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableDocumentFontFamily.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a font family that can be used in a <see cref="PortableDocument" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents a font family that can be used in a <see cref="PortableDocument" />.
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