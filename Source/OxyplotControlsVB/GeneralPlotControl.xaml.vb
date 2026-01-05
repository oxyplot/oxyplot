Imports System.Globalization
Imports System.Windows.Markup
Imports System.Xml
Imports OxyPlot
Public Class GeneralPlotControl
    Public Shared ReadOnly GeneralPropertiesTag As String = "General"

    Public Shared PlotProperty As DependencyProperty = DependencyProperty.Register(NameOf(Plot), GetType(Wpf.Plot), GetType(GeneralPlotControl))
    Public Property Plot As Wpf.Plot
        Get
            Return DirectCast(GetValue(PlotProperty), Wpf.Plot)
        End Get
        Set(value As Wpf.Plot)
            SetValue(PlotProperty, value)
        End Set
    End Property


    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(GeneralPlotControl))
    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'ExpanderStyle = CType(FindResource(" ExcelExpanderStyle"), Style)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="plot"></param>
    ''' <returns></returns>
    Public Shared Function GeneralPropertiesToXElement(plot As Wpf.Plot) As XElement
        Dim generalProperties As New XElement(GeneralPropertiesTag)
        generalProperties.SetAttributeValue(NameOf(plot.IsEnabled), plot.IsEnabled.ToString())
        '
        Dim weightConverter As New FontWeightConverter()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Title Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim titleProperties As New XElement("Title")
        titleProperties.SetAttributeValue(NameOf(plot.Title), plot.Title)
        titleProperties.SetAttributeValue(NameOf(plot.TitleColor), plot.TitleColor.ToString()) 'ToString() returns the hexadecimal notation, use ColorConverter.ConvertFromString() to convert from hexidecimal to color.
        titleProperties.SetAttributeValue(NameOf(plot.TitleFont), plot.TitleFont)
        titleProperties.SetAttributeValue(NameOf(plot.TitleFontSize), plot.TitleFontSize.ToString("G17", CultureInfo.InvariantCulture))
        titleProperties.SetAttributeValue(NameOf(plot.TitleFontWeight), weightConverter.ConvertToInvariantString(plot.TitleFontWeight))
        titleProperties.SetAttributeValue(NameOf(plot.TitlePadding), plot.TitlePadding.ToString("G17", CultureInfo.InvariantCulture))
        generalProperties.Add(titleProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'SubTitle Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim subTitleProperties As New XElement("Subtitle")
        subTitleProperties.SetAttributeValue(NameOf(plot.Subtitle), plot.Subtitle)
        subTitleProperties.SetAttributeValue(NameOf(plot.SubtitleColor), plot.SubtitleColor.ToString())
        subTitleProperties.SetAttributeValue(NameOf(plot.SubtitleFont), plot.SubtitleFont)
        subTitleProperties.SetAttributeValue(NameOf(plot.SubtitleFontSize), plot.SubtitleFontSize.ToString("G17", CultureInfo.InvariantCulture))
        subTitleProperties.SetAttributeValue(NameOf(plot.SubtitleFontWeight), weightConverter.ConvertToInvariantString(plot.SubtitleFontWeight))
        generalProperties.Add(subTitleProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Chart Area Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim chartProperties As New XElement("Chart")
        '
        Dim bc As New BrushConverter()
        Dim tc As New ThicknessConverter()
        chartProperties.SetAttributeValue(NameOf(plot.Background), bc.ConvertToInvariantString(plot.Background))
        chartProperties.SetAttributeValue(NameOf(plot.BorderBrush), bc.ConvertToInvariantString(plot.BorderBrush))
        chartProperties.SetAttributeValue(NameOf(plot.BorderThickness), tc.ConvertToInvariantString(plot.BorderThickness)) 'Use a ThicknessConverter() to convert from string to thickness
        generalProperties.Add(chartProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Plot Area Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim plotAreaProperties As New XElement("Plot")
        plotAreaProperties.SetAttributeValue(NameOf(plot.PlotAreaBackground), bc.ConvertToInvariantString(plot.PlotAreaBackground))
        plotAreaProperties.SetAttributeValue(NameOf(plot.PlotAreaBorderColor), plot.PlotAreaBorderColor.ToString())
        plotAreaProperties.SetAttributeValue(NameOf(plot.PlotAreaBorderThickness), tc.ConvertToInvariantString(plot.PlotAreaBorderThickness))
        generalProperties.Add(plotAreaProperties)
        '
        Return generalProperties
    End Function
    '
    ''' <summary>
    ''' Load general plot property settings from XElement.
    ''' </summary>
    ''' <param name="plot">WPF Oxyplot Plot control.</param>
    ''' <param name="element">XElement that contains the settings.</param>
    Public Shared Sub XElementToGeneralProperties(plot As Wpf.Plot, element As XElement)
        'Early Exit
        If IsNothing(plot) Then Exit Sub
        If element.Name <> GeneralPropertiesTag Then Exit Sub
        'get enabled or not
        GetBooleanAttribute(element, NameOf(plot.IsEnabled), plot.IsEnabled)
        'Set up converters
        Dim weightConverter = New FontWeightConverter()
        Dim thicknessConverter = New ThicknessConverter()
        Dim brushConverter = New BrushConverter()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Title Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim titleElement = element.Element("Title")
        If Not IsNothing(titleElement) Then
            GetStringAttribute(titleElement, NameOf(plot.Title), plot.Title)
            GetColorAttribute(titleElement, NameOf(plot.TitleColor), plot.TitleColor)
            GetStringAttribute(titleElement, NameOf(plot.TitleFont), plot.TitleFont)
            GetDoubleAttribute(titleElement, NameOf(plot.TitleFontSize), plot.TitleFontSize)
            GetFontWeightAttribute(titleElement, NameOf(plot.TitleFontWeight), weightConverter, plot.TitleFontWeight)
            GetDoubleAttribute(titleElement, NameOf(plot.TitlePadding), plot.TitlePadding)

            ' Backward compatibility
            GetColorAttribute(titleElement, "Color", plot.TitleColor)
            GetStringAttribute(titleElement, "Font", plot.TitleFont)
            GetDoubleAttribute(titleElement, "Size", plot.TitleFontSize)
            GetFontWeightAttribute(titleElement, "Weight", weightConverter, plot.TitleFontWeight)
            GetDoubleAttribute(titleElement, "Padding", plot.TitlePadding)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'SubTitle Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim subTitleElement = element.Element("Subtitle")
        If Not IsNothing(subTitleElement) Then
            GetStringAttribute(subTitleElement, NameOf(plot.Subtitle), plot.Subtitle)
            GetColorAttribute(subTitleElement, NameOf(plot.SubtitleColor), plot.SubtitleColor)
            GetStringAttribute(subTitleElement, NameOf(plot.SubtitleFont), plot.SubtitleFont)
            GetDoubleAttribute(subTitleElement, NameOf(plot.SubtitleFontSize), plot.SubtitleFontSize)
            GetFontWeightAttribute(subTitleElement, NameOf(plot.SubtitleFontWeight), weightConverter, plot.SubtitleFontWeight)

            ' Backward compatibility
            GetStringAttribute(subTitleElement, "Title", plot.Subtitle)
            GetColorAttribute(subTitleElement, "Color", plot.SubtitleColor)
            GetStringAttribute(subTitleElement, "Font", plot.SubtitleFont)
            GetDoubleAttribute(subTitleElement, "Size", plot.SubtitleFontSize)
            GetFontWeightAttribute(subTitleElement, "Weight", weightConverter, plot.SubtitleFontWeight)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Chart Area Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim chartElement = element.Element("Chart")
        If Not IsNothing(chartElement) Then
            GetBrushAttribute(chartElement, NameOf(plot.Background), brushConverter, plot.Background)
            GetBrushAttribute(chartElement, NameOf(plot.BorderBrush), brushConverter, plot.BorderBrush)
            GetThicknessAttribute(chartElement, NameOf(plot.BorderThickness), thicknessConverter, plot.BorderThickness)

            ' Backward compatibility
            Dim backgroundElement = chartElement.Element("BackgroundBrush")
            If Not IsNothing(backgroundElement) Then
                Dim bg = TryCast(DeserializeFromXElement(backgroundElement.Elements.First), Brush)
                If Not IsNothing(bg) Then plot.Background = bg
            End If
            '
            Dim borderElement = chartElement.Element("BorderBrush")
            If Not IsNothing(borderElement) Then
                Dim bg = TryCast(DeserializeFromXElement(borderElement.Elements.First), Brush)
                If Not IsNothing(bg) Then plot.BorderBrush = bg
            End If
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Plot Area Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim plotAreaElement = element.Element("Plot")
        If Not IsNothing(plotAreaElement) Then
            GetBrushAttribute(plotAreaElement, NameOf(plot.PlotAreaBackground), brushConverter, plot.PlotAreaBackground)
            GetColorAttribute(plotAreaElement, NameOf(plot.PlotAreaBorderColor), plot.PlotAreaBorderColor)
            GetThicknessAttribute(plotAreaElement, NameOf(plot.PlotAreaBorderThickness), thicknessConverter, plot.PlotAreaBorderThickness)

            ' Backward compatibility
            Dim backgroundElement = plotAreaElement.Element("BackgroundBrush")
            If Not IsNothing(backgroundElement) Then
                Dim bg = TryCast(DeserializeFromXElement(backgroundElement.Elements.First), Brush)
                If Not IsNothing(bg) Then plot.PlotAreaBackground = bg
            End If
            GetColorAttribute(plotAreaElement, "BorderColor", plot.PlotAreaBorderColor)
            GetThicknessAttribute(plotAreaElement, "BorderThickness", thicknessConverter, plot.PlotAreaBorderThickness)

        End If
    End Sub

    Public Shared Function DeserializeFromXElement(element As XElement) As Object
        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(element.ToString)
        Return XamlReader.Load(New XmlNodeReader(doc))
    End Function
End Class

''' <summary>
''' This is a complete bullcrap converter. Oxyplot decided to make their (Automatic) color ARGB(0,0,0,1). In Oxyplot code, if
''' the "OxyColor" is automatic then there is a function called GetActualOxyColor(defaultIfAuto), I know....it's stupid, that returns 
''' the input color if IsAutomatic is true else returns the color. This is beyond stupid.
''' </summary>
Public Class OxyAutomaticColorConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If IsNothing(value) Then Return value
        If value.GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(value, Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        If oxyCol.IsAutomatic() Then Return New SolidColorBrush(Color.FromArgb(255, 0, 0, 0))
        Return New SolidColorBrush(c)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return DirectCast(value, SolidColorBrush).Color
    End Function
End Class
''' <summary>
''' The following is a converter to the default font size when a font size is defined as double.NAN
''' </summary>
Public Class OxyDefaultFontSizeConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If IsNothing(value) Then Return CDbl(12)
        If value.GetType <> GetType(Double) Then Return CDbl(12)
        Dim doubleVal As Double = DirectCast(value, Double)
        If Double.IsNaN(doubleVal) OrElse Double.IsInfinity(doubleVal) Then Return CDbl(12)
        Return doubleVal
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If IsNothing(value) Then Return Double.NaN
        If value.GetType <> GetType(Double) Then Return Double.NaN
        Dim doubleVal As Double = DirectCast(value, Double)
        If doubleVal = CDbl(12) Then Return Double.NaN
        Return doubleVal
    End Function
End Class

Public Class SolidColorBrushConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If IsNothing(value) Then Return Nothing
        Return DirectCast(value, SolidColorBrush)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If IsNothing(value) Then Return Nothing
        Return DirectCast(value, Brush)
    End Function
End Class
