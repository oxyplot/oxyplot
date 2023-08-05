// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowsFormsConverterExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Extension method used to convert to/from Windows/Windows.Media classes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WindowsForms
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Extension method used to convert to/from WindowsForms classes.
    /// </summary>
    public static class WindowsFormsConverterExtensions
    {
        /// <summary>
        /// Converts a <see cref="MouseButtons" /> to a <see cref="OxyMouseButton" />.
        /// </summary>
        /// <param name="button">The button to convert.</param>
        /// <returns>The converted mouse button.</returns>
        public static OxyMouseButton Convert(this MouseButtons button)
        {
            return button switch
            {
                MouseButtons.Left => OxyMouseButton.Left,
                MouseButtons.Middle => OxyMouseButton.Middle,
                MouseButtons.Right => OxyMouseButton.Right,
                MouseButtons.XButton1 => OxyMouseButton.XButton1,
                MouseButtons.XButton2 => OxyMouseButton.XButton2,
                _ => OxyMouseButton.None,
            };
        }

        /// <summary>
        /// Converts <see cref="MouseEventArgs" /> to <see cref="OxyMouseWheelEventArgs" /> for a mouse wheel event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <returns>A <see cref="OxyMouseWheelEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseWheelEventArgs ToMouseWheelEventArgs(this MouseEventArgs e, OxyModifierKeys modifiers)
        {
            return new OxyMouseWheelEventArgs
            {
                Position = e.Location.ToScreenPoint(),
                ModifierKeys = modifiers,
                Delta = e.Delta
            };
        }

        /// <summary>
        /// Converts <see cref="MouseEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a mouse down event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <returns>A <see cref="OxyMouseDownEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseDownEventArgs ToMouseDownEventArgs(this MouseEventArgs e, OxyModifierKeys modifiers)
        {
            return new OxyMouseDownEventArgs
            {
                ChangedButton = e.Button.Convert(),
                ClickCount = e.Clicks,
                Position = e.Location.ToScreenPoint(),
                ModifierKeys = modifiers
            };
        }

        /// <summary>
        /// Converts <see cref="MouseEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a mouse up event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseEventArgs ToMouseUpEventArgs(this MouseEventArgs e, OxyModifierKeys modifiers)
        {
            return new OxyMouseEventArgs
            {
                Position = e.Location.ToScreenPoint(),
                ModifierKeys = modifiers
            };
        }

        /// <summary>
        /// Converts <see cref="MouseEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a mouse event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseEventArgs ToMouseEventArgs(this MouseEventArgs e, OxyModifierKeys modifiers)
        {
            return new OxyMouseEventArgs
            {
                Position = e.Location.ToScreenPoint(),
                ModifierKeys = modifiers
            };
        }

        /// <summary>
        /// Converts <see cref="MouseEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a mouse event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseEventArgs ToMouseEventArgs(this EventArgs e, OxyModifierKeys modifiers)
        {
            return new OxyMouseEventArgs
            {
                ModifierKeys = modifiers
            };
        }

        /// <summary>
        /// Converts the specified key.
        /// </summary>
        /// <param name="k">The key to convert.</param>
        /// <returns>The converted key.</returns>
        public static OxyKey Convert(this Keys k)
        {
            return k switch
            {
                Keys.A => OxyKey.A,
                Keys.Add => OxyKey.Add,
                Keys.B => OxyKey.B,
                Keys.Back => OxyKey.Backspace,
                Keys.C => OxyKey.C,
                Keys.D => OxyKey.D,
                Keys.D0 => OxyKey.D0,
                Keys.D1 => OxyKey.D1,
                Keys.D2 => OxyKey.D2,
                Keys.D3 => OxyKey.D3,
                Keys.D4 => OxyKey.D4,
                Keys.D5 => OxyKey.D5,
                Keys.D6 => OxyKey.D6,
                Keys.D7 => OxyKey.D7,
                Keys.D8 => OxyKey.D8,
                Keys.D9 => OxyKey.D9,
                Keys.Decimal => OxyKey.Decimal,
                Keys.Delete => OxyKey.Delete,
                Keys.Divide => OxyKey.Divide,
                Keys.Down => OxyKey.Down,
                Keys.E => OxyKey.E,
                Keys.End => OxyKey.End,
                Keys.Enter => OxyKey.Enter,
                Keys.Escape => OxyKey.Escape,
                Keys.F => OxyKey.F,
                Keys.F1 => OxyKey.F1,
                Keys.F10 => OxyKey.F10,
                Keys.F11 => OxyKey.F11,
                Keys.F12 => OxyKey.F12,
                Keys.F2 => OxyKey.F2,
                Keys.F3 => OxyKey.F3,
                Keys.F4 => OxyKey.F4,
                Keys.F5 => OxyKey.F5,
                Keys.F6 => OxyKey.F6,
                Keys.F7 => OxyKey.F7,
                Keys.F8 => OxyKey.F8,
                Keys.F9 => OxyKey.F9,
                Keys.G => OxyKey.G,
                Keys.H => OxyKey.H,
                Keys.Home => OxyKey.Home,
                Keys.I => OxyKey.I,
                Keys.Insert => OxyKey.Insert,
                Keys.J => OxyKey.J,
                Keys.K => OxyKey.K,
                Keys.L => OxyKey.L,
                Keys.Left => OxyKey.Left,
                Keys.M => OxyKey.M,
                Keys.Multiply => OxyKey.Multiply,
                Keys.N => OxyKey.N,
                Keys.NumPad0 => OxyKey.NumPad0,
                Keys.NumPad1 => OxyKey.NumPad1,
                Keys.NumPad2 => OxyKey.NumPad2,
                Keys.NumPad3 => OxyKey.NumPad3,
                Keys.NumPad4 => OxyKey.NumPad4,
                Keys.NumPad5 => OxyKey.NumPad5,
                Keys.NumPad6 => OxyKey.NumPad6,
                Keys.NumPad7 => OxyKey.NumPad7,
                Keys.NumPad8 => OxyKey.NumPad8,
                Keys.NumPad9 => OxyKey.NumPad9,
                Keys.O => OxyKey.O,
                Keys.P => OxyKey.P,
                Keys.PageDown => OxyKey.PageDown,
                Keys.PageUp => OxyKey.PageUp,
                Keys.Q => OxyKey.Q,
                Keys.R => OxyKey.R,
                Keys.Right => OxyKey.Right,
                Keys.S => OxyKey.S,
                Keys.Space => OxyKey.Space,
                Keys.Subtract => OxyKey.Subtract,
                Keys.T => OxyKey.T,
                Keys.Tab => OxyKey.Tab,
                Keys.U => OxyKey.U,
                Keys.Up => OxyKey.Up,
                Keys.V => OxyKey.V,
                Keys.W => OxyKey.W,
                Keys.X => OxyKey.X,
                Keys.Y => OxyKey.Y,
                Keys.Z => OxyKey.Z,
                _ => OxyKey.Unknown,
            };
        }
    }
}
