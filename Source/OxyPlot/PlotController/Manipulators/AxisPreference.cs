// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisPreference.cs" company="OxyPlot">
//   Copyright (c) 2022 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies which axis a manipulator will prefer to operate on.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies which axis a manipulator will prefer to operate on.
    /// </summary>
    public enum AxisPreference
    {
        /// <summary>
        /// Manipulation will not prefer a particular axis.
        /// </summary>
        None,

        /// <summary>
        /// Manipulation will prefer to operate on the X axis.
        /// </summary>
        X,

        /// <summary>
        /// Manipulation will prefer to operate on the Y axis.
        /// </summary>
        Y,
    }
}
