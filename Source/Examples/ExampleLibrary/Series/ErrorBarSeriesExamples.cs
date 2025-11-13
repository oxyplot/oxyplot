// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorBarSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("ErrorBarSeries"), Tags("Series")]
    public class ErrorBarSeriesExamples
    {
        [Example("ErrorBarSeries")]
        [DocumentationExample("Series/ErrorBarSeries")]
        public static PlotModel GetErrorBarSeries()
        {
            var model = new PlotModel
            {
                Title = "ErrorBarSeries"
            };

            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            model.Legends.Add(l);

            var s1 = new ErrorBarSeries { Title = "Series 1", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Items.Add(new ErrorBarItem { Value = 25, Error = 2 });
            s1.Items.Add(new ErrorBarItem { Value = 137, Error = 25 });
            s1.Items.Add(new ErrorBarItem { Value = 18, Error = 4 });
            s1.Items.Add(new ErrorBarItem { Value = 40, Error = 29 });

            var s2 = new ErrorBarSeries { Title = "Series 2", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Items.Add(new ErrorBarItem { Value = 35, Error = 20 });
            s2.Items.Add(new ErrorBarItem { Value = 17, Error = 7 });
            s2.Items.Add(new ErrorBarItem { Value = 118, Error = 44 });
            s2.Items.Add(new ErrorBarItem { Value = 49, Error = 29 });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");

            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);

            return model;
        }

        [Example("ErrorBarSeries (thick error lines)")]
        public static PlotModel GetErrorBarSeriesThickErrorLines()
        {
            var model = GetErrorBarSeries();
            foreach (ErrorBarSeries s in model.Series)
            {
                s.ErrorWidth = 0;
                s.ErrorStrokeThickness = 4;
            }

            return model;
        }
    }
}
