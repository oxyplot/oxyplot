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
    using System.Reflection;

    using OxyPlot.Axes;

    /// <summary>
    /// Provides extension methods based on reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
#if UNIVERSAL
        public static PropertyInfo GetProperty(this Type type, string name)
        {
            return type.GetRuntimeProperty(name);
        }
        public static MethodInfo GetSetMethod(this PropertyInfo pi)
        {
            return pi.SetMethod;
        }
        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            return type.GetRuntimeProperties();
        }
        public static IEnumerable<FieldInfo> GetFields(this Type type)
        {
            return type.GetRuntimeFields();
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