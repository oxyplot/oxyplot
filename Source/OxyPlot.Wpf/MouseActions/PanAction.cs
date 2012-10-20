// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PanAction.cs" company="OxyPlot">
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
using System.Windows.Input;

namespace OxyPlot.Wpf
{
    public class PanAction : MouseAction
    {
        public PanAction(PlotControl pc)
            : base(pc)
        {
        }

        private OxyPlot.AxisBase xaxis;
        private OxyPlot.AxisBase yaxis;
        private DataPoint previousPoint;

        private bool isPanning;

        public override void OnMouseDown(System.Windows.Point pt, MouseButton button, int clickCount, bool control, bool shift)
        {
            if (button != MouseButton.Right || control)
                return;
            pc.GetAxesFromPoint(pt, out xaxis, out yaxis);

            // Right button double click resets the axis
            if (clickCount == 2)
            {
                if (xaxis != null) xaxis.Reset();
                if (yaxis != null) yaxis.Reset();
                pc.Refresh();
            }

            previousPoint = pc.InverseTransform(pt, xaxis, yaxis);
            pc.CaptureMouse();
            pc.Cursor = Cursors.SizeAll;
            isPanning = true;
        }

        public override void OnMouseMove(System.Windows.Point pt, bool control, bool shift)
        {
            if (!isPanning)
                return;
            var currentPoint = pc.InverseTransform(pt, xaxis, yaxis);
            double dx = currentPoint.X - previousPoint.X;
            double dy = currentPoint.Y - previousPoint.Y;
            if (xaxis != null)
                pc.Pan(xaxis, -dx);
            if (yaxis != null)
                pc.Pan(yaxis, -dy);
            pc.Refresh();
            previousPoint = pc.InverseTransform(pt, xaxis, yaxis);
        }

        public override void OnMouseUp()
        {
            if (!isPanning)
                return;

            pc.Cursor = Cursors.Arrow;
            pc.ReleaseMouseCapture();
            isPanning = false;
        }

    }
}