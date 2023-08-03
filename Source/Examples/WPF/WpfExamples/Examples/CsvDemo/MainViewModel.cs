// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CsvDemo
{
    using System.Globalization;
    using System.IO;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    using WpfExamples;

    public class MainViewModel : Observable
    {
        private PlotModel model;

        public PlotModel Model
        {
            get
            {
                return this.model;
            }

            set
            {
                if (this.model != value)
                {
                    this.SetValue(ref this.model, value);
                }
            }
        }

        public void Open(string file)
        {
            var doc = new CsvDocument();
            doc.Load(file);
            var tmp = new PlotModel
            {
                Title = Path.GetFileNameWithoutExtension(file),
                PlotMargins = new OxyThickness(50, 0, 0, 40)
            };

            var l = new Legend
            {
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside
            };

            tmp.Legends.Add(l);

            for (int i = 1; i < doc.Headers.Length; i++)
            {
                var ls = new LineSeries { Title = doc.Headers[i] };
                foreach (var item in doc.Items)
                {
                    double x = this.ParseDouble(item[0]);
                    double y = this.ParseDouble(item[i]);
                    ls.Points.Add(new DataPoint(x, y));
                }

                tmp.Series.Add(ls);
            }

            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = doc.Headers[0] });
            this.Model = tmp;
        }

        private double ParseDouble(string s)
        {
            if (s == null)
            {
                return double.NaN;
            }
            s = s.Replace(',', '.');
            double result;
            if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }

            return double.NaN;
        }
    }
}
