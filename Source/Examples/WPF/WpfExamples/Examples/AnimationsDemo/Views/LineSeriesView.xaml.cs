// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeriesView.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    public partial class LineSeriesView
    {
        public LineSeriesView()
        {
            this.InitializeComponent();
            this.DataContext = new LineSeriesViewModel();
        }
    }
}