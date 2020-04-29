// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CursorType.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines the cursor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Defines the cursor type.
    /// </summary>
    public enum CursorType
    {
        /// <summary>
        /// The default cursor
        /// </summary>
        Default = 0,

        /// <summary>
        /// The pan cursor
        /// </summary>
        Pan,

        /// <summary>
        /// The zoom rectangle cursor
        /// </summary>
        ZoomRectangle,

        /// <summary>
        /// The horizontal zoom cursor
        /// </summary>
        ZoomHorizontal,

        /// <summary>
        /// The vertical zoom cursor
        /// </summary>
        ZoomVertical
    }
}