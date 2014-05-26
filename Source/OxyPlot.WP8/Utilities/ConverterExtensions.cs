// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterExtensions.cs" company="OxyPlot">
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
//   Extension method used to convert to/from Windows/Windows.Media classes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WP8
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

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
            return new SolidColorBrush(c.ToColor());
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
        /// Converts a Color to an OxyColor.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>An OxyColor.</returns>
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
            var scb = brush as SolidColorBrush;
            return scb != null ? scb.Color.ToOxyColor() : OxyColors.Undefined;
        }

        /// <summary>
        /// Converts a Thickness to an OxyThickness.
        /// </summary>
        /// <param name="t">The thickness.</param>
        /// <returns>An OxyPlot thickness.</returns>
        public static OxyThickness ToOxyThickness(this Thickness t)
        {
            return new OxyThickness(t.Left, t.Top, t.Right, t.Bottom);
        }

        /// <summary>
        /// Converts a ScreenPoint to a Point.
        /// </summary>
        /// <param name="pt">The screen point.</param>
        /// <param name="aliased">use pixel alignment conversion if set to <c>true</c>.</param>
        /// <returns>A point.</returns>
        public static Point ToPoint(this ScreenPoint pt, bool aliased)
        {
            // adding 0.5 to get pixel boundary alignment, seems to work
            // http://weblogs.asp.net/mschwarz/archive/2008/01/04/silverlight-rectangles-paths-and-line-comparison.aspx
            // http://www.wynapse.com/Silverlight/Tutor/Silverlight_Rectangles_Paths_And_Lines_Comparison.aspx
            if (aliased)
            {
                return new Point(0.5 + (int)pt.X, 0.5 + (int)pt.Y);
            }

            return new Point(pt.X, pt.Y);
        }

        /// <summary>
        /// Converts an <see cref="OxyRect" /> to a <see cref="Rect" />.
        /// </summary>
        /// <param name="r">The rectangle to convert.</param>
        /// <param name="aliased">use pixel alignment if set to <c>true</c>.</param>
        /// <returns>A <see cref="Rect" />.</returns>
        public static Rect ToRect(this OxyRect r, bool aliased)
        {
            if (aliased)
            {
                double x = 0.5 + (int)r.Left;
                double y = 0.5 + (int)r.Top;
                double ri = 0.5 + (int)r.Right;
                double bo = 0.5 + (int)r.Bottom;
                return new Rect(x, y, ri - x, bo - y);
            }

            return new Rect(r.Left, r.Top, r.Width, r.Height);
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
        /// Converts the specified vector to a ScreenVector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>A <see cref="ScreenVector" />.</returns>
        public static ScreenVector ToScreenVector(this Point vector)
        {
            return new ScreenVector(vector.X, vector.Y);
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
        /// Converts the specified key.
        /// </summary>
        /// <param name="k">The key to convert.</param>
        /// <returns>The converted key.</returns>
        public static OxyKey Convert(this Key k)
        {
            switch (k)
            {
                case Key.A:
                    return OxyKey.A;
                case Key.Add:
                    return OxyKey.Add;
                case Key.B:
                    return OxyKey.B;
                case Key.Back:
                    return OxyKey.Backspace;
                case Key.C:
                    return OxyKey.C;
                case Key.D:
                    return OxyKey.D;
                case Key.D0:
                    return OxyKey.D0;
                case Key.D1:
                    return OxyKey.D1;
                case Key.D2:
                    return OxyKey.D2;
                case Key.D3:
                    return OxyKey.D3;
                case Key.D4:
                    return OxyKey.D4;
                case Key.D5:
                    return OxyKey.D5;
                case Key.D6:
                    return OxyKey.D6;
                case Key.D7:
                    return OxyKey.D7;
                case Key.D8:
                    return OxyKey.D8;
                case Key.D9:
                    return OxyKey.D9;
                case Key.Decimal:
                    return OxyKey.Decimal;
                case Key.Delete:
                    return OxyKey.Delete;
                case Key.Divide:
                    return OxyKey.Divide;
                case Key.Down:
                    return OxyKey.Down;
                case Key.E:
                    return OxyKey.E;
                case Key.End:
                    return OxyKey.End;
                case Key.Enter:
                    return OxyKey.Enter;
                case Key.Escape:
                    return OxyKey.Escape;
                case Key.F:
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
                    return OxyKey.G;
                case Key.H:
                    return OxyKey.H;
                case Key.Home:
                    return OxyKey.Home;
                case Key.I:
                    return OxyKey.I;
                case Key.Insert:
                    return OxyKey.Insert;
                case Key.J:
                    return OxyKey.J;
                case Key.K:
                    return OxyKey.K;
                case Key.L:
                    return OxyKey.L;
                case Key.Left:
                    return OxyKey.Left;
                case Key.M:
                    return OxyKey.M;
                case Key.Multiply:
                    return OxyKey.Multiply;
                case Key.N:
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
                    return OxyKey.O;
                case Key.P:
                    return OxyKey.P;
                case Key.PageDown:
                    return OxyKey.PageDown;
                case Key.PageUp:
                    return OxyKey.PageUp;
                case Key.Q:
                    return OxyKey.Q;
                case Key.R:
                    return OxyKey.R;
                case Key.Right:
                    return OxyKey.Right;
                case Key.S:
                    return OxyKey.S;
                case Key.Space:
                    return OxyKey.Space;
                case Key.Subtract:
                    return OxyKey.Subtract;
                case Key.T:
                    return OxyKey.T;
                case Key.Tab:
                    return OxyKey.Tab;
                case Key.U:
                    return OxyKey.U;
                case Key.Up:
                    return OxyKey.Up;
                case Key.V:
                    return OxyKey.V;
                case Key.W:
                    return OxyKey.W;
                case Key.X:
                    return OxyKey.X;
                case Key.Y:
                    return OxyKey.Y;
                case Key.Z:
                    return OxyKey.Z;
                default:
                    return OxyKey.Unknown;
            }
        }

        /// <summary>
        /// Converts <see cref="MouseWheelEventArgs" /> to <see cref="OxyMouseWheelEventArgs" /> for a mouse wheel event.
        /// </summary>
        /// <param name="e">The <see cref="MouseWheelEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseWheelEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseWheelEventArgs ToMouseWheelEventArgs(this MouseWheelEventArgs e, UIElement relativeTo)
        {
            return new OxyMouseWheelEventArgs
            {
                Position = e.GetPosition(relativeTo).ToScreenPoint(),
                ModifierKeys = Keyboard.GetModifierKeys(),
                Delta = e.Delta
            };
        }

        /// <summary>
        /// Converts <see cref="MouseButtonEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a mouse down event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        /// <param name="button">The button.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseDownEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseDownEventArgs ToMouseDownEventArgs(this MouseButtonEventArgs e, OxyMouseButton button, UIElement relativeTo)
        {
            return new OxyMouseDownEventArgs
            {
                ChangedButton = button,
                ClickCount = 1,
                Position = e.GetPosition(relativeTo).ToScreenPoint(),
                ModifierKeys = Keyboard.GetModifierKeys()
            };
        }

        /// <summary>
        /// Converts <see cref="MouseButtonEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a mouse up event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        /// <param name="button">The button.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseEventArgs ToMouseUpEventArgs(this MouseButtonEventArgs e, OxyMouseButton button, UIElement relativeTo)
        {
            return new OxyMouseEventArgs
            {
                Position = e.GetPosition(relativeTo).ToScreenPoint(),
                ModifierKeys = Keyboard.GetModifierKeys()
            };
        }

        /// <summary>
        /// Converts <see cref="MouseEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a mouse event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseEventArgs ToMouseEventArgs(this MouseEventArgs e, UIElement relativeTo)
        {
            return new OxyMouseEventArgs
            {
                Position = e.GetPosition(relativeTo).ToScreenPoint(),
                ModifierKeys = Keyboard.GetModifierKeys()
            };
        }

        /// <summary>
        /// Converts <see cref="ManipulationStartedEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a touch started event.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationStartedEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs(this ManipulationStartedEventArgs e, UIElement relativeTo)
        {
            return new OxyTouchEventArgs
            {
                Position = e.ManipulationOrigin.ToScreenPoint(),
            };
        }

        /// <summary>
        /// Converts <see cref="ManipulationDeltaEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a touch delta event.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationDeltaEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs(this ManipulationDeltaEventArgs e, UIElement relativeTo)
        {
            return new OxyTouchEventArgs
            {
                Position = e.ManipulationOrigin.ToScreenPoint() + e.CumulativeManipulation.Translation.ToScreenVector(),
                DeltaTranslation = e.DeltaManipulation.Translation.ToScreenVector(),
                DeltaScale = e.DeltaManipulation.Scale.ToScreenVector()
            };
        }

        /// <summary>
        /// Converts <see cref="ManipulationCompletedEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a touch completed event.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationCompletedEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs(this ManipulationCompletedEventArgs e, UIElement relativeTo)
        {
            return new OxyTouchEventArgs
            {
                Position = e.ManipulationOrigin.ToScreenPoint() + e.TotalManipulation.Translation.ToScreenVector(),
            };
        }
    }
}