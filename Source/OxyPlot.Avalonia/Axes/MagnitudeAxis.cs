// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagnitudeAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.MagnitudeAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Avalonia
{
    using global::Avalonia;
    using OxyPlot.Axes;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.MagnitudeAxis.
    /// </summary>
    public class MagnitudeAxis : LinearAxis
    {
        /// <summary>
        /// Initializes static members of the <see cref = "MagnitudeAxis" /> class.
        /// </summary>
        static MagnitudeAxis()
        {
            MajorGridlineStyleProperty.OverrideDefaultValue<MagnitudeAxis>(LineStyle.Solid);
            MinorGridlineStyleProperty.OverrideDefaultValue<MagnitudeAxis>(LineStyle.Solid);
            PositionProperty.OverrideDefaultValue<MagnitudeAxis>(AxisPosition.None);
            PositionProperty.Changed.AddClassHandler<MagnitudeAxis>(AppearanceChanged);
            IsPanEnabledProperty.OverrideDefaultValue<MagnitudeAxis>(false);
            IsZoomEnabledProperty.OverrideDefaultValue<MagnitudeAxis>(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "MagnitudeAxis" /> class.
        /// </summary>
        public MagnitudeAxis()
        {
            InternalAxis = new Axes.MagnitudeAxis();
        }
    }
}