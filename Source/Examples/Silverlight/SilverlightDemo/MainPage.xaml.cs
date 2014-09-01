// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SilverlightDemo
{
    using System;
    using System.Windows.Controls;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            this.InitializeComponent();

            var tmp = new PlotModel { Title = "Silverlight demo" };
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
            tmp.Series.Add(new FunctionSeries(x => Math.Sin(x) / x, -10, 10, 0.03, "sin(x)/x"));
            plot1.Model = tmp;
        }
    }
}