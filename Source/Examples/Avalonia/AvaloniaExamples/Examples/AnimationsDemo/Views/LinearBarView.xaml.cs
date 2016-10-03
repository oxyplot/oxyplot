// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearBarView.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia.Controls;

namespace AvaloniaExamples.Examples.AnimationsDemo.Views
{
    public partial class LinearBarView : UserControl
    {
        public LinearBarView()
        {
            this.InitializeComponent();
            this.DataContext = new LinearBarViewModel();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}