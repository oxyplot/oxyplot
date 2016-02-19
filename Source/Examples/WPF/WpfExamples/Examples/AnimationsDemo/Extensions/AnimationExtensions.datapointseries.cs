// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OxyPlot;
    using OxyPlot.Series;

    public static partial class AnimationExtensions
    {
        public static AnimationFrame GetFinalAnimationFrame(DataPointSeries series)
        {
            var animationFrame = new AnimationFrame
            {
                Duration = TimeSpan.Zero
            };

            var points = series.GetAnimatablePoints();
            foreach (var point in points)
            {
                animationFrame.AnimationPoints.Add(new AnimationPoint
                {
                    X = point.FinalX,
                    Y = point.FinalY
                });
            }

            return animationFrame;
        }

        public static async Task AnimateSeriesAsync(
            this PlotModel plotModel,
            DataPointSeries series,
            List<AnimationFrame> animationFrames)
        {
            if (animationFrames.Count == 0)
            {
                return;
            }

            var finalAnimationFrame = GetFinalAnimationFrame(series);
            animationFrames.Add(finalAnimationFrame);

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

            var previousDataFieldX = series.DataFieldX;
            var previousDataFieldY = series.DataFieldY;

            // Always fix up the data fields (we are using IAnimatablePoint from now on)
            series.DataFieldX = "X";
            series.DataFieldY = "Y";

            var points = series.GetAnimatablePoints();

            foreach (var animationFrame in animationFrames)
            {
                // TODO: consider implementing the IsVisible feature

                var animationPoints = animationFrame.AnimationPoints;
                if (animationPoints.Count > 0)
                {
                    for (var j = 0; j < points.Count; j++)
                    {
                        var animatablePoint = points[j];
                        if (animatablePoint != null)
                        {
                            if (j < animationPoints.Count)
                            {
                                var animationPoint = animationPoints[j];

                                animatablePoint.X = animationPoint.X;
                                animatablePoint.Y = animationPoint.Y;
                            }
                        }
                    }
                }

                plotModel.InvalidatePlot(true);

                await Task.Delay(animationFrame.Duration);
            }

            xAxis.Minimum = oldXAxisMinimum;
            xAxis.Maximum = oldXAxisMaximum;

            yAxis.Minimum = oldYAxisMinimum;
            yAxis.Maximum = oldYAxisMaximum;

            series.DataFieldX = previousDataFieldX;
            series.DataFieldY = previousDataFieldY;

            plotModel.InvalidatePlot(true);
        }
    }
}