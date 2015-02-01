// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Conversions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Xwt;
using Xwt.Drawing;

namespace OxyPlot.Xwt
{
    /// <summary>
    /// Provides conversions from and to Xwt structures.
    /// </summary>
    public static class Conversions
    {
        /// <summary>
        /// Converts an <see cref="OxyRect" /> to a <see cref="Rectangle" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <param name="aliased">Use pixel alignment if set to <c>true</c>.</param>
        /// <returns>The converted rectangle.</returns>
        public static Rectangle ToXwtRect (this OxyRect r, bool aliased = false)
        {
            if (aliased) {
                var x = 0.5 + (int)r.Left;
                var y = 0.5 + (int)r.Top;
                var ri = 0.5 + (int)r.Right;
                var bo = 0.5 + (int)r.Bottom;
                return new Rectangle (x, y, ri - x, bo - y);
            }

            return new Rectangle (r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Converts an <see cref="Rectangle" /> to a <see cref="OxyRect" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <param name="aliased">Use pixel alignment if set to <c>true</c>.</param>
        /// <returns>The converted rectangle.</returns>
        public static OxyRect ToOxyRect (this Rectangle r, bool aliased = false)
        {
            if (aliased) {
                var x = 0.5 + (int)r.Left;
                var y = 0.5 + (int)r.Top;
                var ri = 0.5 + (int)r.Right;
                var bo = 0.5 + (int)r.Bottom;
                return new OxyRect (x, y, ri - x, bo - y);
            }

            return new OxyRect (r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Converts a <see cref="OxyColor" /> to a <see cref="Color" />.
        /// </summary>
        /// <returns>The Xwt color.</returns>
        /// <param name="c">The Oxy color.</param>
        public static Color ToXwtColor (this OxyColor c)
        {
			return Color.FromBytes (c.R, c.G, c.B, c.A);
		}

		/// <summary>
		/// Converts a <see cref="Color" /> to a <see cref="OxyColor" />.
		/// </summary>
		/// <returns>The Oxy color.</returns>
		/// <param name="c">The Xwt color.</param>
		public static OxyColor ToOxyColor (this Color c)
		{
			return OxyColor.FromArgb ((byte)(c.Alpha * 255.0),
			                          (byte)(c.Red * 255.0),
			                          (byte)(c.Green * 255.0),
			                          (byte)(c.Blue * 255.0));
		}

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a Xwt <see cref="Point" />.
        /// </summary>
        /// <param name="pt">The point to convert.</param>
        /// <param name="aliased">Alias if set to <c>true</c>.</param>
        /// <returns>The converted point.</returns>
        public static Point ToXwtPoint (this ScreenPoint pt, bool aliased)
        {
            if (aliased) {
                return new Point (0.5 + (int)pt.X, 0.5 + (int)pt.Y);
            }

            return new Point (pt.X, pt.Y);
        }

        /// <summary>
        /// Converts a cursor type to an Xwt cursor type.
        /// </summary>
        /// <param name="cursorType">Type of the cursor.</param>
        /// <returns>An Xwt cursor type.</returns>
        public static global::Xwt.CursorType ToXwtCursorType (this CursorType cursorType)
        {
            switch (cursorType) {
                case OxyPlot.CursorType.Pan:
                    return global::Xwt.CursorType.Hand;
                case OxyPlot.CursorType.ZoomRectangle:
                    return global::Xwt.CursorType.Crosshair;
                case OxyPlot.CursorType.ZoomHorizontal:
                    return global::Xwt.CursorType.ResizeLeftRight;
                case OxyPlot.CursorType.ZoomVertical:
                    return global::Xwt.CursorType.ResizeUpDown;
                default:
                    return global::Xwt.CursorType.Arrow;
            }
        }

        /// <summary>
        /// Creates the mouse down event arguments.
        /// </summary>
        /// <param name="args">The instance containing the event data.</param>
        /// <returns>Mouse event arguments.</returns>
        public static OxyMouseDownEventArgs ToOxyMouseDownEventArgs (this ButtonEventArgs args)
        {
            return new OxyMouseDownEventArgs {
                ChangedButton = args.Button.ToOxyMouseButton (),
                ClickCount = 1,
                Position = new ScreenPoint (args.X, args.Y),
                ModifierKeys = OxyModifierKeys.None
            };
        }

        /// <summary>
        /// Creates the mouse up event arguments.
        /// </summary>
        /// <param name="args">The instance containing the event data.</param>
        /// <returns>Mouse event arguments.</returns>
        public static OxyMouseEventArgs ToOxyMouseUpEventArgs (this ButtonEventArgs args)
        {
            return new OxyMouseEventArgs {
                Position = new ScreenPoint (args.X, args.Y),
                ModifierKeys = OxyModifierKeys.None
            };
        }

        /// <summary>
        /// Creates the mouse event arguments.
        /// </summary>
        /// <param name="args">The motion event args.</param>
        /// <returns>Mouse event arguments.</returns>
        public static OxyMouseEventArgs ToOxyMouseEventArgs (this MouseMovedEventArgs args)
        {
            return new OxyMouseEventArgs {
                Position = new ScreenPoint (args.X, args.Y),
                ModifierKeys = OxyModifierKeys.None
            };
        }

        /// <summary>
        /// Creates the mouse wheel event arguments.
        /// </summary>
        /// <param name="args">The scroll event args.</param>
        /// <returns>Mouse event arguments.</returns>
        public static OxyMouseWheelEventArgs ToOxyMouseWheelEventArgs (this MouseScrolledEventArgs args)
        {
            return new OxyMouseWheelEventArgs {
                Delta = args.Direction == ScrollDirection.Down ? -120 : 120,
                Position = new ScreenPoint (args.X, args.Y),
                ModifierKeys = OxyModifierKeys.None
            };
        }

        /// <summary>
        /// Creates the key event arguments.
        /// </summary>
        /// <param name="e">The key event args.</param>
        /// <returns>Key event arguments.</returns>
        public static OxyKeyEventArgs ToOxyKeyEventArgs (this KeyEventArgs e)
        {
            return new OxyKeyEventArgs {
                ModifierKeys = ToOxyModifierKeys (e.Modifiers),
                Key = e.Key.ToOxyKey ()
            };
        }

        /// <summary>
        /// Converts the specified key.
        /// </summary>
        /// <param name="k">The key to convert.</param>
        /// <returns>The converted key.</returns>
        static OxyKey ToOxyKey (this Key k)
        {
            switch (k) {
                case Key.A:
                case Key.a:
                    return OxyKey.A;
                case Key.Plus:
                case Key.NumPadAdd:
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
                case Key.K0:
                    return OxyKey.D0;
                case Key.K1:
                    return OxyKey.D1;
                case Key.K2:
                    return OxyKey.D2;
                case Key.K3:
                    return OxyKey.D3;
                case Key.K4:
                    return OxyKey.D4;
                case Key.K5:
                    return OxyKey.D5;
                case Key.K6:
                    return OxyKey.D6;
                case Key.K7:
                    return OxyKey.D7;
                case Key.K8:
                    return OxyKey.D8;
                case Key.K9:
                    return OxyKey.D9;
                case Key.NumPadDecimal:
                    return OxyKey.Decimal;
                case Key.Delete:
                case Key.NumPadDelete:
                    return OxyKey.Delete;
                case Key.NumPadDivide:
                    return OxyKey.Divide;
                case Key.Down:
                case Key.NumPadDown:
                    return OxyKey.Down;
                case Key.E:
                case Key.e:
                    return OxyKey.E;
                case Key.End:
                case Key.NumPadEnd:
                    return OxyKey.End;
                case Key.Return:
                case Key.NumPadEnter:
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
                case Key.NumPadHome:
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
                case Key.Asterisk:
                case Key.NumPadMultiply:
                    return OxyKey.Multiply;
                case Key.N:
                case Key.n:
                    return OxyKey.N;
                case Key.NumPad0:
                    return OxyKey.NumPad0;
                case Key.NumPad1:
                    return OxyKey.NumPad1;
                case Key.NumPad2:
                    return OxyKey.NumPad2;
                case Key.NumPad3:
                    return OxyKey.NumPad3;
                case Key.NumPad4:
                    return OxyKey.NumPad4;
                case Key.NumPad5:
                    return OxyKey.NumPad5;
                case Key.NumPad6:
                    return OxyKey.NumPad6;
                case Key.NumPad7:
                    return OxyKey.NumPad7;
                case Key.NumPad8:
                    return OxyKey.NumPad8;
                case Key.NumPad9:
                    return OxyKey.NumPad9;
                case Key.O:
                case Key.o:
                    return OxyKey.O;
                case Key.P:
                case Key.p:
                    return OxyKey.P;
                case Key.PageDown:
                    return OxyKey.PageDown;
                case Key.PageUp:
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
                case Key.Space:
                case Key.NumPadSpace:
                    return OxyKey.Space;
                case Key.Minus:
                case Key.NumPadSubtract:
                    return OxyKey.Subtract;
                case Key.T:
                case Key.t:
                    return OxyKey.T;
                case Key.Tab:
                case Key.NumPadTab:
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

        static OxyModifierKeys ToOxyModifierKeys (ModifierKeys state)
        {
            var result = OxyModifierKeys.None;

            if ((state & ModifierKeys.Shift) != 0)
                result |= OxyModifierKeys.Shift;

            if ((state & ModifierKeys.Control) != 0)
                result |= OxyModifierKeys.Control;

            if ((state & ModifierKeys.Alt) != 0)
                result |= OxyModifierKeys.Alt;

            if ((state & ModifierKeys.Command) != 0)
                result |= OxyModifierKeys.Windows;

            return result;
        }

        static OxyMouseButton ToOxyMouseButton (this PointerButton button)
        {
            switch (button) {
                case PointerButton.Left:
                    return OxyMouseButton.Left;
                case PointerButton.Middle:
                    return OxyMouseButton.Middle;
                case PointerButton.Right:
                    return OxyMouseButton.Right;
                case PointerButton.ExtendedButton1:
                    return OxyMouseButton.XButton1;
                case PointerButton.ExtendedButton2:
                    return OxyMouseButton.XButton2;
            }

            return OxyMouseButton.Left;
        }
    }
}

