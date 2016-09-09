namespace HeatMapMappingDemo
{
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for HeatMapMappingWindow.xaml
    /// </summary>
    [Example("Demonstrates the HeatMapSeries with mapped ItemsSource.")]
    public partial class HeatMapMappingWindow : Window
    {
        public PlotModel PlotModel { get; set; }

        public HeatMapMappingWindow()
        {
            this.PlotModel = new PlotModel();

            this.PlotModel.Series.Add(new HeatMapSeries()
                                          {

                                          });

            this.InitializeComponent();
        }
    }
}
