// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearBarView.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    public partial class LinearBarView
    {
        public LinearBarView()
        {
            this.InitializeComponent();
            this.DataContext = new LinearBarViewModel();
        }
    }
}