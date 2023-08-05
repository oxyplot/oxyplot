// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterExtensions.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Extension method used to convert to/from Windows/Windows.Media classes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using HorizontalAlignment = OxyPlot.HorizontalAlignment;
    using VerticalAlignment = OxyPlot.VerticalAlignment;

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
            var dx = p1.X - p2.X;
            var dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Converts an <see cref="OxyColor" /> to a <see cref="Brush" />.
        /// </summary>
        /// <param name="c">The color.</param>
        /// <returns>A <see cref="SolidColorBrush" />.</returns>
        public static Brush ToBrush(this OxyColor c)
        {
            return !c.IsUndefined() ? new SolidColorBrush(c.ToColor()) : null;
        }

        /// <summary>
        /// Converts an <see cref="OxyColor" /> to a <see cref="Color" />.
        /// </summary>
        /// <param name="c">The color.</param>
        /// <returns>A Color.</returns>
        public static Color ToColor(this OxyColor c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Converts an OxyThickness to a Thickness.
        /// </summary>
        /// <param name="c">The thickness.</param>
        /// <returns>A <see cref="Thickness" /> instance.</returns>
        public static Thickness ToThickness(this OxyThickness c)
        {
            return new Thickness(c.Left, c.Top, c.Right, c.Bottom);
        }

        /// <summary>
        /// Converts a ScreenVector to a Vector.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>A <see cref="Vector" /> instance.</returns>
        public static Vector ToVector(this ScreenVector c)
        {
            return new Vector(c.X, c.Y);
        }

        /// <summary>
        /// Converts a HorizontalAlignment to a HorizontalAlignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>A HorizontalAlignment.</returns>
        public static HorizontalAlignment ToHorizontalAlignment(this System.Windows.HorizontalAlignment alignment)
        {
            return alignment switch
            {
                System.Windows.HorizontalAlignment.Center => HorizontalAlignment.Center,
                System.Windows.HorizontalAlignment.Right => HorizontalAlignment.Right,
                _ => HorizontalAlignment.Left,
            };
        }

        /// <summary>
        /// Converts a HorizontalAlignment to a VerticalAlignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>A VerticalAlignment.</returns>
        public static VerticalAlignment ToVerticalAlignment(this System.Windows.VerticalAlignment alignment)
        {
            return alignment switch
            {
                System.Windows.VerticalAlignment.Center => VerticalAlignment.Middle,
                System.Windows.VerticalAlignment.Top => VerticalAlignment.Top,
                _ => VerticalAlignment.Bottom,
            };
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
        /// <param name="brush">The brush.</param>
        /// <returns>An <see cref="OxyColor" />.</returns>
        public static OxyColor ToOxyColor(this Brush brush)
        {
            return brush is SolidColorBrush scb ? scb.Color.ToOxyColor() : OxyColors.Undefined;
        }

        /// <summary>
        /// Converts a Thickness to an <see cref="OxyThickness" />.
        /// </summary>
        /// <param name="t">The thickness.</param>
        /// <returns>An <see cref="OxyThickness" />.</returns>
        public static OxyThickness ToOxyThickness(this Thickness t)
        {
            return new OxyThickness(t.Left, t.Top, t.Right, t.Bottom);
        }

        /// <summary>
        /// Converts a <see cref="Point" /> to a <see cref="ScreenPoint" />.
        /// </summary>
        /// <param name="pt">The point.</param>
        /// <returns>A <see cref="ScreenPoint" />.</returns>
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
            for (var i = 0; i < points.Length; i++)
            {
                pts[i] = points[i].ToScreenPoint();
            }

            return pts;
        }

        /// <summary>
        /// Converts the specified vector to a ScreenVector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>A <see cref="ScreenVector" />.</returns>
        public static ScreenVector ToScreenVector(this Vector vector)
        {
            return new ScreenVector(vector.X, vector.Y);
        }

        /// <summary>
        /// Converts the specified key.
        /// </summary>
        /// <param name="k">The key to convert.</param>
        /// <returns>The converted key.</returns>
        public static OxyKey Convert(this Key k)
        {
            return k switch
            {
                Key.A => OxyKey.A,
                Key.Add => OxyKey.Add,
                Key.B => OxyKey.B,
                Key.Back => OxyKey.Backspace,
                Key.C => OxyKey.C,
                Key.D => OxyKey.D,
                Key.D0 => OxyKey.D0,
                Key.D1 => OxyKey.D1,
                Key.D2 => OxyKey.D2,
                Key.D3 => OxyKey.D3,
                Key.D4 => OxyKey.D4,
                Key.D5 => OxyKey.D5,
                Key.D6 => OxyKey.D6,
                Key.D7 => OxyKey.D7,
                Key.D8 => OxyKey.D8,
                Key.D9 => OxyKey.D9,
                Key.Decimal => OxyKey.Decimal,
                Key.Delete => OxyKey.Delete,
                Key.Divide => OxyKey.Divide,
                Key.Down => OxyKey.Down,
                Key.E => OxyKey.E,
                Key.End => OxyKey.End,
                Key.Enter => OxyKey.Enter,
                Key.Escape => OxyKey.Escape,
                Key.F => OxyKey.F,
                Key.F1 => OxyKey.F1,
                Key.F10 => OxyKey.F10,
                Key.F11 => OxyKey.F11,
                Key.F12 => OxyKey.F12,
                Key.F2 => OxyKey.F2,
                Key.F3 => OxyKey.F3,
                Key.F4 => OxyKey.F4,
                Key.F5 => OxyKey.F5,
                Key.F6 => OxyKey.F6,
                Key.F7 => OxyKey.F7,
                Key.F8 => OxyKey.F8,
                Key.F9 => OxyKey.F9,
                Key.G => OxyKey.G,
                Key.H => OxyKey.H,
                Key.Home => OxyKey.Home,
                Key.I => OxyKey.I,
                Key.Insert => OxyKey.Insert,
                Key.J => OxyKey.J,
                Key.K => OxyKey.K,
                Key.L => OxyKey.L,
                Key.Left => OxyKey.Left,
                Key.M => OxyKey.M,
                Key.Multiply => OxyKey.Multiply,
                Key.N => OxyKey.N,
                Key.NumPad0 => OxyKey.NumPad0,
                Key.NumPad1 => OxyKey.NumPad1,
                Key.NumPad2 => OxyKey.NumPad2,
                Key.NumPad3 => OxyKey.NumPad3,
                Key.NumPad4 => OxyKey.NumPad4,
                Key.NumPad5 => OxyKey.NumPad5,
                Key.NumPad6 => OxyKey.NumPad6,
                Key.NumPad7 => OxyKey.NumPad7,
                Key.NumPad8 => OxyKey.NumPad8,
                Key.NumPad9 => OxyKey.NumPad9,
                Key.O => OxyKey.O,
                Key.P => OxyKey.P,
                Key.PageDown => OxyKey.PageDown,
                Key.PageUp => OxyKey.PageUp,
                Key.Q => OxyKey.Q,
                Key.R => OxyKey.R,
                Key.Right => OxyKey.Right,
                Key.S => OxyKey.S,
                Key.Space => OxyKey.Space,
                Key.Subtract => OxyKey.Subtract,
                Key.T => OxyKey.T,
                Key.Tab => OxyKey.Tab,
                Key.U => OxyKey.U,
                Key.Up => OxyKey.Up,
                Key.V => OxyKey.V,
                Key.W => OxyKey.W,
                Key.X => OxyKey.X,
                Key.Y => OxyKey.Y,
                Key.Z => OxyKey.Z,
                _ => OxyKey.Unknown,
            };
        }

        /// <summary>
        /// Converts the specified button.
        /// </summary>
        /// <param name="button">The button to convert.</param>
        /// <returns>The converted mouse button.</returns>
        public static OxyMouseButton Convert(this MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => OxyMouseButton.Left,
                MouseButton.Middle => OxyMouseButton.Middle,
                MouseButton.Right => OxyMouseButton.Right,
                MouseButton.XButton1 => OxyMouseButton.XButton1,
                MouseButton.XButton2 => OxyMouseButton.XButton2,
                _ => OxyMouseButton.None,
            };
        }

        /// <summary>
        /// Converts <see cref="MouseWheelEventArgs" /> to <see cref="OxyMouseWheelEventArgs" /> for a mouse wheel event.
        /// </summary>
        /// <param name="e">The <see cref="MouseWheelEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="IInputElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseWheelEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseWheelEventArgs ToMouseWheelEventArgs(this MouseWheelEventArgs e, IInputElement relativeTo)
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
        /// <param name="relativeTo">The <see cref="IInputElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseDownEventArgs ToMouseDownEventArgs(this MouseButtonEventArgs e, IInputElement relativeTo)
        {
            return new OxyMouseDownEventArgs
            {
                ChangedButton = e.ChangedButton.Convert(),
                ClickCount = e.ClickCount,
                Position = e.GetPosition(relativeTo).ToScreenPoint(),
                ModifierKeys = Keyboard.GetModifierKeys()
            };
        }

        /// <summary>
        /// Converts <see cref="MouseButtonEventArgs" /> to <see cref="OxyMouseEventArgs" /> for a mouse up event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        /// <param name="relativeTo">The <see cref="IInputElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseEventArgs ToMouseReleasedEventArgs(this MouseButtonEventArgs e, IInputElement relativeTo)
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
        /// <param name="relativeTo">The <see cref="IInputElement" /> that the event is relative to.</param>
        /// <returns>A <see cref="OxyMouseEventArgs" /> containing the converted event arguments.</returns>
        public static OxyMouseEventArgs ToMouseEventArgs(this MouseEventArgs e, IInputElement relativeTo)
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
                Position = e.ManipulationOrigin.ToScreenPoint(),
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
                Position = e.ManipulationOrigin.ToScreenPoint()
            };
        }
    }
}
