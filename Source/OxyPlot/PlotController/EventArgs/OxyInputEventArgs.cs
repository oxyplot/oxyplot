// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyInputEventArgs.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for classes that contain event data for input events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides an abstract base class for classes that contain event data for input events.
    /// </summary>
    public abstract class OxyInputEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the event was handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets the modifier keys.
        /// </summary>
        public OxyModifierKeys ModifierKeys { get; set; }

        /// <summary>
        /// Gets a value indicating whether the alt key was pressed when the event was raised.
        /// </summary>
        public bool IsAltDown
        {
            get
            {
                return (this.ModifierKeys & OxyModifierKeys.Alt) == OxyModifierKeys.Alt;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control key was pressed when the event was raised.
        /// </summary>
        public bool IsControlDown
        {
            get
            {
                return (this.ModifierKeys & OxyModifierKeys.Control) == OxyModifierKeys.Control;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the shift key was pressed when the event was raised.
        /// </summary>
        public bool IsShiftDown
        {
            get
            {
                return (this.ModifierKeys & OxyModifierKeys.Shift) == OxyModifierKeys.Shift;
            }
        }
    }
}