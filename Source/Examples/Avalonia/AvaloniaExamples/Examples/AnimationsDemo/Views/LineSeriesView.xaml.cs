// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeriesView.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia.Controls;

namespace AvaloniaExamples.Examples.AnimationsDemo.Views
{
    public partial class LineSeriesView : UserControl
    {
        public LineSeriesView()
        {
            this.InitializeComponent();
            this.DataContext = new LineSeriesViewModel();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}