using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HeatMapDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.Data = this.GenerateHeatMap();
        }

        /// <summary>
        /// The generate heat map data.
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
        public double[,] Data { get; private set; }
    }
}
