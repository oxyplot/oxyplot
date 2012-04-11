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
    /// Reflection methods.
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

        /// <summary>
        /// Fills a list by from source items and specified properties.
        /// </summary>
        /// <typeparam name="T">The type of the destination items.</typeparam>
        /// <param name="source">The source items.</param>
        /// <param name="target">The target list.</param>
        /// <param name="propertyNames">The property names.</param>
        /// <param name="setPropertyActions">The set property actions.</param>
        public static void FillList<T>(IEnumerable source, IList<T> target, string[] propertyNames, params Action<T, object>[] setPropertyActions) where T : new()
        {
            PropertyInfo[] pi = null;
            Type t = null;
            foreach (var sourceItem in source)
            {
                if (pi == null || sourceItem.GetType() != t)
                {
                    t = sourceItem.GetType();
                    pi = new PropertyInfo[propertyNames.Length];
                    for (int i = 0; i < propertyNames.Length; i++)
                    {
                        if (string.IsNullOrEmpty(propertyNames[i]))
                        {
                            continue;
                        }

                        pi[i] = t.GetProperty(propertyNames[i]);
                        if (pi[i] == null)
                        {
                            throw new InvalidOperationException(
                                string.Format("Could not find field {0} on type {1}", propertyNames[i], t));
                        }
                    }
                }

                var item = new T();
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (pi[i] != null)
                    {
                        var value = pi[i].GetValue(sourceItem, null);
                        setPropertyActions[i](item, value);
                    }
                }

                target.Add(item);
            }
        }
        #endregion
    }
}