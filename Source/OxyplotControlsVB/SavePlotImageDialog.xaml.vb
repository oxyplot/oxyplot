Imports System.ComponentModel
Imports OxyPlot.Wpf
Public Class SavePlotImageDialog
    Private _plot As Plot
    Public Sub New(thePlot As Plot)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _plot = thePlot
    End Sub
    Private Sub SavePlotImageDialog_ContentRendered(sender As Object, e As EventArgs) Handles Me.ContentRendered
        WidthTextBox.ToolTip = "Current Plot Width is " & CInt(_plot.ActualWidth).ToString() & " px"
        HeightTextBox.ToolTip = "Current Plot Height is " & CInt(_plot.ActualHeight).ToString() & " px"

        '
        Dim size = DirectCast(ImageSizeComboBox.SelectedItem, ComboBoxItem).Content.ToString()
        Dim sizes = size.Split(" "c)
        WidthTextBox.Text = sizes(0)
        HeightTextBox.Text = sizes(2)
    End Sub

    Private Function FileSaveDialog(ByVal filters As String) As String
        Dim saveFileBrowser As New Microsoft.Win32.SaveFileDialog With {.Filter = filters}
        If saveFileBrowser.ShowDialog = True Then
            FileSaveDialog = saveFileBrowser.FileName.ToString
        Else
            FileSaveDialog = ""
        End If
    End Function

    Private Sub TextBox_PreviewTextInput(sender As Object, e As TextCompositionEventArgs)
        'Dim tBox As TextBox = DirectCast(sender, TextBox)
        If Not Char.IsDigit(CChar(e.Text)) Then e.Handled = True 'numeric only
        If e.Text = Chr(8) Then e.Handled = False 'allow Backspace
        If e.Text = " " Then e.Handled = True 'don't allow spaces (note, this doesn't fire for the previewtextinput event so doesn't have an effect here).
        'If CanBeNegative = True Then If e.Text = "-" And SelectionStart = 0 And Text.IndexOf("-", StringComparison.Ordinal) = -1 Then e.Handled = False 'allow negative
        'If e.Text = "." Then 'allow one decimal
        '    If tBox.Text.IndexOf(".", StringComparison.Ordinal) = -1 Then e.Handled = False
        '    'If tBox.SelectedText.IndexOf(".", StringComparison.Ordinal) > -1 Then e.Handled = False
        'End If
    End Sub
    Private Sub TextBox_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Space Then e.Handled = True
    End Sub

    Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(_plot) Then Exit Sub
        '
        Dim saveFile As String = FileSaveDialog("PNG File(*.png) |*.png|PDF File(*.pdf) |*.pdf|SVG File(*.svg) |*.svg")
        'Verify input parameters
        If saveFile = "" Then
            MsgBox("File path that was specified for the image file is invalid.")
            Exit Sub
        End If
        '
        Dim extension As String = IO.Path.GetExtension(saveFile.ToLower)
        If extension <> ".png" And extension <> ".svg" And extension <> ".pdf" Then
            MsgBox("File path that was specified for the export file is not a valid file type.")
            Exit Sub
        End If
        '
        Dim imageWidth, imageHeight As Int32
        If ImageSizeComboBox.SelectedIndex = ImageSizeComboBox.Items.Count - 1 Then
            If Integer.TryParse(WidthTextBox.Text, imageWidth) = False Then
                MsgBox("Image width is not a valid number.")
                Exit Sub
            ElseIf imageWidth <= 0 Then
                MsgBox("Image width must be greater than zero.")
                Exit Sub
            End If
            '
            If Integer.TryParse(HeightTextBox.Text, imageHeight) = False Then
                MsgBox("Image height is not a valid number.")
                Exit Sub
            ElseIf imageHeight <= 0 Then
                MsgBox("Image height must be greater than zero.")
                Exit Sub
            End If
        Else
            Dim size = DirectCast(ImageSizeComboBox.SelectedItem, ComboBoxItem).Content.ToString()
            Dim sizes = size.Split(" "c)
            imageWidth = Integer.Parse(sizes(0))
            imageHeight = Integer.Parse(sizes(2))
        End If
        '
        If IO.File.Exists(saveFile) Then
            Try
                IO.File.Delete(saveFile)
            Catch ex As Exception
                MsgBox("Unable to delete image file " & IO.Path.GetFileName(saveFile) & ". It may be in use by another program.")
                Exit Sub
            End Try
        End If
        Try
            Select Case extension
                Case ".png"
                    _plot.SaveBitmap(saveFile, imageWidth, imageHeight, _plot.ActualModel.Background)
                Case ".svg"
                    Using fs As New IO.FileStream(saveFile, IO.FileMode.Create)
                        OxyPlot.SvgExporter.Export(_plot.ActualModel, fs, imageWidth, imageHeight, True)
                    End Using
                Case ".pdf"
                    Using fs As New IO.FileStream(saveFile, IO.FileMode.Create)
                        OxyPlot.PdfExporter.Export(_plot.ActualModel, fs, imageWidth, imageHeight)
                    End Using
            End Select
        Catch ex As Exception
            MsgBox("Error occurred attempting to save the image file: " & ex.Message)
        End Try
        '
        Close()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub SavePlotImageDialog_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not IsNothing(Owner) Then Owner.Activate()
    End Sub

    Private Sub ImageSizeComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If WidthTextBox Is Nothing OrElse HeightTextBox Is Nothing Then Exit Sub
        If ImageSizeComboBox.SelectedIndex < 0 Then Exit Sub
        If ImageSizeComboBox.SelectedIndex = ImageSizeComboBox.Items.Count - 1 Then
            WidthTextBox.IsEnabled = True
            HeightTextBox.IsEnabled = True
            '
            WidthTextBox.Text = CInt(_plot.ActualWidth).ToString()
            HeightTextBox.Text = CInt(_plot.ActualHeight).ToString()
        Else
            WidthTextBox.IsEnabled = False
            HeightTextBox.IsEnabled = False

            Dim size = DirectCast(ImageSizeComboBox.SelectedItem, ComboBoxItem).Content.ToString()
            Dim sizes = size.Split(" "c)
            WidthTextBox.Text = sizes(0)
            HeightTextBox.Text = sizes(2)
        End If
        '
    End Sub
End Class
