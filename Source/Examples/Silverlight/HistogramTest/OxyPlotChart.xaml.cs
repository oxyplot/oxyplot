// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPlotChart.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Visiblox.Charts.Examples
{
    using System.Windows.Controls;
    using OxyPlot.Silverlight;

    public partial class OxyPlotChart : UserControl
    {
        public OxyPlotChart()
        {
            this.InitializeComponent();
        }

        public PlotView Chart
        {
            get { return this.chart; }
        }
    }
}