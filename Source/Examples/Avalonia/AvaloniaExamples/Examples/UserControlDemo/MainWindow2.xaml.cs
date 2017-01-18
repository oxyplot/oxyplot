// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow2.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.UserControlDemo
{
    using System.Collections.Generic;
    using System.Windows;

    using AvaloniaExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Demonstrates a Plot in a UserControl in a DataTemplate.")]
    public partial class MainWindow2 : Avalonia.Controls.Window
    {
        public MainWindow2()
        {
            this.InitializeComponent();
            this.DataContext = new { Models = new List<ViewModel> { new ViewModel { Title = "Plot1" }, new ViewModel { Title = "Plot2" } } };
            App.AttachDevTools(this);
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }

        public IList<ViewModel> Models { get; set; }
    }
}