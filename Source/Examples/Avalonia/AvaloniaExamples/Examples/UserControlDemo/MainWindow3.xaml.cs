// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow3.xaml.cs" company="OxyPlot">
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
    [Example("Demonstrates a Plot in a DataTemplate.")]
    public partial class MainWindow3 : Avalonia.Controls.Window
    {
        public MainWindow3()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel { Title = "Plot1" };
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}