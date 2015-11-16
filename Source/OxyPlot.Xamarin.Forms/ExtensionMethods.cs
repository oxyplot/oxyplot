// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods related to OxyPlot and Xamarin.Forms.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xamarin.Forms
{
    /// <summary>
    /// Provides extension methods related to OxyPlot and Xamarin.Forms.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts from <see cref="T:OxyColor"/> to <see cref="T:Color"/>.
        /// </summary>
        /// <param name="c">The color to convert.</param>
        /// <returns>The converted color.</returns>
        public static global::Xamarin.Forms.Color ToXamarinForms(this OxyColor c)
        {
            return global::Xamarin.Forms.Color.FromRgba(c.R, c.G, c.B, c.A);
        }

        /// <summary>
        /// Converts from <see cref="T:Color"/> to <see cref="T:OxyColor"/>.
        /// </summary>
        /// <param name="c">The color to convert.</param>
        /// <returns>The converted color.</returns>
        public static OxyColor ToOxyColor(this global::Xamarin.Forms.Color c)
        {
            return OxyColor.FromArgb((byte)(c.A * 255), (byte)(c.R * 255), (byte)(c.G * 255), (byte)(c.B * 255));
        }
    }
}
