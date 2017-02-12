namespace RectangleSeriesDemo
{
    using OxyPlot;
    using OxyPlot.Series;

    using WpfExamples;
    using System.Collections.Generic;

    using OxyPlot.Axes;

    public class RectangleWithValue
    {
        public double X1;

        public double X2;

        public double Y1;

        public double Y2;

        public double Value;

        public RectangleWithValue(int seed)
        {
            this.X1 = seed;
            this.X2 = 2 * seed;
            this.Y1 = seed;
            this.Y2 = 2 * seed;
            this.Value = seed;
        }
    }

    /// <summary>
    /// Interaction logic for RectangleSeries.xaml
    /// </summary>
    [Example("Demonstrates the RectangleSeries with mapped ItemsSource.")]
    public partial class MainWindow
    {
        public const int NumberOfItems = 10;

        public MainWindow()
        {
            // generate some dummy items
            this.Items = new List<RectangleWithValue>();
            for (int i = 0; i < NumberOfItems; i++)
            {
                this.Items.Add(new RectangleWithValue(i));
            }

            this.PlotModel = new PlotModel();

            this.PlotModel.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Jet(100)
            });

            this.PlotModel.Series.Add(new RectangleSeries
            {
                ItemsSource = this.Items,
                Mapping = obj =>
                {
                    var rectangleWithValue = (RectangleWithValue)obj;

                    return new RectangleItem(
                        rectangleWithValue.X1,
                        rectangleWithValue.X2,
                        rectangleWithValue.Y1,
                        rectangleWithValue.Y2,
                        rectangleWithValue.Value);
                }
            });

            this.InitializeComponent();
        }

        public PlotModel PlotModel { get; }

        public IList<RectangleWithValue> Items { get; }

    }
}
