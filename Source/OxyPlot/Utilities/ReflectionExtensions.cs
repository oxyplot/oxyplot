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
    using System.Reflection;

    using OxyPlot.Axes;

    /// <summary>
    /// Provides extension methods based on reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
#if UNIVERSAL
        /// <summary>
        /// Gets a specific property of the current Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The string containing the name of the public property to get.</param>
        /// <returns>An object representing the public property with the specified name, if found; otherwise, null.</returns>
        public static PropertyInfo GetProperty(this Type type, string name)
        {
            return type.GetRuntimeProperty(name);
        }

        /// <summary>
        /// Returns the public set accessor for this property.
        /// </summary>
        /// <param name="pi">The property.</param>
        /// <returns>The MethodInfo object representing the Set method for this property, or null if the set accessor is not public.</returns>
        public static MethodInfo GetSetMethod(this PropertyInfo pi)
        {
            return pi.SetMethod;
        }

        /// <summary>
        /// Returns all the public properties of the current Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>An array of PropertyInfo objects representing all public properties of the current Type.</returns>
        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            return type.GetRuntimeProperties().Where(pi => pi.GetMethod.IsPublic);
        }

        /// <summary>
        /// Returns all the public fields of the current Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>An array of FieldInfo objects representing all the public fields defined for the current Type.</returns>
        public static IEnumerable<FieldInfo> GetFields(this Type type)
        {
            return type.GetRuntimeFields().Where(fi => fi.IsPublic);
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
        /// Fills a formatted string collection by the specified property of a source enumerable.
        /// </summary>
        /// <param name="target">The target list to be filled.</param>
        /// <param name="source">The source target.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="formatString">The format string.</param>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.InvalidOperationException">Could not find property.</exception>
        public static void AddFormattedRange(this List<string> target, IEnumerable source, string propertyName, string formatString, IFormatProvider provider)
        {
            var pi = new ReflectionPath(propertyName);
            var fs = "{0:" + formatString + "}";
            foreach (var o in source)
            {
                var v = pi.GetValue(o);
                var value = string.Format(provider, fs, v);
                target.Add(value);
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