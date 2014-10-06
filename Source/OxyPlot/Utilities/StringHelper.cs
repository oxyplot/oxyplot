// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringHelper.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extended string formatting functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides extended string formatting functionality.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// The formatting expression.
        /// </summary>
        private static readonly Regex FormattingExpression = new Regex("{(?<Property>.+?)(?<Format>\\:.*?)?}");

        /// <summary>
        /// Replaces the format items in the specified string.
        /// </summary>
        /// <param name="provider">The culture specific format provider.</param>
        /// <param name="formatString">The format string.</param>
        /// <param name="item">The item.</param>
        /// <param name="values">The values.</param>
        /// <returns>The formatted string.</returns>
        /// <remarks>The format string and values works as in <c>String.Format</c>.
        /// In addition, you can format properties of the item object by using the syntax
        /// <c>{PropertyName:Formatstring}</c>.
        /// E.g. if you have a "Value" property in your item's class, use <c>"{Value:0.00}"</c> to output the value with two digits.
        /// Note that this formatting is using reflection and does not have the same performance as string.Format.</remarks>
        public static string Format(IFormatProvider provider, string formatString, object item, params object[] values)
        {
            // Replace items on the format {Property[:Formatstring]}
            var s = FormattingExpression.Replace(
                formatString,
                delegate(Match match)
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
    }
}