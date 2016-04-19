// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Extension method used to convert to/from Windows/Windows.Media classes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.GtkSharp
{
    using System;
    using Cairo;

    using Gdk;

    using Point = Cairo.Point;
    using Rectangle = Cairo.Rectangle;

    /// <summary>
    /// Extension method used to convert to/from Windows/Windows.Media classes.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        /// Converts an <see cref="OxyPlot.LineJoin" /> to a <see cref="LineJoin" />.
        /// </summary>
        /// <param name="lineJoin">The <see cref="OxyPlot.LineJoin" /> to convert.</param>
        /// <returns>The converted value.</returns>
        public static LineJoin ToLineJoin(this OxyPlot.LineJoin lineJoin)
        {
            switch (lineJoin)
            {
                case OxyPlot.LineJoin.Round:
                    return LineJoin.Round;
                case OxyPlot.LineJoin.Bevel:
                    return LineJoin.Bevel;
                default:
                    return LineJoin.Miter;
            }
        }

        /// <summary>
        /// Sets the source color for the Cairo context.
        /// </summary>
        /// <param name="g">The Cairo context.</param>
        /// <param name="c">The color.</param>
        public static void SetSourceColor(this Context g, OxyColor c)
        {
            g.SetSourceRGBA(c.R / 256.0, c.G / 256.0, c.B / 256.0, c.A / 256.0);
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a Cairo <see cref="PointD" />.
        /// </summary>
        /// <param name="pt">The point to convert.</param>
        /// <param name="aliased">Alias if set to <c>true</c>.</param>
        /// <returns>The converted point.</returns>
        public static PointD ToPointD(this ScreenPoint pt, bool aliased)
        {
            if (aliased)
            {
                return new PointD(0.5 + (int)pt.X, 0.5 + (int)pt.Y);
            }

            return new PointD(pt.X, pt.Y);
        }

        /// <summary>
        /// Converts an <see cref="OxyRect" /> to a <see cref="Cairo.Rectangle" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <param name="aliased">Use pixel alignment if set to <c>true</c>.</param>
        /// <returns>The converted rectangle.</returns>
        public static Rectangle ToRect(this OxyRect r, bool aliased = false)
        {
            if (aliased)
            {
                var x = 0.5 + (int)r.Left;
                var y = 0.5 + (int)r.Top;
                var ri = 0.5 + (int)r.Right;
                var bo = 0.5 + (int)r.Bottom;
                return new Rectangle(x, y, ri - x, bo - y);
            }

            return new Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Converts an <see cref="Gdk.Rectangle" /> to a <see cref="OxyRect" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <param name="aliased">Use pixel alignment if set to <c>true</c>.</param>
        /// <returns>The converted rectangle.</returns>
        public static OxyRect ToOxyRect(this Gdk.Rectangle r, bool aliased = false)
        {
            if (aliased)
            {
                var x = 0.5 + (int)r.Left;
                var y = 0.5 + (int)r.Top;
                var ri = 0.5 + (int)r.Right;
                var bo = 0.5 + (int)r.Bottom;
                return new OxyRect(x, y, ri - x, bo - y);
            }

            return new OxyRect(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Converts a point to a ScreenPoint.
        /// </summary>
        /// <param name="pt">The point.</param>
        /// <returns>A screen point.</returns>
        public static ScreenPoint ToScreenPoint(this Point pt)
        {
            return new ScreenPoint(pt.X, pt.Y);
        }

        /// <summary>
        /// Creates the mouse down event arguments.
        /// </summary>
        /// <param name="e">The instance containing the event data.</param>
        /// <returns>Mouse event arguments.</returns>
        public static OxyMouseDownEventArgs ToMouseDownEventArgs(this EventButton e)
        {
            return new OxyMouseDownEventArgs
            {
                ChangedButton = ConvertButton(e),
                ClickCount = e.Type == EventType.ButtonPress ? 1 : e.Type == EventType.TwoButtonPress ? 2 : e.Type == EventType.ThreeButtonPress ? 3 : 1,
                Position = new ScreenPoint(e.X, e.Y),
                ModifierKeys = GetModifiers(e.State)
            };
        }

        /// <summary>
        /// Creates the mouse up event arguments.
        /// </summary>
        /// <param name="e">The instance containing the event data.</param>
        /// <returns>Mouse event arguments.</returns>
        public static OxyMouseEventArgs ToMouseUpEventArgs(this EventButton e)
        {
            return new OxyMouseEventArgs
            {
                Position = new ScreenPoint(e.X, e.Y),
                ModifierKeys = GetModifiers(e.State)
            };
        }

        /// <summary>
        /// Creates the mouse event arguments.
        /// </summary>
        /// <param name="e">The motion event args.</param>
        /// <returns>Mouse event arguments.</returns>
        public static OxyMouseEventArgs ToMouseEventArgs(this EventMotion e)
        {
            return new OxyMouseEventArgs
            {
                Position = new ScreenPoint(e.X, e.Y),
                ModifierKeys = GetModifiers(e.State)
            };
        }

        /// <summary>
        /// Creates the mouse event arguments for an enter/leave event.
        /// </summary>
        /// <param name="e">The event crossing args.</param>
        /// <returns>Mouse event arguments.</returns>
        public static OxyMouseEventArgs ToMouseEventArgs(this EventCrossing e)
        {
            return new OxyMouseEventArgs
            {
                Position = new ScreenPoint(e.X, e.Y),
                ModifierKeys = GetModifiers(e.State)
            };
        }

        /// <summary>
        /// Creates the mouse wheel event arguments.
        /// </summary>
        /// <param name="e">The scroll event args.</param>
        /// <returns>Mouse event arguments.</returns>
        public static OxyMouseWheelEventArgs ToMouseWheelEventArgs(this EventScroll e)
        {
            return new OxyMouseWheelEventArgs
            {
                Delta = e.Direction == ScrollDirection.Down ? -120 : 120,
                Position = new ScreenPoint(e.X, e.Y),
                ModifierKeys = GetModifiers(e.State)
            };
        }

        /// <summary>
        /// Creates the key event arguments.
        /// </summary>
        /// <param name="e">The key event args.</param>
        /// <returns>Key event arguments.</returns>
        public static OxyKeyEventArgs ToKeyEventArgs(this EventKey e)
        {
            return new OxyKeyEventArgs { ModifierKeys = GetModifiers(e.State), Key = e.Key.Convert() };
        }

        /// <summary>
        /// Converts the specified key.
        /// </summary>
        /// <param name="k">The key to convert.</param>
        /// <returns>The converted key.</returns>
        public static OxyKey Convert(this Key k)
        {
            switch (k)
            {
                case Key.A:
                case Key.a:
                    return OxyKey.A;
                case Key.plus:
                case Key.KP_Add:
                    return OxyKey.Add;
                case Key.B:
                case Key.b:
                    return OxyKey.B;
                case Key.BackSpace:
                    return OxyKey.Backspace;
                case Key.C:
                case Key.c:
                    return OxyKey.C;
                case Key.D:
                case Key.d:
                    return OxyKey.D;
                case Key.Key_0:
                    return OxyKey.D0;
                case Key.Key_1:
                    return OxyKey.D1;
                case Key.Key_2:
                    return OxyKey.D2;
                case Key.Key_3:
                    return OxyKey.D3;
                case Key.Key_4:
                    return OxyKey.D4;
                case Key.Key_5:
                    return OxyKey.D5;
                case Key.Key_6:
                    return OxyKey.D6;
                case Key.Key_7:
                    return OxyKey.D7;
                case Key.Key_8:
                    return OxyKey.D8;
                case Key.Key_9:
                    return OxyKey.D9;
                case Key.KP_Decimal:
                    return OxyKey.Decimal;
                case Key.Delete:
                case Key.KP_Delete:
                    return OxyKey.Delete;
                case Key.KP_Divide:
                    return OxyKey.Divide;
                case Key.Down:
                case Key.KP_Down:
                    return OxyKey.Down;
                case Key.E:
                case Key.e:
                    return OxyKey.E;
                case Key.End:
                case Key.KP_End:
                    return OxyKey.End;
                case Key.Return:
                case Key.KP_Enter:
                    return OxyKey.Enter;
                case Key.Escape:
                    return OxyKey.Escape;
                case Key.F:
                case Key.f:
                    return OxyKey.F;
                case Key.F1:
                    return OxyKey.F1;
                case Key.F10:
                    return OxyKey.F10;
                case Key.F11:
                    return OxyKey.F11;
                case Key.F12:
                    return OxyKey.F12;
                case Key.F2:
                    return OxyKey.F2;
                case Key.F3:
                    return OxyKey.F3;
                case Key.F4:
                    return OxyKey.F4;
                case Key.F5:
                    return OxyKey.F5;
                case Key.F6:
                    return OxyKey.F6;
                case Key.F7:
                    return OxyKey.F7;
                case Key.F8:
                    return OxyKey.F8;
                case Key.F9:
                    return OxyKey.F9;
                case Key.G:
                case Key.g:
                    return OxyKey.G;
                case Key.H:
                case Key.h:
                    return OxyKey.H;
                case Key.Home:
                case Key.KP_Home:
                    return OxyKey.Home;
                case Key.I:
                case Key.i:
                    return OxyKey.I;
                case Key.Insert:
                    return OxyKey.Insert;
                case Key.J:
                case Key.j:
                    return OxyKey.J;
                case Key.K:
                case Key.k:
                    return OxyKey.K;
                case Key.L:
                case Key.l:
                    return OxyKey.L;
                case Key.Left:
                    return OxyKey.Left;
                case Key.M:
                case Key.m:
                    return OxyKey.M;
                case Key.asterisk:
                case Key.KP_Multiply:
                    return OxyKey.Multiply;
                case Key.N:
                case Key.n:
                    return OxyKey.N;
                case Key.KP_0:
                    return OxyKey.NumPad0;
                case Key.KP_1:
                    return OxyKey.NumPad1;
                case Key.KP_2:
                    return OxyKey.NumPad2;
                case Key.KP_3:
                    return OxyKey.NumPad3;
                case Key.KP_4:
                    return OxyKey.NumPad4;
                case Key.KP_5:
                    return OxyKey.NumPad5;
                case Key.KP_6:
                    return OxyKey.NumPad6;
                case Key.KP_7:
                    return OxyKey.NumPad7;
                case Key.KP_8:
                    return OxyKey.NumPad8;
                case Key.KP_9:
                    return OxyKey.NumPad9;
                case Key.O:
                case Key.o:
                    return OxyKey.O;
                case Key.P:
                case Key.p:
                    return OxyKey.P;
                case Key.Page_Down:
                    return OxyKey.PageDown;
                case Key.Page_Up:
                    return OxyKey.PageUp;
                case Key.Q:
                case Key.q:
                    return OxyKey.Q;
                case Key.R:
                case Key.r:
                    return OxyKey.R;
                case Key.Right:
                    return OxyKey.Right;
                case Key.S:
                case Key.s:
                    return OxyKey.S;
                case Key.space:
                case Key.KP_Space:
                    return OxyKey.Space;
                case Key.minus:
                case Key.KP_Subtract:
                    return OxyKey.Subtract;
                case Key.T:
                case Key.t:
                    return OxyKey.T;
                case Key.Tab:
                case Key.KP_Tab:
                    return OxyKey.Tab;
                case Key.U:
                case Key.u:
                    return OxyKey.U;
                case Key.Up:
                    return OxyKey.Up;
                case Key.V:
                case Key.v:
                    return OxyKey.V;
                case Key.W:
                case Key.w:
                    return OxyKey.W;
                case Key.X:
                case Key.x:
                    return OxyKey.X;
                case Key.Y:
                case Key.y:
                    return OxyKey.Y;
                case Key.Z:
                case Key.z:
                    return OxyKey.Z;
                default:
                    return OxyKey.Unknown;
            }
        }

        /// <summary>
        /// Converts the changed button.
        /// </summary>
        /// <param name="e">The instance containing the event data.</param>
        /// <returns>The mouse button.</returns>
        private static OxyMouseButton ConvertButton(EventButton e)
        {
            switch (e.Button)
            {
                case 1:
                    return OxyMouseButton.Left;
                case 2:
                    return OxyMouseButton.Middle;
                case 3:
                    return OxyMouseButton.Right;
                case 4:
                    return OxyMouseButton.XButton1;
                case 5:
                    return OxyMouseButton.XButton2;
            }

            return OxyMouseButton.Left;
        }

        /// <summary>
        /// Converts a <see cref="ModifierType" /> to a <see cref="OxyModifierKeys" />.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>The modifier keys.</returns>
        private static OxyModifierKeys GetModifiers(ModifierType state)
        {
            var result = OxyModifierKeys.None;

            if ((state & ModifierType.ShiftMask) != 0)
            {
                result |= OxyModifierKeys.Shift;
            }

            if ((state & ModifierType.ControlMask) != 0)
            {
                result |= OxyModifierKeys.Control;
            }

            if ((state & ModifierType.Mod1Mask) != 0)
            {
                result |= OxyModifierKeys.Alt;
            }

            return result;
        }
    }
}