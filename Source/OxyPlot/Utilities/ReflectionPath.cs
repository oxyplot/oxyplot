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
            this.items = path != null ? path.Split('.') : new string[0];
            this.infos = new PropertyInfo[this.items.Length];
            this.reflectedTypes = new Type[this.items.Length];
        }

        /// <summary>
        /// Gets the value for the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The value.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Could not find property.</exception>
        public object GetValue(object instance)
        {
            object result;
            if (this.TryGetValue(instance, out result))
            {
                return result;
            }

            throw new InvalidOperationException("Could not find property " + string.Join(".", this.items) + " in " + instance);
        }

        /// <summary>
        /// Tries to get the value for the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="result">The result.</param>
        /// <returns>
        /// <c>true</c> if the value was found.
        /// </returns>
        public bool TryGetValue(object instance, out object result)
        {
            var current = instance;
            for (int i = 0; i < this.items.Length; i++)
            {
                if (current == null)
                {
                    result = null;
                    return true;
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
                    result = null;
                    return false;
                }

                current = pi.GetValue(current, null);
            }

            result = current;
            return true;
        }
    }
}
