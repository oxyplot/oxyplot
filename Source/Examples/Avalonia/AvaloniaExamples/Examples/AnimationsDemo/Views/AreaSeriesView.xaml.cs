// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaSeriesView.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia.Controls;

namespace AvaloniaExamples.Examples.AnimationsDemo.Views
{
    public partial class AreaSeriesView : UserControl
    {
        public AreaSeriesView()
        {
            this.InitializeComponent();
            this.DataContext = new AreaSeriesViewModel();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}