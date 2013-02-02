// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
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
using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;

namespace SparklineDemo
{
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public partial class MainPage
    {
        public List<Trend> Trends { get; set; }
        public MainPage()
        {
            InitializeComponent();
            Trends = new List<Trend>
                         {
                             new Trend("Revenue"),
                             new Trend("Profit"),
                             new Trend("Order size"),
                             new Trend("On Time Delivery"),
                             new Trend("New Customers"),
                             new Trend("Customer satisfaction"),
                             new Trend("Market share")
                         };
            DataContext = this;
        }
    }
    public class Trend
    {
        public string Name { get; set; }
        public List<TrendData> Data { get; set; }
        public double LastValue { get { return Data.Last().Value; } }
        public PlotModel PlotModel { get { return CreatePlotModel(); } }

        private PlotModel CreatePlotModel()
        {
            var model = new PlotModel
                            {
                                PlotMargins = new OxyThickness(0),
                                Padding = new OxyThickness(0),
                                PlotAreaBorderThickness = 0,
                                AutoAdjustPlotMargins = false
                            };
            var ls = new LineSeries
                         {
                             Color = OxyColors.Black,
                             ItemsSource = Data,
                             DataFieldX = "Time",
                             DataFieldY = "Value"
                         };
            model.Series.Add(ls);
            model.Axes.Add(new DateTimeAxis { IsAxisVisible = false });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { IsAxisVisible = false });
            return model;
        }

        static readonly Random RandomGenerator = new Random();

        public Trend(string name)
        {
            this.Name = name;
            Data = new List<TrendData>();
            double y = RandomGenerator.Next(250);
            for (int i = -48; i <= 0; i++)
            {
                y += RandomGenerator.NextDouble() - 0.2;
                Data.Add(new TrendData { Time = DateTime.Now.AddHours(i), Value = y });
            }

        }
    }

    public class TrendData
    {
        public DateTime Time { get; set; }
        public double Value { get; set; }
    }
}