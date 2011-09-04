//-----------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// The reflection helper.
    /// </summary>
    public static class ReflectionHelper
    {
        #region Public Methods

        /// <summary>
        /// The fill values.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="field">
        /// The field.
        /// </param>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public static void FillValues<T>(IEnumerable source, string field, IList<T> list)
        {
            PropertyInfo pi = null;
            Type t = null;
            foreach (object o in source)
            {
                if (pi == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pi = t.GetProperty(field);
                    if (pi == null)
                    {
                        throw new InvalidOperationException(
                            string.Format("Could not find field {0} on type {1}", field, t));
                    }
                }

                var value = (T)Convert.ChangeType(pi.GetValue(o, null), typeof(T), CultureInfo.InvariantCulture);
                list.Add(value);
            }
        }

        #endregion
    }
}
