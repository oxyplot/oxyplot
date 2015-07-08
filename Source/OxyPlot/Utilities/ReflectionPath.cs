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
        /// The reflected types.
        /// </summary>
        private readonly Type[] reflectedTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionPath"/> class.
        /// </summary>
        /// <param name="path">The reflection path.</param>
        public ReflectionPath(string path)
        {
            this.items = path.Split('.');
            this.infos = new PropertyInfo[this.items.Length];
            this.reflectedTypes = new Type[this.items.Length];
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

                var currentType = current.GetType();

                var pi = this.infos[i];
                if (pi == null || this.reflectedTypes[i] != currentType)
                {
                    pi = this.infos[i] = currentType.GetRuntimeProperty(this.items[i]);
                    this.reflectedTypes[i] = currentType;
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