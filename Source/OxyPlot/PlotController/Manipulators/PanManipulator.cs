// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PanManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
        /// <param name="plotView">The plot view.</param>
        public PanManipulator(IPlotView plotView)
            : base(plotView)
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

            this.PlotView.InvalidatePlot(false);
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