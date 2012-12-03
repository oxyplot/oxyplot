// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseEventExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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

    [Examples("Mouse Events")]
    public class MouseEventExamples : ExamplesBase
    {
        [Example("PlotModel mouse events")]
        public static PlotModel MouseEvents()
        {
            var model = new PlotModel("Mouse events", "Left click and drag");
            var yaxis = new LinearAxis(AxisPosition.Left, -1, 1);
            var xaxis = new LinearAxis(AxisPosition.Bottom, -1, 1);
            model.Axes.Add(yaxis);
            model.Axes.Add(xaxis);

            LineSeries s1 = null;

            // Subscribe to the mouse down event on the line series
            model.MouseDown += (s, e) =>
            {
                // only handle the left mouse button (right button can still be used to pan)
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    // Add a line series
                    s1 = new LineSeries("LineSeries" + (model.Series.Count + 1))
                    {
                        // Color = OxyColors.SkyBlue,
                        MarkerType = MarkerType.None,
                        StrokeThickness = 2
                    };
                    s1.Points.Add(Axis.InverseTransform(e.Position, xaxis, yaxis));
                    model.Series.Add(s1);
                    model.RefreshPlot(false);
                    e.Handled = true;
                }
            };

            model.MouseMove += (s, e) =>
            {
                if (s1 != null)
                {
                    s1.Points.Add(Axis.InverseTransform(e.Position, xaxis, yaxis));
                    model.RefreshPlot(false);
                    e.Handled = true;
                }
            };

            model.MouseUp += (s, e) =>
            {
                if (s1 != null)
                {
                    s1 = null;
                    e.Handled = true;
                }
            };
            return model;
        }

        [Example("LineSeries and PlotModel MouseDown event")]
        public static PlotModel MouseDownEvent()
        {
            var model = new PlotModel("MouseDown", "Left click to edit or add points.") { LegendSymbolLength = 40 };

            // Add a line series
            var s1 = new LineSeries("LineSeries1")
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };
            s1.Points.Add(new DataPoint(0, 10));
            s1.Points.Add(new DataPoint(10, 40));
            s1.Points.Add(new DataPoint(40, 20));
            s1.Points.Add(new DataPoint(60, 30));
            model.Series.Add(s1);

            int indexOfPointToMove = -1;

            // Subscribe to the mouse down event on the line series
            s1.MouseDown += (s, e) =>
            {
                // only handle the left mouse button (right button can still be used to pan)
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    int indexOfNearestPoint = (int)Math.Round(e.HitTestResult.Index);
                    var nearestPoint = s1.Transform(s1.Points[indexOfNearestPoint]);

                    // Check if we are near a point
                    if ((nearestPoint - e.Position).Length < 10)
                    {
                        // Start editing this point
                        indexOfPointToMove = indexOfNearestPoint;
                    }
                    else
                    {
                        // otherwise create a point on the current line segment
                        int i = (int)e.HitTestResult.Index + 1;
                        s1.Points.Insert(i, s1.InverseTransform(e.Position));
                        indexOfPointToMove = i;
                    }

                    // Change the linestyle while editing
                    s1.LineStyle = LineStyle.DashDot;

                    // Remember to refresh/invalidate of the plot
                    model.RefreshPlot(false);

                    // Set the event arguments to handled - no other handlers will be called.
                    e.Handled = true;
                }
            };

            s1.MouseMove += (s, e) =>
                {
                    if (indexOfPointToMove >= 0)
                    {
                        // Move the point being edited.
                        s1.Points[indexOfPointToMove] = s1.InverseTransform(e.Position);
                        model.RefreshPlot(false);
                        e.Handled = true;
                    }
                };

            s1.MouseUp += (s, e) =>
            {
                // Stop editing
                indexOfPointToMove = -1;
                s1.LineStyle = LineStyle.Solid;
                model.RefreshPlot(false);
                e.Handled = true;
            };

            model.MouseDown += (s, e) =>
                {
                    if (e.ChangedButton == OxyMouseButton.Left)
                    {
                        // Add a point to the line series.
                        s1.Points.Add(s1.InverseTransform(e.Position));
                        indexOfPointToMove = s1.Points.Count - 1;

                        model.RefreshPlot(false);
                        e.Handled = true;
                    }
                };
            return model;
        }

        [Example("Add annotations")]
        public static PlotModel AddAnnotations()
        {
            var model = new PlotModel("Add arrow annotations", "Press and drag the left mouse button");
            var xaxis = new LinearAxis(AxisPosition.Bottom);
            var yaxis = new LinearAxis(AxisPosition.Left);
            model.Axes.Add(xaxis);
            model.Axes.Add(yaxis);
            model.Series.Add(new FunctionSeries(x => Math.Sin(x / 4) * Math.Acos(Math.Sin(x)), 0, Math.PI * 8, 2000, "sin(x/4)*acos(sin(x))"));

            ArrowAnnotation tmp = null;

            // Add handlers to the PlotModel's mouse events
            model.MouseDown += (s, e) =>
            {
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    // Create a new arrow annotation
                    tmp = new ArrowAnnotation();
                    tmp.StartPoint = tmp.EndPoint = Axis.InverseTransform(e.Position, xaxis, yaxis);
                    model.Annotations.Add(tmp);
                    e.Handled = true;
                }
            };

            // Handle mouse movements (note: this is only called when the mousedown event was handled)
            model.MouseMove += (s, e) =>
            {
                if (tmp != null)
                {
                    // Modify the end point
                    tmp.EndPoint = Axis.InverseTransform(e.Position, xaxis, yaxis);
                    tmp.Text = string.Format("Y = {0:0.###}", tmp.EndPoint.Y);

                    // Redraw the plot
                    model.RefreshPlot(false);
                    e.Handled = true;
                }
            };

            model.MouseUp += (s, e) =>
                {
                    if (tmp != null)
                    {
                        tmp = null;
                        e.Handled = true;
                    }
                };

            return model;
        }

        [Example("LineAnnotation")]
        public static PlotModel LineAnnotation()
        {
            var model = new PlotModel("LineAnnotation", "Click and drag the annotation line.");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -20, 80));
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -10, 10));
            var la = new LineAnnotation { Type = LineAnnotationType.Vertical, X = 4 };
            la.MouseDown += (s, e) =>
                {
                    if (e.ChangedButton != OxyMouseButton.Left)
                    {
                        return;
                    }

                    la.StrokeThickness *= 5;
                    model.RefreshPlot(false);
                    e.Handled = true;
                };

            // Handle mouse movements (note: this is only called when the mousedown event was handled)
            la.MouseMove += (s, e) =>
                {
                    la.X = la.InverseTransform(e.Position).X;
                    model.RefreshPlot(false);
                    e.Handled = true;
                };
            la.MouseUp += (s, e) =>
                {
                    la.StrokeThickness /= 5;
                    model.RefreshPlot(false);
                    e.Handled = true;
                };
            model.Annotations.Add(la);
            return model;
        }

        [Example("ArrowAnnotation")]
        public static PlotModel ArrowAnnotation()
        {
            var model = new PlotModel("ArrowAnnotation", "Click and drag the arrow.");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -40, 60));
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -10, 10));

            var arrow = new ArrowAnnotation { StartPoint = new DataPoint(8, 4), EndPoint = new DataPoint(0, 0), Text = "Move me!" };

            var lastPoint = DataPoint.Undefined;
            bool moveStartPoint = false;
            bool moveEndPoint = false;
            var originalColor = OxyColors.White;

            // Handle left mouse clicks
            arrow.MouseDown += (s, e) =>
            {
                if (e.ChangedButton != OxyMouseButton.Left)
                {
                    return;
                }

                lastPoint = arrow.InverseTransform(e.Position);
                moveStartPoint = e.HitTestResult.Index != 2;
                moveEndPoint = e.HitTestResult.Index != 1;
                originalColor = arrow.Color;
                arrow.Color = OxyColors.Red;
                model.InvalidatePlot(false);
                e.Handled = true;
            };

            // Handle mouse movements (note: this is only called when the mousedown event was handled)
            arrow.MouseMove += (s, e) =>
                {
                    var thisPoint = arrow.InverseTransform(e.Position);
                    double dx = thisPoint.X - lastPoint.X;
                    double dy = thisPoint.Y - lastPoint.Y;
                    if (moveStartPoint)
                    {
                        arrow.StartPoint = new DataPoint(arrow.StartPoint.X + dx, arrow.StartPoint.Y + dy);
                    }

                    if (moveEndPoint)
                    {
                        arrow.EndPoint = new DataPoint(arrow.EndPoint.X + dx, arrow.EndPoint.Y + dy);
                    }

                    lastPoint = thisPoint;
                    model.InvalidatePlot(false);
                    e.Handled = true;
                };

            // Handle mouse up (note: this is only called when the mousedown event was handled)
            arrow.MouseUp += (s, e) =>
            {
                arrow.Color = originalColor;
            };
            model.Annotations.Add(arrow);
            return model;
        }

        [Example("PolygonAnnotation")]
        public static PlotModel PolygonAnnotation()
        {
            var model = new PlotModel("PolygonAnnotation", "Click the polygon");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -20, 20));
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -10, 10));
            var pa = new PolygonAnnotation
                {
                    Points =
                        new[]
                            {
                                new DataPoint(4, -2), new DataPoint(8, -4), new DataPoint(17, 7), new DataPoint(5, 8),
                                new DataPoint(2, 5)
                            },
                    Text = "Polygon 1"
                };

            // Handle left mouse clicks
            int hitCount = 1;
            pa.MouseDown += (s, e) =>
                {
                    if (e.ChangedButton != OxyMouseButton.Left)
                    {
                        return;
                    }

                    pa.Text = "Hit # " + hitCount++;
                    model.InvalidatePlot(false);
                    e.Handled = true;
                };

            model.Annotations.Add(pa);
            return model;
        }

        [Example("Add Series")]
        public static PlotModel AddSeriesByMouseDownEvent()
        {
            var model = new PlotModel("MouseDown", "Left click to add series.") { LegendSymbolLength = 40 };

            model.MouseDown += (s, e) =>
            {
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    double a = model.Series.Count + 1;
                    model.Series.Add(new FunctionSeries(x => Math.Sin(a * x), 0, 10, 1000));
                    model.RefreshPlot(true);
                    e.Handled = true;
                }
            };

            return model;
        }

        [Example("Select range")]
        public static PlotModel SelectRange()
        {
            var model = new PlotModel("Select range", "Left click and drag to select a range.");
            model.Series.Add(new FunctionSeries(Math.Cos, 0, 40, 0.1));

            RectangleAnnotation range = null;

            double startx = double.NaN;

            model.MouseDown += (s, e) =>
            {
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    if (range == null)
                    {
                        // Create and add the annotation to the plot
                        range = new RectangleAnnotation { Fill = OxyColors.SkyBlue.ChangeAlpha(120) };
                        model.Annotations.Add(range);
                        model.RefreshPlot(true);
                    }

                    startx = range.InverseTransform(e.Position).X;
                    range.MinimumX = startx;
                    range.MaximumX = startx;
                    model.RefreshPlot(true);
                    e.Handled = true;
                }
            };
            model.MouseMove += (s, e) =>
                {
                    if (e.ChangedButton == OxyMouseButton.Left && !double.IsNaN(startx))
                    {
                        var x = range.InverseTransform(e.Position).X;
                        range.MinimumX = Math.Min(x, startx);
                        range.MaximumX = Math.Max(x, startx);
                        range.Text = string.Format("∫ cos(x) dx =  {0:0.00}", Math.Sin(range.MaximumX) - Math.Sin(range.MinimumX));
                        model.Subtitle = string.Format("Integrating from {0:0.00} to {1:0.00}", range.MinimumX, range.MaximumX);
                        model.RefreshPlot(true);
                        e.Handled = true;
                    }
                };

            model.MouseUp += (s, e) =>
            {
                startx = double.NaN;
            };

            return model;
        }
    }
}