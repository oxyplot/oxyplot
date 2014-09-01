// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow2.xaml.cs" company="OxyPlot">
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
    [Example("Demonstrates a Plot in a UserControl in a DataTemplate.")]
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            this.InitializeComponent();
            this.Models = new List<ViewModel> { new ViewModel { Title = "Plot1" }, new ViewModel { Title = "Plot2" } };
            this.DataContext = this;
        }

        public IList<ViewModel> Models { get; set; }
    }
}