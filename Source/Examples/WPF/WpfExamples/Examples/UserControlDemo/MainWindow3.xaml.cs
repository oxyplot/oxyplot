// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow3.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UserControlDemo
{
    using System.Collections.Generic;
    using System.Windows;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Demonstrates a Plot in a DataTemplate.")]
    public partial class MainWindow3 : Window
    {
        public MainWindow3()
        {
            this.InitializeComponent();
            this.Models = new List<ViewModel> { new ViewModel { Title = "Plot1" }, new ViewModel { Title = "Plot2" } };
            this.DataContext = this;
        }

        public IList<ViewModel> Models { get; private set; }
    }
}