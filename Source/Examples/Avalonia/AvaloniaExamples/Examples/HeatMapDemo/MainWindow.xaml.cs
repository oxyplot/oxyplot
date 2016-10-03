// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.HeatMapDemo
{
    using System;

    using AvaloniaExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Demonstrates the HeatMapSeries.")]
    public partial class MainWindow : Avalonia.Controls.Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new { Data = this.GenerateHeatMap() };
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Generates the heat map data.
        /// </summary>
        /// <returns>
        /// The heat map data array.
        /// </returns>
        private double[,] GenerateHeatMap()
        {
            const int Rows = 100;
            const int Cols = 100;
            var result = new double[Rows, Cols];
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    result[i, j] = Math.Sin(2 * Math.PI * i / Rows) * Math.Sin(2 * Math.PI * j / Cols);
                }
            }

            return result;
        }
    }
}