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
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
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
                delegate (Match match)
                {
                    var property = match.Groups["Property"].Value;
                    if (property.Length > 0 && char.IsDigit(property[0]))
                    {
                        return match.Value;
                    }

                    var pi = item.GetType().GetRuntimeProperty(property);
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

        /// <summary>
        /// Creates a valid format string on the form "{0:###}".
        /// </summary>
        /// <param name="input">The input format string.</param>
        /// <returns>The corrected format string.</returns>
        public static string CreateValidFormatString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "{0}";
            }

            if (input.Contains("{"))
            {
                return input;
            }

            return string.Concat("{0:", input, "}");
        }

        /// <summary>
        /// Formats each item in a sequence by the specified format string and property.
        /// </summary>
        /// <param name="source">The source target.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="formatString">The format string. The format argument {0} can be used for the value of the property in each element of the sequence.</param>
        /// <param name="provider">The format provider.</param>
        /// <exception cref="System.InvalidOperationException">Could not find property.</exception>
        public static IEnumerable<string> Format(this IEnumerable source, string propertyName, string formatString, IFormatProvider provider)
        {
            var fs = CreateValidFormatString(formatString);
            if (string.IsNullOrEmpty(propertyName))
            {
                foreach (var element in source)
                {
                    yield return string.Format(provider, fs, element);
                }
            }
            else
            {
                var reflectionPath = new ReflectionPath(propertyName);
                foreach (var element in source)
                {
                    var value = reflectionPath.GetValue(element);
                    yield return string.Format(provider, fs, value);
                }
            }
        }

        /// <summary>
        /// Splits the given text into separate lines.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>An array of the individual lines.</returns>
        public static string[] SplitLines(string text)
        {
            return Regex.Split(text, "\r?\n");
        }
    }
}
