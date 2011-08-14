using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OxyPlot.Silverlight
{
    /// <summary>
    /// Converts Thickness to double.
    /// This is used to convert BorderThickness properties to Path.StrokeThickness (double).
    /// The maximum thickness value is used.
    /// </summary>
   // [ValueConversion(typeof (Thickness), typeof (double))]
    public class ThicknessConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness)
            {
                var t = (Thickness) value;
                if (targetType == typeof (double))
                    return Math.Max(Math.Max(t.Left,t.Right),Math.Max(t.Top,t.Bottom));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}