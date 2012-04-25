// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides support for type methods not found in Metro (?).
    /// </summary>
    public static class TypeHelper
    {
        #region Public Methods

        /// <summary>
        /// Gets the short name of the type.
        /// </summary>
        /// <remarks>
        /// This method is added since there is no Type.Name property in the .NET 4.5 Metro style framework
        /// </remarks>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The get name.
        /// </returns>
        public static string GetTypeName(Type type)
        {
#if METRO
            int idx = type.FullName.LastIndexOf('.');
            return type.FullName.Substring(idx + 1);
#else
            return type.Name;
#endif
        }

        #endregion
    }
}