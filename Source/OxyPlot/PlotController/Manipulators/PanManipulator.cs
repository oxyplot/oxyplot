// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PanManipulator.cs" company="OxyPlot">
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
//   Provides a manipulator for panning functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides a manipulator for panning functionality.
    /// </summary>
    public class PanManipulator : MouseManipulator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PanManipulator" /> class.
        /// </summary>
        /// <param name="plotControl">The plot control.</param>
        public PanManipulator(IPlotView plotControl)
            : base(plotControl)
        {
        }

        /// <summary>
        /// Gets or sets the previous position.
        /// </summary>
        private ScreenPoint PreviousPosition { get; set; }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);
            if (this.XAxis != null)
            {
                this.XAxis.Pan(this.PreviousPosition, e.Position);
            }

            if (this.YAxis != null)
            {
                this.YAxis.Pan(this.PreviousPosition, e.Position);
            }

            this.PlotControl.InvalidatePlot(false);
            this.PreviousPosition = e.Position;
        }

        /// <summary>
        /// Gets the cursor for the manipulation.
        /// </summary>
        /// <returns>The cursor.</returns>
        public override CursorType GetCursorType()
        {
            return CursorType.Pan;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            this.PreviousPosition = e.Position;
        }
    }
}