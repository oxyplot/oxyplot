// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AngleAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.AngleAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{

    using OxyPlot.Axes;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.AngleAxis.
    /// </summary>
    public class AngleAxis : LinearAxis
    {
        /// <summary>
        /// Identifies the <see cref="StartAngle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StartAngleProperty = AvaloniaProperty.Register<AngleAxis, double>(nameof(StartAngle), 0d);

        /// <summary>
        /// Identifies the <see cref="EndAngle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> EndAngleProperty = AvaloniaProperty.Register<AngleAxis, double>(nameof(EndAngle), 360d);

        /// <summary>
        /// Initializes static members of the <see cref = "AngleAxis" /> class.
        /// </summary>
        static AngleAxis()
        {
            MajorGridlineStyleProperty.OverrideMetadata(typeof(AngleAxis), new StyledPropertyMetadata<LineStyle>(LineStyle.Solid));
            MinorGridlineStyleProperty.OverrideMetadata(typeof(AngleAxis), new StyledPropertyMetadata<LineStyle>(LineStyle.Solid));
            PositionProperty.OverrideMetadata(typeof(AngleAxis), new StyledPropertyMetadata<AxisPosition>(AxisPosition.None));
            TickStyleProperty.OverrideMetadata(typeof(AngleAxis), new StyledPropertyMetadata<TickStyle>(TickStyle.None));
            IsPanEnabledProperty.OverrideMetadata(typeof(AngleAxis), new StyledPropertyMetadata<bool>(false));
            IsZoomEnabledProperty.OverrideMetadata(typeof(AngleAxis), new StyledPropertyMetadata<bool>(false));
            StartAngleProperty.Changed.AddClassHandler<AngleAxis>(AppearanceChanged);
            EndAngleProperty.Changed.AddClassHandler<AngleAxis>(AppearanceChanged);
            PositionProperty.Changed.AddClassHandler<AngleAxis>(AppearanceChanged);
            TickStyleProperty.Changed.AddClassHandler<AngleAxis>(AppearanceChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AngleAxis" /> class.
        /// </summary>
        public AngleAxis()
        {
            InternalAxis = new Axes.AngleAxis();
            MajorGridlineStyle = LineStyle.Solid;
            MinorGridlineStyle = LineStyle.Solid;
        }

        /// <summary>
        /// Gets or sets the start angle (degrees).
        /// </summary>
        public double StartAngle
        {
            get { return GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the end angle (degrees).
        /// </summary>
        public double EndAngle
        {
            get { return GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Axes.AngleAxis)InternalAxis;
            a.StartAngle = StartAngle;
            a.EndAngle = EndAngle;
            a.FractionUnitSymbol = FractionUnitSymbol;
        }
    }
}