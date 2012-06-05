// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStackableSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Specifies a series that can be stacked.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies a series that can be stacked.
    /// </summary>
    public interface IStackableSeries
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this series is stacked.
        /// </summary>
        bool IsStacked { get; }

        /// <summary>
        /// Gets the stack group.
        /// </summary>
        /// <value>
        /// The stack group. 
        /// </value>
        string StackGroup { get; }

        #endregion
    }
}