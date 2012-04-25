// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerHitResult.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    ///   Provides a data container for a tracker hit result.
    /// </summary>
    /// <remarks>
    ///   This is used as DataContext for the TrackerControl.
    ///   The TrackerControl is visible when the user use the left mouse button to "track" points on the series.
    /// </remarks>
    public class TrackerHitResult
    {
        #region Constants and Fields

        /// <summary>
        ///   The default format string.
        /// </summary>
        private const string DefaultFormatString = "{0}\n{1}: {2}\n{3}: {4}";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerHitResult"/> class.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <param name="dp">The data point.</param>
        /// <param name="sp">The screen point.</param>
        /// <param name="item">The item.</param>
        /// <param name="index">The index.</param>
        /// <param name="text">The text.</param>
        public TrackerHitResult(Series series, IDataPoint dp, ScreenPoint sp, object item = null, double index = -1, string text = null)
        {
            this.DataPoint = dp;
            this.Position = sp;
            this.Item = item;
            this.Index = index;
            this.Series = series;
            this.Text = text;
            var ds = series as DataPointSeries;
            if (ds != null)
            {
                this.XAxis = ds.XAxis;
                this.YAxis = ds.YAxis;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the nearest or interpolated data point.
        /// </summary>
        public IDataPoint DataPoint { get; set; }

        /// <summary>
        ///   Gets or sets the source item of the point.
        ///   If the current point is from an ItemsSource and is not interpolated, this property will contain the item.
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// Gets or sets the index for the Item.
        /// </summary>
        public double Index { get; set; }

        /// <summary>
        ///   Gets or sets the horizontal/vertical line extents.
        /// </summary>
        public OxyRect LineExtents { get; set; }

        /// <summary>
        ///   Gets or sets the plot model.
        /// </summary>
        public PlotModel PlotModel { get; set; }

        /// <summary>
        ///   Gets or sets the position in screen coordinates.
        /// </summary>
        public ScreenPoint Position { get; set; }

        /// <summary>
        ///   Gets or sets the series that is being tracked.
        /// </summary>
        public Series Series { get; set; }

        /// <summary>
        ///   Gets or sets the text shown in the tracker.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///   Gets or sets the X axis.
        /// </summary>
        public Axis XAxis { get; set; }

        /// <summary>
        ///   Gets or sets the Y axis.
        /// </summary>
        public Axis YAxis { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (this.Text != null)
            {
                return this.Text;
            }

            var ts = this.Series as ITrackableSeries;
            string formatString = DefaultFormatString;
            if (ts != null && !string.IsNullOrEmpty(ts.TrackerFormatString))
            {
                formatString = ts.TrackerFormatString;
            }

            string xaxisTitle = (this.XAxis != null ? this.XAxis.Title : null) ?? "X";
            string yaxisTitle = (this.YAxis != null ? this.YAxis.Title : null) ?? "Y";
            object xvalue = this.XAxis != null ? this.XAxis.GetValue(this.DataPoint.X) : this.DataPoint.X;
            object yvalue = this.YAxis != null ? this.YAxis.GetValue(this.DataPoint.Y) : this.DataPoint.Y;

            return StringHelper.Format(
                this.Series.ActualCulture,
                formatString,
                this.Item,
                this.Series.Title,
                xaxisTitle,
                xvalue,
                yaxisTitle,
                yvalue,
                this.Item).Trim();
        }

        #endregion
    }
}