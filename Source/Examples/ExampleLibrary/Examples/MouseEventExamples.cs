// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseEventExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Reflection;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("Mouse Events")]
    public class MouseEventExamples
    {
        [Example("PlotModel mouse events")]
        public static PlotModel MouseEvents()
        {
            var model = new PlotModel { Title = "Mouse events", Subtitle = "Left click and drag" };
            var yaxis = new LinearAxis { Position = AxisPosition.Left, Minimum = -1, Maximum = 1 };
            var xaxis = new LinearAxis { Position = AxisPosition.Bottom, Minimum = -1, Maximum = 1 };
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
                    s1 = new LineSeries
                    {
                        Title = "LineSeries" + (model.Series.Count + 1),
                        MarkerType = MarkerType.None,
                        StrokeThickness = 2
                    };
                    s1.Points.Add(xaxis.InverseTransform(e.Position.X, e.Position.Y, yaxis));
                    model.Series.Add(s1);
                    model.InvalidatePlot(false);
                    e.Handled = true;
                }
            };

            model.MouseMove += (s, e) =>
            {
                if (s1 != null)
                {
                    s1.Points.Add(xaxis.InverseTransform(e.Position.X, e.Position.Y, yaxis));
                    model.InvalidatePlot(false);
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

        [Example("MouseDown event and HitTestResult")]
        public static PlotModel MouseDownEventHitTestResult()
        {
            var model = new PlotModel { Title = "MouseDown HitTestResult", Subtitle = "Reports the index of the nearest point." };

            var s1 = new LineSeries();
            s1.Points.Add(new DataPoint(0, 10));
            s1.Points.Add(new DataPoint(10, 40));
            s1.Points.Add(new DataPoint(40, 20));
            s1.Points.Add(new DataPoint(60, 30));
            model.Series.Add(s1);
            s1.MouseDown += (s, e) =>
                {
                    model.Subtitle = "Index of nearest point in LineSeries: " + Math.Round(e.HitTestResult.Index);
                    model.InvalidatePlot(false);
                };

            var s2 = new ScatterSeries();
            s2.Points.Add(new ScatterPoint(0, 15));
            s2.Points.Add(new ScatterPoint(10, 45));
            s2.Points.Add(new ScatterPoint(40, 25));
            s2.Points.Add(new ScatterPoint(60, 35));
            model.Series.Add(s2);
            s2.MouseDown += (s, e) =>
                {
                    model.Subtitle = "Index of nearest point in ScatterSeries: " + (int)e.HitTestResult.Index;
                    model.InvalidatePlot(false);
                };

            return model;
        }

        [Example("LineSeries and PlotModel MouseDown event")]
        public static PlotModel MouseDownEvent()
        {
            var model = new PlotModel { Title = "MouseDown", Subtitle = "Left click to edit or add points." };
            var l = new Legend
            {
                LegendSymbolLength = 40
            };

            model.Legends.Add(l);

            // Add a line series
            var s1 = new LineSeries
            {
                Title = "LineSeries1",
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
                    model.InvalidatePlot(false);

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
                        model.InvalidatePlot(false);
                        e.Handled = true;
                    }
                };

            s1.MouseUp += (s, e) =>
            {
                // Stop editing
                indexOfPointToMove = -1;
                s1.LineStyle = LineStyle.Solid;
                model.InvalidatePlot(false);
                e.Handled = true;
            };

            model.MouseDown += (s, e) =>
                {
                    if (e.ChangedButton == OxyMouseButton.Left)
                    {
                        // Add a point to the line series.
                        s1.Points.Add(s1.InverseTransform(e.Position));
                        indexOfPointToMove = s1.Points.Count - 1;

                        model.InvalidatePlot(false);
                        e.Handled = true;
                    }
                };
            return model;
        }

        [Example("Add arrow annotations")]
        public static PlotModel AddAnnotations()
        {
            var model = new PlotModel { Title = "Add arrow annotations", Subtitle = "Press and drag the left mouse button" };
            var xaxis = new LinearAxis { Position = AxisPosition.Bottom };
            var yaxis = new LinearAxis { Position = AxisPosition.Left };
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
                    tmp.StartPoint = tmp.EndPoint = xaxis.InverseTransform(e.Position.X, e.Position.Y, yaxis);
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
                    tmp.EndPoint = xaxis.InverseTransform(e.Position.X, e.Position.Y, yaxis);
                    tmp.Text = string.Format("Y = {0:0.###}", tmp.EndPoint.Y);

                    // Redraw the plot
                    model.InvalidatePlot(false);
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
            var model = new PlotModel { Title = "LineAnnotation", Subtitle = "Click and drag the annotation line." };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            var la = new LineAnnotation { Type = LineAnnotationType.Vertical, X = 4 };
            la.MouseDown += (s, e) =>
                {
                    if (e.ChangedButton != OxyMouseButton.Left)
                    {
                        return;
                    }

                    la.StrokeThickness *= 5;
                    model.InvalidatePlot(false);
                    e.Handled = true;
                };

            // Handle mouse movements (note: this is only called when the mousedown event was handled)
            la.MouseMove += (s, e) =>
                {
                    la.X = la.InverseTransform(e.Position).X;
                    model.InvalidatePlot(false);
                    e.Handled = true;
                };
            la.MouseUp += (s, e) =>
                {
                    la.StrokeThickness /= 5;
                    model.InvalidatePlot(false);
                    e.Handled = true;
                };
            model.Annotations.Add(la);
            return model;
        }

        [Example("ArrowAnnotation")]
        public static PlotModel ArrowAnnotation()
        {
            var model = new PlotModel { Title = "ArrowAnnotation", Subtitle = "Click and drag the arrow." };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -40, Maximum = 60 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });

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
            var model = new PlotModel { Title = "PolygonAnnotation", Subtitle = "Click the polygon" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -20, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 20 });
            var pa = new PolygonAnnotation
                {
                    Text = "Polygon 1"
                };
            pa.Points.AddRange(new[]
                            {
                                new DataPoint(4, -2), new DataPoint(8, -4), new DataPoint(17, 7), new DataPoint(5, 8),
                                new DataPoint(2, 5)
                            });

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

        [Example("TextAnnotation")]
        public static PlotModel TextAnnotation()
        {
            var model = new PlotModel { Title = "TextAnnotation", Subtitle = "Click the text" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            var ta = new TextAnnotation
            {
                TextPosition = new DataPoint(4, -2),
                Text = "Click here"
            };

            // Handle left mouse clicks
            ta.MouseDown += (s, e) =>
            {
                if (e.ChangedButton != OxyMouseButton.Left)
                {
                    return;
                }

                ta.Background = ta.Background.IsUndefined() ? OxyColors.LightGreen : OxyColors.Undefined;
                model.InvalidatePlot(false);
                e.Handled = true;
            };

            model.Annotations.Add(ta);
            return model;
        }

        [Example("ImageAnnotation")]
        public static PlotModel ImageAnnotation()
        {
            var model = new PlotModel { Title = "ImageAnnotation", Subtitle = "Click the image" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });

            OxyImage image;
            var assembly = typeof(MouseEventExamples).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("ExampleLibrary.Resources.OxyPlot.png"))
            {
                image = new OxyImage(stream);
            }

            var ia = new ImageAnnotation { ImageSource = image, X = new PlotLength(4, PlotLengthUnit.Data), Y = new PlotLength(2, PlotLengthUnit.Data), HorizontalAlignment = HorizontalAlignment.Right };
            model.Annotations.Add(ia);

            // Handle left mouse clicks
            ia.MouseDown += (s, e) =>
            {
                if (e.ChangedButton != OxyMouseButton.Left)
                {
                    return;
                }

                ia.HorizontalAlignment = ia.HorizontalAlignment == HorizontalAlignment.Right ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                model.InvalidatePlot(false);
                e.Handled = true;
            };

            return model;
        }

        [Example("Add Series")]
        public static PlotModel AddSeriesByMouseDownEvent()
        {
            var model = new PlotModel { Title = "MouseDown", Subtitle = "Left click to add series." };
            var l = new Legend
            {
                LegendSymbolLength = 40
            };

            model.Legends.Add(l);

            model.MouseDown += (s, e) =>
            {
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    double a = model.Series.Count + 1;
                    model.Series.Add(new FunctionSeries(x => Math.Sin(a * x), 0, 10, 1000));
                    model.InvalidatePlot(true);
                    e.Handled = true;
                }
            };

            return model;
        }

        [Example("Select range")]
        public static PlotModel SelectRange()
        {
            var model = new PlotModel { Title = "Select range", Subtitle = "Left click and drag to select a range." };
            model.Series.Add(new FunctionSeries(Math.Cos, 0, 40, 0.1));

            var range = new RectangleAnnotation { Fill = OxyColor.FromAColor(120, OxyColors.SkyBlue), MinimumX = 0, MaximumX = 0 };
            model.Annotations.Add(range);

            double startx = double.NaN;

            model.MouseDown += (s, e) =>
            {
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    startx = range.InverseTransform(e.Position).X;
                    range.MinimumX = startx;
                    range.MaximumX = startx;
                    model.InvalidatePlot(true);
                    e.Handled = true;
                }
            };
            model.MouseMove += (s, e) =>
                {
                    if (!double.IsNaN(startx))
                    {
                        var x = range.InverseTransform(e.Position).X;
                        range.MinimumX = Math.Min(x, startx);
                        range.MaximumX = Math.Max(x, startx);
                        range.Text = string.Format("∫ cos(x) dx =  {0:0.00}", Math.Sin(range.MaximumX) - Math.Sin(range.MinimumX));
                        model.Subtitle = string.Format("Integrating from {0:0.00} to {1:0.00}", range.MinimumX, range.MaximumX);
                        model.InvalidatePlot(true);
                        e.Handled = true;
                    }
                };

            model.MouseUp += (s, e) =>
            {
                startx = double.NaN;
            };

            return model;
        }

        [Example("Hover")]
        public static PlotModel Hover()
        {
            var model = new PlotModel { Title = "Hover" };
            LineSeries series = null;

            model.MouseEnter += (s, e) =>
            {
                model.Subtitle = "The mouse entered";
                series = new LineSeries();
                model.Series.Add(series);
                model.InvalidatePlot(false);
                e.Handled = true;
            };

            model.MouseMove += (s, e) =>
            {
                if (series != null && series.XAxis != null)
                {
                    series.Points.Add(series.InverseTransform(e.Position));
                    model.InvalidatePlot(false);
                }
            };

            model.MouseLeave += (s, e) =>
            {
                model.Subtitle = "The mouse left";
                model.InvalidatePlot(false);
                e.Handled = true;
            };

            return model;
        }

        [Example("Touch")]
        public static PlotModel Touch()
        {
            var model = new PlotModel { Title = "Touch" };
            var series = new LineSeries();
            model.Series.Add(series);

            model.TouchStarted += (s, e) =>
            {
                model.Subtitle = "The touch gesture started";
                model.InvalidatePlot(false);
                e.Handled = true;
            };

            model.TouchDelta += (s, e) =>
            {
                series.Points.Add(series.InverseTransform(e.Position));
                model.InvalidatePlot(false);
            };

            model.TouchCompleted += (s, e) =>
            {
                model.Subtitle = "The touch gesture completed";
                model.InvalidatePlot(false);
                e.Handled = true;
            };

            return model;
        }

        [Example("Touch on a LineSeries")]
        public static PlotModel TouchSeries()
        {
            var model = new PlotModel { Title = "Touch on a LineSeries" };
            var series = new LineSeries();
            series.Points.Add(new DataPoint(0, 0));
            series.Points.Add(new DataPoint(10, 10));
            model.Series.Add(series);

            series.TouchStarted += (s, e) =>
            {
                model.Subtitle = "The touch gesture started on the LineSeries";
                model.InvalidatePlot(false);
                e.Handled = true;
            };

            series.TouchDelta += (s, e) =>
            {
                series.Points.Add(series.InverseTransform(e.Position));
                model.InvalidatePlot(false);
            };

            series.TouchCompleted += (s, e) =>
            {
                model.Subtitle = "The touch gesture completed";
                model.InvalidatePlot(false);
                e.Handled = true;
            };

            return model;
        }

        [Example("RectangleAnnotation click")]
        public static PlotModel RectangleAnnotationClick()
        {
            var plotModel = new PlotModel { Title = "RectangleAnnotation click" };

            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var annotation = new RectangleAnnotation() { MinimumX = 10, MaximumX = 60, MinimumY = 10, MaximumY = 20 };
            plotModel.Annotations.Add(annotation);

            int i = 0;
            annotation.MouseDown += (s, e) =>
            {
                annotation.Text = "Clicked " + (++i) + " times.";
                plotModel.InvalidatePlot(false);
            };

            return plotModel;
        }

        [Example("Clicking on an annotation")]
        public static PlotModel ClickingOnAnAnnotation()
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

            return plotModel;
        }
    }
}
