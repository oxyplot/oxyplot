// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RefreshDemo
{
    using System.Windows;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Demonstrates invalidating/refreshing the plot.")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RefreshMethod1_Click(object sender, RoutedEventArgs e)
        {
            new Window1().Show();
        }

        private void RefreshMethod2_Click(object sender, RoutedEventArgs e)
        {
            new Window2().Show();
        }

        private void RefreshMethod3_Click(object sender, RoutedEventArgs e)
        {
            new Window3().Show();
        }

        private void RefreshMethod4_Click(object sender, RoutedEventArgs e)
        {
            new Window4().Show();
        }

        private void RefreshMethod6_Click(object sender, RoutedEventArgs e)
        {
            new Window6().Show();
        }
    }
}