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
            switch (button)
            {
                case MouseButtons.Left:
                    return OxyMouseButton.Left;
                case MouseButtons.Middle:
                    return OxyMouseButton.Middle;
                case MouseButtons.Right:
                    return OxyMouseButton.Right;
                case MouseButtons.XButton1:
                    return OxyMouseButton.XButton1;
                case MouseButtons.XButton2:
                    return OxyMouseButton.XButton2;
            }

            return OxyMouseButton.None;
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
            switch (k)
            {
                case Keys.A:
                    return OxyKey.A;
                case Keys.Add:
                    return OxyKey.Add;
                case Keys.B:
                    return OxyKey.B;
                case Keys.Back:
                    return OxyKey.Backspace;
                case Keys.C:
                    return OxyKey.C;
                case Keys.D:
                    return OxyKey.D;
                case Keys.D0:
                    return OxyKey.D0;
                case Keys.D1:
                    return OxyKey.D1;
                case Keys.D2:
                    return OxyKey.D2;
                case Keys.D3:
                    return OxyKey.D3;
                case Keys.D4:
                    return OxyKey.D4;
                case Keys.D5:
                    return OxyKey.D5;
                case Keys.D6:
                    return OxyKey.D6;
                case Keys.D7:
                    return OxyKey.D7;
                case Keys.D8:
                    return OxyKey.D8;
                case Keys.D9:
                    return OxyKey.D9;
                case Keys.Decimal:
                    return OxyKey.Decimal;
                case Keys.Delete:
                    return OxyKey.Delete;
                case Keys.Divide:
                    return OxyKey.Divide;
                case Keys.Down:
                    return OxyKey.Down;
                case Keys.E:
                    return OxyKey.E;
                case Keys.End:
                    return OxyKey.End;
                case Keys.Enter:
                    return OxyKey.Enter;
                case Keys.Escape:
                    return OxyKey.Escape;
                case Keys.F:
                    return OxyKey.F;
                case Keys.F1:
                    return OxyKey.F1;
                case Keys.F10:
                    return OxyKey.F10;
                case Keys.F11:
                    return OxyKey.F11;
                case Keys.F12:
                    return OxyKey.F12;
                case Keys.F2:
                    return OxyKey.F2;
                case Keys.F3:
                    return OxyKey.F3;
                case Keys.F4:
                    return OxyKey.F4;
                case Keys.F5:
                    return OxyKey.F5;
                case Keys.F6:
                    return OxyKey.F6;
                case Keys.F7:
                    return OxyKey.F7;
                case Keys.F8:
                    return OxyKey.F8;
                case Keys.F9:
                    return OxyKey.F9;
                case Keys.G:
                    return OxyKey.G;
                case Keys.H:
                    return OxyKey.H;
                case Keys.Home:
                    return OxyKey.Home;
                case Keys.I:
                    return OxyKey.I;
                case Keys.Insert:
                    return OxyKey.Insert;
                case Keys.J:
                    return OxyKey.J;
                case Keys.K:
                    return OxyKey.K;
                case Keys.L:
                    return OxyKey.L;
                case Keys.Left:
                    return OxyKey.Left;
                case Keys.M:
                    return OxyKey.M;
                case Keys.Multiply:
                    return OxyKey.Multiply;
                case Keys.N:
                    return OxyKey.N;
                case Keys.NumPad0:
                    return OxyKey.NumPad0;
                case Keys.NumPad1:
                    return OxyKey.NumPad1;
                case Keys.NumPad2:
                    return OxyKey.NumPad2;
                case Keys.NumPad3:
                    return OxyKey.NumPad3;
                case Keys.NumPad4:
                    return OxyKey.NumPad4;
                case Keys.NumPad5:
                    return OxyKey.NumPad5;
                case Keys.NumPad6:
                    return OxyKey.NumPad6;
                case Keys.NumPad7:
                    return OxyKey.NumPad7;
                case Keys.NumPad8:
                    return OxyKey.NumPad8;
                case Keys.NumPad9:
                    return OxyKey.NumPad9;
                case Keys.O:
                    return OxyKey.O;
                case Keys.P:
                    return OxyKey.P;
                case Keys.PageDown:
                    return OxyKey.PageDown;
                case Keys.PageUp:
                    return OxyKey.PageUp;
                case Keys.Q:
                    return OxyKey.Q;
                case Keys.R:
                    return OxyKey.R;
                case Keys.Right:
                    return OxyKey.Right;
                case Keys.S:
                    return OxyKey.S;
                case Keys.Space:
                    return OxyKey.Space;
                case Keys.Subtract:
                    return OxyKey.Subtract;
                case Keys.T:
                    return OxyKey.T;
                case Keys.Tab:
                    return OxyKey.Tab;
                case Keys.U:
                    return OxyKey.U;
                case Keys.Up:
                    return OxyKey.Up;
                case Keys.V:
                    return OxyKey.V;
                case Keys.W:
                    return OxyKey.W;
                case Keys.X:
                    return OxyKey.X;
                case Keys.Y:
                    return OxyKey.Y;
                case Keys.Z:
                    return OxyKey.Z;
                default:
                    return OxyKey.Unknown;
            }
        }
    }
}
