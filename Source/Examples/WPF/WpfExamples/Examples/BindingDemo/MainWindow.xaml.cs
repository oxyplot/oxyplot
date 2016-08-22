// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BindingDemo
{
    using System.Windows;

    using global::BindingDemo;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Demonstrates data binding.")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            var vm = new MainViewModel();
            this.DataContext = vm;
        }
    }
}