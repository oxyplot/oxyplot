// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaSeriesViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public class AreaSeriesViewModel : AnimationViewModelBase
    {
        public AreaSeriesViewModel()
        {
            var pnls = new List<Pnl>();

            var random = new Random(31);
            var dateTime = DateTime.Today.Add(TimeSpan.FromHours(9));
            for (var pointIndex = 0; pointIndex < 50; pointIndex++)
            {
                pnls.Add(new Pnl
                {
                    Time = dateTime,
                    Value = -200 + random.Next(1000),
                });
                dateTime = dateTime.AddMinutes(1);
            }

            var minimum = pnls.Min(x => x.Value);
            var maximum = pnls.Max(x => x.Value);

            var plotModel = this.PlotModel;
            plotModel.Title = "Area Series Animation Demo";

            var series = new AreaSeries
            {
                Title = "P & L",
                ItemsSource = pnls,
                DataFieldX = "Time",
                DataFieldY = "Value",
                Color = OxyColor.Parse("#4CAF50"),
                Fill = OxyColor.Parse("#454CAF50"),
                MarkerSize = 3,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse("#4CAF50"),
                MarkerStrokeThickness = 1.5,
                MarkerType = MarkerType.Circle,
                StrokeThickness = 1,
            };
            plotModel.Series.Add(series);

            var annotation = new LineAnnotation
            {
                Type = LineAnnotationType.Horizontal,
                Y = 0
            };
            plotModel.Annotations.Add(annotation);

            var dateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                IntervalType = DateTimeIntervalType.Hours,
                IntervalLength = 50
            };
            plotModel.Axes.Add(dateTimeAxis);

            var margin = (maximum - minimum) * 0.05;

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = minimum - margin,
                Maximum = maximum + margin,
            };
            plotModel.Axes.Add(valueAxis);
        }

        public override bool SupportsEasingFunction { get { return true; } }

        public override async Task AnimateAsync(AnimationSettings animationSettings)
        {
            var plotModel = this.PlotModel;
            var series = plotModel.Series.First() as AreaSeries;
            if (series != null)
            {
                await plotModel.AnimateSeriesAsync(series, animationSettings);
            }
        }
    }
}