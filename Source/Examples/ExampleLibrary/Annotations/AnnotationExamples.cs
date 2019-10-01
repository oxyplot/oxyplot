// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System;
    using System.Threading.Tasks;

    [Examples("Annotations"), Tags("Annotations")]
    public static class AnnotationExamples
    {
        [Example("Tool tips")]
        public static PlotModel ToolTips()
        {
            var model = new PlotModel { Title = "Tool tips" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Annotations.Add(new LineAnnotation { Slope = 0.1, Intercept = 1, Text = "LineAnnotation", ToolTip = "This is a tool tip for the LineAnnotation" });
            model.Annotations.Add(new RectangleAnnotation { MinimumX = 20, MaximumX = 70, MinimumY = 10, MaximumY = 40, TextRotation = 10, Text = "RectangleAnnotation", ToolTip = "This is a tooltip for the RectangleAnnotation", Fill = OxyColor.FromAColor(99, OxyColors.Blue), Stroke = OxyColors.Black, StrokeThickness = 2 });
            model.Annotations.Add(new EllipseAnnotation { X = 20, Y = 60, Width = 20, Height = 15, Text = "EllipseAnnotation", ToolTip = "This is a tool tip for the EllipseAnnotation", TextRotation = 10, Fill = OxyColor.FromAColor(99, OxyColors.Green), Stroke = OxyColors.Black, StrokeThickness = 2 });
            model.Annotations.Add(new PointAnnotation { X = 50, Y = 50, Text = "P1", ToolTip = "This is a tool tip for the PointAnnotation" });
            model.Annotations.Add(new ArrowAnnotation { StartPoint = new DataPoint(8, 4), EndPoint = new DataPoint(0, 0), Color = OxyColors.Green, Text = "ArrowAnnotation", ToolTip = "This is a tool tip for the ArrowAnnotation" });
            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(60, 60), Text = "TextAnnotation", ToolTip = "This is a tool tip for the TextAnnotation" });
            return model;
        }

        /// <summary>
        /// The category in which this example is put is temporary.
        /// </summary>
        /// <returns></returns>
        [Example("Crazy Refresh")]
        public static PlotModel CrazyRefresh()
        {
            var plot = new PlotModel();

            plot.IsLegendVisible = true;

            //var rs = new RectangleSeries();
            //rs.Items.Add(new RectangleItem(0.5, 9.5, 0.6, 0.8, 1));
            //plot.Series.Add(rs);

            var fs = new FunctionSeries(x => Math.Sin(x), 0, 10, 0.001);
            plot.Series.Add(fs);
            fs = new FunctionSeries(x => Math.Cos(x), 0, 10, 0.001);
            plot.Series.Add(fs);

            var ra = new RectangleAnnotation()
            {
                MinimumX = 0.5,
                MinimumY = -0.8,
                MaximumX = 9.5,
                MaximumY = -0.6,
                ToolTip = "Test",
                Text = "Hover cant work because this element gets replaced.\n" +
                "Clicking me doesn't count as clicking the plot,\n" +
                "But sometimes they go to MouseGrid.",
            };
            plot.Annotations.Add(ra);

            var rnd = new Random();
            Crazy(plot, rnd, fs);

            plot.MouseDown += (s, e) =>
            {
                plot.Title = "MouseDown " + rnd.Next(100000);
                e.Handled = true;
            };
            plot.MouseUp += (s, e) =>
            {
                plot.Title = "MouseUp " + rnd.Next(100000);
                e.Handled = true;
            };
            plot.MouseMove += (s, e) =>
            {
                plot.Title = "MouseMove " + rnd.Next(100000);
                e.Handled = true;
            };


            return plot;
        }

        private static async Task Crazy(PlotModel plot, Random rnd, FunctionSeries fs)
        {
            await Task.Delay(20);
            fs.Color = OxyColor.FromRgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));

            plot.InvalidatePlot(true);
            await Crazy(plot, rnd, fs);
        }

    }
}
