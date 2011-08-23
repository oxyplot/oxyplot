// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AngleAxis.cs" company="OxyPlot">
//   see http://oxyplot.codeplex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    /// <summary>
    /// This is a WPF wrapper of OxyPlot.AngleAxis.
    /// </summary>
    public class AngleAxis : LinearAxis
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AngleAxis"/> class.
        /// </summary>
        public AngleAxis()
        {
            this.internalAxis = new OxyPlot.AngleAxis();
        }

        #endregion
    }
}