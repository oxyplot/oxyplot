2014.1.*
--------

New features

- Support data binding paths ("Point.X") (#210)
- Support for Xamarin.Forms (#204)
- Support for Windows Universal apps (#190)

Enhancements

- Improve TrackerFormatString consistency (#214)
- Support LineColor.BrokenLineColor
- LabelFormatString for ScatterSeries (#12)

Breaking changes

- Changed tracker format strings arguments (#214)
- Rename OxyPenLineJoin to LineJoin
- Rename LineStyle.Undefined to LineStyle.Automatic

Bugfixes

- Improved text rendering for Android and iOS (#209)
- Custom shape outline for PointAnnotation (#174)
- Synchronize Wpf.Axis.MinimumRange (#205)
- TrackerHitResult bug (#198)
- Position of axis when PositionAtZeroCrossing = true (#189)
- Expose ScatterSeries.ActualPoints (#201)
- Add overridable Axis.FormatValueOverride (#181)
- PngExporter text formatting (#170)