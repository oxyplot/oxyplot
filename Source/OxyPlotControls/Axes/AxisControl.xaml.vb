Imports System.Globalization
Imports OxyPlot
Public Class AxisControl

    Private Shared _epsilon As Double = 0.0000000000000001

    Public Shared ReadOnly AxisPropertiesTag As String = "Axis"
    Public Shared ReadOnly Property LineStyleOptions As List(Of DoubleCollection) = GenericControls.LineStyleSelectorControl.LineStyleOptions
    Public Shared ReadOnly Property AxisPositionOptions As New List(Of Axes.AxisPosition)(DirectCast([Enum].GetValues(GetType(Axes.AxisPosition)), Axes.AxisPosition()))
    Public Shared ReadOnly Property AxisTickStyleOptions As New List(Of Axes.TickStyle)(DirectCast([Enum].GetValues(GetType(Axes.TickStyle)), Axes.TickStyle()))
    Public Shared ReadOnly Property AxisLayerOptions As New List(Of Axes.AxisLayer)(DirectCast([Enum].GetValues(GetType(Axes.AxisLayer)), Axes.AxisLayer()))

    Public Shared AxisProperty As DependencyProperty = DependencyProperty.Register(NameOf(Axis), GetType(Wpf.Axis), GetType(AxisControl), New PropertyMetadata(Nothing, AddressOf InitializePlot))
    Public Property Axis As Wpf.Axis
        Get
            Return DirectCast(GetValue(AxisProperty), Wpf.Axis)
        End Get
        Set(value As Wpf.Axis)
            SetValue(AxisProperty, value)
        End Set
    End Property
    '
    Private _oldLinearAxis As Wpf.LinearAxis
    Private _oldLogAxis As Wpf.LogarithmicAxis
    Private _ignoreMaxMinChange As Boolean = False

    Public Event AxisTypeChanged(oldAxis As Wpf.Axis, newAxis As Wpf.Axis)

    Private Shared Sub InitializePlot(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsNothing(d) Then Exit Sub
        If d.GetType <> GetType(AxisControl) Then Exit Sub
        Dim thisControl = DirectCast(d, AxisControl)
        '
        Dim oldAxis As Wpf.Axis = TryCast(e.OldValue, Wpf.Axis)
        If oldAxis IsNot Nothing Then

        End If

        '
        Dim newAxis As Wpf.Axis = TryCast(e.NewValue, Wpf.Axis)
        If newAxis Is Nothing OrElse thisControl.Content Is Nothing Then Exit Sub
        '
        Dim axisTypeComboBox As ComboBox = CType(thisControl.AxisTypeSelector.InnerContent, ComboBox)
        If axisTypeComboBox Is Nothing Then Exit Sub
        '
        Dim isSupportedType As Boolean = True

        RemoveHandler axisTypeComboBox.SelectionChanged, AddressOf thisControl.AxisTypeComboBox_SelectionChanged
        ' Hide axis specific properties and clear bindings.
        'thisControl.LogBaseControl.Visibility = Visibility.Collapsed
        'BindingOperations.ClearBinding(thisControl.LogBaseControl, GenericControls.NumericPropertySelectorControl.SelectedNumberProperty)
        thisControl.PowerPaddingControl.Visibility = Visibility.Collapsed
        BindingOperations.ClearBinding(thisControl.PowerPaddingControl, GenericControls.BooleanPropertyControl.IsSelectedProperty)
        'thisControl.CalendarWeekControl.Visibility = Visibility.Collapsed
        'BindingOperations.ClearBinding(thisControl.CalendarWeekControl, GenericControls.CalendarWeekRulePropertyControl.SelectedCalendarWeekRuleProperty)
        thisControl.GapWidthSelector.Visibility = Visibility.Collapsed
        BindingOperations.ClearBinding(thisControl.GapWidthSelector, GenericControls.NumericPropertySelectorControl.SelectedNumberProperty)
        thisControl.AxisLabelsControl.Visibility = Visibility.Collapsed
        BindingOperations.ClearBinding(thisControl.AxisLabelsControl, GenericControls.StringListPropertyControl.StringListProperty)
        thisControl.TickCenteredControl.Visibility = Visibility.Collapsed
        BindingOperations.ClearBinding(thisControl.TickCenteredControl, GenericControls.BooleanPropertyControl.IsSelectedProperty)
        thisControl.DateAxisMinimum.Visibility = Visibility.Collapsed
        thisControl.DateAxisMaximum.Visibility = Visibility.Collapsed
        thisControl.AxisMinimum.Visibility = Visibility.Collapsed
        thisControl.AxisMaximum.Visibility = Visibility.Collapsed

        Select Case newAxis.GetType
            Case GetType(Wpf.LinearAxis)
                axisTypeComboBox.SelectedIndex = 0
                thisControl.AxisMinimum.Visibility = Visibility.Visible
                thisControl.AxisMaximum.Visibility = Visibility.Visible
                thisControl.AxisMinimum.DefaultNumber = Double.NaN
                thisControl.AxisMinimum.MinValue = Double.MinValue
                thisControl.AxisMinimum.MaxValue = Double.MaxValue
                thisControl.AxisMaximum.DefaultNumber = Double.NaN
                thisControl.AxisMaximum.MinValue = Double.MinValue
                thisControl.AxisMaximum.MaxValue = Double.MaxValue
                thisControl.LabelTypeSelector.Visibility = Visibility.Visible
                thisControl.DecimalPlaces.Visibility = Visibility.Visible
            Case GetType(Wpf.LogarithmicAxis)
                axisTypeComboBox.SelectedIndex = 1
                thisControl.AxisMinimum.Visibility = Visibility.Visible
                thisControl.AxisMaximum.Visibility = Visibility.Visible
                thisControl.AxisMinimum.DefaultNumber = Double.NaN
                thisControl.AxisMinimum.MinValue = _epsilon
                thisControl.AxisMinimum.MaxValue = Double.MaxValue
                thisControl.AxisMaximum.DefaultNumber = Double.NaN
                thisControl.AxisMaximum.MinValue = _epsilon
                thisControl.AxisMaximum.MaxValue = Double.MaxValue
                thisControl.LabelTypeSelector.Visibility = Visibility.Visible
                thisControl.DecimalPlaces.Visibility = Visibility.Visible
                'thisControl.LogBaseControl.Visibility = Visibility.Visible
                'Dim logBaseBinding As New Binding(NameOf(Wpf.LogarithmicAxis.Base)) With {.Source = newAxis, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.TwoWay}
                'BindingOperations.SetBinding(thisControl.LogBaseControl, GenericControls.NumericPropertySelectorControl.SelectedNumberProperty, logBaseBinding)
                thisControl.PowerPaddingControl.Visibility = Visibility.Visible
                Dim powerPaddingBinding As New Binding(NameOf(Wpf.LogarithmicAxis.PowerPadding)) With {.Source = newAxis, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.TwoWay}
                BindingOperations.SetBinding(thisControl.PowerPaddingControl, GenericControls.BooleanPropertyControl.IsSelectedProperty, powerPaddingBinding)
            Case GetType(Wpf.NormalProbabilityAxis)
                axisTypeComboBox.SelectedIndex = 2
                thisControl.AxisMinimum.Visibility = Visibility.Visible
                thisControl.AxisMaximum.Visibility = Visibility.Visible
                thisControl.AxisMinimum.DefaultNumber = 0.0000001
                thisControl.AxisMinimum.MinValue = _epsilon
                thisControl.AxisMinimum.MaxValue = 1 - _epsilon
                thisControl.AxisMaximum.DefaultNumber = 0.999
                thisControl.AxisMaximum.MinValue = _epsilon
                thisControl.AxisMaximum.MaxValue = 1 - _epsilon
                thisControl.LabelTypeSelector.Visibility = Visibility.Collapsed
                thisControl.DecimalPlaces.Visibility = Visibility.Collapsed
            Case GetType(Wpf.GumbelProbabilityAxis)
                axisTypeComboBox.SelectedIndex = 3
                thisControl.AxisMinimum.Visibility = Visibility.Visible
                thisControl.AxisMaximum.Visibility = Visibility.Visible
                thisControl.AxisMinimum.DefaultNumber = 0.0000001
                thisControl.AxisMinimum.MinValue = _epsilon
                thisControl.AxisMinimum.MaxValue = 1 - _epsilon
                thisControl.AxisMaximum.DefaultNumber = 0.99
                thisControl.AxisMaximum.MinValue = _epsilon
                thisControl.AxisMaximum.MaxValue = 1 - _epsilon
                thisControl.LabelTypeSelector.Visibility = Visibility.Collapsed
                thisControl.DecimalPlaces.Visibility = Visibility.Collapsed
            Case GetType(Wpf.CategoryAxis)
                thisControl.AxisTypeSelector.Visibility = Visibility.Collapsed

                thisControl.GapWidthSelector.Visibility = Visibility.Visible
                Dim gapWidthBinding As New Binding(NameOf(Wpf.CategoryAxis.GapWidth)) With {.Source = newAxis}
                BindingOperations.SetBinding(thisControl.GapWidthSelector, GenericControls.NumericPropertySelectorControl.SelectedNumberProperty, gapWidthBinding)

                thisControl.AxisLabelsControl.Visibility = Visibility.Visible
                If DirectCast(newAxis, Wpf.CategoryAxis).ItemsSource IsNot Nothing AndAlso (DirectCast(newAxis, Wpf.CategoryAxis).Labels Is Nothing OrElse DirectCast(newAxis, Wpf.CategoryAxis).Labels.Count = 0) Then
                    Dim axisLabelsBinding As New Binding(NameOf(Wpf.CategoryAxis.ItemsSource)) With {.Source = newAxis, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.TwoWay}
                    BindingOperations.SetBinding(thisControl.AxisLabelsControl, GenericControls.StringListPropertyControl.StringListProperty, axisLabelsBinding)
                Else
                    Dim axisLabelsBinding As New Binding(NameOf(Wpf.CategoryAxis.Labels)) With {.Source = newAxis, .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, .Mode = BindingMode.TwoWay}
                    BindingOperations.SetBinding(thisControl.AxisLabelsControl, GenericControls.StringListPropertyControl.StringListProperty, axisLabelsBinding)
                End If

                thisControl.TickCenteredControl.Visibility = Visibility.Visible
                Dim tickCenteredBinding As New Binding(NameOf(Wpf.CategoryAxis.IsTickCentered)) With {.Source = newAxis}
                BindingOperations.SetBinding(thisControl.TickCenteredControl, GenericControls.BooleanPropertyControl.IsSelectedProperty, tickCenteredBinding)

                thisControl.LabelTypeSelector.Visibility = Visibility.Collapsed
                thisControl.DecimalPlaces.Visibility = Visibility.Collapsed
            Case GetType(Wpf.DateTimeAxis)
                thisControl.DateAxisMinimum.Visibility = Visibility.Visible
                thisControl.DateAxisMaximum.Visibility = Visibility.Visible
                thisControl.AxisTypeSelector.Visibility = Visibility.Collapsed
                thisControl.LabelTypeSelector.Visibility = Visibility.Collapsed
                thisControl.DecimalPlaces.Visibility = Visibility.Collapsed
                'AddHandler DirectCast(newAxis.Parent, Wpf.Plot).ActualModel.Updated, AddressOf thisControl.ModelUpdated

                thisControl.LabelTypeSelector.Visibility = Visibility.Collapsed
                thisControl.DecimalPlaces.Visibility = Visibility.Collapsed
                'thisControl.CalendarWeekControl.Visibility = Visibility.Visible
                'Dim calendarWeekBinding As New Binding(NameOf(Wpf.DateTimeAxis.CalendarWeekRule)) With {.Source = newAxis}
                'BindingOperations.SetBinding(thisControl.CalendarWeekControl, GenericControls.CalendarWeekRulePropertyControl.SelectedCalendarWeekRuleProperty, calendarWeekBinding)

            Case Else
                isSupportedType = False
        End Select

        BindingOperations.GetBindingExpression(thisControl.AxisMinimum, thisControl.AxisMinimum.NumberProperty).UpdateSource()
        BindingOperations.GetBindingExpression(thisControl.AxisMinimum, thisControl.AxisMinimum.NumberProperty).UpdateTarget()
        BindingOperations.GetBindingExpression(thisControl.AxisMaximum, thisControl.AxisMaximum.NumberProperty).UpdateSource()
        BindingOperations.GetBindingExpression(thisControl.AxisMaximum, thisControl.AxisMaximum.NumberProperty).UpdateTarget()

        AddHandler axisTypeComboBox.SelectionChanged, AddressOf thisControl.AxisTypeComboBox_SelectionChanged
        '
        If isSupportedType Then
            axisTypeComboBox.IsEnabled = True
            axisTypeComboBox.Visibility = Visibility.Visible
        Else
            axisTypeComboBox.SelectedIndex = -1
            axisTypeComboBox.IsEnabled = False
            axisTypeComboBox.Visibility = Visibility.Collapsed
        End If


        Dim labelTypeComboBox As ComboBox = CType(thisControl.LabelTypeSelector.InnerContent, ComboBox)
        If IsNothing(labelTypeComboBox) Then Exit Sub
        RemoveHandler labelTypeComboBox.SelectionChanged, AddressOf thisControl.LabelType_SelectionChanged
        If newAxis.GetType() = GetType(Wpf.DateTimeAxis) OrElse newAxis.GetType() = GetType(Wpf.CategoryAxis) Then Return
        '
        Dim stringFormatCategory = ""
        Dim stringFormatDecimal = ""
        If newAxis.StringFormat IsNot Nothing AndAlso newAxis.StringFormat.Length > 0 Then
            stringFormatCategory = newAxis.StringFormat.Substring(0, 1)
            If stringFormatCategory = "C" OrElse stringFormatCategory = "c" Then
                labelTypeComboBox.SelectedIndex = 0
            ElseIf stringFormatCategory = "G" OrElse stringFormatCategory = "g" Then
                labelTypeComboBox.SelectedIndex = 1
            ElseIf stringFormatCategory = "N" OrElse stringFormatCategory = "n" Then
                labelTypeComboBox.SelectedIndex = 2
            ElseIf stringFormatCategory = "P" OrElse stringFormatCategory = "p" Then
                labelTypeComboBox.SelectedIndex = 3
            ElseIf stringFormatCategory = "E" OrElse stringFormatCategory = "e" Then
                labelTypeComboBox.SelectedIndex = 4
            Else
                labelTypeComboBox.SelectedIndex = 1
                stringFormatCategory = "G"
            End If
        Else
            labelTypeComboBox.SelectedIndex = 1
            stringFormatCategory = "G"
        End If
        If newAxis.StringFormat IsNot Nothing AndAlso newAxis.StringFormat.Length > 1 Then
            stringFormatDecimal = newAxis.StringFormat.Substring(1, newAxis.StringFormat.Length - 1)
            Double.TryParse(stringFormatDecimal, thisControl.DecimalPlaces.Number)
        End If
        If stringFormatDecimal = "" Then thisControl.DecimalPlaces.Number = Double.NaN
        thisControl._stringFormatCategory = stringFormatCategory
        thisControl._stringFormatDecimals = stringFormatDecimal
        AddHandler labelTypeComboBox.SelectionChanged, AddressOf thisControl.LabelType_SelectionChanged

    End Sub

    'Private Sub ModelUpdated(sender As Object, e As EventArgs)
    '    If Axis Is Nothing OrElse Axis.InternalAxis.GetType() <> GetType(Axes.DateTimeAxis) Then Return
    '    Dim dAxis = DirectCast(Axis.InternalAxis, Axes.DateTimeAxis)
    '    Dim Min As Double = dAxis.ActualMinimum
    '    Dim Max As Double = dAxis.ActualMaximum
    '    Dim DateSpan As TimeSpan = Axes.DateTimeAxis.ToDateTime(Max).Subtract(Axes.DateTimeAxis.ToDateTime(Min))
    '    With dAxis
    '        If DateSpan.TotalDays > 365 * 4 Then
    '            .StringFormat = "yyyy"
    '        ElseIf DateSpan.TotalDays > 365 Then
    '            .StringFormat = "MMM-yyyy"
    '        ElseIf DateSpan.TotalDays > 90 Then
    '            .StringFormat = "MMM-yyyy"
    '        ElseIf DateSpan.TotalDays > 2 Then
    '            .StringFormat = "dd-MMM"
    '        Else
    '            .StringFormat = "HH:mm"
    '        End If
    '    End With
    'End Sub

    Public Shared TabItemStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(TabItemStyle), GetType(Style), GetType(AxisControl), New PropertyMetadata(New Style(GetType(TabItem))))
    Public Property TabItemStyle As Style
        Get
            Return DirectCast(GetValue(TabItemStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(TabItemStyleProperty, value)
        End Set
    End Property

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(AxisControl), New PropertyMetadata(New Style(GetType(Expander))))
    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property


    Public Shared Function AxisPropertiesToXElement(axis As Wpf.Axis) As XElement
        Dim axisProperties As New XElement(AxisPropertiesTag)
        axisProperties.SetAttributeValue("AxisType", axis.GetType.ToString())
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'General
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim generalProperties As New XElement("General")
        generalProperties.SetAttributeValue(NameOf(Wpf.Axis.Name), If(IsNothing(axis.Name), "", axis.Name))
        generalProperties.SetAttributeValue(NameOf(axis.IsEnabled), axis.IsEnabled.ToString())
        generalProperties.SetAttributeValue(NameOf(axis.IsAxisVisible), axis.IsAxisVisible.ToString)
        generalProperties.SetAttributeValue(NameOf(axis.StartPosition), axis.StartPosition.ToString("G17", CultureInfo.InvariantCulture))
        generalProperties.SetAttributeValue(NameOf(axis.EndPosition), axis.EndPosition.ToString("G17", CultureInfo.InvariantCulture))
        generalProperties.SetAttributeValue(NameOf(axis.IsPanEnabled), axis.IsPanEnabled.ToString)
        generalProperties.SetAttributeValue(NameOf(axis.IsZoomEnabled), axis.IsZoomEnabled.ToString)
        axisProperties.Add(generalProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Numeric Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim numericProperties As New XElement("Numbers")
        numericProperties.SetAttributeValue(NameOf(axis.Maximum), axis.Maximum.ToString("G17", CultureInfo.InvariantCulture))
        numericProperties.SetAttributeValue(NameOf(axis.Minimum), axis.Minimum.ToString("G17", CultureInfo.InvariantCulture))
        numericProperties.SetAttributeValue(NameOf(axis.AbsoluteMaximum), axis.AbsoluteMaximum.ToString("G17", CultureInfo.InvariantCulture))
        numericProperties.SetAttributeValue(NameOf(axis.AbsoluteMinimum), axis.AbsoluteMinimum.ToString("G17", CultureInfo.InvariantCulture))
        numericProperties.SetAttributeValue(NameOf(axis.FilterMaxValue), axis.FilterMaxValue.ToString("G17", CultureInfo.InvariantCulture))
        numericProperties.SetAttributeValue(NameOf(axis.FilterMinValue), axis.FilterMinValue.ToString("G17", CultureInfo.InvariantCulture))
        axisProperties.Add(numericProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Style Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim styleProperties As New XElement("Style")
        styleProperties.SetAttributeValue(NameOf(axis.AxislineColor), axis.AxislineColor.ToString())
        styleProperties.SetAttributeValue(NameOf(axis.AxislineStyle), axis.AxislineStyle.ToString())
        styleProperties.SetAttributeValue(NameOf(axis.AxislineThickness), axis.AxislineThickness.ToString("G17", CultureInfo.InvariantCulture))
        axisProperties.Add(styleProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Position Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim positionProperties As New XElement("Position")
        positionProperties.SetAttributeValue(NameOf(axis.AxisDistance), axis.AxisDistance.ToString("G17", CultureInfo.InvariantCulture))
        positionProperties.SetAttributeValue(NameOf(axis.PositionAtZeroCrossing), axis.PositionAtZeroCrossing.ToString())
        positionProperties.SetAttributeValue(NameOf(axis.Position), axis.Position.ToString())
        positionProperties.SetAttributeValue(NameOf(axis.Key), axis.Key)
        positionProperties.SetAttributeValue(NameOf(axis.PositionTier), axis.PositionTier)
        axisProperties.Add(positionProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Title Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim weightConverter As New FontWeightConverter()
        Dim titleProperties As New XElement("Title")
        titleProperties.SetAttributeValue(NameOf(axis.Title), axis.Title)
        titleProperties.SetAttributeValue(NameOf(axis.TitleColor), axis.TitleColor.ToString()) 'ToString() returns the hexadecimal notation, use ColorConverter.ConvertFromString() to convert from hexidecimal to color.
        titleProperties.SetAttributeValue(NameOf(axis.TitleFont), axis.TitleFont)
        titleProperties.SetAttributeValue(NameOf(axis.TitleFontSize), axis.TitleFontSize.ToString("G17", CultureInfo.InvariantCulture))
        titleProperties.SetAttributeValue(NameOf(axis.TitleFontWeight), weightConverter.ConvertToInvariantString(axis.TitleFontWeight))
        titleProperties.SetAttributeValue(NameOf(axis.AxisTitleDistance), axis.AxisTitleDistance.ToString("G17", CultureInfo.InvariantCulture))
        titleProperties.SetAttributeValue(NameOf(axis.Unit), axis.Unit)
        axisProperties.Add(titleProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Label Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim labelProperties As New XElement("Labels")
        labelProperties.SetAttributeValue(NameOf(axis.TextColor), axis.TextColor.ToString()) 'ToString() returns the hexadecimal notation, use ColorConverter.ConvertFromString() to convert from hexidecimal to color.
        labelProperties.SetAttributeValue(NameOf(axis.Font), axis.Font)
        labelProperties.SetAttributeValue(NameOf(axis.FontSize), axis.FontSize.ToString("G17", CultureInfo.InvariantCulture))
        labelProperties.SetAttributeValue(NameOf(axis.FontWeight), weightConverter.ConvertToInvariantString(axis.FontWeight))
        labelProperties.SetAttributeValue(NameOf(axis.Angle), axis.Angle.ToString("G17", CultureInfo.InvariantCulture))
        labelProperties.SetAttributeValue(NameOf(axis.AxisTickToLabelDistance), axis.AxisTickToLabelDistance.ToString("G17", CultureInfo.InvariantCulture))
        labelProperties.SetAttributeValue(NameOf(axis.StringFormat), axis.StringFormat)
        labelProperties.SetAttributeValue(NameOf(axis.UseSuperExponentialFormat), axis.UseSuperExponentialFormat.ToString)
        axisProperties.Add(labelProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Major Gridline Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim majorGridlineProperties As New XElement("MajorGridlines")
        majorGridlineProperties.SetAttributeValue(NameOf(axis.MajorGridlineColor), axis.MajorGridlineColor.ToString()) 'ToString() returns the hexadecimal notation, use ColorConverter.ConvertFromString() to convert from hexidecimal to color.
        majorGridlineProperties.SetAttributeValue(NameOf(axis.MajorGridlineStyle), axis.MajorGridlineStyle.ToString())
        majorGridlineProperties.SetAttributeValue(NameOf(axis.MajorGridlineThickness), axis.MajorGridlineThickness.ToString("G17", CultureInfo.InvariantCulture))
        majorGridlineProperties.SetAttributeValue(NameOf(axis.MajorStep), axis.MajorStep.ToString("G17", CultureInfo.InvariantCulture))
        majorGridlineProperties.SetAttributeValue(NameOf(axis.MajorTickSize), axis.MajorTickSize.ToString("G17", CultureInfo.InvariantCulture))
        axisProperties.Add(majorGridlineProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Minor Gridline Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim minorGridlineProperties As New XElement("MinorGridlines")
        minorGridlineProperties.SetAttributeValue(NameOf(axis.MinorGridlineColor), axis.MinorGridlineColor.ToString()) 'ToString() returns the hexadecimal notation, use ColorConverter.ConvertFromString() to convert from hexidecimal to color.
        minorGridlineProperties.SetAttributeValue(NameOf(axis.MinorGridlineStyle), axis.MinorGridlineStyle.ToString())
        minorGridlineProperties.SetAttributeValue(NameOf(axis.MinorGridlineThickness), axis.MinorGridlineThickness.ToString("G17", CultureInfo.InvariantCulture))
        minorGridlineProperties.SetAttributeValue(NameOf(axis.MinorStep), axis.MinorStep.ToString("G17", CultureInfo.InvariantCulture))
        minorGridlineProperties.SetAttributeValue(NameOf(axis.MinorTickSize), axis.MinorTickSize.ToString("G17", CultureInfo.InvariantCulture))
        axisProperties.Add(minorGridlineProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Tick Style Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim tickStyleProperties As New XElement("Tick")
        tickStyleProperties.SetAttributeValue(NameOf(axis.TickStyle), axis.TickStyle.ToString())
        tickStyleProperties.SetAttributeValue(NameOf(axis.TicklineColor), axis.TicklineColor.ToString())
        axisProperties.Add(tickStyleProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Concrete axis implementation properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Select Case axis.GetType
            Case GetType(Wpf.LinearAxis)
                Dim linearAxis = DirectCast(axis, Wpf.LinearAxis)
                Dim linearAxisProperties As New XElement("LinearAxis")
                linearAxisProperties.SetAttributeValue(NameOf(linearAxis.FormatAsFractions), linearAxis.FormatAsFractions.ToString)
                axisProperties.Add(linearAxisProperties)
            Case GetType(Wpf.CategoryAxis)
                Dim categoryAxis = DirectCast(axis, Wpf.CategoryAxis)
                Dim categoryAxisProperties As New XElement("CategoryAxis")
                categoryAxisProperties.SetAttributeValue(NameOf(categoryAxis.IsTickCentered), categoryAxis.IsTickCentered.ToString)
                ' categoryAxisProperties.SetAttributeValue(NameOf(categoryAxis.Labels), String.Join("|", categoryAxis.Labels))
                categoryAxisProperties.SetAttributeValue(NameOf(categoryAxis.GapWidth), categoryAxis.GapWidth.ToString("G17", CultureInfo.InvariantCulture))
                axisProperties.Add(categoryAxisProperties)
            Case GetType(Wpf.LogarithmicAxis)
                Dim logAxis = TryCast(axis, Wpf.LogarithmicAxis)
                Dim logAxisProperties As New XElement("LogarithmicAxis")
                logAxisProperties.SetAttributeValue(NameOf(logAxis.Base), logAxis.Base.ToString("G17", CultureInfo.InvariantCulture))
                logAxisProperties.SetAttributeValue(NameOf(logAxis.PowerPadding), logAxis.PowerPadding.ToString)
                axisProperties.Add(logAxisProperties)
            Case GetType(Wpf.DateTimeAxis)
                Dim dateAxis = DirectCast(axis, Wpf.DateTimeAxis)
                Dim dateAxisProperties As New XElement("DateTimeAxis")
                dateAxisProperties.SetAttributeValue(NameOf(dateAxis.CalendarWeekRule), dateAxis.CalendarWeekRule.ToString)
                axisProperties.Add(dateAxisProperties)
        End Select

        Return axisProperties
    End Function
    '
    ''' <summary>
    ''' Load general plot property settings from XElement.
    ''' </summary>
    ''' <param name="element">XElement that contains the settings.</param>
    Public Shared Function XElementToAxisProperties(element As XElement, Optional ByVal targetAxis As Wpf.Axis = Nothing) As Wpf.Axis
        'Early Exit
        If element.Name <> AxisPropertiesTag Then Return Nothing
        'Set up converters
        Dim fontWeightConverter = New FontWeightConverter()
        'Set up Axis to return
        Dim axis As Wpf.Axis
        Dim axisType As String = ""
        If element.Attribute("AxisType") IsNot Nothing Then axisType = element.Attribute("AxisType").Value
        If targetAxis IsNot Nothing Then
            axis = targetAxis
        Else
            If axisType = GetType(Wpf.LinearAxis).ToString Then
                axis = New Wpf.LinearAxis()
            ElseIf axisType = GetType(Wpf.CategoryAxis).ToString Then
                axis = New Wpf.CategoryAxis()
            ElseIf axisType = GetType(Wpf.LogarithmicAxis).ToString Then
                axis = New Wpf.LogarithmicAxis()
            ElseIf axisType = GetType(Wpf.DateTimeAxis).ToString Then
                axis = New Wpf.DateTimeAxis()
            ElseIf axisType = GetType(Wpf.AngleAxis).ToString Then
                axis = New Wpf.AngleAxis()
            ElseIf axisType = GetType(Wpf.LinearColorAxis).ToString Then
                axis = New Wpf.LinearColorAxis()
            ElseIf axisType = GetType(Wpf.MagnitudeAxis).ToString Then
                axis = New Wpf.MagnitudeAxis()
            ElseIf axisType = GetType(Wpf.TimeSpanAxis).ToString Then
                axis = New Wpf.TimeSpanAxis()
            ElseIf axisType = GetType(Wpf.NormalProbabilityAxis).ToString Then
                axis = New Wpf.NormalProbabilityAxis()
            ElseIf axisType = GetType(Wpf.GumbelProbabilityAxis).ToString Then
                axis = New Wpf.GumbelProbabilityAxis()
            Else
                axis = New Wpf.LinearAxis()
            End If
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'General Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim generalElement = element.Element("General")
        If Not IsNothing(generalElement) Then
            GetStringAttribute(generalElement, NameOf(axis.Name), axis.Name)
            GetBooleanAttribute(generalElement, NameOf(axis.IsEnabled), axis.IsEnabled)
            GetBooleanAttribute(generalElement, NameOf(axis.IsAxisVisible), axis.IsAxisVisible)
            GetDoubleAttribute(generalElement, NameOf(axis.StartPosition), axis.StartPosition)
            GetDoubleAttribute(generalElement, NameOf(axis.EndPosition), axis.EndPosition)
            GetBooleanAttribute(generalElement, NameOf(axis.IsPanEnabled), axis.IsPanEnabled)
            GetBooleanAttribute(generalElement, NameOf(axis.IsZoomEnabled), axis.IsZoomEnabled)

            'Backward compatibility
            GetBooleanAttribute(generalElement, "AxisVisible", axis.IsAxisVisible)
            GetBooleanAttribute(generalElement, "CanPan", axis.IsPanEnabled)
            GetBooleanAttribute(generalElement, "CanZoom", axis.IsZoomEnabled)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Number Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim numbersElement = element.Element("Numbers")
        If Not IsNothing(numbersElement) Then
            GetDoubleAttribute(numbersElement, NameOf(axis.Maximum), axis.Maximum)
            GetDoubleAttribute(numbersElement, NameOf(axis.Minimum), axis.Minimum)
            GetDoubleAttribute(numbersElement, NameOf(axis.AbsoluteMaximum), axis.AbsoluteMaximum)
            GetDoubleAttribute(numbersElement, NameOf(axis.AbsoluteMinimum), axis.AbsoluteMinimum)
            GetDoubleAttribute(numbersElement, NameOf(axis.FilterMaxValue), axis.FilterMaxValue)
            GetDoubleAttribute(numbersElement, NameOf(axis.FilterMinValue), axis.FilterMinValue)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Style Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim styleElement = element.Element("Style")
        If Not IsNothing(styleElement) Then
            GetColorAttribute(styleElement, NameOf(axis.AxislineColor), axis.AxislineColor)
            GetEnumAttribute(styleElement, NameOf(axis.AxislineStyle), axis.AxislineStyle)
            GetDoubleAttribute(styleElement, NameOf(axis.AxislineThickness), axis.AxislineThickness)

            ' Backward compatibilty
            GetColorAttribute(styleElement, "Color", axis.AxislineColor)
            GetEnumAttribute(styleElement, "Style", axis.AxislineStyle)
            GetDoubleAttribute(styleElement, "Thickness", axis.AxislineThickness)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Position Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim positionElement = element.Element("Position")
        If Not IsNothing(positionElement) Then
            GetDoubleAttribute(positionElement, NameOf(axis.AxisDistance), axis.AxisDistance)
            GetBooleanAttribute(positionElement, NameOf(axis.PositionAtZeroCrossing), axis.PositionAtZeroCrossing)
            GetEnumAttribute(positionElement, NameOf(axis.Position), axis.Position)
            GetStringAttribute(positionElement, NameOf(axis.Key), axis.Key)
            GetIntegerAttribute(positionElement, NameOf(axis.PositionTier), axis.PositionTier)

            ' Backward compatibilty
            GetDoubleAttribute(positionElement, "Distance", axis.AxisDistance)
            GetBooleanAttribute(positionElement, "ZeroCrossing", axis.PositionAtZeroCrossing)
            GetIntegerAttribute(positionElement, "Tier", axis.PositionTier)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Title Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim titleElement = element.Element("Title")
        If Not IsNothing(titleElement) Then
            GetStringAttribute(titleElement, NameOf(axis.Title), axis.Title)
            GetColorAttribute(titleElement, NameOf(axis.TitleColor), axis.TitleColor)
            GetStringAttribute(titleElement, NameOf(axis.TitleFont), axis.TitleFont)
            GetDoubleAttribute(titleElement, NameOf(axis.TitleFontSize), axis.TitleFontSize)
            GetFontWeightAttribute(titleElement, NameOf(axis.TitleFontWeight), fontWeightConverter, axis.TitleFontWeight)
            GetDoubleAttribute(titleElement, NameOf(axis.AxisTitleDistance), axis.AxisTitleDistance)
            GetStringAttribute(titleElement, NameOf(axis.Unit), axis.Unit)

            ' Backward compatibilty
            GetColorAttribute(titleElement, "Color", axis.TitleColor)
            GetStringAttribute(titleElement, "Font", axis.TitleFont)
            GetDoubleAttribute(titleElement, "Size", axis.TitleFontSize)
            GetFontWeightAttribute(titleElement, "Weight", fontWeightConverter, axis.TitleFontWeight)
            GetDoubleAttribute(titleElement, "Distance", axis.AxisTitleDistance)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Label Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim labelElement = element.Element("Labels")
        If Not IsNothing(labelElement) Then
            GetColorAttribute(labelElement, NameOf(axis.TextColor), axis.TextColor)
            GetStringAttribute(labelElement, NameOf(axis.Font), axis.Font)
            GetDoubleAttribute(labelElement, NameOf(axis.FontSize), axis.FontSize)
            GetFontWeightAttribute(labelElement, NameOf(axis.FontWeight), fontWeightConverter, axis.FontWeight)
            GetDoubleAttribute(labelElement, NameOf(axis.Angle), axis.Angle)
            GetDoubleAttribute(labelElement, NameOf(axis.AxisTickToLabelDistance), axis.AxisTickToLabelDistance)
            GetStringAttribute(labelElement, NameOf(axis.StringFormat), axis.StringFormat)
            GetBooleanAttribute(labelElement, NameOf(axis.UseSuperExponentialFormat), axis.UseSuperExponentialFormat)

            ' Backward compatiblity
            GetColorAttribute(labelElement, "Color", axis.TextColor)
            GetDoubleAttribute(labelElement, "Size", axis.FontSize)
            GetFontWeightAttribute(labelElement, "Weight", fontWeightConverter, axis.FontWeight)
            GetDoubleAttribute(labelElement, "TickDistance", axis.AxisTickToLabelDistance)
            GetBooleanAttribute(labelElement, "Superscript", axis.UseSuperExponentialFormat)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Major Gridline Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim majorGridlineElement = element.Element("MajorGridlines")
        If Not IsNothing(majorGridlineElement) Then
            GetColorAttribute(majorGridlineElement, NameOf(axis.MajorGridlineColor), axis.MajorGridlineColor)
            GetEnumAttribute(majorGridlineElement, NameOf(axis.MajorGridlineStyle), axis.MajorGridlineStyle)
            GetDoubleAttribute(majorGridlineElement, NameOf(axis.MajorGridlineThickness), axis.MajorGridlineThickness)
            GetDoubleAttribute(majorGridlineElement, NameOf(axis.MajorStep), axis.MajorStep)
            GetDoubleAttribute(majorGridlineElement, NameOf(axis.MajorTickSize), axis.MajorTickSize)

            ' Backward compatibility
            GetColorAttribute(majorGridlineElement, "Color", axis.MajorGridlineColor)
            GetEnumAttribute(majorGridlineElement, "Style", axis.MajorGridlineStyle)
            GetDoubleAttribute(majorGridlineElement, "Thickness", axis.MajorGridlineThickness)
            GetDoubleAttribute(majorGridlineElement, "Step", axis.MajorStep)
            GetDoubleAttribute(majorGridlineElement, "TickSize", axis.MajorTickSize)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Minor Gridline Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim minorGridlineElement = element.Element("MinorGridlines")
        If Not IsNothing(minorGridlineElement) Then
            GetColorAttribute(minorGridlineElement, NameOf(axis.MinorGridlineColor), axis.MinorGridlineColor)
            GetEnumAttribute(minorGridlineElement, NameOf(axis.MinorGridlineStyle), axis.MinorGridlineStyle)
            GetDoubleAttribute(minorGridlineElement, NameOf(axis.MinorGridlineThickness), axis.MinorGridlineThickness)
            GetDoubleAttribute(minorGridlineElement, NameOf(axis.MinorStep), axis.MinorStep)
            GetDoubleAttribute(minorGridlineElement, NameOf(axis.MinorTickSize), axis.MinorTickSize)

            ' Backward compatibility
            GetColorAttribute(minorGridlineElement, "Color", axis.MinorGridlineColor)
            GetEnumAttribute(minorGridlineElement, "Style", axis.MinorGridlineStyle)
            GetDoubleAttribute(minorGridlineElement, "Thickness", axis.MinorGridlineThickness)
            GetDoubleAttribute(minorGridlineElement, "Step", axis.MinorStep)
            GetDoubleAttribute(minorGridlineElement, "TickSize", axis.MinorTickSize)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Tick Style Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim tickStyleElement = element.Element("Tick")
        If Not IsNothing(tickStyleElement) Then
            GetColorAttribute(tickStyleElement, NameOf(axis.TicklineColor), axis.TicklineColor)
            GetEnumAttribute(tickStyleElement, NameOf(axis.TickStyle), axis.TickStyle)

            ' Backward compatibility
            GetColorAttribute(tickStyleElement, "Color", axis.TicklineColor)
            GetEnumAttribute(tickStyleElement, "Style", axis.TickStyle)
        End If
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Concrete axis implementation properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Select Case axis.GetType()
            Case GetType(Wpf.LinearAxis)
                Dim linearAxisElement = element.Element("LinearAxis")
                If Not IsNothing(linearAxisElement) Then
                    GetBooleanAttribute(linearAxisElement, NameOf(Wpf.LinearAxis.FormatAsFractions), DirectCast(axis, Wpf.LinearAxis).FormatAsFractions)
                    ' Backward compatibility
                    GetBooleanAttribute(linearAxisElement, "FractionFormat", DirectCast(axis, Wpf.LinearAxis).FormatAsFractions)
                End If
            Case GetType(Wpf.CategoryAxis)
                Dim categoryAxisElement = element.Element("CategoryAxis")
                If Not IsNothing(categoryAxisElement) Then
                    GetBooleanAttribute(categoryAxisElement, NameOf(Wpf.CategoryAxis.IsTickCentered), DirectCast(axis, Wpf.CategoryAxis).IsTickCentered)
                    'GetStringArrayAttribute(categoryAxisElement, NameOf(Wpf.CategoryAxis.Labels), DirectCast(axis, Wpf.CategoryAxis).Labels)
                    GetDoubleAttribute(categoryAxisElement, NameOf(Wpf.CategoryAxis.GapWidth), DirectCast(axis, Wpf.CategoryAxis).GapWidth)

                    ' Backward compatibility
                    GetBooleanAttribute(categoryAxisElement, "Centered", DirectCast(axis, Wpf.CategoryAxis).IsTickCentered)
                End If
            Case GetType(Wpf.LogarithmicAxis)
                Dim logAxisElement = element.Element("LogarithmicAxis")
                If Not IsNothing(logAxisElement) Then
                    GetDoubleAttribute(logAxisElement, NameOf(Wpf.LogarithmicAxis.Base), DirectCast(axis, Wpf.LogarithmicAxis).Base)
                    GetBooleanAttribute(logAxisElement, NameOf(Wpf.LogarithmicAxis.PowerPadding), DirectCast(axis, Wpf.LogarithmicAxis).PowerPadding)

                    ' Backward compatibility
                    GetDoubleAttribute(logAxisElement, "LogBase", DirectCast(axis, Wpf.LogarithmicAxis).Base)
                End If
            Case GetType(Wpf.DateTimeAxis)
                Dim dateAxisElement = element.Element("DateTimeAxis")
                If Not IsNothing(dateAxisElement) Then
                    GetEnumAttribute(dateAxisElement, NameOf(Wpf.DateTimeAxis.CalendarWeekRule), DirectCast(axis, Wpf.DateTimeAxis).CalendarWeekRule)

                    ' Backward compatibility
                    GetEnumAttribute(dateAxisElement, "CalendarWeek", DirectCast(axis, Wpf.DateTimeAxis).CalendarWeekRule)
                End If
        End Select
        '
        Return axis
    End Function

    Public Shared Function ConvertAxisToLogarithmicAxis(wpfAxis As Wpf.Axis, Optional logBase As Double = 10, Optional powerPadding As Boolean = True) As Wpf.LogarithmicAxis
        Dim newAxis As New Wpf.LogarithmicAxis() With {.Base = logBase, .PowerPadding = powerPadding}
        newAxis.FromAxisProperties(wpfAxis)
        If newAxis.Minimum <= 0 Then newAxis.Minimum = _epsilon
        newAxis.Maximum = wpfAxis.Maximum
        newAxis.StartPosition = wpfAxis.StartPosition
        newAxis.EndPosition = wpfAxis.EndPosition
        '
        Return newAxis
    End Function
    Public Shared Function ConvertAxisToLinearAxis(wpfAxis As Wpf.Axis, Optional formatAsFractions As Boolean = False, Optional fractionUnits As Double = 1, Optional fractionSymbol As String = Nothing) As Wpf.LinearAxis
        Dim newAxis As New Wpf.LinearAxis() With {.FormatAsFractions = formatAsFractions, .FractionUnit = fractionUnits, .FractionUnitSymbol = fractionSymbol}
        newAxis.FromAxisProperties(wpfAxis)
        newAxis.Minimum = wpfAxis.Minimum
        newAxis.Maximum = wpfAxis.Maximum
        newAxis.StartPosition = wpfAxis.StartPosition
        newAxis.EndPosition = wpfAxis.EndPosition
        Return newAxis
    End Function
    Public Shared Function ConvertAxisToNormalAxis(wpfAxis As Wpf.Axis) As Wpf.NormalProbabilityAxis
        Dim newAxis As New Wpf.NormalProbabilityAxis()
        newAxis.FromAxisProperties(wpfAxis)
        If newAxis.Minimum < 0.0000000000000001 Then newAxis.Minimum = 0.0000001
        If newAxis.Maximum > 0.999 OrElse Double.IsNaN(newAxis.Maximum) Then newAxis.Maximum = 0.999
        newAxis.StartPosition = wpfAxis.StartPosition
        newAxis.EndPosition = wpfAxis.EndPosition
        '
        Return newAxis
    End Function
    Public Shared Function ConvertAxisToGumbelAxis(wpfAxis As Wpf.Axis) As Wpf.GumbelProbabilityAxis
        Dim newAxis As New Wpf.GumbelProbabilityAxis()
        newAxis.FromAxisProperties(wpfAxis)
        If newAxis.Minimum < 0.0000000000000001 Then newAxis.Minimum = 0.0000001
        If newAxis.Maximum > 0.99 OrElse Double.IsNaN(newAxis.Maximum) Then newAxis.Maximum = 0.99
        newAxis.StartPosition = wpfAxis.StartPosition
        newAxis.EndPosition = wpfAxis.EndPosition
        '
        Return newAxis
    End Function

    Public Shared Function ConvertAxisToDateTimeAxis(wpfAxis As Wpf.Axis) As Wpf.DateTimeAxis
        Dim newAxis As New Wpf.DateTimeAxis()
        newAxis.FromAxisProperties(wpfAxis)
        newAxis.Minimum = wpfAxis.Minimum
        newAxis.Maximum = wpfAxis.Maximum
        newAxis.StartPosition = wpfAxis.StartPosition
        newAxis.EndPosition = wpfAxis.EndPosition
        '
        Return newAxis
    End Function



    ''' <summary>
    ''' When the axis is changed, set up the min and max extents. 
    ''' </summary>
    Private Sub AxisTypeComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If IsNothing(sender) Then Exit Sub
        '
        Dim axisTypeComboBox As ComboBox = CType(sender, ComboBox)
        If IsNothing(axisTypeComboBox) Then Exit Sub
        If axisTypeComboBox.SelectedIndex = -1 Then Exit Sub
        If Axis Is Nothing Then Exit Sub
        If Axis.Parent Is Nothing Then Exit Sub
        If Axis.Parent.GetType <> GetType(Wpf.Plot) Then Exit Sub
        '
        Select Case Axis.GetType()
            Case GetType(Wpf.LinearAxis)
                _oldLinearAxis = DirectCast(Axis, Wpf.LinearAxis)
            Case GetType(Wpf.LogarithmicAxis)
                _oldLogAxis = DirectCast(Axis, Wpf.LogarithmicAxis)
            Case GetType(Wpf.NormalProbabilityAxis)
            Case GetType(Wpf.GumbelProbabilityAxis)

        End Select

        Dim thePlot = DirectCast(Axis.Parent, Wpf.Plot)
        Dim newAxis As Wpf.Axis
        'Unfortunately we lose the axis specific properties (e.g. LogBase for logarithmic axis).
        Select Case axisTypeComboBox.SelectedIndex
            Case 0 ' Linear

                ' Set min and max allowable values.
                AxisMinimum.DefaultNumber = Double.NaN
                AxisMinimum.MinValue = Double.MinValue
                AxisMinimum.MaxValue = Double.MaxValue
                AxisMaximum.DefaultNumber = Double.NaN
                AxisMaximum.MinValue = Double.MinValue
                AxisMaximum.MaxValue = Double.MaxValue

                If Axis.GetType() = GetType(Wpf.LinearAxis) Then Exit Sub
                newAxis = ConvertAxisToLinearAxis(Axis)

                LabelTypeSelector.Visibility = Visibility.Visible
                DecimalPlaces.Visibility = Visibility.Visible

            Case 1 ' Logarithmic

                ' Set min and max allowable values.
                AxisMinimum.DefaultNumber = Double.NaN
                AxisMinimum.MinValue = _epsilon
                AxisMinimum.MaxValue = Double.MaxValue
                AxisMaximum.DefaultNumber = Double.NaN
                AxisMaximum.MinValue = _epsilon
                AxisMaximum.MaxValue = Double.MaxValue

                If Axis.GetType() = GetType(Wpf.LogarithmicAxis) Then Exit Sub
                newAxis = ConvertAxisToLogarithmicAxis(Axis)

                LabelTypeSelector.Visibility = Visibility.Visible
                DecimalPlaces.Visibility = Visibility.Visible

            Case 2 ' Normal Probability

                If Axis.InternalAxis.DataMinimum < 0 OrElse Axis.InternalAxis.DataMaximum > 1 Then
                    MessageBox.Show("Axis cannot be converted to a Normal probability axis because the data is not between 0 and 1.", "Normal Probability Axis", MessageBoxButton.OK, MessageBoxImage.Error)
                    axisTypeComboBox.SelectedItem = e.RemovedItems.Item(0)
                    e.Handled = True
                    Exit Sub
                End If

                ' Set min and max allowable values.
                AxisMinimum.DefaultNumber = 0.0000001
                AxisMinimum.MinValue = _epsilon
                AxisMinimum.MaxValue = 1 - _epsilon
                AxisMaximum.DefaultNumber = 0.999
                AxisMaximum.MinValue = _epsilon
                AxisMaximum.MaxValue = 1 - _epsilon

                If Axis.GetType() = GetType(Wpf.NormalProbabilityAxis) Then Exit Sub
                newAxis = ConvertAxisToNormalAxis(Axis)

                LabelTypeSelector.Visibility = Visibility.Collapsed
                DecimalPlaces.Visibility = Visibility.Collapsed

            Case 3 ' Gumbel Probability

                If Axis.InternalAxis.DataMinimum < 0 OrElse Axis.InternalAxis.DataMaximum > 1 Then
                    MessageBox.Show("Axis cannot be converted to a Gumbel probability axis because the data is not between 0 and 1.", "Gumbel Probability Axis", MessageBoxButton.OK, MessageBoxImage.Error)
                    axisTypeComboBox.SelectedItem = e.RemovedItems.Item(0)
                    e.Handled = True
                    Exit Sub
                End If

                ' Set min and max allowable values.
                AxisMinimum.DefaultNumber = 0.0000001
                AxisMinimum.MinValue = _epsilon
                AxisMinimum.MaxValue = 1 - _epsilon
                AxisMaximum.DefaultNumber = 0.99
                AxisMaximum.MinValue = _epsilon
                AxisMaximum.MaxValue = 1 - _epsilon

                If Axis.GetType() = GetType(Wpf.GumbelProbabilityAxis) Then Exit Sub
                newAxis = ConvertAxisToGumbelAxis(Axis)

                LabelTypeSelector.Visibility = Visibility.Collapsed
                DecimalPlaces.Visibility = Visibility.Collapsed
            Case 4 ' Date Time

                If Axis.GetType() = GetType(Wpf.DateTimeAxis) Then Exit Sub
                newAxis = ConvertAxisToDateTimeAxis(Axis)
                ' If Axis.InternalAxis.DataMinimum <
            Case Else
                Exit Sub
        End Select
        '
        thePlot.Axes.Remove(Axis)
        thePlot.Axes.Add(newAxis)
        Axis = newAxis
        '
        RaiseEvent AxisTypeChanged(Axis, newAxis)
        '        
        thePlot.InvalidatePlot()
    End Sub

    Public Sub CloseExpanders()
        GeneralEXP.IsExpanded = False
        LabelsEXP.IsExpanded = False
        'NumericalEXP.IsExpanded = False
        'AxisStyeEXP.IsExpanded = False
        'AxisPositionEXP.IsExpanded = False
        TitleEXP.IsExpanded = False
        MajorGridLinesEXP.IsExpanded = False
        MinorGridLinesEXP.IsExpanded = False
        TickOptionsEXP.IsExpanded = False
    End Sub

    '''' <summary>
    '''' Handles when the log base control number changes. 
    '''' </summary>
    'Private Sub LogBaseControl_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
    '    If e.PropertyName = NameOf(GenericControls.NumericPropertySelectorControl.SelectedNumber) Then
    '        ' The log base cannot be less than or equal to 1.
    '        If LogBaseControl.SelectedNumber <= 1 Then
    '            LogBaseControl.SelectedNumber = 2
    '        End If
    '    End If
    'End Sub

    Private Sub AxisMinimum_PreviewNumberChanged(oldValue As Object, newValue As Object, ByRef cancel As Boolean)
        If _ignoreMaxMinChange = True Then Exit Sub
        Dim newNumber As Double
        If Double.TryParse(newValue.ToString, newNumber) = False Then
            If newValue.GetType <> GetType(Double) Then
                Exit Sub
            End If
            newNumber = DirectCast(newValue, Double)
        End If
        If newNumber >= AxisMaximum.Number Then
            Dim oldNumber As Double = DirectCast(oldValue, Double)
            _ignoreMaxMinChange = True
            AxisMinimum.Number = oldNumber
            _ignoreMaxMinChange = False
            cancel = True
        End If
    End Sub

    Private Sub AxisMaximum_PreviewNumberChanged(oldValue As Object, newValue As Object, ByRef cancel As Boolean)
        If _ignoreMaxMinChange = True Then Exit Sub
        Dim newNumber As Double
        If Double.TryParse(newValue.ToString, newNumber) = False Then
            If newValue.GetType <> GetType(Double) Then
                Exit Sub
            End If
            newNumber = DirectCast(newValue, Double)
        End If
        If newNumber <= AxisMinimum.Number Then
            Dim oldNumber As Double = DirectCast(oldValue, Double)
            _ignoreMaxMinChange = True
            AxisMaximum.Number = oldNumber
            _ignoreMaxMinChange = False
            cancel = True
        End If
    End Sub


    Private _stringFormatCategory As String = ""
    Private _stringFormatDecimals As String = ""

    Private Sub LabelType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If IsNothing(sender) Then Exit Sub
        Dim comboBox As ComboBox = CType(sender, ComboBox)
        If IsNothing(comboBox) Then Exit Sub
        If comboBox.SelectedIndex = -1 Then Exit Sub
        If IsNothing(Axis.Parent) Then Exit Sub
        If Axis.Parent.GetType <> GetType(Wpf.Plot) Then Exit Sub
        '
        Select Case comboBox.SelectedIndex
            Case 0 ' Currency
                _stringFormatCategory = "C"
            Case 1 ' General
                _stringFormatCategory = "G"
            Case 2 ' Number
                _stringFormatCategory = "N"
            Case 3 ' Percent
                _stringFormatCategory = "P"
            Case 4 ' Scientific
                _stringFormatCategory = "E"
        End Select
        Axis.StringFormat = _stringFormatCategory + _stringFormatDecimals
    End Sub

    Private Sub DecimalPlaces_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName = NameOf(GenericControls.NumericAutoPropertyControl.Number) Then
            If DecimalPlaces.Number.ToString() = "NaN" Then
                _stringFormatDecimals = ""
            Else
                _stringFormatDecimals = DecimalPlaces.Number.ToString()
            End If
            Axis.StringFormat = _stringFormatCategory + _stringFormatDecimals
        End If
    End Sub

    Private Sub DecimalPlaces_PreviewNumberChanged(oldValue As Object, newValue As Object, ByRef cancel As Boolean)
        Dim newNumber As Double
        If Double.TryParse(newValue.ToString, newNumber) = False Then
            If newValue.GetType <> GetType(Double) Then
                Exit Sub
            End If
            newNumber = DirectCast(newValue, Double)
        End If
        DecimalPlaces.Number = CDbl(Math.Floor(newNumber))
    End Sub
End Class

Public Class ReverseAxisConverter
    Implements IMultiValueConverter

    Private _axis As Wpf.Axis

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Start and End Positions
        Dim startPosition As Double
        Double.TryParse(values(0).ToString, startPosition)
        Dim endPosition As Double
        Double.TryParse(values(1).ToString, endPosition)
        'Get Axis (this should only be set on convert with one-way binding)
        If IsNothing(values(2)) Then _axis = Nothing : Return False
        _axis = TryCast(values(2), Wpf.Axis)
        '
        Return endPosition < startPosition
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        If IsNothing(_axis) Then Return {CDbl(0), CDbl(1), Nothing}
        'Get boolean value
        Dim result As Boolean
        Boolean.TryParse(value.ToString(), result)
        'Test to see if start and end need to be swapped.
        Dim switchPositions As Boolean = False
        If result = False Then
            switchPositions = _axis.StartPosition > _axis.EndPosition
        Else
            switchPositions = _axis.StartPosition < _axis.EndPosition
        End If
        'return results
        If switchPositions = True Then Return {_axis.EndPosition, _axis.StartPosition, _axis}
        '
        Return {_axis.StartPosition, _axis.EndPosition, _axis}
    End Function
End Class

Public Class OxyLineStyleToDashArrayConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If IsNothing(value) Then Return value
        If value.GetType <> GetType(OxyPlot.LineStyle) Then Return New DoubleCollection()
        '
        Dim lineStyle = DirectCast(value, OxyPlot.LineStyle)
        If lineStyle = LineStyle.Solid Then Return New DoubleCollection({})
        Dim dashArray = lineStyle.GetDashArray()
        If IsNothing(dashArray) Then Return New DoubleCollection({0})
        Return New DoubleCollection(dashArray)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If IsNothing(value) Then Return OxyPlot.LineStyle.None
        If value.GetType <> GetType(DoubleCollection) Then Return OxyPlot.LineStyle.None
        Dim dashArray As DoubleCollection = DirectCast(value, DoubleCollection)
        '
        If dashArray.Count = 0 Then Return OxyPlot.LineStyle.Solid
        '
        For Each style In DirectCast([Enum].GetValues(GetType(OxyPlot.LineStyle)), OxyPlot.LineStyle())
            Dim oxyArray = style.GetDashArray()
            If Not IsNothing(oxyArray) Then If dashArray.SequenceEqual(oxyArray) Then Return style
        Next
        '
        Return OxyPlot.LineStyle.None
    End Function
End Class
Public Class EmptyStringToNullConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If IsNothing(value) Then Return value
        'If value.ToString = "" Then Return Nothing
        '
        Return value.ToString
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If IsNothing(value) Then Return Nothing
        If value.ToString = "" Then Return Nothing
        Return value
    End Function
End Class

'Public Class TabItemSizeConverter
'    Implements IValueConverter

'    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
'        Dim width As Double = Double.Parse(value.ToString())
'        If width < 12 Then Return 0
'        Return 0.33333333333 * width - 4
'    End Function

'    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
'        Throw New NotSupportedException()
'    End Function
'End Class

Public Class VerticalTabSizeConverter
    Implements IMultiValueConverter

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        Dim tabControl As TabControl = CType(values(0), TabControl)
        Dim height As Double = (tabControl.ActualHeight / tabControl.Items.Count)
        If height < 12 Then Return 0
        Return height - 1 '- (1 * tabControl.Items.Count + 1)
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class

Public Class DateToNumberConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If value Is Nothing OrElse value.GetType() <> GetType(Double) Then Return value

        Return OxyPlot.Axes.DateTimeAxis.ToDateTime(DirectCast(value, Double))
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If value Is Nothing OrElse value.GetType() <> GetType(DateTime) Then Return value

        Dim dt = DirectCast(value, DateTime)
        If dt.Equals(DateTime.MinValue) Then Return Double.NaN
        Return OxyPlot.Axes.DateTimeAxis.ToDouble(DirectCast(value, DateTime))
    End Function
End Class