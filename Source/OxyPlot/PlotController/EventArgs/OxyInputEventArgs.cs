// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyInputEventArgs.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2014 OxyPlot contributors
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides a base class for classes that contain event data for input events.
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