// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using OxyPlot;

namespace ScatterDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel ScatterModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var tmp = new PlotModel("Scatter plot","Barnsley fern (IFS)");
            var s1 = new LineSeries
                         {
                             StrokeThickness = 0,
                             MarkerSize = 3,
                             // MarkerFill = OxyColors.Blue,
                             MarkerStroke=OxyColors.ForestGreen,
                             MarkerType = MarkerType.Plus
                         };

            foreach (var pt in Fern.Generate(2000))
                s1.Points.Add(new DataPoint(pt.X, -pt.Y));

            //var r = new Random();
            //for (int i = 0; i < 1000; i++)
            //    s1.Points.Add(new DataPoint(r.NextDouble(), r.NextDouble()));

            tmp.Series.Add(s1);
            ScatterModel = tmp;
        }
    }
}