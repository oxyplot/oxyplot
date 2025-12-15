using System;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using WpfExamples;

namespace GumbelProbabilityAxisDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Plotting a Gumbel distribution with a Gumbel probability axis.")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var plot = new PlotModel { Title = "Gumbel Probability Plot", Subtitle = "Gumbel distributed data will plot as a straight line." };
            plot.Axes.Add(new GumbelProbabilityAxis() { Position = AxisPosition.Bottom, Title = "Exceedance Probability", StartPosition = 1, EndPosition = 0, Maximum = 0.99 });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Magnitude" });


            var s1 = new LineSeries
            {
                StrokeThickness = 0,
                MarkerSize = 3,
                MarkerStroke = OxyColors.ForestGreen,
                MarkerType = MarkerType.Plus
            };

            var dist = new GumbelDistribution(100, 15);
            int N = 1000;
            var rnd = new double[N];
            var pp = new double[N];
            var prng = new Random(12345);
            for (int i = 0; i < N; i++)
            {
                rnd[i] = dist.InverseCDF(prng.NextDouble());
                pp[i] = (i + 1.0) / (N + 1.0);
            }
            Array.Sort(rnd);
            Array.Reverse(rnd);

            for (int i = 0; i < N; i++)
            {
                s1.Points.Add(new DataPoint(pp[i], rnd[i]));
            }

            plot.Series.Add(s1);
            this.ScatterModel = plot;

        }

        public PlotModel ScatterModel { get; set; }
    }
}
