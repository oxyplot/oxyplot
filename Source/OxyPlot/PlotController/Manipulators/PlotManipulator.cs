// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for plot manipulators.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using OxyPlot.Axes;

    /// <summary>
    /// Provides an abstract base class for plot manipulators.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments.</typeparam>
    public abstract class PlotManipulator<T> : ManipulatorBase<T> where T : OxyInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotManipulator{T}" /> class.
        /// </summary>
        /// <param name="view">The plot view.</param>
        protected PlotManipulator(IPlotView view)
            : base(view)
        {
            this.PlotView = view;
        }

        /// <summary>
        /// Gets the plot view where the event was raised.
        /// </summary>
        /// <value>The plot view.</value>
        public IPlotView PlotView { get; private set; }

        /// <summary>
        /// Gets or sets the X axis.
        /// </summary>
        /// <value>The X axis.</value>
        protected Axis XAxis { get; set; }

        /// <summary>
        /// Gets or sets the Y axis.
        /// </summary>
        /// <value>The Y axis.</value>
        protected Axis YAxis { get; set; }

        /// <summary>
        /// Transforms a point from screen coordinates to data coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A data point.</returns>
        protected DataPoint InverseTransform(double x, double y)
        {
            if (this.XAxis != null)
            {
                return this.XAxis.InverseTransform(x, y, this.YAxis);
            }

            if (this.YAxis != null)
            {
                return new DataPoint(0, this.YAxis.InverseTransform(y));
            }

            return new DataPoint();
        }

        /// <summary>
        /// Assigns the axes to this manipulator by the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        protected void AssignAxes(ScreenPoint position)
        {
            Axis xaxis;
            Axis yaxis;
            if (this.PlotView.ActualModel != null)
            {
                this.PlotView.ActualModel.GetAxesFromPoint(position, out xaxis, out yaxis);
            }
            else
            {
                xaxis = null;
                yaxis = null;
            }

            this.XAxis = xaxis;
            this.YAxis = yaxis;
        }
    }
}