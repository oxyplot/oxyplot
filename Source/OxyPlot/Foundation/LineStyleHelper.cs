namespace OxyPlot
{
    /// <summary>
    ///   LineStyle helper class.
    /// </summary>
    public static class LineStyleHelper
    {
        /// <summary>
        ///   Gets the stroke dash collection for a given <see cref = "LineStyle" />.
        /// </summary>
        /// <param name = "style">The line style.</param>
        /// <returns></returns>
        public static double[] GetDashArray(LineStyle style)
        {
            switch (style)
            {
                case LineStyle.Solid:
                    return null;
                case LineStyle.Dash:
                    return new double[] { 3, 1 };
                case LineStyle.Dot:
                    return new double[] { 1, 1 };
                case LineStyle.DashDot:
                    return new double[] { 3, 1, 1, 1 };
                case LineStyle.DashDashDot:
                    return new double[] { 3, 1, 3, 1, 1, 1 };
                case LineStyle.DashDotDot:
                    return new double[] { 3, 1, 1, 1, 1, 1 };
                default:
                    return null;
            }
        }
    }
}