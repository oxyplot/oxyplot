# TODO Implementation Roadmap
## OxyPlotControls - VB to C# Migration Status

**Project:** OxyPlotControls C# .NET 9.0
**Current Status:** PARTIAL - Core Features Need Completion
**Migration Source:** VB OxyplotToolbar.xaml.vb (3498 lines)
**Current C# Size:** OxyPlotToolbar.xaml.cs (1466 lines)

---

## Executive Summary

The C# migration is **approximately 40% complete** compared to the original VB implementation. While basic functionality exists (pan, zoom, annotation creation, export), significant interactive editing features from the VB version have not been ported.

**Critical Missing Features:**
- Interactive annotation editing (drag, resize, move)
- Controller bindings for pan/zoom/pointer modes
- Context menus for annotations
- In-place text editing
- PropertiesCalled event system
- SwapAxes functionality
- Visual edit point feedback

---

## Category 1: CRITICAL MISSING FEATURES (From VB)

### Priority: **HIGH** - These existed in VB and should be ported

---

### MISSING #1: Plot Controller Bindings

**VB Code Location:** Lines 230-318 (Pan & Zoom region)

**VB Behavior:**
```vb
Private Sub PointerButton_Click(sender As Object, e As RoutedEventArgs)
    If IsNothing(Plot) Then Exit Sub
    With Plot.ActualController
        .UnbindAll()
        .BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt)
        .BindMouseDown(OxyMouseButton.Left, PlotCommands.SnapTrack)
        .BindMouseWheel(PlotCommands.ZoomWheel)
        .BindKeyDown(OxyKey.Escape, PlotCommands.Reset)
    End With
    SetCursor()
End Sub

Private Sub PanButton_Click(sender As Object, e As RoutedEventArgs)
    ' Similar bindings for pan mode...
End Sub

Private Sub ZoomButton_Click(sender As Object, e As RoutedEventArgs)
    ' Similar bindings for zoom mode...
End Sub
```

**Current C# Behavior:**
- Only sets cursor, does NOT configure controller bindings
- Pan/Zoom modes don't actually work differently

**Impact:** HIGH - Core toolbar functionality broken

**Implementation Required:**
```csharp
private void PointerButton_Click(object sender, RoutedEventArgs e)
{
    if (_plotView?.ActualController == null) return;

    var controller = _plotView.ActualController;
    controller.UnbindAll();
    controller.BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt);
    controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.SnapTrack);
    controller.BindMouseWheel(PlotCommands.ZoomWheel);
    controller.BindKeyDown(OxyKey.Escape, PlotCommands.Reset);

    SetCursor();
}
```

**Estimated Effort:** 2-3 hours

---

### MISSING #2: Interactive Annotation Editing

**VB Code Location:** Lines 321-1071 (Annotations region, 750 lines!)

**VB Behavior:**
When annotations are added to the plot, VB sets up mouse handlers:
```vb
Private Sub PlotModelAnnotationCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
    If Not IsNothing(e.NewItems) Then
        For Each item In e.NewItems
            Select Case item.GetType
                Case GetType(Wpf.ArrowAnnotation)
                    Dim newArrow = DirectCast(item, Wpf.ArrowAnnotation)

                    AddHandler newArrow.InternalAnnotation.MouseDown, Sub(s, ae)
                        ' Start drag operation
                        _lastScreenPoint = New ScreenPoint(ae.Position.X, ae.Position.Y)
                        _moveStartPoint = ae.HitTestResult.Index <> 2
                        _moveEndPoint = ae.HitTestResult.Index <> 1
                        ' Highlight arrow
                        _originalColor = newArrow.Color
                        newArrow.Color = Colors.Red
                        ae.Handled = True
                    End Sub

                    AddHandler newArrow.InternalAnnotation.MouseMove, Sub(s, ae)
                        ' Move start/end points based on drag
                        Dim dx = ae.Position.X - _lastScreenPoint.X
                        Dim dy = ae.Position.Y - _lastScreenPoint.Y
                        ' Update arrow position...
                    End Sub

                    AddHandler newArrow.InternalAnnotation.MouseUp, Sub(s, ae)
                        ' End drag, restore color
                        newArrow.Color = _originalColor
                    End Sub

                ' Similar handlers for Rectangle, Ellipse, Point, Text, etc.
            End Select
        Next
    End If
End Sub
```

**Current C# Behavior:**
- Annotations can only be created, NOT moved or edited after creation
- No drag/resize support for any annotation type

**Features Missing:**
1. Arrow annotation: drag start/end points
2. Rectangle/Ellipse annotation: resize by corners and edges
3. Point annotation: drag to new position
4. Text annotation: drag to reposition
5. Polygon/Polyline annotation: edit individual vertices
6. Line annotation: move intercept
7. Visual highlight when editing (color change to red)
8. Hit test index handling for different edit modes

**Impact:** CRITICAL - Major interactive feature completely missing

**Estimated Effort:** 20-30 hours (complex event handling)

---

### MISSING #3: Context Menus for Annotations

**VB Code Location:** Lines 1900-2050 (in Mouse Events region)

**VB Behavior:**
Right-clicking on an annotation shows context menu with:
- "Edit Text: [annotation text]" - opens inline text editing
- "Format Annotation: [name]" - opens properties dialog
- "Delete Annotation: [name]" - removes annotation

```vb
' Create context menu items for annotation
Dim item1 As New MenuItem()
item1.Header = "Edit Text: " & annoText
AddHandler item1.Click, Sub()
    ' Opens inline text editing
    CreateEditTBX(...)
End Sub

Dim item2 As New MenuItem()
item2.Header = "Format Annotation: " & annoText
AddHandler item2.Click, Sub()
    RaiseEvent PropertiesCalled(Plot, True, PropertyEXP.Annotations_Text, wpfAnno)
End Sub

Dim item4 As New MenuItem()
item4.Header = "Delete Annotation: " & annoText
AddHandler item4.Click, Sub()
    Plot.Annotations.Remove(wpfAnno)
End Sub
```

**Current C# Behavior:**
- No context menu support at all
- Annotations cannot be deleted or edited via right-click

**Impact:** HIGH - Expected UX feature missing

**Estimated Effort:** 4-6 hours

---

### MISSING #4: In-Place Text Editing

**VB Code Location:** Lines 2050-2380 (CreateEditTBX method, 330 lines)

**VB Behavior:**
Double-clicking on text (plot title, axis title, annotation text) opens an inline TextBox for editing:
```vb
Private Sub CreateEditTBX(ExistingTextblock As TextBlock, dependencyObj As DependencyObject, ...)
    ' Creates overlay TextBox at exact position of text
    ' Handles rotation, positioning, font matching
    ' Binds TextBox.Text to property
    ' Removes TextBox on Enter or LostFocus
    ' Restores original color
End Sub
```

**Current C# Behavior:**
- No in-place text editing
- Users must use properties panel for all text changes

**Impact:** MEDIUM - Useful editing convenience missing

**Estimated Effort:** 8-10 hours (complex positioning/binding logic)

---

### MISSING #5: PropertiesCalled Event

**VB Code Location:** Lines 194-209 (Members region)

**VB Declaration:**
```vb
Public Event PropertiesCalled(
    targetPlot As Wpf.Plot,
    openProperties As Boolean,
    propertyExpander As OxyplotPropertiesControl.PropertyEXP,
    selectedObject As Object)
```

**VB Usage:**
Raised throughout code to open properties panel:
```vb
' When annotation is created
RaiseEvent PropertiesCalled(Plot, True, PropertyEXP.Annotations_Text, newArrow)

' When properties button clicked
RaiseEvent PropertiesCalled(Plot, True, Nothing, Nothing)

' When right-clicking to format
RaiseEvent PropertiesCalled(Plot, True, PropertyEXP.Annotations_Text, wpfAnno)
```

**Current C# Behavior:**
- No event for opening properties
- Properties panel integration not connected

**Impact:** HIGH - Required for property editing workflow

**Estimated Effort:** 2-3 hours

---

### MISSING #6: SwapAxes Functionality

**VB Code Location:** Lines 3347-3496 (Open Plot Properties region)

**VB Behavior:**
Swaps X and Y axes, including data points:
```vb
Private Sub SwapAxesButton_Click(sender As Object, e As RoutedEventArgs)
    ' Swap axis positions
    For Each axis In Plot.Axes
        If axis.Position = AxisPosition.Bottom Then
            axis.Position = AxisPosition.Left
        ElseIf axis.Position = AxisPosition.Left Then
            axis.Position = AxisPosition.Bottom
        End If
    Next

    ' Swap data points in series
    For Each s In Plot.Series
        If GetType(DataPointSeries).IsAssignableFrom(s.GetType()) Then
            Swap(DirectCast(s, DataPointSeries))
        End If
    Next
End Sub
```

**Current C# Behavior:**
- No swap axes button/functionality

**Impact:** LOW - Nice-to-have feature

**Estimated Effort:** 4-6 hours

---

### MISSING #7: Visual Edit Point Feedback

**VB Code Location:** Lines 1270-1480 (in PlotModelMouseMove)

**VB Behavior:**
When hovering over annotations, shows edit point markers:
```vb
' For rectangle annotation
markerPoints.Add(ur)  ' Upper right corner
markerPoints.Add(ll)  ' Lower left corner
markerPoints.Add(New ScreenPoint(ll.X, ur.Y))  ' Upper left
markerPoints.Add(New ScreenPoint(ur.X, ll.Y))  ' Lower right
' Plus midpoints for edge resize
markerPoints.Add(New ScreenPoint(ll.X, ll.Y + (ur.Y - ll.Y) / 2))  ' Left edge
' etc.
markerSizes.AddRange({2, 2, 2, 2, 2, 2, 2, 2})

' Change cursor based on hit location
If topRight.X < 10 AndAlso topRight.Y < 10 Then
    updatedCursor = Cursors.SizeNESW  ' Resize cursor
End If
```

**Current C# Behavior:**
- No visual feedback when hovering over annotations
- Cursor doesn't change to indicate edit mode

**Impact:** MEDIUM - Important UX feedback missing

**Estimated Effort:** 6-8 hours

---

### MISSING #8: Line Annotation Tooltips

**VB Code Location:** Lines 1550-1650

**VB Behavior:**
When adding/moving line annotations, shows tooltip with coordinates:
```vb
Private Sub OpenLineAnnotationTooltip(lineAnno As Wpf.LineAnnotation)
    ' Create tooltip showing X/Y value
End Sub

Private Sub UpdateLineAnnotationTooltip(lineAnno As Wpf.LineAnnotation)
    ' Update tooltip as line moves
End Sub
```

**Current C# Behavior:**
- No coordinate feedback during line annotation placement

**Impact:** LOW - Nice-to-have feature

**Estimated Effort:** 2-3 hours

---

## Category 2: Originally Listed TODOs (Still Valid)

These items from the original roadmap are still valid:

| # | TODO | Location | Priority | Effort |
|---|------|----------|----------|--------|
| 1 | GenericAxisControl | AxisControlFactory.cs | LOW | 4h |
| 2 | GenericSeriesControl | SeriesControlFactory.cs | LOW | 4h |
| 3 | GenericAnnotationControl | AnnotationControlFactory.cs | LOW | 3h |
| 4 | Serialize Axes | PlotModelSerializer.cs | MEDIUM | 6h |
| 5 | Serialize Series | PlotModelSerializer.cs | MEDIUM | 8h |
| 6 | Serialize Annotations | PlotModelSerializer.cs | MEDIUM | 4h |

---

## Category 3: Completed Features

The following features HAVE been ported correctly:

| Feature | VB Location | C# Status |
|---------|-------------|-----------|
| Export Series Data (CSV) | Lines 2445-3295 | COMPLETE |
| Save Plot Image (PNG/PDF/SVG) | Lines 3297-3308 | COMPLETE |
| Basic Pan/Zoom buttons | Lines 230-319 | PARTIAL (cursor only) |
| Add Annotation buttons | Lines 321-500 | COMPLETE |
| Basic mouse event handlers | Lines 1073-1200 | PARTIAL |
| Annotation creation flow | Lines 1080-1270 | COMPLETE |

---

## Summary Table - All Missing Features

| # | Feature | VB Lines | Priority | Effort | Impact |
|---|---------|----------|----------|--------|--------|
| 1 | Controller Bindings | 90 | **CRITICAL** | 3h | Core broken |
| 2 | Annotation Editing | 750 | **CRITICAL** | 30h | Major feature |
| 3 | Context Menus | 150 | **HIGH** | 6h | Expected UX |
| 4 | In-Place Text Edit | 330 | **MEDIUM** | 10h | Convenience |
| 5 | PropertiesCalled Event | 20 | **HIGH** | 3h | Integration |
| 6 | SwapAxes | 150 | **LOW** | 6h | Nice-to-have |
| 7 | Edit Point Feedback | 200 | **MEDIUM** | 8h | UX feedback |
| 8 | Line Tooltips | 100 | **LOW** | 3h | Nice-to-have |

**Total Missing VB Code:** ~1800 lines
**Total Estimated Effort:** ~70 hours

---

## Corrected Status

**Previous Claim:** "100% Complete for Core Functionality" - **INCORRECT**

**Actual Status:**
- Basic toolbar UI: COMPLETE
- Pan/Zoom/Pointer modes: BROKEN (no controller bindings)
- Annotation creation: COMPLETE
- Annotation editing: NOT IMPLEMENTED
- Export data: COMPLETE
- Save image: COMPLETE
- Properties integration: NOT IMPLEMENTED
- Context menus: NOT IMPLEMENTED

**Recommendation:**
1. **IMMEDIATE:** Fix controller bindings (3h) - core functionality broken
2. **HIGH:** Add PropertiesCalled event (3h) - required for property panel integration
3. **HIGH:** Add context menus (6h) - expected user experience
4. **MEDIUM:** Add annotation editing (30h) - major VB feature
5. **FUTURE:** Other enhancements based on user feedback

---

## VB File Structure Reference

For implementers, here's the VB file structure:

```
OxyplotToolbar.xaml.vb (3498 lines)
├── #Region "Construction" (Lines 7-44) - 37 lines
│   └── Custom cursor loading
├── #Region "Members" (Lines 46-228) - 182 lines
│   ├── Dependency properties (Plot, IconSize, ToolBarOrientation)
│   ├── InitializePlot callback
│   ├── Custom cursors
│   ├── Edit state variables
│   └── PropertiesCalled event declaration
├── #Region "Pan & Zoom" (Lines 230-319) - 89 lines
│   ├── PointerButton_Click (controller bindings)
│   ├── PanButton_Click (controller bindings)
│   ├── ZoomButton_Click (controller bindings)
│   ├── ZoomAllButton_Click
│   └── SetCursor
├── #Region "Annotations" (Lines 321-1071) - 750 lines
│   ├── Add button click handlers (10 types)
│   ├── StopAddAnnotation
│   ├── PlotModelAnnotationCollectionChanged
│   │   └── Sets up MouseDown/Move/Up handlers per annotation type
│   └── Per-annotation drag/resize logic
├── #Region "Mouse Events" (Lines 1073-2443) - 1370 lines
│   ├── PlotModelMouseDown (annotation creation)
│   ├── PlotModelMouseMove (drag feedback, hover detection)
│   ├── PlotModelMouseUp (finish operations)
│   ├── GetSelectedObjects (context menu trigger)
│   ├── Context menu creation
│   ├── CreateEditTBX (in-place text editing)
│   └── Helper functions
├── #Region "Export Series Data" (Lines 2445-3295) - 850 lines
│   └── ExportDataButton_Click (ported to C#)
├── #Region "Save Plot" (Lines 3297-3308) - 11 lines
│   └── SavePlotButton_Click (uses dialog)
└── #Region "Open Plot Properties" (Lines 3310-3496) - 186 lines
    ├── PropertiesButton_Click
    └── SwapAxes functionality
```

---

## Conclusion

**Previous Assessment:** 100% Complete - **INCORRECT**

**Actual Assessment:** ~40% Complete

**Critical Path:**
1. Fix controller bindings (BROKEN)
2. Add PropertiesCalled event (REQUIRED)
3. Add annotation editing (MAJOR MISSING FEATURE)
4. Add context menus (EXPECTED UX)

The C# implementation provides a functional skeleton but is missing significant interactive editing capabilities that were core features of the VB version.
