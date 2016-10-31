// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoxPlotSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.BoxPlotSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;
    using System.Linq;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.BoxPlotSeries
    /// </summary>
    public class BoxPlotSeries : XYAxisSeries
    {
        /// <summary>
        /// Identifies this <see cref="BoxWidthProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> BoxWidthProperty = AvaloniaProperty.Register<BoxPlotSeries, double>(nameof(BoxWidth), 0.3);

        /// <summary>
        /// Identifies this <see cref="FillProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> FillProperty = AvaloniaProperty.Register<BoxPlotSeries, Color>(nameof(Fill), MoreColors.Automatic);

        /// <summary>
        /// Identifies this <see cref="LineStyleProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> LineStyleProperty = AvaloniaProperty.Register<BoxPlotSeries, LineStyle>(nameof(LineStyle), LineStyle.Solid);

        /// <summary>
        /// Identifies this <see cref="MedianPointSizeProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MedianPointSizeProperty = AvaloniaProperty.Register<BoxPlotSeries, double>(nameof(MedianPointSize), 2.0);

        /// <summary>
        /// Identifies this <see cref="MedianThicknessProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MedianThicknessProperty = AvaloniaProperty.Register<BoxPlotSeries, double>(nameof(MedianThickness), 2.0);

        /// <summary>
        /// Identifies this <see cref="OutlierSizeProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> OutlierSizeProperty = AvaloniaProperty.Register<BoxPlotSeries, double>(nameof(OutlierSize), 2.0);

        /// <summary>
        /// Identifies this <see cref="OutlierTypeProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<MarkerType> OutlierTypeProperty = AvaloniaProperty.Register<BoxPlotSeries, MarkerType>(nameof(OutlierType), MarkerType.Circle);

        /// <summary>
        /// Identifies this <see cref="OutlierOutlineProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Point[]> OutlierOutlineProperty = AvaloniaProperty.Register<BoxPlotSeries, Point[]>(nameof(OutlierOutline), null);

        /// <summary>
        /// Identifies this <see cref="ShowBoxProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowBoxProperty = AvaloniaProperty.Register<BoxPlotSeries, bool>(nameof(ShowBox), true);

        /// <summary>
        /// Identifies this <see cref="ShowMedianAsDotProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowMedianAsDotProperty = AvaloniaProperty.Register<BoxPlotSeries, bool>(nameof(ShowMedianAsDot), false);

        /// <summary>
        /// Identifies this <see cref="StrokeProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> StrokeProperty = AvaloniaProperty.Register<BoxPlotSeries, Color>(nameof(Stroke), Colors.Black);

        /// <summary>
        /// Identifies this <see cref="StrokeThicknessProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<BoxPlotSeries, double>(nameof(StrokeThickness), 1.0);

        /// <summary>
        /// Identifies this <see cref="WhiskerWidthProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> WhiskerWidthProperty = AvaloniaProperty.Register<BoxPlotSeries, double>(nameof(WhiskerWidth), 0.5);

        /// <summary>
        /// Initializes static members of the <see cref="BoxPlotSeries"/> class.
        /// </summary>
        static BoxPlotSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(BoxPlotSeries), new StyledPropertyMetadata<string>(OxyPlot.Series.BoxPlotSeries.DefaultTrackerFormatString));
            BoxWidthProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            FillProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            LineStyleProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            MedianPointSizeProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            MedianThicknessProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            OutlierSizeProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            OutlierTypeProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            OutlierOutlineProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            ShowBoxProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            ShowMedianAsDotProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            StrokeProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            StrokeThicknessProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            WhiskerWidthProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
            TrackerFormatStringProperty.Changed.AddClassHandler<BoxPlotSeries>(AppearanceChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxPlotSeries" /> class.
        /// </summary>
        public BoxPlotSeries()
        {
            InternalSeries = new OxyPlot.Series.BoxPlotSeries();
        }

        /// <summary>
        /// Gets or sets the width of the boxes (specified in x-axis units).
        /// </summary>
        /// <value>The width of the boxes.</value>
        public double BoxWidth
        {
            get { return GetValue(BoxWidthProperty); }
            set { SetValue(BoxWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the fill color. If <c>null</c>, this color will be automatically set.
        /// </summary>
        /// <value>The fill color.</value>
        public Color Fill
        {
            get { return GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle
        {
            get { return GetValue(LineStyleProperty); }
            set { SetValue(LineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the median point.
        /// </summary>
        /// <remarks>This property is only used when MedianStyle = Dot.</remarks>
        public double MedianPointSize
        {
            get { return GetValue(MedianPointSizeProperty); }
            set { SetValue(MedianPointSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the median thickness, relative to the StrokeThickness.
        /// </summary>
        /// <value>The median thickness.</value>
        public double MedianThickness
        {
            get { return GetValue(MedianThicknessProperty); }
            set { SetValue(MedianThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the diameter of the outlier circles (specified in points).
        /// </summary>
        /// <value>The size of the outlier.</value>
        public double OutlierSize
        {
            get { return GetValue(OutlierSizeProperty); }
            set { SetValue(OutlierSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of the outliers.
        /// </summary>
        /// <value>The type of the outliers.</value>
        /// <remarks>MarkerType.Custom is currently not supported.</remarks>
        public MarkerType OutlierType
        {
            get { return GetValue(OutlierTypeProperty); }
            set { SetValue(OutlierTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the a custom polygon outline for the outlier markers. Set <see cref="OutlierType" /> to <see cref="OxyPlot.MarkerType.Custom" /> to use this property.
        /// </summary>
        /// <value>A polyline. The default is <c>null</c>.</value>
        public Point[] OutlierOutline
        {
            get { return GetValue(OutlierOutlineProperty); }
            set { SetValue(OutlierOutlineProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the boxes.
        /// </summary>
        public bool ShowBox
        {
            get { return GetValue(ShowBoxProperty); }
            set { SetValue(ShowBoxProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the median as a dot.
        /// </summary>
        public bool ShowMedianAsDot
        {
            get { return GetValue(ShowMedianAsDotProperty); }
            set { SetValue(ShowMedianAsDotProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        /// <value>The stroke color.</value>
        public Color Stroke
        {
            get { return GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness
        {
            get { return GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the whiskers (relative to the BoxWidth).
        /// </summary>
        /// <value>The width of the whiskers.</value>
        public double WhiskerWidth
        {
            get { return GetValue(WhiskerWidthProperty); }
            set { SetValue(WhiskerWidthProperty, value); }
        }

        /// <summary>
        /// Creates the internal series.
        /// </summary>
        /// <returns>The internal series.</returns>
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

            var s = (OxyPlot.Series.BoxPlotSeries)series;
            s.Fill = Fill.ToOxyColor();
            s.LineStyle = LineStyle;
            s.MedianPointSize = MedianPointSize;
            s.OutlierSize = OutlierSize;
            s.OutlierType = OutlierType;
            s.OutlierOutline = (OutlierOutline ?? Enumerable.Empty<Point>()).Select(point => point.ToScreenPoint()).ToArray();
            s.ShowBox = ShowBox;
            s.ShowMedianAsDot = ShowMedianAsDot;
            s.Stroke = Stroke.ToOxyColor();
            s.StrokeThickness = StrokeThickness;
            s.WhiskerWidth = WhiskerWidth;

            if (Items != null)
            {
                s.Items.Clear();
                foreach (var item in Items)
                {
                    s.Items.Add((OxyPlot.Series.BoxPlotItem)item);
                }
            }
        }
    }
}
