using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace OxyPlotControls.Converters;

/// <summary>
/// Converter that calculates the width of TabItems to evenly fill the TabControl.
/// </summary>
public class TabSizeConverter : IMultiValueConverter
{
    /// <summary>
    /// Converts the TabControl and its width to calculate equal-width tabs.
    /// </summary>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            return 100.0;

        if (values[0] is not TabControl tabControl)
            return 100.0;

        if (values[1] is not double actualWidth || actualWidth <= 0)
            return 100.0;

        var itemCount = tabControl.Items.Count;
        if (itemCount == 0)
            return actualWidth;

        // Calculate width per tab, accounting for borders and margins
        var calculatedWidth = (actualWidth - 4) / itemCount;
        return Math.Max(calculatedWidth, 40); // Minimum width of 40
    }

    /// <summary>
    /// Not implemented - one-way converter.
    /// </summary>
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
