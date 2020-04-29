// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectionMode.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines the mode of selection used by <see cref="SelectableElement" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Defines the mode of selection used by <see cref="Element.SelectionMode" />.
    /// </summary>
    public enum SelectionMode
    {
        /// <summary>
        /// All the elements will be selected
        /// </summary>
        All,

        /// <summary>
        /// A single element will be selected
        /// </summary>
        Single,

        /// <summary>
        /// Multiple elements can be selected
        /// </summary>
        Multiple
    }
}
