// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShowMeTheNumbersExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Examples from the book "Show Me the Numbers" by Stephen Few
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ExampleLibrary
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using OxyPlot;

    /// <summary>
    /// Examples from the book "Show Me the Numbers" by Stephen Few
    /// </summary>
    [Examples("Examples from the book 'Show Me the Numbers'")]
    public class ShowMeTheNumbersExamples : ExamplesBase
    {
        /// <summary>
        /// The graph 1.
        /// </summary>
        /// <returns>
        /// </returns>
        [Example("Q1 2003 Calls by Region")]
        public static PlotModel Graph1()
        {
            var pm = new PlotModel("Q1 2003 Calls by Region");
            pm.PlotAreaBorderThickness = 0;
            pm.Axes.Add(
                new CategoryAxis
                    {
                        AxislineStyle = LineStyle.Solid,
                        Labels = new List<string> { "North", "East", "South", "West" },
                        TickStyle = TickStyle.None
                    });
            pm.Axes.Add(
                new LinearAxis(AxisPosition.Left, 0, 6000, 1000, 1000)
                    {
                        AxislineStyle = LineStyle.Solid,
                        TickStyle = TickStyle.Outside,
                        StringFormat = "#,0"
                    });
            var series = new ColumnSeries { FillColor = OxyColors.Black };
            series.Items.Add(new ColumnItem { Value = 3000 });
            series.Items.Add(new ColumnItem { Value = 4500 });
            series.Items.Add(new ColumnItem { Value = 2100 });
            series.Items.Add(new ColumnItem { Value = 4800 });
            pm.Series.Add(series);
            return pm;
        }

        /// <summary>
        /// The graph 2.
        /// </summary>
        /// <returns>
        /// </returns>
        [Example("2003 Sales")]
        public static PlotModel Graph2()
        {
            var pm = new PlotModel("2003 Sales");
            pm.PlotAreaBorderThickness = 0;
            pm.IsLegendVisible = false;
            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var sales1 = new[] { 1000, 1010, 1020, 1010, 1020, 1030, 1000, 500, 1000, 900, 900, 1000 };
            var sales2 = new[] { 2250, 2500, 2750, 2500, 2750, 3000, 2500, 2750, 3100, 2800, 3100, 3500 };
            pm.Axes.Add(new CategoryAxis
            {
                AxislineStyle = LineStyle.Solid,
                Labels = new List<string>(months),
                TickStyle = TickStyle.None
            });
            pm.Axes.Add(
                new LinearAxis(AxisPosition.Left, 0, 4000, 500, 500)
                    {
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
        /// <returns>
        /// </returns>
        [Example("Headcount")]
        public static PlotModel Graph3()
        {
            var pm = new PlotModel("Headcount");
            pm.PlotAreaBorderThickness = 0;
            pm.PlotMargins = new OxyThickness(100, 40, 20, 40);
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
            pm.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 250, 50, 50) { AxislineStyle = LineStyle.Solid, TickStyle = TickStyle.Outside, MinimumPadding = 0, MaximumPadding = 0 });
            pm.Series.Add(new BarSeries { FillColor = OxyColors.Black, ItemsSource = values, ValueField = "Value" });
            return pm;
        }

        /// <summary>
        /// The graph 4.
        /// </summary>
        /// <returns>
        /// </returns>
        [Example("Regional % of Total Expenses")]
        public static PlotModel Graph4()
        {
            var pm = new PlotModel("Regional % of Total Expenses");
            pm.PlotAreaBorderThickness = 0;
            pm.Axes.Add(
                new CategoryAxis
                    {
                        Labels = new List<string> { "West\n34%", "East\n30%", "North\n20%", "South\n16%" },
                        TickStyle = TickStyle.None,
                        GapWidth = 0
                    });
            pm.Axes.Add(
                new LinearAxis(AxisPosition.Left, 0, 0.35 + double.Epsilon, 0.05, 0.05)
                    {
                        AxislineStyle = LineStyle.Solid,
                        TickStyle = TickStyle.Outside,
                        StringFormat = "P0"
                    });

            var series = new ColumnSeries
                    {
                        ColumnWidth = 1.0,
                        StrokeColor = OxyColors.DarkGray,
                        StrokeThickness = 1.0,
                        FillColor = OxyColors.Black,
                    };
            series.Items.Add(new ColumnItem { Value = 0.34 });
            series.Items.Add(new ColumnItem { Value = 0.3 });
            series.Items.Add(new ColumnItem { Value = 0.2 });
            series.Items.Add(new ColumnItem { Value = 0.16 });
            pm.Series.Add(series);
            return pm;
        }

        /// <summary>
        /// The graph 5.
        /// </summary>
        /// <returns>
        /// </returns>
        [Example("Actual to Plan Variance")]
        public static PlotModel Graph5()
        {
            var pm = new PlotModel("Actual to Plan Variance");
            pm.PlotAreaBorderThickness = 0;
            var values = new Dictionary<string, double>();
            values.Add("Sales", 7);
            values.Add("Marketing", -7);
            values.Add("Systems", -2);
            values.Add("HR", -17);
            values.Add("Finance", 5);
            pm.Axes.Add(new CategoryAxis { ItemsSource = values, LabelField = "Key", TickStyle = TickStyle.None });
            pm.Axes.Add(
                new LinearAxis(AxisPosition.Left, -20, 10, 5, 5)
                    {
                        Layer = AxisLayer.AboveSeries,
                        AxislineStyle = LineStyle.Solid,
                        ExtraGridlines = new double[] { 0 },
                        ExtraGridlineColor = OxyColors.Black,
                        ExtraGridlineThickness = 3,
                        TickStyle = TickStyle.Outside,
                        StringFormat = "+0;-0;0"
                    });
            pm.Series.Add(
                new ColumnSeries
                    {
                        FillColor = OxyColors.Orange,
                        NegativeFillColor = OxyColors.Gray,
                        ItemsSource = values,
                        ValueField = "Value",
                    });
            return pm;
        }

        /// <summary>
        /// The graph 6.
        /// </summary>
        /// <returns>
        /// </returns>
        [Example("Order Count by Order Size")]
        public static PlotModel Graph6()
        {
            var pm = new PlotModel("Order Count by Order Size");
            pm.PlotAreaBorderThickness = 0;
            pm.PlotMargins = new OxyThickness(60, 4, 4, 60);
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
                TickStyle = TickStyle.None
            });
            pm.Axes.Add(
                new LinearAxis(AxisPosition.Left, 0, 6000, 1000, 1000)
                    {
                        AxislineStyle = LineStyle.Solid,
                        TickStyle = TickStyle.Outside,
                        StringFormat = "+0;-0;0"
                    });
            pm.Series.Add(new ColumnSeries { FillColor = OxyColors.Orange, ItemsSource = values, ValueField = "Value" });
            return pm;
        }

        /// <summary>
        /// The graph 7.
        /// </summary>
        /// <returns>
        /// </returns>
        [Example("Correlation of Employee Heights and Salaries")]
        public static PlotModel Graph7()
        {
            var pm = new PlotModel("Correlation of Employee Heights and Salaries")
                {
                    PlotAreaBorderThickness = 0
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
                new LinearAxis(AxisPosition.Left, 30000, 90000, 10000, 10000)
                    {
                        AxislineStyle = LineStyle.Solid,
                        TickStyle = TickStyle.Outside,
                        StringFormat = "0,0"
                    });
            pm.Axes.Add(new LinearAxis(AxisPosition.Bottom, 60, 80, 5, 5)
            {
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