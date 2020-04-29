// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionAnnotationType.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines the definition of function in a <see cref="FunctionAnnotation" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    /// <summary>
    /// Defines the definition of function in a <see cref="FunctionAnnotation" />.
    /// </summary>
    public enum FunctionAnnotationType
    {
        /// <summary>
        /// Curve equation x=f(y) given by the Equation property
        /// </summary>
        EquationX,

        /// <summary>
        /// Curve equation y=f(x) given by the Equation property
        /// </summary>
        EquationY
    }
}