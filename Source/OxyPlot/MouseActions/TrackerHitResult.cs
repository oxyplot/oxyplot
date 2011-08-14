using System;
using System.Globalization;

namespace OxyPlot
{
    /// <summary>
    /// Tracker data class.
    /// This is used as DataContext for the TrackerControl.
    /// The TrackerControl is visible when the user use the left mouse button to "track" points on the series.
    /// </summary>
    public class TrackerHitResult
    {
        /// <summary>
        /// Gets or sets the position in screen coordinates.
        /// </summary>
        public ScreenPoint Position { get; set; }

        /// <summary>
        /// Gets or sets the nearest or interpolated data point.
        /// </summary>
        public DataPoint DataPoint { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        public ISeries Series { get; set; }

        /// <summary>
        /// Gets or sets the plot model.
        /// </summary>
        public PlotModel PlotModel { get; set; }

        /// <summary>
        /// Gets or sets the horizontal/vertical line extents.
        /// </summary>
        public OxyRect LineExtents { get; set; }

        /// <summary>
        /// Gets or sets the X axis.
        /// </summary>
        public IAxis XAxis { get; set; }

        /// <summary>
        /// Gets or sets the Y axis.
        /// </summary>
        public IAxis YAxis { get; set; }

        /// <summary>
        /// Gets or sets the source item of the point.
        /// If the current point is from an ItemsSource and is not interpolated, this property will contain the item.
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// Gets or sets the text shown in the tracker.
        /// </summary>
        public string Text { get; set; }

        private const string DefaultFormatString = "{1}: {2}\n{3}: {4}";

        public TrackerHitResult(ISeries series, DataPoint dp, ScreenPoint sp, object item, string text=null)
        {
            this.DataPoint = dp;
            this.Position = sp;
            this.Item = item;
            this.Series = series;
            this.Text = text;
            var ds = series as DataPointSeries;
            if (ds!=null)
            {
                XAxis = ds.XAxis;
                YAxis = ds.YAxis;
            }
        }

        public override string ToString()
        {
            if (Text!=null)
                return Text;

            var ts = Series as ITrackableSeries;
            string formatString = DefaultFormatString;
            if (ts != null && !String.IsNullOrEmpty(ts.TrackerFormatString))
                formatString = ts.TrackerFormatString;

            string xAxisTitle = (XAxis != null ? XAxis.Title : null) ?? "X";
            string yAxisTitle = (YAxis != null ? YAxis.Title : null) ?? "Y";
            object xValue = XAxis != null ? XAxis.GetValue(DataPoint.X) : DataPoint.X;
            object yValue = YAxis != null ? YAxis.GetValue(DataPoint.Y) : DataPoint.Y;
            return String.Format(CultureInfo.InvariantCulture, formatString, Series.Title, xAxisTitle, xValue, yAxisTitle, yValue, Item);     
        }
    }
}