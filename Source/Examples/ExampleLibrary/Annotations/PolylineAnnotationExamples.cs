// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolylineAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;

    [Examples("PolylineAnnotation"), Tags("Annotations")]
    public static class PolylineAnnotationExamples
    {
        [Example("PolylineAnnotation")]
        public static PlotModel PolylineAnnotations()
        {
            var model = new PlotModel { Title = "PolylineAnnotation" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 30 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 30 });
            var a1 = new PolylineAnnotation { Text = "Polyline" };
            a1.Points.AddRange(new[] { new DataPoint(0, 10), new DataPoint(5, 5), new DataPoint(20, 1), new DataPoint(30, 20) });
            var a2 = new PolylineAnnotation
            {
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
                Text = "Smooth Polyline"
            };
            a2.Points.AddRange(new[] { new DataPoint(0, 15), new DataPoint(3, 23), new DataPoint(9, 30), new DataPoint(20, 12), new DataPoint(30, 10) });
            model.Annotations.Add(a1);
            model.Annotations.Add(a2);
            return model;
        }
    }
}