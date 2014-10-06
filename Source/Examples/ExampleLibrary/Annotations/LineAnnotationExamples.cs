// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;

    [Examples("LineAnnotation"), Tags("Annotations")]
    public static class LineAnnotationExamples
    {
        [Example("LineAnnotation on linear axes")]
        public static PlotModel LineAnnotationOnLinearAxes()
        {
            var model = new PlotModel { Title = "LineAnnotation on linear axes" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            model.Annotations.Add(new LineAnnotation { Slope = 0.1, Intercept = 1, Text = "First" });
            model.Annotations.Add(
                new LineAnnotation { Slope = 0.3, Intercept = 2, MaximumX = 40, Color = OxyColors.Red, Text = "Second" });
            model.Annotations.Add(
                new LineAnnotation
                    {
                        Type = LineAnnotationType.Vertical,
                        X = 4,
                        MaximumY = 10,
                        Color = OxyColors.Green,
                        Text = "Vertical"
                    });
            model.Annotations.Add(
                new LineAnnotation
                    {
                        Type = LineAnnotationType.Horizontal,
                        Y = 2,
                        MaximumX = 4,
                        Color = OxyColors.Gold,
                        Text = "Horizontal"
                    });
            return model;
        }

        [Example("LineAnnotation on logarithmic axes")]
        public static PlotModel LineAnnotationOnLogarithmicAxes()
        {
            var model = new PlotModel { Title = "LineAnnotation on logarithmic axes" };
            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Bottom, Minimum = 1, Maximum = 80 });
            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left, Minimum = 1, Maximum = 10 });
            model.Annotations.Add(new LineAnnotation { Slope = 0.1, Intercept = 1, Text = "First", TextMargin = 40 });
            model.Annotations.Add(
                new LineAnnotation { Slope = 0.3, Intercept = 2, MaximumX = 40, Color = OxyColors.Red, Text = "Second" });
            model.Annotations.Add(
                new LineAnnotation
                    {
                        Type = LineAnnotationType.Vertical,
                        X = 4,
                        MaximumY = 10,
                        Color = OxyColors.Green,
                        Text = "Vertical"
                    });
            model.Annotations.Add(
                new LineAnnotation
                    {
                        Type = LineAnnotationType.Horizontal,
                        Y = 2,
                        MaximumX = 4,
                        Color = OxyColors.Gold,
                        Text = "Horizontal"
                    });
            return model;
        }

        [Example("LineAnnotation with text orientation specified")]
        public static PlotModel LineAnnotationOnLinearAxesWithTextOrientation()
        {
            var model = new PlotModel { Title = "LineAnnotations", Subtitle = "with TextOrientation specified" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            model.Annotations.Add(
                new LineAnnotation
                    {
                        Slope = 0.1,
                        Intercept = 1,
                        Text = "Horizontal",
                        TextOrientation = AnnotationTextOrientation.Horizontal,
                        TextVerticalAlignment = VerticalAlignment.Bottom
                    });
            model.Annotations.Add(
                new LineAnnotation
                    {
                        Slope = 0.3,
                        Intercept = 2,
                        MaximumX = 40,
                        Color = OxyColors.Red,
                        Text = "Vertical",
                        TextOrientation = AnnotationTextOrientation.Vertical
                    });
            model.Annotations.Add(
                new LineAnnotation
                    {
                        Type = LineAnnotationType.Vertical,
                        X = 4,
                        MaximumY = 10,
                        Color = OxyColors.Green,
                        Text = "Horizontal (x=4)",
                        TextPadding = 8,
                        TextOrientation = AnnotationTextOrientation.Horizontal
                    });
            model.Annotations.Add(
                new LineAnnotation
                    {
                        Type = LineAnnotationType.Vertical,
                        X = 45,
                        MaximumY = 10,
                        Color = OxyColors.Green,
                        Text = "Horizontal (x=45)",
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        TextPadding = 8,
                        TextOrientation = AnnotationTextOrientation.Horizontal
                    });
            model.Annotations.Add(
                new LineAnnotation
                    {
                        Type = LineAnnotationType.Horizontal,
                        Y = 2,
                        MaximumX = 4,
                        Color = OxyColors.Gold,
                        Text = "Horizontal",
                        TextLinePosition = 0.5,
                        TextOrientation = AnnotationTextOrientation.Horizontal
                    });
            return model;
        }

        [Example("LineAnnotation - ClipByAxis property")]
        public static PlotModel LinearAxesMultipleAxes()
        {
            var model = new PlotModel { Title = "ClipByAxis property", Subtitle = "This property specifies if the annotation should be clipped by the current axes or by the full plot area." };
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

        [Example("LineAnnotation on reversed axes")]
        public static PlotModel ReversedAxes()
        {
            var model = new PlotModel { Title = "LineAnnotation on reversed axes" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80, StartPosition = 1, EndPosition = 0 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10, StartPosition = 1, EndPosition = 0 });
            model.Annotations.Add(new LineAnnotation { Slope = 0.1, Intercept = 1, Text = "First", TextHorizontalAlignment = HorizontalAlignment.Left });
            model.Annotations.Add(new LineAnnotation { Slope = 0.3, Intercept = 2, MaximumX = 40, Color = OxyColors.Red, Text = "Second", TextHorizontalAlignment = HorizontalAlignment.Left, TextVerticalAlignment = VerticalAlignment.Bottom });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Vertical, X = 4, MaximumY = 10, Color = OxyColors.Green, Text = "Vertical", TextHorizontalAlignment = HorizontalAlignment.Right });
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Horizontal, Y = 2, MaximumX = 4, Color = OxyColors.Gold, Text = "Horizontal", TextHorizontalAlignment = HorizontalAlignment.Left });
            return model;
        }
    }
}