// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Extension method used to convert to/from Windows/Windows.Media classes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WindowsForms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Extension method used to convert to/from Windows/Windows.Media classes.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        /// Calculate the distance between two points.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns>The distance.</returns>
        public static double DistanceTo(this Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        /// <summary>
        /// Converts a color to a Brush.
        /// </summary>
        /// <param name="c">The color.</param>
        /// <returns>A SolidColorBrush.</returns>
        public static Brush ToBrush(this OxyColor c)
        {
            return new SolidBrush(c.ToColor());
        }

        /// <summary>
        /// Converts an OxyColor to a Color.
        /// </summary>
        /// <param name="c">The color.</param>
        /// <returns>A Color.</returns>
        public static Color ToColor(this OxyColor c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Converts a HorizontalAlignment to a HorizontalTextAlign.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>A HorizontalTextAlign.</returns>
        public static OxyPlot.HorizontalAlignment ToHorizontalTextAlign(this HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    return OxyPlot.HorizontalAlignment.Center;
                case HorizontalAlignment.Right:
                    return OxyPlot.HorizontalAlignment.Right;
                default:
                    return OxyPlot.HorizontalAlignment.Left;
            }
        }

        /// <summary>
        /// Converts a <see cref="Color" /> to an <see cref="OxyColor" />.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>An <see cref="OxyColor" />.</returns>
        public static OxyColor ToOxyColor(this Color color)
        {
            return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts a <see cref="Brush" /> to an <see cref="OxyColor" />.
        /// </summary>
        /// <param name="brush">The brush to convert.</param>
        /// <returns>An <see cref="OxyColor" />.</returns>
        public static OxyColor ToOxyColor(this Brush brush)
        {
            var scb = brush as SolidBrush;
            return scb != null ? scb.Color.ToOxyColor() : OxyColors.Undefined;
        }

        /// <summary>
        /// Converts a Thickness to an OxyThickness.
        /// </summary>
        /// <param name="pt">The screen point.</param>
        /// <param name="aliased">use pixel alignment conversion if set to <c>true</c>.</param>
        /// <returns>An OxyPlot thickness.</returns>
        public static Point ToPoint(this ScreenPoint pt, bool aliased)
        {
            // adding 0.5 to get pixel boundary alignment, seems to work
            // http://weblogs.asp.net/mschwarz/archive/2008/01/04/silverlight-rectangles-paths-and-line-comparison.aspx
            // http://www.wynapse.com/Silverlight/Tutor/Silverlight_Rectangles_Paths_And_Lines_Comparison.aspx
            if (aliased)
            {
                return new Point((int)pt.X, (int)pt.Y);
            }

            return new Point((int)Math.Round(pt.X), (int)Math.Round(pt.Y));
        }

        /// <summary>
        /// Converts an <see cref="OxyRect" /> to a <see cref="Rectangle" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <param name="aliased">use pixel alignment if set to <c>true</c>.</param>
        /// <returns>A <see cref="Rectangle" />.</returns>
        public static Rectangle ToRect(this OxyRect r, bool aliased)
        {
            if (aliased)
            {
                var x = (int)r.Left;
                var y = (int)r.Top;
                var ri = (int)r.Right;
                var bo = (int)r.Bottom;
                return new Rectangle(x, y, ri - x, bo - y);
            }

            return new Rectangle(
                (int)Math.Round(r.Left), (int)Math.Round(r.Top), (int)Math.Round(r.Width), (int)Math.Round(r.Height));
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
        /// Converts a Point array to a ScreenPoint array.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>A ScreenPoint array.</returns>
        public static ScreenPoint[] ToScreenPointArray(this Point[] points)
        {
            if (points == null)
            {
                return null;
            }

            var pts = new ScreenPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pts[i] = points[i].ToScreenPoint();
            }

            return pts;
        }

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