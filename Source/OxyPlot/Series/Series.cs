// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Series.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for plot series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using OxyPlot.Axes;

    /// <summary>
    /// Provides an abstract base class for plot series.
    /// </summary>
    /// <remarks>This class contains internal methods that should be called only from the PlotModel.</remarks>
    public abstract class Series : PlotElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Series" /> class.
        /// </summary>
        protected Series()
        {
            this.IsVisible = true;
            this.Background = OxyColors.Undefined;
            this.RenderInLegend = true;
        }

        /// <summary>
        /// Gets or sets the background color of the series. The default is <c>OxyColors.Undefined</c>.
        /// </summary>
        /// <remarks>This property defines the background color in the area defined by the x and y axes used by this series.</remarks>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this series is visible. The default is <c>true</c>.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the title of the series. The default is <c>null</c>.
        /// </summary>
        /// <value>The title that is shown in the legend of the plot. The default value is <c>null</c>.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the key for the Legend to use on this series. The default is <c>null</c>.
        /// </summary>
        /// <remarks>
        /// This key may be used by the plot model to show a custom Legend for the series.
        /// </remarks>
        public string LegendKey { get; set; }

        /// <summary>
        /// Gets or sets the groupname for the Series. The default is <c>null</c>.
        /// </summary>
        /// <remarks>
        /// This groupname may for e.g. be used by the Legend class to group series into separated blocks.
        /// </remarks>
        public string SeriesGroupName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the series should be rendered in the legend. The default is <c>true</c>.
        /// </summary>
        public bool RenderInLegend { get; set; }

        /// <summary>
        /// Gets or sets a format string used for the tracker. The default depends on the series.
        /// </summary>
        /// <remarks>
        /// The arguments for the format string may be different for each type of series. See the documentation.
        /// </remarks>
        public string TrackerFormatString { get; set; }

        /// <summary>
        /// Gets or sets the key for the tracker to use on this series. The default is <c>null</c>.
        /// </summary>
        /// <remarks>
        /// This key may be used by the plot view to show a custom tracker for the series.
        /// </remarks>
        public string TrackerKey { get; set; }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public virtual TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            return null;
        }

        /// <summary>
        /// Renders the series on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public abstract void Render(IRenderContext rc);

        /// <summary>
        /// Renders the legend symbol on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The legend rectangle.</param>
        public abstract void RenderLegend(IRenderContext rc, OxyRect legendBox);

        /// <summary>
        /// Checks if this data series requires X/Y axes. (e.g. Pie series do not require axes)
        /// </summary>
        /// <returns><c>true</c> if axes are required.</returns>
        protected internal abstract bool AreAxesRequired();

        /// <summary>
        /// Ensures that the axes of the series are defined.
        /// </summary>
        protected internal abstract void EnsureAxes();

        /// <summary>
        /// Checks if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">The axis that should be checked.</param>
        /// <returns><c>true</c> if the axis is in use.</returns>
        protected internal abstract bool IsUsing(Axis axis);

        /// <summary>
        /// Sets the default values (colors, line style etc.) from the plot model.
        /// </summary>
        protected internal abstract void SetDefaultValues();

        /// <summary>
        /// Updates the maximum and minimum values of the axes used by this series.
        /// </summary>
        protected internal abstract void UpdateAxisMaxMin();

        /// <summary>
        /// Updates the data of the series.
        /// </summary>
        protected internal abstract void UpdateData();

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        /// <remarks>This method is called when the <see cref="PlotModel" /> is updated with the <c>updateData</c> parameter set to <c>true</c>.</remarks>
        protected internal abstract void UpdateMaxMin();

        /// <summary>
        /// When overridden in a derived class, tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// The result of the hit test.
        /// </returns>
        protected override HitTestResult HitTestOverride(HitTestArguments args)
        {
            var thr = this.GetNearestPoint(args.Point, true) ?? this.GetNearestPoint(args.Point, false);

            if (thr != null)
            {
                double distance = thr.Position.DistanceTo(args.Point);
                if (distance > args.Tolerance)
                {
                    return null;
                }

                return new HitTestResult(this, thr.Position, thr.Item, thr.Index);
            }

            return null;
        }
    }
}
