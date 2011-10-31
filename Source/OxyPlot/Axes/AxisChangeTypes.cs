// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisChangeTypes.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Change types of the IAxis.AxisChanged event.
    /// </summary>
    public enum AxisChangeTypes
    {
        /// <summary>
        ///   The zoom.
        /// </summary>
        Zoom, 

        /// <summary>
        ///   The pan.
        /// </summary>
        Pan, 

        /// <summary>
        ///   The reset.
        /// </summary>
        Reset
    }
}