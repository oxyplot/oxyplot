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
    public abstract class Annotation : IAnnotation
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
        ///   Gets the parent plot model.
        /// </summary>
        public PlotModel PlotModel { get; internal set; }

        /// <summary>
        ///   Annotation text.
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