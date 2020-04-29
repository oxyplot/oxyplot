// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListBuilder{T}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to build a list by reflecting specified properties on a sequence.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides functionality to build a list by reflecting specified properties on a sequence.
    /// </summary>
    /// <typeparam name="T">The target list item type.</typeparam>
    /// <remarks>This class uses reflection.</remarks>
    public class ListBuilder<T>
    {
        /// <summary>
        /// The properties.
        /// </summary>
        private readonly List<string> properties;

        /// <summary>
        /// The default values
        /// </summary>
        private readonly List<object> defaultValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBuilder{T}" /> class.
        /// </summary>
        public ListBuilder()
        {
            this.properties = new List<string>();
            this.defaultValues = new List<object>();
        }

        /// <summary>
        /// Adds a property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="defaultValue">The default value.</param>
        public void Add<TProperty>(string propertyName, TProperty defaultValue)
        {
            this.properties.Add(propertyName);
            this.defaultValues.Add(defaultValue);
        }

        /// <summary>
        /// Fills the specified target list.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <param name="instanceCreator">The instance creator.</param>
        public void FillT(IList<T> target, IEnumerable source, Func<IList<object>, T> instanceCreator)
        {
            this.Fill((IList)target, source, args => instanceCreator(args));
        }

        /// <summary>
        /// Fills the specified target list.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source list.</param>
        /// <param name="instanceCreator">The instance creator.</param>
        public void Fill(IList target, IEnumerable source, Func<IList<object>, object> instanceCreator)
        {
            ReflectionPath[] pi = null;
            Type t = null;
            foreach (var sourceItem in source)
            {
                if (pi == null || sourceItem.GetType() != t)
                {
                    t = sourceItem.GetType();
                    pi = this.properties.Select(p => p != null ? new ReflectionPath(p) : null).ToArray();
                }

                var args = new List<object>(pi.Length);
                for (int j = 0; j < pi.Length; j++)
                {
                    object value;
                    args.Add(pi[j] != null && pi[j].TryGetValue(sourceItem, out value) ? value : this.defaultValues[j]);
                }

                var item = instanceCreator(args);
                target.Add(item);
            }
        }
    }
}