// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Annotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for annotations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    using System;

    using OxyPlot.Axes;

    /// <summary>
    /// Provides an abstract base class for annotations.
    /// </summary>
    public abstract class Annotation : PlotElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Annotation" /> class.
        /// </summary>
        protected Annotation()
        {
            this.Layer = AnnotationLayer.AboveSeries;
        }

        /// <summary>
        /// Gets or sets the rendering layer of the annotation. The default value is <see cref="AnnotationLayer.AboveSeries" />.
        /// </summary>
        public AnnotationLayer Layer { get; set; }

        /// <summary>
        /// Gets the X axis.
        /// </summary>
        /// <value>The X axis.</value>
        public Axis XAxis { get; private set; }

        /// <summary>
        /// Gets or sets the X axis key.
        /// </summary>
        /// <value>The X axis key.</value>
        public string XAxisKey { get; set; }

        /// <summary>
        /// Gets the Y axis.
        /// </summary>
        /// <value>The Y axis.</value>
        public Axis YAxis { get; private set; }

        /// <summary>
        /// Gets or sets the Y axis key.
        /// </summary>
        /// <value>The Y axis key.</value>
        public string YAxisKey { get; set; }

        /// <summary>
        /// Ensures that the annotation axes are set.
        /// </summary>
        public void EnsureAxes()
        {
            this.XAxis = this.PlotModel.GetAxisOrDefault(this.XAxisKey, this.PlotModel.DefaultXAxis);
            this.YAxis = this.PlotModel.GetAxisOrDefault(this.YAxisKey, this.PlotModel.DefaultYAxis);
        }

        /// <summary>
        /// Renders the annotation on the specified context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public virtual void Render(IRenderContext rc)
        {
        }

        /// <summary>
        /// Transforms the specified coordinates to a screen point.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A screen point.</returns>
        public ScreenPoint Transform(double x, double y)
        {
            return this.XAxis.Transform(x, y, this.YAxis);
        }

        /// <summary>
        /// Transforms the specified data point to a screen point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>A screen point.</returns>
        public ScreenPoint Transform(DataPoint p)
        {
            return this.XAxis.Transform(p.X, p.Y, this.YAxis);
        }

        /// <summary>
        /// Transforms the specified screen position to a data point.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>A data point</returns>
        public DataPoint InverseTransform(ScreenPoint position)
        {
            return this.XAxis.InverseTransform(position.X, position.Y, this.YAxis);
        }

        /// <summary>
        /// Tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// A hit test result.
        /// </returns>
        protected override HitTestResult HitTestOverride(HitTestArguments args)
        {
            return null;
        }

        /// <summary>
        /// Gets the clipping rectangle.
        /// </summary>
        /// <returns>The clipping rectangle.</returns>
        protected OxyRect GetClippingRect()
        {
            double minX = Math.Min(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double minY = Math.Min(this.YAxis.ScreenMin.Y, this.YAxis.ScreenMax.Y);
            double maxX = Math.Max(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double maxY = Math.Max(this.YAxis.ScreenMin.Y, this.YAxis.ScreenMax.Y);

            return new OxyRect(minX, minY, maxX - minX, maxY - minY);
        }
    }
}