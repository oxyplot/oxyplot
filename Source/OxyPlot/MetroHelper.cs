// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetroHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Provides extension methods not available in Metro.
    /// </summary>
    public static class MetroHelper
    {
        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>The property info.</returns>
        public static PropertyInfo GetProperty(this Type type, string name)
        {
            return type.GetTypeInfo().GetDeclaredProperty(name);
        }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>The fields.</returns>
        public static FieldInfo[] GetFields(this Type type, BindingFlags flags) {
            // return type.GetTypeInfo().GetFields();
            return null;
        }

        /// <summary>
        /// Formats the specified date to a short date string.
        /// </summary>
        /// <param name="dt">The date.</param>
        /// <returns>The string.</returns>
        public static string ToShortDateString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }                      
    }

    /// <summary>
    /// Defines binding flags.
    /// </summary>
    public enum BindingFlags
    {
        /// <summary>
        /// Public flag.
        /// </summary>
        Public, 
        
        /// <summary>
        /// Static flag.
        /// </summary>
        Static
    }

}