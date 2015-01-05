// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TwoColorAreaDemo
{
    using System.Windows;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example(null, "Plotting with TwoColorAreaSeries.")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}