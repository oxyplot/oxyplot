using System;

namespace OxyPlot
{
    public class LineSeries : DataSeries
    {
        public LineSeries()
        {
            MinimumSegmentLength = 2;
            Thickness = 2;

            MarkerSize = 3;
            MarkerStrokeThickness = 1;
        }

        public LineSeries(OxyColor c, double thickness = 1, string title = null)
            : this()
        {
            this.Color = c;
            this.Thickness = thickness;
            this.Title = title;
        }

        public OxyColor Color { get; set; }


        public double Thickness { get; set; }
        public LineStyle LineStyle { get; set; }
        public double[] Dashes { get; set; }

        public MarkerType MarkerType { get; set; }
        public double MarkerSize { get; set; }
        public OxyColor MarkerStroke { get; set; }
        public double MarkerStrokeThickness { get; set; }
        public OxyColor MarkerFill { get; set; }

        /// <summary>
        /// Minimum length of a segment on the curve
        /// Increasing this number will increase performance, 
        /// but make the curve less accurate
        /// </summary>
        public double MinimumSegmentLength { get; set; }
    }

    public class FunctionSeries : LineSeries
    {
        public FunctionSeries(Func<double, double> f, double xmin, double xmax, double dx, string title = null)
        {
            Title = title;
            for (double x = xmin; x <= xmax+dx/2; x += dx)
                Points.Add(new DataPoint(x, f(x)));
        }

        public FunctionSeries(Func<double, double> fx, Func<double, double> fy, double t0, double t1, double dt, string title = null)
        {
            Title = title;
            for (double t = t0; t <= t1+dt/2; t += dt)
                Points.Add(new DataPoint(fx(t), fy(t)));
        }
    }
}