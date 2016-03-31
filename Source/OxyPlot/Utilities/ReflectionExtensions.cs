// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides utility methods reflection based support methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Provides extension methods based on reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
#if !UNIVERSAL        
        /// <summary>
        /// Retrieves an object that represents a specified property.
        /// </summary>
        /// <param name="type">The type that contains the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>An object that represents the specified property, or null if the property is not found.</returns>
        public static PropertyInfo GetRuntimeProperty(this Type type, string name)
        {
            return type.GetProperty(name);
        }
#endif

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
            var fs = StringHelper.CreateValidFormatString(formatString);
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
    }
}