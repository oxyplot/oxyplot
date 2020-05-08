// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableDocumentFont.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a font that can be used in a <see cref="PortableDocument" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents a font that can be used in a <see cref="PortableDocument" />.
    /// </summary>
    public class PortableDocumentFont
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PortableDocumentFont" /> class.
        /// </summary>
        public PortableDocumentFont()
        {
            this.FirstChar = 0;
            this.Encoding = FontEncoding.WinAnsiEncoding;
        }

        /// <summary>
        /// Gets or sets the font subtype.
        /// </summary>
        public FontSubType SubType { get; set; }

        /// <summary>
        /// Gets or sets the base font.
        /// </summary>
        public string BaseFont { get; set; }

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        public FontEncoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the first character in the Widths array.
        /// </summary>
        public int FirstChar { get; set; }

        /// <summary>
        /// Gets or sets the character Widths array.
        /// </summary>
        public int[] Widths { get; set; }

        /// <summary>
        /// Gets or sets the font ascent.
        /// </summary>
        public int Ascent { get; set; }

        /// <summary>
        /// Gets or sets the font cap height.
        /// </summary>
        public int CapHeight { get; set; }

        /// <summary>
        /// Gets or sets the font descent.
        /// </summary>
        public int Descent { get; set; }

        /// <summary>
        /// Gets or sets the font flags.
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// Gets or sets the font bounding box.
        /// </summary>
        public int[] FontBoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the italic angle.
        /// </summary>
        public int ItalicAngle { get; set; }

        /// <summary>
        /// Gets or sets the stem v.
        /// </summary>
        public int StemV { get; set; }

        /// <summary>
        /// Gets or sets the x height.
        /// </summary>
        public int XHeight { get; set; }

        /// <summary>
        /// Gets or sets the font name.
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Measures the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontSize">The font size</param>
        /// <param name="width">The width of the text.</param>
        /// <param name="height">The height of the text.</param>
        public void Measure(string text, double fontSize, out double width, out double height)
        {
            int wmax = 0;

            var lines = StringHelper.SplitLines(text);

            int lineCount = lines.Length;

            foreach (string line in lines)
            {
                int w = 0;

                for (int i = 0; i < line.Length; i++)
                {
                    var c = line[i];
                    if (c >= this.FirstChar + this.Widths.Length)
                    {
                        continue;
                    }

                    w += this.Widths[c - this.FirstChar];
                }

                if (w > wmax)
                {
                    wmax = w;
                }
            }

            width = wmax * fontSize / 1000;
            height = lineCount * (this.Ascent - this.Descent) * fontSize / 1000;
        }
    }
}
