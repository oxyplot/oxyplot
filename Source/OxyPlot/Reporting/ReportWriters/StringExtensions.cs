// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods for <see cref="string"/> objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Provides extension methods for <see cref="string"/> objects.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Repeats the specified string <paramref name="n" /> times.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="n">The number of times to repeat.</param>
        /// <returns>The repeated string.</returns>
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
        /// Splits the specified string to lines of maximum <paramref name="lineLength" /> length.
        /// </summary>
        /// <param name="s">The string to split.</param>
        /// <param name="lineLength">The line length.</param>
        /// <returns>The split lines.</returns>
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

        /// <summary>
        /// Finds the length of the line starting at <paramref name="i" /> that has maximum length <paramref name="maxLineLength" />.
        /// </summary>
        /// <param name="text">The text source.</param>
        /// <param name="i">The start index.</param>
        /// <param name="maxLineLength">The maximum line length.</param>
        /// <returns>The length of the line.</returns>
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
    }
}