// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotControllerExamples.cs" company="OxyPlot">
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
    using OxyPlot.Series;

    [Examples("PlotController examples")]
    public static class PlotControllerExamples
    {
        [Example("Basic controller example")]
        public static Example BasicExample()
        {
            var model = new PlotModel { Title = "Basic Controller example", Subtitle = "Panning with left mouse button" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var controller = new PlotController();
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            return new Example(model, controller);
        }

        [Example("Show tracker without clicking")]
        public static Example HoverTracking()
        {
            var model = new PlotModel { Title = "Show tracker without clicking" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Series.Add(new FunctionSeries(t => (Math.Cos(t) * 5) + Math.Cos(t * 50), t => (Math.Sin(t) * 5) + Math.Sin(t * 50), 0, Math.PI * 2, 20000));

            // create a new plot controller with default bindings
            var controller = new PlotController();

            // add a tracker command to the mouse enter event
            controller.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);

            return new Example(model, controller);
        }

        [Example("Mouse handling example")]
        public static Example MouseHandlingExample()
        {
            var model = new PlotModel { Title = "Mouse handling example" };
            var series = new ScatterSeries();
            model.Series.Add(series);

            // Create a command that adds points to the scatter series
            var command = new DelegatePlotCommand<OxyMouseDownEventArgs>(
                (v, c, a) =>
                {
                    a.Handled = true;
                    var point = series.InverseTransform(a.Position);
                    series.Points.Add(new ScatterPoint(point.X, point.Y));
                    model.InvalidatePlot(true);
                });

            var controller = new PlotController();
            controller.BindMouseDown(OxyMouseButton.Left, command);

            return new Example(model, controller);
        }

        [Example("Clicking on an annotation")]
        public static Example ClickingOnAnAnnotation()
        {
            var plotModel = new PlotModel { Title = "Clicking on an annotation", Subtitle = "Click on the rectangles" };

            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var annotation1 = new RectangleAnnotation { Fill = OxyColors.Green, Text = "RectangleAnnotation 1", MinimumX = 25, MaximumX = 75, MinimumY = 20, MaximumY = 40 };
            plotModel.Annotations.Add(annotation1);

            var annotation2 = new RectangleAnnotation { Fill = OxyColors.SkyBlue, Text = "RectangleAnnotation 2", MinimumX = 25, MaximumX = 75, MinimumY = 60, MaximumY = 80 };
            plotModel.Annotations.Add(annotation2);

            EventHandler<OxyMouseDownEventArgs> handleMouseClick = (s, e) =>
            {
                plotModel.Subtitle = "You clicked " + ((RectangleAnnotation)s).Text;
                plotModel.InvalidatePlot(false);
            };

            annotation1.MouseDown += handleMouseClick;
            annotation2.MouseDown += handleMouseClick;

            var controller = new PlotController();
            var handleClick = new DelegatePlotCommand<OxyMouseDownEventArgs>(
                (v, c, e) =>
                {
                    var args = new HitTestArguments(e.Position, 10);
                    var firstHit = v.ActualModel.HitTest(args).FirstOrDefault(x => x.Element is RectangleAnnotation);
                    if (firstHit != null)
                    {
                        e.Handled = true;
                        plotModel.Subtitle = "You clicked " + ((RectangleAnnotation)firstHit.Element).Text;
                        plotModel.InvalidatePlot(false);
                    }
                });
            controller.Bind(new OxyMouseDownGesture(OxyMouseButton.Left), handleClick);

            return new Example(plotModel, controller);
        }
    }
}