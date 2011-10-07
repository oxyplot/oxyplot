//-----------------------------------------------------------------------
// <copyright file="IAnnotation.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// The annotation layer.
    /// </summary>
    public enum AnnotationLayer
    {
        /// <summary>
        ///   The below series.
        /// </summary>
        BelowSeries, 

        /// <summary>
        ///   The over series.
        /// </summary>
        OverSeries
    }

    /// <summary>
    /// The i annotation.
    /// </summary>
    public interface IAnnotation
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets Layer.
        /// </summary>
        AnnotationLayer Layer { get; set; }

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
        void EnsureAxes(Collection<Axis> axes, IAxis defaultXAxis, IAxis defaultYAxis);

        /// <summary>
        /// The render.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        void Render(IRenderContext rc, PlotModel model);

        #endregion
    }
}
