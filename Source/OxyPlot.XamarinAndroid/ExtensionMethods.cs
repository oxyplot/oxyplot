// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods that converts between Android types and OxyPlot types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.XamarinAndroid
{
    using System;

    using Android.Graphics;
    using Android.Views;

    /// <summary>
    /// Provides extension methods that converts between Android types and OxyPlot types.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts an <see cref="OxyColor" /> to a <see cref="Color" />.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The converted color.</returns>
        public static Color ToColor(this OxyColor color)
        {
            return color.IsInvisible() ? Color.Transparent : new Color(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Converts an <see cref="LineJoin" /> to a <see cref="Paint.Join" />.
        /// </summary>
        /// <param name="join">The join value to convert.</param>
        /// <returns>The converted join value.</returns>
        public static Paint.Join Convert(this LineJoin join)
        {
            switch (join)
            {
                case LineJoin.Bevel:
                    return Paint.Join.Bevel;
                case LineJoin.Miter:
                    return Paint.Join.Miter;
                case LineJoin.Round:
                    return Paint.Join.Round;
                default:
                    throw new InvalidOperationException("Invalid join type.");
            }
        }

        /// <summary>
        /// Converts an <see cref="MotionEvent" /> to a <see cref="OxyTouchEventArgs" />.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <param name = "scale">The resolution scale factor.</param>
        /// <returns>The converted event arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs(this MotionEvent e, double scale)
        {
            return new OxyTouchEventArgs
            {
                Position = new ScreenPoint(e.GetX(e.ActionIndex) / scale, e.GetY(e.ActionIndex) / scale),
                DeltaTranslation = new ScreenVector(0, 0),
                DeltaScale = new ScreenVector(1, 1)
            };
        }

        /// <summary>
        /// Gets the touch points from the specified <see cref="MotionEvent" /> argument.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <param name = "scale">The resolution scale factor.</param>
        /// <returns>The touch points.</returns>
        public static ScreenPoint[] GetTouchPoints(this MotionEvent e, double scale)
        {
            var result = new ScreenPoint[e.PointerCount];
            for (int i = 0; i < e.PointerCount; i++)
            {
                result[i] = new ScreenPoint(e.GetX(i) / scale, e.GetY(i) / scale);
            }

            return result;
        }

        /// <summary>
        /// Converts an <see cref="Keycode" /> to a <see cref="OxyKey" />.
        /// </summary>
        /// <param name="keyCode">The key code.</param>
        /// <returns>The converted key.</returns>
        public static OxyKey Convert(this Keycode keyCode)
        {
            switch (keyCode)
            {
                case Keycode.A:
                    return OxyKey.A;
                case Keycode.Plus:
                    return OxyKey.Add;
                case Keycode.B:
                    return OxyKey.B;
                case Keycode.Back:
                    return OxyKey.Backspace;
                case Keycode.C:
                    return OxyKey.C;
                case Keycode.D:
                    return OxyKey.D;
                case Keycode.Num0:
                    return OxyKey.D0;
                case Keycode.Num1:
                    return OxyKey.D1;
                case Keycode.Num2:
                    return OxyKey.D2;
                case Keycode.Num3:
                    return OxyKey.D3;
                case Keycode.Num4:
                    return OxyKey.D4;
                case Keycode.Num5:
                    return OxyKey.D5;
                case Keycode.Num6:
                    return OxyKey.D6;
                case Keycode.Num7:
                    return OxyKey.D7;
                case Keycode.Num8:
                    return OxyKey.D8;
                case Keycode.Num9:
                    return OxyKey.D9;
                case Keycode.Comma:
                    return OxyKey.Decimal;
                case Keycode.Del:
                    return OxyKey.Delete;
                case Keycode.Slash:
                    return OxyKey.Divide;
                case Keycode.DpadDown:
                    return OxyKey.Down;
                case Keycode.E:
                    return OxyKey.E;
                case Keycode.Enter:
                    return OxyKey.Enter;
                case Keycode.F:
                    return OxyKey.F;

                case Keycode.G:
                    return OxyKey.G;
                case Keycode.H:
                    return OxyKey.H;
                case Keycode.Home:
                    return OxyKey.Home;
                case Keycode.I:
                    return OxyKey.I;
                case Keycode.J:
                    return OxyKey.J;
                case Keycode.K:
                    return OxyKey.K;
                case Keycode.L:
                    return OxyKey.L;
                case Keycode.DpadLeft:
                    return OxyKey.Left;
                case Keycode.M:
                    return OxyKey.M;
                case Keycode.Star:
                    return OxyKey.Multiply;
                case Keycode.N:
                    return OxyKey.N;
                case Keycode.O:
                    return OxyKey.O;
                case Keycode.P:
                    return OxyKey.P;
                case Keycode.Q:
                    return OxyKey.Q;
                case Keycode.R:
                    return OxyKey.R;
                case Keycode.DpadRight:
                    return OxyKey.Right;
                case Keycode.S:
                    return OxyKey.S;
                case Keycode.Space:
                    return OxyKey.Space;
                case Keycode.Minus:
                    return OxyKey.Subtract;
                case Keycode.T:
                    return OxyKey.T;
                case Keycode.Tab:
                    return OxyKey.Tab;
                case Keycode.U:
                    return OxyKey.U;
                case Keycode.DpadUp:
                    return OxyKey.Up;
                case Keycode.V:
                    return OxyKey.V;
                case Keycode.W:
                    return OxyKey.W;
                case Keycode.X:
                    return OxyKey.X;
                case Keycode.Y:
                    return OxyKey.Y;
                case Keycode.Z:
                    return OxyKey.Z;
                default:
                    return OxyKey.Unknown;
            }
        }

        /// <summary>
        /// Gets the <see cref="OxyModifierKeys" /> from a <see cref="KeyEvent" /> .
        /// </summary>
        /// <param name="e">The key event arguments.</param>
        /// <returns>The converted modifier keys.</returns>
        public static OxyModifierKeys GetModifierKeys(this KeyEvent e)
        {
            var result = OxyModifierKeys.None;

            if (e.IsAltPressed)
            {
                result |= OxyModifierKeys.Alt;
            }

            if (e.IsShiftPressed)
            {
                result |= OxyModifierKeys.Shift;
            }

            if (e.IsSymPressed)
            {
                // The SYM meta key is pressed. Can we use this as control?
                result |= OxyModifierKeys.Control;
            }

            return result;
        }

        /// <summary>
        /// Converts an <see cref="KeyEvent" /> to a <see cref="OxyKeyEventArgs" />.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <returns>The converted event arguments.</returns>
        /// <remarks>See also <a href="http://developer.android.com/reference/android/view/KeyEvent.html">KeyEvent</a> reference.</remarks>
        public static OxyKeyEventArgs ToKeyEventArgs(this KeyEvent e)
        {
            return new OxyKeyEventArgs
            {
                Key = e.KeyCode.Convert(),
                ModifierKeys = e.GetModifierKeys()
            };
        }
    }
}