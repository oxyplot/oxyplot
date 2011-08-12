using System.Collections.ObjectModel;
using System.Windows;
using OxyPlot;

namespace BarSeriesDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var items = new Collection<Item>();
            items.Add(new Item { Label = "Apples", Value1 = 37, Value2 = 12, Value3 = 19 });
            items.Add(new Item { Label = "Pears", Value1 = 7, Value2 = 21, Value3 = 9 });
            items.Add(new Item { Label = "Bananas", Value1 = 23, Value2 = 2, Value3 = 29 });

            var tmp = new PlotModel("Bar series");
            var xaxis = new CategoryAxis { ItemsSource = items, LabelField = "Label" };
            xaxis.UpdateLabels();
            tmp.Axes.Add(xaxis);

            var bars = new BarSeries() { ItemsSource = items, ValueField = "Value1"};
            tmp.Series.Add(bars);

            Model1 = tmp;
            DataContext = this;
        }

        public PlotModel Model1 { get; set; }
    }

    public class Item
    {
        public string Label { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public double Value3 { get; set; }
    }
}