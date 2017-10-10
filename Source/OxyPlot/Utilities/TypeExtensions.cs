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
            return type.GetTypeInfo().DeclaredProperties.FirstOrDefault(pi => pi.Name == name);
        }
    }
}