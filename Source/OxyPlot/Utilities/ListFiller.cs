// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListFiller.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to fill a list by specified properties of another list.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Provides functionality to fill a list by specified properties of another list.
    /// </summary>
    /// <typeparam name="T">The target list item type.</typeparam>
    /// <remarks>This class uses reflection.</remarks>
    public class ListFiller<T>
        where T : class, new()
    {
        /// <summary>
        /// The properties.
        /// </summary>
        private readonly Dictionary<string, Action<T, object>> properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListFiller{T}" /> class.
        /// </summary>
        public ListFiller()
        {
            this.properties = new Dictionary<string, Action<T, object>>();
        }

        /// <summary>
        /// Adds a setter for the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="setter">The setter.</param>
        public void Add(string propertyName, Action<T, object> setter)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            this.properties.Add(propertyName, setter);
        }

        /// <summary>
        /// Fills the specified target list.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public void FillT(IList<T> target, IEnumerable source)
        {
            this.Fill((IList)target, source);
        }

        /// <summary>
        /// Fills the specified target list.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source list.</param>
        public void Fill(IList target, IEnumerable source)
        {
            PropertyInfo[] pi = null;
            Type t = null;
            foreach (var sourceItem in source)
            {
                if (pi == null || sourceItem.GetType() != t)
                {
                    t = sourceItem.GetType();
                    pi = new PropertyInfo[this.properties.Count];
                    int i = 0;
                    foreach (var p in this.properties)
                    {
                        if (string.IsNullOrEmpty(p.Key))
                        {
                            i++;
                            continue;
                        }

                        pi[i] = t.GetProperty(p.Key);
                        if (pi[i] == null)
                        {
                            throw new InvalidOperationException(
                                string.Format("Could not find field {0} on type {1}", p.Key, t));
                        }

                        i++;
                    }
                }

                var item = new T();

                int j = 0;
                foreach (var p in this.properties)
                {
                    if (pi[j] != null)
                    {
                        var value = pi[j].GetValue(sourceItem, null);
                        p.Value(item, value);
                    }

                    j++;
                }

                target.Add(item);
            }
        }
    }
}