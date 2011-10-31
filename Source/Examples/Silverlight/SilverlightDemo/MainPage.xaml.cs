// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows.Controls;
using OxyPlot;

namespace SilverlightDemo
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            var tmp = new PlotModel("Silverlight demo");
            tmp.Axes.Add(new LinearAxis(AxisPosition.Left) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
            tmp.Axes.Add(new LinearAxis(AxisPosition.Bottom) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
            tmp.Series.Add(new FunctionSeries(x => Math.Sin(x) / x, -10, 10, 0.03, "sin(x)/x"));
            plot1.Model = tmp;
        }
    }
}