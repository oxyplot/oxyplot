// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotationExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using OxyPlot;

namespace ExampleLibrary
{
    [Examples("Annotations")]
    public static class AnnotationExamples
    {
        [Example("Linear axes")]
        public static PlotModel LinearAxes()
        {
            var model = new PlotModel("Annotations on linear axes");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -20, 80));
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -10, 10));
            model.Annotations.Add(new LineAnnotation() { Slope = 0.1, Intercept = 1, Text = "First" });
            model.Annotations.Add(new LineAnnotation() { Slope = 0.3, Intercept = 2, MaximumX = 40, Color = OxyColors.Red, Text = "Second" });
            model.Annotations.Add(new LineAnnotation() { Type = LineAnnotationType.Vertical, X = 4, MaximumY = 10, Color = OxyColors.Green, Text = "Vertical" });
            model.Annotations.Add(new LineAnnotation() { Type = LineAnnotationType.Horizontal, Y = 2, MaximumX = 4, Color = OxyColors.Gold, Text = "Horizontal" });
            return model;
        }


        [Example("Logarithmic axes")]
        public static PlotModel LogarithmicAxes()
        {
            var model = new PlotModel("Annotations on logarithmic axes");
            model.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, 1, 80));
            model.Axes.Add(new LogarithmicAxis(AxisPosition.Left, 1, 10));
            model.Annotations.Add(new LineAnnotation() { Slope = 0.1, Intercept = 1, Text = "First" });
            model.Annotations.Add(new LineAnnotation() { Slope = 0.3, Intercept = 2, MaximumX = 40, Color = OxyColors.Red, Text = "Second" });
            model.Annotations.Add(new LineAnnotation() { Type = LineAnnotationType.Vertical, X = 4, MaximumY = 10, Color = OxyColors.Green, Text = "Vertical" });
            model.Annotations.Add(new LineAnnotation() { Type = LineAnnotationType.Horizontal, Y = 2, MaximumX = 4, Color = OxyColors.Gold, Text = "Horizontal" });
            return model;
        }

        [Example("No clipping")]
        public static PlotModel LinearAxesMultipleAxes()
        {
            var model = new PlotModel("ClipByXAxis = false, ClipByYAxis = false");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -20, 80) { StartPosition = 0, EndPosition = 0.5 });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -10, 10) { StartPosition = 0, EndPosition = 0.5 });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -20, 80) { StartPosition = 0.5, EndPosition = 1 });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -10, 10) { StartPosition = 0.5, EndPosition = 1 });

            //  model.Annotations.Add(new LineAnnotation() { ClipByXAxis = false, ClipByYAxis = false, Slope = 0.1, Intercept = 1, Text = "First" });
            //  model.Annotations.Add(new LineAnnotation() { ClipByXAxis = false, ClipByYAxis = false, Slope = 0.3, Intercept = 2, MaximumX = 40, Color = OxyColors.Red, Text = "Second" });
            model.Annotations.Add(new LineAnnotation { ClipByYAxis = false, Type = LineAnnotationType.Vertical, X = 0, Color = OxyColors.Green, Text = "Vertical" });
            model.Annotations.Add(new LineAnnotation { ClipByXAxis = false, Type = LineAnnotationType.Horizontal, Y = 2, Color = OxyColors.Gold, Text = "Horizontal" });
            return model;
        }
    }
}