using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleLibrary.Series
{
    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Legends;
    using OxyPlot.Axes;

    [Examples("PolygonSeries"), Tags("Series")]
    public static class PolygonSeriesExamples
    {
        [Example("PolygonSeries")]
        [DocumentationExample("Series/PolygonSeries")]
        public static PlotModel PolygonSeries()
        {
            var model = new PlotModel { Title = "Polygon Series", PlotType = PlotType.Cartesian };

            model.Axes.Add(new LinearColorAxis());

            var ps = new PolygonSeries();
            var outlines = new List<DataPoint[]>();
            for (int i = 0; i < 5; i++)
            {
                ps.Items.Add(new PolygonItem(RegularPolygon(new DataPoint(i * 5, 0), 2, 3 + i), i));
                outlines.Add(RegularPolygon(new DataPoint(i * 5, 5), 2, 3 + i));
            }
            ps.Items.Add(new PolygonItem(outlines, 10));
            model.Series.Add(ps);

            return model;
        }

        [Example("Hexagonal Grid")]
        [DocumentationExample("Series/PolygonSeries")]
        public static PlotModel HexGrid()
        {
            var model = new PlotModel { Title = "Hexagonal Grid" };
            
            model.Axes.Add(new LinearColorAxis() { Position = AxisPosition.Right });

            double eval(DataPoint p) => Math.Sin(p.X / 5) + Math.Sqrt(p.Y);

            var dim = 1.0;
            var w = dim * 2 - Math.Cos(Math.PI / 3) * dim;
            var h = Math.Sin(Math.PI / 3) * dim * 2;

            var ps = new PolygonSeries() { Stroke = OxyColors.Black, StrokeThickness = 1 };
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 40; y++)
                {
                    var oy = (x % 2 == 0) ? 0 : h / 2;

                    var p = new DataPoint(x * w, y * h + oy);

                    ps.Items.Add(new PolygonItem(RegularPolygon(p, dim, 6), eval(p)));
                }
            }
            model.Series.Add(ps);

            return model;
        }

        private static DataPoint[] RegularPolygon(DataPoint center, double dimension, int polyCount)
        {
            var res = new DataPoint[polyCount];
            
            for (int i = 0; i < res.Length; i++)
            {
                var angle = Math.PI * 2 * i / res.Length;

                var x = Math.Cos(angle) * dimension;
                var y = Math.Sin(angle) * dimension;

                res[i] = new DataPoint(center.X + x, center.Y + y);
            }

            return res;
        }
    }
}
