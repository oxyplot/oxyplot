// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Extension method used to convert to/from Windows/Windows.Media classes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Windows
{
    using System;

    using global::Windows.Foundation;
    using global::Windows.System;
    using global::Windows.UI;
    using global::Windows.UI.Input;
    using global::Windows.UI.Xaml;
    using global::Windows.UI.Xaml.Input;
    using global::Windows.UI.Xaml.Media;

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
        /// Converts a <see cref="OxyColor" /> to a <see cref="Brush" />.
        /// </summary>
        /// <param name="c">The color.</param>
        /// <returns>A SolidColorBrush.</returns>
        public static Brush ToBrush(this OxyColor c)
        {
            return new SolidColorBrush(c.ToColor());
        }

        /// <summary>
        /// Converts an <see cref="OxyColor" /> to a <see cref="T:Color" />.
        /// </summary>
        /// <param name="c">The color.</param>
        /// <returns>A <see cref="T:Color" />.</returns>
        public static Color ToColor(this OxyColor c)
        {
            return new Color { A = c.A, R = c.R, G = c.G, B = c.B };
        }

        /// <summary>
        /// Converts a <see cref="HorizontalAlignment" /> to a <see cref="OxyPlot.HorizontalAlignment" />.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>A <see cref="OxyPlot.HorizontalAlignment" />.</returns>
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
        /// Converts a <see cref="T:Color" /> to an <see cref="T:OxyColor" />.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>An <see cref="T:OxyColor" />.</returns>
        public static OxyColor ToOxyColor(this Color color)
        {
            return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts a <see cref="Nullable{Color}" /> to an <see cref="OxyColor" />.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The converted color, or <see cref="OxyColors.Undefined" /> if the <paramref name="color" /> is <c>null</c>.</returns>
        public static OxyColor ToOxyColor(this Color? color)
        {
            return color.HasValue ? color.Value.ToOxyColor() : OxyColors.Undefined;
        }

        /// <summary>
        /// Converts a <see cref="SolidColorBrush" /> to an <see cref="OxyColor" />.
        /// </summary>
        /// <param name="brush">The brush to convert.</param>
        /// <returns>An <see cref="OxyColor" />.</returns>
        public static OxyColor ToOxyColor(this Brush brush)
        {
            var scb = brush as SolidColorBrush;
            return scb != null ? scb.Color.ToOxyColor() : OxyColors.Undefined;
        }

        /// <summary>
        /// Converts a <see cref="T:Thickness" /> to an <see cref="OxyThickness" />.
        /// </summary>
        /// <param name="t">The thickness.</param>
        /// <returns>An <see cref="OxyThickness" />.</returns>
        public static OxyThickness ToOxyThickness(this Thickness t)
        {
            return new OxyThickness(t.Left, t.Top, t.Right, t.Bottom);
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a <see cref="T:Point" />.
        /// </summary>
        /// <param name="pt">The point to convert.</param>
        /// <param name="aliased">Use pixel alignment conversion if set to <c>true</c>.</param>
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
        /// Converts an <see cref="OxyRect" /> to a <see cref="T:Rect" />.
        /// </summary>
        /// <param name="r">The rectangle to convert.</param>
        /// <param name="aliased">Use pixel alignment if set to <c>true</c>.</param>
        /// <returns>The converted rectangle.</returns>
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
        /// Gets the pressed mouse button from the specified <see cref="PointerPointProperties" />.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns>The pressed mouse button.</returns>
        public static OxyMouseButton GetPressedMouseButton(this PointerPointProperties properties)
        {
            if (properties.IsLeftButtonPressed)
            {
                return OxyMouseButton.Left;
            }

            if (properties.IsMiddleButtonPressed)
            {
                return OxyMouseButton.Middle;
            }

            if (properties.IsRightButtonPressed)
            {
                return OxyMouseButton.Right;
            }

            if (properties.IsXButton1Pressed)
            {
                return OxyMouseButton.XButton1;
            }

            if (properties.IsXButton2Pressed)
            {
                return OxyMouseButton.XButton2;
            }

            return OxyMouseButton.None;
        }

        /// <summary>
        /// Converts <see cref="PointerRoutedEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a mouse down event.
        /// </summary>
        /// <param name="e">The <see cref="PointerRoutedEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseDownEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseDownEventArgs ToMouseDownEventArgs(this PointerRoutedEventArgs e, UIElement relativeTo)
        {
            var point = e.GetCurrentPoint(relativeTo);

            int clickCount = 1;
            if (MouseButtonHelper.IsDoubleClick(relativeTo, point.Position))
            {
                clickCount = 2;
            }

            return new OxyMouseDownEventArgs
            {
                ChangedButton = point.Properties.GetPressedMouseButton(),
                Position = point.Position.ToScreenPoint(),
                ModifierKeys = e.GetModifierKeys(),
                ClickCount = clickCount
            };
        }

        /// <summary>
        /// Converts <see cref="PointerRoutedEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a mouse event.
        /// </summary>
        /// <param name="e">The <see cref="PointerRoutedEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseEventArgs ToMouseEventArgs(this PointerRoutedEventArgs e, UIElement relativeTo)
        {
            var point = e.GetCurrentPoint(relativeTo);
            return new OxyMouseEventArgs
            {
                Position = point.Position.ToScreenPoint(),
                ModifierKeys = e.GetModifierKeys(),
            };
        }

        /// <summary>
        /// Converts <see cref="PointerRoutedEventArgs" /> to <see cref="OxyMouseWheelEventArgs" /> for a mouse wheel event.
        /// </summary>
        /// <param name="e">The <see cref="PointerRoutedEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseWheelEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseWheelEventArgs ToMouseWheelEventArgs(this PointerRoutedEventArgs e, UIElement relativeTo)
        {
            var point = e.GetCurrentPoint(relativeTo);
            return new OxyMouseWheelEventArgs
            {
                Position = point.Position.ToScreenPoint(),
                ModifierKeys = e.GetModifierKeys(),
                Delta = point.Properties.MouseWheelDelta
            };
        }

        /// <summary>
        /// Converts <see cref="ManipulationStartedRoutedEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a touch started event.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationStartedRoutedEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs(this ManipulationStartedRoutedEventArgs e, UIElement relativeTo)
        {
            return new OxyTouchEventArgs
            {
                Position = e.Position.ToScreenPoint(),
            };
        }

        /// <summary>
        /// Converts <see cref="ManipulationDeltaRoutedEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a touch delta event.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationDeltaRoutedEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs(this ManipulationDeltaRoutedEventArgs e, UIElement relativeTo)
        {
            return new OxyTouchEventArgs
            {
                Position = e.Position.ToScreenPoint(),
                DeltaTranslation = new ScreenVector(e.Delta.Translation.X, e.Delta.Translation.Y),
                DeltaScale = new ScreenVector(e.Delta.Scale, e.Delta.Scale)
            };
        }

        /// <summary>
        /// Converts <see cref="ManipulationCompletedRoutedEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a touch completed event.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationCompletedRoutedEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs(this ManipulationCompletedRoutedEventArgs e, UIElement relativeTo)
        {
            return new OxyTouchEventArgs
            {
                Position = e.Position.ToScreenPoint(),
            };
        }

        /// <summary>
        /// Converts <see cref="PointerRoutedEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a touch event.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationCompletedRoutedEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="UIElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs(this PointerRoutedEventArgs e, UIElement relativeTo)
        {
            var point = e.GetCurrentPoint(relativeTo);

            var eventArgs = new OxyTouchEventArgs
            {
                Position = point.Position.ToScreenPoint(),
                ModifierKeys = e.GetModifierKeys(),
            };

            return eventArgs;
        }

        /// <summary>
        /// Gets the modifier keys.
        /// </summary>
        /// <param name="e">The <see cref="PointerRoutedEventArgs" /> instance containing the event data.</param>
        /// <returns>Modifier keys.</returns>
        public static OxyModifierKeys GetModifierKeys(this PointerRoutedEventArgs e)
        {
            var result = OxyModifierKeys.None;
            if ((e.KeyModifiers & VirtualKeyModifiers.Shift) == VirtualKeyModifiers.Shift)
            {
                result |= OxyModifierKeys.Shift;
            }

            if ((e.KeyModifiers & VirtualKeyModifiers.Control) == VirtualKeyModifiers.Control)
            {
                result |= OxyModifierKeys.Control;
            }

            if ((e.KeyModifiers & VirtualKeyModifiers.Menu) == VirtualKeyModifiers.Menu)
            {
                result |= OxyModifierKeys.Alt;
            }

            if ((e.KeyModifiers & VirtualKeyModifiers.Windows) == VirtualKeyModifiers.Windows)
            {
                result |= OxyModifierKeys.Windows;
            }

            return result;
        }

        /// <summary>
        /// Converts the specified key to an <see cref="OxyKey" />.
        /// </summary>
        /// <param name="k">The key to convert.</param>
        /// <returns>The converted key.</returns>
        public static OxyKey Convert(this VirtualKey k)
        {
            switch (k)
            {
                case VirtualKey.A:
                    return OxyKey.A;
                case VirtualKey.Add:
                    return OxyKey.Add;
                case VirtualKey.B:
                    return OxyKey.B;
                case VirtualKey.Back:
                    return OxyKey.Backspace;
                case VirtualKey.C:
                    return OxyKey.C;
                case VirtualKey.D:
                    return OxyKey.D;
                case VirtualKey.Number0:
                    return OxyKey.D0;
                case VirtualKey.Number1:
                    return OxyKey.D1;
                case VirtualKey.Number2:
                    return OxyKey.D2;
                case VirtualKey.Number3:
                    return OxyKey.D3;
                case VirtualKey.Number4:
                    return OxyKey.D4;
                case VirtualKey.Number5:
                    return OxyKey.D5;
                case VirtualKey.Number6:
                    return OxyKey.D6;
                case VirtualKey.Number7:
                    return OxyKey.D7;
                case VirtualKey.Number8:
                    return OxyKey.D8;
                case VirtualKey.Number9:
                    return OxyKey.D9;
                case VirtualKey.Decimal:
                    return OxyKey.Decimal;
                case VirtualKey.Delete:
                    return OxyKey.Delete;
                case VirtualKey.Divide:
                    return OxyKey.Divide;
                case VirtualKey.Down:
                    return OxyKey.Down;
                case VirtualKey.E:
                    return OxyKey.E;
                case VirtualKey.End:
                    return OxyKey.End;
                case VirtualKey.Enter:
                    return OxyKey.Enter;
                case VirtualKey.Escape:
                    return OxyKey.Escape;
                case VirtualKey.F:
                    return OxyKey.F;
                case VirtualKey.F1:
                    return OxyKey.F1;
                case VirtualKey.F10:
                    return OxyKey.F10;
                case VirtualKey.F11:
                    return OxyKey.F11;
                case VirtualKey.F12:
                    return OxyKey.F12;
                case VirtualKey.F2:
                    return OxyKey.F2;
                case VirtualKey.F3:
                    return OxyKey.F3;
                case VirtualKey.F4:
                    return OxyKey.F4;
                case VirtualKey.F5:
                    return OxyKey.F5;
                case VirtualKey.F6:
                    return OxyKey.F6;
                case VirtualKey.F7:
                    return OxyKey.F7;
                case VirtualKey.F8:
                    return OxyKey.F8;
                case VirtualKey.F9:
                    return OxyKey.F9;
                case VirtualKey.G:
                    return OxyKey.G;
                case VirtualKey.H:
                    return OxyKey.H;
                case VirtualKey.Home:
                    return OxyKey.Home;
                case VirtualKey.I:
                    return OxyKey.I;
                case VirtualKey.Insert:
                    return OxyKey.Insert;
                case VirtualKey.J:
                    return OxyKey.J;
                case VirtualKey.K:
                    return OxyKey.K;
                case VirtualKey.L:
                    return OxyKey.L;
                case VirtualKey.Left:
                    return OxyKey.Left;
                case VirtualKey.M:
                    return OxyKey.M;
                case VirtualKey.Multiply:
                    return OxyKey.Multiply;
                case VirtualKey.N:
                    return OxyKey.N;
                case VirtualKey.NumberPad0:
                    return OxyKey.NumPad0;
                case VirtualKey.NumberPad1:
                    return OxyKey.NumPad1;
                case VirtualKey.NumberPad2:
                    return OxyKey.NumPad2;
                case VirtualKey.NumberPad3:
                    return OxyKey.NumPad3;
                case VirtualKey.NumberPad4:
                    return OxyKey.NumPad4;
                case VirtualKey.NumberPad5:
                    return OxyKey.NumPad5;
                case VirtualKey.NumberPad6:
                    return OxyKey.NumPad6;
                case VirtualKey.NumberPad7:
                    return OxyKey.NumPad7;
                case VirtualKey.NumberPad8:
                    return OxyKey.NumPad8;
                case VirtualKey.NumberPad9:
                    return OxyKey.NumPad9;
                case VirtualKey.O:
                    return OxyKey.O;
                case VirtualKey.P:
                    return OxyKey.P;
                case VirtualKey.PageDown:
                    return OxyKey.PageDown;
                case VirtualKey.PageUp:
                    return OxyKey.PageUp;
                case VirtualKey.Q:
                    return OxyKey.Q;
                case VirtualKey.R:
                    return OxyKey.R;
                case VirtualKey.Right:
                    return OxyKey.Right;
                case VirtualKey.S:
                    return OxyKey.S;
                case VirtualKey.Space:
                    return OxyKey.Space;
                case VirtualKey.Subtract:
                    return OxyKey.Subtract;
                case VirtualKey.T:
                    return OxyKey.T;
                case VirtualKey.Tab:
                    return OxyKey.Tab;
                case VirtualKey.U:
                    return OxyKey.U;
                case VirtualKey.Up:
                    return OxyKey.Up;
                case VirtualKey.V:
                    return OxyKey.V;
                case VirtualKey.W:
                    return OxyKey.W;
                case VirtualKey.X:
                    return OxyKey.X;
                case VirtualKey.Y:
                    return OxyKey.Y;
                case VirtualKey.Z:
                    return OxyKey.Z;
                default:
                    return OxyKey.Unknown;
            }
        }

        /// <summary>
        /// Converts a <see cref="T:Point" /> to a <see cref="ScreenPoint" />.
        /// </summary>
        /// <param name="pt">The point to convert.</param>
        /// <returns>A <see cref="ScreenPoint" />.</returns>
        public static ScreenPoint ToScreenPoint(this Point pt)
        {
            return new ScreenPoint(pt.X, pt.Y);
        }

        /// <summary>
        /// Converts a <see cref="T:Point" /> to a <see cref="ScreenVector" />.
        /// </summary>
        /// <param name="pt">The vector to convert.</param>
        /// <returns>A <see cref="ScreenVector" />.</returns>
        public static ScreenVector ToScreenVector(this Point pt)
        {
            return new ScreenVector(pt.X, pt.Y);
        }

        /// <summary>
        /// Converts a <see cref="T:Point" /> array to a <see cref="ScreenPoint" /> array.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>A <see cref="ScreenPoint" /> array.</returns>
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
    }
}