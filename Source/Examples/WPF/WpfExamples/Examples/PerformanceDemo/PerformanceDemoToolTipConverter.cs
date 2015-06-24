using System;
using System.Globalization;
using System.Windows.Data;
using OxyPlot;

namespace WpfExamples.Examples.PerformanceDemo
{
    public class PerformanceDemoToolTipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var plotModel = value as PlotModel;
            if (plotModel == null)
            {
                return "No plot model available";
            }

            return plotModel.Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}