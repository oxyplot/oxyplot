# VB to C# OxyPlotControls Comparison Matrix

## Overview

This document provides a comprehensive comparison between the original **VB.NET OxyplotControls** library (on the `develop` branch) and the modernized **C# OxyPlotControls** library (on this feature branch).

---

## Executive Summary

| Aspect | VB Version | C# Version | Status |
|--------|-----------|------------|--------|
| **Language** | VB.NET | C# 11+ | ✅ Modernized |
| **Architecture** | Plot (WPF control) based | PlotView/PlotModel based | ✅ Improved |
| **Namespaces** | Single namespace | Organized sub-namespaces | ✅ Improved |
| **Null Safety** | No | C# nullable reference types | ✅ Improved |
| **File Structure** | Flat hierarchy | Logical folder structure | ✅ Improved |
| **Pattern Matching** | VB Select Case | C# switch expressions | ✅ Modernized |

---

## Core Components Comparison

### OxyplotToolbar

| Feature | VB Version | C# Version | Status |
|---------|-----------|------------|--------|
| **Plot Binding** | `Plot` property (Wpf.Plot) | `PlotView` property (PlotView) | ✅ Updated |
| **Icon Size** | DependencyProperty | DependencyProperty | ✅ Equivalent |
| **Orientation** | DependencyProperty | DependencyProperty | ✅ Equivalent |
| **Pointer Mode** | ✅ SnapTrack | ✅ SnapTrack | ✅ Equivalent |
| **Pan Mode** | ✅ PanAt | ✅ PanAt | ✅ Equivalent |
| **Zoom Mode** | ✅ ZoomRectangle | ✅ ZoomRectangle | ✅ Equivalent |
| **Zoom All** | ✅ ResetAllAxes | ✅ ResetAllAxes | ✅ Equivalent |
| **Custom Cursors** | From resources | ✅ From resources | ✅ Equivalent |
| **Properties Event** | ✅ PropertiesCalled | ✅ PropertiesCalled | ✅ Equivalent |

#### Annotation Creation Tools

| Annotation Type | VB Version | C# Version | Status |
|-----------------|-----------|------------|--------|
| Arrow | ✅ AddArrowAnnotation | ✅ AddArrowAnnotation | ✅ Equivalent |
| Text | ✅ AddTextAnnotation | ✅ AddTextAnnotation | ✅ Equivalent |
| Vertical Line | ✅ AddVerticalLineAnnotation | ✅ AddVerticalLineAnnotation | ✅ Equivalent |
| Horizontal Line | ✅ AddHorizontalLineAnnotation | ✅ AddHorizontalLineAnnotation | ✅ Equivalent |
| Rectangle | ✅ AddRectangleAnnotation | ✅ AddRectangleAnnotation | ✅ Equivalent |
| Ellipse | ✅ AddEllipseAnnotation | ✅ AddEllipseAnnotation | ✅ Equivalent |
| Point | ✅ AddPointAnnotation | ✅ AddPointAnnotation | ✅ Equivalent |
| Polygon | ✅ AddPolygonAnnotation | ✅ AddPolygonAnnotation | ✅ Equivalent |
| Polyline | ✅ AddPolylineAnnotation | ✅ AddPolylineAnnotation | ✅ Equivalent |

#### Annotation Editing Handlers

| Feature | VB Version | C# Version | Status |
|---------|-----------|------------|--------|
| Arrow drag/resize | ✅ Start/End point | ✅ Start/End point | ✅ Equivalent |
| Text drag | ✅ TextPosition | ✅ TextPosition | ✅ Equivalent |
| Rectangle resize | ✅ Corner/Edge | ✅ Corner/Edge | ✅ Equivalent |
| Ellipse resize | ✅ Corner/Edge | ✅ Center + Width/Height | ✅ Equivalent |
| Point drag | ✅ X/Y position | ✅ X/Y position | ✅ Equivalent |
| Polygon vertex edit | ✅ Per-vertex | ✅ Per-vertex | ✅ Equivalent |
| Polyline vertex edit | ✅ Per-vertex | ✅ Per-vertex | ✅ Equivalent |
| Line drag | ✅ X or Y | ✅ X or Y | ✅ Equivalent |
| Visual feedback | ✅ Red highlight | ✅ Red highlight | ✅ Equivalent |

#### Export Features

| Feature | VB Version | C# Version | Status |
|---------|-----------|------------|--------|
| Save Image Button | ✅ SavePlotImageDialog | ✅ SavePlotImageDialog | ✅ Equivalent |
| PNG Export | ✅ SaveBitmap | ✅ SaveBitmap | ✅ Equivalent |
| PDF Export | ✅ PdfExporter | ✅ PdfExporter | ✅ Equivalent |
| SVG Export | ✅ SvgExporter | ✅ SvgExporter | ✅ Equivalent |
| Export Data | ✅ Series to CSV/Excel/SQLite | ✅ Series to CSV/Excel/SQLite | ✅ Equivalent |
| Series data types | LineSeries, ScatterSeries, AreaSeries, etc. | LineSeries, ScatterSeries, AreaSeries, etc. | ✅ Equivalent |

---

### OxyplotPropertiesControl

| Feature | VB Version | C# Version | Status |
|---------|-----------|------------|--------|
| **Plot Binding** | `Plot` property (Wpf.Plot) | `Model` property (PlotModel) | ✅ Updated |
| **Lazy Loading** | ✅ Controls loaded on selection | ✅ Controls loaded on selection | ✅ Equivalent |
| **Section Navigation** | ✅ ComboBox with 5 sections | ✅ ComboBox with 5 sections | ✅ Equivalent |
| **PropertyEXP enum** | ✅ 22 property expanders | ✅ 22 PropertyExpander values | ✅ Equivalent |
| **Close Button** | ✅ ShowCloseButton | ✅ ShowCloseButton | ✅ Equivalent |
| **Style Properties** | ✅ BackButtonStyle, ExpanderStyle, etc. | ✅ BackButtonStyle, ExpanderStyle, TabItemStyle, ComboBoxStyle | ✅ Equivalent |

#### Sections

| Section | VB Version | C# Version | Status |
|---------|-----------|------------|--------|
| General | ✅ GeneralPlotControl | ✅ GeneralPlotControl | ✅ Equivalent |
| Legend | ✅ LegendControl | ✅ LegendControl | ✅ Equivalent |
| Axes | ✅ AxesControl | ✅ AxesControl | ✅ Equivalent |
| Series | ✅ SeriesSelectorControl | ✅ SeriesSelectorControl | ✅ Equivalent |
| Annotations | ✅ AnnotationSelectorControl | ✅ AnnotationSelectorControl | ✅ Equivalent |

---

### Property Controls

#### General Controls

| Feature | VB Version | C# Version | Status |
|---------|-----------|------------|--------|
| GeneralPlotControl | ✅ GeneralPlotControl.xaml.vb | ✅ GeneralPlotControl.xaml.cs | ✅ Equivalent |
| Title properties | ✅ Title, Color, Font, Size, Weight, Padding | ✅ Via PlotModel | ✅ Equivalent |
| Subtitle properties | ✅ Subtitle, Color, Font, Size, Weight | ✅ Via PlotModel | ✅ Equivalent |
| Chart area | ✅ Background, Border, Thickness | ✅ Via PlotModel | ✅ Equivalent |
| Plot area | ✅ Background, Border, Thickness | ✅ Via PlotModel | ✅ Equivalent |

#### Legend Controls

| Feature | VB Version | C# Version | Status |
|---------|-----------|------------|--------|
| LegendControl | ✅ LegendControl.xaml.vb | ✅ LegendControl.xaml.cs | ✅ Equivalent |
| Visibility | ✅ IsLegendVisible | ✅ Via PlotModel | ✅ Equivalent |
| Position | ✅ LegendPosition, LegendPlacement | ✅ Via PlotModel | ✅ Equivalent |
| Orientation | ✅ LegendOrientation | ✅ Via PlotModel | ✅ Equivalent |
| Styling | ✅ Background, Border, Title, Items | ✅ Via PlotModel | ✅ Equivalent |

#### Axes Controls

| Axis Type | VB Version | C# Version | Status |
|-----------|-----------|------------|--------|
| Linear | ✅ AxisControl handles | ✅ LinearAxisControl | ✅ Equivalent |
| Logarithmic | ✅ AxisControl handles | ✅ LogarithmicAxisControl | ✅ Equivalent |
| Category | ✅ AxisControl handles | ✅ CategoryAxisControl | ✅ Equivalent |
| DateTime | ✅ AxisControl handles | ✅ DateTimeAxisControl | ✅ Equivalent |
| TimeSpan | ❌ Not in VB | ✅ TimeSpanAxisControl | ✅ C# Added |
| Normal Probability | ❌ Not in VB | ✅ NormalProbabilityAxisControl | ✅ C# Added |
| Gumbel Probability | ❌ Not in VB | ✅ GumbelProbabilityAxisControl | ✅ C# Added |
| Angle | ❌ Not in VB | ✅ AngleAxisControl | ✅ C# Added |
| Magnitude | ❌ Not in VB | ✅ MagnitudeAxisControl | ✅ C# Added |
| LinearColor | ❌ Not in VB | ✅ LinearColorAxisControl | ✅ C# Added |

#### Series Controls

| Series Type | VB Version | C# Version | Status |
|-------------|-----------|------------|--------|
| Line | ✅ LineSeriesControl | ✅ LineSeriesControl | ✅ Equivalent |
| Scatter | ✅ ScatterSeriesControl | ✅ ScatterSeriesControl | ✅ Equivalent |
| Bar | ✅ BarSeriesControl | ✅ BarSeriesControl | ✅ Equivalent |
| BoxPlot | ✅ BoxPlotSeriesControl | ✅ BoxPlotSeriesControl | ✅ Equivalent |
| Area | ❌ Not in VB | ✅ AreaSeriesControl | ✅ C# Added |
| Pie | ❌ Not in VB | ✅ PieSeriesControl | ✅ C# Added |
| Generic | ✅ GenericSeriesControl | ❌ Replaced by specific | ✅ Improved |

#### Annotation Controls

| Annotation Type | VB Version | C# Version | Status |
|-----------------|-----------|------------|--------|
| Arrow | ✅ AnnotationControl handles | ✅ ArrowAnnotationControl | ✅ Improved |
| Text | ✅ AnnotationControl handles | ✅ TextAnnotationControl | ✅ Improved |
| Line | ✅ AnnotationControl handles | ✅ LineAnnotationControl | ✅ Improved |
| Rectangle | ✅ AnnotationControl handles | ✅ RectangleAnnotationControl | ✅ Improved |
| Ellipse | ✅ AnnotationControl handles | ✅ EllipseAnnotationControl | ✅ Improved |
| Point | ✅ AnnotationControl handles | ✅ PointAnnotationControl | ✅ Improved |
| Polygon | ✅ AnnotationControl handles | ✅ PolygonAnnotationControl | ✅ Improved |
| Polyline | ✅ AnnotationControl handles | ✅ PolylineAnnotationControl | ✅ Improved |

---

### Serialization

| Feature | VB Version | C# Version | Status |
|---------|-----------|------------|--------|
| **Module/Class** | OxyplotSettingsSerializer (Module) | PlotModelSerializer (Static Class) | ✅ Modernized |
| **Main Tag** | "OxyplotProperties" | "OxyPlotSettings" | ✅ Renamed |
| **Version Support** | Single version | Versioned (1.0, 2.0) | ✅ Improved |
| **V1 Compatibility** | N/A | ✅ Backward compatible | ✅ Added |
| **Culture Invariant** | ✅ InvariantCulture | ✅ InvariantCulture | ✅ Equivalent |

#### Serialization Helpers

| Helper | VB Version | C# Version | Status |
|--------|-----------|------------|--------|
| Color | ✅ GetColorAttribute | ✅ SerializeColor/DeserializeColor | ✅ Equivalent |
| Double | ✅ GetDoubleAttribute | ✅ SerializeDouble/DeserializeDouble | ✅ Equivalent |
| Integer | ✅ GetIntegerAttribute | ✅ SerializeInt/DeserializeInt | ✅ Equivalent |
| Boolean | ✅ GetBooleanAttribute | ✅ SerializeBoolean/DeserializeBoolean | ✅ Equivalent |
| String | ✅ GetStringAttribute | ✅ Direct attribute access | ✅ Equivalent |
| FontWeight | ✅ GetFontWeightAttribute | ✅ SerializeFontWeight/DeserializeFontWeight | ✅ Equivalent |
| Thickness | ✅ GetThicknessAttribute | ✅ SerializeThickness/DeserializeThickness | ✅ Equivalent |
| Enum | ✅ GetEnumAttribute | ✅ SerializeEnum/DeserializeEnum | ✅ Equivalent |
| DataPoint | ✅ GetDataPointAttribute | ✅ SerializeDataPoint/DeserializeDataPoint | ✅ Equivalent |
| ScreenPoint | ✅ GetScreenPointAttribute | ✅ SerializeScreenPoint/DeserializeScreenPoint | ✅ Equivalent |
| ScreenVector | ✅ GetScreenVectorAttribute | ✅ SerializeScreenVector/DeserializeScreenVector | ✅ Equivalent |
| LineStyle | ❌ N/A | ✅ SerializeLineStyle/DeserializeLineStyle | ✅ C# Added |
| Brush | ✅ GetBrushAttribute | ✅ SerializeBrush/DeserializeBrush | ✅ Equivalent |
| FontFamily | ✅ GetFontFamilyAttribute | ✅ SerializeFontFamily/DeserializeFontFamily | ✅ Equivalent |

---

### Extension Methods / Utilities

| Feature | VB Version | C# Version | Status |
|---------|-----------|------------|--------|
| **Location** | ExtensionsModule.vb | OxyPlotExtensions.cs | ✅ Equivalent |
| DataPoint ToPrettyText | ✅ Extension | ❌ Not yet | ⚠️ Missing |
| ScreenPoint ToPrettyText | ✅ Extension | ❌ Not yet | ⚠️ Missing |
| ScreenVector ToPrettyText | ✅ Extension | ❌ Not yet | ⚠️ Missing |
| FromAxisProperties | ✅ Extension | ❌ Not yet | ⚠️ Missing |
| IsBound | ✅ Extension | ❌ Not yet | ⚠️ Missing |
| CopyBinding | ✅ Extension | ❌ Not yet | ⚠️ Missing |
| GetFirstAbstractBaseType | ✅ Extension | ❌ Not yet | ⚠️ Missing |

---

### Infrastructure

| Component | VB Version | C# Version | Status |
|-----------|-----------|------------|--------|
| **Converters** | Inline | OxyColorConverter, OxyFontSizeConverter, OxyThicknessConverter | ✅ Improved |
| **Factories** | Inline type switching | AxisControlFactory, AnnotationControlFactory, SeriesControlFactory | ✅ Improved |
| **Managers** | Inline | AnnotationManager, CursorManager, PlotGestureHandler, SeriesManager | ✅ Improved |
| **Base Classes** | None | AxisControlBase, AnnotationControlBase, SeriesControlBase, PlotControlBase | ✅ Added |
| **Validation** | Inline | ValidationControl | ✅ Added |

---

### File Structure Comparison

| VB Structure | C# Structure | Status |
|--------------|--------------|--------|
| `/Annotations/AnnotationControl.xaml` | `/Controls/Annotations/ArrowAnnotationControl.xaml` | ✅ Split by type |
| `/Annotations/AnnotationSelectorControl.xaml` | `/Controls/Selectors/AnnotationSelectorControl.xaml` | ✅ Reorganized |
| `/Axes/AxesControl.xaml` | `/Controls/Selectors/AxesControl.xaml` | ✅ Reorganized |
| `/Axes/AxisControl.xaml` | `/Controls/Axes/LinearAxisControl.xaml` (etc.) | ✅ Split by type |
| `/Series/*.xaml` | `/Controls/Series/*.xaml` | ✅ Reorganized |
| `ExtensionsModule.vb` | `/Utilities/OxyPlotExtensions.cs` | ✅ Reorganized |
| `OxyplotSettingsSerializer.vb` | `/Serialization/PlotModelSerializer.cs` | ✅ Reorganized |
| N/A | `/Controls/Base/*.cs` | ✅ Added |
| N/A | `/Factories/*.cs` | ✅ Added |
| N/A | `/Managers/*.cs` | ✅ Added |
| N/A | `/Converters/*.cs` | ✅ Added |

---

## Feature Parity Status

### Fully Implemented (✅)

1. **OxyplotToolbar core functionality**
   - Pan/Zoom/Pointer modes
   - All 9 annotation drawing tools
   - Annotation editing for all types
   - Save image dialog
   - Properties event

2. **Property controls**
   - GeneralPlotControl
   - LegendControl
   - AxesControl with per-type editors
   - SeriesSelectorControl with per-type editors
   - AnnotationSelectorControl with per-type editors

3. **Serialization**
   - General properties
   - Version detection
   - Backward compatibility

### Partially Implemented (⚠️)

1. **Some extension methods** - Not all VB extensions ported (IsBound, CopyBinding, etc.)

### Not Yet Implemented (❌)

1. Binding copy utilities (IsBound, CopyBinding extensions)

---

## Improvements in C# Version

1. **Architecture**
   - Uses PlotView/PlotModel instead of Plot control
   - Proper separation of concerns with Managers, Factories, Base classes
   - File-scoped namespaces

2. **Type Safety**
   - Nullable reference types
   - Pattern matching with switch expressions
   - Modern C# language features

3. **Control Organization**
   - Each annotation type has its own control
   - Each axis type has its own control
   - Base classes reduce code duplication

4. **New Axis Types**
   - TimeSpanAxisControl
   - NormalProbabilityAxisControl
   - GumbelProbabilityAxisControl
   - AngleAxisControl
   - MagnitudeAxisControl
   - LinearColorAxisControl

5. **New Series Types**
   - AreaSeriesControl
   - PieSeriesControl

6. **Serialization**
   - Versioned format
   - V1 backward compatibility
   - Cleaner helper methods

---

## Recommendations for Full Parity

### High Priority
1. Implement LogarithmicAxisControl
2. Add remaining serialization helpers
3. Complete Export Data feature (Excel/SQLite)

### Medium Priority
1. Add custom cursor support from resources
2. Implement lazy loading for property controls
3. Port remaining extension methods

### Low Priority
1. Add ShowCloseButton to OxyplotPropertiesControl
2. Implement full PropertyEXP enum navigation
3. Add style dependency properties

---

## Migration Notes

### For Developers
1. Replace `Plot` property references with `PlotView`
2. Update serialization format to v2.0
3. Use factory classes for dynamic control creation
4. Leverage base classes for custom controls

### For Users
1. Existing XML settings will load via V1 compatibility
2. New features require re-saving settings
3. UI behavior remains consistent
