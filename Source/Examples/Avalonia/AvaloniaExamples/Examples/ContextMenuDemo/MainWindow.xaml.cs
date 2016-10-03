// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.ContextMenuDemo
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using OxyPlot;
    using OxyPlot.Series;

    using AvaloniaExamples;

    using DelegateCommand = PropertyTools.Wpf.DelegateCommand;
    using ICommand = System.Windows.Input.ICommand;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Integrate with a ContextMenu")]
    public partial class MainWindow : Avalonia.Controls.Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var model = new PlotModel { Title = "ContextMenu" };
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 200));
            this.Model = model;

            this.ResetCommand = new DelegateCommand(() =>
            {
                this.Model.ResetAllAxes();
                this.Model.InvalidatePlot(false);
            });

            this.DataContext = this;
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }

        public ICommand ResetCommand { get; set; }

        public PlotModel Model { get; set; }
    }
}