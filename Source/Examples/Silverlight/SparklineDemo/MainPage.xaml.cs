// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
                                PlotAreaBorderThickness = new OxyThickness(0)
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
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, IsAxisVisible = false });
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