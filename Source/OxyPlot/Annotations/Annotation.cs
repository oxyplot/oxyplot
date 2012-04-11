// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Annotation.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Annotation base class.
    /// </summary>
    [Serializable]
    public abstract class Annotation : PlotElement
    {
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
        ///   Gets or sets the layer.
        /// </summary>
        public AnnotationLayer Layer { get; set; }

        /// <summary>
        /// Gets or sets the annotation text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        ///   Gets or sets the X axis.
        /// </summary>
        /// <value>The X axis.</value>
        public Axis XAxis { get; set; }

        /// <summary>
        ///   Gets or sets the X axis key.
        /// </summary>
        /// <value>The X axis key.</value>
        public string XAxisKey { get; set; }

        /// <summary>
        ///   Gets or sets the Y axis.
        /// </summary>
        /// <value>The Y axis.</value>
        public Axis YAxis { get; set; }

        /// <summary>
        ///   Gets or sets the Y axis key.
        /// </summary>
        /// <value>The Y axis key.</value>
        public string YAxisKey { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Ensures that the annotation axes are set.
        /// </summary>
        public void EnsureAxes()
        {
            // todo: refactor - this code is shared with DataPointSeries
            if (this.XAxisKey != null)
            {
                this.XAxis = PlotModel.Axes.FirstOrDefault(a => a.Key == this.XAxisKey);
            }

            if (this.YAxisKey != null)
            {
                this.YAxis = PlotModel.Axes.FirstOrDefault(a => a.Key == this.YAxisKey);
            }

            // If axes are not found, use the default axes
            if (this.XAxis == null)
            {
                this.XAxis = PlotModel.DefaultXAxis;
            }

            if (this.YAxis == null)
            {
                this.YAxis = PlotModel.DefaultYAxis;
            }
        }

        /// <summary>
        /// The render.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public virtual void Render(IRenderContext rc, PlotModel model)
        {
        }

        /// <summary>
        /// Transforms the specified coordinates to a screen point.
        /// </summary>
        /// <param name="x">
        /// The x coordinate. 
        /// </param>
        /// <param name="y">
        /// The y coordinate. 
        /// </param>
        /// <returns>
        /// A screen point. 
        /// </returns>
        protected ScreenPoint Transform(double x, double y)
        {
            return this.XAxis.Transform(x, y, this.YAxis);
        }

        /// <summary>
        /// Transforms the specified data point to a screen point.
        /// </summary>
        /// <param name="p">
        /// The point. 
        /// </param>
        /// <returns>
        /// A screen point. 
        /// </returns>
        protected ScreenPoint Transform(IDataPoint p)
        {
            return this.XAxis.Transform(p.X, p.Y, this.YAxis);
        }

        /// <summary>
        /// Gets the clipping rectangle.
        /// </summary>
        /// <returns>
        /// The clipping rectangle.
        /// </returns>
        protected OxyRect GetClippingRect()
        {
            double minX = Math.Min(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double minY = Math.Min(this.YAxis.ScreenMin.Y, this.YAxis.ScreenMax.Y);
            double maxX = Math.Max(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double maxY = Math.Max(this.YAxis.ScreenMin.Y, this.YAxis.ScreenMax.Y);

            return new OxyRect(minX, minY, maxX - minX, maxY - minY);
        }
        #endregion
    }
}