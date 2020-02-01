namespace ExampleLibrary
{
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("RectangleSeries"), Tags("Series")]
    public static class RectangleSeriesExamples
    {
        [Example("RectangleSeries")]
        [DocumentationExample("Series/RectangleSeries")]
        public static PlotModel FromItems()
        {
            const int NumberOfItems = 10;
            var model = new PlotModel { Title = "RectangleSeries" };

            // the RectangleSeries requires a color axis
            model.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Jet(100)
            });

            // create the series and add some rectangles with values
            var s = new RectangleSeries() { LabelFontSize = 12 };
            for (int i = NumberOfItems - 1; i >= 0; i--)
            {
                s.Items.Add(new RectangleItem(-i * 0.5, i * 0.5, i * i, i * (i + 3), i));
            }

            model.Series.Add(s);

            return model;
        }

        [Example("RectangleSeries from ItemsSource and Mapping")]
        public static PlotModel FromItemsSource()
        {
            const int NumberOfItems = 10;
            var model = new PlotModel { Title = "RectangleSeries" };

            // the RectangleSeries requires a color axis
            model.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Jet(100)
            });

            // create the data
            var items = new List<MyItem>();
            for (int i = 0; i < NumberOfItems; i++)
            {
                items.Add(new MyItem { X = i, Value = i });
            }

            model.Series.Add(new RectangleSeries
            {
                ItemsSource = items,
                Mapping = x =>
                {
                    var r = (MyItem)x;
                    return new RectangleItem(r.X, r.X * 2, r.X, r.X * 2, r.Value);
                }
            });

            return model;
        }

        public class MyItem
        {
            public double X { get; set; }
            public double Value { get; set; }
        }
    }
}
