namespace HeatMapMappingDemo
{
    using System;
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Series;

    using WpfExamples;
    using System.Collections.Generic;

    public class HeatMapItem
    {
        public double X1;

        public double X2;

        public double Y1;

        public double Y2;

        public double Value;

        public HeatMapItem(int seed)
        {
            this.X1 = seed;
            this.X2 = 2 * seed;
            this.Y1 = seed;
            this.Y2 = 2 * seed;
            this.Value = seed;
        }
    }

    /// <summary>
    /// Interaction logic for HeatMapMappingWindow.xaml
    /// </summary>
    [Example("Demonstrates the HeatMapSeries with mapped ItemsSource.")]
    public partial class HeatMapMappingWindow : Window
    {
        public const int NumberOfItems = 10;

        public PlotModel PlotModel { get; set; }

        public IList<HeatMapItem> Items;

        public HeatMapMappingWindow()
        {
            // generate some dummy items
            this.Items = new List<HeatMapItem>();
            for (int i = 0; i < NumberOfItems; i++)
            {
                this.Items.Add(new HeatMapItem(i));
            }

            this.PlotModel = new PlotModel();

            this.PlotModel.Series.Add(new HeatMapSeries()
            {
                ItemsSource = this.Items,
                Mapping = this.HeatMapItemToDataRectMapping
            });

            this.InitializeComponent();
        }

        private DataRect HeatMapItemToDataRectMapping(object obj)
        {
            HeatMapItem heatMapItem = (HeatMapItem)obj;

            return new DataRect(
                heatMapItem.X1,
                heatMapItem.X2,
                heatMapItem.Y1,
                heatMapItem.Y2,
                heatMapItem.Value);
        }
    }
}
