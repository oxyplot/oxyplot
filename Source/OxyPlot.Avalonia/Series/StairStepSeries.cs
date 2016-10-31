// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StairStepSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.StairStepSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.StairStepSeries
    /// </summary>
    public class StairStepSeries : LineSeries
    {
        /// <summary>
        /// Identifies the <see cref="VerticalStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> VerticalStrokeThicknessProperty = AvaloniaProperty.Register<StairStepSeries, double>(nameof(VerticalStrokeThickness), double.NaN);

        /// <summary>
        /// Identifies the <see cref="VerticalStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> VerticalLineStyleProperty = AvaloniaProperty.Register<StairStepSeries, LineStyle>(nameof(VerticalLineStyle), LineStyle.Automatic);

        /// <summary>
        /// Initializes a new instance of the <see cref="StairStepSeries"/> class.
        /// </summary>
        public StairStepSeries()
        {
            InternalSeries = new OxyPlot.Series.StairStepSeries();
        }

        /// <summary>
        /// Gets or sets the stroke thickness of the vertical line segments.
        /// </summary>
        /// <value>The vertical stroke thickness.</value>
        /// <remarks>Set the value to NaN to use the StrokeThickness property for both horizontal and vertical segments.
        /// Using the VerticalStrokeThickness property will have a small performance hit.</remarks>
        public double VerticalStrokeThickness
        {
            get { return GetValue(VerticalStrokeThicknessProperty); }
            set { SetValue(VerticalStrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the line style of the vertical line segments.
        /// </summary>
        /// <value>The vertical line style.</value>
        public LineStyle VerticalLineStyle
        {
            get { return GetValue(VerticalLineStyleProperty); }
            set { SetValue(VerticalLineStyleProperty, value); }
        }

        /// <summary>
        /// Creates the internal series.
        /// </summary>
        /// <returns>
        /// The internal series.
        /// </returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            SynchronizeProperties(InternalSeries);
            return InternalSeries;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.StairStepSeries)series;
            s.VerticalLineStyle = VerticalLineStyle;
            s.VerticalStrokeThickness = VerticalStrokeThickness;
        }

        static StairStepSeries()
        {
            VerticalStrokeThicknessProperty.Changed.AddClassHandler<StairStepSeries>(AppearanceChanged);
            VerticalLineStyleProperty.Changed.AddClassHandler<StairStepSeries>(AppearanceChanged);
        }
    }
}