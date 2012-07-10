// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyColorExtensions.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides extension methods for the OxyColor class.
    /// </summary>
    public static class OxyColorExtensions
    {
        /// <summary>
        /// Changes the opacity value.
        /// </summary>
        /// <param name="color">The original color.</param>
        /// <param name="newAlpha">The new alpha.</param>
        /// <returns>The new color.</returns>
        public static OxyColor ChangeAlpha(this OxyColor color, byte newAlpha)
        {
            return OxyColor.FromArgb(newAlpha, color.R, color.G, color.B);
        }
    }
}