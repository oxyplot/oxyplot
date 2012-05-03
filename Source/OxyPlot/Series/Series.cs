// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Series.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Abstract base class for all series.
    /// </summary>
    /// <remarks>
    /// This class contains internal methods that should be called only from the PlotModel.
    /// </remarks>
    [Serializable]
    public abstract class Series : UIPlotElement, ITrackableSeries
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="Series" /> class.
        /// </summary>
        protected Series()
        {
            this.IsVisible = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the actual culture.
        /// </summary>
        /// <remarks>
        ///   The culture is defined in the parent PlotModel.
        /// </remarks>
        public CultureInfo ActualCulture
        {
            get
            {
                return this.PlotModel != null ? this.PlotModel.ActualCulture : CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        ///   Gets or sets the background color of the series. The background area is defined by the x and y axes.
        /// </summary>
        /// <value> The background color. </value>
        public OxyColor Background { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this series is visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        ///   Gets or sets the title of the Series.
        /// </summary>
        /// <value> The title. </value>
        public string Title { get; set; }

        /// <summary>
        ///   Gets or sets a format string used for the tracker.
        /// </summary>
        public string TrackerFormatString { get; set; }

        /// <summary>
        ///   Gets or sets the key for the tracker to use on this series.
        /// </summary>
        public string TrackerKey { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>
        /// A TrackerHitResult for the current hit.
        /// </returns>
        public abstract TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate);

        /// <summary>
        /// Renders the series on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context. 
        /// </param>
        /// <param name="model">
        /// The model. 
        /// </param>
        public abstract void Render(IRenderContext rc, PlotModel model);

        /// <summary>
        /// Renders the legend symbol on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context. 
        /// </param>
        /// <param name="legendBox">
        /// The legend rectangle. 
        /// </param>
        public abstract void RenderLegend(IRenderContext rc, OxyRect legendBox);

        /// <summary>
        /// Tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>
        /// A hit test result.
        /// </returns>
        protected internal override HitTestResult HitTest(ScreenPoint point, double tolerance)
        {
            var thr = this.GetNearestPoint(point, true);
            if (thr != null)
            {
                double distance = thr.Position.DistanceTo(point);
                if (distance > tolerance)
                {
                    return null;
                }

                return new HitTestResult(thr.Position, thr.Item, thr.Index);
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if this data series requires X/Y axes. (e.g. Pie series do not require axes)
        /// </summary>
        /// <returns>
        /// True if no axes are required. 
        /// </returns>
        protected internal abstract bool AreAxesRequired();

        /// <summary>
        /// Ensures that the axes of the series is defined.
        /// </summary>
        protected internal abstract void EnsureAxes();

        /// <summary>
        /// Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">
        /// An axis which should be checked if used
        /// </param>
        /// <returns>
        /// True if the axis is in use. 
        /// </returns>
        protected internal abstract bool IsUsing(Axis axis);

        /// <summary>
        /// Sets default values (colors, line style etc) from the plotmodel.
        /// </summary>
        /// <param name="model">
        /// A plot model. 
        /// </param>
        protected internal abstract void SetDefaultValues(PlotModel model);

        /// <summary>
        /// Updates the axis maximum and minimum values.
        /// </summary>
        protected internal abstract void UpdateAxisMaxMin();

        /// <summary>
        /// Updates the data.
        /// </summary>
        protected internal abstract void UpdateData();

        /// <summary>
        /// Updates the valid data.
        /// </summary>
        protected internal abstract void UpdateValidData();

        /// <summary>
        /// Updates the maximum and minimum of the series.
        /// </summary>
        protected internal abstract void UpdateMaxMin();

        #endregion
    }
}