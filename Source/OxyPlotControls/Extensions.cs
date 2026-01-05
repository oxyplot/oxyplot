using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace OxyPlotControls
{
    /// <summary>
    /// Provides extension methods for OxyPlot types and data conversion utilities.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the first abstract base type in the inheritance hierarchy.
        /// </summary>
        /// <param name="type">The type to search from.</param>
        /// <returns>The first abstract base type, or null if none found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
        public static Type? GetFirstAbstractBaseType(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            Type? baseType = type.BaseType;
            if (baseType == null || baseType.IsAbstract) return baseType;

            return baseType.GetFirstAbstractBaseType();
        }

        /// <summary>
        /// Converts a ScreenVector to a formatted string representation.
        /// </summary>
        /// <param name="sv">The screen vector to convert.</param>
        /// <returns>A string in the format "X, Y" using invariant culture.</returns>
        public static string ToPrettyText(this ScreenVector sv)
        {
            return sv.X.ToString("G17", CultureInfo.InvariantCulture) + ", " + sv.Y.ToString("G17", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses a formatted string to create a ScreenVector.
        /// </summary>
        /// <param name="svString">The string in "X, Y" format.</param>
        /// <returns>A ScreenVector parsed from the string, or default if parsing fails.</returns>
        public static ScreenVector FromPrettyVectorText(this string svString)
        {
            var svStringSplit = svString.Split(", ", StringSplitOptions.None);
            if (svStringSplit.Length != 2) return default;

            if (!double.TryParse(svStringSplit[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double x)) return default;
            if (!double.TryParse(svStringSplit[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double y)) return default;
            return new ScreenVector(x, y);
        }

        /// <summary>
        /// Converts a DataPoint to a formatted string representation.
        /// </summary>
        /// <param name="dp">The data point to convert.</param>
        /// <returns>A string in the format "X, Y" using invariant culture.</returns>
        public static string ToPrettyText(this DataPoint dp)
        {
            return dp.X.ToString("G17", CultureInfo.InvariantCulture) + ", " + dp.Y.ToString("G17", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses a formatted string to create a DataPoint.
        /// </summary>
        /// <param name="dpString">The string in "X, Y" format.</param>
        /// <returns>A DataPoint parsed from the string, or DataPoint.Undefined if parsing fails.</returns>
        public static DataPoint FromPrettyDataText(this string dpString)
        {
            var dpStringSplit = dpString.Split(", ", StringSplitOptions.None);
            if (dpStringSplit.Length != 2) return DataPoint.Undefined;

            if (!double.TryParse(dpStringSplit[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double x)) return DataPoint.Undefined;
            if (!double.TryParse(dpStringSplit[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double y)) return DataPoint.Undefined;
            return new DataPoint(x, y);
        }

        /// <summary>
        /// Converts a DataPoint to an XML element for serialization.
        /// </summary>
        /// <param name="dp">The data point to convert.</param>
        /// <returns>An XElement representing the data point.</returns>
        public static XElement ToXElement(this DataPoint dp)
        {
            var dpElement = new XElement("DataPoint");
            dpElement.SetAttributeValue("X", dp.X.ToString("G17", CultureInfo.InvariantCulture));
            dpElement.SetAttributeValue("Y", dp.Y.ToString("G17", CultureInfo.InvariantCulture));
            return dpElement;
        }

        /// <summary>
        /// Parses an XML element to create a DataPoint.
        /// </summary>
        /// <param name="dpElement">The XML element containing X and Y attributes.</param>
        /// <returns>A DataPoint parsed from the element, or DataPoint.Undefined if parsing fails.</returns>
        public static DataPoint PointFromXElement(this XElement dpElement)
        {
            if (dpElement.Name != "DataPoint") return DataPoint.Undefined;
            if (dpElement.Attribute("X") == null) return DataPoint.Undefined;
            if (dpElement.Attribute("Y") == null) return DataPoint.Undefined;

            if (!double.TryParse(dpElement.Attribute("X")!.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double x)) return DataPoint.Undefined;
            if (!double.TryParse(dpElement.Attribute("Y")!.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double y)) return DataPoint.Undefined;
            return new DataPoint(x, y);
        }

        /// <summary>
        /// Converts a ScreenPoint to a formatted string representation.
        /// </summary>
        /// <param name="sp">The screen point to convert.</param>
        /// <returns>A string in the format "X, Y" using invariant culture.</returns>
        public static string ToPrettyText(this ScreenPoint sp)
        {
            return sp.X.ToString("G17", CultureInfo.InvariantCulture) + ", " + sp.Y.ToString("G17", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses a formatted string to create a ScreenPoint.
        /// </summary>
        /// <param name="spString">The string in "X, Y" format.</param>
        /// <returns>A ScreenPoint parsed from the string, or ScreenPoint.Undefined if parsing fails.</returns>
        public static ScreenPoint FromPrettyScreenText(this string spString)
        {
            var spStringSplit = spString.Split(", ", StringSplitOptions.None);
            if (spStringSplit.Length != 2) return ScreenPoint.Undefined;

            if (!double.TryParse(spStringSplit[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double x)) return ScreenPoint.Undefined;
            if (!double.TryParse(spStringSplit[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double y)) return ScreenPoint.Undefined;
            return new ScreenPoint(x, y);
        }

        /// <summary>
        /// Converts a Vector to a formatted string representation.
        /// </summary>
        /// <param name="v">The vector to convert.</param>
        /// <returns>A string in the format "X, Y" using invariant culture.</returns>
        public static string ToPrettyText(this Vector v)
        {
            return v.X.ToString("G17", CultureInfo.InvariantCulture) + ", " + v.Y.ToString("G17", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses a formatted string to create a Vector.
        /// </summary>
        /// <param name="vString">The string in "X, Y" format.</param>
        /// <returns>A Vector parsed from the string, or default if parsing fails.</returns>
        public static Vector FromPrettyVectorString(this string vString)
        {
            var vStringSplit = vString.Split(", ", StringSplitOptions.None);
            if (vStringSplit.Length != 2) return default;

            if (!double.TryParse(vStringSplit[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double x)) return default;
            if (!double.TryParse(vStringSplit[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double y)) return default;
            return new Vector(x, y);
        }

        /// <summary>
        /// Converts a list of DataPoints to an XML element for serialization.
        /// </summary>
        /// <param name="dataPoints">The list of data points to convert.</param>
        /// <param name="name">The name for the parent XML element.</param>
        /// <returns>An XElement containing all data points as child elements.</returns>
        public static XElement ToXElement(this IList<DataPoint> dataPoints, string name)
        {
            var el = new XElement(name);
            foreach (var dp in dataPoints)
            {
                el.Add(dp.ToXElement());
            }
            return el;
        }

        /// <summary>
        /// Parses an XML element to create a list of DataPoints.
        /// </summary>
        /// <param name="dpelements">The XML element containing DataPoint child elements.</param>
        /// <returns>A list of DataPoints parsed from the element.</returns>
        public static IList<DataPoint> PointsFromXElement(this XElement dpelements)
        {
            if (dpelements.Name != "DataPoints" && dpelements.Name != "Points") return new List<DataPoint>();

            var dpList = new List<DataPoint>();
            foreach (var dp in dpelements.Elements("DataPoint"))
            {
                dpList.Add(dp.PointFromXElement());
            }
            return dpList;
        }

        /// <summary>
        /// Copies a binding from one dependency object to another.
        /// </summary>
        /// <param name="fromTarget">The source dependency object.</param>
        /// <param name="toTarget">The target dependency object.</param>
        /// <param name="dp">The dependency property to copy the binding for.</param>
        /// <returns>True if a binding was found and copied; otherwise, false.</returns>
        public static bool CopyBinding(this DependencyObject fromTarget, DependencyObject toTarget, DependencyProperty dp)
        {
            var te = BindingOperations.GetBinding(fromTarget, dp);
            if (te == null) return false;
            BindingOperations.SetBinding(toTarget, dp, te);
            return true;
        }

        /// <summary>
        /// Checks if a dependency property has a binding.
        /// </summary>
        /// <param name="target">The dependency object to check.</param>
        /// <param name="dp">The dependency property to check.</param>
        /// <returns>True if the property has a binding; otherwise, false.</returns>
        public static bool IsBound(this DependencyObject target, DependencyProperty dp)
        {
            return BindingOperations.GetBinding(target, dp) != null;
        }

        /// <summary>
        /// Copies common axis properties from one axis to another.
        /// </summary>
        /// <param name="toAxis">The target axis to copy properties to.</param>
        /// <param name="fromAxis">The source axis to copy properties from.</param>
        public static void CopyFromAxis(this Axis toAxis, Axis fromAxis)
        {
            // General properties
            toAxis.AbsoluteMaximum = fromAxis.AbsoluteMaximum;
            toAxis.AbsoluteMinimum = fromAxis.AbsoluteMinimum;
            toAxis.Angle = fromAxis.Angle;
            toAxis.AxisDistance = fromAxis.AxisDistance;
            toAxis.AxislineColor = fromAxis.AxislineColor;
            toAxis.AxislineStyle = fromAxis.AxislineStyle;
            toAxis.AxislineThickness = fromAxis.AxislineThickness;
            toAxis.AxisTitleDistance = fromAxis.AxisTitleDistance;
            toAxis.AxisTickToLabelDistance = fromAxis.AxisTickToLabelDistance;
            toAxis.ClipTitle = fromAxis.ClipTitle;
            toAxis.EndPosition = fromAxis.EndPosition;
            toAxis.ExtraGridlineColor = fromAxis.ExtraGridlineColor;
            toAxis.ExtraGridlineStyle = fromAxis.ExtraGridlineStyle;
            toAxis.ExtraGridlineThickness = fromAxis.ExtraGridlineThickness;
            toAxis.ExtraGridlines = fromAxis.ExtraGridlines;
            toAxis.FilterMaxValue = fromAxis.FilterMaxValue;
            toAxis.FilterMinValue = fromAxis.FilterMinValue;
            toAxis.Font = fromAxis.Font;
            toAxis.FontSize = fromAxis.FontSize;
            toAxis.FontWeight = fromAxis.FontWeight;
            toAxis.IntervalLength = fromAxis.IntervalLength;
            toAxis.IsPanEnabled = fromAxis.IsPanEnabled;
            toAxis.IsAxisVisible = fromAxis.IsAxisVisible;
            toAxis.IsZoomEnabled = fromAxis.IsZoomEnabled;
            toAxis.Key = fromAxis.Key;
            toAxis.Layer = fromAxis.Layer;
            toAxis.MajorGridlineColor = fromAxis.MajorGridlineColor;
            toAxis.MinorGridlineColor = fromAxis.MinorGridlineColor;
            toAxis.MajorGridlineStyle = fromAxis.MajorGridlineStyle;
            toAxis.MinorGridlineStyle = fromAxis.MinorGridlineStyle;
            toAxis.MajorGridlineThickness = fromAxis.MajorGridlineThickness;
            toAxis.MinorGridlineThickness = fromAxis.MinorGridlineThickness;
            toAxis.MajorStep = fromAxis.MajorStep;
            toAxis.MajorTickSize = fromAxis.MajorTickSize;
            toAxis.MinorStep = fromAxis.MinorStep;
            toAxis.MinorTickSize = fromAxis.MinorTickSize;
            toAxis.Minimum = fromAxis.Minimum;
            toAxis.Maximum = fromAxis.Maximum;
            toAxis.MinimumRange = fromAxis.MinimumRange;
            toAxis.MaximumRange = fromAxis.MaximumRange;
            toAxis.MinimumPadding = fromAxis.MinimumPadding;
            toAxis.MaximumPadding = fromAxis.MaximumPadding;
            toAxis.Position = fromAxis.Position;
            toAxis.PositionTier = fromAxis.PositionTier;
            toAxis.PositionAtZeroCrossing = fromAxis.PositionAtZeroCrossing;
            toAxis.StartPosition = fromAxis.StartPosition;
            toAxis.StringFormat = fromAxis.StringFormat;
            toAxis.TextColor = fromAxis.TextColor;
            toAxis.TicklineColor = fromAxis.TicklineColor;
            toAxis.TitleClippingLength = fromAxis.TitleClippingLength;
            toAxis.TitleColor = fromAxis.TitleColor;
            toAxis.TitleFont = fromAxis.TitleFont;
            toAxis.TitleFontSize = fromAxis.TitleFontSize;
            toAxis.TitleFontWeight = fromAxis.TitleFontWeight;
            toAxis.TitleFormatString = fromAxis.TitleFormatString;
            toAxis.Title = fromAxis.Title;
            toAxis.ToolTip = fromAxis.ToolTip;
            toAxis.TickStyle = fromAxis.TickStyle;
            toAxis.TitlePosition = fromAxis.TitlePosition;
            toAxis.Unit = fromAxis.Unit;
            toAxis.UseSuperExponentialFormat = fromAxis.UseSuperExponentialFormat;
            toAxis.LabelFormatter = fromAxis.LabelFormatter;
            toAxis.Tag = fromAxis.Tag;
        }

        /// <summary>
        /// Copies common series properties from one series to another.
        /// </summary>
        /// <param name="toSeries">The target series to copy properties to.</param>
        /// <param name="fromSeries">The source series to copy properties from.</param>
        public static void CopyFromSeries(this Series toSeries, Series fromSeries)
        {
            toSeries.Title = fromSeries.Title;
            toSeries.IsVisible = fromSeries.IsVisible;
            toSeries.Background = fromSeries.Background;
            toSeries.TrackerKey = fromSeries.TrackerKey;
            toSeries.TrackerFormatString = fromSeries.TrackerFormatString;
            toSeries.Tag = fromSeries.Tag;
            toSeries.RenderInLegend = fromSeries.RenderInLegend;
            toSeries.EdgeRenderingMode = fromSeries.EdgeRenderingMode;
        }

        /// <summary>
        /// Copies common XY axis series properties from one series to another.
        /// </summary>
        /// <param name="toSeries">The target series to copy properties to.</param>
        /// <param name="fromSeries">The source series to copy properties from.</param>
        public static void CopyFromXYAxisSeries(this XYAxisSeries toSeries, XYAxisSeries fromSeries)
        {
            toSeries.CopyFromSeries(fromSeries);
            toSeries.XAxisKey = fromSeries.XAxisKey;
            toSeries.YAxisKey = fromSeries.YAxisKey;
        }

        /// <summary>
        /// Copies common annotation properties from one annotation to another.
        /// </summary>
        /// <param name="toAnnotation">The target annotation to copy properties to.</param>
        /// <param name="fromAnnotation">The source annotation to copy properties from.</param>
        public static void CopyFromAnnotation(this Annotation toAnnotation, Annotation fromAnnotation)
        {
            toAnnotation.Layer = fromAnnotation.Layer;
            toAnnotation.XAxisKey = fromAnnotation.XAxisKey;
            toAnnotation.YAxisKey = fromAnnotation.YAxisKey;
            toAnnotation.ClipByXAxis = fromAnnotation.ClipByXAxis;
            toAnnotation.ClipByYAxis = fromAnnotation.ClipByYAxis;
            toAnnotation.Tag = fromAnnotation.Tag;
            toAnnotation.ToolTip = fromAnnotation.ToolTip;
            toAnnotation.TextColor = fromAnnotation.TextColor;
            toAnnotation.Font = fromAnnotation.Font;
            toAnnotation.FontSize = fromAnnotation.FontSize;
            toAnnotation.FontWeight = fromAnnotation.FontWeight;
        }

        /// <summary>
        /// Creates a new axis of the same type with copied properties.
        /// </summary>
        /// <param name="axis">The axis to clone.</param>
        /// <returns>A new axis with the same properties.</returns>
        public static Axis CloneAxis(this Axis axis)
        {
            Axis newAxis = axis switch
            {
                LinearAxis => new LinearAxis(),
                LogarithmicAxis => new LogarithmicAxis(),
                DateTimeAxis => new DateTimeAxis(),
                TimeSpanAxis => new TimeSpanAxis(),
                CategoryAxis => new CategoryAxis(),
                AngleAxis => new AngleAxis(),
                MagnitudeAxis => new MagnitudeAxis(),
                LinearColorAxis => new LinearColorAxis(),
                LogarithmicColorAxis => new LogarithmicColorAxis(),
                RangeColorAxis => new RangeColorAxis(),
                NormalProbabilityAxis => new NormalProbabilityAxis(),
                GumbelProbabilityAxis => new GumbelProbabilityAxis(),
                _ => new LinearAxis()
            };

            newAxis.CopyFromAxis(axis);

            // Copy type-specific properties
            switch (axis)
            {
                case LinearAxis linearAxis when newAxis is LinearAxis newLinear:
                    newLinear.FormatAsFractions = linearAxis.FormatAsFractions;
                    newLinear.FractionUnit = linearAxis.FractionUnit;
                    newLinear.FractionUnitSymbol = linearAxis.FractionUnitSymbol;
                    break;
                case LogarithmicAxis logAxis when newAxis is LogarithmicAxis newLog:
                    newLog.Base = logAxis.Base;
                    newLog.PowerPadding = logAxis.PowerPadding;
                    break;
                case DateTimeAxis dateAxis when newAxis is DateTimeAxis newDate:
                    newDate.CalendarWeekRule = dateAxis.CalendarWeekRule;
                    newDate.FirstDayOfWeek = dateAxis.FirstDayOfWeek;
                    newDate.IntervalType = dateAxis.IntervalType;
                    newDate.MinorIntervalType = dateAxis.MinorIntervalType;
                    break;
                case CategoryAxis catAxis when newAxis is CategoryAxis newCat:
                    newCat.GapWidth = catAxis.GapWidth;
                    newCat.IsTickCentered = catAxis.IsTickCentered;
                    foreach (var label in catAxis.Labels)
                        newCat.Labels.Add(label);
                    if (catAxis.ItemsSource != null)
                        newCat.ItemsSource = catAxis.ItemsSource;
                    break;
            }

            return newAxis;
        }

        /// <summary>
        /// Gets the friendly name for an axis type.
        /// </summary>
        /// <param name="axis">The axis to get the name for.</param>
        /// <returns>A human-readable name for the axis type.</returns>
        public static string GetAxisTypeName(this Axis axis)
        {
            return axis switch
            {
                LinearAxis => "Linear",
                LogarithmicAxis => "Logarithmic",
                DateTimeAxis => "Date/Time",
                TimeSpanAxis => "Time Span",
                CategoryAxis => "Category",
                AngleAxis => "Angle",
                MagnitudeAxis => "Magnitude",
                LinearColorAxis => "Linear Color",
                LogarithmicColorAxis => "Logarithmic Color",
                RangeColorAxis => "Range Color",
                NormalProbabilityAxis => "Normal Probability",
                GumbelProbabilityAxis => "Gumbel Probability",
                _ => axis.GetType().Name
            };
        }

        /// <summary>
        /// Gets the friendly name for a series type.
        /// </summary>
        /// <param name="series">The series to get the name for.</param>
        /// <returns>A human-readable name for the series type.</returns>
        public static string GetSeriesTypeName(this Series series)
        {
            return series switch
            {
                LineSeries => "Line",
                ScatterSeries => "Scatter",
                AreaSeries => "Area",
                BarSeries => "Bar",
                ColumnSeries => "Column",
                BoxPlotSeries => "Box Plot",
                PieSeries => "Pie",
                StemSeries => "Stem",
                StairStepSeries => "Stair Step",
                TwoColorLineSeries => "Two Color Line",
                TwoColorAreaSeries => "Two Color Area",
                LinearBarSeries => "Linear Bar",
                RectangleBarSeries => "Rectangle Bar",
                CandleStickSeries => "Candle Stick",
                HighLowSeries => "High Low",
                ContourSeries => "Contour",
                HeatMapSeries => "Heat Map",
                ScatterErrorSeries => "Scatter Error",
                _ => series.GetType().Name
            };
        }

        /// <summary>
        /// Gets the friendly name for an annotation type.
        /// </summary>
        /// <param name="annotation">The annotation to get the name for.</param>
        /// <returns>A human-readable name for the annotation type.</returns>
        public static string GetAnnotationTypeName(this Annotation annotation)
        {
            return annotation switch
            {
                LineAnnotation => "Line",
                RectangleAnnotation => "Rectangle",
                EllipseAnnotation => "Ellipse",
                PolygonAnnotation => "Polygon",
                PolylineAnnotation => "Polyline",
                TextAnnotation => "Text",
                ArrowAnnotation => "Arrow",
                PointAnnotation => "Point",
                ImageAnnotation => "Image",
                FunctionAnnotation => "Function",
                _ => annotation.GetType().Name
            };
        }
    }

    /// <summary>
    /// Converts binding expressions to markup extensions for serialization.
    /// </summary>
    public class BindingConvertor : ExpressionConverter
    {
        /// <summary>
        /// Determines whether this converter can convert to the specified type.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>True if the destination type is MarkupExtension; otherwise, false.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(MarkupExtension);
        }

        /// <summary>
        /// Converts a binding expression to a markup extension.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="culture">The culture info.</param>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>The parent binding of the binding expression.</returns>
        /// <exception cref="InvalidOperationException">Thrown when value is not a BindingExpression.</exception>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(MarkupExtension))
            {
                if (value is not BindingExpression bindingExpression)
                    throw new InvalidOperationException("Value must be a BindingExpression");
                return bindingExpression.ParentBinding;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// Provides helper methods for registering type converters with the type descriptor system.
    /// </summary>
    public static class EditorHelper
    {
        /// <summary>
        /// Registers a type converter for a specific type.
        /// </summary>
        /// <typeparam name="T">The type to register the converter for.</typeparam>
        /// <typeparam name="TC">The type converter to register.</typeparam>
        public static void Register<T, TC>()
        {
            Attribute[] attr = [new TypeConverterAttribute(typeof(TC))];
            TypeDescriptor.AddAttributes(typeof(T), attr);
        }
    }
}
