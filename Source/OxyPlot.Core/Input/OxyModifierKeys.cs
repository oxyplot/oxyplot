// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyModifierKeys.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines the set of modifier keys.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Defines the set of modifier keys.
    /// </summary>
    [Flags]
    public enum OxyModifierKeys
    {
        /// <summary>
        /// No modifiers are pressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// The Control key.
        /// </summary>
        Control = 1,

        /// <summary>
        /// The Alt/Menu key.
        /// </summary>
        Alt = 2,

        /// <summary>
        /// The Shift key.
        /// </summary>
        Shift = 4,

        /// <summary>
        /// The Windows key.
        /// </summary>
        Windows = 8
    }
}