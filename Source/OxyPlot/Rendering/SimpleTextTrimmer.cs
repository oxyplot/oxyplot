// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleTextTrimmer.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   A simple trimmer that doesn't use glyph information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A simple trimmer that doesn't use glyph information.
    /// </summary>
    public class SimpleTextTrimmer : ITextTrimmer
    {
        /// <summary>
        /// The default ellipsis, comprising three ascii stop symbols.
        /// </summary>
        public static readonly string AsciiEllipsis = "...";

        /// <summary>
        /// The unicode horizontal ellipsis.
        /// </summary>
        public static readonly string UnicodeEllipsis = "…";

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTextTrimmer" /> class.
        /// </summary>
        public SimpleTextTrimmer()
        {
            this.TrimToWord = false;
            this.Ellipsis = UnicodeEllipsis;
            this.AppendEllipsis = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether text should be trimmed to word boundaries.
        /// </summary>
        public bool TrimToWord { get; set; }

        /// <summary>
        /// Gets or sets a value to append to any trimmed text when <see cref="AppendEllipsis"/> is <c>true</c>.
        /// </summary>
        public string Ellipsis { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an ellipsis should be appended to any trimmed text.
        /// </summary>
        public bool AppendEllipsis { get; set; }

        /// <summary>
        /// Gets a list of indexes that correspond to the first <char>c</char> after a character in the single line of text given.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The list of character boundaries.</returns>
        public static IList<int> GetCharacterBoundaries(string text)
        {
            var res = new List<int>();

            // handle surrogate pairs explicitly (U+200D is zero-width-joiner for emoji)
            var matches = Regex.Matches(text, @"([\uD800-\uDBFF}][\uDC00-\uDFFF](\u200D[\uD800-\uDBFF}][\uDC00-\uDFFF])*|[^\s])\p{M}*", RegexOptions.Singleline);
            for (int i = 0; i < matches.Count; i++)
            {
                var m = matches[i];
                res.Add(m.Index + m.Length);
            }

            return res;
        }

        /// <summary>
        /// Gets a list of indexes that correspond to the first <c>char</c> after a word in the single line of text given.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The list of word boundaries.</returns>
        public static IList<int> GetWordBoundaries(string text)
        {
            var res = new List<int>();

            var matches = Regex.Matches(text, @"[^\s]+", RegexOptions.Singleline);
            for (int i = 0; i < matches.Count; i++)
            {
                var m = matches[i];
                res.Add(m.Index + m.Length);
            }

            return res;
        }

        /// <summary>
        /// Trims the given text at the given boundaries.
        /// </summary>
        /// <param name="textMeasurer">The <see cref="ITextMeasurer"/> to use.</param>
        /// <param name="line">The line of text to trim.</param>
        /// <param name="width">The width in which the text must fix.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size in 1/96ths of an inch.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="ellipsis">The ellipsis, if any, to append to the end of the text.</param>
        /// <param name="boundaries">The word boundaries at which to trim.</param>
        /// <returns>The trimmed line of text.</returns>
        public static string TrimAtBoundaries(ITextMeasurer textMeasurer, string line, double width, string fontFamily, double fontSize, double fontWeight, string ellipsis, IList<int> boundaries)
        {
            if (BoundaryCheck(textMeasurer, line, width, fontFamily, fontSize, fontWeight, ellipsis, out var boundaryResult))
            {
                return boundaryResult;
            }

            ellipsis = ellipsis ?? string.Empty;

            int s = -1;
            int e = boundaries.Count - 1;

            while (e > s)
            {
                var m = s + ((e - s + 1) / 2);
                var lineWidth = textMeasurer.MeasureTextWidth(line.Substring(0, boundaries[m]) + ellipsis, fontFamily, fontSize, fontWeight);

                if (lineWidth > width)
                {
                    e = m - 1;
                }
                else
                {
                    s = m;
                }
            }

            if (s == -1)
            {
                // TODO: need to think about this condition?
                return ellipsis;
            }
            else
            {
                return line.Substring(0, boundaries[s]) + ellipsis;
            }
        }

        /// <inheritdoc/>
        public string Trim(ITextMeasurer textMeasurer, string line, double width, string fontFamily, double fontSize, double fontWeight)
        {
            var ellipsis = this.AppendEllipsis ? this.Ellipsis : null;

            if (this.TrimToWord)
            {
                var boundaries = GetWordBoundaries(line);
                return TrimAtBoundaries(textMeasurer, line, width, fontFamily, fontSize, fontWeight, ellipsis, boundaries);
            }
            else
            {
                var boundaries = GetCharacterBoundaries(line);
                return TrimAtBoundaries(textMeasurer, line, width, fontFamily, fontSize, fontWeight, ellipsis, boundaries);
            }
        }

        /// <summary>
        /// Performs common boundary condition checks. Returns true if the <paramref name="boundaryResult"/> should be returned immediately in lieu of other trimming.
        /// </summary>
        /// <param name="textMeasurer">The <see cref="ITextMeasurer"/> to use.</param>
        /// <param name="line">The line of text to trim.</param>
        /// <param name="width">The width in which the text must fix.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size in 1/96ths of an inch.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="ellipsis">The ellipsis, if any, to append to the end of the text.</param>
        /// <param name="boundaryResult">The resulting text if a boundary condition was observed, otherwise <c>null</c>.</param>
        /// <returns><c>true</c> if a boundary condition was met, otherwise <c>false</c>.</returns>
        private static bool BoundaryCheck(ITextMeasurer textMeasurer, string line, double width, string fontFamily, double fontSize, double fontWeight, string ellipsis, out string boundaryResult)
        {
            if (textMeasurer == null)
            {
                throw new ArgumentNullException(nameof(textMeasurer));
            }

            if (line == null)
            {
                throw new ArgumentNullException(nameof(line));
            }

            if (width <= 0)
            {
                // nothing will fit
                boundaryResult = string.Empty;
                return true;
            }

            var lineWidth = textMeasurer.MeasureTextWidth(line, fontFamily, fontSize, fontWeight);
            if (lineWidth <= width)
            {
                // do nothing
                boundaryResult = line;
                return true;
            }

            if (ellipsis != null)
            {
                var ellipsisWidth = textMeasurer.MeasureTextWidth(ellipsis, fontFamily, fontSize, fontWeight);
                if (width < ellipsisWidth)
                {
                    // ellipsis won't fit
                    boundaryResult = string.Empty;
                    return true;
                }
            }

            boundaryResult = null;
            return false;
        }
    }
}
