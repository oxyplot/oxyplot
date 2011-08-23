// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotation.cs" company="OxyPlot">
//   see http://oxyplot.codeplex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    /// <summary>
    /// The annotation interface.
    /// </summary>
    public interface IAnnotation
    {
        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        OxyPlot.Annotation CreateModel();

        #endregion
    }
}