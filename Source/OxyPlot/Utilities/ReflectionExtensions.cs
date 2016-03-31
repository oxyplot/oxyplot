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
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Reflection;

    using OxyPlot.Axes;

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
        /// Fills a target by the specified property of a source target/enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the destination target items (and the source property).</typeparam>
        /// <param name="target">The target list to be filled.</param>
        /// <param name="source">The source target.</param>
        /// <param name="propertyName">The property name.</param>
        /// <exception cref="System.InvalidOperationException">Could not find property.</exception>
        public static void AddRange<T>(this List<T> target, IEnumerable source, string propertyName)
        {
            var pi = new ReflectionPath(propertyName);
            foreach (var o in source)
            {
                var v = pi.GetValue(o);
                var value = (T)Convert.ChangeType(v, typeof(T), CultureInfo.InvariantCulture);
                target.Add(value);
            }
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

        /// <summary>
        /// Adds data points from the specified source to the specified destination.
        /// </summary>
        /// <param name="target">The destination target.</param>
        /// <param name="itemsSource">The source.</param>
        /// <param name="dataFieldX">The x-coordinate data field.</param>
        /// <param name="dataFieldY">The y-coordinate data field.</param>
        public static void AddRange(this List<DataPoint> target, IEnumerable itemsSource, string dataFieldX, string dataFieldY)
        {
            var pix = new ReflectionPath(dataFieldX);
            var piy = new ReflectionPath(dataFieldY);
            foreach (var o in itemsSource)
            {
                target.Add(new DataPoint(Axis.ToDouble(pix.GetValue(o)), Axis.ToDouble(piy.GetValue(o))));
            }
        }
    }
}