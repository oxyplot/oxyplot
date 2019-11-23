// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrowAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;

    [Examples("ArrowAnnotation"), Tags("Annotations")]
    public static class ArrowAnnotationExamples
    {
        [Example("ArrowAnnotation")]
        public static PlotModel ArrowAnnotation()
        {
            var model = new PlotModel { Title = "ArrowAnnotations" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -40, Maximum = 60 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            model.Annotations.Add(
                new ArrowAnnotation
                    {
                        StartPoint = new DataPoint(8, 4),
                        EndPoint = new DataPoint(0, 0),
                        Color = OxyColors.Green,
                        Text = "StartPoint and EndPoint"
                    });

            model.Annotations.Add(
                new ArrowAnnotation
                    {
                        ArrowDirection = new ScreenVector(30, 70),
                        EndPoint = new DataPoint(40, -3),
                        Color = OxyColors.Blue,
                        Text = "ArrowDirection and EndPoint"
                    });

            model.Annotations.Add(
                new ArrowAnnotation
                    {
                        ArrowDirection = new ScreenVector(30, -70),
                        EndPoint = new DataPoint(10, -3),
                        HeadLength = 14,
                        HeadWidth = 6,
                        Veeness = 4,
                        Color = OxyColors.Red,
                        Text = "HeadLength = 20, HeadWidth = 10, Veeness = 4"
                    });

            return model;
        }

        [Example("Rotations")]
        public static PlotModel Rotations()
        {
            var model = new PlotModel { Title = "ArrowAnnotation Rotations" };
            model.Axes.Add(
                new LinearAxis
                    {
                        Position = AxisPosition.Bottom,
                        Minimum = -5,
                        Maximum = 45,
                        MajorGridlineStyle = LineStyle.Solid
                    });

            model.Axes.Add(
                new LinearAxis
                    {
                        Position = AxisPosition.Left,
                        StartPosition = 1,
                        EndPosition = 0,
                        Minimum = -1,
                        Maximum = 8,
                        MajorGridlineStyle = LineStyle.Solid
                    });

            for (var i = 0; i < 360; i += 5)
            {
                var rad = i / 360d * Math.PI * 2;
                model.Annotations.Add(
                    new ArrowAnnotation
                        {
                            EndPoint = new DataPoint(i % 45, i / 45),
                            Text = $"{i}°",
                            ArrowDirection = new ScreenVector(Math.Cos(rad), Math.Sin(rad)) * 25,
                            HeadLength = 5
                        });
            }

            return model;
        }
    }
}
