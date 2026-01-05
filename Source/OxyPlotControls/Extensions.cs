using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml.Linq;
using OxyPlot;
using Wpf = OxyPlot.Wpf;

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
        public static Type GetFirstAbstractBaseType(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            Type baseType = type.BaseType;
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
            var svStringSplit = svString.Split(new[] { ", " }, StringSplitOptions.None);
            if (svStringSplit.Length != 2) return default(ScreenVector);

            double x, y;
            if (double.TryParse(svStringSplit[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x) == false) return default(ScreenVector);
            if (double.TryParse(svStringSplit[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y) == false) return default(ScreenVector);
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
            var dpStringSplit = dpString.Split(new[] { ", " }, StringSplitOptions.None);
            if (dpStringSplit.Length != 2) return DataPoint.Undefined;

            double x, y;
            if (double.TryParse(dpStringSplit[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x) == false) return DataPoint.Undefined;
            if (double.TryParse(dpStringSplit[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y) == false) return DataPoint.Undefined;
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

            double x, y;
            if (double.TryParse(dpElement.Attribute("X").Value, NumberStyles.Any, CultureInfo.InvariantCulture, out x) == false) return DataPoint.Undefined;
            if (double.TryParse(dpElement.Attribute("Y").Value, NumberStyles.Any, CultureInfo.InvariantCulture, out y) == false) return DataPoint.Undefined;
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
            var spStringSplit = spString.Split(new[] { ", " }, StringSplitOptions.None);
            if (spStringSplit.Length != 2) return ScreenPoint.Undefined;

            double x, y;
            if (double.TryParse(spStringSplit[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x) == false) return ScreenPoint.Undefined;
            if (double.TryParse(spStringSplit[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y) == false) return ScreenPoint.Undefined;
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
            var vStringSplit = vString.Split(new[] { ", " }, StringSplitOptions.None);
            if (vStringSplit.Length != 2) return default(Vector);

            double x, y;
            if (double.TryParse(vStringSplit[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x) == false) return default(Vector);
            if (double.TryParse(vStringSplit[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y) == false) return default(Vector);
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
            if (dpelements.Name == "DataPoints") // Backwards Compatibility
            {
                foreach (var dp in dpelements.Elements("DataPoint"))
                {
                    dpList.Add(dp.PointFromXElement());
                }
            }
            else if (dpelements.Name == "Points")
            {
                foreach (var dp in dpelements.Elements("DataPoint"))
                {
                    dpList.Add(dp.PointFromXElement());
                }
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
        /// Copies axis properties from one axis to another, respecting bindings.
        /// </summary>
        /// <param name="toAxis">The target axis to copy properties to.</param>
        /// <param name="fromAxis">The source axis to copy properties from.</param>
        /// <param name="ignoreToAxisBound">If true, skip properties that already have bindings on the target axis.</param>
        public static void FromAxisProperties(this Wpf.Axis toAxis, Wpf.Axis fromAxis, bool ignoreToAxisBound = true)
        {
            if (ignoreToAxisBound)
            {
                if (toAxis.IsBound(Wpf.Axis.AbsoluteMaximumProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.AbsoluteMaximumProperty) == false) toAxis.AbsoluteMaximum = fromAxis.AbsoluteMaximum;
                if (toAxis.IsBound(Wpf.Axis.AbsoluteMinimumProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.AbsoluteMinimumProperty) == false) toAxis.AbsoluteMinimum = fromAxis.AbsoluteMinimum;
                if (toAxis.IsBound(Wpf.Axis.AngleProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.AngleProperty) == false) toAxis.Angle = fromAxis.Angle;
                if (toAxis.IsBound(Wpf.Axis.AxisDistanceProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.AxisDistanceProperty) == false) toAxis.AxisDistance = fromAxis.AxisDistance;
                if (toAxis.IsBound(Wpf.Axis.AxislineColorProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.AxislineColorProperty) == false) toAxis.AxislineColor = fromAxis.AxislineColor;
                if (toAxis.IsBound(Wpf.Axis.AxislineStyleProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.AxislineStyleProperty) == false) toAxis.AxislineStyle = fromAxis.AxislineStyle;
                if (toAxis.IsBound(Wpf.Axis.AxislineThicknessProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.AxislineThicknessProperty) == false) toAxis.AxislineThickness = fromAxis.AxislineThickness;
                if (toAxis.IsBound(Wpf.Axis.AxisTitleDistanceProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.AxisTitleDistanceProperty) == false) toAxis.AxisTitleDistance = fromAxis.AxisTitleDistance;
                if (toAxis.IsBound(Wpf.Axis.AxisTickToLabelDistanceProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.AxisTickToLabelDistanceProperty) == false) toAxis.AxisTickToLabelDistance = fromAxis.AxisTickToLabelDistance;
                if (toAxis.IsBound(Wpf.Axis.ClipTitleProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.ClipTitleProperty) == false) toAxis.ClipTitle = fromAxis.ClipTitle;
                if (toAxis.IsBound(Wpf.Axis.EndPositionProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.EndPositionProperty) == false) toAxis.EndPosition = fromAxis.EndPosition;
                if (toAxis.IsBound(Wpf.Axis.ExtraGridlineColorProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.ExtraGridlineColorProperty) == false) toAxis.ExtraGridlineColor = fromAxis.ExtraGridlineColor;
                if (toAxis.IsBound(Wpf.Axis.ExtraGridlineStyleProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.ExtraGridlineStyleProperty) == false) toAxis.ExtraGridlineStyle = fromAxis.ExtraGridlineStyle;
                if (toAxis.IsBound(Wpf.Axis.ExtraGridlineThicknessProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.ExtraGridlineThicknessProperty) == false) toAxis.ExtraGridlineThickness = fromAxis.ExtraGridlineThickness;
                if (toAxis.IsBound(Wpf.Axis.ExtraGridlinesProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.ExtraGridlinesProperty) == false) toAxis.ExtraGridlines = fromAxis.ExtraGridlines;
                if (toAxis.IsBound(Wpf.Axis.FilterFunctionProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.FilterFunctionProperty) == false) toAxis.FilterFunction = fromAxis.FilterFunction;
                if (toAxis.IsBound(Wpf.Axis.FilterMaxValueProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.FilterMaxValueProperty) == false) toAxis.FilterMaxValue = fromAxis.FilterMaxValue;
                if (toAxis.IsBound(Wpf.Axis.FilterMinValueProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.FilterMinValueProperty) == false) toAxis.FilterMinValue = fromAxis.FilterMinValue;
                if (toAxis.IsBound(Wpf.Axis.FontProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.FontProperty) == false) toAxis.Font = fromAxis.Font;
                if (toAxis.IsBound(Wpf.Axis.FontSizeProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.FontSizeProperty) == false) toAxis.FontSize = fromAxis.FontSize;
                if (toAxis.IsBound(Wpf.Axis.FontWeightProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.FontWeightProperty) == false) toAxis.FontWeight = fromAxis.FontWeight;
                if (toAxis.IsBound(Wpf.Axis.IntervalLengthProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.IntervalLengthProperty) == false) toAxis.IntervalLength = fromAxis.IntervalLength;
                if (toAxis.IsBound(Wpf.Axis.IsPanEnabledProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.IsPanEnabledProperty) == false) toAxis.IsPanEnabled = fromAxis.IsPanEnabled;
                if (toAxis.IsBound(Wpf.Axis.IsAxisVisibleProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.IsAxisVisibleProperty) == false) toAxis.IsAxisVisible = fromAxis.IsAxisVisible;
                if (toAxis.IsBound(Wpf.Axis.IsZoomEnabledProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.IsZoomEnabledProperty) == false) toAxis.IsZoomEnabled = fromAxis.IsZoomEnabled;
                if (toAxis.IsBound(Wpf.Axis.KeyProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.KeyProperty) == false) toAxis.Key = fromAxis.Key;
                if (toAxis.IsBound(Wpf.Axis.LayerProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.LayerProperty) == false) toAxis.Layer = fromAxis.Layer;
                if (toAxis.IsBound(Wpf.Axis.MajorGridlineColorProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MajorGridlineColorProperty) == false) toAxis.MajorGridlineColor = fromAxis.MajorGridlineColor;
                if (toAxis.IsBound(Wpf.Axis.MinorGridlineColorProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MinorGridlineColorProperty) == false) toAxis.MinorGridlineColor = fromAxis.MinorGridlineColor;
                if (toAxis.IsBound(Wpf.Axis.MajorGridlineStyleProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MajorGridlineStyleProperty) == false) toAxis.MajorGridlineStyle = fromAxis.MajorGridlineStyle;
                if (toAxis.IsBound(Wpf.Axis.MinorGridlineStyleProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MinorGridlineStyleProperty) == false) toAxis.MinorGridlineStyle = fromAxis.MinorGridlineStyle;
                if (toAxis.IsBound(Wpf.Axis.MajorGridlineThicknessProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MajorGridlineThicknessProperty) == false) toAxis.MajorGridlineThickness = fromAxis.MajorGridlineThickness;
                if (toAxis.IsBound(Wpf.Axis.MinorGridlineThicknessProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MinorGridlineThicknessProperty) == false) toAxis.MinorGridlineThickness = fromAxis.MinorGridlineThickness;
                if (toAxis.IsBound(Wpf.Axis.MajorStepProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MajorStepProperty) == false) toAxis.MajorStep = fromAxis.MajorStep;
                if (toAxis.IsBound(Wpf.Axis.MajorTickSizeProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MajorTickSizeProperty) == false) toAxis.MajorTickSize = fromAxis.MajorTickSize;
                if (toAxis.IsBound(Wpf.Axis.MinorStepProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MinorStepProperty) == false) toAxis.MinorStep = fromAxis.MinorStep;
                if (toAxis.IsBound(Wpf.Axis.MinorTickSizeProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MinorTickSizeProperty) == false) toAxis.MinorTickSize = fromAxis.MinorTickSize;
                if (toAxis.IsBound(Wpf.Axis.MinimumProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MinimumProperty) == false) toAxis.Minimum = fromAxis.Minimum;
                if (toAxis.IsBound(Wpf.Axis.MaximumProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MaximumProperty) == false) toAxis.Maximum = fromAxis.Maximum;
                if (toAxis.IsBound(Wpf.Axis.MinimumRangeProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MinimumRangeProperty) == false) toAxis.MinimumRange = fromAxis.MinimumRange;
                if (toAxis.IsBound(Wpf.Axis.MaximumRangeProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MaximumRangeProperty) == false) toAxis.MaximumRange = fromAxis.MaximumRange;
                if (toAxis.IsBound(Wpf.Axis.MinimumPaddingProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MinimumPaddingProperty) == false) toAxis.MinimumPadding = fromAxis.MinimumPadding;
                if (toAxis.IsBound(Wpf.Axis.MaximumPaddingProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.MaximumPaddingProperty) == false) toAxis.MaximumPadding = fromAxis.MaximumPadding;
                if (toAxis.IsBound(Wpf.Axis.PositionProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.PositionProperty) == false) toAxis.Position = fromAxis.Position;
                if (toAxis.IsBound(Wpf.Axis.PositionTierProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.PositionTierProperty) == false) toAxis.PositionTier = fromAxis.PositionTier;
                if (toAxis.IsBound(Wpf.Axis.PositionAtZeroCrossingProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.PositionAtZeroCrossingProperty) == false) toAxis.PositionAtZeroCrossing = fromAxis.PositionAtZeroCrossing;
                if (toAxis.IsBound(Wpf.Axis.StartPositionProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.StartPositionProperty) == false) toAxis.StartPosition = fromAxis.StartPosition;
                if (toAxis.IsBound(Wpf.Axis.StringFormatProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.StringFormatProperty) == false) toAxis.StringFormat = fromAxis.StringFormat;
                if (toAxis.IsBound(Wpf.Axis.TextColorProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TextColorProperty) == false) toAxis.TextColor = fromAxis.TextColor;
                if (toAxis.IsBound(Wpf.Axis.TicklineColorProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TicklineColorProperty) == false) toAxis.TicklineColor = fromAxis.TicklineColor;
                if (toAxis.IsBound(Wpf.Axis.TitleClippingLengthProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleClippingLengthProperty) == false) toAxis.TitleClippingLength = fromAxis.TitleClippingLength;
                if (toAxis.IsBound(Wpf.Axis.TitleColorProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleColorProperty) == false) toAxis.TitleColor = fromAxis.TitleColor;
                if (toAxis.IsBound(Wpf.Axis.TitleFontProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleFontProperty) == false) toAxis.TitleFont = fromAxis.TitleFont;
                if (toAxis.IsBound(Wpf.Axis.TitleFontSizeProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleFontSizeProperty) == false) toAxis.TitleFontSize = fromAxis.TitleFontSize;
                if (toAxis.IsBound(Wpf.Axis.TitleFontWeightProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleFontWeightProperty) == false) toAxis.TitleFontWeight = fromAxis.TitleFontWeight;
                if (toAxis.IsBound(Wpf.Axis.TitleFormatStringProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleFormatStringProperty) == false) toAxis.TitleFormatString = fromAxis.TitleFormatString;
                if (toAxis.IsBound(Wpf.Axis.TitleProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleProperty) == false) toAxis.Title = fromAxis.Title;
                if (toAxis.IsBound(Wpf.Axis.ToolTipProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.ToolTipProperty) == false) toAxis.ToolTip = fromAxis.ToolTip == null ? null : fromAxis.ToolTip.ToString();
                if (toAxis.IsBound(Wpf.Axis.TickStyleProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TickStyleProperty) == false) toAxis.TickStyle = fromAxis.TickStyle;
                if (toAxis.IsBound(Wpf.Axis.TitlePositionProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.TitlePositionProperty) == false) toAxis.TitlePosition = fromAxis.TitlePosition;
                if (toAxis.IsBound(Wpf.Axis.UnitProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.UnitProperty) == false) toAxis.Unit = fromAxis.Unit;
                if (toAxis.IsBound(Wpf.Axis.UseSuperExponentialFormatProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.UseSuperExponentialFormatProperty) == false) toAxis.UseSuperExponentialFormat = fromAxis.UseSuperExponentialFormat;
                if (toAxis.IsBound(Wpf.Axis.LabelFormatterProperty) == false && fromAxis.CopyBinding(toAxis, Wpf.Axis.LabelFormatterProperty) == false) toAxis.LabelFormatter = fromAxis.LabelFormatter;
            }
            else
            {
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.AbsoluteMaximumProperty) == false) toAxis.AbsoluteMaximum = fromAxis.AbsoluteMaximum;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.AbsoluteMinimumProperty) == false) toAxis.AbsoluteMinimum = fromAxis.AbsoluteMinimum;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.AngleProperty) == false) toAxis.Angle = fromAxis.Angle;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.AxisDistanceProperty) == false) toAxis.AxisDistance = fromAxis.AxisDistance;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.AxislineColorProperty) == false) toAxis.AxislineColor = fromAxis.AxislineColor;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.AxislineStyleProperty) == false) toAxis.AxislineStyle = fromAxis.AxislineStyle;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.AxislineThicknessProperty) == false) toAxis.AxislineThickness = fromAxis.AxislineThickness;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.AxisTitleDistanceProperty) == false) toAxis.AxisTitleDistance = fromAxis.AxisTitleDistance;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.AxisTickToLabelDistanceProperty) == false) toAxis.AxisTickToLabelDistance = fromAxis.AxisTickToLabelDistance;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.ClipTitleProperty) == false) toAxis.ClipTitle = fromAxis.ClipTitle;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.EndPositionProperty) == false) toAxis.EndPosition = fromAxis.EndPosition;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.ExtraGridlineColorProperty) == false) toAxis.ExtraGridlineColor = fromAxis.ExtraGridlineColor;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.ExtraGridlineStyleProperty) == false) toAxis.ExtraGridlineStyle = fromAxis.ExtraGridlineStyle;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.ExtraGridlineThicknessProperty) == false) toAxis.ExtraGridlineThickness = fromAxis.ExtraGridlineThickness;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.ExtraGridlinesProperty) == false) toAxis.ExtraGridlines = fromAxis.ExtraGridlines;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.FilterFunctionProperty) == false) toAxis.FilterFunction = fromAxis.FilterFunction;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.FilterMaxValueProperty) == false) toAxis.FilterMaxValue = fromAxis.FilterMaxValue;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.FilterMinValueProperty) == false) toAxis.FilterMinValue = fromAxis.FilterMinValue;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.FontProperty) == false) toAxis.Font = fromAxis.Font;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.FontSizeProperty) == false) toAxis.FontSize = fromAxis.FontSize;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.FontWeightProperty) == false) toAxis.FontWeight = fromAxis.FontWeight;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.IntervalLengthProperty) == false) toAxis.IntervalLength = fromAxis.IntervalLength;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.IsPanEnabledProperty) == false) toAxis.IsPanEnabled = fromAxis.IsPanEnabled;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.IsAxisVisibleProperty) == false) toAxis.IsAxisVisible = fromAxis.IsAxisVisible;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.IsZoomEnabledProperty) == false) toAxis.IsZoomEnabled = fromAxis.IsZoomEnabled;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.KeyProperty) == false) toAxis.Key = fromAxis.Key;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.LayerProperty) == false) toAxis.Layer = fromAxis.Layer;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MajorGridlineColorProperty) == false) toAxis.MajorGridlineColor = fromAxis.MajorGridlineColor;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MinorGridlineColorProperty) == false) toAxis.MinorGridlineColor = fromAxis.MinorGridlineColor;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MajorGridlineStyleProperty) == false) toAxis.MajorGridlineStyle = fromAxis.MajorGridlineStyle;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MinorGridlineStyleProperty) == false) toAxis.MinorGridlineStyle = fromAxis.MinorGridlineStyle;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MajorGridlineThicknessProperty) == false) toAxis.MajorGridlineThickness = fromAxis.MajorGridlineThickness;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MinorGridlineThicknessProperty) == false) toAxis.MinorGridlineThickness = fromAxis.MinorGridlineThickness;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MajorStepProperty) == false) toAxis.MajorStep = fromAxis.MajorStep;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MajorTickSizeProperty) == false) toAxis.MajorTickSize = fromAxis.MajorTickSize;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MinorStepProperty) == false) toAxis.MinorStep = fromAxis.MinorStep;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MinorTickSizeProperty) == false) toAxis.MinorTickSize = fromAxis.MinorTickSize;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MinimumProperty) == false) toAxis.Minimum = fromAxis.Minimum;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MaximumProperty) == false) toAxis.Maximum = fromAxis.Maximum;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MinimumRangeProperty) == false) toAxis.MinimumRange = fromAxis.MinimumRange;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MaximumRangeProperty) == false) toAxis.MaximumRange = fromAxis.MaximumRange;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MinimumPaddingProperty) == false) toAxis.MinimumPadding = fromAxis.MinimumPadding;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.MaximumPaddingProperty) == false) toAxis.MaximumPadding = fromAxis.MaximumPadding;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.PositionProperty) == false) toAxis.Position = fromAxis.Position;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.PositionTierProperty) == false) toAxis.PositionTier = fromAxis.PositionTier;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.PositionAtZeroCrossingProperty) == false) toAxis.PositionAtZeroCrossing = fromAxis.PositionAtZeroCrossing;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.StartPositionProperty) == false) toAxis.StartPosition = fromAxis.StartPosition;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.StringFormatProperty) == false) toAxis.StringFormat = fromAxis.StringFormat;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TextColorProperty) == false) toAxis.TextColor = fromAxis.TextColor;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TicklineColorProperty) == false) toAxis.TicklineColor = fromAxis.TicklineColor;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleClippingLengthProperty) == false) toAxis.TitleClippingLength = fromAxis.TitleClippingLength;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleColorProperty) == false) toAxis.TitleColor = fromAxis.TitleColor;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleFontProperty) == false) toAxis.TitleFont = fromAxis.TitleFont;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleFontSizeProperty) == false) toAxis.TitleFontSize = fromAxis.TitleFontSize;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleFontWeightProperty) == false) toAxis.TitleFontWeight = fromAxis.TitleFontWeight;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleFormatStringProperty) == false) toAxis.TitleFormatString = fromAxis.TitleFormatString;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TitleProperty) == false) toAxis.Title = fromAxis.Title;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.ToolTipProperty) == false) toAxis.ToolTip = fromAxis.ToolTip == null ? null : fromAxis.ToolTip.ToString();
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TickStyleProperty) == false) toAxis.TickStyle = fromAxis.TickStyle;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.TitlePositionProperty) == false) toAxis.TitlePosition = fromAxis.TitlePosition;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.UnitProperty) == false) toAxis.Unit = fromAxis.Unit;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.UseSuperExponentialFormatProperty) == false) toAxis.UseSuperExponentialFormat = fromAxis.UseSuperExponentialFormat;
                if (fromAxis.CopyBinding(toAxis, Wpf.Axis.LabelFormatterProperty) == false) toAxis.LabelFormatter = fromAxis.LabelFormatter;
            }
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
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(MarkupExtension))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Converts a binding expression to a markup extension.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="culture">The culture info.</param>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>The parent binding of the binding expression.</returns>
        /// <exception cref="Exception">Thrown when value is not a BindingExpression.</exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(MarkupExtension))
            {
                BindingExpression bindingExpression = value as BindingExpression;
                if (bindingExpression == null) throw new Exception();
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
            Attribute[] attr = new Attribute[1];
            TypeConverterAttribute vConv = new TypeConverterAttribute(typeof(TC));
            attr[0] = vConv;
            TypeDescriptor.AddAttributes(typeof(T), attr);
        }
    }
}
