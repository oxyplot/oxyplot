// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Provides reflection based support methods.
    /// </summary>
    public static class ReflectionHelper
    {
        #region Public Methods

        /// <summary>
        /// Fills a list by the specified property of a source list/enumerable.
        /// </summary>
        /// <param name="source">
        /// The source list.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="list">
        /// The list to be filled.
        /// </param>
        /// <typeparam name="T">
        /// The type of the destination list items (and the source property).
        /// </typeparam>
        public static void FillList<T>(IEnumerable source, string propertyName, IList<T> list)
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
                        throw new InvalidOperationException(
                            string.Format("Could not find field {0} on type {1}", propertyName, t));
                    }
                }

                var v = pi.GetValue(o, null);
                var value = (T)Convert.ChangeType(v, typeof(T), CultureInfo.InvariantCulture);
                list.Add(value);
            }
        }

        #endregion
    }
}