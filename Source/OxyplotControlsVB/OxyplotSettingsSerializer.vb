Imports System.Globalization
Imports System.Text
Imports System.Windows.Markup
Imports System.Xml
Imports System.Xml.Serialization
Imports OxyPlot.Wpf

Public Module OxyplotSettingsSerializer

    Public ReadOnly OxyplotPropertiesTag As String = "OxyplotProperties"

    Public Function ToXelement(plot As Plot) As XElement
        Dim plotPropertiesElement As New XElement(OxyplotPropertiesTag)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'General Settings
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        plotPropertiesElement.Add(GeneralPlotControl.GeneralPropertiesToXElement(plot))
        '
        plotPropertiesElement.Add(LegendControl.LegendPropertiesToXElement(plot))
        '
        plotPropertiesElement.Add(AxesControl.AxesPropertiesToXElement(plot))
        '
        plotPropertiesElement.Add(AnnotationSelectorControl.AnnotationsPropertiesToXElement(plot))
        '
        plotPropertiesElement.Add(GenericSeriesControl.SeriesPropertiesToXElement(plot))

        Return plotPropertiesElement
    End Function

    Public Sub FromXelement(plot As Plot, element As XElement)
        'General
        Dim generalElement = element.Element(GeneralPlotControl.GeneralPropertiesTag)
        If Not IsNothing(generalElement) Then GeneralPlotControl.XElementToGeneralProperties(plot, generalElement)
        'Legend
        Dim legendElement = element.Element(LegendControl.LegendPropertiesTag)
        If Not IsNothing(legendElement) Then LegendControl.XElementToLegendProperties(plot, legendElement)
        'Axes
        Dim axesElement = element.Element(AxesControl.AxesPropertiesTag)
        If Not IsNothing(axesElement) Then AxesControl.XElementToAxesProperties(plot, axesElement)
        'Annotations
        Dim annotationsElement = element.Element(AnnotationSelectorControl.AnnotationsPropertiesTag)
        If Not IsNothing(annotationsElement) Then AnnotationSelectorControl.XElementToAnnotationsProperties(plot, annotationsElement)
        'Series
        Dim seriesElement = element.Element(GenericSeriesControl.SeriesPropertiesTag)
        If Not IsNothing(seriesElement) Then GenericSeriesControl.XElementToSeriesProperties(plot, seriesElement)

        'Update the plot.
        plot.InvalidatePlot(True)
    End Sub

    Public Function GetColorAttribute(el As XElement, attributeName As String, ByRef c As Color) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        c = CType(ColorConverter.ConvertFromString(value), Color)
        Return True
    End Function
    Public Function GetBrushAttribute(el As XElement, attributeName As String, converter As BrushConverter, ByRef b As Brush) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        b = CType(converter.ConvertFromInvariantString(value), Brush)
        Return True
    End Function
    Public Function GetStringAttribute(el As XElement, attributeName As String, ByRef s As String) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        '
        s = el.Attribute(attributeName).Value
        Return True
    End Function
    Public Function GetDoubleAttribute(el As XElement, attributeName As String, ByRef d As Double) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        Return Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, d)
    End Function
    Public Function GetIntegerAttribute(el As XElement, attributeName As String, ByRef i As Integer) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        Return Integer.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, i)
    End Function
    Public Function GetBooleanAttribute(el As XElement, attributeName As String, ByRef b As Boolean) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        Return Boolean.TryParse(value, b)
    End Function
    Public Function GetFontFamilyAttribute(el As XElement, attributeName As String, converter As FontFamilyConverter, ByRef ff As FontFamily) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        ff = CType(converter.ConvertFromInvariantString(value), FontFamily)
        Return True
    End Function
    Public Function GetFontWeightAttribute(el As XElement, attributeName As String, converter As FontWeightConverter, ByRef fw As FontWeight) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        fw = CType(converter.ConvertFromInvariantString(value), FontWeight)
        Return True
    End Function
    Public Function GetThicknessAttribute(el As XElement, attributeName As String, converter As Windows.ThicknessConverter, ByRef t As Thickness) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        t = CType(converter.ConvertFromInvariantString(value), Thickness)
        Return True
    End Function

    Public Function GetEnumAttribute(Of TEnum As Structure)(el As XElement, attributeName As String, ByRef e As TEnum) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        Return [Enum].TryParse(value, e)
    End Function
    Public Function GetDataPointAttribute(el As XElement, attributeName As String, ByRef dp As OxyPlot.DataPoint) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        dp = value.FromPrettyDataText()
        Return True
    End Function
    Public Function GetScreenVectorAttribute(el As XElement, attributeName As String, ByRef vp As OxyPlot.ScreenVector) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        vp = value.FromPrettyVectorText()
        Return True
    End Function
    Public Function GetScreenPointAttribute(el As XElement, attributeName As String, ByRef vp As OxyPlot.ScreenPoint) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        vp = value.FromPrettyScreenText()
        Return True
    End Function
    Public Function GetVectorAttribute(el As XElement, attributeName As String, ByRef v As Vector) As Boolean
        If IsNothing(el.Attribute(attributeName)) Then Return False
        Dim value As String = el.Attribute(attributeName).Value
        If String.IsNullOrEmpty(value) Then Return False
        '
        v = value.FromPrettyVectorString()
        Return True
    End Function
End Module
