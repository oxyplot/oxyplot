// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodeGenerating.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides functionality to generate c# code of an object.
    /// </summary>
    public interface ICodeGenerating
    {
        #region Public Methods

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>
        /// C# code.
        /// </returns>
        string ToCode();

        #endregion
    }
}