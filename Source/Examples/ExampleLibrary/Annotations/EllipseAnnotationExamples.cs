// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EllipseAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;

    [Examples("EllipseAnnotation"), Tags("Annotations")]
    public static class EllipseAnnotationExamples
    {
        [Example("EllipseAnnotation")]
        public static PlotModel EllipseAnnotation()
        {
            var model = new PlotModel { Title = "EllipseAnnotation" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Annotations.Add(new EllipseAnnotation { X = 20, Y = 60, Width = 20, Height = 15, Text = "EllipseAnnotation", TextRotation = 10, Fill = OxyColor.FromAColor(99, OxyColors.Green), Stroke = OxyColors.Black, StrokeThickness = 2 });

            model.Annotations.Add(new EllipseAnnotation { X = 20, Y = 20, Width = 20, Height = 20, Fill = OxyColor.FromAColor(99, OxyColors.Green), Stroke = OxyColors.Black, StrokeThickness = 2 });
            model.Annotations.Add(new EllipseAnnotation { X = 30, Y = 20, Width = 20, Height = 20, Fill = OxyColor.FromAColor(99, OxyColors.Red), Stroke = OxyColors.Black, StrokeThickness = 2 });
            model.Annotations.Add(new EllipseAnnotation { X = 25, Y = 30, Width = 20, Height = 20, Fill = OxyColor.FromAColor(99, OxyColors.Blue), Stroke = OxyColors.Black, StrokeThickness = 2 });
            return model;
        }
    }
}