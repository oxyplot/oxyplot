// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("RectangleAnnotation"), Tags("Annotations")]
    public static class RectangleAnnotationExamples
    {
        [Example("RectangleAnnotation")]
        public static PlotModel RectangleAnnotation()
        {
            var model = new PlotModel { Title = "RectangleAnnotation" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 20, MaximumX = 70, MinimumY = 10, MaximumY = 40, TextRotation = 10, Text = "RectangleAnnotation", Fill = OxyColor.FromAColor(99, OxyColors.Blue), Stroke = OxyColors.Black, StrokeThickness = 2 });
            return model;
        }

        [Example("RectangleAnnotations - vertical limit")]
        public static PlotModel RectangleAnnotationVerticalLimit()
        {
            var model = new PlotModel { Title = "RectangleAnnotations - vertical limit" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Annotations.Add(new RectangleAnnotation { MaximumY = 89.5, Text = "Valid area", Fill = OxyColor.FromAColor(99, OxyColors.Black) });
            return model;
        }

        [Example("RectangleAnnotation - horizontal bands")]
        public static PlotModel RectangleAnnotationHorizontals()
        {
            var model = new PlotModel { Title = "RectangleAnnotation - horizontal bands" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 10 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 87, Maximum = 97, MajorStep = 1, MinorStep = 1 });
            model.Annotations.Add(new RectangleAnnotation { MinimumY = 89.5, MaximumY = 90.8, Text = "Invalid", Fill = OxyColor.FromAColor(99, OxyColors.Red) });
            model.Annotations.Add(new RectangleAnnotation { MinimumY = 90.8, MaximumY = 92.1, Fill = OxyColor.FromAColor(99, OxyColors.Orange) });
            model.Annotations.Add(new RectangleAnnotation { MinimumY = 92.1, MaximumY = 94.6, Fill = OxyColor.FromAColor(99, OxyColors.Yellow) });
            model.Annotations.Add(new RectangleAnnotation { MinimumY = 94.6, MaximumY = 96, Text = "Ok", Fill = OxyColor.FromAColor(99, OxyColors.Green) });
            LineSeries series1;
            model.Series.Add(series1 = new LineSeries { Color = OxyColors.Black, StrokeThickness = 6.0, LineJoin = LineJoin.Round });
            series1.Points.Add(new DataPoint(0.5, 90.7));
            series1.Points.Add(new DataPoint(1.5, 91.2));
            series1.Points.Add(new DataPoint(2.5, 91));
            series1.Points.Add(new DataPoint(3.5, 89.5));
            series1.Points.Add(new DataPoint(4.5, 92.5));
            series1.Points.Add(new DataPoint(5.5, 93.1));
            series1.Points.Add(new DataPoint(6.5, 94.5));
            series1.Points.Add(new DataPoint(7.5, 95.5));
            series1.Points.Add(new DataPoint(8.5, 95.7));
            series1.Points.Add(new DataPoint(9.5, 96.0));
            return model;
        }

        [Example("RectangleAnnotation - vertical bands")]
        public static PlotModel RectangleAnnotationVerticals()
        {
            var model = new PlotModel { Title = "RectangleAnnotation - vertical bands" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 10 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 87, Maximum = 97, MajorStep = 1, MinorStep = 1 });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 2.5, MaximumX = 2.8, TextRotation = 90, Text = "Red", Fill = OxyColor.FromAColor(99, OxyColors.Red) });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 2.8, MaximumX = 6.1, TextRotation = 90, Text = "Orange", Fill = OxyColor.FromAColor(99, OxyColors.Orange) });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 6.1, MaximumX = 7.6, TextRotation = 90, Text = "Yellow", Fill = OxyColor.FromAColor(99, OxyColors.Yellow) });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 7.6, MaximumX = 9.7, TextRotation = 270, Text = "Green", Fill = OxyColor.FromAColor(99, OxyColors.Green) });
            LineSeries series1;
            model.Series.Add(series1 = new LineSeries { Color = OxyColors.Black, StrokeThickness = 6.0, LineJoin = LineJoin.Round });
            series1.Points.Add(new DataPoint(0.5, 90.7));
            series1.Points.Add(new DataPoint(1.5, 91.2));
            series1.Points.Add(new DataPoint(2.5, 91));
            series1.Points.Add(new DataPoint(3.5, 89.5));
            series1.Points.Add(new DataPoint(4.5, 92.5));
            series1.Points.Add(new DataPoint(5.5, 93.1));
            series1.Points.Add(new DataPoint(6.5, 94.5));
            series1.Points.Add(new DataPoint(7.5, 95.5));
            series1.Points.Add(new DataPoint(8.5, 95.7));
            series1.Points.Add(new DataPoint(9.5, 96.0));
            return model;
        }
    }
}