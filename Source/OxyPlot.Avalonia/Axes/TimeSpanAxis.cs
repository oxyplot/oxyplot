// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.TimeSpanAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Avalonia
{
    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.TimeSpanAxis.
    /// </summary>
    public class TimeSpanAxis : Axis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "TimeSpanAxis" /> class.
        /// </summary>
        public TimeSpanAxis()
        {
            InternalAxis = new Axes.TimeSpanAxis();
        }

        /// <summary>
        /// Creates the internal axis.
        /// </summary>
        /// <returns>The internal axis.</returns>
        public override Axes.Axis CreateModel()
        {
            SynchronizeProperties();
            return InternalAxis;
        }

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            // ReSharper disable once UnusedVariable
            var a = (Axes.TimeSpanAxis)InternalAxis;
            //// Currently no values to synchronize
        }
    }
}