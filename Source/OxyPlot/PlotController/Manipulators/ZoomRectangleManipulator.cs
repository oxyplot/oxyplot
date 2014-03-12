// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomRectangleManipulator.cs" company="OxyPlot">
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
// <summary>
//   The zoom manipulator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides a plot control manipulator for zoom by rectangle functionality.
    /// </summary>
    public class ZoomRectangleManipulator : MouseManipulator
    {
        /// <summary>
        /// The zoom rectangle.
        /// </summary>
        private OxyRect zoomRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomRectangleManipulator"/> class.
        /// </summary>
        /// <param name="plotControl">The plot control.</param>
        public ZoomRectangleManipulator(IPlotControl plotControl)
            : base(plotControl)
        {
        }

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public override void Completed(OxyMouseEventArgs e)
        {
            base.Completed(e);

            this.PlotControl.HideZoomRectangle();

            if (this.zoomRectangle.Width > 10 && this.zoomRectangle.Height > 10)
            {
                var p0 = this.InverseTransform(this.zoomRectangle.Left, this.zoomRectangle.Top);
                var p1 = this.InverseTransform(this.zoomRectangle.Right, this.zoomRectangle.Bottom);

                if (this.XAxis != null)
                {
                    this.XAxis.Zoom(p0.X, p1.X);
                }

                if (this.YAxis != null)
                {
                    this.YAxis.Zoom(p0.Y, p1.Y);
                }

                this.PlotControl.InvalidatePlot();
            }
        }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);

            var plotArea = this.PlotControl.ActualModel.PlotArea;

            double x = Math.Min(this.StartPosition.X, e.Position.X);
            double w = Math.Abs(this.StartPosition.X - e.Position.X);
            double y = Math.Min(this.StartPosition.Y, e.Position.Y);
            double h = Math.Abs(this.StartPosition.Y - e.Position.Y);

            if (this.XAxis == null || !this.XAxis.IsZoomEnabled)
            {
                x = plotArea.Left;
                w = plotArea.Width;
            }

            if (this.YAxis == null || !this.YAxis.IsZoomEnabled)
            {
                y = plotArea.Top;
                h = plotArea.Height;
            }

            this.zoomRectangle = new OxyRect(x, y, w, h);
            this.PlotControl.ShowZoomRectangle(this.zoomRectangle);
        }

        /// <summary>
        /// Gets the cursor for the manipulation.
        /// </summary>
        /// <returns>
        /// The cursor.
        /// </returns>
        public override CursorType GetCursorType()
        {
            if (this.XAxis == null)
            {
                return CursorType.ZoomVertical;
            }

            if (this.YAxis == null)
            {
                return CursorType.ZoomHorizontal;
            }

            return CursorType.ZoomRectangle;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            this.zoomRectangle = new OxyRect(this.StartPosition.X, this.StartPosition.Y, 0, 0);
            this.PlotControl.ShowZoomRectangle(this.zoomRectangle);
        }
    }
}