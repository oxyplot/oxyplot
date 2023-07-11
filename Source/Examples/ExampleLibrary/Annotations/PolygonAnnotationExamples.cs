// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolygonAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("PolygonAnnotation"), Tags("Annotations")]
    public static class PolygonAnnotationExamples
    {
        [Example("PolygonAnnotation")]
        public static PlotModel PolygonAnnotation()
        {
            var model = new PlotModel { Title = "PolygonAnnotation" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            var a1 = new PolygonAnnotation { Text = "Polygon 1" };
            a1.Points.AddRange(new[] { new DataPoint(4, -2), new DataPoint(8, -4), new DataPoint(17, 7), new DataPoint(5, 8), new DataPoint(2, 5) });
            model.Annotations.Add(a1);
            return model;
        }

        [Example("PolygonAnnotation with custom text position and alignment")]
        public static PlotModel PolygonAnnotationTextPosition()
        {
            var model = new PlotModel { Title = "PolygonAnnotation with fixed text position and alignment" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            var a1 = new PolygonAnnotation { Text = "Polygon 1", TextHorizontalAlignment = HorizontalAlignment.Left, TextVerticalAlignment = VerticalAlignment.Bottom, TextPosition = new DataPoint(4.1, -1.9) };
            a1.Points.AddRange(new[] { new DataPoint(4, -2), new DataPoint(8, -2), new DataPoint(17, 7), new DataPoint(5, 8), new DataPoint(4, 5) });
            model.Annotations.Add(a1);
            return model;
        }

        [Example("AnnotationLayer property")]
        public static PlotModel AnnotationLayerProperty()
        {
            var model = new PlotModel { Title = "Annotation Layers" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 30, MajorGridlineStyle = LineStyle.Solid, MajorGridlineThickness = 1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10, MajorGridlineStyle = LineStyle.Solid, MajorGridlineThickness = 1 });

            var a1 = new PolygonAnnotation
                         {
                             Layer = AnnotationLayer.BelowAxes,
                             Text = "Layer = BelowAxes"
                         };
            a1.Points.AddRange(new[]
                                   {
                                       new DataPoint(-11, -2), new DataPoint(-7, -4), new DataPoint(-3, 7), new DataPoint(-10, 8),
                                       new DataPoint(-13, 5)
                                   });
            model.Annotations.Add(a1);
            var a2 = new PolygonAnnotation
                         {
                             Layer = AnnotationLayer.BelowSeries,
                             Text = "Layer = BelowSeries"
                         };
            a2.Points.AddRange(new DataPoint[]
                                   {
                                       new DataPoint(4, -2), new DataPoint(8, -4), new DataPoint(12, 7), new DataPoint(5, 8),
                                       new DataPoint(2, 5)
                                   });
            model.Annotations.Add(a2);
            var a3 = new PolygonAnnotation { Layer = AnnotationLayer.AboveSeries, Text = "Layer = AboveSeries" };
            a3.Points.AddRange(new[] { new DataPoint(19, -2), new DataPoint(23, -4), new DataPoint(27, 7), new DataPoint(20, 8), new DataPoint(17, 5) });
            model.Annotations.Add(a3);

            model.Series.Add(new FunctionSeries(Math.Sin, -20, 30, 400));
            return model;
        }

        [Example("Koch Snowflakes")]
        public static PlotModel KockSnowflakes()
        {
            DataPoint[] triangle(DataPoint centre)
            {
                return new[]
                {
                    new DataPoint(centre.X, centre.Y + 1),
                    new DataPoint(centre.X + Math.Sin(Math.PI * 2 / 3), centre.Y + Math.Cos(Math.PI * 2 / 3)),
                    new DataPoint(centre.X + Math.Sin(Math.PI * 4 / 3), centre.Y + Math.Cos(Math.PI * 4 / 3)),
                };
            }

            var model = new PlotModel { Title = "PolygonAnnotation", PlotType = PlotType.Cartesian };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -4, Maximum = 4 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -2, Maximum = 2 });

            var a1 = new PolygonAnnotation { Text = "MSL = 4", MinimumSegmentLength = 4 };
            a1.Points.AddRange(KochFractal(triangle(new DataPoint(-2, 0)), 8, true, true));
            model.Annotations.Add(a1);

            var a2 = new PolygonAnnotation { Text = "MSL = 2", MinimumSegmentLength = 2 };
            a2.Points.AddRange(KochFractal(triangle(new DataPoint(0, 0)), 8, true, true));
            model.Annotations.Add(a2);

            var a3 = new PolygonAnnotation { Text = "MSL = 1", MinimumSegmentLength = 1 };
            a3.Points.AddRange(KochFractal(triangle(new DataPoint(2, 0)), 8, true, true));
            model.Annotations.Add(a3);

            return model;
        }

        public static DataPoint[] KochFractal(DataPoint[] seed, int n, bool clockwise, bool closed)
        {
            var cos60 = Math.Cos(Math.PI / 3);
            var sin60 = Math.Sin(Math.PI / 3);
            var cur = seed;

            for (int i = 0; i < n; i++)
            {
                var next = new DataPoint[closed ? cur.Length * 4 : cur.Length * 4 - 3];
                for (int j = 0; j < (closed ? cur.Length : cur.Length - 1); j++)
                {
                    var p0 = cur[j];
                    var p1 = cur[(j + 1) % cur.Length];

                    var dx = (p1.X - p0.X) / 3;
                    var dy = (p1.Y - p0.Y) / 3;

                    double dx2, dy2;
                    if (clockwise)
                    {
                        dx2 = cos60 * dx - sin60 * dy;
                        dy2 = cos60 * dy + sin60 * dx;
                    }
                    else
                    {
                        dx2 = cos60 * dx - sin60 * dy;
                        dy2 = cos60 * dy + sin60 * dx;
                    }

                    next[j * 4] = p0;
                    next[j * 4 + 1] = new DataPoint(p0.X + dx, p0.Y + dy);
                    next[j * 4 + 2] = new DataPoint(p0.X + dx + dx2, p0.Y + dy + dy2);
                    next[j * 4 + 3] = new DataPoint(p0.X + dx * 2, p0.Y + dy * 2);
                }

                if (!closed)
                {
                    next[next.Length - 1] = cur[cur.Length - 1];
                }

                cur = next;
            }

            return cur;
        }
    }
}
