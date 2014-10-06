// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;

    [Examples("PointAnnotation"), Tags("Annotations")]
    public static class PointAnnotationExamples
    {
        [Example("PointAnnotation")]
        public static PlotModel PointAnnotation()
        {
            var model = new PlotModel { Title = "PointAnnotation" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            model.Annotations.Add(new PointAnnotation { X = 50, Y = 50, Text = "P1" });
            return model;
        }

        [Example("PointAnnotation - shapes")]
        public static PlotModel PointAnnotationShapes()
        {
            var model = new PlotModel { Title = "PointAnnotation - shapes" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Maximum = 120 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            // filled
            model.Annotations.Add(
                new PointAnnotation
                    {
                        X = 20,
                        Y = 60,
                        Text = "Circle",
                        Shape = MarkerType.Circle,
                        Fill = OxyColors.LightGray,
                        Stroke = OxyColors.DarkGray,
                        StrokeThickness = 1
                    });
            model.Annotations.Add(
                new PointAnnotation
                    {
                        X = 40,
                        Y = 60,
                        Text = "Square",
                        Shape = MarkerType.Square,
                        Fill = OxyColors.LightBlue,
                        Stroke = OxyColors.DarkBlue,
                        StrokeThickness = 1
                    });
            model.Annotations.Add(
                new PointAnnotation
                    {
                        X = 60,
                        Y = 60,
                        Text = "Triangle",
                        Shape = MarkerType.Triangle,
                        Fill = OxyColors.IndianRed,
                        Stroke = OxyColors.Black,
                        StrokeThickness = 1
                    });
            model.Annotations.Add(
                new PointAnnotation
                    {
                        X = 80,
                        Y = 60,
                        Text = "Diamond",
                        Shape = MarkerType.Diamond,
                        Fill = OxyColors.ForestGreen,
                        Stroke = OxyColors.Black,
                        StrokeThickness = 1
                    });
            model.Annotations.Add(
                new PointAnnotation
                    {
                        X = 100,
                        Y = 60,
                        Text = "Custom",
                        Shape = MarkerType.Custom,
                        CustomOutline =
                            new[]
                                {
                                    new ScreenPoint(-1, -1), new ScreenPoint(1, 1), new ScreenPoint(-1, 1),
                                    new ScreenPoint(1, -1)
                                },
                        Stroke = OxyColors.Black,
                        Fill = OxyColors.CadetBlue,
                        StrokeThickness = 1
                    });

            // not filled
            model.Annotations.Add(
                new PointAnnotation
                    {
                        X = 20,
                        Y = 40,
                        Text = "Cross",
                        Shape = MarkerType.Cross,
                        Stroke = OxyColors.IndianRed,
                        StrokeThickness = 1
                    });
            model.Annotations.Add(
                new PointAnnotation
                    {
                        X = 40,
                        Y = 40,
                        Text = "Plus",
                        Shape = MarkerType.Plus,
                        Stroke = OxyColors.Navy,
                        StrokeThickness = 1
                    });
            model.Annotations.Add(
                new PointAnnotation
                    {
                        X = 60,
                        Y = 40,
                        Text = "Star",
                        Shape = MarkerType.Star,
                        Stroke = OxyColors.DarkOliveGreen,
                        StrokeThickness = 1
                    });

            return model;
        }

        [Example("PointAnnotation - text alignments")]
        public static PlotModel PointAnnotationTextAlignment()
        {
            var model = new PlotModel { Title = "PointAnnotation - text alignments" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -50, Maximum = 50 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -50, Maximum = 50 });

            for (var ha = -1; ha <= 1; ha++)
            {
                var h = (HorizontalAlignment)ha;
                for (var va = -1; va <= 1; va++)
                {
                    var v = (VerticalAlignment)va;
                    model.Annotations.Add(
                        new PointAnnotation
                            {
                                X = ha * 20,
                                Y = va * 20,
                                Size = 10,
                                Text = h + "," + v,
                                TextHorizontalAlignment = h,
                                TextVerticalAlignment = v
                            });
                }
            }

            return model;
        }
    }
}