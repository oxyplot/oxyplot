// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        #region Public Methods

        /// <summary>
        /// The repeat.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="n">
        /// The n.
        /// </param>
        /// <returns>
        /// The repeat.
        /// </returns>
        public static string Repeat(this string source, int n)
        {
            var sb = new StringBuilder(n * source.Length);
            for (int i = 0; i < n; i++)
            {
                sb.Append(source);
            }

            return sb.ToString();
        }

        /// <summary>
        /// The split lines.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="lineLength">
        /// The line length.
        /// </param>
        /// <returns>
        /// </returns>
        public static string[] SplitLines(this string s, int lineLength = 80)
        {
            var lines = new List<string>();

            int i = 0;
            while (i < s.Length)
            {
                int len = FindLineLength(s, i, lineLength);
                lines.Add(len == 0 ? s.Substring(i).Trim() : s.Substring(i, len).Trim());
                i += len;
                if (len == 0)
                {
                    break;
                }
            }

            return lines.ToArray();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The find line length.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="i">
        /// The i.
        /// </param>
        /// <param name="maxLineLength">
        /// The max line length.
        /// </param>
        /// <returns>
        /// The find line length.
        /// </returns>
        private static int FindLineLength(string text, int i, int maxLineLength)
        {
            int i2 = i + 1;
            int len = 0;
            while (i2 < i + maxLineLength && i2 < text.Length)
            {
                i2 = text.IndexOfAny(" \n\r".ToCharArray(), i2 + 1);
                if (i2 == -1)
                {
                    i2 = text.Length;
                }

                if (i2 - i < maxLineLength)
                {
                    len = i2 - i;
                }
            }

            return len;
        }

        #endregion
    }
}