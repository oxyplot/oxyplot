// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotControllerExamples.cs" company="OxyPlot">
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
            var model = new PlotModel { Title = "Basic Controller example", Subtitle = "Panning with left mouse button" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var controller = new PlotController();
            controller.InputCommandBindings.Clear();
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
            var command = new DelegatePlotControllerCommand<OxyMouseEventArgs>(
                (v, c, a) =>
                {
                    var point = series.InverseTransform(a.Position);
                    series.Points.Add(new ScatterPoint(point.X, point.Y));
                    model.InvalidatePlot(true);
                });

            var controller = new PlotController();
            controller.BindMouseDown(OxyMouseButton.Left, command);

            return new Example(model, controller);
        }
    }
}