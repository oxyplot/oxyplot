// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarAndColumnSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public abstract class BarAndColumnSeriesExamples<TSeries, TItem>
        where TSeries : BarSeriesBase<TItem>, new()
        where TItem : BarItemBase, new()
    {
        [Example("Simple model")]
        public static PlotModel SimpleModel()
        {
            return CreateSimpleModel(false, "Simple model");
        }

        [Example("With labels")]
        public static PlotModel WithLabels()
        {
            var model = CreateSimpleModel(false, "With labels");
            var s0 = model.Series[0] as BarSeriesBase;
            var s1 = model.Series[1] as BarSeriesBase;
            s0.LabelFormatString = "{0}";
            s1.LabelFormatString = "{0:0.00}";
            s1.LabelPlacement = LabelPlacement.Middle;
            s1.TextColor = OxyColors.White;
            return model;
        }

        [Example("With labels (negative values)")]
        public static PlotModel WithLabelsNegativeValues()
        {
            var model = new PlotModel
            {
                Title = "With labels (negative values)",
            };

            var s1 = new TSeries { LabelFormatString = "{0}" };
            s1.Items.Add(new TItem { Value = 25 });
            s1.Items.Add(new TItem { Value = 137 });
            s1.Items.Add(new TItem { Value = -18 });
            s1.Items.Add(new TItem { Value = 40 });

            var s2 = new TSeries { LabelFormatString = "{0:0.00}" };
            s2.Items.Add(new TItem { Value = 12 });
            s2.Items.Add(new TItem { Value = 14 });
            s2.Items.Add(new TItem { Value = 120 });
            s2.Items.Add(new TItem { Value = -26 });

            var categoryAxis = new CategoryAxis { Position = CategoryAxisPosition() };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            var valueAxis = new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0.06, MaximumPadding = 0.06, ExtraGridlines = new[] { 0d } };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        [Example("Stacked")]
        public static PlotModel StackedSeries()
        {
            return CreateSimpleModel(true, "Simple stacked model");
        }

        [Example("Empty series")]
        public static PlotModel EmptySeries()
        {
            var model = new PlotModel { Title = "Empty series" };

            var s1 = new TSeries { Title = "Series 1" };
            var s2 = new TSeries { Title = "Series 2" };
            var categoryAxis = new CategoryAxis { Position = CategoryAxisPosition() };
            var valueAxis = new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        [Example("No category axis defined")]
        public static PlotModel NoCategoryAxisDefined()
        {
            var model = new PlotModel { Title = "No category axis defined" };

            var s1 = new TSeries { Title = "Series 1", ItemsSource = new[] { new TItem { Value = 25 }, new TItem { Value = 137 } } };
            var s2 = new TSeries { Title = "Series 2", ItemsSource = new[] { new TItem { Value = 52 }, new TItem { Value = 317 } } };
            var valueAxis = new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(valueAxis);
            return model;
        }

        [Example("Binding to ItemsSource")]
        public static PlotModel BindingItemsSource()
        {
            var items = new Collection<Item>
                            {
                                new Item { Label = "Apples", Value1 = 37, Value2 = 12, Value3 = 19 },
                                new Item { Label = "Pears", Value1 = 7, Value2 = 21, Value3 = 9 },
                                new Item { Label = "Bananas", Value1 = 23, Value2 = 2, Value3 = 29 }
                            };

            var plotModel1 = new PlotModel { LegendPlacement = LegendPlacement.Outside, Title = "Binding to ItemsSource" };
            var categoryAxis1 = new CategoryAxis { Position = CategoryAxisPosition(), LabelField = "Label", ItemsSource = items, MajorStep = 1, MinorStep = 1 };
            plotModel1.Axes.Add(categoryAxis1);
            var linearAxis1 = new LinearAxis { Position = ValueAxisPosition(), AbsoluteMinimum = 0, MinimumPadding = 0 };
            plotModel1.Axes.Add(linearAxis1);
            var series1 = new TSeries
            {
                FillColor = OxyColor.FromArgb(255, 78, 154, 6),
                ValueField = "Value1",
                Title = "2009",
                ItemsSource = items
            };
            plotModel1.Series.Add(series1);
            var series2 = new TSeries
            {
                FillColor = OxyColor.FromArgb(255, 200, 141, 0),
                ValueField = "Value2",
                Title = "2010",
                ItemsSource = items
            };
            plotModel1.Series.Add(series2);
            var series3 = new TSeries
            {
                FillColor = OxyColor.FromArgb(255, 204, 0, 0),
                ValueField = "Value3",
                Title = "2011",
                ItemsSource = items
            };
            plotModel1.Series.Add(series3);
            return plotModel1;
        }

        [Example("Binding to ItemsSource (array)")]
        public static PlotModel BindingToItemsSourceArray()
        {
            var model = new PlotModel { Title = "Binding to ItemsSource", Subtitle = "The items are defined by an array of BarItem/ColumnItem" };
            model.Series.Add(new TSeries { Title = "Series 1", ItemsSource = new[] { new TItem { Value = 25 }, new TItem { Value = 137 } } });
            model.Series.Add(new TSeries { Title = "Series 2", ItemsSource = new[] { new TItem { Value = 52 }, new TItem { Value = 317 } } });
            model.Axes.Add(new CategoryAxis { Position = CategoryAxisPosition() });
            model.Axes.Add(new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0 });
            return model;
        }

        [Example("Binding to ItemsSource (list)")]
        public static PlotModel BindingToItemsSourceListT()
        {
            var model = new PlotModel { Title = "Binding to ItemsSource", Subtitle = "The items are defined by a List of BarItem/ColumnItem" };
            model.Series.Add(new TSeries { Title = "Series 1", ItemsSource = new List<TItem>(new[] { new TItem { Value = 25 }, new TItem { Value = 137 } }) });
            model.Series.Add(new TSeries { Title = "Series 2", ItemsSource = new List<TItem>(new[] { new TItem { Value = 52 }, new TItem { Value = 317 } }) });
            model.Axes.Add(new CategoryAxis { Position = CategoryAxisPosition() });
            model.Axes.Add(new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0 });
            return model;
        }

        [Example("Binding to ItemsSource (reflection)")]
        public static PlotModel BindingToItemsSourceReflection()
        {
            var model = new PlotModel { Title = "Binding to ItemsSource", Subtitle = "Reflect by 'ValueField'" };
            model.Series.Add(new TSeries { Title = "Series 1", ValueField = "Value1", ItemsSource = new[] { new Item { Value1 = 25 }, new Item { Value1 = 137 } } });
            model.Series.Add(new TSeries { Title = "Series 2", ValueField = "Value1", ItemsSource = new[] { new Item { Value1 = 52 }, new Item { Value1 = 317 } } });
            model.Axes.Add(new CategoryAxis { Position = CategoryAxisPosition() });
            model.Axes.Add(new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0 });
            return model;
        }

        [Example("Defined by Items")]
        public static PlotModel DefinedByItems()
        {
            var model = new PlotModel { Title = "Defined by Items", Subtitle = "The items are added to the `Items` property." };

            var s1 = new TSeries { Title = "Series 1" };
            s1.Items.AddRange(new[] { new TItem { Value = 25 }, new TItem { Value = 137 } });
            var s2 = new TSeries { Title = "Series 2" };
            s2.Items.AddRange(new[] { new TItem { Value = 52 }, new TItem { Value = 317 } });
            model.Series.Add(s1);
            model.Series.Add(s2);

            model.Axes.Add(new CategoryAxis { Position = CategoryAxisPosition() });
            model.Axes.Add(new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0 });
            return model;
        }

        [Example("Empty category axis")]
        public static PlotModel EmptyCategoryAxis()
        {
            var model = new PlotModel { Title = "Empty category axis" };

            var s1 = new TSeries { Title = "Series 1" };
            s1.Items.Add(new TItem { Value = 25 });
            s1.Items.Add(new TItem { Value = 137 });
            s1.Items.Add(new TItem { Value = 18 });
            s1.Items.Add(new TItem { Value = 40 });
            var s2 = new TSeries { Title = "Series 2" };
            s2.Items.Add(new TItem { Value = -12 });
            s2.Items.Add(new TItem { Value = -14 });
            s2.Items.Add(new TItem { Value = -120 });
            s2.Items.Add(new TItem { Value = -26 });
            var categoryAxis = new CategoryAxis { Position = CategoryAxisPosition() };
            var valueAxis = new LinearAxis
            {
                Position = ValueAxisPosition(),
                MinimumPadding = 0.06,
                MaximumPadding = 0.06,
                ExtraGridlines = new[] { 0.0 },
                ExtraGridlineStyle = LineStyle.Solid,
                ExtraGridlineColor = OxyColors.Black,
                ExtraGridlineThickness = 1
            };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        [Example("With negative values")]
        public static PlotModel WithNegativeValue()
        {
            return CreateModelWithNegativeValues(false, "With negative values");
        }

        [Example("Stacked with negative values")]
        public static PlotModel StackedWithNegativeValue()
        {
            return CreateModelWithNegativeValues(true, "Stacked with negative values");
        }

        [Example("Mixed with LineSeries")]
        public static PlotModel MixedWithLineSeries()
        {
            var model = CreateSimpleModel(false, "Mixed with LineSeries");
            model.Title = "Mixed with LineSeries";
            var s1 = new LineSeries { Title = "LineSeries 1" };
            if (typeof(TSeries) == typeof(ColumnSeries))
            {
                s1.Points.Add(new DataPoint(0, 25));
                s1.Points.Add(new DataPoint(1, 137));
                s1.Points.Add(new DataPoint(2, 18));
                s1.Points.Add(new DataPoint(3, 40));
            }
            else
            {
                s1.Points.Add(new DataPoint(25, 0));
                s1.Points.Add(new DataPoint(137, 1));
                s1.Points.Add(new DataPoint(18, 2));
                s1.Points.Add(new DataPoint(40, 3));
            }

            model.Series.Add(s1);
            return model;
        }

        [Example("No axes defined")]
        public static PlotModel NoAxes()
        {
            var model = CreateSimpleModel(false, "No axes defined");
            model.Axes.Clear(); // default axes will be generated
            return model;
        }

        [Example("Stacked and no axes defined")]
        public static PlotModel StackedNoAxes()
        {
            var model = CreateSimpleModel(true, "Stacked and no axes defined");
            model.Axes.Clear(); // default axes will be generated
            return model;
        }

        [Example("Logarithmic axis")]
        public static PlotModel LogAxis()
        {
            var model = new PlotModel
            {
                Title = "Logarithmic axis",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            var s1 = new TSeries { Title = "Series 1", BaseValue = 0.1, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Items.Add(new TItem { Value = 25 });
            s1.Items.Add(new TItem { Value = 37 });
            s1.Items.Add(new TItem { Value = 18 });
            s1.Items.Add(new TItem { Value = 40 });

            var categoryAxis = new CategoryAxis { Position = CategoryAxisPosition() };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            model.Series.Add(s1);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(new LogarithmicAxis { Position = ValueAxisPosition(), Minimum = 0.1, MinimumPadding = 0, AbsoluteMinimum = 0 });
            return model;
        }

        [Example("Logarithmic axis (not stacked)")]
        public static PlotModel LogAxis2()
        {
            var model = new PlotModel { Title = "Logarithmic axis", LegendPlacement = LegendPlacement.Outside };

            var items = new Collection<Item>
                            {
                                new Item {Label = "Apples", Value1 = 37, Value2 = 12, Value3 = 19},
                                new Item {Label = "Pears", Value1 = 7, Value2 = 21, Value3 = 9},
                                new Item {Label = "Bananas", Value1 = 23, Value2 = 2, Value3 = 29}
                            };

            model.Series.Add(new TSeries { Title = "2009", ItemsSource = items, ValueField = "Value1" });
            model.Series.Add(new TSeries { Title = "2010", ItemsSource = items, ValueField = "Value2" });
            model.Series.Add(new TSeries { Title = "2011", ItemsSource = items, ValueField = "Value3" });

            model.Axes.Add(new CategoryAxis { Position = CategoryAxisPosition(), ItemsSource = items, LabelField = "Label" });
            model.Axes.Add(new LogarithmicAxis { Position = ValueAxisPosition(), Minimum = 1 });
            return model;
        }

        [Example("Logarithmic axis (stacked series)")]
        public static PlotModel LogAxis3()
        {
            var model = LogAxis2();
            foreach (var s in model.Series.OfType<TSeries>())
            {
                s.IsStacked = true;
            }

            return model;
        }

        private static PlotModel CreateSimpleModel(bool stacked, string title)
        {
            var model = new PlotModel
            {
                Title = title,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            var s1 = new TSeries { Title = "Series 1", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Items.Add(new TItem { Value = 25 });
            s1.Items.Add(new TItem { Value = 137 });
            s1.Items.Add(new TItem { Value = 18 });
            s1.Items.Add(new TItem { Value = 40 });

            var s2 = new TSeries { Title = "Series 2", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Items.Add(new TItem { Value = 12 });
            s2.Items.Add(new TItem { Value = 14 });
            s2.Items.Add(new TItem { Value = 120 });
            s2.Items.Add(new TItem { Value = 26 });

            var categoryAxis = new CategoryAxis { Position = CategoryAxisPosition() };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            var valueAxis = new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        private static PlotModel CreateModelWithNegativeValues(bool stacked, string title)
        {
            var model = new PlotModel
            {
                Title = title,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            var s1 = new TSeries { Title = "Series 1", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Items.Add(new TItem { Value = 25 });
            s1.Items.Add(new TItem { Value = 137 });
            s1.Items.Add(new TItem { Value = 18 });
            s1.Items.Add(new TItem { Value = 40 });

            var s2 = new TSeries { Title = "Series 2", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Items.Add(new TItem { Value = -12 });
            s2.Items.Add(new TItem { Value = -14 });
            s2.Items.Add(new TItem { Value = -120 });
            s2.Items.Add(new TItem { Value = -26 });

            var s3 = new TSeries { Title = "Series 3", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s3.Items.Add(new TItem { Value = 21 });
            s3.Items.Add(new TItem { Value = 8 });
            s3.Items.Add(new TItem { Value = 48 });
            s3.Items.Add(new TItem { Value = 3 });

            var s4 = new TSeries { Title = "Series 4", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s4.Items.Add(new TItem { Value = -8 });
            s4.Items.Add(new TItem { Value = -21 });
            s4.Items.Add(new TItem { Value = -3 });
            s4.Items.Add(new TItem { Value = -48 });

            var categoryAxis = new CategoryAxis { Position = CategoryAxisPosition() };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");

            var valueAxis = new LinearAxis
            {
                Position = ValueAxisPosition(),
                MinimumPadding = 0.06,
                MaximumPadding = 0.06,
                ExtraGridlines = new[] { 0.0 },
                ExtraGridlineStyle = LineStyle.Solid,
                ExtraGridlineColor = OxyColors.Black,
                ExtraGridlineThickness = 1
            };

            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Series.Add(s3);
            model.Series.Add(s4);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        public class Item
        {
            public string Label { get; set; }
            public double Value1 { get; set; }
            public double Value2 { get; set; }
            public double Value3 { get; set; }
        }

        public class HistogramBin
        {
            public string Label { get; set; }
            public double Value { get; set; }
        }

        [Example("Histogram (bins=5)")]
        public static PlotModel Histogram3()
        {
            return CreateHistogram(100000, 5);
        }

        [Example("Histogram (bins=20)")]
        public static PlotModel Histogram20()
        {
            return CreateHistogram(100000, 20);
        }

        [Example("Histogram (bins=200)")]
        public static PlotModel Histogram200()
        {
            return CreateHistogram(100000, 200);
        }

        public static PlotModel CreateHistogram(int n, int binCount)
        {
            var bins = new HistogramBin[binCount];
            for (int i = 0; i < bins.Length; i++)
            {
                bins[i] = new HistogramBin { Label = i.ToString() };
            }

            var r = new Random(31);
            for (int i = 0; i < n; i++)
            {
                int value = r.Next(binCount);
                bins[value].Value++;
            }

            var temp = new PlotModel { Title = string.Format("Histogram (bins={0}, n={1})", binCount, n), Subtitle = "Pseudorandom numbers" };
            var series1 = new TSeries { ItemsSource = bins, ValueField = "Value" };
            if (binCount < 100)
            {
                series1.LabelFormatString = "{0}";
            }

            temp.Series.Add(series1);

            temp.Axes.Add(new CategoryAxis { Position = CategoryAxisPosition(), ItemsSource = bins, LabelField = "Label", GapWidth = 0 });
            temp.Axes.Add(new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0, MaximumPadding = 0.1, AbsoluteMinimum = 0 });

            return temp;
        }

        [Example("Histogram (standard normal distribution)")]
        public static PlotModel HistogramStandardNormalDistribution()
        {
            return CreateNormalDistributionHistogram(100000, 2000);
        }

        public static PlotModel CreateNormalDistributionHistogram(int n, int binCount)
        {
            var bins = new HistogramBin[binCount];
            double min = -10;
            double max = 10;
            for (int i = 0; i < bins.Length; i++)
            {
                var v = min + (max - min) * i / (bins.Length - 1);
                bins[i] = new HistogramBin { Label = v.ToString("0.0") };
            }

            var r = new Random(31);
            for (int i = 0; i < n; i++)
            {
                // http://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
                var u1 = r.NextDouble();
                var u2 = r.NextDouble();
                var v = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);

                int bin = (int)Math.Round((v - min) / (max - min) * (bins.Length - 1));
                if (bin >= 0 && bin < bins.Length)
                {
                    bins[bin].Value++;
                }
            }

            var temp = new PlotModel { Title = string.Format("Histogram (bins={0}, n={1})", binCount, n), Subtitle = "Standard normal distribution by Box-Muller transform" };
            var series1 = new TSeries { ItemsSource = bins, ValueField = "Value" };
            temp.Series.Add(series1);

            var categoryAxis = new CategoryAxis
            {
                Position = CategoryAxisPosition(),
                GapWidth = 0
            };

            categoryAxis.Labels.AddRange(bins.Select(b => b.Label));
            categoryAxis.IsAxisVisible = false;
            temp.Axes.Add(categoryAxis);

            // todo: link category and linear axis
            temp.Axes.Add(new LinearAxis { Position = CategoryAxisPosition(), Minimum = min, Maximum = max });

            temp.Axes.Add(new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0, AbsoluteMinimum = 0 });

            return temp;
        }

        private static AxisPosition CategoryAxisPosition()
        {
            if (typeof(TSeries) == typeof(ColumnSeries))
            {
                return AxisPosition.Bottom;
            }

            return AxisPosition.Left;
        }

        private static AxisPosition ValueAxisPosition()
        {
            if (typeof(TSeries) == typeof(ColumnSeries))
            {
                return AxisPosition.Left;
            }

            return AxisPosition.Bottom;
        }

        [Example("Different colors within the same series")]
        public static PlotModel DifferentColors()
        {
            var model = new PlotModel { Title = "Different colors within the same series" };
            var series = new TSeries { Title = "Series 1" };
            series.Items.Add(new TItem { Value = 1, Color = OxyColors.Red });
            series.Items.Add(new TItem { Value = 2, Color = OxyColors.Green });
            series.Items.Add(new TItem { Value = 1, Color = OxyColors.Blue });

            var categoryAxis = new CategoryAxis
            {
                Title = "Category",
                Position = CategoryAxisPosition()
            };
            categoryAxis.Labels.AddRange(new[] { "A", "B", "C" });
            model.Axes.Add(categoryAxis);

            model.Series.Add(series);
            return model;
        }

        [Example("Different stacking groups")]
        public static PlotModel StackingGroups()
        {
            var model = new PlotModel { Title = "Stacking groups" };
            var series = new TSeries { Title = "Series 1", StackGroup = "1", IsStacked = true };
            series.Items.Add(new TItem { Value = 1 });
            series.Items.Add(new TItem { Value = 2 });
            model.Series.Add(series);

            var series2 = new TSeries { Title = "Series 2", StackGroup = "2", IsStacked = true };
            series2.Items.Add(new TItem { Value = 2 });
            series2.Items.Add(new TItem { Value = 1 });
            model.Series.Add(series2);

            var series3 = new TSeries { Title = "Series 3", StackGroup = "1", IsStacked = true };
            series3.Items.Add(new TItem { Value = 3 });
            series3.Items.Add(new TItem { Value = 1 });
            model.Series.Add(series3);

            var categoryAxis = new CategoryAxis
            {
                Title = "Category",
                Position = CategoryAxisPosition()
            };
            categoryAxis.Labels.AddRange(new[] { "A", "B" });
            model.Axes.Add(categoryAxis);
            return model;
        }

        [Example("Different widths")]
        public static PlotModel DifferentWidths()
        {
            var model = new PlotModel { Title = "Different widths", Subtitle = "Series1=0.6 and Series2=0.3" };
            var series1 = new TSeries { Title = "Series 1" };
            SetBarWidth(series1, 0.6);
            series1.Items.Add(new TItem { Value = 1 });
            series1.Items.Add(new TItem { Value = 2 });
            model.Series.Add(series1);

            var series2 = new TSeries { Title = "Series 2" };
            SetBarWidth(series2, 0.3);
            series2.Items.Add(new TItem { Value = 3 });
            series2.Items.Add(new TItem { Value = 1 });
            model.Series.Add(series2);

            var categoryAxis = new CategoryAxis
            {
                Title = "Category",
                Position = CategoryAxisPosition()
            };
            categoryAxis.Labels.AddRange(new[] { "A", "B" });
            model.Axes.Add(categoryAxis);

            return model;
        }

        private static void SetBarWidth(TSeries series, double width)
        {
            if (typeof(TSeries) == typeof(ColumnSeries))
            {
                (series as ColumnSeries).ColumnWidth = width;
            }
            else
            {
                (series as BarSeries).BarWidth = width;
            }
        }

        [Example("Different widths (stacked)")]
        public static PlotModel DifferentWidthsStacked()
        {
            var model = new PlotModel { Title = "Different widths (stacked)" };
            var series1 = new TSeries { Title = "Series 1", IsStacked = true };
            SetBarWidth(series1, 0.6);
            series1.Items.Add(new TItem { Value = 1 });
            series1.Items.Add(new TItem { Value = 2 });
            model.Series.Add(series1);

            var series2 = new TSeries { Title = "Series 2", IsStacked = true };
            SetBarWidth(series2, 0.3);
            series2.Items.Add(new TItem { Value = 3 });
            series2.Items.Add(new TItem { Value = 1 });
            model.Series.Add(series2);

            var categoryAxis = new CategoryAxis
            {
                Title = "Category",
                Position = CategoryAxisPosition()
            };
            categoryAxis.Labels.AddRange(new[] { "A", "B" });
            model.Axes.Add(categoryAxis);

            return model;
        }

        [Example("Invalid values")]
        public static PlotModel InvalidValues()
        {
            var model = new PlotModel { Title = "Invalid values", Subtitle = "Series 1 contains a NaN value for category B." };
            var series1 = new TSeries { Title = "Series 1" };
            series1.Items.Add(new TItem { Value = 1 });
            series1.Items.Add(new TItem { Value = double.NaN });
            model.Series.Add(series1);

            var series2 = new TSeries { Title = "Series 2" };
            series2.Items.Add(new TItem { Value = 3 });
            series2.Items.Add(new TItem { Value = 1 });
            model.Series.Add(series2);

            var categoryAxis = new CategoryAxis
            {
                Title = "Category",
                Position = CategoryAxisPosition()
            };
            categoryAxis.Labels.AddRange(new[] { "A", "B" });
            model.Axes.Add(categoryAxis);

            return model;
        }

        [Example("Missing values")]
        public static PlotModel MissingValues()
        {
            var model = new PlotModel { Title = "Missing values", Subtitle = "Series 1 contains only one item with CategoryIndex = 1" };
            var series1 = new TSeries { Title = "Series 1" };
            series1.Items.Add(new TItem { Value = 1, CategoryIndex = 1 });
            model.Series.Add(series1);

            var series2 = new TSeries { Title = "Series 2" };
            series2.Items.Add(new TItem { Value = 3 });
            series2.Items.Add(new TItem { Value = 1.2 });
            model.Series.Add(series2);

            var categoryAxis = new CategoryAxis
            {
                Title = "Category",
                Position = CategoryAxisPosition()
            };
            categoryAxis.Labels.AddRange(new[] { "A", "B" });
            model.Axes.Add(categoryAxis);

            return model;
        }

        [Example("CategoryIndex")]
        public static PlotModel CategoryIndex()
        {
            var model = new PlotModel { Title = "CategoryIndex", Subtitle = "Setting CategoryIndex = 0 for both items in the series." };
            var series = new TSeries { Title = "Series 1", StrokeThickness = 1 };
            series.Items.Add(new TItem { Value = 1, CategoryIndex = 0 });
            series.Items.Add(new TItem { Value = 2, CategoryIndex = 0 });
            model.Series.Add(series);

            var categoryAxis = new CategoryAxis
            {
                Title = "Category",
                Position = CategoryAxisPosition()
            };
            categoryAxis.Labels.AddRange(new[] { "A", "B" });
            model.Axes.Add(categoryAxis);

            return model;
        }

        [Example("CategoryIndex (stacked)")]
        public static PlotModel CategoryIndexStacked()
        {
            var model = new PlotModel { Title = "CategoryIndex (stacked)", Subtitle = "Setting CategoryIndex = 0 for both items in the series." };
            var series = new TSeries { Title = "Series 1", IsStacked = true, StrokeThickness = 1 };
            series.Items.Add(new TItem { Value = 1, CategoryIndex = 0 });
            series.Items.Add(new TItem { Value = 2, CategoryIndex = 0 });
            model.Series.Add(series);

            var categoryAxis = new CategoryAxis
            {
                Title = "Category",
                Position = CategoryAxisPosition()
            };
            categoryAxis.Labels.AddRange(new[] { "A", "B" });
            model.Axes.Add(categoryAxis);

            return model;
        }

        [Example("BaseValue")]
        public static PlotModel BaseValue()
        {
            var model = new PlotModel { Title = "BaseValue", Subtitle = "BaseValue = -1" };
            var series1 = new TSeries { Title = "Series 1", BaseValue = -1 };
            series1.Items.Add(new TItem { Value = 1 });
            series1.Items.Add(new TItem { Value = 2 });
            model.Series.Add(series1);
            var series2 = new TSeries { Title = "Series 2", BaseValue = -1 };
            series2.Items.Add(new TItem { Value = 4 });
            series2.Items.Add(new TItem { Value = 7 });
            model.Series.Add(series2);

            var categoryAxis = new CategoryAxis
            {
                Title = "Category",
                Position = CategoryAxisPosition()
            };
            categoryAxis.Labels.AddRange(new[] { "A", "B" });
            model.Axes.Add(categoryAxis);

            return model;
        }

        [Example("BaseValue (stacked)")]
        public static PlotModel BaseValueStacked()
        {
            var model = new PlotModel { Title = "BaseValue (stacked)", Subtitle = "BaseValue = -1" };
            var series1 = new TSeries { Title = "Series 1", IsStacked = true, BaseValue = -1 };
            series1.Items.Add(new TItem { Value = 1 });
            series1.Items.Add(new TItem { Value = 2 });
            model.Series.Add(series1);
            var series2 = new TSeries { Title = "Series 2", IsStacked = true, BaseValue = -1 };
            series2.Items.Add(new TItem { Value = 4 });
            series2.Items.Add(new TItem { Value = 7 });
            model.Series.Add(series2);

            var categoryAxis = new CategoryAxis
            {
                Title = "Category",
                Position = CategoryAxisPosition()
            };
            categoryAxis.Labels.AddRange(new[] { "A", "B" });
            model.Axes.Add(categoryAxis);
            return model;
        }

        [Example("GapWidth 0%")]
        public static PlotModel GapWidth0()
        {
            return CreateGapWidthModel(0, "GapWidth 0%");
        }

        [Example("GapWidth 100% (default)")]
        public static PlotModel GapWidth100()
        {
            return CreateGapWidthModel(1, "GapWidth 100% (default)");
        }

        [Example("GapWidth 200%")]
        public static PlotModel GapWidth200()
        {
            return CreateGapWidthModel(2, "GapWidth 200%");
        }

        private static PlotModel CreateGapWidthModel(double gapWidth, string title)
        {
            var model = CreateSimpleModel(false, title);
            ((CategoryAxis)model.Axes[0]).GapWidth = gapWidth;
            return model;
        }

        // [Example("All in one")]
        public static PlotModel AllInOne()
        {
            var model = new PlotModel
            {
                Title = "All in one",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            var categoryAxis = new CategoryAxis { Position = CategoryAxisPosition(), GapWidth = 0.01 };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            var valueAxis = new LinearAxis
            {
                Position = ValueAxisPosition(),
                MinimumPadding = 0.06,
                MaximumPadding = 0.06,
                ExtraGridlines = new[] { 0.0 },
                ExtraGridlineStyle = LineStyle.Solid,
                ExtraGridlineColor = OxyColors.Black,
                ExtraGridlineThickness = 1
            };

            var categoryA = 0;
            var categoryB = 1;
            var categoryC = 2;
            var categoryD = 3;

            var s1 = new TSeries { Title = "Series 1", IsStacked = true, StrokeColor = OxyColors.Black, StrokeThickness = 1, StackGroup = "3" };
            s1.Items.Add(new TItem { Value = 25 });
            s1.Items.Add(new TItem { Value = 137 });
            s1.Items.Add(new TItem { Value = 18 });
            s1.Items.Add(new TItem { Value = 40 });
            var s2 = new TSeries { Title = "Series 2", IsStacked = true, StrokeColor = OxyColors.Black, StrokeThickness = 1, StackGroup = "3" };
            s2.Items.Add(new TItem { Value = -12 });
            s2.Items.Add(new TItem { Value = -14 });
            s2.Items.Add(new TItem { Value = -120 });
            s2.Items.Add(new TItem { Value = -26 });

            var s3 = new TSeries { Title = "Series 3", IsStacked = true, StrokeColor = OxyColors.Black, StrokeThickness = 1, StackGroup = "5" };
            s3.Items.Add(new TItem { Value = 21 });
            s3.Items.Add(new TItem { Value = 8 });
            s3.Items.Add(new TItem { Value = 48 });
            s3.Items.Add(new TItem { Value = 3 });
            var s4 = new TSeries { Title = "Series 4", IsStacked = true, StrokeColor = OxyColors.Black, StrokeThickness = 1, StackGroup = "5", LabelFormatString = "{0:0}", LabelPlacement = LabelPlacement.Middle };
            s4.Items.Add(new TItem { Value = -8, CategoryIndex = categoryA });
            s4.Items.Add(new TItem { Value = -8, CategoryIndex = categoryA });
            s4.Items.Add(new TItem { Value = -8, CategoryIndex = categoryA });
            s4.Items.Add(new TItem { Value = -21, CategoryIndex = categoryB });
            s4.Items.Add(new TItem { Value = -3, CategoryIndex = categoryC });
            s4.Items.Add(new TItem { Value = -48, CategoryIndex = categoryD });
            s4.Items.Add(new TItem { Value = 8, CategoryIndex = categoryA });
            s4.Items.Add(new TItem { Value = 21, CategoryIndex = categoryB });
            s4.Items.Add(new TItem { Value = 3, CategoryIndex = categoryC });
            s4.Items.Add(new TItem { Value = 48, CategoryIndex = categoryD });

            var s5 = new TSeries { Title = "Series 5", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s5.Items.Add(new TItem { Value = 17, CategoryIndex = categoryA });
            s5.Items.Add(new TItem { Value = 179, CategoryIndex = categoryB });
            s5.Items.Add(new TItem { Value = 45, CategoryIndex = categoryC });
            s5.Items.Add(new TItem { Value = 65, CategoryIndex = categoryD });
            s5.Items.Add(new TItem { Value = 97, CategoryIndex = categoryA });
            s5.Items.Add(new TItem { Value = 21, CategoryIndex = categoryD });

            var s6 = new TSeries { Title = "Series 6", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1, LabelFormatString = "{0:0}", LabelPlacement = LabelPlacement.Base };
            s6.Items.Add(new TItem { Value = 7 });
            s6.Items.Add(new TItem { Value = 54 });
            s6.Items.Add(new TItem { Value = 68 });
            s6.Items.Add(new TItem { Value = 12 });

            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Series.Add(s3);
            model.Series.Add(s4);
            model.Series.Add(s5);
            model.Series.Add(s6);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }
    }
}
