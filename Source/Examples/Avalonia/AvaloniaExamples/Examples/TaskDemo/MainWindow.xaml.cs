// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.TaskDemo
{
    using System.ComponentModel;
    using System.Windows;

    using AvaloniaExamples;
    using System;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Updating a LineSeries from a Task running on the UI thread synchronization context.")]
    public partial class MainWindow : Avalonia.Controls.Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();
            this.Closed += MainWindow_Closed;
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            ((MainViewModel)DataContext).Closing();
        }
    }
}