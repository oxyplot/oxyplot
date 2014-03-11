namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("PlotController examples")]
    public static class PlotControllerExamples
    {
        [Example("Basic controller example")]
        public static Example BasicExample()
        {
            var model = new PlotModel("Basic Controller example", "Panning with left mouse button");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            model.Axes.Add(new LinearAxis(AxisPosition.Left));
            var controller = new PlotController();
            controller.InputCommandBindings.Clear();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            return new Example(model, controller);
        }

        [Example("Show tracker without clicking")]
        public static Example HoverTracking()
        {
            var model = new PlotModel("Show tracker without clicking");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            model.Axes.Add(new LinearAxis(AxisPosition.Left));
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
            var model = new PlotModel("Mouse handling example");
            var series = new ScatterSeries();
            model.Series.Add(series);

            // Create a command that adds points to the scatter series
            var command = new DelegatePlotControllerCommand<OxyMouseEventArgs>(
                (v, c, a) =>
                {
                    var point = series.InverseTransform(a.Position);
                    series.Points.Add(point);
                    model.InvalidatePlot(true);
                });

            var controller = new PlotController();
            controller.BindMouseDown(OxyMouseButton.Left, command);

            return new Example(model, controller);
        }
    }
}