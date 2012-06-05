// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using OxyPlot;

namespace CsvDemo
{
    public class MainViewModel : Observable
    {
        private PlotModel model;

        public MainViewModel()
        {
        }

        public PlotModel Model
        {
            get { return model; }
            set
            {
                if (model != value)
                {
                    model = value;
                    RaisePropertyChanged(() => Model);
                }
            }
        }

        public void Open(string file)
        {
            var doc = new CsvDocument();
            doc.Load(file);
            var tmp = new PlotModel(Path.GetFileNameWithoutExtension(file));
            tmp.LegendPosition = LegendPosition.RightTop;
            tmp.LegendPlacement = LegendPlacement.Outside;
            tmp.PlotMargins= new OxyThickness(50,0,0,40);
            for (int i = 1; i < doc.Headers.Length; i++)
            {
                var ls = new LineSeries(doc.Headers[i]);
                foreach (var item in doc.Items)
                {
                    double x = ParseDouble(item[0]);
                    double y = ParseDouble(item[i]); 
                    ls.Points.Add(new DataPoint(x, y));
                }
                tmp.Series.Add(ls);
            }
            tmp.Axes.Add(new LinearAxis(AxisPosition.Bottom, doc.Headers[0]));
            // tmp.Axes.Add(new LogarithmicAxis(AxisPosition.Left));
            Model = tmp;
        }

        private double ParseDouble(string s)
        {
            if (s == null)
                return double.NaN;
            s = s.Replace(',', '.');
            double result;
            if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
                return result;
            return double.NaN;
        }

        public void SaveReport(string fileName)
        {
        }
    }
}