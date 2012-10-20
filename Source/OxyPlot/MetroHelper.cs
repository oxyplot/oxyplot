// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetroHelper.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides extension methods not available in Metro.
// </summary>
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