Imports System.ComponentModel
Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Windows.Markup
Imports OxyPlot

Module ExtensionsModule

    <Extension()>
    Function GetFirstAbstractBaseType(ByVal type As Type) As Type
        If type Is Nothing Then Throw New ArgumentNullException("type")
        '
        Dim baseType As Type = type.BaseType
        If baseType Is Nothing OrElse baseType.IsAbstract Then Return baseType
        '
        Return baseType.GetFirstAbstractBaseType()
    End Function

    <Extension()>
    Function ToPrettyText(sv As ScreenVector) As String
        Return sv.X.ToString("G17", CultureInfo.InvariantCulture) & ", " & sv.Y.ToString("G17", CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function FromPrettyVectorText(svString As String) As ScreenVector
        Dim svStringSplit = Split(svString, ", ")
        If svStringSplit.Count <> 2 Then Return Nothing
        '
        Dim x, y As Double
        If Double.TryParse(svStringSplit(0), NumberStyles.Any, CultureInfo.InvariantCulture, x) = False Then Return Nothing
        If Double.TryParse(svStringSplit(1), NumberStyles.Any, CultureInfo.InvariantCulture, y) = False Then Return Nothing
        Return New ScreenVector(x, y)
    End Function

    <Extension()>
    Function ToPrettyText(dp As DataPoint) As String
        Return dp.X.ToString("G17", CultureInfo.InvariantCulture) & ", " & dp.Y.ToString("G17", CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function FromPrettyDataText(dpString As String) As DataPoint
        Dim dpStringSplit = Split(dpString, ", ")
        If dpStringSplit.Count <> 2 Then Return DataPoint.Undefined
        '
        Dim x, y As Double
        If Double.TryParse(dpStringSplit(0), NumberStyles.Any, CultureInfo.InvariantCulture, x) = False Then Return DataPoint.Undefined
        If Double.TryParse(dpStringSplit(1), NumberStyles.Any, CultureInfo.InvariantCulture, y) = False Then Return DataPoint.Undefined
        Return New DataPoint(x, y)
    End Function

    <Extension()>
    Function ToXElement(dp As DataPoint) As XElement
        Dim dpElement = New XElement("DataPoint")
        dpElement.SetAttributeValue("X", dp.X.ToString("G17", CultureInfo.InvariantCulture))
        dpElement.SetAttributeValue("Y", dp.Y.ToString("G17", CultureInfo.InvariantCulture))
        '
        Return dpElement
    End Function

    <Extension()>
    Function PointFromXElement(dpElement As XElement) As DataPoint
        If dpElement.Name <> "DataPoint" Then Return DataPoint.Undefined
        If IsNothing(dpElement.Attribute("X")) Then Return DataPoint.Undefined
        If IsNothing(dpElement.Attribute("Y")) Then Return DataPoint.Undefined
        '
        Dim x, y As Double
        If Double.TryParse(dpElement.Attribute("X").Value, NumberStyles.Any, CultureInfo.InvariantCulture, x) = False Then Return DataPoint.Undefined
        If Double.TryParse(dpElement.Attribute("Y").Value, NumberStyles.Any, CultureInfo.InvariantCulture, y) = False Then Return DataPoint.Undefined
        Return New DataPoint(x, y)
    End Function


    <Extension()>
    Function ToPrettyText(sp As ScreenPoint) As String
        Return sp.X.ToString("G17", CultureInfo.InvariantCulture) & ", " & sp.Y.ToString("G17", CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function FromPrettyScreenText(spString As String) As ScreenPoint
        Dim spStringSplit = Split(spString, ", ")
        If spStringSplit.Count <> 2 Then Return ScreenPoint.Undefined
        '
        Dim x, y As Double
        If Double.TryParse(spStringSplit(0), NumberStyles.Any, CultureInfo.InvariantCulture, x) = False Then Return ScreenPoint.Undefined
        If Double.TryParse(spStringSplit(1), NumberStyles.Any, CultureInfo.InvariantCulture, y) = False Then Return ScreenPoint.Undefined
        Return New ScreenPoint(x, y)
    End Function

    <Extension()>
    Function ToPrettyText(v As Vector) As String
        Return v.X.ToString("G17", CultureInfo.InvariantCulture) & ", " & v.Y.ToString("G17", CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function FromPrettyVectorString(vString As String) As Vector
        Dim vStringSplit = Split(vString, ", ")
        If vStringSplit.Count <> 2 Then Return Nothing
        '
        Dim x, y As Double
        If Double.TryParse(vStringSplit(0), NumberStyles.Any, CultureInfo.InvariantCulture, x) = False Then Return Nothing
        If Double.TryParse(vStringSplit(1), NumberStyles.Any, CultureInfo.InvariantCulture, y) = False Then Return Nothing
        Return New Vector(x, y)
    End Function

    <Extension()>
    Function ToXElement(dataPoints As IList(Of DataPoint), name As String) As XElement
        Dim el As New XElement(name)
        '
        For Each dp In dataPoints
            el.Add(dp.ToXElement())
        Next
        '
        Return el
    End Function

    <Extension()>
    Function PointsFromXElement(dpelements As XElement) As IList(Of DataPoint)
        If dpelements.Name <> "DataPoints" AndAlso dpelements.Name <> "Points" Then Return New List(Of DataPoint)
        '
        Dim dpList As New List(Of DataPoint)
        If dpelements.Name = "DataPoints" Then ' Backwards Compatibility
            For Each dp In dpelements.Elements("DataPoint")
                dpList.Add(dp.PointFromXElement())
            Next
        ElseIf dpelements.Name = "Points" Then
            For Each dp In dpelements.Elements("DataPoint")
                dpList.Add(dp.PointFromXElement())
            Next
        End If
        '
        Return dpList
    End Function

    <Extension()>
    Function CopyBinding(fromTarget As DependencyObject, toTarget As DependencyObject, dp As DependencyProperty) As Boolean
        Dim te = BindingOperations.GetBinding(fromTarget, dp)
        If IsNothing(te) Then Return False
        BindingOperations.SetBinding(toTarget, dp, te)
        Return True
    End Function

    <Extension()>
    Public Function IsBound(target As DependencyObject, dp As DependencyProperty) As Boolean
        Return Not IsNothing(BindingOperations.GetBinding(target, dp))
    End Function

    <Extension()>
    Sub FromAxisProperties(toAxis As Wpf.Axis, fromAxis As Wpf.Axis, Optional ignoreToAxisBound As Boolean = True)
        If ignoreToAxisBound Then
            With toAxis
                If toAxis.IsBound(Wpf.Axis.AbsoluteMaximumProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.AbsoluteMaximumProperty) = False Then .AbsoluteMaximum = fromAxis.AbsoluteMaximum
                If toAxis.IsBound(Wpf.Axis.AbsoluteMinimumProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.AbsoluteMinimumProperty) = False Then .AbsoluteMinimum = fromAxis.AbsoluteMinimum
                If toAxis.IsBound(Wpf.Axis.AngleProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.AngleProperty) = False Then .Angle = fromAxis.Angle
                If toAxis.IsBound(Wpf.Axis.AxisDistanceProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.AxisDistanceProperty) = False Then .AxisDistance = fromAxis.AxisDistance
                If toAxis.IsBound(Wpf.Axis.AxislineColorProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.AxislineColorProperty) = False Then .AxislineColor = fromAxis.AxislineColor
                If toAxis.IsBound(Wpf.Axis.AxislineStyleProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.AxislineStyleProperty) = False Then .AxislineStyle = fromAxis.AxislineStyle
                If toAxis.IsBound(Wpf.Axis.AxislineThicknessProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.AxislineThicknessProperty) = False Then .AxislineThickness = fromAxis.AxislineThickness
                If toAxis.IsBound(Wpf.Axis.AxisTitleDistanceProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.AxisTitleDistanceProperty) = False Then .AxisTitleDistance = fromAxis.AxisTitleDistance
                If toAxis.IsBound(Wpf.Axis.AxisTickToLabelDistanceProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.AxisTickToLabelDistanceProperty) = False Then .AxisTickToLabelDistance = fromAxis.AxisTickToLabelDistance
                If toAxis.IsBound(Wpf.Axis.ClipTitleProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.ClipTitleProperty) = False Then .ClipTitle = fromAxis.ClipTitle
                If toAxis.IsBound(Wpf.Axis.EndPositionProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.EndPositionProperty) = False Then .EndPosition = fromAxis.EndPosition
                If toAxis.IsBound(Wpf.Axis.ExtraGridlineColorProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.ExtraGridlineColorProperty) = False Then .ExtraGridlineColor = fromAxis.ExtraGridlineColor
                If toAxis.IsBound(Wpf.Axis.ExtraGridlineStyleProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.ExtraGridlineStyleProperty) = False Then .ExtraGridlineStyle = fromAxis.ExtraGridlineStyle
                If toAxis.IsBound(Wpf.Axis.ExtraGridlineThicknessProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.ExtraGridlineThicknessProperty) = False Then .ExtraGridlineThickness = fromAxis.ExtraGridlineThickness
                If toAxis.IsBound(Wpf.Axis.ExtraGridlinesProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.ExtraGridlinesProperty) = False Then .ExtraGridlines = fromAxis.ExtraGridlines
                If toAxis.IsBound(Wpf.Axis.FilterFunctionProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.FilterFunctionProperty) = False Then .FilterFunction = fromAxis.FilterFunction
                If toAxis.IsBound(Wpf.Axis.FilterMaxValueProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.FilterMaxValueProperty) = False Then .FilterMaxValue = fromAxis.FilterMaxValue
                If toAxis.IsBound(Wpf.Axis.FilterMinValueProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.FilterMinValueProperty) = False Then .FilterMinValue = fromAxis.FilterMinValue
                If toAxis.IsBound(Wpf.Axis.FontProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.FontProperty) = False Then .Font = fromAxis.Font
                If toAxis.IsBound(Wpf.Axis.FontSizeProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.FontSizeProperty) = False Then .FontSize = fromAxis.FontSize
                If toAxis.IsBound(Wpf.Axis.FontWeightProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.FontWeightProperty) = False Then .FontWeight = fromAxis.FontWeight
                If toAxis.IsBound(Wpf.Axis.IntervalLengthProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.IntervalLengthProperty) = False Then .IntervalLength = fromAxis.IntervalLength
                If toAxis.IsBound(Wpf.Axis.IsPanEnabledProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.IsPanEnabledProperty) = False Then .IsPanEnabled = fromAxis.IsPanEnabled
                If toAxis.IsBound(Wpf.Axis.IsAxisVisibleProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.IsAxisVisibleProperty) = False Then .IsAxisVisible = fromAxis.IsAxisVisible
                If toAxis.IsBound(Wpf.Axis.IsZoomEnabledProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.IsZoomEnabledProperty) = False Then .IsZoomEnabled = fromAxis.IsZoomEnabled
                If toAxis.IsBound(Wpf.Axis.KeyProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.KeyProperty) = False Then .Key = fromAxis.Key
                If toAxis.IsBound(Wpf.Axis.LayerProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.LayerProperty) = False Then .Layer = fromAxis.Layer
                If toAxis.IsBound(Wpf.Axis.MajorGridlineColorProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MajorGridlineColorProperty) = False Then .MajorGridlineColor = fromAxis.MajorGridlineColor
                If toAxis.IsBound(Wpf.Axis.MinorGridlineColorProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MinorGridlineColorProperty) = False Then .MinorGridlineColor = fromAxis.MinorGridlineColor
                If toAxis.IsBound(Wpf.Axis.MajorGridlineStyleProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MajorGridlineStyleProperty) = False Then .MajorGridlineStyle = fromAxis.MajorGridlineStyle
                If toAxis.IsBound(Wpf.Axis.MinorGridlineStyleProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MinorGridlineStyleProperty) = False Then .MinorGridlineStyle = fromAxis.MinorGridlineStyle
                If toAxis.IsBound(Wpf.Axis.MajorGridlineThicknessProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MajorGridlineThicknessProperty) = False Then .MajorGridlineThickness = fromAxis.MajorGridlineThickness
                If toAxis.IsBound(Wpf.Axis.MinorGridlineThicknessProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MinorGridlineThicknessProperty) = False Then .MinorGridlineThickness = fromAxis.MinorGridlineThickness
                If toAxis.IsBound(Wpf.Axis.MajorStepProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MajorStepProperty) = False Then .MajorStep = fromAxis.MajorStep
                If toAxis.IsBound(Wpf.Axis.MajorTickSizeProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MajorTickSizeProperty) = False Then .MajorTickSize = fromAxis.MajorTickSize
                If toAxis.IsBound(Wpf.Axis.MinorStepProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MinorStepProperty) = False Then .MinorStep = fromAxis.MinorStep
                If toAxis.IsBound(Wpf.Axis.MinorTickSizeProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MinorTickSizeProperty) = False Then .MinorTickSize = fromAxis.MinorTickSize
                If toAxis.IsBound(Wpf.Axis.MinimumProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MinimumProperty) = False Then .Minimum = fromAxis.Minimum
                If toAxis.IsBound(Wpf.Axis.MaximumProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MaximumProperty) = False Then .Maximum = fromAxis.Maximum
                If toAxis.IsBound(Wpf.Axis.MinimumRangeProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MinimumRangeProperty) = False Then .MinimumRange = fromAxis.MinimumRange
                If toAxis.IsBound(Wpf.Axis.MaximumRangeProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MaximumRangeProperty) = False Then .MaximumRange = fromAxis.MaximumRange
                If toAxis.IsBound(Wpf.Axis.MinimumPaddingProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MinimumPaddingProperty) = False Then .MinimumPadding = fromAxis.MinimumPadding
                If toAxis.IsBound(Wpf.Axis.MaximumPaddingProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.MaximumPaddingProperty) = False Then .MaximumPadding = fromAxis.MaximumPadding
                If toAxis.IsBound(Wpf.Axis.PositionProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.PositionProperty) = False Then .Position = fromAxis.Position
                If toAxis.IsBound(Wpf.Axis.PositionTierProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.PositionTierProperty) = False Then .PositionTier = fromAxis.PositionTier
                If toAxis.IsBound(Wpf.Axis.PositionAtZeroCrossingProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.PositionAtZeroCrossingProperty) = False Then .PositionAtZeroCrossing = fromAxis.PositionAtZeroCrossing
                If toAxis.IsBound(Wpf.Axis.StartPositionProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.StartPositionProperty) = False Then .StartPosition = fromAxis.StartPosition
                If toAxis.IsBound(Wpf.Axis.StringFormatProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.StringFormatProperty) = False Then .StringFormat = fromAxis.StringFormat
                If toAxis.IsBound(Wpf.Axis.TextColorProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TextColorProperty) = False Then .TextColor = fromAxis.TextColor
                If toAxis.IsBound(Wpf.Axis.TicklineColorProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TicklineColorProperty) = False Then .TicklineColor = fromAxis.TicklineColor
                If toAxis.IsBound(Wpf.Axis.TitleClippingLengthProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleClippingLengthProperty) = False Then .TitleClippingLength = fromAxis.TitleClippingLength
                If toAxis.IsBound(Wpf.Axis.TitleColorProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleColorProperty) = False Then .TitleColor = fromAxis.TitleColor
                If toAxis.IsBound(Wpf.Axis.TitleFontProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleFontProperty) = False Then .TitleFont = fromAxis.TitleFont
                If toAxis.IsBound(Wpf.Axis.TitleFontSizeProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleFontSizeProperty) = False Then .TitleFontSize = fromAxis.TitleFontSize
                If toAxis.IsBound(Wpf.Axis.TitleFontWeightProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleFontWeightProperty) = False Then .TitleFontWeight = fromAxis.TitleFontWeight
                If toAxis.IsBound(Wpf.Axis.TitleFormatStringProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleFormatStringProperty) = False Then .TitleFormatString = fromAxis.TitleFormatString
                If toAxis.IsBound(Wpf.Axis.TitleProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleProperty) = False Then .Title = fromAxis.Title
                If toAxis.IsBound(Wpf.Axis.ToolTipProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.ToolTipProperty) = False Then .ToolTip = If(IsNothing(fromAxis.ToolTip), Nothing, fromAxis.ToolTip.ToString())
                If toAxis.IsBound(Wpf.Axis.TickStyleProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TickStyleProperty) = False Then .TickStyle = fromAxis.TickStyle
                If toAxis.IsBound(Wpf.Axis.TitlePositionProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TitlePositionProperty) = False Then .TitlePosition = fromAxis.TitlePosition
                If toAxis.IsBound(Wpf.Axis.UnitProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.UnitProperty) = False Then .Unit = fromAxis.Unit
                If toAxis.IsBound(Wpf.Axis.UseSuperExponentialFormatProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.UseSuperExponentialFormatProperty) = False Then .UseSuperExponentialFormat = fromAxis.UseSuperExponentialFormat
                If toAxis.IsBound(Wpf.Axis.LabelFormatterProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.LabelFormatterProperty) = False Then .LabelFormatter = fromAxis.LabelFormatter
                If toAxis.IsBound(Wpf.Axis.TickStyleProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TickStyleProperty) = False Then .TickStyle = fromAxis.TickStyle
                If toAxis.IsBound(Wpf.Axis.TitlePositionProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.TitlePositionProperty) = False Then .TitlePosition = fromAxis.TitlePosition
                If toAxis.IsBound(Wpf.Axis.UnitProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.UnitProperty) = False Then .Unit = fromAxis.Unit
                If toAxis.IsBound(Wpf.Axis.UseSuperExponentialFormatProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.UseSuperExponentialFormatProperty) = False Then .UseSuperExponentialFormat = fromAxis.UseSuperExponentialFormat
                If toAxis.IsBound(Wpf.Axis.LabelFormatterProperty) = False AndAlso CopyBinding(fromAxis, toAxis, Wpf.Axis.LabelFormatterProperty) = False Then .LabelFormatter = fromAxis.LabelFormatter
            End With
        Else
            With toAxis
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.AbsoluteMaximumProperty) = False Then .AbsoluteMaximum = fromAxis.AbsoluteMaximum
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.AbsoluteMinimumProperty) = False Then .AbsoluteMinimum = fromAxis.AbsoluteMinimum
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.AngleProperty) = False Then .Angle = fromAxis.Angle
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.AxisDistanceProperty) = False Then .AxisDistance = fromAxis.AxisDistance
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.AxislineColorProperty) = False Then .AxislineColor = fromAxis.AxislineColor
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.AxislineStyleProperty) = False Then .AxislineStyle = fromAxis.AxislineStyle
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.AxislineThicknessProperty) = False Then .AxislineThickness = fromAxis.AxislineThickness
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.AxisTitleDistanceProperty) = False Then .AxisTitleDistance = fromAxis.AxisTitleDistance
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.AxisTickToLabelDistanceProperty) = False Then .AxisTickToLabelDistance = fromAxis.AxisTickToLabelDistance
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.ClipTitleProperty) = False Then .ClipTitle = fromAxis.ClipTitle
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.EndPositionProperty) = False Then .EndPosition = fromAxis.EndPosition
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.ExtraGridlineColorProperty) = False Then .ExtraGridlineColor = fromAxis.ExtraGridlineColor
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.ExtraGridlineStyleProperty) = False Then .ExtraGridlineStyle = fromAxis.ExtraGridlineStyle
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.ExtraGridlineThicknessProperty) = False Then .ExtraGridlineThickness = fromAxis.ExtraGridlineThickness
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.ExtraGridlinesProperty) = False Then .ExtraGridlines = fromAxis.ExtraGridlines
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.FilterFunctionProperty) = False Then .FilterFunction = fromAxis.FilterFunction
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.FilterMaxValueProperty) = False Then .FilterMaxValue = fromAxis.FilterMaxValue
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.FilterMinValueProperty) = False Then .FilterMinValue = fromAxis.FilterMinValue
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.FontProperty) = False Then .Font = fromAxis.Font
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.FontSizeProperty) = False Then .FontSize = fromAxis.FontSize
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.FontWeightProperty) = False Then .FontWeight = fromAxis.FontWeight
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.IntervalLengthProperty) = False Then .IntervalLength = fromAxis.IntervalLength
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.IsPanEnabledProperty) = False Then .IsPanEnabled = fromAxis.IsPanEnabled
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.IsAxisVisibleProperty) = False Then .IsAxisVisible = fromAxis.IsAxisVisible
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.IsZoomEnabledProperty) = False Then .IsZoomEnabled = fromAxis.IsZoomEnabled
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.KeyProperty) = False Then .Key = fromAxis.Key
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.LayerProperty) = False Then .Layer = fromAxis.Layer
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MajorGridlineColorProperty) = False Then .MajorGridlineColor = fromAxis.MajorGridlineColor
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MinorGridlineColorProperty) = False Then .MinorGridlineColor = fromAxis.MinorGridlineColor
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MajorGridlineStyleProperty) = False Then .MajorGridlineStyle = fromAxis.MajorGridlineStyle
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MinorGridlineStyleProperty) = False Then .MinorGridlineStyle = fromAxis.MinorGridlineStyle
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MajorGridlineThicknessProperty) = False Then .MajorGridlineThickness = fromAxis.MajorGridlineThickness
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MinorGridlineThicknessProperty) = False Then .MinorGridlineThickness = fromAxis.MinorGridlineThickness
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MajorStepProperty) = False Then .MajorStep = fromAxis.MajorStep
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MajorTickSizeProperty) = False Then .MajorTickSize = fromAxis.MajorTickSize
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MinorStepProperty) = False Then .MinorStep = fromAxis.MinorStep
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MinorTickSizeProperty) = False Then .MinorTickSize = fromAxis.MinorTickSize
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MinimumProperty) = False Then .Minimum = fromAxis.Minimum
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MaximumProperty) = False Then .Maximum = fromAxis.Maximum
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MinimumRangeProperty) = False Then .MinimumRange = fromAxis.MinimumRange
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MaximumRangeProperty) = False Then .MaximumRange = fromAxis.MaximumRange
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MinimumPaddingProperty) = False Then .MinimumPadding = fromAxis.MinimumPadding
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.MaximumPaddingProperty) = False Then .MaximumPadding = fromAxis.MaximumPadding
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.PositionProperty) = False Then .Position = fromAxis.Position
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.PositionTierProperty) = False Then .PositionTier = fromAxis.PositionTier
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.PositionAtZeroCrossingProperty) = False Then .PositionAtZeroCrossing = fromAxis.PositionAtZeroCrossing
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.StartPositionProperty) = False Then .StartPosition = fromAxis.StartPosition
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.StringFormatProperty) = False Then .StringFormat = fromAxis.StringFormat
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TextColorProperty) = False Then .TextColor = fromAxis.TextColor
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TicklineColorProperty) = False Then .TicklineColor = fromAxis.TicklineColor
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleClippingLengthProperty) = False Then .TitleClippingLength = fromAxis.TitleClippingLength
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleColorProperty) = False Then .TitleColor = fromAxis.TitleColor
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleFontProperty) = False Then .TitleFont = fromAxis.TitleFont
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleFontSizeProperty) = False Then .TitleFontSize = fromAxis.TitleFontSize
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleFontWeightProperty) = False Then .TitleFontWeight = fromAxis.TitleFontWeight
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleFormatStringProperty) = False Then .TitleFormatString = fromAxis.TitleFormatString
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TitleProperty) = False Then .Title = fromAxis.Title
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.ToolTipProperty) = False Then .ToolTip = If(IsNothing(fromAxis.ToolTip), Nothing, fromAxis.ToolTip.ToString())
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TickStyleProperty) = False Then .TickStyle = fromAxis.TickStyle
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TitlePositionProperty) = False Then .TitlePosition = fromAxis.TitlePosition
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.UnitProperty) = False Then .Unit = fromAxis.Unit
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.UseSuperExponentialFormatProperty) = False Then .UseSuperExponentialFormat = fromAxis.UseSuperExponentialFormat
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.LabelFormatterProperty) = False Then .LabelFormatter = fromAxis.LabelFormatter
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TickStyleProperty) = False Then .TickStyle = fromAxis.TickStyle
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.TitlePositionProperty) = False Then .TitlePosition = fromAxis.TitlePosition
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.UnitProperty) = False Then .Unit = fromAxis.Unit
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.UseSuperExponentialFormatProperty) = False Then .UseSuperExponentialFormat = fromAxis.UseSuperExponentialFormat
                If CopyBinding(fromAxis, toAxis, Wpf.Axis.LabelFormatterProperty) = False Then .LabelFormatter = fromAxis.LabelFormatter
            End With
        End If
    End Sub

    Class BindingConvertor
        Inherits ExpressionConverter

        Public Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destinationType As Type) As Boolean
            If destinationType = GetType(MarkupExtension) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object
            If destinationType = GetType(MarkupExtension) Then
                Dim bindingExpression As BindingExpression = TryCast(value, BindingExpression)
                If bindingExpression Is Nothing Then Throw New Exception()
                Return bindingExpression.ParentBinding
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class


End Module
Module EditorHelper
    Sub Register(Of T, TC)()
        Dim attr As Attribute() = New Attribute(0) {}
        Dim vConv As TypeConverterAttribute = New TypeConverterAttribute(GetType(TC))
        attr(0) = vConv
        TypeDescriptor.AddAttributes(GetType(T), attr)
    End Sub
End Module