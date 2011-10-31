// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotation.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
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