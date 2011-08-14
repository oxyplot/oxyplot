using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace OxyPlot.Silverlight
{
  //  [ValueConversion(typeof(OxyColor), typeof(Rect))]
    public class OxyColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OxyColor)
            {
                var color = (OxyColor)value;
                if (targetType == typeof(Color))
                    return color.ToColor();
                if (targetType == typeof(Brush))
                    return color.ToBrush();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(OxyColor))
            {
                if (value is Color)
                {
                    var color = (Color)value;
                    return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
                }
                if (value is SolidColorBrush)
                {
                    var brush = (SolidColorBrush)value;
                    var color = brush.Color;
                    return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
                }
            }
            return null;
        }
    }
}