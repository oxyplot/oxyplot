namespace OxyPlot
{
    using System.Collections;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Abstract base class for Series that can contains items.
    /// </summary>
    public abstract class ItemsSeries : ITrackableSeries
    {
        /// <summary>
        ///   Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        ///   Gets or sets the background color of the series.
        ///   The background area is defined by the x and y axes.
        /// </summary>
        /// <value>The background color.</value>
        public OxyColor Background { get; set; }

        /// <summary>
        ///   Gets the title of the Series.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Renders the Series on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="model">The model.</param>
        public abstract void Render(IRenderContext rc, PlotModel model);

        /// <summary>
        /// Renders the legend symbol on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The rect.</param>
        public abstract void RenderLegend(IRenderContext rc, OxyRect legendBox);

        /// <summary>
        /// Updates the data.
        /// </summary>
        public abstract void UpdateData();

        /// <summary>
        /// Check if this data series requires X/Y axes.
        /// (e.g. Pie series do not require axes)
        /// </summary>
        /// <returns></returns>
        public abstract bool AreAxesRequired();

        /// <summary>
        /// Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public abstract bool IsUsing(IAxis axis);

        /// <summary>
        /// Ensures that the series has axes.
        /// </summary>
        /// <param name="axes">The axes collection of the parent PlotModel.</param>
        /// <param name="defaultXAxis">The default X axis of the parent PlotModel.</param>
        /// <param name="defaultYAxis">The default Y axis of the parent PlotModel.</param>
        public abstract void EnsureAxes(Collection<IAxis> axes, IAxis defaultXAxis, IAxis defaultYAxis);

        /// <summary>
        /// Updates the maximum and minimum of the axes related to the series.
        /// </summary>
        public abstract void UpdateMaxMin();

        /// <summary>
        /// Sets default values from the plotmodel.
        /// </summary>
        /// <param name="model"></param>
        public abstract void SetDefaultValues(PlotModel model);

        /// <summary>
        /// Gets or sets a format string used for the tracker.
        /// </summary>
        public string TrackerFormatString { get; set; }

        /// <summary>
        /// Gets or sets the key for the tracker to use on this series.
        /// </summary>
        public string TrackerKey { get; set; }

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public abstract TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate);

        /// <summary>
        /// Gets the item of the specified index.
        /// Returns null if ItemsSource is not set, or the index is outside the boundaries.
        /// </summary>
        /// <param name="itemsSource">The items source.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected object GetItem(IEnumerable itemsSource, int index)
        {
            if (itemsSource == null || index < 0)
            {
                return null;
            }

            var list = itemsSource as IList;
            if (list != null)
            {
                if (index < list.Count && index >= 0)
                {
                    return list[index];
                }
                return null;
            }

            // todo: can this be improved?
            int i = 0;
            foreach (object item in itemsSource)
            {
                if (i++ == index)
                {
                    return item;
                }
            }

            return null;
        }       
    }
}