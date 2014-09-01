// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ContextMenuDemo
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using OxyPlot;
    using OxyPlot.Series;

    using WpfExamples;

    using DelegateCommand = PropertyTools.Wpf.DelegateCommand;
    using ICommand = System.Windows.Input.ICommand;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Integrate with a ContextMenu")]
    public partial class MainWindow : Window
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

        public ICommand ResetCommand { get; set; }

        public PlotModel Model { get; set; }
    }
}