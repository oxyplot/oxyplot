using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace OxyPlot
{
    public abstract class PlotSeriesBase : ISeries
    {
        /// <summary>
        ///   Gets or sets the X axis.
        /// </summary>
        /// <value>The X axis.</value>
        public IAxis XAxis { get; set; }

        /// <summary>
        ///   Gets or sets the Y axis.
        /// </summary>
        /// <value>The Y axis.</value>
        public IAxis YAxis { get; set; }

        /// <summary>
        ///   Gets or sets the X axis key.
        /// </summary>
        /// <value>The X axis key.</value>
        public string XAxisKey { get; set; }

        /// <summary>
        ///   Gets or sets the Y axis key.
        /// </summary>
        /// <value>The Y axis key.</value>
        public string YAxisKey { get; set; }

        /// <summary>
        ///   Gets or sets the min X of the dataset.
        /// </summary>
        /// <value>The min X.</value>
        public double MinX { get; protected set; }

        /// <summary>
        ///   Gets or sets the max X of the dataset.
        /// </summary>
        /// <value>The max X.</value>
        public double MaxX { get; protected set; }

        /// <summary>
        ///   Gets or sets the min Y of the dataset.
        /// </summary>
        /// <value>The min Y.</value>
        public double MinY { get; protected set; }

        /// <summary>
        ///   Gets or sets the max Y of the dataset.
        /// </summary>
        /// <value>The max Y.</value>
        public double MaxY { get; protected set; }

        /// <summary>
        ///   Gets or sets the background of the series.
        ///   The background area is defined by the x and y axes.
        /// </summary>
        /// <value>The background.</value>
        public OxyColor Background { get; set; }


        #region ISeries Members

        /// <summary>
        ///   Gets the title of the Series.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        ///   Renders the Series on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "model">The model.</param>
        public virtual void Render(IRenderContext rc, PlotModel model)
        {
        }

        /// <summary>
        ///   Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "legendBox">The rect.</param>
        public virtual void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
        }

        public virtual void UpdateData()
        {
        }

        public virtual bool AreAxesRequired()
        {
            return true;
        }

        public void EnsureAxes(Collection<IAxis> axes, IAxis defaultXAxis, IAxis defaultYAxis)
        {
            if (XAxisKey != null)
            {
                XAxis = axes.FirstOrDefault(a => a.Key == XAxisKey);
            }

            if (YAxisKey != null)
            {
                YAxis = axes.FirstOrDefault(a => a.Key == YAxisKey);
            }

            // If axes are not found, use the default axes
            if (XAxis == null)
            {
                XAxis = defaultXAxis;
            }

            if (YAxis == null)
            {
                YAxis = defaultYAxis;
            }
        }

        /// <summary>
        /// Gets the rectangle the series uses on the screen (screen coordinates).
        /// </summary>
        /// <returns></returns>
        public OxyRect GetScreenRectangle()
        {
            return GetClippingRect();
        }

        protected OxyRect GetClippingRect()
        {
            var minX = Math.Min(XAxis.ScreenMin.X, XAxis.ScreenMax.X);
            var minY = Math.Min(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y);
            var maxX = Math.Max(XAxis.ScreenMin.X, XAxis.ScreenMax.X);
            var maxY = Math.Max(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y);

            return new OxyRect(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        ///   Updates the max/min values.
        /// </summary>
        public virtual void UpdateMaxMin()
        {
            MinX = MinY = MaxX = MaxY = double.NaN;
        }

        public abstract bool GetNearestPoint(ScreenPoint point, out DataPoint dpn, out ScreenPoint spn);

        public virtual void SetDefaultValues(PlotModel model)
        {
        }

        public abstract bool GetNearestInterpolatedPoint(ScreenPoint point, out DataPoint dpn, out ScreenPoint spn);

        #endregion

    }
}