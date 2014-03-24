// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="OxyPlot">
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
        public static Color ToColor (this OxyColor color)
        {
            return color.IsInvisible () ? Color.Transparent : new Color (color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Converts an <see cref="OxyRect" /> to a <see cref="RectF" />.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns>The converted rectangle.</returns>
        public static RectF ToRectF (this OxyRect rect)
        {
            return new RectF ((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom);
        }

        /// <summary>
        /// Converts an <see cref="OxyPenLineJoin" /> to a <see cref="Paint.Join" />.
        /// </summary>
        /// <param name="join">The join value to convert.</param>
        /// <returns>The converted join value.</returns>
        public static Paint.Join Convert (this OxyPenLineJoin join)
        {
            switch (join) {
            case OxyPenLineJoin.Bevel:
                return Paint.Join.Bevel;
            case OxyPenLineJoin.Miter:
                return Paint.Join.Miter;
            case OxyPenLineJoin.Round:
                return Paint.Join.Round;
            default:
                throw new InvalidOperationException ("Invalid join type.");
            }
        }

        /// <summary>
        /// Converts an <see cref="MotionEvent" /> to a <see cref="OxyTouchEventArgs" />.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <returns>The converted event arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs (this MotionEvent e)
        {
            return new OxyTouchEventArgs {
                Position = new ScreenPoint (e.GetX (e.ActionIndex), e.GetY (e.ActionIndex)),
                DeltaTranslation = new ScreenVector (0, 0),
                DeltaScale = new ScreenVector (1, 1)
            };
        }

        /// <summary>
        /// Gets the touch points from the specified <see cref="MotionEvent" /> argument.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <returns>The touch points.</returns>
        public static ScreenPoint[] GetTouchPoints (this MotionEvent e)
        {
            var result = new ScreenPoint[e.PointerCount];
            for (int i = 0; i < e.PointerCount; i++) {
                result [i] = new ScreenPoint (e.GetX (i), e.GetY (i));
            }

            return result;
        }

        /// <summary>
        /// Converts an <see cref="Keycode" /> to a <see cref="OxyKey" />.
        /// </summary>
        /// <param name="keyCode">The key code.</param>
        /// <returns>The converted key.</returns>
        public static OxyKey Convert (this Keycode keyCode)
        {
            switch (keyCode) {
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
            //case Keycode.End:
            //    return OxyKey.End;
            case Keycode.Enter:
                return OxyKey.Enter;
            //case Keycode.Escape:
            //    return OxyKey.Escape;
            case Keycode.F:
                return OxyKey.F;
            //case Keycode.F1:
            //    return OxyKey.F1;
            //case Keycode.F10:
            //    return OxyKey.F10;
            //case Keycode.F11:
            //    return OxyKey.F11;
            //case Keycode.F12:
            //    return OxyKey.F12;
            //case Keycode.F2:
            //    return OxyKey.F2;
            //case Keycode.F3:
            //    return OxyKey.F3;
            //case Keycode.F4:
            //    return OxyKey.F4;
            //case Keycode.F5:
            //    return OxyKey.F5;
            //case Keycode.F6:
            //    return OxyKey.F6;
            //case Keycode.F7:
            //    return OxyKey.F7;
            //case Keycode.F8:
            //    return OxyKey.F8;
            //case Keycode.F9:
            //    return OxyKey.F9;
            case Keycode.G:
                return OxyKey.G;
            case Keycode.H:
                return OxyKey.H;
            case Keycode.Home:
                return OxyKey.Home;
            case Keycode.I:
                return OxyKey.I;
            //case Keycode.Insert:
            //    return OxyKey.Insert;
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
            //case Keycode.NumPad0:
            //    return OxyKey.NumPad0;
            //case Keycode.NumPad1:
            //    return OxyKey.NumPad1;
            //case Keycode.NumPad2:
            //    return OxyKey.NumPad2;
            //case Keycode.NumPad3:
            //    return OxyKey.NumPad3;
            //case Keycode.NumPad4:
            //    return OxyKey.NumPad4;
            //case Keycode.NumPad5:
            //    return OxyKey.NumPad5;
            //case Keycode.NumPad6:
            //    return OxyKey.NumPad6;
            //case Keycode.NumPad7:
            //    return OxyKey.NumPad7;
            //case Keycode.NumPad8:
            //    return OxyKey.NumPad8;
            //case Keycode.NumPad9:
            //    return OxyKey.NumPad9;
            case Keycode.O:
                return OxyKey.O;
            case Keycode.P:
                return OxyKey.P;
            //case Keycode.PageDown:
            //    return OxyKey.PageDown;
            //case Keycode.PageUp:
            //    return OxyKey.PageUp;
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
        public static OxyModifierKeys GetModifierKeys (this KeyEvent e)
        {
            var result = OxyModifierKeys.None;

            if (e.IsAltPressed) {
                result |= OxyModifierKeys.Alt;
            }

            if (e.IsShiftPressed) {
                result |= OxyModifierKeys.Shift;
            }

            if (e.IsSymPressed) {
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
        public static OxyKeyEventArgs ToKeyEventArgs (this KeyEvent e)
        {
            return new OxyKeyEventArgs {
                Key = e.KeyCode.Convert (),
                ModifierKeys = e.GetModifierKeys ()
            };
        }
    }
}