// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineAnnotationType.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The line annotation type.
    /// </summary>
    public enum LineAnnotationType
    {
        /// <summary>
        ///   Horizontal line given by the Y property
        /// </summary>
        Horizontal, 

        /// <summary>
        ///   Vertical line given by the X property
        /// </summary>
        Vertical, 

        /// <summary>
        ///   Linear equation y=mx+b given by the Slope and Intercept properties
        /// </summary>
        LinearEquation, 

        /// <summary>
        ///   Curve equation x=f(y) given by the Equation property
        /// </summary>
        EquationX, 

        /// <summary>
        ///   Curve equation y=f(x) given by the Equation property
        /// </summary>
        EquationY
    }
}