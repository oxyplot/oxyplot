// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionPath.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to reflect a path of properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Provides functionality to reflect a path of properties.
    /// </summary>
    public class ReflectionPath
    {
        /// <summary>
        /// The path items.
        /// </summary>
        private readonly string[] items;

        /// <summary>
        /// The property metadata.
        /// </summary>
        private readonly PropertyInfo[] infos;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionPath"/> class.
        /// </summary>
        /// <param name="path">The reflection path.</param>
        public ReflectionPath(string path)
        {
            this.items = path.Split('.');
            this.infos = new PropertyInfo[this.items.Length];
        }

        /// <summary>
        /// Gets the value for the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The value.</returns>
        /// <exception cref="System.InvalidOperationException">Could not find property.</exception>
        public object GetValue(object instance)
        {
            var current = instance;
            for (int i = 0; i < this.items.Length; i++)
            {
                if (current == null)
                {
                    return null;
                }

                var pi = this.infos[i];
                var currentType = current.GetType();
                if (pi == null || pi.ReflectedType != currentType)
                {
                    pi = this.infos[i] = currentType.GetProperty(this.items[i]);
                }

                if (pi == null)
                {
                    throw new InvalidOperationException("Could not find property " + this.items[i] + " in " + current);
                }

                current = pi.GetValue(current, null);
            }

            return current;
        }
    }
}