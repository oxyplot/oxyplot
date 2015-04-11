// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomStepManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a plot view manipulator for stepwise zoom functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides a plot view manipulator for stepwise zoom functionality.
    /// </summary>
    public class ZoomStepManipulator : MouseManipulator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomStepManipulator" /> class.
        /// </summary>
        /// <param name="plotView">The plot view.</param>
        public ZoomStepManipulator(IPlotView plotView)
            : base(plotView)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether FineControl.
        /// </summary>
        public bool FineControl { get; set; }

        /// <summary>
        /// Gets or sets Step.
        /// </summary>
        public double Step { get; set; }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);

            var isZoomEnabled = (this.XAxis != null && this.XAxis.IsZoomEnabled)
                                || (this.YAxis != null && this.YAxis.IsZoomEnabled);

            if (!isZoomEnabled)
            {
                return;
            }

            var current = this.InverseTransform(e.Position.X, e.Position.Y);

            var scale = this.Step;
            if (this.FineControl)
            {
                scale *= 3;
            }

            scale = 1 + scale;

            // make sure the zoom factor is not negative
            if (scale < 0.1)
            {
                scale = 0.1;
            }

            if (this.XAxis != null)
            {
                this.XAxis.ZoomAt(scale, current.X);
            }

            if (this.YAxis != null)
            {
                this.YAxis.ZoomAt(scale, current.Y);
            }

            this.PlotView.InvalidatePlot(false);
            e.Handled = true;
        }
    }
}