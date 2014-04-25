// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Keyboard.cs" company="OxyPlot">
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
//   Provides utility methods related to the keyboard.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WP8
{
    using System.Windows.Input;

    /// <summary>
    /// Provides utility methods related to the keyboard.
    /// </summary>
    internal static class Keyboard
    {
        /// <summary>
        /// Gets the current modifier keys.
        /// </summary>
        /// <returns>A <see cref="OxyModifierKeys" /> value.</returns>
        public static OxyModifierKeys GetModifierKeys()
        {
            var modifiers = OxyModifierKeys.None;
            var keys = System.Windows.Input.Keyboard.Modifiers;
            if ((keys & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                modifiers |= OxyModifierKeys.Shift;
            }

            if ((keys & ModifierKeys.Control) == ModifierKeys.Control)
            {
                modifiers |= OxyModifierKeys.Control;
            }

            if ((keys & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                modifiers |= OxyModifierKeys.Alt;
            }

            if ((keys & ModifierKeys.Windows) == ModifierKeys.Windows)
            {
                modifiers |= OxyModifierKeys.Windows;
            }

            return modifiers;
        }
    }
}