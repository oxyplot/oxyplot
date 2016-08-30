// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaSeriesView.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    public partial class AreaSeriesView
    {
        public AreaSeriesView()
        {
            this.InitializeComponent();
            this.DataContext = new AreaSeriesViewModel();
        }
    }
}