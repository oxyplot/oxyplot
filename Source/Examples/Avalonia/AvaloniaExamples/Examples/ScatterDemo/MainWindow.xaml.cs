// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.ScatterDemo
{
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Series;

    using AvaloniaExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Plotting a barnsley fern with a scatter series.")]
    public partial class MainWindow : Avalonia.Controls.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var tmp = new PlotModel { Title = "Scatter plot", Subtitle = "Barnsley fern (IFS)" };
            var s1 = new LineSeries
                         {
                             StrokeThickness = 0,
                             MarkerSize = 3,
                             MarkerStroke = OxyColors.ForestGreen,
                             MarkerType = MarkerType.Plus
                         };

            foreach (var pt in Fern.Generate(2000))
            {
                s1.Points.Add(new DataPoint(pt.X, -pt.Y));
            }

            tmp.Series.Add(s1);
            this.ScatterModel = tmp;
            DataContext = new { ScatterModel };
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }

        public PlotModel ScatterModel { get; set; }
    }
}