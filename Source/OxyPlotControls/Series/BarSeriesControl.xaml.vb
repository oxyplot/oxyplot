Imports OxyPlot
Imports OxyplotControls.OxyplotPropertiesControl
Imports System.Globalization

Public Class BarSeriesControl
    Public Shared ReadOnly BarSeriesPropertiesTag As String = "BarSeries"  'Should this be unique because you could have multiple bar series?

    Public Shared SeriesProperty As DependencyProperty = DependencyProperty.Register(NameOf(Series), GetType(Wpf.BarSeriesBase), GetType(BarSeriesControl), New PropertyMetadata(Nothing))
    Public Property Series As Wpf.BarSeriesBase
        Get
            Return DirectCast(GetValue(SeriesProperty), Wpf.BarSeriesBase)
        End Get
        Set(value As Wpf.BarSeriesBase)
            SetValue(SeriesProperty, value)
        End Set
    End Property

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(BarSeriesControl))
    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property

    Public Sub CloseExpanders()
        'LabelingEXP.IsExpanded = False
        DisplayEXP.IsExpanded = False
    End Sub

    Public Sub Expand(expansionZone As OxyplotPropertiesControl.PropertyEXP)
        Select Case expansionZone
            Case PropertyEXP.Series_General
                'LabelingEXP.IsExpanded = True
                DisplayEXP.IsExpanded = True
            Case PropertyEXP.Series_Display
                DisplayEXP.IsExpanded = True
        End Select
    End Sub


    Public Shared Sub XElementToBarSeriesProperties(barSeries As Wpf.BarSeries, element As XElement)
        'Early Exit
        If IsNothing(barSeries) Then Exit Sub
        If element.Name <> BarSeriesPropertiesTag Then Exit Sub
        'get enabled or not
        GetBooleanAttribute(element, "IsEnabled", barSeries.IsEnabled)
        'Set up converters
        Dim fontWeightConverter = New FontWeightConverter()
        Dim thicknessConverter = New ThicknessConverter()
        Dim oxycolorConverter = New Wpf.OxyColorConverter()
        Dim brushConverter = New BrushConverter()

        'Labeling Properties
        Dim labelingElement = element.Element("Labeling")
        If Not IsNothing(labelingElement) Then
            If Not IsNothing(labelingElement.Attribute("Title")) Then barSeries.Title = labelingElement.Attribute("Title").Value
            If Not IsNothing(labelingElement.Attribute("LabelPlacement")) Then
                Dim placement As OxyPlot.Series.LabelPlacement = OxyPlot.Series.LabelPlacement.Inside
                OxyplotSettingsSerializer.GetEnumAttribute(Of OxyPlot.Series.LabelPlacement)(labelingElement, "LabelPlacement", placement)
                barSeries.LabelPlacement = placement
            End If
            If Not IsNothing(labelingElement.Attribute("TextColor")) Then barSeries.Foreground = CType(brushConverter.ConvertFromString(labelingElement.Attribute("TextColor").Value), Brush)
            If Not IsNothing(labelingElement.Attribute("Font")) Then barSeries.InternalSeries.Font = labelingElement.Attribute("Font").Value
            If Not IsNothing(labelingElement.Attribute("FontSize")) Then Double.TryParse(labelingElement.Attribute("FontSize").Value, barSeries.FontSize)
            If Not IsNothing(labelingElement.Attribute("FontWeight")) Then barSeries.FontWeight = CType(fontWeightConverter.ConvertFromString(labelingElement.Attribute("FontWeight").Value), FontWeight)
            If Not IsNothing(labelingElement.Attribute("Padding")) Then barSeries.Padding = CType(thicknessConverter.ConvertFromString(labelingElement.Attribute("Padding").Value), Thickness)
            If Not IsNothing(labelingElement.Attribute("RenderInLegend")) Then barSeries.RenderInLegend = Convert.ToBoolean(labelingElement.Attribute("RenderInLegend").Value)
            If Not IsNothing(labelingElement.Attribute("XAxisKey")) Then barSeries.XAxisKey = labelingElement.Attribute("XAxisKey").Value
            If Not IsNothing(labelingElement.Attribute("YAxisKey")) Then barSeries.YAxisKey = labelingElement.Attribute("YAxisKey").Value
            If Not IsNothing(labelingElement.Attribute("TrackerKey")) Then barSeries.TrackerKey = labelingElement.Attribute("TrackerKey").Value
            If Not IsNothing(labelingElement.Attribute("TrackerFormat")) Then barSeries.TrackerFormatString = labelingElement.Attribute("TrackerFormat").Value
            If Not IsNothing(labelingElement.Attribute("LabelFormat")) Then barSeries.LabelFormatString = labelingElement.Attribute("LabelFormat").Value
        End If

        'Display Properties
        Dim displayElement = element.Element("Display")
        If Not IsNothing(displayElement) Then
            If Not IsNothing(displayElement.Attribute("Background")) Then barSeries.Background = CType(brushConverter.ConvertFromString(displayElement.Attribute("Background").Value), Brush)
            If Not IsNothing(displayElement.Attribute("Color")) Then barSeries.Color = CType(ColorConverter.ConvertFromString(displayElement.Attribute("Color").Value), Color)
            If Not IsNothing(displayElement.Attribute("Fill")) Then barSeries.FillColor = CType(ColorConverter.ConvertFromString(displayElement.Attribute("Fill").Value), Color)
            'If Not IsNothing(displayElement.Attribute("JoinStyle")) Then barSeries.InternalSeries.TextColor = CType(oxycolorConverter.ConvertBack(displayElement.Attribute("TextColor").Value, Nothing, Nothing, Nothing), OxyPlot.OxyColor)
            'If Not IsNothing(displayElement.Attribute("LineStyle")) Then barSeries.InternalSeries.Font = displayElement.Attribute("Font").Value
            If Not IsNothing(displayElement.Attribute("LineThickness")) Then Double.TryParse(displayElement.Attribute("LineThickness").Value, barSeries.StrokeThickness)
        End If

        ''Markers Properties
        'Dim markerElement = element.Element("Markers")
        'If Not IsNothing(markerElement) Then
        '    If Not IsNothing(markerElement.Attribute("MarkerFill")) Then barSeries.Title = markerElement.Attribute("Title").Value
        '    If Not IsNothing(markerElement.Attribute("MarkerResolution")) Then barSeries.LabelPlacement = DirectCast(CType(markerElement.Attribute("LabelPlacement").Value, Integer), OxyPlot.Series.LabelPlacement)
        '    If Not IsNothing(markerElement.Attribute("MarkerSize")) Then barSeries.InternalSeries.TextColor = CType(oxycolorConverter.ConvertBack(markerElement.Attribute("TextColor").Value, Nothing, Nothing, Nothing), OxyPlot.OxyColor)
        '    If Not IsNothing(markerElement.Attribute("MarkerLineColor")) Then barSeries.InternalSeries.Font = markerElement.Attribute("Font").Value
        '    If Not IsNothing(markerElement.Attribute("Marker")) Then Double.TryParse(markerElement.Attribute("FontSize").Value, barSeries.FontSize)
        'End If

        ''BoxPlot Props
        'Dim boxPlotElement = element.Element("BoxPlot")
        'If Not IsNothing(boxPlotElement) Then
        '    If Not IsNothing(boxPlotElement.Attribute("BoxWidth")) Then Double.TryParse(boxPlotElement.Attribute("BoxWidth").Value, barSeries.boxwidth)
        '    If Not IsNothing(boxPlotElement.Attribute("FillColor")) Then barSeries.FillColor = CType(ColorConverter.ConvertFromString(titleElement.Attribute("Color").Value), Color)
        '    If Not IsNothing(boxPlotElement.Attribute("MedianAsDot")) Then barSeries.medianasdot = CType(oxycolorConverter.ConvertBack(boxPlotElement.Attribute("TextColor").Value, Nothing, Nothing, Nothing), OxyPlot.OxyColor)
        '    If Not IsNothing(boxPlotElement.Attribute("MedianPointSize")) Then barSeries.medianpointsize = boxPlotElement.Attribute("Font").Value
        '    If Not IsNothing(boxPlotElement.Attribute("OutlierSize")) Then Double.TryParse(boxPlotElement.Attribute("FontSize").Value, barSeries.FontSize)
        '    If Not IsNothing(boxPlotElement.Attribute("ShowBox")) Then barSeries.showbox = Convert.ToBoolean(boxPlotElement.Attribute("ShowBox").Value)
        '    If Not IsNothing(boxPlotElement.Attribute("WhiskerLength")) Then barSeries.InternalSeries.Font = Double.TryParse(boxPlotElement.Attribute("WhiskerLength").Value, barSeries.whiskerlength)
        'End If


        ''ErrorSeries Props
        'Dim errorSeriesElement = element.Element("SeriesErrorBars")
        'If Not IsNothing(errorSeriesElement) Then
        '    If Not IsNothing(errorSeriesElement.Attribute("Color")) Then barSeries.errorbarscolor = CType(ColorConverter.ConvertFromString(errorSeriesElement.Attribute("Color").Value), Color)
        '    If Not IsNothing(errorSeriesElement.Attribute("StopWidth")) Then barSeries.LabelPlacement = DirectCast(CType(errorSeriesElement.Attribute("LabelPlacement").Value, Integer), OxyPlot.Series.LabelPlacement)
        '    If Not IsNothing(errorSeriesElement.Attribute("Thickness")) Then barSeries.InternalSeries.TextColor = CType(oxycolorConverter.ConvertBack(errorSeriesElement.Attribute("TextColor").Value, Nothing, Nothing, Nothing), OxyPlot.OxyColor)
        '    If Not IsNothing(errorSeriesElement.Attribute("MinimumErrorSize")) Then barSeries.InternalSeries.Font = errorSeriesElement.Attribute("Font").Value
        'End If


    End Sub

    Public Shared Function BarSeriesPropertiesToXElement(barSeries As Wpf.BarSeries) As XElement
        Dim properties As New XElement(BarSeriesPropertiesTag)
        properties.SetAttributeValue("IsEnabled", barSeries.IsEnabled.ToString())


        'Labeling properties
        Dim labelProps As New XElement("Labeling")
        labelProps.SetAttributeValue("Title", barSeries.Title)
        labelProps.SetAttributeValue("LabelPlacement", barSeries.LabelPlacement.ToString())
        labelProps.SetAttributeValue("TextColor", barSeries.Foreground.ToString()) 'ToString() returns the hexadecimal notation, use ColorConverter.ConvertFromString() to convert from hexidecimal to color.
        labelProps.SetAttributeValue("Font", barSeries.FontFamily)
        labelProps.SetAttributeValue("FontSize", barSeries.FontSize)
        labelProps.SetAttributeValue("FontWeight", barSeries.FontWeight.ToString())
        labelProps.SetAttributeValue("Padding", barSeries.Padding.ToString)
        labelProps.SetAttributeValue("RenderInLegend", barSeries.RenderInLegend)
        labelProps.SetAttributeValue("XAxisKey", barSeries.XAxisKey)
        labelProps.SetAttributeValue("YAxisKey", barSeries.YAxisKey)
        labelProps.SetAttributeValue("TrackerKey", barSeries.TrackerKey)
        labelProps.SetAttributeValue("TrackerFormat", barSeries.TrackerFormatString)
        labelProps.SetAttributeValue("LabelFormat", barSeries.LabelFormatString)
        properties.Add(labelProps)

        Dim displayProps As New XElement("Display")
        displayProps.SetAttributeValue("Background", barSeries.Background)
        displayProps.SetAttributeValue("Color", barSeries.Foreground)
        displayProps.SetAttributeValue("Fill", barSeries.FillColor)
        'displayProps.SetAttributeValue("JoinStyle", barSeries.Style)
        'displayProps.SetAttributeValue("LineStyle", barSeries.Style)
        displayProps.SetAttributeValue("LineThickness", barSeries.StrokeThickness)
        properties.Add(displayProps)

        'Dim markerProps As New XElement("Markers")
        'markerProps.SetAttributeValue("MarkerFill", barSeries.markerfill)
        'markerProps.SetAttributeValue("MarkerResolution", barSeries.markerResolution)
        'markerProps.SetAttributeValue("MarkerSize", barSeries.markerSize)
        'markerProps.SetAttributeValue("MarkerLineColor", barSeries.markerLineColor)
        'markerProps.SetAttributeValue("MarkerLineThickness", barSeries.markerLineThickness)
        'markerProps.SetAttributeValue("Marker", barSeries.marker)
        'properties.Add(markerProps)

        'Dim boxPlotProps As New XElement("BoxPlot")
        'boxPlotProps.SetAttributeValue("BoxWidth", barSeries.boxwidth)
        'boxPlotProps.SetAttributeValue("FillColor", barSeries.FillColor)
        'boxPlotProps.SetAttributeValue("MedianAsDot", barSeries.medianasdot)
        'boxPlotProps.SetAttributeValue("MedianPointSize", barSeries.medianpointsize)
        'boxPlotProps.SetAttributeValue("OutlierSize", barSeries.outliersize)
        'boxPlotProps.SetAttributeValue("ShowBox", barSeries.showbox)
        'boxPlotProps.SetAttributeValue("WhiskerLength", barSeries.whiskerlength)
        'properties.Add(boxPlotProps)

        'Dim errorBarsProps As New XElement("SeriesErrorBars")
        'errorBarsProps.SetAttributeValue("Color", barSeries.color)
        'errorBarsProps.SetAttributeValue("StopWidth", barSeries.stopwidth)
        'errorBarsProps.SetAttributeValue("Thickness", barSeries.thickness)
        'errorBarsProps.SetAttributeValue("MinimumErrorSize", barSeries.minimumerrorsize)
        'properties.Add(errorBarsProps)

        Return properties
    End Function

End Class
Public Class BarSeriesFillConverter
    Implements IMultiValueConverter

    Private _series As Series.BarSeriesBase

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color
        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then Return New SolidColorBrush(c)
        _series = TryCast(DirectCast(values(1), Wpf.BarSeriesBase).InternalSeries, Series.BarSeriesBase)
        If IsNothing(_series) Then Return New SolidColorBrush(c)
        'Convert
        If oxyCol.IsAutomatic() Then
            With _series.ActualFillColor
                Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
            End With
        End If

        Return New SolidColorBrush(c)
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        If IsNothing(_series) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        'Get color value
        If value.GetType <> GetType(SolidColorBrush) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        Dim c As Color = DirectCast(value, SolidColorBrush).Color
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        If OxyColor.ColorDifference(oxyCol, _series.ActualFillColor) = 0 Then
            With _series.ActualFillColor
                Return {Color.FromArgb(.A, .R, .G, .B), _series}
            End With
        End If
        '
        Return {c, _series}
    End Function

End Class

'Public Class LabelPlacementConverter
'    Implements IMultiValueConverter

'    Private _series As Series.BarSeriesBase

'    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
'        ''Get Color
'        'If IsNothing(values(0)) Then Return Nothing
'        'If values(0).GetType <> GetType(OxyPlot.Series.LabelPlacement) Then Return Nothing
'        'Dim l As OxyPlot.Series.LabelPlacement = DirectCast(values(0), OxyPlot.Series.LabelPlacement)
'        'Dim oxyCol = l 'OxyColor.FromArgb(c.A, c.R, c.G, c.B)
'        ''
'        ''Get Series (this should only be set on convert with one-way binding)
'        'If IsNothing(values(1)) Then Return New SolidColorBrush(c)
'        '_series = TryCast(DirectCast(values(1), Wpf.BarSeriesBase).InternalSeries, Series.BarSeriesBase)
'        'If IsNothing(_series) Then Return New SolidColorBrush(c)
'        ''Convert
'        'If oxyCol.IsAutomatic() Then
'        '    With _series.ActualFillColor
'        '        Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
'        '    End With
'        'End If

'        'Return New SolidColorBrush(c)
'    End Function

'    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
'        If IsNothing(_series) Then Return {OxyPlot.Series.LabelPlacement.Inside, Nothing}
'        'Get value
'        If value.GetType <> GetType(String) Then Return {OxyPlot.Series.LabelPlacement.Inside, Nothing}
'        Dim l As OxyPlot.Series.LabelPlacement = DirectCast(value, OxyPlot.Series.LabelPlacement)
'        'Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
'        '
'        'If OxyColor.ColorDifference(oxyCol, _series.ActualFillColor) = 0 Then
'        '    With _series.ActualFillColor
'        '        Return {Color.FromArgb(.A, .R, .G, .B), _series}
'        '    End With
'        'End If
'        '
'        Return {l, _series}
'    End Function

'End Class
