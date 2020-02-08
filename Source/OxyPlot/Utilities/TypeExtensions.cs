// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods for types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides extension methods for types.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Retrieves an object that represents a specified property.
        /// </summary>
        /// <param name="type">The type that contains the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>An object that represents the specified property, or null if the property is not found.</returns>
        public static PropertyInfo GetRuntimeProperty(this Type type, string name)
        {
#if NET40
            var source = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
#else
            var typeInfo = type.GetTypeInfo();
            var source = typeInfo.AsType().GetRuntimeProperties();
#endif

            foreach (var x in source)
            {
                if (x.Name == name) return x;
            }

            return null;
        }
    }
}
