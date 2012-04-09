// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FractionHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// String formatting utilities.
    /// </summary>
    public static class StringHelper
    {
        static Regex formattingExpression = new Regex("{(?<Property>.+?)(?<Format>\\:.*?)?}");

        /// <summary>
        /// Replaces the format items in the specified string.
        /// </summary>
        /// <param name="provider">The culture specific format provider.</param>
        /// <param name="formatString">The format string.</param>
        /// <param name="item">The item.</param>
        /// <param name="values">The values.</param>
        /// <remarks>
        /// The formatString and values works as in string.Format.
        /// In addition, you can format properties of the item object by using the syntax {PropertyName:Formatstring}.
        /// E.g. if you have a "Value" property in your item's class, use "{Value:0.00}" to output the value with two digits.
        /// Note that this formatting is using reflection and does not have the same performance as string.Format.
        /// </remarks>
        /// <returns>The formatted string.</returns>
        public static string Format(IFormatProvider provider, string formatString, object item, params object[] values)
        {
            // Replace items on the format {Property[:Formatstring]}
            var s = formattingExpression.Replace(formatString, delegate(Match match)
            {
                var property = match.Groups["Property"].Value;
                if (property.Length > 0 && char.IsDigit(property[0]))
                {
                    return match.Value;
                }

                var pi = item.GetType().GetProperty(property);
                if (pi == null)
                {
                    return string.Empty;
                }

                var v = pi.GetValue(item, null);
                var format = match.Groups["Format"].Value;

                var fs = "{0" + format + "}";
                return string.Format(provider, fs, v);
            });

            // Also apply the standard formatting
            s = string.Format(provider, s, values);
            return s;
        }

#if !SILVERLIGHT && !METRO
        /// <summary>
        /// Creates a valid file name.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>A file name.</returns>
        public static string CreateValidFileName(string title, string extension)
        {
            string validFileName = title.Trim();
            foreach (char invalChar in Path.GetInvalidFileNameChars())
            {
                validFileName = validFileName.Replace(invalChar.ToString(), "");
            }

            foreach (char invalChar in Path.GetInvalidPathChars())
            {
                validFileName = validFileName.Replace(invalChar.ToString(), "");
            }

            if (validFileName.Length > 160)
            {
                //safe value threshold is 260
                validFileName = validFileName.Remove(156) + "...";
            }

            return validFileName + extension;
        }
#endif
    }
}