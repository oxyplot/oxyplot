// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HeatMapDemo
{
    using System;
    using System.Reflection;
    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Demonstrates the HeatMapSeries.")]
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.Data = this.GenerateHeatMap();

            this.DataNaNValues = this.GenerateHeatMapNaNValues();
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public double[,] Data { get; private set; }

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

        /// <summary>
        /// Gets the data.
        /// </summary>
        public double[,] DataNaNValues { get; private set; }

        /// <summary>
        /// Generates the heat map data.
        /// </summary>
        /// <returns>
        /// The heat map data array.
        /// </returns>
        private double[,] GenerateHeatMapNaNValues()
        {
            const int Rows = 100;
            const int Cols = 100;
            var result = new double[Rows, Cols];
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    result[i, j] = (i==j)?double.NaN :Math.Sin(2 * Math.PI * i / Rows) * Math.Sin(2 * Math.PI * j / Cols);
                }
            }

            return result;
        }

    }
}