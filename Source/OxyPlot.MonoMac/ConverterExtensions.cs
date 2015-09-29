// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods that converts between MonoTouch and OxyPlot types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Mono.Mac
{
    using System;

	using System.Drawing;

	using MonoMac.AppKit;

    using MonoMac.CoreGraphics;
 
    using OxyPlot;

    /// <summary>
    /// Provides extension methods that converts between MonoTouch and OxyPlot types.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        /// Converts a <see cref="System.Drawing.PointF" /> to a <see cref="ScreenPoint" />.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>The converted point.</returns>
		public static ScreenPoint ToScreenPoint (this PointF p)
        {
            return new ScreenPoint (p.X, p.Y);
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.PointF" /> to a <see cref="ScreenPoint" />.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>The converted point.</returns>
		public static ScreenPoint LocationToScreenPoint (this PointF p,  RectangleF bounds)
        {
            return new ScreenPoint (p.X, bounds.Height - p.Y);
        }

        /// <summary>
        /// Converts a <see cref="OxyColor" /> to a <see cref="CGColor" />.
        /// </summary>
        /// <param name="c">The color to convert.</param>
        /// <returns>The converted color.</returns>
        // ReSharper disable once InconsistentNaming
        public static CGColor ToCGColor (this OxyColor c)
        {
            return new CGColor (c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        }

        /// <summary>
        /// Converts a <see cref="LineJoin" /> to a <see cref="CGLineCap" />.
        /// </summary>
        /// <param name="lineJoin">The line join.</param>
        /// <returns>The converted join.</returns>
        public static CGLineJoin Convert (this LineJoin lineJoin)
        {
            switch (lineJoin) {
            case LineJoin.Bevel:
                return CGLineJoin.Bevel;
            case LineJoin.Miter:
                return CGLineJoin.Miter;
            case LineJoin.Round:
                return CGLineJoin.Round;
            default:
                throw new InvalidOperationException ("Invalid join type.");
            }
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a <see cref="PointF" />.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>The converted point.</returns>
        public static PointF Convert (this ScreenPoint p)
        {
            return new PointF ((float)p.X, (float)p.Y);
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a pixel center aligned <see cref="PointF" />.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>The converted point.</returns>
        public static PointF ConvertAliased (this ScreenPoint p)
        {
            return new PointF (0.5f + (float)Math.Round (p.X), 0.5f + (float)Math.Round (p.Y));
        }

        /// <summary>
        /// Converts a <see cref="OxyRect" /> to a pixel center aligned <see cref="RectF" />.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns>The converted rectangle.</returns>
        public static RectangleF ConvertAliased (this OxyRect rect)
        {
            float x = 0.5f + (float)Math.Round (rect.Left);
            float y = 0.5f + (float)Math.Round (rect.Top);
            float w = 0.5f + (float)Math.Round (rect.Right) - x;
            float h = 0.5f + (float)Math.Round (rect.Bottom) - y;
            return new RectangleF (x, y, w, h);
        }

        /// <summary>
        /// Converts a <see cref="OxyRect" /> to a <see cref="RectF" />.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns>The converted rectangle.</returns>
        public static RectangleF Convert (this OxyRect rect)
        {
            return new RectangleF ((float)rect.Left, (float)rect.Top, (float)(rect.Right - rect.Left), (float)(rect.Bottom - rect.Top));
        }

        public static OxyMouseButton ToButton (this NSEventType theType)
        {
            switch (theType) {
            case NSEventType.LeftMouseDown:
                return OxyMouseButton.Left;
            case NSEventType.RightMouseDown:
                return OxyMouseButton.Right;
            case NSEventType.OtherMouseDown:
                return OxyMouseButton.Middle;
            default:
                return OxyMouseButton.None;
            }
        }

        public static OxyModifierKeys ToModifierKeys (this NSEventModifierMask theMask)
        {
            var keys = OxyModifierKeys.None;
            if ((theMask & NSEventModifierMask.ShiftKeyMask) == NSEventModifierMask.ShiftKeyMask)
                keys |= OxyModifierKeys.Shift;
            if ((theMask & NSEventModifierMask.ControlKeyMask) == NSEventModifierMask.ControlKeyMask)
                keys |= OxyModifierKeys.Control;
            if ((theMask & NSEventModifierMask.AlternateKeyMask) == NSEventModifierMask.AlternateKeyMask)
                keys |= OxyModifierKeys.Alt;

            // TODO
            if ((theMask & NSEventModifierMask.CommandKeyMask) == NSEventModifierMask.CommandKeyMask)
                keys |= OxyModifierKeys.Control;

            return keys;
        }

        public static OxyMouseDownEventArgs ToMouseDownEventArgs (this NSEvent theEvent, RectangleF bounds)
        {
            // https://developer.apple.com/library/mac/documentation/Cocoa/Reference/ApplicationKit/Classes/NSEvent_Class/Reference/Reference.html
            return new OxyMouseDownEventArgs {
                Position = theEvent.LocationInWindow.LocationToScreenPoint (bounds),
                ChangedButton = theEvent.Type.ToButton (),
                ModifierKeys = theEvent.ModifierFlags.ToModifierKeys (),
                ClickCount = (int)theEvent.ClickCount,
            };
        }

        public static OxyMouseEventArgs ToMouseEventArgs (this NSEvent theEvent, RectangleF bounds)
        {
            return new OxyMouseEventArgs {
                Position = theEvent.LocationInWindow.LocationToScreenPoint (bounds),
                ModifierKeys = theEvent.ModifierFlags.ToModifierKeys (),
            };
        }

        public static OxyMouseWheelEventArgs ToMouseWheelEventArgs (this NSEvent theEvent, RectangleF bounds)
        {
            return new OxyMouseWheelEventArgs {
                Delta = (int)theEvent.ScrollingDeltaY,
                Position = theEvent.LocationInWindow.LocationToScreenPoint (bounds),
                ModifierKeys = theEvent.ModifierFlags.ToModifierKeys (),
            };
        }

        public static OxyKeyEventArgs ToKeyEventArgs (this NSEvent theEvent)
        {
            return new OxyKeyEventArgs {
                Key = theEvent.KeyCode.ToKey (),
                ModifierKeys = theEvent.ModifierFlags.ToModifierKeys (),
            };
        }

        public static OxyKey ToKey (this ushort keycode)
        {
            // TODO
            return OxyKey.A;
        }
    }
}