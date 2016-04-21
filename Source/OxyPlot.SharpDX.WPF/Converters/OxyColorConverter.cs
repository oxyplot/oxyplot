// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyColorConverter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Converts from OxyPlot colors to Windows.UI.Color and vice versa.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OxyPlot.SharpDX.WPF
{
    
    

    /// <summary>
    /// Converts from OxyPlot colors to Windows.UI.Color and vice versa.
    /// </summary>
    public class OxyColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is OxyColor))
            {
                return null;
            }

            var color = (OxyColor)value;
            if (targetType == typeof(Color))
            {
                return color.ToColor();
            }

            if (targetType == typeof(Brush))
            {
                return color.ToBrush();
            }

            return null;
        }

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The converted value.</returns>   

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(OxyColor))
            {
                return null;
            }

            if (value is Color)
            {
                var color = (Color)value;
                return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
            }

            var scb = value as SolidColorBrush;
            if (scb != null)
            {
                var color = scb.Color;
                return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
            }

            return null;
        }


    }
}