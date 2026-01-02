# C# OxyPlotControls Bug & Edge Case Review Report

**Date:** 2026-01-02
**Reviewer:** Claude
**Branch:** claude/review-oxyplot-vb-upgrade-Xzts7

---

## Executive Summary

This document provides a comprehensive bug/edge case review of the C# OxyPlotControls library and identifies remaining feature disparity with the VB version. The review covers:

1. OxyPlotToolbar.xaml.cs (2074 lines)
2. PlotModelSerializer.cs (1355 lines)
3. SerializationHelpers.cs (507 lines)
4. OxyplotPropertiesControl.xaml.cs (117 lines)

---

## Critical Issues

### 1. ScatterSeries Type Mismatch in Serialization ✅ FIXED

**Location:** `PlotModelSerializer.cs` lines 553-564 vs 1180-1183

**Issue:** Serialization uses `ScatterSeries` (non-generic) but deserialization creates `ScatterSeries<ScatterPoint>` (generic). This type mismatch could cause issues during round-trip serialization.

**Fix Applied:** Changed deserialization to use non-generic `ScatterSeries` to match serialization.

### 2. HeatMap Export Division by Zero Risk ✅ FIXED

**Location:** `OxyPlotToolbar.xaml.cs` line 1872

**Issue:** If HeatMap has only one row (`Data.GetLength(1) == 1`), division by zero occurs:
```csharp
row[1] = heatMapSeries.Y0 + y * (heatMapSeries.Y1 - heatMapSeries.Y0) / (heatMapSeries.Data.GetLength(1) - 1);
```

**Fix Applied:** Added conditional check to use `Y0` directly when there's only one row, avoiding division by zero.

### 3. CategoryAxis Labels Pipe Character Issue ✅ FIXED

**Location:** `PlotModelSerializer.cs` lines 303, 920

**Issue:** Category labels are joined with `|` separator, but if a label contains `|`, the deserialization will incorrectly split it.

**Fix Applied:** Changed serialization to use child XML elements (`<Labels><Label>...</Label></Labels>`) instead of pipe-separated attribute. Deserialization has backward compatibility to read old pipe-separated format.

---

## Medium Priority Issues

### 4. V1 Deserialization Incomplete

**Location:** `PlotModelSerializer.cs` line 1284

**Issue:** V1 backward compatibility only deserializes General properties. Axes, Series, and Annotations are not deserialized from V1 format (marked with TODO).

**Impact:** Users loading old VB-generated XML files will lose axes, series, and annotations.

### 5. Line Annotation Tooltip Missing Axis Formatting

**Location:** `OxyPlotToolbar.xaml.cs` lines 1634-1641

**Issue:** Tooltip uses generic `G6` format:
```csharp
_lineAnnotationTooltip.Content = $"{axis} = {value:G6}";
```

The VB version used axis-specific formatting (DateTimeAxis shows dates, CategoryAxis shows labels).

**Impact:** Poor UX when adding line annotations on DateTime or Category axes.

### 6. Polygon Creation Allows Degenerate Polygons

**Location:** `OxyPlotToolbar.xaml.cs` lines 1254-1266

**Issue:** If user double-clicks immediately after first or second point, a degenerate polygon (1-2 points) may be created.

**Impact:** Visual artifacts or rendering errors.

---

## Low Priority Issues

### 7. Memory Leak Risk - Event Handler Accumulation

**Location:** `OxyPlotToolbar.xaml.cs` lines 210-227

**Issue:** Event handlers are unsubscribed only when `oldPlot?.ActualModel != null`. If PlotView.ActualModel changes without PlotView changing, stale handlers may accumulate.

**Impact:** Minor memory leak over many PlotModel changes.

### 8. Leader Line Canvas Positioning

**Location:** `OxyPlotToolbar.xaml.cs` lines 272-278

**Issue:** `LeaderLine` initialization doesn't verify parent Canvas alignment with PlotView. May cause visual offset issues in complex layouts.

**Impact:** Visual misalignment in certain layout scenarios.

### 9. Swap Axes Limited Series Support

**Location:** `OxyPlotToolbar.xaml.cs` lines 1708-1737

**Issue:** Only handles `LineSeries`, `ScatterSeries`, and `AreaSeries`. Missing support for:
- TwoColorLineSeries
- BoxPlotSeries
- HighLowSeries
- CandleStickSeries
- StairStepSeries

**Impact:** Swap axes button does nothing for unsupported series types.

---

## Feature Disparity: VB vs C#

### Remaining Gaps (Previously documented in VB_to_CSharp_Comparison_Matrix.md)

| Feature | VB Status | C# Status | Priority |
|---------|-----------|-----------|----------|
| LogarithmicAxisControl | Has generic control | ✅ Now implemented | ~~High~~ Done |
| Custom cursors (.cur files) | Loaded from resources | Using standard WPF cursors | Low |
| Excel/SQLite export | Full DatabaseManager support | CSV only | Medium |
| Ctrl+Click polyline/polygon point insertion | Implemented | ✅ Now implemented | ~~Medium~~ Done |
| Line annotation axis-aware tooltip | Axis-specific formatting | Generic G6 format | Medium |
| Property control lazy loading | Implemented | Not implemented | Low |
| ShowCloseButton on PropertiesControl | Implemented | Not implemented | Low |

### Features Added in C# (Not in VB)

| Feature | Notes |
|---------|-------|
| TimeSpanAxisControl | New axis type |
| NormalProbabilityAxisControl | New axis type |
| GumbelProbabilityAxisControl | New axis type |
| AngleAxisControl | Polar chart support |
| MagnitudeAxisControl | Polar chart support |
| LinearColorAxisControl | Color mapping support |
| AreaSeriesControl | New series type |
| PieSeriesControl | New series type |
| ScreenPoint serialization | Culture-invariant |
| ScreenVector serialization | Culture-invariant |
| Versioned XML format | V1/V2 detection |

---

## Serialization Helpers Status Update

The comparison matrix indicated ScreenPoint and ScreenVector helpers were missing. **These have now been implemented:**

```csharp
// SerializationHelpers.cs lines 418-492
public static string SerializeScreenPoint(ScreenPoint point)
public static ScreenPoint DeserializeScreenPoint(string value)
public static string SerializeScreenVector(ScreenVector vector)
public static ScreenVector DeserializeScreenVector(string value)
```

---

## Recommendations

### Immediate Fixes Needed

1. **Fix ScatterSeries type mismatch** - Change deserialization to use `ScatterSeries` instead of `ScatterSeries<ScatterPoint>`
2. **Fix HeatMap division by zero** - Add denominator check
3. **Fix CategoryAxis pipe separator** - Use escaping or XML elements for labels

### Future Enhancements

1. Complete V1 deserialization for full backward compatibility
2. Add axis-aware formatting for line annotation tooltips
3. Consider implementing Ctrl+click for polyline editing
4. Add LogarithmicAxisControl to axis control factory

---

## Code Quality Assessment

### Strengths
- Modern C# patterns (nullable, pattern matching, switch expressions)
- Well-organized file structure with factories and managers
- Culture-invariant serialization throughout
- Comprehensive annotation editing support
- Good separation of concerns

### Areas for Improvement
- Some edge cases not fully handled (degenerate shapes, single-element data)
- V1 deserialization incomplete
- Limited series type support in some features

---

## Conclusion

The C# OxyPlotControls library is a substantial and well-executed modernization of the VB version. While there are some bugs and edge cases to address, the core functionality is solid and several improvements have been made over the original. The recommended fixes above should be applied before production use, particularly the critical serialization and division-by-zero issues.
