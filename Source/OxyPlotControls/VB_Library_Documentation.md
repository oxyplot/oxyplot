# OxyPlotControls VB Library Documentation

## Overview

The **OxyPlotControls** library is a Visual Basic (VB.NET) WPF control library that provides a comprehensive set of UI controls for interacting with OxyPlot charts. It extends the OxyPlot.Wpf library with advanced features for plot manipulation, annotation editing, property management, and data export capabilities.

## Architecture

### Core Components

The VB library is organized around these primary components:

1. **OxyplotToolbar** - Main interactive toolbar control
2. **OxyplotPropertiesControl** - Master property editor panel
3. **Settings Serializer** - XML serialization for plot settings
4. **Property Controls** - Individual controls for specific plot elements

---

## OxyplotToolbar

### Purpose
The `OxyplotToolbar` is the main interactive control that connects to an OxyPlot `Plot` control and provides user interaction capabilities.

### Key Features

#### 1. Plot Connection
```vb
' Dependency property for Plot binding
Public Property Plot As Wpf.Plot
```

When a Plot is connected, the toolbar:
- Sets up mouse event handlers (MouseDown, MouseMove, MouseUp)
- Configures custom cursors for different modes
- Initializes controller bindings for pan/zoom
- Sets up annotation collection change handlers

#### 2. Tool Modes

| Mode | Button | Description |
|------|--------|-------------|
| **Pointer** | PointerButton | Default mode with SnapTrack for data point tracking |
| **Pan** | PanButton | Click-and-drag panning with left mouse button |
| **Zoom** | ZoomButton | Rectangle zoom with left mouse button |
| **Zoom All** | ZoomAllButton | Reset to full extents |

#### 3. Annotation Drawing Tools

The toolbar supports creating these annotation types:

| Annotation Type | AddToolMode Enum | Creation Method |
|-----------------|------------------|-----------------|
| Arrow | AddArrowAnnotation | Click start, drag to end |
| Text | AddTextAnnotation | Single click to place |
| Vertical Line | AddVerticalLineAnnotation | Click to place X position |
| Horizontal Line | AddHorizontalLineAnnotation | Click to place Y position |
| Rectangle | AddRectangleAnnotation | Click and drag to size |
| Ellipse | AddEllipseAnnotation | Click and drag to size |
| Point | AddPointAnnotation | Single click to place |
| Polygon | AddPolygonAnnotation | Multi-click, double-click to finish |
| Polyline | AddPolylineAnnotation | Multi-click, double-click to finish |

#### 4. Annotation Editing

Each annotation type has mouse handlers for:
- **MouseDown**: Start editing, change color to red, track which part is selected
- **MouseMove**: Update position/size based on drag
- **MouseUp**: Restore original color, finalize edit

Special editing features:
- Arrow annotations: Move start point, end point, or entire annotation
- Rectangle/Ellipse: Resize from edges/corners or move entire shape
- Polygon/Polyline: Move individual vertices or entire shape
- Line annotations: Drag to reposition

#### 5. Context Menu

Right-click on plot provides options for:
- Edit annotation text
- Format annotation (open properties)
- Delete annotation
- Copy/Export data

#### 6. Export Capabilities

- **Save Image** - Save plot as PNG, PDF, or SVG
- **Export Data** - Export series data to CSV/Excel/SQLite

### Custom Cursors

The toolbar loads custom cursors from resources:
- `_movePointsCursor` - For moving annotation points
- `_addPointCursor` - For adding annotations
- `_panHandCursor` / `_panHandClosedCursor` - For panning
- `_zoomCursor` - For zooming

### Events

```vb
Public Event PropertiesCalled(targetPlot As Wpf.Plot, openProperties As Boolean,
                               propertyExpander As OxyplotPropertiesControl.PropertyEXP,
                               selectedObject As Object)
```

---

## OxyplotPropertiesControl

### Purpose
Master property editor that provides a unified interface to edit all plot elements.

### Sections

| Section Index | Section Name | Control | Description |
|---------------|--------------|---------|-------------|
| 0 | General | GeneralPlotControl | Title, subtitle, plot area, background |
| 1 | Legend | LegendControl | Legend visibility, position, styling |
| 2 | Axes | AxesControl | Axis properties (all axis types) |
| 3 | Series | SeriesSelectorControl | Series properties |
| 4 | Annotations | AnnotationSelectorControl | Annotation properties |

### Lazy Loading
Controls are instantiated only when their section is first selected, improving initialization performance.

### Property Expander Navigation
```vb
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
```

---

## Property Controls

### GeneralPlotControl
Edits general plot properties:
- **Plot Title**: Text, color, font, size, weight, padding
- **Plot Subtitle**: Text, color, font, size, weight
- **Chart Area**: Background brush, border brush, border thickness
- **Plot Area**: Background brush, border color, border thickness

### LegendControl
Edits legend properties:
- **Area**: Visibility, background, border, thickness, padding
- **Position**: Placement, position, orientation
- **Title**: Text, color, font, size, weight
- **Items**: Text color, symbol length/margin/placement, spacing, alignment, order

### AxesControl
Selector control for axis properties:
- Supports: LinearAxis, LogarithmicAxis, CategoryAxis, DateTimeAxis, TimeSpanAxis
- Special axes: NormalProbabilityAxis, GumbelProbabilityAxis
- Tabs: General, Labels, GridLines

### SeriesSelectorControl
Selector control for series properties:
- Supports: LineSeries, ScatterSeries, BarSeries, BoxPlotSeries
- Move series up/down in z-order
- Series-specific property editors

### AnnotationSelectorControl
Selector control for annotation properties:
- Dynamic annotation-specific controls
- Add new annotations from dropdown
- Delete annotation button

---

## OxyplotSettingsSerializer

### Purpose
Provides XML serialization/deserialization for plot settings.

### Main Functions
```vb
Public Function ToXelement(plot As Plot) As XElement
Public Sub FromXelement(plot As Plot, element As XElement)
```

### Serialized Elements
- General settings (title, subtitle, plot area, chart area)
- Legend settings
- Axes settings (per-axis)
- Annotations settings (per-annotation with points data)
- Series settings (per-series)

### Helper Functions
Type-safe attribute parsing:
- `GetColorAttribute`
- `GetBrushAttribute`
- `GetStringAttribute`
- `GetDoubleAttribute`
- `GetIntegerAttribute`
- `GetBooleanAttribute`
- `GetFontFamilyAttribute`
- `GetFontWeightAttribute`
- `GetThicknessAttribute`
- `GetEnumAttribute`
- `GetDataPointAttribute`
- `GetScreenVectorAttribute`

---

## ExtensionsModule

### Extension Methods
Provides VB extension methods for OxyPlot types:

```vb
' Data point serialization
Extension Function ToPrettyText(dp As DataPoint) As String
Extension Function FromPrettyDataText(dpString As String) As DataPoint
Extension Function ToXElement(dp As DataPoint) As XElement
Extension Function PointFromXElement(dpElement As XElement) As DataPoint

' Screen point/vector serialization
Extension Function ToPrettyText(sv As ScreenVector) As String
Extension Function FromPrettyVectorText(svString As String) As ScreenVector
Extension Function ToPrettyText(sp As ScreenPoint) As String
Extension Function FromPrettyScreenText(spString As String) As ScreenPoint

' Axis property copying
Extension Sub FromAxisProperties(toAxis As Wpf.Axis, fromAxis As Wpf.Axis)

' Binding utilities
Extension Function CopyBinding(fromTarget, toTarget, dp) As Boolean
Extension Function IsBound(target, dp) As Boolean
```

---

## Workflow: Connecting and Using OxyplotToolbar

### Step 1: Add References
```xml
<!-- Required references -->
<Reference Include="OxyPlot"/>
<Reference Include="OxyPlot.Wpf"/>
<Reference Include="OxyplotControls"/>
```

### Step 2: XAML Setup
```xml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto"/>
        <ColumnDefinition Width="*"/>
        <ColumnDefinition Width="300"/>
    </Grid.ColumnDefinitions>

    <!-- Toolbar bound to Plot -->
    <local:OxyplotToolbar x:Name="PlotToolbar"
                          Grid.Column="0"
                          Plot="{Binding ElementName=MainPlot}"
                          Orientation="Vertical"
                          IconSize="24"/>

    <!-- OxyPlot Plot control -->
    <oxy:Plot x:Name="MainPlot" Grid.Column="1"/>

    <!-- Properties panel (optional) -->
    <local:OxyplotPropertiesControl x:Name="PropertiesPanel"
                                     Grid.Column="2"
                                     Plot="{Binding ElementName=MainPlot}"/>
</Grid>
```

### Step 3: Handle PropertiesCalled Event
```vb
Private Sub PlotToolbar_PropertiesCalled(targetPlot As Wpf.Plot,
                                          openProperties As Boolean,
                                          propertyExpander As PropertyEXP,
                                          selectedObject As Object)
    If openProperties Then
        PropertiesPanel.Visibility = Visibility.Visible
        PropertiesPanel.ExpandProperty(propertyExpander, selectedObject)
    End If
End Sub
```

### Step 4: Save/Load Settings
```vb
' Save plot settings
Dim settings As XElement = OxyplotSettingsSerializer.ToXelement(MainPlot)
settings.Save("PlotSettings.xml")

' Load plot settings
Dim element As XElement = XElement.Load("PlotSettings.xml")
OxyplotSettingsSerializer.FromXelement(MainPlot, element)
MainPlot.InvalidatePlot(True)
```

---

## File Structure

```
OxyPlotControls/
├── Annotations/
│   ├── AnnotationControl.xaml(.vb)
│   └── AnnotationSelectorControl.xaml(.vb)
├── Axes/
│   ├── AxesControl.xaml(.vb)
│   └── AxisControl.xaml(.vb)
├── Series/
│   ├── BarSeriesControl.xaml(.vb)
│   ├── BoxPlotSeriesControl.xaml(.vb)
│   ├── GenericSeriesControl.xaml(.vb)
│   ├── LineSeriesControl.xaml(.vb)
│   ├── ScatterSeriesControl.xaml(.vb)
│   ├── SeriesControl.xaml(.vb)
│   └── SeriesSelectorControl.xaml(.vb)
├── Expander/
│   └── ExpanderStyle.xaml
├── Resources/
│   ├── Cursors (.cur files)
│   ├── Icons (.png files)
│   └── IconResources.xaml
├── My Project/
│   └── Assembly/Resources configuration
├── ExtensionsModule.vb
├── GeneralPlotControl.xaml(.vb)
├── LegendControl.xaml(.vb)
├── OxyplotPropertiesControl.xaml(.vb)
├── OxyplotSettingsSerializer.vb
├── OxyplotToolbar.xaml(.vb)
├── SavePlotImageDialog.xaml(.vb)
└── OxyplotControls.vbproj
```

---

## Dependencies

- **OxyPlot** (core library)
- **OxyPlot.Wpf** (WPF controls)
- **GenericControls** (shared UI controls library)
- **.NET Framework 4.x** or **.NET 5+**

---

## Key Design Patterns

1. **Dependency Properties**: All bindable properties use WPF DependencyProperty pattern
2. **Lazy Loading**: Property controls instantiated on-demand
3. **Event Handlers**: Extensive use of AddHandler for dynamic event subscriptions
4. **XML Serialization**: Custom serialization for settings persistence
5. **Extension Methods**: VB Module for type extensions
6. **Factory Pattern**: Controls created based on annotation/series/axis type
