Imports System.Globalization
Imports OxyPlot
Public Class LegendControl
    Public Shared ReadOnly LegendPropertiesTag As String = "Legend"

    Public Shared ReadOnly PlotProperty As DependencyProperty = DependencyProperty.Register(NameOf(Plot), GetType(Wpf.Plot), GetType(LegendControl))
    Public Property Plot As Wpf.Plot
        Get
            Return DirectCast(GetValue(PlotProperty), Wpf.Plot)
        End Get
        Set(value As Wpf.Plot)
            SetValue(PlotProperty, value)
        End Set
    End Property

    Public Shared ReadOnly Property OrientationOptions As New List(Of LegendOrientation)(DirectCast([Enum].GetValues(GetType(LegendOrientation)), LegendOrientation()))
    Public Shared ReadOnly Property ItemOrderOptions As New List(Of LegendItemOrder)(DirectCast([Enum].GetValues(GetType(LegendItemOrder)), LegendItemOrder()))
    Public Shared ReadOnly Property PlacementOptions As New List(Of LegendPlacement)(DirectCast([Enum].GetValues(GetType(LegendPlacement)), LegendPlacement()))
    Public Shared ReadOnly Property PositionOptions As New List(Of LegendPosition)(DirectCast([Enum].GetValues(GetType(LegendPosition)), LegendPosition()))
    Public Shared ReadOnly Property SymbolPlacementOptions As New List(Of LegendSymbolPlacement)(DirectCast([Enum].GetValues(GetType(LegendSymbolPlacement)), LegendSymbolPlacement()))

    'Public Shared TitleMinWidthProp As DependencyProperty = DependencyProperty.Register(NameOf(TitleMinWidth), GetType(Integer), GetType(LegendControl), New UIPropertyMetadata(110))
    'Public Property TitleMinWidth As Integer
    '    Get
    '        Return DirectCast(GetValue(TitleMinWidthProp), Integer)
    '    End Get
    '    Set(value As Integer)
    '        SetValue(TitleMinWidthProp, value)
    '    End Set
    'End Property

    'Public Shared LeaderLinesVisibilityProp As DependencyProperty = DependencyProperty.Register(NameOf(LeaderLinesVisibility), GetType(Visibility), GetType(LegendControl), New UIPropertyMetadata(Visibility.Visible))
    'Public Property LeaderLinesVisibility As Visibility
    '    Get
    '        Return DirectCast(GetValue(LeaderLinesVisibilityProp), Visibility)
    '    End Get
    '    Set(value As Visibility)
    '        SetValue(LeaderLinesVisibilityProp, value)
    '    End Set
    'End Property

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(LegendControl))
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
    End Sub

    Public Shared Function LegendPropertiesToXElement(plot As Wpf.Plot) As XElement
        Dim legendProperties As New XElement(LegendPropertiesTag)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Legend Area
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim fwc As New FontWeightConverter()
        Dim legendAreaProperties As New XElement("Area")
        legendAreaProperties.SetAttributeValue(NameOf(plot.IsLegendVisible), plot.IsLegendVisible)
        legendAreaProperties.SetAttributeValue(NameOf(plot.LegendBackground), plot.LegendBackground.ToString()) 'ToString() returns the hexadecimal notation, use ColorConverter.ConvertFromString() to convert from hexidecimal to color.
        legendAreaProperties.SetAttributeValue(NameOf(plot.LegendBorder), plot.LegendBorder.ToString())
        legendAreaProperties.SetAttributeValue(NameOf(plot.LegendBorderThickness), plot.LegendBorderThickness.ToString("G17", CultureInfo.InvariantCulture))
        legendAreaProperties.SetAttributeValue(NameOf(plot.LegendPadding), plot.LegendPadding.ToString("G17", CultureInfo.InvariantCulture))
        legendProperties.Add(legendAreaProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Legend Position Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim subTitleProperties As New XElement("Position")
        subTitleProperties.SetAttributeValue(NameOf(plot.LegendPlacement), plot.LegendPlacement.ToString)
        subTitleProperties.SetAttributeValue(NameOf(plot.LegendPosition), plot.LegendPosition.ToString)
        subTitleProperties.SetAttributeValue(NameOf(plot.LegendOrientation), plot.LegendOrientation.ToString)
        legendProperties.Add(subTitleProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Title Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim titleProperties As New XElement("Title")
        titleProperties.SetAttributeValue(NameOf(plot.LegendTitle), plot.LegendTitle)
        titleProperties.SetAttributeValue(NameOf(plot.LegendTitleColor), plot.LegendTitleColor.ToString())
        titleProperties.SetAttributeValue(NameOf(plot.LegendTitleFont), plot.LegendTitleFont)
        titleProperties.SetAttributeValue(NameOf(plot.LegendTitleFontSize), plot.LegendTitleFontSize.ToString("G17", CultureInfo.InvariantCulture))
        titleProperties.SetAttributeValue(NameOf(plot.LegendTitleFontWeight), fwc.ConvertToInvariantString(plot.LegendTitleFontWeight))
        legendProperties.Add(titleProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Legend Item Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim itemProperties As New XElement("Items")
        itemProperties.SetAttributeValue(NameOf(plot.LegendTextColor), plot.LegendTextColor.ToString())
        itemProperties.SetAttributeValue(NameOf(plot.LegendSymbolLength), plot.LegendSymbolLength.ToString("G17", CultureInfo.InvariantCulture))
        itemProperties.SetAttributeValue(NameOf(plot.LegendSymbolMargin), plot.LegendSymbolMargin.ToString("G17", CultureInfo.InvariantCulture))
        itemProperties.SetAttributeValue(NameOf(plot.LegendSymbolPlacement), plot.LegendSymbolPlacement.ToString())
        itemProperties.SetAttributeValue(NameOf(plot.LegendColumnSpacing), plot.LegendColumnSpacing.ToString("G17", CultureInfo.InvariantCulture))
        itemProperties.SetAttributeValue(NameOf(plot.LegendItemAlignment), plot.LegendItemAlignment.ToString())
        itemProperties.SetAttributeValue(NameOf(plot.LegendItemOrder), plot.LegendItemOrder.ToString())
        itemProperties.SetAttributeValue(NameOf(plot.LegendItemSpacing), plot.LegendItemSpacing.ToString("G17", CultureInfo.InvariantCulture))
        itemProperties.SetAttributeValue(NameOf(plot.LegendLineSpacing), plot.LegendLineSpacing.ToString("G17", CultureInfo.InvariantCulture))
        legendProperties.Add(itemProperties)
        '
        '
        Return legendProperties
    End Function
    '
    ''' <summary>
    ''' Load general plot property settings from XElement.
    ''' </summary>
    ''' <param name="plot">WPF Oxyplot Plot control.</param>
    ''' <param name="element">XElement that contains the settings.</param>
    Public Shared Sub XElementToLegendProperties(plot As Wpf.Plot, element As XElement)
        'Early Exit
        If IsNothing(plot) Then Exit Sub
        If element.Name <> LegendPropertiesTag Then Exit Sub
        'Set up converters
        Dim fontWeightConverter = New FontWeightConverter()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Area Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim areaElement = element.Element("Area")
        If Not IsNothing(areaElement) Then
            GetBooleanAttribute(areaElement, NameOf(plot.IsLegendVisible), plot.IsLegendVisible)
            GetColorAttribute(areaElement, NameOf(plot.LegendBackground), plot.LegendBackground)
            GetColorAttribute(areaElement, NameOf(plot.LegendBorder), plot.LegendBorder)
            GetDoubleAttribute(areaElement, NameOf(plot.LegendBorderThickness), plot.LegendBorderThickness)
            GetDoubleAttribute(areaElement, NameOf(plot.LegendPadding), plot.LegendPadding)

            ' Backward compatibility
            GetBooleanAttribute(areaElement, "LegendVisible", plot.IsLegendVisible)
            GetColorAttribute(areaElement, "BackgroundColor", plot.LegendBackground)
            GetColorAttribute(areaElement, "BorderColor", plot.LegendBorder)
            GetDoubleAttribute(areaElement, "BorderThickness", plot.LegendBorderThickness)
            GetDoubleAttribute(areaElement, "Padding", plot.LegendPadding)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Position Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim positionElement = element.Element("Position")
        If Not IsNothing(positionElement) Then
            GetEnumAttribute(positionElement, NameOf(plot.LegendPlacement), plot.LegendPlacement)
            GetEnumAttribute(positionElement, NameOf(plot.LegendPosition), plot.LegendPosition)
            GetEnumAttribute(positionElement, NameOf(plot.LegendOrientation), plot.LegendOrientation)

            ' Backward compatibility
            GetEnumAttribute(positionElement, "Placement", plot.LegendPlacement)
            GetEnumAttribute(positionElement, "Position", plot.LegendPosition)
            GetEnumAttribute(positionElement, "Orientation", plot.LegendOrientation)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Title Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim titleElement = element.Element("Title")
        If Not IsNothing(titleElement) Then
            GetStringAttribute(titleElement, NameOf(plot.LegendTitle), plot.LegendTitle)
            GetColorAttribute(titleElement, NameOf(plot.LegendTitleColor), plot.LegendTitleColor)
            GetStringAttribute(titleElement, NameOf(plot.LegendTitleFont), plot.LegendTitleFont)
            GetDoubleAttribute(titleElement, NameOf(plot.LegendTitleFontSize), plot.LegendTitleFontSize)
            GetFontWeightAttribute(titleElement, NameOf(plot.LegendTitleFontWeight), fontWeightConverter, plot.LegendTitleFontWeight)

            ' Backward compatibility
            GetStringAttribute(titleElement, "Title", plot.LegendTitle)
            GetColorAttribute(titleElement, "Color", plot.LegendTitleColor)
            GetStringAttribute(titleElement, "Font", plot.LegendTitleFont)
            GetDoubleAttribute(titleElement, "Size", plot.LegendTitleFontSize)
            GetFontWeightAttribute(titleElement, "Weight", fontWeightConverter, plot.LegendTitleFontWeight)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Legend Item Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim itemsElement = element.Element("Items")
        If Not IsNothing(itemsElement) Then
            GetColorAttribute(itemsElement, NameOf(plot.LegendTextColor), plot.LegendTextColor)
            GetDoubleAttribute(itemsElement, NameOf(plot.LegendSymbolLength), plot.LegendSymbolLength)
            GetDoubleAttribute(itemsElement, NameOf(plot.LegendSymbolMargin), plot.LegendSymbolMargin)
            GetEnumAttribute(itemsElement, NameOf(plot.LegendSymbolPlacement), plot.LegendSymbolPlacement)
            GetDoubleAttribute(itemsElement, NameOf(plot.LegendColumnSpacing), plot.LegendColumnSpacing)
            GetEnumAttribute(itemsElement, NameOf(plot.LegendItemAlignment), plot.LegendItemAlignment)
            GetEnumAttribute(itemsElement, NameOf(plot.LegendItemOrder), plot.LegendItemOrder)
            GetDoubleAttribute(itemsElement, NameOf(plot.LegendItemSpacing), plot.LegendItemSpacing)
            GetDoubleAttribute(itemsElement, NameOf(plot.LegendLineSpacing), plot.LegendLineSpacing)

            ' Backward compatibility
            GetColorAttribute(itemsElement, "Color", plot.LegendTextColor)
            GetDoubleAttribute(itemsElement, "SymbolLength", plot.LegendSymbolLength)
            GetDoubleAttribute(itemsElement, "SymbolMargin", plot.LegendSymbolMargin)
            GetEnumAttribute(itemsElement, "SymbolPlacement", plot.LegendSymbolPlacement)
            GetDoubleAttribute(itemsElement, "ColumnSpacing", plot.LegendColumnSpacing)
            GetEnumAttribute(itemsElement, "ItemAlignment", plot.LegendItemAlignment)
            GetEnumAttribute(itemsElement, "ItemOrder", plot.LegendItemOrder)
            GetDoubleAttribute(itemsElement, "ItemSpacing", plot.LegendItemSpacing)
            GetDoubleAttribute(itemsElement, "LineSpacing", plot.LegendLineSpacing)
        End If
    End Sub
End Class
