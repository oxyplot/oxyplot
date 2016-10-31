// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Windows;
using AvaloniaExamples;

namespace AvaloniaExamples.Examples.ToolTipDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Shows OxyPlot tool tips on the title which can be set via the model and via xaml.")]
    public partial class MainWindow : Avalonia.Controls.Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = new MainViewModel();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}