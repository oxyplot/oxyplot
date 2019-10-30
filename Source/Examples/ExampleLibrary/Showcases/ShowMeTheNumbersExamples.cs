// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShowMeTheNumbersExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Examples from the book "Show Me the Numbers" by Stephen Few
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Examples from the book "Show Me the Numbers" by Stephen Few
    /// </summary>
    [Examples("Examples from the book 'Show Me the Numbers'"), Tags("Showcase")]
    public class ShowMeTheNumbersExamples
    {
        /// <summary>
        /// The graph 1.
        /// </summary>
        /// <returns></returns>
        [Example("Q1 2003 Calls by Region")]
        public static PlotModel Graph1()
        {
            var pm = new PlotModel { Title = "Q1 2003 Calls by Region", PlotAreaBorderThickness = new OxyThickness(0) };
            var categoryAxis = new CategoryAxis
                    {
                        AxislineStyle = LineStyle.Solid,
                        TickStyle = TickStyle.None,
                        Key = "y"
                    };
            categoryAxis.Labels.AddRange(new[] { "North", "East", "South", "West" });
            pm.Axes.Add(categoryAxis);
            pm.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = 0,
                    Maximum = 6000,
                    MajorStep = 1000,
                    MinorStep = 1000,
                    AxislineStyle = LineStyle.Solid,
                    TickStyle = TickStyle.Outside,
                    StringFormat = "#,0",
                    Key = "x"
                });
            var series = new BarSeries { FillColor = OxyColors.Black, XAxisKey = "x", YAxisKey = "y" };
            series.Items.Add(new BarItem { Value = 3000 });
            series.Items.Add(new BarItem { Value = 4500 });
            series.Items.Add(new BarItem { Value = 2100 });
            series.Items.Add(new BarItem { Value = 4800 });
            pm.Series.Add(series);
            return pm;
        }

        /// <summary>
        /// The graph 2.
        /// </summary>
        /// <returns></returns>
        [Example("2003 Sales")]
        public static PlotModel Graph2()
        {
            var pm = new PlotModel
                         {
                             Title = "2003 Sales",
                             PlotAreaBorderThickness = new OxyThickness(0),
                             IsLegendVisible = false
                         };
            var sales1 = new[] { 1000, 1010, 1020, 1010, 1020, 1030, 1000, 500, 1000, 900, 900, 1000 };
            var sales2 = new[] { 2250, 2500, 2750, 2500, 2750, 3000, 2500, 2750, 3100, 2800, 3100, 3500 };
            var categoryAxis = new CategoryAxis
            {
                AxislineStyle = LineStyle.Solid,
                TickStyle = TickStyle.None
            };
            categoryAxis.Labels.AddRange(new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" });
            pm.Axes.Add(categoryAxis);
            pm.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = 0,
                    Maximum = 4000,
                    MajorStep = 500,
                    MinorStep = 500,
                    AxislineStyle = LineStyle.Solid,
                    TickStyle = TickStyle.Outside,
                    StringFormat = "#,0"
                });
            var s1 = new LineSeries { Color = OxyColors.Orange };
            for (int i = 0; i < 12; i++)
            {
                s1.Points.Add(new DataPoint(i, sales1[i]));
            }

            var s2 = new LineSeries { Color = OxyColors.Gray };
            for (int i = 0; i < 12; i++)
            {
                s2.Points.Add(new DataPoint(i, sales2[i]));
            }

            pm.Series.Add(s1);
            pm.Series.Add(s2);
            return pm;
        }

        /// <summary>
        /// The graph 3.
        /// </summary>
        /// <returns></returns>
        [Example("Headcount")]
        public static PlotModel Graph3()
        {
            var pm = new PlotModel
                         {
                             Title = "Headcount",
                             PlotAreaBorderThickness = new OxyThickness(0),
                             PlotMargins = new OxyThickness(100, 40, 20, 40)
                         };
            var values = new Dictionary<string, double> {
                    { "Manufacturing", 240 },
                    { "Sales", 160 },
                    { "Engineering", 50 },
                    { "Operations", 45 },
                    { "Finance", 40 },
                    { "Info Systems", 39 },
                    { "Legal", 25 },
                    { "Marketing", 10 }
                };
            pm.Axes.Add(
                new CategoryAxis
                    {
                        Position = AxisPosition.Left,
                        ItemsSource = values,
                        LabelField = "Key",
                        TickStyle = TickStyle.None,
                        AxisTickToLabelDistance = 10,
                        StartPosition = 1,
                        EndPosition = 0
                    });
            pm.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 250, MajorStep = 50, MinorStep = 50, AxislineStyle = LineStyle.Solid, TickStyle = TickStyle.Outside, MinimumPadding = 0, MaximumPadding = 0 });
            pm.Series.Add(new BarSeries { FillColor = OxyColors.Black, ItemsSource = values, ValueField = "Value" });
            return pm;
        }

        /// <summary>
        /// The graph 4.
        /// </summary>
        /// <returns></returns>
        [Example("Regional % of Total Expenses")]
        public static PlotModel Graph4()
        {
            var pm = new PlotModel { Title = "Regional % of Total Expenses", PlotAreaBorderThickness = new OxyThickness(0) };
            var categoryAxis = new CategoryAxis
            {
                TickStyle = TickStyle.None,
                GapWidth = 0,
                Key = "y"
            };
            categoryAxis.Labels.AddRange(new[] { "West\n34%", "East\n30%", "North\n20%", "South\n16%" });
            pm.Axes.Add(categoryAxis);

            pm.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = 0,
                    Maximum = 0.35 + double.Epsilon,
                    MajorStep = 0.05,
                    MinorStep = 0.05,
                    AxislineStyle = LineStyle.Solid,
                    TickStyle = TickStyle.Outside,
                    StringFormat = "P0",
                    Key = "x"
                });

            var series = new BarSeries
                    {
                        BarWidth = 1.0,
                        StrokeColor = OxyColors.DarkGray,
                        StrokeThickness = 1.0,
                        FillColor = OxyColors.Black,
                        XAxisKey = "x",
                        YAxisKey = "y"
                    };
            series.Items.Add(new BarItem { Value = 0.34 });
            series.Items.Add(new BarItem { Value = 0.3 });
            series.Items.Add(new BarItem { Value = 0.2 });
            series.Items.Add(new BarItem { Value = 0.16 });
            pm.Series.Add(series);
            return pm;
        }

        /// <summary>
        /// The graph 5.
        /// </summary>
        /// <returns></returns>
        [Example("Actual to Plan Variance")]
        public static PlotModel Graph5()
        {
            var pm = new PlotModel { Title = "Actual to Plan Variance", PlotAreaBorderThickness = new OxyThickness(0) };
            var values = new Dictionary<string, double>();
            values.Add("Sales", 7);
            values.Add("Marketing", -7);
            values.Add("Systems", -2);
            values.Add("HR", -17);
            values.Add("Finance", 5);
            pm.Axes.Add(new CategoryAxis { ItemsSource = values, LabelField = "Key", TickStyle = TickStyle.None, Key = "y" });
            pm.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = -20,
                    Maximum = 10,
                    MinorStep = 5,
                    MajorStep = 5,
                    Layer = AxisLayer.AboveSeries,
                    AxislineStyle = LineStyle.Solid,
                    ExtraGridlines = new double[] { 0 },
                    ExtraGridlineColor = OxyColors.Black,
                    ExtraGridlineThickness = 3,
                    TickStyle = TickStyle.Outside,
                    StringFormat = "+0;-0;0",
                    Key = "x"
                });
            pm.Series.Add(
                new BarSeries
                    {
                        FillColor = OxyColors.Orange,
                        NegativeFillColor = OxyColors.Gray,
                        ItemsSource = values,
                        ValueField = "Value",
                        XAxisKey = "x",
                        YAxisKey = "y"
                    });
            return pm;
        }

        /// <summary>
        /// The graph 6.
        /// </summary>
        /// <returns></returns>
        [Example("Order Count by Order Size")]
        public static PlotModel Graph6()
        {
            var pm = new PlotModel
                         {
                             Title = "Order Count by Order Size",
                             PlotAreaBorderThickness = new OxyThickness(0),
                             PlotMargins = new OxyThickness(60, 4, 4, 60)
                         };
            var values = new Dictionary<string, double>
                {
                    { " <$10", 5000 },
                    { ">=$10\n    &\n <$20", 1500 },
                    { ">=$20\n    &\n <$30", 1000 },
                    { ">=$40\n    &\n <$40", 500 },
                    { ">=$40", 200 }
                };
            pm.Axes.Add(new CategoryAxis
            {
                AxislineStyle = LineStyle.Solid,
                ItemsSource = values,
                LabelField = "Key",
                TickStyle = TickStyle.None,
                Key = "y"
            });
            pm.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = 0,
                    Maximum = 6000,
                    MajorStep = 1000,
                    MinorStep = 1000,
                    AxislineStyle = LineStyle.Solid,
                    TickStyle = TickStyle.Outside,
                    StringFormat = "+0;-0;0",
                    Key = "x"
                });
            pm.Series.Add(new BarSeries { FillColor = OxyColors.Orange, ItemsSource = values, ValueField = "Value", XAxisKey = "x", YAxisKey = "y" });
            return pm;
        }

        /// <summary>
        /// The graph 7.
        /// </summary>
        /// <returns></returns>
        [Example("Correlation of Employee Heights and Salaries")]
        public static PlotModel Graph7()
        {
            var pm = new PlotModel
            {
                Title = "Correlation of Employee Heights and Salaries",
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            var values = new[]
            {
                    new DataPoint(62, 39000),
                    new DataPoint(66, 44000),
                    new DataPoint(64, 50000),
                    new DataPoint(66, 49500),
                    new DataPoint(67, 52000),
                    new DataPoint(68, 50000),
                    new DataPoint(66, 56000),
                    new DataPoint(67, 56000),
                    new DataPoint(72, 56000),
                    new DataPoint(68, 58000),
                    new DataPoint(69, 62000),
                    new DataPoint(71, 63000),
                    new DataPoint(65, 64000),
                    new DataPoint(68, 71000),
                    new DataPoint(72, 72000),
                    new DataPoint(74, 69000),
                    new DataPoint(74, 79000),
                    new DataPoint(77, 81000)
                };
            pm.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = 30000,
                    Maximum = 90000,
                    MajorStep = 10000,
                    MinorStep = 10000,
                    AxislineStyle = LineStyle.Solid,
                    TickStyle = TickStyle.Outside,
                    StringFormat = "0,0"
                });
            pm.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 60,
                Maximum = 80,
                MajorStep = 5,
                MinorStep = 5,
                AxislineStyle = LineStyle.Solid,
                TickStyle = TickStyle.Outside
            });
            pm.Series.Add(
                new ScatterSeries
                    {
                        ItemsSource = values,
                        MarkerType = MarkerType.Circle,
                        MarkerSize = 3.0,
                        MarkerFill = OxyColors.White,
                        MarkerStroke = OxyColors.Black,
                        DataFieldX = "X",
                        DataFieldY = "Y"
                    });
            return pm;
        }

    }
}
