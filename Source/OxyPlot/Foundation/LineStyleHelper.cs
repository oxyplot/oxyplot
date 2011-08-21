// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineStyleHelper.cs" company="OxyPlot">
//   See http://oxyplot.codeplex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// LineStyle helper class.
    /// </summary>
    public static class LineStyleHelper
    {
        #region Public Methods

        /// <summary>
        /// Gets the stroke dash collection for a given <see cref="LineStyle"/>.
        /// </summary>
        /// <param name="style">
        /// The line style.
        /// </param>
        /// <returns>
        /// A dash array.
        /// </returns>
        public static double[] GetDashArray(LineStyle style)
        {
            switch (style)
            {
                case LineStyle.Solid:
                    return null;
                case LineStyle.Dash:
                    return new double[] { 4, 1 };
                case LineStyle.Dot:
                    return new double[] { 1, 1 };
                case LineStyle.DashDot:
                    return new double[] { 4, 1, 1, 1 };
                case LineStyle.DashDashDot:
                    return new double[] { 4, 1, 4, 1, 1, 1 };
                case LineStyle.DashDotDot:
                    return new double[] { 4, 1, 1, 1, 1, 1 };
                case LineStyle.DashDashDotDot:
                    return new double[] { 4, 1, 4, 1, 1, 1, 1, 1 };
                case LineStyle.LongDash:
                    return new double[] { 10, 1 };
                case LineStyle.LongDashDot:
                    return new double[] { 10, 1, 1, 1 };
                case LineStyle.LongDashDotDot:
                    return new double[] { 10, 1, 1, 1, 1, 1 };
                default:
                    return null;
            }
        }

        #endregion
    }
}