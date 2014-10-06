// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolygonAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("PolygonAnnotation"), Tags("Annotations")]
    public static class PolygonAnnotationExamples
    {
        [Example("PolygonAnnotation")]
        public static PlotModel PolygonAnnotation()
        {
            var model = new PlotModel { Title = "PolygonAnnotation" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            var a1 = new PolygonAnnotation { Text = "Polygon 1" };
            a1.Points.AddRange(new[] { new DataPoint(4, -2), new DataPoint(8, -4), new DataPoint(17, 7), new DataPoint(5, 8), new DataPoint(2, 5) });
            model.Annotations.Add(a1);
            return model;
        }

        [Example("PolygonAnnotation with custom text position and alignment")]
        public static PlotModel PolygonAnnotationTextPosition()
        {
            var model = new PlotModel { Title = "PolygonAnnotation with fixed text position and alignment" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            var a1 = new PolygonAnnotation { Text = "Polygon 1", TextHorizontalAlignment = HorizontalAlignment.Left, TextVerticalAlignment = VerticalAlignment.Bottom, TextPosition = new DataPoint(4.1, -1.9) };
            a1.Points.AddRange(new[] { new DataPoint(4, -2), new DataPoint(8, -2), new DataPoint(17, 7), new DataPoint(5, 8), new DataPoint(4, 5) });
            model.Annotations.Add(a1);
            return model;
        }

        [Example("AnnotationLayer property")]
        public static PlotModel AnnotationLayerProperty()
        {
            var model = new PlotModel { Title = "Annotation Layers" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 30, MajorGridlineStyle = LineStyle.Solid, MajorGridlineThickness = 1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10, MajorGridlineStyle = LineStyle.Solid, MajorGridlineThickness = 1 });

            var a1 = new PolygonAnnotation
                         {
                             Layer = AnnotationLayer.BelowAxes,
                             Text = "Layer = BelowAxes"
                         };
            a1.Points.AddRange(new[]
                                   {
                                       new DataPoint(-11, -2), new DataPoint(-7, -4), new DataPoint(-3, 7), new DataPoint(-10, 8),
                                       new DataPoint(-13, 5)
                                   });
            model.Annotations.Add(a1);
            var a2 = new PolygonAnnotation
                         {
                             Layer = AnnotationLayer.BelowSeries,
                             Text = "Layer = BelowSeries"
                         };
            a2.Points.AddRange(new DataPoint[]
                                   {
                                       new DataPoint(4, -2), new DataPoint(8, -4), new DataPoint(12, 7), new DataPoint(5, 8),
                                       new DataPoint(2, 5)
                                   });
            model.Annotations.Add(a2);
            var a3 = new PolygonAnnotation { Layer = AnnotationLayer.AboveSeries, Text = "Layer = AboveSeries" };
            a3.Points.AddRange(new[] { new DataPoint(19, -2), new DataPoint(23, -4), new DataPoint(27, 7), new DataPoint(20, 8), new DataPoint(17, 5) });
            model.Annotations.Add(a3);

            model.Series.Add(new FunctionSeries(Math.Sin, -20, 30, 400));
            return model;
        }
    }
}