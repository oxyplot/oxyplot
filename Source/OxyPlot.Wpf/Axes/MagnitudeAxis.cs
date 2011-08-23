// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagnitudeAxis.cs" company="OxyPlot">
//   see http://oxyplot.codeplex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    /// <summary>
    /// This is a WPF wrapper of OxyPlot.MagnitudeAxis.
    /// </summary>
    public class MagnitudeAxis : LinearAxis
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MagnitudeAxis"/> class.
        /// </summary>
        public MagnitudeAxis()
        {
            this.internalAxis = new OxyPlot.MagnitudeAxis();
        }

        #endregion
    }
}