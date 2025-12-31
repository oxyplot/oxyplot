Imports OxyPlot

Public Class AxesControl
    Public Shared ReadOnly AxesPropertiesTag As String = "Axes"

    Public Shared PlotProperty As DependencyProperty = DependencyProperty.Register(NameOf(Plot), GetType(Wpf.Plot), GetType(AxesControl), New PropertyMetadata(Nothing, AddressOf InitializePlot))
    Public Property Plot As Wpf.Plot
        Get
            Return DirectCast(GetValue(PlotProperty), Wpf.Plot)
        End Get
        Set(value As Wpf.Plot)
            SetValue(PlotProperty, value)
        End Set
    End Property

    Public Shared SelectedAxisProperty As DependencyProperty = DependencyProperty.Register(NameOf(SelectedAxis), GetType(Wpf.Axis), GetType(AxesControl), New PropertyMetadata(Nothing))
    Public Property SelectedAxis As Wpf.Axis
        Get
            Return DirectCast(GetValue(SelectedAxisProperty), Wpf.Axis)
        End Get
        Set(value As Wpf.Axis)
            SetValue(SelectedAxisProperty, value)
        End Set
    End Property

    Public Shared TitleMinWidthProp As DependencyProperty = DependencyProperty.Register(NameOf(TitleMinWidth), GetType(Integer), GetType(AxesControl), New UIPropertyMetadata(110))
    Public Property TitleMinWidth As Integer
        Get
            Return DirectCast(GetValue(TitleMinWidthProp), Integer)
        End Get
        Set(value As Integer)
            SetValue(TitleMinWidthProp, value)
        End Set
    End Property

    Public Shared LeaderLinesVisibilityProp As DependencyProperty = DependencyProperty.Register(NameOf(LeaderLinesVisibility), GetType(Visibility), GetType(AxesControl), New UIPropertyMetadata(Visibility.Visible))
    Public Property LeaderLinesVisibility As Visibility
        Get
            Return DirectCast(GetValue(LeaderLinesVisibilityProp), Visibility)
        End Get
        Set(value As Visibility)
            SetValue(LeaderLinesVisibilityProp, value)
        End Set
    End Property

    Public Shared TabItemStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(TabItemStyle), GetType(Style), GetType(AxesControl))
    Public Property TabItemStyle As Style
        Get
            Return DirectCast(GetValue(TabItemStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(TabItemStyleProperty, value)
        End Set
    End Property


    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(AxesControl))
    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property

    Public Shared ComboBoxStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ComboBoxStyle), GetType(Style), GetType(AxesControl), New PropertyMetadata(Nothing))
    Public Property ComboBoxStyle As Style
        Get
            Return DirectCast(GetValue(ComboBoxStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ComboBoxStyleProperty, value)
        End Set
    End Property
    Private Sub SetDefaultComboboxStyle()
        ComboBoxStyle = CType(FindResource("CleanComboBoxStyle"), Style)
    End Sub

    Private Shared Sub InitializePlot(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsNothing(d) Then Exit Sub
        If d.GetType <> GetType(AxesControl) Then Exit Sub
        Dim thisControl = DirectCast(d, AxesControl)
        '
        thisControl.AxesPropertyControlComboBox.ItemsSource = Nothing
        If IsNothing(e.NewValue) Then Exit Sub
        If e.NewValue.GetType <> GetType(Wpf.Plot) Then Exit Sub
        Dim newPlot As Wpf.Plot = DirectCast(e.NewValue, Wpf.Plot)
        '

        If IsNothing(thisControl.ComboBoxStyle) Then thisControl.SetDefaultComboboxStyle()
        thisControl.AxesPropertyControlComboBox.ItemsSource = newPlot.Axes
        '
        thisControl.AxesPropertyControlComboBox.ApplyTemplate()
        Dim t = thisControl.AxesPropertyControlComboBox.FindResource("ComboBoxTemplate")
        'Dim v = DirectCast(t, ControlTemplate).FindName("SpecialOptions", thisControl.AxesPropertyControlComboBox)
        'Dim cntrl = DirectCast(v, ItemsControl) 'thisControl.AxesPropertyControlComboBox.GetSpecialOptionsItemsControl
        'If Not IsNothing(cntrl) AndAlso cntrl.Items.Count = 0 Then
        'cntrl.Items.Add(New Separator())
        ''
        'Dim addLinear As New ComboBoxItem() With {.Content = "Add Linear Axis", .FontStyle = FontStyles.Italic}
        'AddHandler addLinear.PreviewMouseLeftButtonUp, Sub()
        '                                                   thisControl.Plot.Axes.Add(New Wpf.LinearAxis() With {.Title = "Linear Axis Title"})
        '                                                   thisControl.AxesPropertyControlComboBox.SelectedItem = thisControl.Plot.Axes.Last
        '                                                   thisControl.AxesPropertyControlComboBox.IsDropDownOpen = False
        '                                                   thisControl.AxisPropertiesControl.Focus()
        '                                               End Sub
        'cntrl.Items.Add(addLinear)
        ''
        'Dim addLog As New ComboBoxItem() With {.Content = "Add Logarithmic Axis", .FontStyle = FontStyles.Italic}
        'AddHandler addLog.PreviewMouseLeftButtonUp, Sub()
        '                                                thisControl.Plot.Axes.Add(New Wpf.LogarithmicAxis() With {.Title = "Log Axis Title"})
        '                                                thisControl.AxesPropertyControlComboBox.SelectedItem = thisControl.Plot.Axes.Last
        '                                                thisControl.AxesPropertyControlComboBox.IsDropDownOpen = False
        '                                                thisControl.AxisPropertiesControl.Focus()
        '                                            End Sub
        'cntrl.Items.Add(addLog)
        ''
        'Dim addCategory As New ComboBoxItem() With {.Content = "Add Category Axis", .FontStyle = FontStyles.Italic}
        'AddHandler addCategory.PreviewMouseLeftButtonUp, Sub()
        '                                                     thisControl.Plot.Axes.Add(New Wpf.CategoryAxis() With {.Title = "Category Axis Title", .Labels = New List(Of String)({"Label 1", "Label 2"})})
        '                                                     thisControl.AxesPropertyControlComboBox.SelectedItem = thisControl.Plot.Axes.Last
        '                                                     thisControl.AxesPropertyControlComboBox.IsDropDownOpen = False
        '                                                     thisControl.AxisPropertiesControl.Focus()
        '                                                 End Sub
        'cntrl.Items.Add(addCategory)
        ''
        'Dim addDateTime As New ComboBoxItem() With {.Content = "Add Date-Time Axis", .FontStyle = FontStyles.Italic}
        'AddHandler addDateTime.PreviewMouseLeftButtonUp, Sub()
        '                                                     thisControl.Plot.Axes.Add(New Wpf.DateTimeAxis() With {.Title = "Date-Time Axis Title"})
        '                                                     thisControl.AxesPropertyControlComboBox.SelectedItem = thisControl.Plot.Axes.Last
        '                                                     thisControl.AxesPropertyControlComboBox.IsDropDownOpen = False
        '                                                     thisControl.AxisPropertiesControl.Focus()
        '                                                 End Sub
        'cntrl.Items.Add(addDateTime)
        'End If
        '
        If newPlot.Axes.Count > 0 Then thisControl.AxesPropertyControlComboBox.SelectedIndex = 0
    End Sub

    Private Sub AxesPropertyControlComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        'Early exit if nothing is selected.
        If IsNothing(AxesPropertyControlComboBox.SelectedItem) Then Exit Sub

        Dim axisToSelect As Wpf.Axis = TryCast(AxesPropertyControlComboBox.SelectedItem, Wpf.Axis)
        If IsNothing(axisToSelect) Then Exit Sub
        AxisPropertiesControl.Axis = axisToSelect
    End Sub

    Private Sub DeleteAxisButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(AxesPropertyControlComboBox.SelectedItem) Then Exit Sub
        If IsNothing(Plot) Then Exit Sub
        If IsNothing(sender) Then Exit Sub
        If sender.GetType <> GetType(Button) Then Exit Sub
        Dim btn As Button = DirectCast(sender, Button)
        If IsNothing(btn.DataContext) Then Exit Sub
        '
        Dim axisToDelete As Wpf.Axis = TryCast(btn.DataContext, Wpf.Axis)
        If IsNothing(axisToDelete) Then Exit Sub
        '
        Dim index As Int32 = Plot.Axes.IndexOf(axisToDelete)
        If index = AxesPropertyControlComboBox.SelectedIndex Then
            If index > 0 Then index -= 1
            If Plot.Axes.Count = 1 Then index = -1
        End If
        Plot.Axes.Remove(axisToDelete)
        AxesPropertyControlComboBox.SelectedIndex = index

        AxisPropertiesControl.CloseExpanders()
    End Sub

    Public Shared Function AxesPropertiesToXElement(plot As Wpf.Plot) As XElement
        Dim axesProperties As New XElement(AxesPropertiesTag)
        For Each axis In plot.Axes
            axesProperties.Add(AxisControl.AxisPropertiesToXElement(axis))
        Next
        '
        Return axesProperties
    End Function
    '
    ''' <summary>
    ''' Load general plot property settings from XElement.
    ''' </summary>
    ''' <param name="element">XElement that contains the settings.</param>
    Public Shared Sub XElementToAxesProperties(plot As Wpf.Plot, element As XElement)
        'Early Exit
        If element.Name <> AxesPropertiesTag Then Exit Sub
        'Set up the axes
        plot.Axes.Clear()
        Dim tempAxis As Wpf.Axis
        'Dim axisName As String = ""
        For Each el In element.Elements(AxisControl.AxisPropertiesTag)
            tempAxis = AxisControl.XElementToAxisProperties(el)
            If IsNothing(tempAxis) Then Continue For
            plot.Axes.Add(tempAxis)
        Next
    End Sub

    Private Sub AxisPropertiesControl_AxisTypeChanged(oldAxis As Wpf.Axis, newAxis As Wpf.Axis)
        AxesPropertyControlComboBox.ItemsSource = Plot.Axes
        SelectedAxis = newAxis
        AxesPropertyControlComboBox.SelectedItem = newAxis
    End Sub

End Class
'Public Class ComboBoxItemTemplateSelector
'    Inherits DataTemplateSelector

'    Public Property DropDownTemplate As DataTemplate
'    Public Property SelectedTemplate As DataTemplate

'    Public Overrides Function SelectTemplate(ByVal item As Object, ByVal container As DependencyObject) As DataTemplate
'        Dim comboBoxItem As ComboBoxItem = GetVisualParent(Of ComboBoxItem)(container)

'        If comboBoxItem IsNot Nothing Then
'            Return DropDownTemplate
'        End If

'        Return SelectedTemplate
'    End Function

'    Public Shared Function GetVisualParent(Of T As Visual)(ByVal childObject As Object) As T
'        Dim child As DependencyObject = TryCast(childObject, DependencyObject)

'        While (child IsNot Nothing) AndAlso Not (TypeOf child Is T)
'            child = VisualTreeHelper.GetParent(child)
'        End While

'        Return TryCast(child, T)
'    End Function
'End Class

