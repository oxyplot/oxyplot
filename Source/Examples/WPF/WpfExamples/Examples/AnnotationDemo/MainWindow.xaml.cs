// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AnnotationDemo
{
    using System.Collections.Generic;
    using System.Windows;

    using OxyPlot;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example(null, "Shows different types of annotations.")]
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.Points = new List<DataPoint>
                {
                   new DataPoint(10, 10), new DataPoint(80, 30), new DataPoint(60, 70)
                };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        public IList<DataPoint> Points { get; private set; }
    }
}