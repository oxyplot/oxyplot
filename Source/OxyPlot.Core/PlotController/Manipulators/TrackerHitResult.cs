// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerHitResult.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides data for a tracker hit result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using OxyPlot.Series;

    /// <summary>
    /// Provides data for a tracker hit result.
    /// </summary>
    /// <remarks>This is used as DataContext for the TrackerControl.
    /// The TrackerControl is visible when the user use the left mouse button to "track" points on the series.</remarks>
    public class TrackerHitResult
    {
        /// <summary>
        /// Gets or sets the nearest or interpolated data point.
        /// </summary>
        public DataPoint DataPoint { get; set; }

        /// <summary>
        /// Gets or sets the source item of the point.
        /// If the current point is from an ItemsSource and is not interpolated, this property will contain the item.
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// Gets or sets the index for the Item.
        /// </summary>
        public double Index { get; set; }

        /// <summary>
        /// Gets or sets the horizontal/vertical line extents.
        /// </summary>
        public OxyRect LineExtents { get; set; }

        /// <summary>
        /// Gets or sets the plot model.
        /// </summary>
        public PlotModel PlotModel { get; set; }

        /// <summary>
        /// Gets or sets the position in screen coordinates.
        /// </summary>
        public ScreenPoint Position { get; set; }

        /// <summary>
        /// Gets or sets the series that is being tracked.
        /// </summary>
        public Series.Series Series { get; set; }

        /// <summary>
        /// Gets or sets the text shown in the tracker.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets the X axis.
        /// </summary>
        public Axes.Axis XAxis
        {
            get
            {
                var xyas = this.Series as XYAxisSeries;
                return xyas != null ? xyas.XAxis : null;
            }
        }

        /// <summary>
        /// Gets the Y axis.
        /// </summary>
        public Axes.Axis YAxis
        {
            get
            {
                var xyas = this.Series as XYAxisSeries;
                return xyas != null ? xyas.YAxis : null;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.Text != null ? this.Text.Trim() : string.Empty;
        }
    }
}