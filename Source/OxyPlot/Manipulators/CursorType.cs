// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CursorType.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The oxy cursor.
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