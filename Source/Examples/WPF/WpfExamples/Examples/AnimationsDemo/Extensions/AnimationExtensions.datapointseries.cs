// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OxyPlot;
    using OxyPlot.Series;

    public static partial class AnimationExtensions
    {
        public static async Task AnimateSeriesAsync(
            this PlotModel plotModel,
            DataPointSeries series,
            List<AnimationFrame> animationFrames)
        {
            if (animationFrames.Count == 0)
            {
                return;
            }

            var xAxis = plotModel.DefaultXAxis;
            var oldXAxisMinimum = xAxis.Minimum;
            var oldXAxisMaximum = xAxis.Maximum;

            xAxis.Minimum = xAxis.ActualMinimum;
            xAxis.Maximum = xAxis.ActualMaximum;

            var yAxis = plotModel.DefaultYAxis;
            var oldYAxisMinimum = yAxis.Minimum;
            var oldYAxisMaximum = yAxis.Maximum;

            yAxis.Minimum = yAxis.ActualMinimum;
            yAxis.Maximum = yAxis.ActualMaximum;

            // First frame animation
            await Task.Delay(15);

            // Always fix up the data fields (we are using IAnimatablePoint from now on)
            series.DataFieldX = "X";
            series.DataFieldY = "Y";

            var points = series.GetAnimatablePoints();

            foreach (var animationFrame in animationFrames)
            {
                // TODO: consider implementing the IsVisible feature

                for (var j = 0; j < points.Count; j++)
                {
                    var animatablePoint = points[j];
                    if (animatablePoint != null)
                    {
                        var animationPoint = animationFrame.AnimationPoints[j];

                        animatablePoint.X = animationPoint.X;
                        animatablePoint.Y = animationPoint.Y;
                    }
                }

                plotModel.InvalidatePlot(true);

                await Task.Delay(animationFrame.Duration);
            }

            xAxis.Minimum = oldXAxisMinimum;
            xAxis.Maximum = oldXAxisMaximum;

            yAxis.Minimum = oldYAxisMinimum;
            yAxis.Maximum = oldYAxisMaximum;
        }
    }
}