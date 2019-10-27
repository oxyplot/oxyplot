// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.TimeSpanAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    /// <summary>
    /// This is a WPF wrapper of OxyPlot.TimeSpanAxis.
    /// </summary>
    public class TimeSpanAxis : Axis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "TimeSpanAxis" /> class.
        /// </summary>
        public TimeSpanAxis()
        {
            this.InternalAxis = new Axes.TimeSpanAxis();
        }

        /// <summary>
        /// Creates the internal axis.
        /// </summary>
        /// <returns>The internal axis.</returns>
        public override Axes.Axis CreateModel()
        {
            this.SynchronizeProperties();
            return this.InternalAxis;
        }

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            // ReSharper disable once UnusedVariable
            var a = (Axes.TimeSpanAxis)this.InternalAxis;
            //// Currently no values to synchronize
        }
    }
}