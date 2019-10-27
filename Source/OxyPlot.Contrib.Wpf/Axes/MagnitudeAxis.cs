// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagnitudeAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.MagnitudeAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    using OxyPlot.Axes;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.MagnitudeAxis.
    /// </summary>
    public class MagnitudeAxis : LinearAxis
    {
        /// <summary>
        /// Initializes static members of the <see cref = "MagnitudeAxis" /> class.
        /// </summary>
        static MagnitudeAxis()
        {
            MajorGridlineStyleProperty.OverrideMetadata(typeof(MagnitudeAxis), new PropertyMetadata(LineStyle.Solid));
            MinorGridlineStyleProperty.OverrideMetadata(typeof(MagnitudeAxis), new PropertyMetadata(LineStyle.Solid));
            PositionProperty.OverrideMetadata(typeof(MagnitudeAxis), new PropertyMetadata(AxisPosition.None, AppearanceChanged));
            IsPanEnabledProperty.OverrideMetadata(typeof(MagnitudeAxis), new PropertyMetadata(false));
            IsZoomEnabledProperty.OverrideMetadata(typeof(MagnitudeAxis), new PropertyMetadata(false));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "MagnitudeAxis" /> class.
        /// </summary>
        public MagnitudeAxis()
        {
            this.InternalAxis = new Axes.MagnitudeAxis();
        }
    }
}