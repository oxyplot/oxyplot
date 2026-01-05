Imports OxyPlot
Imports OxyPlot.Wpf

Public Class OxyplotPropertiesControl

    Public Shared PlotProperty As DependencyProperty = DependencyProperty.Register(NameOf(Plot), GetType(Wpf.Plot), GetType(OxyplotPropertiesControl), New PropertyMetadata(Nothing, AddressOf InitializePlot))
    Private Shared Sub InitializePlot(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsNothing(d) Then Exit Sub
        If d.GetType <> GetType(OxyplotPropertiesControl) Then Exit Sub
        Dim thisControl = DirectCast(d, OxyplotPropertiesControl)
        '
        If IsNothing(e.NewValue) Then Exit Sub
        If e.NewValue.GetType <> GetType(Plot) Then Exit Sub

        'If IsNothing(thisControl.PropertyControlComboBoxStyle) Then thisControl.SetDefaultStyles()
    End Sub

    Public Property Plot As Wpf.Plot
        Get
            Return DirectCast(GetValue(PlotProperty), Wpf.Plot)
        End Get
        Set(value As Wpf.Plot)
            SetValue(PlotProperty, value)
        End Set
    End Property

    Public Shared ShowCloseButtonProperty As DependencyProperty = DependencyProperty.Register(NameOf(ShowCloseButton), GetType(Boolean), GetType(OxyplotPropertiesControl), New PropertyMetadata(True))
    Public Property ShowCloseButton As Boolean
        Get
            Return CType(GetValue(ShowCloseButtonProperty), Boolean)
        End Get
        Set(value As Boolean)
            SetValue(ShowCloseButtonProperty, value)
        End Set
    End Property

    Public Event ClosePropertiesCalled(propertiesControl As OxyplotPropertiesControl)

    Private Sub SetDefaultStyles()
        If IsNothing(BackButtonStyle) Then BackButtonStyle = CType(FindResource("CleanButtonStyle"), Style)
        If IsNothing(PropertyControlComboBoxStyle) Then PropertyControlComboBoxStyle = CType(FindResource("CleanComboBoxStyle"), Style)
        If IsNothing(ExpanderStyle) Then ExpanderStyle = CType(FindResource("ExcelExpanderStyle"), Style)
        If IsNothing(TabItemStyle) Then TabItemStyle = CType(FindResource("CustomTabItemStyle"), Style)
    End Sub



    Public Shared BackButtonStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackButtonStyle), GetType(Style), GetType(OxyplotPropertiesControl), New UIPropertyMetadata(Nothing))
    Public Property BackButtonStyle As Style
        Get
            Return DirectCast(GetValue(BackButtonStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(BackButtonStyleProperty, value)
        End Set
    End Property

    Public Shared TabItemStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(TabItemStyle), GetType(Style), GetType(OxyplotPropertiesControl), New UIPropertyMetadata(Nothing))
    Public Property TabItemStyle As Style
        Get
            Return DirectCast(GetValue(TabItemStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(TabItemStyleProperty, value)
        End Set
    End Property

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(OxyplotPropertiesControl), New UIPropertyMetadata(Nothing))
    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property


    Public Shared PropertyControlComboBoxStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(PropertyControlComboBoxStyle), GetType(Style), GetType(OxyplotPropertiesControl), New UIPropertyMetadata(Nothing))
    Public Property PropertyControlComboBoxStyle As Style
        Get
            Return DirectCast(GetValue(PropertyControlComboBoxStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(PropertyControlComboBoxStyleProperty, value)
        End Set
    End Property
    'Lazy loading to improve initialization times.
    Private _generalControls As GeneralPlotControl
    Private _legendControls As LegendControl
    Private _axesControls As AxesControl
    Private _seriesControls As SeriesSelectorControl
    Private _annotationsControls As AnnotationSelectorControl

    Private _viewPortWidthBinding As New Binding(NameOf(ScrollViewer.ViewportWidth)) With {.Source = ControlScrollViewer}
    Private _plotBinding As New Binding(NameOf(Me.Plot)) With {.Source = Me}
    Private _expanderBinding As New Binding(NameOf(Me.ExpanderStyle)) With {.Source = Me}
    Private _tabItemStyleBinding As New Binding(NameOf(Me.TabItemStyle)) With {.Source = Me}
    Private _comboboxStyleBinding As New Binding(NameOf(Me.PropertyControlComboBoxStyle)) With {.Source = Me}

    Public Sub New()
        GenericControls.PropertyDefaults.DefaultMaxPropertyWidth = 200


        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SetDefaultStyles()
    End Sub
    ''' <summary>
    ''' Select the general settings properties from the control dropdown menu.
    ''' </summary>
    Public Sub SelectGeneralSettings()
        If Not IsNothing(PropertyControlComboBox) Then PropertyControlComboBox.SelectedIndex = 0
    End Sub

    Private Function ToEnumName(Of T As Structure)(ByVal value As Integer) As String
        Return (CType(CObj(value), T)).ToString()
    End Function

    Public Sub DeleteAnnotation(anno As OxyPlot.Wpf.Annotation)
        'AnnotationSelectorControl.DeleteAnnotation(anno)
    End Sub

    Public Sub ExpandProperty(Prop As PropertyEXP, Optional SelectedObject As Object = Nothing)
        Dim str As String = ToEnumName(Of PropertyEXP)(Prop)
        Select Case True
            Case str.Contains("General_")
                PropertyControlComboBox.SelectedIndex = 0
                Select Case Prop
                    Case PropertyEXP.General_PlotTitle
                        _generalControls.PlotTitleEXP.IsExpanded = True
                    Case PropertyEXP.General_PlotSubtitle
                        _generalControls.PlotSubtitleEXP.IsExpanded = True
                    Case PropertyEXP.General_PlotArea
                        _generalControls.PlotAreaEXP.IsExpanded = True
                    Case PropertyEXP.General_PlotBackground
                        _generalControls.PlotBackgroundEXP.IsExpanded = True
                    Case Else
                End Select
            Case str.Contains("Legend_")
                PropertyControlComboBox.SelectedIndex = 1
                Select Case Prop
                    Case PropertyEXP.Legend_Title
                        _legendControls.LegendTitleEXP.IsExpanded = True
                    Case PropertyEXP.Legend_Items
                        _legendControls.LegendItemsEXP.IsExpanded = True
                    Case PropertyEXP.Legend_Area
                        _legendControls.LegendAreaEXP.IsExpanded = True
                    Case PropertyEXP.Legend_Position
                        _legendControls.LegendPositionEXP.IsExpanded = True
                    Case Else
                End Select
            Case str.Contains("Axes_")
                If SelectedObject Is Nothing Then
                    Exit Sub
                End If
                PropertyControlComboBox.SelectedIndex = 2
                _axesControls.AxesPropertyControlComboBox.SelectedItem = SelectedObject

                'For i As Integer = 0 To AxesControl.AxesPropertyControlComboBox.Items.Count - 1
                '    Dim s As String = CType(AxesControl.AxesPropertyControlComboBox.Items(i), OxyPlot.Wpf.Axis).Tag
                '    If s = CType(SelectedObject, OxyPlot.Wpf.Axis).Title Then
                '        AxesControl.AxesPropertyControlComboBox.SelectedIndex = i
                '        Exit For
                '    End If
                'Next

                With _axesControls.AxisPropertiesControl
                    Select Case Prop
                        Case PropertyEXP.Axes_Options
                            .AxisTabControl.SelectedItem = .GeneralTab
                            .GeneralEXP.IsExpanded = True
                        Case PropertyEXP.Axes_Display
                            .AxisTabControl.SelectedItem = .GeneralTab
                            .DisplayEXP.IsExpanded = True
                        Case PropertyEXP.Axes_Title
                            .AxisTabControl.SelectedItem = .LabelsTab
                            .TitleEXP.IsExpanded = True
                        Case PropertyEXP.Axes_Labels
                            .AxisTabControl.SelectedItem = .LabelsTab
                            .LabelsEXP.IsExpanded = True
                        Case PropertyEXP.Axes_MajorGridLines
                            .AxisTabControl.SelectedItem = .GridLinesTab
                            .MajorGridLinesEXP.IsExpanded = True
                        Case PropertyEXP.Axes_MinorGridLines
                            .AxisTabControl.SelectedItem = .GridLinesTab
                            .MinorGridLinesEXP.IsExpanded = True
                        Case PropertyEXP.Axes_TickOptions
                            .AxisTabControl.SelectedItem = .GridLinesTab
                            .TickOptionsEXP.IsExpanded = True
                        Case Else
                    End Select
                End With
            Case str.Contains("Series_")

                PropertyControlComboBox.SelectedIndex = 3

                _seriesControls.SeriesPropertyControlComboBox.SelectedItem = SelectedObject

                _seriesControls.SeriesPropertiesControl.Expand(Prop)

            Case str.Contains("Annotations_")

                PropertyControlComboBox.SelectedIndex = 4

                _annotationsControls.AnnotationPropertyControlComboBox.SelectedItem = SelectedObject

                With _annotationsControls.AnnotationPropertiesControl
                    Select Case Prop
                        Case PropertyEXP.Annotations_Text
                            .TextEXP.IsExpanded = True
                        Case PropertyEXP.Annotations_Display
                            .DisplayOptionsEXP.IsExpanded = True
                        Case Else
                    End Select
                End With

            Case Else


        End Select
    End Sub

    Private Sub ClosePropertiesButton_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent ClosePropertiesCalled(Me)
    End Sub

    Public Enum PropertyEXP
        General_PlotTitle
        General_PlotSubtitle
        General_PlotArea
        General_PlotBackground
        Legend_Title
        Legend_Items
        Legend_Area
        Legend_Position
        Axes_Options
        Axes_Display
        Axes_Title
        Axes_Labels
        Axes_MajorGridLines
        Axes_MinorGridLines
        Axes_TickOptions
        Series_General
        Series_Display
        Series_Markers
        Series_BoxAndWhiskers
        Series_ErrorBarSettings
        Annotations_Text
        Annotations_Display
    End Enum

    Private Sub PropertyControlComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If PropertyControlsGrid Is Nothing Then Exit Sub
        '
        If _generalControls IsNot Nothing Then _generalControls.Visibility = Visibility.Collapsed
        If _legendControls IsNot Nothing Then _legendControls.Visibility = Visibility.Collapsed
        If _axesControls IsNot Nothing Then _axesControls.Visibility = Visibility.Collapsed
        If _seriesControls IsNot Nothing Then _seriesControls.Visibility = Visibility.Collapsed
        If _annotationsControls IsNot Nothing Then _annotationsControls.Visibility = Visibility.Collapsed
        '
        Select Case PropertyControlComboBox.SelectedIndex
            Case 0 'General
                If _generalControls Is Nothing Then
                    _generalControls = New GeneralPlotControl() With {.MinWidth = 200, .MinHeight = 200}
                    BindingOperations.SetBinding(_generalControls, GeneralPlotControl.WidthProperty, _viewPortWidthBinding)
                    BindingOperations.SetBinding(_generalControls, GeneralPlotControl.PlotProperty, _plotBinding)
                    BindingOperations.SetBinding(_generalControls, GeneralPlotControl.ExpanderStyleProperty, _expanderBinding)
                    PropertyControlsGrid.Children.Add(_generalControls)
                End If
                _generalControls.Visibility = Visibility.Visible
            Case 1 'Legend
                If _legendControls Is Nothing Then
                    _legendControls = New LegendControl() With {.MinWidth = 200, .MinHeight = 200}
                    BindingOperations.SetBinding(_legendControls, LegendControl.WidthProperty, _viewPortWidthBinding)
                    BindingOperations.SetBinding(_legendControls, LegendControl.PlotProperty, _plotBinding)
                    BindingOperations.SetBinding(_legendControls, LegendControl.ExpanderStyleProperty, _expanderBinding)
                    PropertyControlsGrid.Children.Add(_legendControls)
                End If
                _legendControls.Visibility = Visibility.Visible
            Case 2 'Axes
                If _axesControls Is Nothing Then
                    _axesControls = New AxesControl() With {.MinWidth = 200, .MinHeight = 200}
                    BindingOperations.SetBinding(_axesControls, AxesControl.WidthProperty, _viewPortWidthBinding)
                    BindingOperations.SetBinding(_axesControls, AxesControl.PlotProperty, _plotBinding)
                    BindingOperations.SetBinding(_axesControls, AxesControl.ExpanderStyleProperty, _expanderBinding)
                    BindingOperations.SetBinding(_axesControls, AxesControl.TabItemStyleProperty, _tabItemStyleBinding)
                    BindingOperations.SetBinding(_axesControls, AxesControl.ComboBoxStyleProperty, _comboboxStyleBinding)
                    PropertyControlsGrid.Children.Add(_axesControls)
                End If
                _axesControls.Visibility = Visibility.Visible
            Case 3 'Series
                If _seriesControls Is Nothing Then
                    _seriesControls = New SeriesSelectorControl() With {.MinWidth = 200, .MinHeight = 200}
                    BindingOperations.SetBinding(_seriesControls, SeriesSelectorControl.WidthProperty, _viewPortWidthBinding)
                    BindingOperations.SetBinding(_seriesControls, SeriesSelectorControl.PlotProperty, _plotBinding)
                    BindingOperations.SetBinding(_seriesControls, SeriesSelectorControl.ExpanderStyleProperty, _expanderBinding)
                    BindingOperations.SetBinding(_seriesControls, SeriesSelectorControl.ComboBoxStyleProperty, _comboboxStyleBinding)
                    PropertyControlsGrid.Children.Add(_seriesControls)
                End If
                _seriesControls.Visibility = Visibility.Visible
            Case 4 'Annotations
                If _annotationsControls Is Nothing Then
                    _annotationsControls = New AnnotationSelectorControl() With {.MinWidth = 200, .MinHeight = 200}
                    BindingOperations.SetBinding(_annotationsControls, AnnotationSelectorControl.WidthProperty, _viewPortWidthBinding)
                    BindingOperations.SetBinding(_annotationsControls, AnnotationSelectorControl.PlotProperty, _plotBinding)
                    BindingOperations.SetBinding(_annotationsControls, AnnotationSelectorControl.ExpanderStyleProperty, _expanderBinding)
                    BindingOperations.SetBinding(_annotationsControls, AnnotationSelectorControl.ComboBoxStyleProperty, _comboboxStyleBinding)
                    PropertyControlsGrid.Children.Add(_annotationsControls)
                End If
                _annotationsControls.Visibility = Visibility.Visible
        End Select

    End Sub
End Class

