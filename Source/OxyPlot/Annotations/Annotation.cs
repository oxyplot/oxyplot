// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Annotation.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Annotation base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Annotation base class.
    /// </summary>
    [Serializable]
    public abstract class Annotation : IAnnotation
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets Layer.
        /// </summary>
        public AnnotationLayer Layer { get; set; }

        /// <summary>
        /// Annotation text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///   Gets or sets the X axis.
        /// </summary>
        /// <value>The X axis.</value>
        public IAxis XAxis { get; set; }

        /// <summary>
        ///   Gets or sets the X axis key.
        /// </summary>
        /// <value>The X axis key.</value>
        public string XAxisKey { get; set; }

        /// <summary>
        ///   Gets or sets the Y axis.
        /// </summary>
        /// <value>The Y axis.</value>
        public IAxis YAxis { get; set; }

        /// <summary>
        ///   Gets or sets the Y axis key.
        /// </summary>
        /// <value>The Y axis key.</value>
        public string YAxisKey { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The ensure axes.
        /// </summary>
        /// <param name="axes">
        /// The axes.
        /// </param>
        /// <param name="defaultXAxis">
        /// The default x axis.
        /// </param>
        /// <param name="defaultYAxis">
        /// The default y axis.
        /// </param>
        public void EnsureAxes(Collection<Axis> axes, IAxis defaultXAxis, IAxis defaultYAxis)
        {
            // todo: refactor - this code is shared with DataPointSeries
            if (this.XAxisKey != null)
            {
                this.XAxis = axes.FirstOrDefault(a => a.Key == this.XAxisKey);
            }

            if (this.YAxisKey != null)
            {
                this.YAxis = axes.FirstOrDefault(a => a.Key == this.YAxisKey);
            }

            // If axes are not found, use the default axes
            if (this.XAxis == null)
            {
                this.XAxis = defaultXAxis;
            }

            if (this.YAxis == null)
            {
                this.YAxis = defaultYAxis;
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

        #endregion
    }
}