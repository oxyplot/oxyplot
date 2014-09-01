// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Keyboard.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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