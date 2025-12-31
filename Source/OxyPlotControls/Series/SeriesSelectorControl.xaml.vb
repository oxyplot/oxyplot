Imports System.Collections.Specialized
Imports OxyPlot
Imports OxyPlot.Series

Public Class SeriesSelectorControl
    Public Shared PlotProperty As DependencyProperty = DependencyProperty.Register(NameOf(Plot), GetType(Wpf.Plot), GetType(SeriesSelectorControl), New PropertyMetadata(Nothing, AddressOf InitializePlot))
    Public Property Plot As Wpf.Plot
        Get
            Return DirectCast(GetValue(PlotProperty), Wpf.Plot)
        End Get
        Set(value As Wpf.Plot)
            SetValue(PlotProperty, value)
        End Set
    End Property

    Public Shared SelectedSeriesProperty As DependencyProperty = DependencyProperty.Register(NameOf(SelectedSeries), GetType(Wpf.Series), GetType(SeriesSelectorControl), New PropertyMetadata(Nothing))
    Public Property SelectedSeries As Wpf.Series
        Get
            Return DirectCast(GetValue(SelectedSeriesProperty), Wpf.Series)
        End Get
        Set(value As Wpf.Series)
            SetValue(SelectedSeriesProperty, value)
        End Set
    End Property

    Public Shared ComboBoxStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ComboBoxStyle), GetType(Style), GetType(SeriesSelectorControl), New PropertyMetadata(Nothing))
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
        If d.GetType <> GetType(SeriesSelectorControl) Then Exit Sub
        Dim thisControl = DirectCast(d, SeriesSelectorControl)
        '
        thisControl.SeriesPropertyControlComboBox.ItemsSource = Nothing
        If IsNothing(e.NewValue) Then Exit Sub
        If e.NewValue.GetType <> GetType(Wpf.Plot) Then Exit Sub
        Dim newPlot As Wpf.Plot = DirectCast(e.NewValue, Wpf.Plot)
        '
        thisControl.AddHandlers()
        If IsNothing(thisControl.ComboBoxStyle) Then thisControl.SetDefaultComboboxStyle()
        '
        thisControl.SeriesPropertyControlComboBox.ItemsSource = newPlot.Series
        '
        'thisControl.SeriesPropertyControlComboBox.ApplyTemplate()
        'Dim t = thisControl.SeriesPropertyControlComboBox.FindResource("ComboBoxTemplate")
        'Dim v = DirectCast(t, ControlTemplate).FindName("SpecialOptions", thisControl.SeriesPropertyControlComboBox)
        'Dim cntrl = DirectCast(v, ItemsControl)
        'If Not IsNothing(cntrl) AndAlso cntrl.Items.Count = 0 Then
        '    cntrl.Items.Add(New Separator())
        '    '
        '    Dim addLinear As New ComboBoxItem() With {.Content = "Add Line Series", .FontStyle = FontStyles.Italic}
        '    AddHandler addLinear.PreviewMouseLeftButtonUp, Sub()
        '                                                       thisControl.Plot.Series.Add(New Wpf.LineSeries() With {.Title = "Line Series Title"})
        '                                                       thisControl.SeriesPropertyControlComboBox.SelectedItem = thisControl.Plot.Series.Last
        '                                                       thisControl.SeriesPropertyControlComboBox.IsDropDownOpen = False
        '                                                       thisControl.SeriesPropertiesControl.Focus()
        '                                                   End Sub
        '    cntrl.Items.Add(addLinear)
        '    '
        '    Dim addScatter As New ComboBoxItem() With {.Content = "Add Scatter Series", .FontStyle = FontStyles.Italic}
        '    AddHandler addScatter.PreviewMouseLeftButtonUp, Sub()
        '                                                        thisControl.Plot.Series.Add(New Wpf.ScatterSeries() With {.Title = "Scatter Series Title"})
        '                                                        thisControl.SeriesPropertyControlComboBox.SelectedItem = thisControl.Plot.Series.Last
        '                                                        thisControl.SeriesPropertyControlComboBox.IsDropDownOpen = False
        '                                                        thisControl.SeriesPropertiesControl.Focus()
        '                                                    End Sub
        '    cntrl.Items.Add(addScatter)
        '    ''
        '    'Dim addCategory As New ComboBoxItem() With {.Content = "Add Category Axis", .FontStyle = FontStyles.Italic}
        '    'AddHandler addCategory.PreviewMouseLeftButtonUp, Sub()
        '    '                                                     thisControl.Plot.Axes.Add(New Wpf.CategoryAxis() With {.Title = "Category Axis Title", .Labels = New List(Of String)({"Label 1", "Label 2"})})
        '    '                                                     thisControl.SeriesPropertyControlComboBox.SelectedItem = thisControl.Plot.Axes.Last
        '    '                                                     thisControl.SeriesPropertyControlComboBox.IsDropDownOpen = False
        '    '                                                     thisControl.SeriesPropertiesControl.Focus()
        '    '                                                 End Sub
        '    'cntrl.Items.Add(addCategory)
        '    ''
        '    'Dim addDateTime As New ComboBoxItem() With {.Content = "Add Date-Time Axis", .FontStyle = FontStyles.Italic}
        '    'AddHandler addDateTime.PreviewMouseLeftButtonUp, Sub()
        '    '                                                     thisControl.Plot.Axes.Add(New Wpf.DateTimeAxis() With {.Title = "Date-Time Axis Title"})
        '    '                                                     thisControl.SeriesPropertyControlComboBox.SelectedItem = thisControl.Plot.Axes.Last
        '    '                                                     thisControl.SeriesPropertyControlComboBox.IsDropDownOpen = False
        '    '                                                     thisControl.SeriesPropertiesControl.Focus()
        '    '                                                 End Sub
        '    'cntrl.Items.Add(addDateTime)
        'End If
        '
        If newPlot.Series.Count > 0 Then thisControl.SeriesPropertyControlComboBox.SelectedIndex = 0
    End Sub

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(SeriesSelectorControl))
    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property

    ''' <summary>
    ''' Add required handlers. 
    ''' </summary>
    Private Sub AddHandlers()
        AddHandler Plot.Series.CollectionChanged, AddressOf Series_CollectionChanged
    End Sub

    Private Sub Series_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        ' New item was added. 
        If Not IsNothing(e.NewItems) Then
            For Each newItem In e.NewItems
                SeriesPropertyControlComboBox.SelectedItem = newItem
                SeriesPropertiesControl.Series = TryCast(newItem, Wpf.Series)
                Exit For
            Next
        End If

        ' Item was removed. 
        If Not IsNothing(e.OldItems) Then
            If Not IsNothing(Plot) AndAlso Plot.Series.Count = 0 Then
                'SeriesPropertiesControl.HideExpanders()
            Else
                SeriesPropertyControlComboBox.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub SeriesPropertyControlComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        ' Early exit if nothing is selected.
        If IsNothing(SeriesPropertyControlComboBox.SelectedItem) Then Exit Sub
        '
        Dim seriesToSelect As Wpf.Series = TryCast(SeriesPropertyControlComboBox.SelectedItem, Wpf.Series)
        If IsNothing(seriesToSelect) Then Exit Sub
        SeriesPropertiesControl.Series = seriesToSelect
    End Sub
    Private Sub MoveSeriesUpButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(Plot) Then Exit Sub
        If IsNothing(sender) Then Exit Sub
        If sender.GetType <> GetType(Button) Then Exit Sub
        Dim btn As Button = DirectCast(sender, Button)
        If IsNothing(btn.DataContext) Then Exit Sub
        Dim seriesToMoveUp As Wpf.Series = TryCast(btn.DataContext, Wpf.Series)
        If IsNothing(seriesToMoveUp) Then Exit Sub
        '
        Dim index As Int32 = Plot.Series.IndexOf(seriesToMoveUp)
        If index = 0 Or index = -1 Then Exit Sub
        ' You can't simply swap the series, no that would be too easy. Instead you have to make a copy of the series, swap on the copy, then add each series back.
        Dim oldSeries = Plot.Series.ToList
        Plot.Series.Clear()
        oldSeries(index) = oldSeries(index - 1)
        oldSeries(index - 1) = seriesToMoveUp
        For i As Int32 = 0 To oldSeries.Count - 1
            Plot.Series.Add(oldSeries(i))
        Next

        SeriesPropertyControlComboBox.SelectedIndex = index + 1
        Plot.InvalidatePlot(True)
    End Sub
    Private Sub MoveSeriesDownButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(Plot) Then Exit Sub
        If IsNothing(sender) Then Exit Sub
        If sender.GetType <> GetType(Button) Then Exit Sub
        Dim btn As Button = DirectCast(sender, Button)
        If IsNothing(btn.DataContext) Then Exit Sub
        '
        Dim seriesToMoveDown As Wpf.Series = TryCast(btn.DataContext, Wpf.Series)
        If IsNothing(seriesToMoveDown) Then Exit Sub
        '
        Dim index As Int32 = Plot.Series.IndexOf(seriesToMoveDown)
        If index = Plot.Series.Count - 1 Or index = -1 Then Exit Sub
        '
        Dim oldSeries = Plot.Series.ToList
        Plot.Series.Clear()
        oldSeries(index) = oldSeries(index + 1)
        oldSeries(index + 1) = seriesToMoveDown
        For i As Int32 = 0 To oldSeries.Count - 1
            Plot.Series.Add(oldSeries(i))
        Next

        SeriesPropertyControlComboBox.SelectedIndex = index + 1
        Plot.InvalidatePlot(True)
    End Sub
    Private Sub DeleteSeriesButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(Plot) Then Exit Sub
        If IsNothing(sender) Then Exit Sub
        If sender.GetType <> GetType(Button) Then Exit Sub
        Dim btn As Button = DirectCast(sender, Button)
        If IsNothing(btn.DataContext) Then Exit Sub
        'If btn.DataContext.GetType.GetFirstAbstractBaseType <> GetType(Wpf.Series) Then Exit Sub
        Dim seriesToDelete As Wpf.Series = TryCast(btn.DataContext, Wpf.Series)
        If IsNothing(seriesToDelete) Then Exit Sub
        '
        Dim index As Int32 = Plot.Series.IndexOf(seriesToDelete)
        If index = SeriesPropertyControlComboBox.SelectedIndex Then
            If index > 0 Then index -= 1
            If Plot.Series.Count = 1 Then index = -1
        End If
        Plot.Series.Remove(seriesToDelete)
        SeriesPropertyControlComboBox.SelectedIndex = index
        Plot.InvalidatePlot(False)

        SeriesPropertiesControl.CloseExpanders()
    End Sub

    'Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
    '    If IsNothing(SeriesPropertyControlComboBox.SelectedItem) Then Exit Sub
    '    If IsNothing(Plot) Then Exit Sub
    '    If IsNothing(sender) Then Exit Sub
    '    If sender.GetType <> GetType(CheckBox) Then Exit Sub
    '    Dim chkBx As CheckBox = DirectCast(sender, CheckBox)
    '    If IsNothing(chkBx.DataContext) Then Exit Sub
    '    If chkBx.DataContext.GetType.GetFirstAbstractBaseType <> GetType(Wpf.Series) Then Exit Sub
    '    Dim seriesToShow As Wpf.Series = DirectCast(chkBx.DataContext, Wpf.Series)
    'End Sub

    'Private Sub CheckBox_Unchecked(sender As Object, e As RoutedEventArgs)

    'End Sub


End Class
