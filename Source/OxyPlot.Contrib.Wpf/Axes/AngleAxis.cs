// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AngleAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.AngleAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    using OxyPlot.Axes;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.AngleAxis.
    /// </summary>
    public class AngleAxis : LinearAxis
    {
        /// <summary>
        /// Identifies the <see cref="StartAngle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(AngleAxis), new PropertyMetadata(0d, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="EndAngle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(AngleAxis), new PropertyMetadata(360d, AppearanceChanged));

        /// <summary>
        /// Initializes static members of the <see cref = "AngleAxis" /> class.
        /// </summary>
        static AngleAxis()
        {
            MajorGridlineStyleProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(LineStyle.Solid));
            MinorGridlineStyleProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(LineStyle.Solid));
            PositionProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(AxisPosition.None, AppearanceChanged));
            TickStyleProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(TickStyle.None, AppearanceChanged));
            IsPanEnabledProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(false));
            IsZoomEnabledProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(false));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AngleAxis" /> class.
        /// </summary>
        public AngleAxis()
        {
            this.InternalAxis = new Axes.AngleAxis();
            this.MajorGridlineStyle = LineStyle.Solid;
            this.MinorGridlineStyle = LineStyle.Solid;
        }

        /// <summary>
        /// Gets or sets the start angle (degrees).
        /// </summary>
        public double StartAngle
        {
            get { return (double)this.GetValue(StartAngleProperty); }
            set { this.SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the end angle (degrees).
        /// </summary>
        public double EndAngle
        {
            get { return (double)this.GetValue(EndAngleProperty); }
            set { this.SetValue(EndAngleProperty, value); }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Axes.AngleAxis)this.InternalAxis;
            a.StartAngle = this.StartAngle;
            a.EndAngle = this.EndAngle;
            a.FractionUnitSymbol = this.FractionUnitSymbol;
        }
    }
}