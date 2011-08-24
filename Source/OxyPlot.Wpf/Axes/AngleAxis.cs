// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AngleAxis.cs" company="OxyPlot">
//   see http://oxyplot.codeplex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.AngleAxis.
    /// </summary>
    public class AngleAxis : LinearAxis
    {
        static AngleAxis()
        {
            MajorGridlineStyleProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(LineStyle.Solid));
            MinorGridlineStyleProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(LineStyle.Solid));
        }

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AngleAxis"/> class.
        /// </summary>
        public AngleAxis()
        {
            this.internalAxis = new OxyPlot.AngleAxis();
            MajorGridlineStyle = LineStyle.Solid;
            MinorGridlineStyle = LineStyle.Solid;
        }

        #endregion
    }
}