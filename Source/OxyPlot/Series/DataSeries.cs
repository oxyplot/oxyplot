using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;

namespace OxyPlot
{
    public interface IDataPointProvider
    {
        DataPoint GetDataPoint();
    }

    public abstract class DataSeries
    {
        protected DataSeries()
        {
            Points = new Collection<DataPoint>();
        }

        public IEnumerable ItemsSource { get; set; }
        public string DataFieldX { get; set; }
        public string DataFieldY { get; set; }

        public Collection<DataPoint> Points { get; set; }

        public Axis XAxis { get; internal set; }
        public Axis YAxis { get; internal set; }

        public string XAxisKey { get; set; }
        public string YAxisKey { get; set; }

        public string Title { get; set; }

        public bool Smooth { get; set; }

        public double MinX { get; protected set; }
        public double MaxX { get; protected set; }
        public double MinY { get; protected set; }
        public double MaxY { get; protected set; }

        internal virtual void UpdatePointsFromItemsSource()
        {
            if (ItemsSource == null) return;
            Points.Clear();

            // Get DataPoints from the items in ItemsSource 
            // if they implement IDataPointProvider
            // If DataFields are set, this is not used
            if (DataFieldX == null || DataFieldY == null)
            {
                foreach (var item in ItemsSource)
                {
                    var idpp = item as IDataPointProvider;
                    if (idpp == null)
                        continue;
                    Points.Add(idpp.GetDataPoint());
                }
                return;
            }

            // TODO: is there a better way to do this?
            // http://msdn.microsoft.com/en-us/library/bb613546.aspx

            // Using reflection on DataFieldX and DataFieldY

            PropertyInfo pix = null;
            PropertyInfo piy = null;
            Type t = null;

            foreach (object o in ItemsSource)
            {
                if (pix == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pix = t.GetProperty(DataFieldX);
                    piy = t.GetProperty(DataFieldY);
                    if (pix == null)
                        throw new InvalidOperationException(string.Format("Could not find data field {0} on type {1}",
                                                                          DataFieldX, t));
                    if (piy == null)
                        throw new InvalidOperationException(string.Format("Could not find data field {0} on type {1}",
                                                                          DataFieldY, t));
                }
                var x = (double)pix.GetValue(o, null);
                var y = (double)piy.GetValue(o, null);


                var pp = new DataPoint(x, y);
                Points.Add(pp);
            }
        }

        public virtual void UpdateMaxMin()
        {
            MinX = MinY = MaxX = MaxY = double.NaN;

            if (Points == null || Points.Count == 0)
                return;
            MinX = MaxX = Points[0].x;
            MinY = MaxY = Points[0].y;
            foreach (DataPoint pt in Points)
            {
                MinX = Math.Min(MinX, pt.x);
                MaxX = Math.Max(MaxX, pt.x);

                MinY = Math.Min(MinY, pt.y);
                MaxY = Math.Max(MaxY, pt.y);
            }
        }

        public DataPoint? GetNearestPoint(DataPoint p3)
        {
            double mindist = double.MaxValue;
            DataPoint? pt = null;
            foreach (var p in Points)
            {
                double dx = p3.X - p.X;
                double dy = p3.Y - p.Y;
                double d2 = dx * dx + dy * dy;

                if (d2 < mindist)
                {
                    pt = p;
                    mindist = d2;
                }
            }
            return pt;
        }

        public DataPoint? GetNearestPointOnLine(DataPoint p3)
        {
            // http://local.wasp.uwa.edu.au/~pbourke/geometry/pointline/
            double mindist = double.MaxValue;
            DataPoint? pt = null;
            for (int i = 0; i + 1 < Points.Count; i++)
            {
                var p1 = Points[i];
                var p2 = Points[i + 1];

                double p21X = p2.X - p1.X;
                double p21Y = p2.Y - p1.Y;
                double u1 = (p3.X - p1.X) * p21X + (p3.Y - p1.Y) * p21Y;
                double u2 = p21X * p21X + p21Y * p21Y;
                if (u2 == 0)
                    continue; // P1 && P2 coincident
                double u = u1 / u2;
                if (u < 0 || u > 1)
                    continue; // outside line
                double x = p1.X + u * p21X;
                double y = p1.Y + u * p21Y;

                double dx = p3.X - x;
                double dy = p3.Y - y;
                double d2 = dx * dx + dy * dy;

                if (d2 < mindist)
                {
                    pt = new DataPoint(x, y);
                    mindist = d2;
                }
            }
            return pt;
        }

        public double? GetValueFromX(double x)
        {
            for (int i = 0; i + 1 < Points.Count; i++)
            {
                if (IsBetween(x, Points[i].X, Points[i + 1].X))
                {
                   return Points[i].Y + (Points[i + 1].Y - Points[i].Y) / (Points[i + 1].X - Points[i].X) * (x - Points[i].X);
                }
            }
            return null;
        }

        static bool IsBetween(double x, double x0, double x1)
        {
            if (x >= x0 && x <= x1) return true;
            if (x >= x1 && x <= x0) return true;
            return false;
        }
    }
}