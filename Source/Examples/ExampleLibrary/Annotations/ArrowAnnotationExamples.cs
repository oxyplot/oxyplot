// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrowAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
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
            model.Annotations.Add(new ArrowAnnotation { StartPoint = new DataPoint(8, 4), EndPoint = new DataPoint(0, 0), Color = OxyColors.Green, Text = "StartPoint and EndPoint" });
            model.Annotations.Add(new ArrowAnnotation { ArrowDirection = new ScreenVector(30, 70), EndPoint = new DataPoint(40, -3), Color = OxyColors.Blue, Text = "ArrowDirection and EndPoint" });
            model.Annotations.Add(new ArrowAnnotation { ArrowDirection = new ScreenVector(30, -70), EndPoint = new DataPoint(10, -3), HeadLength = 14, HeadWidth = 6, Veeness = 4, Color = OxyColors.Red, Text = "HeadLength = 20, HeadWidth = 10, Veeness = 4" });
            return model;
        }
    }
}