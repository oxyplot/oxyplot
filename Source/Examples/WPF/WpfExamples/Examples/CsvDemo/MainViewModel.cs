// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
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

namespace CsvDemo
{
    using System.Globalization;
    using System.IO;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using WpfExamples;

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