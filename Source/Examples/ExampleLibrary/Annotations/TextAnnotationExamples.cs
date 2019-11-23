// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;

    [Examples("TextAnnotation"), Tags("Annotations")]
    public static class TextAnnotationExamples
    {
        [Example("TextAnnotation")]
        public static PlotModel TextAnnotations()
        {
            var model = new PlotModel { Title = "TextAnnotation" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -15, Maximum = 25 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -5, Maximum = 18 });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(-6, 0), Text = "Text annotation 1" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(-7, 10), TextRotation = 80, Text = "Text annotation 2" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(2, 2), TextRotation = 20, TextHorizontalAlignment = HorizontalAlignment.Right, TextVerticalAlignment = VerticalAlignment.Top, Text = "Right/Top" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(2, 4), TextRotation = 20, TextHorizontalAlignment = HorizontalAlignment.Right, TextVerticalAlignment = VerticalAlignment.Middle, Text = "Right/Middle" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(2, 6), TextRotation = 20, TextHorizontalAlignment = HorizontalAlignment.Right, TextVerticalAlignment = VerticalAlignment.Bottom, Text = "Right/Bottom" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(10, 2), TextRotation = 20, TextHorizontalAlignment = HorizontalAlignment.Center, TextVerticalAlignment = VerticalAlignment.Top, Text = "Center/Top" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(10, 4), TextRotation = 20, TextHorizontalAlignment = HorizontalAlignment.Center, TextVerticalAlignment = VerticalAlignment.Middle, Text = "Center/Middle" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(10, 6), TextRotation = 20, TextHorizontalAlignment = HorizontalAlignment.Center, TextVerticalAlignment = VerticalAlignment.Bottom, Text = "Center/Bottom" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(18, 2), TextRotation = 20, TextHorizontalAlignment = HorizontalAlignment.Left, TextVerticalAlignment = VerticalAlignment.Top, Text = "Left/Top" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(18, 4), TextRotation = 20, TextHorizontalAlignment = HorizontalAlignment.Left, TextVerticalAlignment = VerticalAlignment.Middle, Text = "Left/Middle" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(18, 6), TextRotation = 20, TextHorizontalAlignment = HorizontalAlignment.Left, TextVerticalAlignment = VerticalAlignment.Bottom, Text = "Left/Bottom" });

            double d = 0.05;

            Action<double, double> addPoint = (x, y) =>
                {
                    var annotation = new PolygonAnnotation
                                         {
                                             Layer = AnnotationLayer.BelowAxes,
                                         };
                    annotation.Points.AddRange(new[]
                                                   {
                                                       new DataPoint(x - d, y - d), new DataPoint(x + d, y - d), new DataPoint(x + d, y + d),
                                                       new DataPoint(x - d, y + d), new DataPoint(x - d, y - d)
                                                   });
                    model.Annotations.Add(annotation);
                };

            foreach (var a in model.Annotations.ToArray())
            {
                var ta = a as TextAnnotation;
                if (ta != null)
                {
                    addPoint(ta.TextPosition.X, ta.TextPosition.Y);
                }
            }

            return model;
        }

        [Example("Rotations")]
        public static PlotModel Rotations()
        {
            var model = new PlotModel { Title = "TextAnnotation Rotations" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -5, Maximum = 45 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0, Minimum = -1, Maximum = 8 });
            for (var i = 0; i < 360; i += 5)
            {
                model.Annotations.Add(new TextAnnotation
                                          {
                                              TextRotation = i, 
                                              TextPosition = new DataPoint(i % 45, i / 45), 
                                              Text = $"{i}°", 
                                              TextVerticalAlignment = VerticalAlignment.Middle, 
                                              TextHorizontalAlignment = HorizontalAlignment.Center
                                          });
            }

            return model;
        }
    }
}
