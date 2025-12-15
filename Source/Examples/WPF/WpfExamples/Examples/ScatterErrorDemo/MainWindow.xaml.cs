using System;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using WpfExamples;

namespace ScatterErrorDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Plotting a scatter with variable length error bars.")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var plot = new PlotModel { Title = "Scatter with Error Bars", Subtitle = "The error bars have variable lengths." };
            plot.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "X Axis" });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y Axis" });


            var s1 = new ScatterErrorSeries()
            {
                MarkerSize = 3,
                MarkerStroke = OxyColors.ForestGreen,
                MarkerType = MarkerType.Square
            };

            var xDist = new NormalDistribution(100, 15);
            var yDist = new NormalDistribution(150, 25);
            int N = 10;
            var pt = new Point[N];
            var xlo = new double[N];
            var xhi = new double[N];
            var ylo = new double[N];
            var yhi = new double[N];
            var prng = new Random(12345);
            for (int i = 0; i < N; i++)
            {
                pt[i] = new Point(xDist.InverseCDF(prng.NextDouble()), yDist.InverseCDF(prng.NextDouble()));
                xlo[i] = pt[i].X - pt[i].X * prng.NextDouble();
                xhi[i] = pt[i].X + pt[i].X * prng.NextDouble();
                ylo[i] = pt[i].Y - pt[i].Y * prng.NextDouble();
                yhi[i] = pt[i].Y + pt[i].Y * prng.NextDouble();
            }


            for (int i = 0; i < N; i++)
            {
                s1.Points.Add(new ScatterErrorPoint(pt[i].X, pt[i].Y, lowerErrorX: xlo[i], upperErrorX: xhi[i], lowerErrorY: ylo[i], upperErrorY: yhi[i]));
            }

            plot.Series.Add(s1);
            this.ScatterModel = plot;

        }

        public PlotModel ScatterModel { get; set; }
    }
}
