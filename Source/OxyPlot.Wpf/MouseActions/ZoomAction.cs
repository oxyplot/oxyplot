// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomAction.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Windows;
using System.Windows.Input;

namespace OxyPlot.Wpf
{
    public class ZoomAction : MouseAction
    {
        public Point DownPoint;
        private bool isZooming;
        private OxyPlot.AxisBase xaxis;
        private OxyPlot.AxisBase yaxis;

        private Rect zoomRectangle;

        public ZoomAction(PlotControl pc)
            : base(pc)
        {
        }

        public override void OnMouseDown(Point pt, MouseButton button, int clickCount, bool control, bool shift)
        {
            base.OnMouseDown(pt, button, clickCount, control, shift);

            // RMB+Control is the same
            if (button == MouseButton.Right && control)
                button = MouseButton.Middle;

            if (button != MouseButton.Middle)
                return;

            // Middle button double click resets the axis
            if (clickCount == 2)
            {
                if (xaxis != null) pc.Reset(xaxis);
                if (yaxis != null) pc.Reset(yaxis);
                pc.Refresh();
            }

            pc.GetAxesFromPoint(pt, out xaxis, out yaxis);
            DownPoint = pt;
            zoomRectangle = new Rect(pt, pt);
            pc.ShowZoomRectangle(zoomRectangle);
            pc.CaptureMouse();
            pc.Cursor = Cursors.SizeNWSE;
            isZooming = true;
        }

        public override void OnMouseMove(Point pt, bool control, bool shift)
        {
            if (!isZooming)
                return;
            if (pc.Model==null)
                return;

            if (yaxis == null)
            {
                DownPoint.Y = pc.Model.PlotMargins.Top;
                pt.Y = pc.ActualHeight - pc.Model.PlotMargins.Bottom;
            }
            if (xaxis == null)
            {
                DownPoint.X = pc.Model.PlotMargins.Left;
                pt.X = pc.ActualWidth - pc.Model.PlotMargins.Right;
            }

            zoomRectangle = CreateRect(DownPoint, pt);
            pc.ShowZoomRectangle(zoomRectangle);
        }

        private static Rect CreateRect(Point p0, Point p1)
        {
            double x = Math.Min(p0.X, p1.X);
            double w = Math.Abs(p0.X - p1.X);
            double y = Math.Min(p0.Y, p1.Y);
            double h = Math.Abs(p0.Y - p1.Y);
            return new Rect(x, y, w, h);
        }

        public override void OnMouseUp()
        {
            if (!isZooming)
                return;

            pc.Cursor = Cursors.Arrow;
            pc.ReleaseMouseCapture();
            pc.HideZoomRectangle();

            if (zoomRectangle.Width > 10 && zoomRectangle.Height > 10)
            {
                DataPoint p0 = pc.InverseTransform(zoomRectangle.TopLeft, xaxis, yaxis);
                DataPoint p1 = pc.InverseTransform(zoomRectangle.BottomRight, xaxis, yaxis);

                if (xaxis != null)
                    pc.Zoom(xaxis, p0.X, p1.X);
                if (yaxis != null)
                    pc.Zoom(yaxis, p0.Y, p1.Y);
                pc.Refresh();
            }
            isZooming = false;
            base.OnMouseUp();
        }

        public override void OnMouseWheel(Point pt, double delta, bool control, bool shift)
        {
            OxyPlot.AxisBase xa, ya;
            pc.GetAxesFromPoint(pt, out xa, out ya);
            DataPoint current = pc.InverseTransform(pt, xa, ya);
            double f = 0.001;
            if (control) f *= 0.2;
            double s = 1 + delta*f;
            if (xa != null)
                pc.ZoomAt(xa,s, current.X);
            if (ya != null)
                pc.ZoomAt(ya,s, current.Y);
            pc.Refresh();
        }
    }
}