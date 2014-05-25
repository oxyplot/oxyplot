// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomAxisExamples.cs" company="OxyPlot">
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
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Axes;

    [Examples("Custom axes")]
    public static class CustomAxisExamples
    {
        public class ArrowAxis : LinearAxis
        {
            public override void Render(IRenderContext rc, PlotModel model, AxisLayer axisLayer, int pass)
            {
                base.Render(rc, model, axisLayer, pass);
                var points = new List<ScreenPoint>();
                if (this.IsHorizontal())
                {
                    var xmax = this.Transform(this.ActualMaximum);
                    points.Add(new ScreenPoint(xmax + 4, model.PlotArea.Bottom - 4));
                    points.Add(new ScreenPoint(xmax + 18, model.PlotArea.Bottom));
                    points.Add(new ScreenPoint(xmax + 4, model.PlotArea.Bottom + 4));
                    //// etc.
                }
                else
                {
                    var ymax = this.Transform(this.ActualMaximum);
                    points.Add(new ScreenPoint(model.PlotArea.Left - 4, ymax - 4));
                    points.Add(new ScreenPoint(model.PlotArea.Left, ymax - 18));
                    points.Add(new ScreenPoint(model.PlotArea.Left + 4, ymax - 4));
                    //// etc.
                }

                rc.DrawPolygon(points, OxyColors.Black, OxyColors.Undefined);
            }
        }

        [Example("ArrowAxis")]
        public static PlotModel CustomArrowAxis()
        {
            // https://oxyplot.codeplex.com/discussions/467255
            var model = new PlotModel { PlotAreaBorderThickness = new OxyThickness(0), PlotMargins = new OxyThickness(60, 60, 60, 60) };
            model.Axes.Add(new ArrowAxis { Position = AxisPosition.Bottom, AxislineStyle = LineStyle.Solid });
            model.Axes.Add(new ArrowAxis { Position = AxisPosition.Left, AxislineStyle = LineStyle.Solid });
            return model;
        }
    }
}