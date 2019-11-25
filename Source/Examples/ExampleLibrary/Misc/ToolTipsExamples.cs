// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolTipsExamples.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
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

    [Examples("Tooltips"), Tags("Tooltips")]
    public static class ToolTipExamples
    {
        [Example("Simple tooltips on annotations")]
        public static PlotModel SimpleToolTipsOnAnnotations()
        {
            var model = new PlotModel {
                Title = "Hover me to see a tooltip",
                Subtitle = "Me too",
                TitleToolTip = "Our tooltip"
            };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            model.Annotations.Add(new LineAnnotation { Slope = 0.1, Intercept = 1, Text = "LineAnnotation", ToolTip = "This is a tooltip for the LineAnnotation" });

            model.Annotations.Add(new RectangleAnnotation { MinimumX = 20, MaximumX = 70, MinimumY = 10, MaximumY = 40, TextRotation = 10, Text = "RectangleAnnotation", ToolTip = "This is a tooltip for the RectangleAnnotation", Fill = OxyColor.FromAColor(99, OxyColors.Blue), Stroke = OxyColors.Black, StrokeThickness = 2 });

            model.Annotations.Add(new EllipseAnnotation { X = 20, Y = 60, Width = 20, Height = 15, Text = "EllipseAnnotation", ToolTip = "This is a tooltip for the EllipseAnnotation", TextRotation = 10, Fill = OxyColor.FromAColor(99, OxyColors.Green), Stroke = OxyColors.Black, StrokeThickness = 2 });
            
            model.Annotations.Add(new PointAnnotation { X = 50, Y = 50, Text = "P1", ToolTip = "This is a tooltip for the PointAnnotation" });

            model.Annotations.Add(new ArrowAnnotation { StartPoint = new DataPoint(8, 4), EndPoint = new DataPoint(0, 0), Color = OxyColors.Green, Text = "ArrowAnnotation", ToolTip = "This is a tooltip for the ArrowAnnotation" });

            model.Annotations.Add(new TextAnnotation { TextPosition = new DataPoint(60, 60), Text = "TextAnnotation", ToolTip = "This is a tooltip for the TextAnnotation" });

            return model;
        }

        [Example("Tooltips with frequent refreshes")]
        public static PlotModel ToolTipsWithFrequentRefreshes()
        {
            var plot = new PlotModel()
            {
                IsLegendVisible = true,
                Title = "Mouse events are shown here"
            };

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
                Text = "Hover the rectangle to see the fixed tooltip.\n" +
                    "Hover the flashing series to see a new random number each time you hover it.",
            };
            plot.Annotations.Add(ra);

            var rnd = new Random();
            _ = WaitThenChangeColorAndToolTip(plot, rnd, fs);

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

        private static async Task WaitThenChangeColorAndToolTip(PlotModel plot, Random rnd, FunctionSeries fs)
        {
            await Task.Delay(20);
            fs.Color = OxyColor.FromRgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));
            fs.ToolTip = $"Tooltip with random number {rnd.Next(100000)}";

            plot.InvalidatePlot(true);
            await WaitThenChangeColorAndToolTip(plot, rnd, fs);
        }

        [Example("Tooltips, axes and layers")]
        public static PlotModel ToolTipsAxesAndLayers()
        {
            var pm = new PlotModel
            {
                Title = "Tooltips, axes and layers",
                Subtitle = "You can try to drag the mouse cursor from over an element\n" +
                    "with a tooltip onto another element with another tooltip,\n" +
                    "without releasing the mouse button"
            };

            var rs = new FunctionSeries(x => Math.Cos(x), 0, 20, 0.1)
            {
                ToolTip = "My Function Series"
            };
            pm.Series.Add(rs);

            pm.Annotations.Add(new RectangleAnnotation()
            {
                ToolTip = "My Rectangle Annotation",
                Fill = OxyColors.Blue,
                MinimumX = 0,
                MinimumY = 0,
                MaximumX = 10,
                MaximumY = 10,
                Layer = AnnotationLayer.BelowAxes,
                Text = "BelowAxes layer"
            });

            pm.Axes.Clear();
            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = -10,
                Maximum = 20,
                ToolTip = "Bottom axis",
                TickStyle = TickStyle.Inside,
                Title = "Hover me ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                TitlePosition = 0,
                ClipTitle = true
            });
            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -10,
                Maximum = 20,
                ToolTip = "Left axis",
                TickStyle = TickStyle.Inside,
                Title = "Hover me ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                TitlePosition = 0,
                ClipTitle = true
            });
            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Top,
                Minimum = -10,
                Maximum = 20,
                ToolTip = "Top axis",
                TickStyle = TickStyle.Inside,
                Title = "Hover me ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                TitlePosition = 1,
                ClipTitle = true
            });
            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Right,
                Minimum = -10,
                Maximum = 20,
                ToolTip = "Right axis",
                TickStyle = TickStyle.Inside,
                Title = "Hover me ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                TitlePosition = 1,
                ClipTitle = true
            });

            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Right,
                Minimum = -10,
                Maximum = 20,
                ToolTip = "Second right axis",
                TickStyle = TickStyle.Inside,
                Title = "Hover me ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                TitlePosition = 1,
                ClipTitle = true,
                PositionTier = 1
            });

            return pm;
        }

        [Example("Tooltip formatters")]
        public static PlotModel ToolTipFormatters()
        {
            var rnd = new Random();

            var pm = new PlotModel
            {
                Title = "Tooltip formatters",
                TitleToolTipFormatter = (PlotModel m) =>
                {
                    return rnd.Next(100000).ToString();
                },
                TitleToolTip = "This tooltip overrides the TitleToolTipFormatter of the PlotModel"
            };

            pm.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom });
            pm.Axes.Add(new LinearAxis() { Position = AxisPosition.Left });

            pm.Annotations.Add(new RectangleAnnotation()
            {
                Fill = OxyColors.Blue,
                MinimumX = 0,
                MinimumY = 0,
                MaximumX = 50,
                MaximumY = 50,
                Text = "Hover Me",
                ToolTipFormatter = (HitTestResult r) =>
                {
                    return "Random Number: " + rnd.Next(100000);
                }
            });

            return pm;
        }
    }
}
