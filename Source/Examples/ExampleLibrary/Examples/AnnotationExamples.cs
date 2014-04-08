// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotationExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using OxyPlot;

namespace ExampleLibrary
{
    using System;
    using System.Linq;
    using System.Reflection;

    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Annotations")]
    public static class AnnotationExamples
    {
        [Example("LineAnnotation on linear axes")]
        public static PlotModel LineAnnotationOnLinearAxes()
        {
            var model = new PlotModel("LineAnnotation on linear axes");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            model.Annotations.Add(new LineAnnotation { Slope = 0.1, Intercept = 1, Text = "First" });
            model.Annotations.Add(new LineAnnotation { Slope = 0.3, Intercept = 2, MaximumX = 40, Color = OxyColors.Red, Text = "Second" });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Vertical, X = 4, MaximumY = 10, Color = OxyColors.Green, Text = "Vertical" });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Horizontal, Y = 2, MaximumX = 4, Color = OxyColors.Gold, Text = "Horizontal" });
            return model;
        }

        [Example("LineAnnotation on logarithmic axes")]
        public static PlotModel LineAnnotationOnLogarithmicAxes()
        {
            var model = new PlotModel("LineAnnotation on logarithmic axes");
            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Bottom, Minimum = 1, Maximum = 80 });
            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left, Minimum = 1, Maximum = 10 });
            model.Annotations.Add(new LineAnnotation { Slope = 0.1, Intercept = 1, Text = "First", TextMargin = 40 });
            model.Annotations.Add(new LineAnnotation { Slope = 0.3, Intercept = 2, MaximumX = 40, Color = OxyColors.Red, Text = "Second" });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Vertical, X = 4, MaximumY = 10, Color = OxyColors.Green, Text = "Vertical" });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Horizontal, Y = 2, MaximumX = 4, Color = OxyColors.Gold, Text = "Horizontal" });
            return model;
        }

        [Example("LineAnnotation with text orientation specified")]
        public static PlotModel LineAnnotationOnLinearAxesWithTextOrientation()
        {
            var model = new PlotModel("LineAnnotations", "with TextOrientation specified");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            model.Annotations.Add(new LineAnnotation { Slope = 0.1, Intercept = 1, Text = "Horizontal", TextOrientation = AnnotationTextOrientation.Horizontal, TextVerticalAlignment = VerticalAlignment.Bottom });
            model.Annotations.Add(new LineAnnotation { Slope = 0.3, Intercept = 2, MaximumX = 40, Color = OxyColors.Red, Text = "Vertical", TextOrientation = AnnotationTextOrientation.Vertical });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Vertical, X = 4, MaximumY = 10, Color = OxyColors.Green, Text = "Horizontal (x=4)", TextPadding = 8, TextOrientation = AnnotationTextOrientation.Horizontal });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Vertical, X = 45, MaximumY = 10, Color = OxyColors.Green, Text = "Horizontal (x=45)", TextHorizontalAlignment = HorizontalAlignment.Left, TextPadding = 8, TextOrientation = AnnotationTextOrientation.Horizontal });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Horizontal, Y = 2, MaximumX = 4, Color = OxyColors.Gold, Text = "Horizontal", TextPosition = 0.5, TextOrientation = AnnotationTextOrientation.Horizontal });
            return model;
        }

        [Example("FunctionAnnotation")]
        public static PlotModel FunctionAnnotation()
        {
            var model = new PlotModel("FunctionAnnotation");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            model.Annotations.Add(new FunctionAnnotation { Equation = Math.Sin, StrokeThickness = 2, Color = OxyColor.FromAColor(120, OxyColors.Blue), Text = "f(x)=sin(x)" });
            model.Annotations.Add(new FunctionAnnotation { Equation = y => y * y, StrokeThickness = 2, Color = OxyColor.FromAColor(120, OxyColors.Red), Type = FunctionAnnotationType.EquationY, Text = "f(y)=y^2" });
            return model;
        }

        [Example("RectangleAnnotation")]
        public static PlotModel RectangleAnnotation()
        {
            var model = new PlotModel("RectangleAnnotation");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 20, MaximumX = 70, MinimumY = 10, MaximumY = 40, TextRotation = 10, Text = "RectangleAnnotation", Fill = OxyColor.FromAColor(99, OxyColors.Blue), Stroke = OxyColors.Black, StrokeThickness = 2 });
            return model;
        }

        [Example("EllipseAnnotation")]
        public static PlotModel EllipseAnnotation()
        {
            var model = new PlotModel("EllipseAnnotation");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Annotations.Add(new EllipseAnnotation { X = 20, Y = 60, Width = 20, Height = 15, Text = "EllipseAnnotation", TextRotation = 10, Fill = OxyColor.FromAColor(99, OxyColors.Green), Stroke = OxyColors.Black, StrokeThickness = 2 });

            model.Annotations.Add(new EllipseAnnotation { X = 20, Y = 20, Width = 20, Height = 20, Fill = OxyColor.FromAColor(99, OxyColors.Green), Stroke = OxyColors.Black, StrokeThickness = 2 });
            model.Annotations.Add(new EllipseAnnotation { X = 30, Y = 20, Width = 20, Height = 20, Fill = OxyColor.FromAColor(99, OxyColors.Red), Stroke = OxyColors.Black, StrokeThickness = 2 });
            model.Annotations.Add(new EllipseAnnotation { X = 25, Y = 30, Width = 20, Height = 20, Fill = OxyColor.FromAColor(99, OxyColors.Blue), Stroke = OxyColors.Black, StrokeThickness = 2 });
            return model;
        }

        [Example("RectangleAnnotations - vertical limit")]
        public static PlotModel RectangleAnnotationVerticalLimit()
        {
            var model = new PlotModel("RectangleAnnotations - vertical limit");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Annotations.Add(new RectangleAnnotation { MaximumY = 89.5, Text = "Valid area", Fill = OxyColor.FromAColor(99, OxyColors.Black) });
            return model;
        }

        [Example("RectangleAnnotation - horizontal bands")]
        public static PlotModel RectangleAnnotationHorizontals()
        {
            var model = new PlotModel("RectangleAnnotation - horizontal bands");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 10 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 87, Maximum = 97, MajorStep = 1, MinorStep = 1 });
            model.Annotations.Add(new RectangleAnnotation { MinimumY = 89.5, MaximumY = 90.8, Text = "Invalid", Fill = OxyColor.FromAColor(99, OxyColors.Red) });
            model.Annotations.Add(new RectangleAnnotation { MinimumY = 90.8, MaximumY = 92.1, Fill = OxyColor.FromAColor(99, OxyColors.Orange) });
            model.Annotations.Add(new RectangleAnnotation { MinimumY = 92.1, MaximumY = 94.6, Fill = OxyColor.FromAColor(99, OxyColors.Yellow) });
            model.Annotations.Add(new RectangleAnnotation { MinimumY = 94.6, MaximumY = 96, Text = "Ok", Fill = OxyColor.FromAColor(99, OxyColors.Green) });
            LineSeries series1;
            model.Series.Add(series1 = new LineSeries { Color = OxyColors.Black, StrokeThickness = 6.0, LineJoin = OxyPenLineJoin.Round });
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
            var model = new PlotModel("RectangleAnnotation - vertical bands");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 10 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 87, Maximum = 97, MajorStep = 1, MinorStep = 1 });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 2.5, MaximumX = 2.8, TextRotation = 90, Text = "Red", Fill = OxyColor.FromAColor(99, OxyColors.Red) });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 2.8, MaximumX = 6.1, TextRotation = 90, Text = "Orange", Fill = OxyColor.FromAColor(99, OxyColors.Orange) });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 6.1, MaximumX = 7.6, TextRotation = 90, Text = "Yellow", Fill = OxyColor.FromAColor(99, OxyColors.Yellow) });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 7.6, MaximumX = 9.7, TextRotation = 270, Text = "Green", Fill = OxyColor.FromAColor(99, OxyColors.Green) });
            LineSeries series1;
            model.Series.Add(series1 = new LineSeries { Color = OxyColors.Black, StrokeThickness = 6.0, LineJoin = OxyPenLineJoin.Round });
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

        [Example("LineAnnotation - ClipByAxis property")]
        public static PlotModel LinearAxesMultipleAxes()
        {
            var model = new PlotModel("ClipByAxis property", "This property specifies if the annotation should be clipped by the current axes or by the full plot area.");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80, StartPosition = 0, EndPosition = 0.45, TextColor = OxyColors.Red });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10, StartPosition = 0, EndPosition = 0.45, TextColor = OxyColors.Green });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80, StartPosition = 0.55, EndPosition = 1, TextColor = OxyColors.Blue });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10, StartPosition = 0.55, EndPosition = 1, TextColor = OxyColors.Orange });

            model.Annotations.Add(new LineAnnotation { ClipByYAxis = true, Type = LineAnnotationType.Vertical, X = 0, Color = OxyColors.Green, Text = "Vertical, ClipByAxis = true" });
            model.Annotations.Add(new LineAnnotation { ClipByYAxis = false, Type = LineAnnotationType.Vertical, X = 20, Color = OxyColors.Green, Text = "Vertical, ClipByAxis = false" });
            model.Annotations.Add(new LineAnnotation { ClipByXAxis = true, Type = LineAnnotationType.Horizontal, Y = 2, Color = OxyColors.Gold, Text = "Horizontal, ClipByAxis = true" });
            model.Annotations.Add(new LineAnnotation { ClipByXAxis = false, Type = LineAnnotationType.Horizontal, Y = 8, Color = OxyColors.Gold, Text = "Horizontal, ClipByAxis = false" });
            return model;
        }

        [Example("ArrowAnnotation")]
        public static PlotModel ArrowAnnotation()
        {
            var model = new PlotModel("ArrowAnnotations");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -40, Maximum = 60 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            model.Annotations.Add(new ArrowAnnotation { StartPoint = new DataPoint(8, 4), EndPoint = new DataPoint(0, 0), Color = OxyColors.Green, Text = "StartPoint and EndPoint" });
            model.Annotations.Add(new ArrowAnnotation { ArrowDirection = new ScreenVector(30, 70), EndPoint = new DataPoint(40, -3), Color = OxyColors.Blue, Text = "ArrowDirection and EndPoint" });
            model.Annotations.Add(new ArrowAnnotation { ArrowDirection = new ScreenVector(30, -70), EndPoint = new DataPoint(10, -3), HeadLength = 14, HeadWidth = 6, Veeness = 4, Color = OxyColors.Red, Text = "HeadLength = 20, HeadWidth = 10, Veeness = 4" });
            return model;
        }

        [Example("PolylineAnnotation")]
        public static PlotModel PolylineAnnotations()
        {
            var model = new PlotModel("PolylineAnnotation");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 30 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 30 });
            var a1 = new PolylineAnnotation { Text = "Polyline" };
            a1.Points.AddRange(new[] { new DataPoint(0, 10), new DataPoint(5, 5), new DataPoint(20, 1), new DataPoint(30, 20) });
            var a2 = new PolylineAnnotation { Smooth = true, Text = "Smooth Polyline" };
            a2.Points.AddRange(new[] { new DataPoint(0, 15), new DataPoint(3, 23), new DataPoint(9, 30), new DataPoint(20, 12), new DataPoint(30, 10) });
            model.Annotations.Add(a1);
            model.Annotations.Add(a2);
            return model;
        }

        [Example("PolygonAnnotation")]
        public static PlotModel PolygonAnnotation()
        {
            var model = new PlotModel("PolygonAnnotation");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            var a1 = new PolygonAnnotation { Text = "Polygon 1" };
            a1.Points.AddRange(new[] { new DataPoint(4, -2), new DataPoint(8, -4), new DataPoint(17, 7), new DataPoint(5, 8), new DataPoint(2, 5) });
            model.Annotations.Add(a1);
            return model;
        }

        [Example("AnnotationLayer property")]
        public static PlotModel AnnotationLayerProperty()
        {
            var model = new PlotModel("Annotation Layers");
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

        [Example("TextAnnotation")]
        public static PlotModel TextAnnotations()
        {
            var model = new PlotModel("TextAnnotation");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -15, Maximum = 25 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -5, Maximum = 18 });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(-6, 0), Text = "Text annotation 1" });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(-7, 10), Rotation = 80, Text = "Text annotation 2" });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(2, 2), Rotation = 20, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Text = "Right/Top" });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(2, 4), Rotation = 20, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Middle, Text = "Right/Middle" });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(2, 6), Rotation = 20, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Right/Bottom" });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(10, 2), Rotation = 20, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Text = "Center/Top" });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(10, 4), Rotation = 20, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Middle, Text = "Center/Middle" });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(10, 6), Rotation = 20, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom, Text = "Center/Bottom" });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(18, 2), Rotation = 20, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, Text = "Left/Top" });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(18, 4), Rotation = 20, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Middle, Text = "Left/Middle" });
            model.Annotations.Add(new TextAnnotation { Position = new DataPoint(18, 6), Rotation = 20, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, Text = "Left/Bottom" });

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
                    addPoint(ta.Position.X, ta.Position.Y);
                }
            }

            return model;
        }

        [Example("LineAnnotation on reversed axes")]
        public static PlotModel ReversedAxes()
        {
            var model = new PlotModel("LineAnnotation on reversed axes");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80, StartPosition = 1, EndPosition = 0 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10, StartPosition = 1, EndPosition = 0 });
            model.Annotations.Add(new LineAnnotation { Slope = 0.1, Intercept = 1, Text = "First", TextHorizontalAlignment = HorizontalAlignment.Left });
            model.Annotations.Add(new LineAnnotation { Slope = 0.3, Intercept = 2, MaximumX = 40, Color = OxyColors.Red, Text = "Second", TextHorizontalAlignment = HorizontalAlignment.Left, TextVerticalAlignment = VerticalAlignment.Bottom });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Vertical, X = 4, MaximumY = 10, Color = OxyColors.Green, Text = "Vertical", TextHorizontalAlignment = HorizontalAlignment.Right });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Horizontal, Y = 2, MaximumX = 4, Color = OxyColors.Gold, Text = "Horizontal", TextHorizontalAlignment = HorizontalAlignment.Left });
            return model;
        }

        [Example("ImageAnnotation")]
        public static PlotModel ImageAnnotation()
        {
            var model = new PlotModel("ImageAnnotation") { PlotMargins = new OxyThickness(60, 4, 4, 60) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            OxyImage image;
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("ExampleLibrary.Resources.OxyPlot.png"))
            {
                image = new OxyImage(stream);
            }

            // Centered in plot area, filling width
            model.Annotations.Add(new ImageAnnotation
            {
                ImageSource = image,
                Opacity = 0.2,
                Interpolate = false,
                X = new PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea),
                Y = new PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea),
                Width = new PlotLength(1, PlotLengthUnit.RelativeToPlotArea),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle
            });

            // Relative to plot area, inside top/right corner, 120pt wide
            model.Annotations.Add(new ImageAnnotation
            {
                ImageSource = image,
                X = new PlotLength(1, PlotLengthUnit.RelativeToPlotArea),
                Y = new PlotLength(0, PlotLengthUnit.RelativeToPlotArea),
                Width = new PlotLength(120, PlotLengthUnit.ScreenUnits),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            });

            // Relative to plot area, above top/left corner, 20pt high
            model.Annotations.Add(new ImageAnnotation
            {
                ImageSource = image,
                X = new PlotLength(0, PlotLengthUnit.RelativeToPlotArea),
                Y = new PlotLength(0, PlotLengthUnit.RelativeToPlotArea),
                OffsetY = new PlotLength(-5, PlotLengthUnit.ScreenUnits),
                Height = new PlotLength(20, PlotLengthUnit.ScreenUnits),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            });

            // At the point (50,50), 200pt wide
            model.Annotations.Add(new ImageAnnotation
            {
                ImageSource = image,
                X = new PlotLength(50, PlotLengthUnit.Data),
                Y = new PlotLength(50, PlotLengthUnit.Data),
                Width = new PlotLength(200, PlotLengthUnit.ScreenUnits),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            });

            // At the point (50,20), 50 x units wide
            model.Annotations.Add(new ImageAnnotation
            {
                ImageSource = image,
                X = new PlotLength(50, PlotLengthUnit.Data),
                Y = new PlotLength(20, PlotLengthUnit.Data),
                Width = new PlotLength(50, PlotLengthUnit.Data),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            });

            // Relative to the viewport, centered at the bottom, with offset (could also use bottom vertical alignment)
            model.Annotations.Add(new ImageAnnotation
            {
                ImageSource = image,
                X = new PlotLength(0.5, PlotLengthUnit.RelativeToViewport),
                Y = new PlotLength(1, PlotLengthUnit.RelativeToViewport),
                OffsetY = new PlotLength(-35, PlotLengthUnit.ScreenUnits),
                Height = new PlotLength(30, PlotLengthUnit.ScreenUnits),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            });

            // Changing opacity
            for (int y = 0; y < 10; y++)
            {
                model.Annotations.Add(
                    new ImageAnnotation
                        {
                            ImageSource = image,
                            Opacity = (y + 1) / 10.0,
                            X = new PlotLength(10, PlotLengthUnit.Data),
                            Y = new PlotLength(y * 2, PlotLengthUnit.Data),
                            Width = new PlotLength(100, PlotLengthUnit.ScreenUnits),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Bottom
                        });
            }

            return model;
        }

        [Example("ImageAnnotation - gradient backgrounds")]
        public static PlotModel ImageAnnotationAsBackgroundGradient()
        {
            // http://en.wikipedia.org/wiki/Chartjunk
            var model = new PlotModel("Using ImageAnnotations to draw a gradient backgrounds", "But do you really want this? This is called 'chartjunk'!") { PlotMargins = new OxyThickness(60, 4, 4, 60) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            // create a gradient image of height n
            int n = 256;
            var imageData1 = new OxyColor[1, n];
            for (int i = 0; i < n; i++)
            {
                imageData1[0, i] = OxyColor.Interpolate(OxyColors.Blue, OxyColors.Red, i / (n - 1.0));
            }

            var image1 = OxyImage.Create(imageData1, ImageFormat.Png); // png is required for silverlight

            // or create a gradient image of height 2 (requires bitmap interpolation to be supported)
            var imageData2 = new OxyColor[1, 2];
            imageData2[0, 0] = OxyColors.Yellow; // top color
            imageData2[0, 1] = OxyColors.Gray; // bottom color

            var image2 = OxyImage.Create(imageData2, ImageFormat.Png); // png is required for silverlight

            // gradient filling the viewport
            model.Annotations.Add(new ImageAnnotation
            {
                ImageSource = image2,
                Interpolate = true,
                Layer = AnnotationLayer.BelowAxes,
                X = new PlotLength(0, PlotLengthUnit.RelativeToViewport),
                Y = new PlotLength(0, PlotLengthUnit.RelativeToViewport),
                Width = new PlotLength(1, PlotLengthUnit.RelativeToViewport),
                Height = new PlotLength(1, PlotLengthUnit.RelativeToViewport),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            });

            // gradient filling the plot area
            model.Annotations.Add(new ImageAnnotation
            {
                ImageSource = image1,
                Interpolate = true,
                Layer = AnnotationLayer.BelowAxes,
                X = new PlotLength(0, PlotLengthUnit.RelativeToPlotArea),
                Y = new PlotLength(0, PlotLengthUnit.RelativeToPlotArea),
                Width = new PlotLength(1, PlotLengthUnit.RelativeToPlotArea),
                Height = new PlotLength(1, PlotLengthUnit.RelativeToPlotArea),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            });

            // verify that a series is rendered above the gradients
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 7, 0.01));

            return model;
        }

        [Example("ImageAnnotation - normal axes")]
        public static PlotModel ImageAnnotation_NormalAxes()
        {
            var model = new PlotModel("ImageAnnotation - normal axes");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            // create an image
            var pixels = new OxyColor[2, 2];
            pixels[0, 0] = OxyColors.Blue;
            pixels[1, 0] = OxyColors.Yellow;
            pixels[0, 1] = OxyColors.Green;
            pixels[1, 1] = OxyColors.Red;

            var image = OxyImage.Create(pixels, ImageFormat.Png);

            model.Annotations.Add(
                new ImageAnnotation
                    {
                        ImageSource = image,
                        Interpolate = false,
                        X = new PlotLength(0, PlotLengthUnit.Data),
                        Y = new PlotLength(0, PlotLengthUnit.Data),
                        Width = new PlotLength(80, PlotLengthUnit.Data),
                        Height = new PlotLength(50, PlotLengthUnit.Data),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Bottom
                    });
            return model;
        }

        [Example("ImageAnnotation - reverse horizontal axis")]
        public static PlotModel ImageAnnotation_ReverseHorizontalAxis()
        {
            var model = new PlotModel("ImageAnnotation - reverse horizontal axis");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, StartPosition = 1, EndPosition = 0 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            // create an image
            var pixels = new OxyColor[2, 2];
            pixels[0, 0] = OxyColors.Blue;
            pixels[1, 0] = OxyColors.Yellow;
            pixels[0, 1] = OxyColors.Green;
            pixels[1, 1] = OxyColors.Red;

            var image = OxyImage.Create(pixels, ImageFormat.Png);

            model.Annotations.Add(
                new ImageAnnotation
                {
                    ImageSource = image,
                    Interpolate = false,
                    X = new PlotLength(100, PlotLengthUnit.Data),
                    Y = new PlotLength(0, PlotLengthUnit.Data),
                    Width = new PlotLength(80, PlotLengthUnit.Data),
                    Height = new PlotLength(50, PlotLengthUnit.Data),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom
                });
            return model;
        }

        [Example("ImageAnnotation - reverse vertical axis")]
        public static PlotModel ImageAnnotation_ReverseVerticalAxis()
        {
            var model = new PlotModel("ImageAnnotation - reverse vertical axis");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 });

            // create an image
            var pixels = new OxyColor[2, 2];
            pixels[0, 0] = OxyColors.Blue;
            pixels[1, 0] = OxyColors.Yellow;
            pixels[0, 1] = OxyColors.Green;
            pixels[1, 1] = OxyColors.Red;

            var image = OxyImage.Create(pixels, ImageFormat.Png);

            model.Annotations.Add(
                new ImageAnnotation
                {
                    ImageSource = image,
                    Interpolate = false,
                    X = new PlotLength(0, PlotLengthUnit.Data),
                    Y = new PlotLength(100, PlotLengthUnit.Data),
                    Width = new PlotLength(80, PlotLengthUnit.Data),
                    Height = new PlotLength(50, PlotLengthUnit.Data),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom
                });
            return model;
        }

        [Example("TileMapAnnotation (openstreetmap.org)")]
        public static PlotModel TileMapAnnotation2()
        {
            var model = new PlotModel("TileMapAnnotation");
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 10.4, Maximum = 10.6, Title = "Longitude" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 59.88, Maximum = 59.96, Title = "Latitude" });

            // Add the tile map annotation
            model.Annotations.Add(
                new TileMapAnnotation
                {
                    Url = "http://tile.openstreetmap.org/{Z}/{X}/{Y}.png",
                    CopyrightNotice = "OpenStreet map."
                });

            return model;
        }

        [Example("TileMapAnnotation (statkart.no)")]
        public static PlotModel TileMapAnnotation()
        {
            var model = new PlotModel("TileMapAnnotation");

            // TODO: scale ratio between the two axes should be fixed (or depending on latitude...)
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 10.4, Maximum = 10.6, Title = "Longitude" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 59.88, Maximum = 59.96, Title = "Latitude" });

            // Add the tile map annotation
            model.Annotations.Add(
                new TileMapAnnotation
                {
                    Url = "http://opencache.statkart.no/gatekeeper/gk/gk.open_gmaps?layers=toporaster2&zoom={Z}&x={X}&y={Y}",
                    CopyrightNotice = "Kartgrunnlag: Statens kartverk, Geovekst og kommuner.",
                    MinZoomLevel = 5,
                    MaxZoomLevel = 19
                });

            model.Annotations.Add(new ArrowAnnotation
            {
                EndPoint = new DataPoint(10.563, 59.888),
                ArrowDirection = new ScreenVector(-40, -60),
                StrokeThickness = 3,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                TextColor = OxyColor.FromAColor(160, OxyColors.Magenta),
                Color = OxyColor.FromAColor(100, OxyColors.Magenta)
            });

            return model;
        }
    }
}