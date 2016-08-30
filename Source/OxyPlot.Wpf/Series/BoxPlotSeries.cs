// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoxPlotSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.BoxPlotSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.BoxPlotSeries
    /// </summary>
    public class BoxPlotSeries : XYAxisSeries
    {
        /// <summary>
        /// Identifies this <see cref="BoxWidthProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BoxWidthProperty = DependencyProperty.Register(
            "BoxWidth",
            typeof(double),
            typeof(BoxPlotSeries),
            new PropertyMetadata(0.3, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="FillProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill",
            typeof(Color),
            typeof(BoxPlotSeries),
            new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="LineStyleProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle",
            typeof(LineStyle),
            typeof(BoxPlotSeries),
            new PropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="MedianPointSizeProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MedianPointSizeProperty = DependencyProperty.Register(
            "MedianPointSize",
            typeof(double),
            typeof(BoxPlotSeries),
            new PropertyMetadata(2.0, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="MedianThicknessProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MedianThicknessProperty = DependencyProperty.Register(
            "MedianThickness",
            typeof(double),
            typeof(BoxPlotSeries),
            new PropertyMetadata(2.0, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="OutlierSizeProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OutlierSizeProperty = DependencyProperty.Register(
            "OutlierSize",
            typeof(double),
            typeof(BoxPlotSeries),
            new PropertyMetadata(2.0, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="OutlierTypeProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OutlierTypeProperty = DependencyProperty.Register(
            "OutlierType",
            typeof(MarkerType),
            typeof(BoxPlotSeries),
            new PropertyMetadata(MarkerType.Circle, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="OutlierOutlineProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OutlierOutlineProperty = DependencyProperty.Register(
            "OutlierOutline",
            typeof(Point[]),
            typeof(BoxPlotSeries),
            new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="ShowBoxProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowBoxProperty = DependencyProperty.Register(
            "ShowBox",
            typeof(bool),
            typeof(BoxPlotSeries),
            new PropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="ShowMedianAsDotProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowMedianAsDotProperty = DependencyProperty.Register(
            "ShowMedianAsDot",
            typeof(bool),
            typeof(BoxPlotSeries),
            new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="StrokeProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Color),
            typeof(BoxPlotSeries),
            new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="StrokeThicknessProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness",
            typeof(double),
            typeof(BoxPlotSeries),
            new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="WhiskerWidthProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WhiskerWidthProperty = DependencyProperty.Register(
            "WhiskerWidth",
            typeof(double),
            typeof(BoxPlotSeries),
            new PropertyMetadata(0.5, AppearanceChanged));

        /// <summary>
        /// Initializes static members of the <see cref="BoxPlotSeries"/> class.
        /// </summary>
        static BoxPlotSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(BoxPlotSeries), new PropertyMetadata(OxyPlot.Series.BoxPlotSeries.DefaultTrackerFormatString, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxPlotSeries" /> class.
        /// </summary>
        public BoxPlotSeries()
        {
            this.InternalSeries = new OxyPlot.Series.BoxPlotSeries();
        }

        /// <summary>
        /// Gets or sets the width of the boxes (specified in x-axis units).
        /// </summary>
        /// <value>The width of the boxes.</value>
        public double BoxWidth
        {
            get { return (double)GetValue(BoxWidthProperty); }
            set { this.SetValue(BoxWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the fill color. If <c>null</c>, this color will be automatically set.
        /// </summary>
        /// <value>The fill color.</value>
        public Color Fill
        {
            get { return (Color)GetValue(FillProperty); }
            set { this.SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle
        {
            get { return (LineStyle)GetValue(LineStyleProperty); }
            set { this.SetValue(LineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the median point.
        /// </summary>
        /// <remarks>This property is only used when MedianStyle = Dot.</remarks>
        public double MedianPointSize
        {
            get { return (double)GetValue(MedianPointSizeProperty); }
            set { this.SetValue(MedianPointSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the median thickness, relative to the StrokeThickness.
        /// </summary>
        /// <value>The median thickness.</value>
        public double MedianThickness
        {
            get { return (double)GetValue(MedianThicknessProperty); }
            set { this.SetValue(MedianThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the diameter of the outlier circles (specified in points).
        /// </summary>
        /// <value>The size of the outlier.</value>
        public double OutlierSize
        {
            get { return (double)GetValue(OutlierSizeProperty); }
            set { this.SetValue(OutlierSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of the outliers.
        /// </summary>
        /// <value>The type of the outliers.</value>
        /// <remarks>MarkerType.Custom is currently not supported.</remarks>
        public MarkerType OutlierType
        {
            get { return (MarkerType)GetValue(OutlierTypeProperty); }
            set { this.SetValue(OutlierTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the a custom polygon outline for the outlier markers. Set <see cref="OutlierType" /> to <see cref="OxyPlot.MarkerType.Custom" /> to use this property.
        /// </summary>
        /// <value>A polyline. The default is <c>null</c>.</value>
        public Point[] OutlierOutline
        {
            get { return (Point[])GetValue(OutlierOutlineProperty); }
            set { this.SetValue(OutlierOutlineProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the boxes.
        /// </summary>
        public bool ShowBox
        {
            get { return (bool)GetValue(ShowBoxProperty); }
            set { this.SetValue(ShowBoxProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the median as a dot.
        /// </summary>
        public bool ShowMedianAsDot
        {
            get { return (bool)GetValue(ShowMedianAsDotProperty); }
            set { this.SetValue(ShowMedianAsDotProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        /// <value>The stroke color.</value>
        public Color Stroke
        {
            get { return (Color)GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { this.SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the whiskers (relative to the BoxWidth).
        /// </summary>
        /// <value>The width of the whiskers.</value>
        public double WhiskerWidth
        {
            get { return (double)GetValue(WhiskerWidthProperty); }
            set { this.SetValue(WhiskerWidthProperty, value); }
        }

        /// <summary>
        /// Creates the internal series.
        /// </summary>
        /// <returns>The internal series.</returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            this.SynchronizeProperties(this.InternalSeries);
            return this.InternalSeries;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);

            var s = (OxyPlot.Series.BoxPlotSeries)series;
            s.Fill = this.Fill.ToOxyColor();
            s.LineStyle = this.LineStyle;
            s.MedianPointSize = this.MedianPointSize;
            s.OutlierSize = this.OutlierSize;
            s.OutlierType = this.OutlierType;
            s.OutlierOutline = this.OutlierOutline.ToScreenPointArray();
            s.ShowBox = this.ShowBox;
            s.ShowMedianAsDot = this.ShowMedianAsDot;
            s.Stroke = this.Stroke.ToOxyColor();
            s.StrokeThickness = this.StrokeThickness;
            s.WhiskerWidth = this.WhiskerWidth;

            if (this.ItemsSource == null)
            {
                s.Items.Clear();
                foreach (var item in this.Items)
                {
                    s.Items.Add((OxyPlot.Series.BoxPlotItem)item);
                }
            }
        }
    }
}
