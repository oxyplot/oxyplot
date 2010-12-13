using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Shows a tracker with horizontal and vertical lines, and a templated content control.
    /// The ContentTemplate must be set to define how the tracker label looks.
    /// The Plots contains a default template in the Themes/Generic.xaml file. 
    /// </summary>
    public class Tracker : Canvas
    {
        private readonly Line trackerLine1 = new Line();
        private readonly Line trackerLine2 = new Line();
        private readonly ContentControl content = new ContentControl();

        /// <summary>
        /// Gets or sets the tracker label format.
        /// </summary>
        /// <value>The tracker label format.</value>
        public string LabelFormat { get; set; }

        /// <summary>
        /// Gets or sets the content template.
        /// </summary>
        /// <value>The content template.</value>
        public DataTemplate ContentTemplate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracker"/> class.
        /// </summary>
        /// <param name="pc">The parent Plot.</param>
        public Tracker()
        {
            LabelFormat = null; // "{0:0.###} {1:0.###}";

            Children.Add(trackerLine1);
            Children.Add(trackerLine2);
            Children.Add(content);

            trackerLine1.StrokeThickness = 1;
            trackerLine2.StrokeThickness = 1;
            trackerLine1.Stroke = new SolidColorBrush(Color.FromArgb(80, 0, 0, 0));
            trackerLine2.Stroke = trackerLine1.Stroke;

            trackerLine1.SnapsToDevicePixels = true;
            trackerLine2.SnapsToDevicePixels = true;
            trackerLine1.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            trackerLine2.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            Hide();
        }

        /// <summary>
        /// Sets the position of the tracker.
        /// </summary>
        /// <param name="dp">The data point.</param>
        /// <param name="s">The series.</param>
        public void SetPosition(DataPoint dp, OxyPlot.DataSeries s)
        {
            var dp1 = new DataPoint(dp.X, s.YAxis.ActualMaximum);
            var dp2 = new DataPoint(dp.X, s.YAxis.ActualMinimum);
            var dp3 = new DataPoint(s.XAxis.ActualMinimum, dp.Y);
            var dp4 = new DataPoint(s.XAxis.ActualMaximum, dp.Y);

            var pt0 = AxisBase.Transform(dp, s.XAxis, s.YAxis);
            var pt1 = AxisBase.Transform(dp1, s.XAxis, s.YAxis);
            var pt2 = AxisBase.Transform(dp2, s.XAxis, s.YAxis);
            var pt3 = AxisBase.Transform(dp3, s.XAxis, s.YAxis);
            var pt4 = AxisBase.Transform(dp4, s.XAxis, s.YAxis);

            if (ContentTemplate != null)
            {
                content.Content = new TrackerViewModel(s, dp, LabelFormat);
                content.ContentTemplate = ContentTemplate;
                SetLeft(content, pt0.X);
                SetTop(content, pt0.Y);
            }

            trackerLine1.X1 = pt1.X;
            trackerLine1.Y1 = pt1.Y;
            trackerLine1.X2 = pt2.X;
            trackerLine1.Y2 = pt2.Y;

            trackerLine2.X1 = pt3.X;
            trackerLine2.Y1 = pt3.Y;
            trackerLine2.X2 = pt4.X;
            trackerLine2.Y2 = pt4.Y;

            Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides the tracker.
        /// </summary>
        public void Hide()
        {
            Visibility = Visibility.Hidden;
        }
    }

    public class TrackerViewModel
    {
        /// <summary>
        /// Gets or sets the position of the tracker (data coordinates).
        /// </summary>
        /// <value>The point.</value>
        public DataPoint Point { get; set; }

        /// <summary>
        /// Gets or sets the data series.
        /// </summary>
        /// <value>The series.</value>
        public OxyPlot.DataSeries Series { get; set; }

        /// <summary>
        /// Gets or sets the format string.
        /// {0}: {1:0.#####}, {2}: {3:0.#####}
        /// xAxisTitle: xValue, yAxisTitle: yValue
        /// </summary>
        /// <value>The format.</value>
        public string Format { get; set; }

        private const string DefaultFormat = "{0}: {1:0.#####}, {2}: {3:0.#####}";

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerViewModel"/> class.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <param name="point">The point.</param>
        /// <param name="fmt">The format string.</param>
        public TrackerViewModel(OxyPlot.DataSeries series, DataPoint point, string fmt = null)
        {
            if (String.IsNullOrEmpty(fmt))
                fmt = DefaultFormat;

            Format = fmt;
            Series = series;
            Point = point;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var xTitle = Series.XAxis.Title;
            var yTitle = Series.YAxis.Title;
            if (String.IsNullOrEmpty(xTitle)) xTitle = "X";
            if (String.IsNullOrEmpty(yTitle)) yTitle = "Y";
            return String.Format(CultureInfo.InvariantCulture, Format, xTitle, Point.X, yTitle, Point.Y);
        }
    }
}