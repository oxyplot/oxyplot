// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for scatter plots.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;

    /// <summary>
    /// Represents a series for scatter plots.
    /// </summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Scatter_plot</remarks>
    public class ScatterSeries : ScatterSeries<ScatterPoint>
    {
        /// <summary>
        /// Updates the <see cref="F:ItemsSourcePoints" /> from the <see cref="P:ItemsSource" /> and data fields.
        /// </summary>
        protected override void UpdateFromDataFields()
        {
            var filler = new ListBuilder<ScatterPoint>();
            filler.Add(this.DataFieldX, double.NaN);
            filler.Add(this.DataFieldY, double.NaN);
            filler.Add(this.DataFieldSize, double.NaN);
            filler.Add(this.DataFieldValue, double.NaN);
            filler.Add(this.DataFieldTag, (object)null);
            filler.FillT(this.ItemsSourcePoints, this.ItemsSource, args => new ScatterPoint(Axes.Axis.ToDouble(args[0]), Axes.Axis.ToDouble(args[1]), Axes.Axis.ToDouble(args[2]), Axes.Axis.ToDouble(args[3]), args[4]));
        }
    }
}