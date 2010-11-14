using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace OxyPlot
{
    public class AreaSeries : LineSeries
    {
        public Color Fill { get; set; }
        public string DataFieldBase { get; set; }
        public double Baseline { get; set; }
        public Collection<double> BaselineValues { get; set; }

        public AreaSeries()
        {
            BaselineValues = new Collection<double>();
        }

        internal override void UpdatePointsFromItemsSource()
        {
            base.UpdatePointsFromItemsSource();

            if (ItemsSource == null) return;
            BaselineValues.Clear();

            // Do nothing if ItemsSource is set before DataFields are set
            if (DataFieldBase == null)
                return;

            PropertyInfo piy = null;
            Type t = null;

            foreach (object o in ItemsSource)
            {
                if (piy == null || o.GetType() != t)
                {
                    t = o.GetType();
                    piy = t.GetProperty(DataFieldBase);
                    if (piy == null)
                        throw new InvalidOperationException(string.Format("Could not find data field {0} on type {1}",
                                                                          DataFieldBase, t));
                }
                var y = (double)piy.GetValue(o, null);

                BaselineValues.Add(y);
            }
        }
        public override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            if (BaselineValues != null)
                foreach (double d in BaselineValues)
                {
                    MinY = Math.Min(MinY, d);
                    MaxY = Math.Max(MaxY, d);
                }
        }
    }
}