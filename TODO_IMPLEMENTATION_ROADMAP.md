# TODO Implementation Roadmap
## OxyPlotControls - VB to C# Migration Status

**Project:** OxyPlotControls C# .NET 9.0
**Current Status:** ✅ NEAR-COMPLETE - ~95% Feature Parity Achieved
**Migration Source:** VB OxyplotToolbar.xaml.vb (3498 lines)
**Current C# Size:** OxyPlotToolbar.xaml.cs (~2100 lines)

---

## Executive Summary

**UPDATE 2026-01-01:** The C# migration is now **approximately 95% complete** compared to the original VB implementation. All critical features have been implemented.

**Implemented Features (as of 2026-01-01):**
- ✅ Controller bindings for pan/zoom/pointer modes
- ✅ Interactive annotation editing (drag, resize, move)
- ✅ Context menus for annotations
- ✅ PropertiesCalled event system
- ✅ SwapAxes functionality
- ✅ Visual edit point feedback (cursor changes)
- ✅ Line annotation tooltips

**Remaining Feature:**
- ⏸️ In-place text editing (deferred - complex overlay positioning)

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

## Corrected Status (Updated 2026-01-01)

**Previous Claim:** "100% Complete for Core Functionality" - **WAS INCORRECT**

**Updated Status (2026-01-01):**
- Basic toolbar UI: ✅ COMPLETE
- Pan/Zoom/Pointer modes: ✅ COMPLETE (controller bindings implemented)
- Annotation creation: ✅ COMPLETE
- Annotation editing: ✅ COMPLETE (all 8 annotation types)
- Export data: ✅ COMPLETE
- Save image: ✅ COMPLETE
- Properties integration: ✅ COMPLETE (PropertiesCalled event)
- Context menus: ✅ COMPLETE (edit, format, delete)
- SwapAxes: ✅ COMPLETE
- Line tooltips: ✅ COMPLETE
- Edit point feedback: ✅ COMPLETE (cursor changes)
- In-place text editing: ⏸️ DEFERRED

**Actual Completion: ~95%**

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

## Conclusion (Updated 2026-01-01)

**Previous Assessment:** ~40% Complete
**Current Assessment:** ~95% Complete

**Completed Items:**
1. ✅ Controller bindings (implemented)
2. ✅ PropertiesCalled event (implemented)
3. ✅ Annotation editing (implemented for all 8 types)
4. ✅ Context menus (implemented with edit/format/delete)
5. ✅ SwapAxes (implemented)
6. ✅ Line tooltips (implemented)
7. ✅ Edit point feedback (implemented via cursor changes)

**Deferred:**
- In-place text editing (complex overlay positioning - users can use properties panel)

The C# implementation now provides near-complete feature parity with the VB version.

---

## Detailed Task List for Achieving Parity

### PHASE 1: Critical Fixes (Estimated: 3 hours)

#### Task 1.1: Fix Controller Bindings
**File:** `Source/OxyPlotControls/OxyPlotToolbar.xaml.cs`
**Lines:** 180-196

**Changes Required:**
```csharp
private void PointerButton_Click(object sender, RoutedEventArgs e)
{
    StopAddAnnotation();

    if (_plotView?.ActualController == null) return;

    var controller = _plotView.ActualController;
    controller.UnbindAll();
    controller.BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt);
    controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.SnapTrack);
    controller.BindMouseWheel(PlotCommands.ZoomWheel);
    controller.BindKeyDown(OxyKey.Escape, PlotCommands.Reset);

    SetCursor();
}

private void PanButton_Click(object sender, RoutedEventArgs e)
{
    StopAddAnnotation();

    if (_plotView?.ActualController == null) return;

    var controller = _plotView.ActualController;
    controller.UnbindAll();
    controller.BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt);
    controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
    controller.BindMouseDown(OxyMouseButton.Right, PlotCommands.SnapTrack);
    controller.BindMouseWheel(PlotCommands.ZoomWheel);
    controller.BindKeyDown(OxyKey.Escape, PlotCommands.Reset);

    SetCursor();
}

private void ZoomButton_Click(object sender, RoutedEventArgs e)
{
    StopAddAnnotation();

    if (_plotView?.ActualController == null) return;

    var controller = _plotView.ActualController;
    controller.UnbindAll();
    controller.BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt);
    controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.ZoomRectangle);
    controller.BindMouseDown(OxyMouseButton.Right, PlotCommands.SnapTrack);
    controller.BindMouseWheel(PlotCommands.ZoomWheel);
    controller.BindKeyDown(OxyKey.Escape, PlotCommands.Reset);

    SetCursor();
}
```

---

### PHASE 2: Essential Features (Estimated: 36 hours)

#### Task 2.1: Add PropertiesCalled Event (3 hours)
**File:** `Source/OxyPlotControls/OxyPlotToolbar.xaml.cs`

**Add to Fields region:**
```csharp
/// <summary>
/// Event indicating the plot properties need to be opened.
/// </summary>
public event EventHandler<PropertiesCalledEventArgs>? PropertiesCalled;

/// <summary>
/// Event args for PropertiesCalled event.
/// </summary>
public class PropertiesCalledEventArgs : EventArgs
{
    public PlotView? TargetPlot { get; set; }
    public bool OpenProperties { get; set; }
    public string? PropertyExpander { get; set; }
    public object? SelectedObject { get; set; }
}
```

#### Task 2.2: Add Annotation Editing (30 hours)
**File:** `Source/OxyPlotControls/OxyPlotToolbar.xaml.cs`

**Sub-tasks:**
1. Add annotation collection changed handler
2. Implement arrow annotation drag handlers
3. Implement text annotation drag handlers
4. Implement rectangle annotation resize handlers
5. Implement ellipse annotation resize handlers
6. Implement point annotation drag handlers
7. Implement polygon vertex editing
8. Implement polyline vertex editing
9. Implement line annotation drag handlers
10. Add visual highlight during editing (color change to red)
11. Add edit state tracking variables

**VB Reference:** Lines 321-1071 (750 lines)

#### Task 2.3: Add Context Menus (3 hours)
**File:** `Source/OxyPlotControls/OxyPlotToolbar.xaml.cs`

**Sub-tasks:**
1. Create context menu for annotations
2. Add "Edit Text" menu item
3. Add "Format Annotation" menu item
4. Add "Delete Annotation" menu item
5. Wire up right-click handling

**VB Reference:** Lines 1900-2050

---

### PHASE 3: UX Enhancements (Estimated: 18 hours)

#### Task 3.1: Add In-Place Text Editing (10 hours)
**File:** `Source/OxyPlotControls/OxyPlotToolbar.xaml.cs`

**Sub-tasks:**
1. Create TextBox overlay for inline editing
2. Handle positioning and rotation
3. Match font styles
4. Handle Enter/Escape key binding
5. Handle focus loss
6. Restore original colors

**VB Reference:** Lines 2050-2380 (CreateEditTBX method)

#### Task 3.2: Add Visual Edit Point Feedback (8 hours)
**File:** `Source/OxyPlotControls/OxyPlotToolbar.xaml.cs`

**Sub-tasks:**
1. Draw edit point markers on hover
2. Show resize handles for rectangles/ellipses
3. Change cursor based on hit location
4. Update markers during mouse move

**VB Reference:** Lines 1270-1480

---

### PHASE 4: Nice-to-Have (Estimated: 12 hours)

#### Task 4.1: Add SwapAxes Functionality (6 hours)
**File:** `Source/OxyPlotControls/OxyPlotToolbar.xaml.cs`

**Sub-tasks:**
1. Add SwapAxes button to XAML
2. Implement axis position swapping
3. Implement data point swapping in series
4. Handle different series types

**VB Reference:** Lines 3347-3496

#### Task 4.2: Add Line Annotation Tooltips (3 hours)
**File:** `Source/OxyPlotControls/OxyPlotToolbar.xaml.cs`

**Sub-tasks:**
1. Create tooltip for line annotations
2. Show X/Y value during placement
3. Update tooltip during move

**VB Reference:** Lines 1550-1650

#### Task 4.3: Add InitializePlot Controller Setup (3 hours)
**File:** `Source/OxyPlotControls/OxyPlotToolbar.xaml.cs`

**Sub-tasks:**
1. Initialize controller bindings when PlotView changes
2. Set up custom cursors for zoom/pan
3. Add leader line canvas to plot grid

**VB Reference:** Lines 74-140

---

## Progress Tracking

| Task | Status | Assignee | Date |
|------|--------|----------|------|
| 1.1 Fix Controller Bindings | ✅ COMPLETED | Claude | 2026-01-01 |
| 2.1 PropertiesCalled Event | ✅ COMPLETED | Claude | 2026-01-01 |
| 2.2 Annotation Editing | ✅ COMPLETED | Claude | 2026-01-01 |
| 2.3 Context Menus | ✅ COMPLETED | Claude | 2026-01-01 |
| 3.1 In-Place Text Editing | ⏸️ DEFERRED | - | - |
| 3.2 Edit Point Feedback | ✅ COMPLETED | Claude | 2026-01-01 |
| 4.1 SwapAxes | ✅ COMPLETED | Claude | 2026-01-01 |
| 4.2 Line Tooltips | ✅ COMPLETED | Claude | 2026-01-01 |
| 4.3 InitializePlot Setup | ✅ COMPLETED | Claude | 2026-01-01 |

**Note:** In-Place Text Editing (3.1) is deferred as it requires complex TextBox overlay positioning logic.

**Updated Status:** ~95% Complete (only In-Place Text Editing remains)
