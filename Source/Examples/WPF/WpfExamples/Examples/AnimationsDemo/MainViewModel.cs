// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public class MainViewModel
    {
        public MainViewModel()
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

            var plotModel = new PlotModel
            {
                Title = "Animations demo",
            };

            var series = new LinearBarSeries
            {
                Title = "P & L",
                ItemsSource = pnls,
                DataFieldX = "Time",
                DataFieldY = "Value",
                FillColor = OxyColor.Parse("#454CAF50"),
                StrokeColor = OxyColor.Parse("#4CAF50"),
                StrokeThickness = 1,
                BarWidth = 5
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

            this.EasingFunctions = (from type in typeof(CircleEase).Assembly.GetTypes()
                                    where typeof(EasingFunctionBase).IsAssignableFrom(type) && !type.IsAbstract
                                    select type).ToList();
            this.SelectedEasingFunction = this.EasingFunctions.FirstOrDefault();

            this.AnimationDuration = 250;
            this.PlotModel = plotModel;

            this.Animate();
        }

        public PlotModel PlotModel { get; private set; }

        public List<Type> EasingFunctions { get; private set; }

        public Type SelectedEasingFunction { get; set; }

        public int AnimationDuration { get; set; }

        public void Animate()
        {
            var plotModel = this.PlotModel;
            var series = plotModel.Series.First() as ItemsSeries;

            var easingFunction = (EasingFunctionBase)Activator.CreateInstance(this.SelectedEasingFunction);

            plotModel.AnimateSeries(series, easingFunction, duration: TimeSpan.FromMilliseconds(this.AnimationDuration));
        }
    }

    public class Pnl
    {
        public DateTime Time { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return String.Format("{0:HH:mm} {1:0.0}", this.Time, this.Value);
        }
    }
}