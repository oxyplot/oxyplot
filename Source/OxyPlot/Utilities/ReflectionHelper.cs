// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
    /// Provides utility methods reflection based support methods.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Fills a target by the specified property of a source target/enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the destination target items (and the source property).</typeparam>
        /// <param name="target">The target list to be filled.</param>
        /// <param name="source">The source target.</param>
        /// <param name="propertyName">The property name.</param>
        /// <exception cref="System.InvalidOperationException">Could not find property.</exception>
        public static void FillList<T>(List<T> target, IEnumerable source, string propertyName)
        {
            PropertyInfo pi = null;
            Type t = null;
            foreach (var o in source)
            {
                if (pi == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pi = t.GetProperty(propertyName);
                    if (pi == null)
                    {
                        throw new InvalidOperationException(string.Format("Could not find property {0} on type {1}", propertyName, t));
                    }
                }

                var v = pi.GetValue(o, null);
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
        public static void FillList(List<DataPoint> target, IEnumerable itemsSource, string dataFieldX, string dataFieldY)
        {
            PropertyInfo pix = null;
            PropertyInfo piy = null;
            Type t = null;

            foreach (var o in itemsSource)
            {
                if (pix == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pix = t.GetProperty(dataFieldX);
                    piy = t.GetProperty(dataFieldY);
                    if (pix == null)
                    {
                        throw new InvalidOperationException(
                            string.Format("Could not find data field {0} on type {1}", dataFieldX, t));
                    }

                    if (piy == null)
                    {
                        throw new InvalidOperationException(
                            string.Format("Could not find data field {0} on type {1}", dataFieldY, t));
                    }
                }

                double x = Axis.ToDouble(pix.GetValue(o, null));
                double y = Axis.ToDouble(piy.GetValue(o, null));

                var pp = new DataPoint(x, y);
                target.Add(pp);
            }
        }
    }
}