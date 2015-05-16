# Change Log
All notable changes to this project will be documented in this file.

## [Unreleased]
### Added
- Created a new OxyPlot.Mobile NuGet package to combine the mobile platforms into a single package. (#362)
- Support for XWT (#295)
- TwoColorAreaSeries (#299)
- Delta values in AxisChangedEventArgs (#276)
- Enable Git source server (added GitLink build step) (#267,#266)
- iOS PlotView ZoomThreshold/AllowPinchPastZero for use with KeepAspectRatioWhenPinching=false (#359)
- CandleStickAndVolumeSeries and VolumeSeries (#377)
- Axis.DesiredSize property (#383)
- Added WPF Wrapper for BoxPlot (#434)
- Added capability to display mean value to BoxPlot (#440)

### Changed
- Renamed OxyPlot.WindowsUniversal to OxyPlot.Windows (#242)
- Changed OxyPlot.Xamarin.Forms to require OxyPlot.Mobile dependency instead of each separate NuGet. (#362)
- Renamed OxyPlot.XamarinIOS to OxyPlot.MonoTouch (#327)
- Renamed OxyPlot.XamarinAndroid to OxyPlot.Xamarin.Android (#327)
- Renamed OxyPlot.XamarinForms to OxyPlot.Xamarin.Forms (#327)
- Renamed OxyPlot.XamarinForms.iOS to OxyPlot.Xamarin.Forms.Platform.iOS (#327)
- Renamed OxyPlot.XamarinFormsIOS to OxyPlot.Xamarin.Forms.Platform.iOS.Classic (#327)
- Renamed OxyPlot.XamarinFormsAndroid to OxyPlot.Xamarin.Forms.Platform.Android (#327)
- Renamed OxyPlot.XamarinFormsWinPhone to OxyPlot.Xamarin.Forms.Platform.WP8 (#327)
- Changed OxyPlot.Xamarin.Android target to Android level 10 (#223)
- Separated WPF Plot and PlotView (#252,#239)
- Current CandleStickSeries renamed to OldCandleStickSeries, replaced by a faster implementation (#369)
- Fixed axis min/max calc and axis assignment for CandleStick + VolumeSeries (#389)
- Invalidate plot when ItemsSource contents change (INotifyCollectionChanged) on WPF only (#406)
- Xamarin.Forms references updated to 1.4.2 (#293,#439)
- Change OxyPlot.Xamarin.Forms.Platform.Android target to Android level 15 (#439)
- Changed OxyPlot.Xamarin.Forms to portable Profile259 (#439)
- PlotController should not intercept input per default (#446)
- Changed DefaultTrackerFormatString for BoxPlotSeries (to include Mean) (#440)
- Changed Constructor of BoxPlotItem (to include Mean) (#440)
- Changed Axis, Annotation and Series Render() method (removed model parameter)

### Removed
- OxyPlot.Metro project (superseded by OxyPlot.WindowsUniversal) (#241)
- PlotModel.ToSvg method. Use the SvgExporter instead. (#347)
- Constructors with parameters. Use default constructors instead. (#347)
- Axis.ShowMinorTicks property. Use MinorTickSize = 0 instead. (#347)
- ManipulatorBase.GetCursorType method (#447)
- Model.GetElements() method
- Remove SL4 support (#115)
- Remove NET35 support (#115)
- PlotElement.Format method. Use StringHelper.Format instead.

### Fixed
- Tracker position is wrong when PlotView is offset from origin (#455)
- CategoryAxis should use StringFormat (#415)
- Fixed the dependency of OxyPlot.Xamarin.Forms NuGet (#370)
- Add default ctor for Xamarin.Forms iOS renderer (#348)
- Windows Phone cursor exception (#345)
- Bar/ColumSeries tracker format string bug (#333)
- Fix exception for default tracker format strings (#265)
- Fix center-aligned legends (#79)
- Fix Markdown links to tag comparison URL with footnote-style links.
- WPF dispatcher issue (#311,#309)
- Custom colors for scatters (#307)
- Rotated axis labels (#303,#301)
- Floating point error on axis labels (#289,#227)
- Performance of CandleStickSeries (#290)
- Tracker text for StairStepSeries (#263)
- XamarinForms/iOS view not updating when model is changed (#262)
- Improved WPF rendering performance (#260,#259)
- Null reference with MVVM binding (#255)
- WPF PngExporter background (#234)
- XamlExporter background (#233)
- .NET 3.5 build (#229)
- Support WinPhone 8.1 in core NuGet package (#161)
- Draw legend line with custom pattern (#356)
- iOS pan/zoom stability (#336)
- Xamarin.Forms iOS PlotViewRenderer crash (#458) 
- Inaccurate tracker when using LogarithmicAxis (#443)
- Fix reset of transforms in WinForms render context (#489)
- Fix StringFormat for TimeSpanAxis not recognizing f, ff, fff, etc (#330)

## [2014.1.546] - 2014-10-22
### Added
- Support data binding paths ("Point.X") (#210)
- Support for Xamarin.Forms (#204)
- Support for Windows Universal apps (#190)
- Improve TrackerFormatString consistency (#214)
- Support LineColor.BrokenLineColor
- LabelFormatString for ScatterSeries (#12)

### Changed
- Changed tracker format strings arguments (#214)
- Rename OxyPenLineJoin to LineJoin
- Rename LineStyle.Undefined to LineStyle.Automatic

### Fixed
- Improved text rendering for Android and iOS (#209)
- Custom shape outline for PointAnnotation (#174)
- Synchronize Wpf.Axis.MinimumRange (#205)
- TrackerHitResult bug (#198)
- Position of axis when PositionAtZeroCrossing = true (#189)
- Expose ScatterSeries.ActualPoints (#201)
- Add overridable Axis.FormatValueOverride (#181)
- PngExporter text formatting (#170)

[Unreleased]: https://github.com/oxyplot/oxyplot/compare/v2014.1.546...HEAD
[2014.1.546]: https://github.com/oxyplot/oxyplot/compare/v2014.1.319...v2014.1.546
