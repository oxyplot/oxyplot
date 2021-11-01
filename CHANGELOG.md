# Change Log

All notable changes to this project will be documented in this file.

## [Unreleased]

### Fixed

- WPF - OxyPlot doesn't render inside a Popup ([#1796])

## [2.1.0] - 2021-10-02

### Added

- Made Legend Items clickable to toggle series visibility ([#644])
- Added properties LegendKey and SeriesGroupName to Series, allowing grouping series between multiple legends and/or within same legend ([#644])
- OxyPlot.ImageSharp ([#1188])
- WPF ExampleBrowser can display transposed versions of examples ([#1402])
- Support for transposable Annotations ([#1417])
- Support for NET40 ([#1452])
- Support for high DPI for WPF ([#149])
- EdgeRenderingMode property to PlotElement, allowing customization of the way edges are treated by the renderer ([#1428], [#1358], [#1077], [#843], [#145])
- Color palettes Viridis, Plasma, Magma, Inferno and Cividis ([#1505])
- Renderer based on SkiaSharp, including exporters for PNG, JPEG, PDF and SVG ([#1509])
- Example for Issue [#1524]: HitTracker IndexOutOfRangeException with HeatMapSeries
- Text shaping support to SkiaRenderContext ([#1520])
- PlotView based on SkiaRenderContext (OxyPlot.SkiaSharp.Wpf) ([#1515])
- DrawImage support to OxyPlot.ImageSharp ([#1530])
- Dpi support to OxyPlot.ImageSharp ([#1530])
- Dash support to OxyPlot.ImageSharp
- Clipping support to OxyPlot.ImageSharp
- JpegExporter for OxyPlot.ImageSharp
- Multi-line aligned text rendering example
- WindowsForms ExampleBrowser can display transposed versions of examples ([#1535])
- Example for Issue [#1545] showing the use of different line endings
- Support for unix line endings in OxyPlot.ImageSharp, OxyPlot.Svg, and OxyPlot.Pdf ([#1545])
- Multi-Line Text support to SkiaRenderContext ([#1538])
- Added title clipping to PlotModel ([#1510])
- Added LabelStep and LabelSpacing to contour series ([#1511])
- Example for Issue [#1481] showing text rendering with emoji
- Native Clipping for OxyPlot.SvgRenderContext ([#1564])
- Examples of full plot area polar plots with non-zero minimums ([#1586])
- Read-Only collection interfaces for .NET 4.0 ([#1600])
- Add PlotModel.AssignColorsToInvisibleSeries property to control whether invisible series are included or skipped when assigning automatic colors ([#1599])
- Overlapping bar series ([#1265])
- `AxisPosition.All` for axes which need a margin on all sides of the plot area ([#1574])
- IRenderContext.ClipCount property ([#1593])
- Additional parameters for HistogramSeries LabelFormatString
- Absolute screen-space axis margins ([#1569])
- netstandard2.0 TargetFramework ([#1668])
- Add a PlotView.TextMeasurementMethod property to allow using the much faster GlyphTypeface based measurement at runtime ([#1673])
- OxyPlot.Wpf.XamlRenderContext - this doesn't use StreamGeometry and can be used for rendering to XAML ([#1673])
- SkiaRenderContext.MiterLimit property ([#1690])
- Example for Issue [#1685] showing spurious lines in the ContourSeries
- Example for Issue [#1716] showing poor tick spacing on DateTimeAxis with interval types of Weeks or Years
- Example for label placement on BarSeries with non-zero BaseValue ([#1726])
- ExtrapolationLineSeries as described in [#1740] to allow to display pre-defined intervals of a line series with a different style than the rest.
- Added DataRange, unit tests and examples as support for ExtrapolationLineSeries.
- Add control over how far from the series the tracker fires ([#1736])
- Add option to check distance for result between data points ([#1736])
- Legend.AllowUseFullExtent property to control whether legends should be able to use the full extent of the plot ([#1743])
- Legend.ShowInvisibleSeries property to control whether invisible series should be shown on the legend ([#1730])
- OxyPlot.SkiaSharp.Wpf PlotView support for simple render transforms ([#1785])

### Changed

- Legends model ([#644])
- Default behaviour is now plot without Legend ([#644])
- Moved WPF Plot component to Oxyplot.Contrib.Wpf ([#1399])
- ErrorColumnSeries changed to ErrorBarSeries, also works in transposed mode ([#1402])
- Moved reporting functionality to separate projects ([#1403])
- Moved reporting functionality to oxyplot-reporting repository ([#1403])
- Change IPlotModel.Render to take an OxyRect ([#1425])
- Merge UIElement and SelectableElement into Element ([#1426])
- Upgrade to .NET Core 3.1 ([#1488])
- DrawRectangle(...), DrawLine(...), DrawEllipse(...), DrawPolygon(...) and related overloads in IRenderContext and related extensions in RenderingExtensions now require an EdgeRenderingMode
- Default color palette for LinearColorAxis from Jet to Viridis ([#1505])
- Extract most of the functionality from OxyPlot.Wpf into OxyPlot.Wpf.Shared to allow code sharing with other WPF PlotViews ([#1515])
- WPF ExampleBrowser can switch between Canvas and SkiaSharp renderers ([#1515])
- OxyPlot.ImageSharp now targets .NET Standard 1.3 ([#1530])
- SkiaRenderContext does not apply pixel snapping when rendering to vector graphic ([#1539])
- Mark OxyPlot.PdfExporter and OxyPlot.Pdf.PdfExporter as obsolete ([#1527])
- Replace Axis.DesiredSize by Axis.DesiredMargin, change signature of Axis.Measure(...) ([#453])
- Axis renderers now render all ticks they are provided ([#1580])
- Auto margins don't reserve space for axis labels if axis range is fixed ([#1577])
- CategoryAxis should not contain rendering information about BarSeries ([#741])
- CategorizedSeries changed to `BarSeriesBase<T>` ([#741])
- System.Drawing.Common references updated to 4.7.0 ([#1608])
- Invisible series are assigned automatic colors by default, configurable with PlotModel.AssignColorsToInvisibleSeries property that defaults to true ([#1599])
- StemSeries, AreaSeries, TwoColorAreaSeries, and StairStepSeries use `ActualMarkerColor` ([#1630])
- Axes with `AxisPosition.None` make no contribution to margins ([#1574])
- `AngleAxis` has position `AxisPosition.All` by default ([#1574])
- Clipping API changed from SetClip(...) and ResetClip() to PushClip(...) and PopClip() ([#1593])
- Remove TileMapAnnotation examples from automated testing ([#1667])
- Optimize clipping calls ([#1661])
- Mark CandleStickAndVolumeSeries as obsolete ([#1661])
- Implement StreamGeometry-based implementations of DrawEllipses, DrawLine, DrawLineSegments and DrawRectangle(s) which improves the rendering speed on WPF ([#1673])
- Change algorithm of ContourSeries.JoinContourSegments(). This should improve performance in most cases, but will cause labels to appear in different spots than before ([#1685])
- Updated Series.cd with ExtrapolationLineSeries and removed classes that do not exist anymore

### Removed

- Remove PlotModel.Legends ([#644])
- ColumnSeries - functionality is replaced by transposed BarSeries ([#1402])
- Copy to text report Ctrl+Alt+R ([#1403])
- Remove exporter Background properties ([#1409])
- Remove OxyThickness Width and Height properties ([#1429])
- RenderingExtensions.DrawRectangleAsPolygon(...) extension methods. IRenderContext.DrawRectangle(...) with an appropriate EdgeRenderingMode can be used instead.
- SkiaSharp.PdfExporter.Dpi property ([#1591])
- Axis.UpdateFromSeries(...) and Series.UpdateValidData() ([#741])
- Support for IRenderContext implementations without native clipping ([#1593])
- CohenSutherlandClipping and SutherlandHodgmanClipping ([#1593])
- DrawClippedXxx(...) extensions in RenderingExtensions ([#1661])
- PathAnnotation.ClipText property - text is now always clipped ([#1661])
- CanvasRenderContext.UseStreamGeometry property - this functionality is replaced by the new XamlRenderContext ([#1673])

### Fixed

- Legend font size is not affected by DefaultFontSize ([#1396])
- ErrorBarSeries, IntervalBarSeries and TornadoBarSeries work correctly in transposed mode ([#1402])
- Exception when rendering polygon with no points ([#1410])
- Custom tracker strings can cause exception in histogram chart ([#1455])
- OxyPlot.WindowsForms package description ([#1457])
- NullReference in VolumeSeries if no data in Items list ([#1491])
- Possible out-of-bounds exception in HeatMapSeries HitTest ([#1524])
- WPF CanvasRenderContext draws ellipses too small by half stroke thickness ([#1537])
- Text measurement and rendering in OxyPlot.ImageSharp
- ExampleLibrary reporting annotation-only PlotModels as transposable ([#1544])
- Auto plot margin not taking width of labels into account ([#453])
- WPF PlotView still focusable when Focusable is false ([#1440])
- Disposing a SkiaRenderContext can mess up fonts from another SkiaRenderContext instance ([#1573])
- Display of ampersands in OxyPlot.WindowsForms Tracker ([#1585])
- Full Plotarea Polar plot rendering with non-zero minimum values ([#1586])
- Auto margins are set incorrectly if Axis.TitleFontSize is set to non-default value (related to [#1577])
- Incomplete rendering of AreaSeries in some situations ([#1512])
- ColumnSeries / BarSeries not working with more than one value-axis ([#729])
- OxyPlot.SkiaSharp.SvgExporter plot background color ([#1619])
- MinimumPadding incorrect when MaximumPadding is non-zero ([#1625])
- Don't clip zerocrossing axis lines within plot bounds ([#1441])
- Incorrect margins when using Color Axes with AxisPosition.None ([#1574])
- OpenStreetMap example ([#1642])
- Incorrect clipping in TwoColorAreaSeries ([#1678])
- ScreenMin and ScreenMax on Horizontal and Vertical Axes depends on plot bounds ([#1652])
- Windows Forms clipping last line of measured text ([#1659])
- Inconsistent Zooming behaviour ([#1648])
- ContourSeries produce fake connections ([#1685])
- Zero-crossing axis bounds ([#1708])
- Incorrect label placement on BarSeries with non-zero BaseValue ([#1726])
- LineAnnotation Text Placement on Reversed Axes ([#1741])
- Image opacity in WinForms and Core Drawing ([#1766])
- Fix specified Visual is not an ancestor of this Visual issue in WPF rendering ([#1787])

## [2.0.0] - 2019-10-19

### Added

- WindowsForms and Wpf support .NET Core 3.0 ([#1331])
- PngExporter based on System.Common.Drawing (.NET Core) ([#1263])
- New polar plot axes filling the full plot area ([#1056])
- Support for three colors in histogram series ([#1305])
- Command to copy plot to the clipboard in Windows Forms (Ctrl-C) ([#1297])
- New `InterpolationAlgorithm` property in LineSeries and PolylineAnnotation ([#494])
- Catmull-Rom spline interpolation algorithms ([#494])
- FontSize, FontWeight and FontFamily on Wpf.TextAnnotation ([#1023])
- RectangleSeries ([#1060])
- InvalidNumberColor on Wpf.LinearColorAxis ([#1087])
- ContinuousHistogramSeries ([#1145])
- Multiline text support for PortableDocumentFont ([#1146])
- Workaround for text vertical alignment in SVG Export to accomodate viewers which don't support dominant-baseline ([#459], [#1198])
- Support for transposed (X and Y axis switched) plots with XYAxisSeries ([#1334])
- Color property on HistogramItem ([#1347])
- Count property on HistogramSeries ([#1347])
- Bug in HistogramHelpers.Collect when using out-of-order bin breaks ([#1476])

### Changed

- OxyPlot.Core changed to target netstandard 1.0 and net45 ([#946], [#1147])
- OxyPlot.ExampleLibrary changed to target netstandard 1.0 and net45 ([#946], [#1147])
- OxyPlot.Wpf, OxyPlot.WindowsForms, OxyPlot.Pdf changed to target net45 ([#946])
- Place label below negative ColumnSeries ([#1119])
- Use PackageReference instead of packages.config
- Migrated NUnit v2 to v3 and added test adapter
- TrackerControl reuses existing ContentControl when a new hit tracker result uses the same template as the currently shown tracker ([#1281])
- Overhaul HistogramHelpers ([#1345])
- OxyPlot.Windows (UWP) moved to oxyplot-uwp repository ([#1378])
- Make the PngExporters consistent ([#1390])
- Silverlight project moved to separate repository ([#1049])
- WP8 project moved to separate repository ([#1050])
- Windows8 project moved to separate repository ([#1103])
- Avalonia project moved to separate repository
- Xamarin projects moved to separate repository
- Xwt project moved to separate repository
- GTK# projects moved to separate repository
- SharpDX project moved to separate repository

### Deprecated

- OxyPlot.WP8 package. Use OxyPlot.Windows (UWP) instead ([#996])

### Removed

- LabelFontSize property from HistogramSeries ([#1309])
- Smooth property from LineSeries and PolylineAnnotation ([#494])
- Support for net40 ([#960])

### Fixed

- Manipulation when using touch is not working in Windows ([#1011])
- Ensure a suitable folder is used when creating a temporary file for PNG export in Oxyplot.GtkSharp ([#1034])
- RangeColorAxis is not rendered correctly if the axis is reversed ([#1035])
- OxyMouseEvents not caught due to InvalidatePlot() in WPF ([#382])
- When Color Property of LineSeries is set Markers are not shown ([#937])
- Change from linear to logarithmic axis does not work ([#1067])
- OxyPalette.Interpolate() throws exception when paletteSize = 1 ([#1068])
- Infinite loop in LineAnnotation ([#1029])
- OverflowException when zoomed in on logarithmic axis ([#1090])
- ScatterSeries with DateTimeAxis/TimeSpanAxis ([#1132])
- Exporting TextAnnotation with TextColor having 255 alpha to SVG produces opaque text ([#1160])
- Chart is not updated when top and bottom are not visible ([#1219])
- Candle overlap each candle ([#623])
- CandleStick is overlapped when item.Open == item.Close in the CandleStickAndVolumeSeries ([#1245])
- Out of memory exception and performance issue with Catmull-Rom Spline ([#1237])
- Cache and Dispose Brush and Pen objects used by GraphicsRenderContext ([#1230])
- Add checks for non-positive StrokeThickess and LineStyle.None in various places ([#1312])
- Fixed references to RectangleItem in HistogramSeries
- Fix AxisChangedEventArgs.DeltaMaximum in Axes.Reset ([#1306])
- Fixed Tracker for RectangleBarSeries ([#1171])
- RectangleSeries doesn't render Labels (related to [#1334])
- LineSeries line legend placement with reversed X axis (related to [#1334])
- HistogramSeries label placement inconsistent (related to [#1334])
- TwoColorLineSeries and ThreeColorLineSeries don't work with reversed Y axis (related to [#1334])
- Issue with SVG always containing the xml headers ([#1212])
- In WPF, make sure the axes are initalized when the Model is set before the PlotView has been loaded ([#1303])
- MinimumSegmentLength not working for LineSeries ([#1044])
- rendering issues with MagnitudeAxisFullPlotArea ([#1364])
- OxyPlot.Core.Drawing PngExporter background when exporting to stream ([#1382])
- Windows Forms clipping last line of rendered text ([#1124], [#1385])
- Dispose background brush in OxyPlot.Core.Drawing PngExporter ([#1392])

## [1.0.0] - 2016-09-11

### Added

- Added OxyPlot.SharpDX.Wpf NuGet package
- Added DirectX 9.1-10.1 feature level support for SharpDX renderer
- Added SharpDX based renderer and WPF control with SharpDX render ([#124])
- Added MinimumMajorStep and MinimumMinorStep to Axes.Axis ([#816])
- Added support for vertical X axis to HeatMapSeries ([#535])
- Added fall-back rectangle rendering to HeatMapSeries ([#801])
- Added logarithmic HeatMapSeries support ([#802]) and example
- Axis.MaximumRange to limit the zoom ([#401])
- Added OxyPlot.Mobile NuGet package to combine the mobile platforms into a single package ([#362])
- Support for XWT ([#295])
- TwoColorAreaSeries ([#299])
- Delta values in AxisChangedEventArgs ([#276])
- Git source server (added GitLink build step) ([#267],[#266])
- iOS PlotView ZoomThreshold/AllowPinchPastZero for use with KeepAspectRatioWhenPinching=false ([#359])
- CandleStickAndVolumeSeries and VolumeSeries ([#377])
- Axis.DesiredSize property ([#383])
- WPF wrapper for BoxPlotSeries ([#434])
- Capability to display mean value to BoxPlotSeries ([#440])
- LinearBarSeries for WPF ([#506])
- TitleToolTip in PlotModel ([#508])
- TextColor property on WPF Axis ([#452])
- ThreeColorLineSeries ([#378])
- CI of the Xamarin.Android, Xamarin.iOS and Xamarin.Forms packages ([#274])
- PlotModel.LegendLineSpacing ([#622])
- Legend rendering for LinearBarSeries ([#663])
- LegendMaxHeight property in PlotModel and Wpf.Plot ([#668])
- Support for a Xamarin Forms UWP project with sample app ([#697])
- ListBuilder for building lists by reflection ([#705])
- F# example ([#699])
- Support for discontinuities in AreaSeries ([#215])
- Support for Windows Universal 10.0 apps ([#615])
- Support Unicode in OxyPlot.Pdf ([#789])
- TouchTrackerManipulator ([#787])
- Extracted visible window search code from CandleStickSeries and made a generic version in XYSeries. Used it to optimize AreaSeries performance. ([#834])
- Optimized rendering performance of RectangleBarSeries ([#834]).
- PdfExporter implementing IExporter ([#845])
- Color minor and major ticks differently ([#417])
- Support for PieSeries in OxyPlot.Wpf ([#878])
- Filter in example browser ([#118])
- Support for tooltips on WPF annotations
- Support for tracker in OxyPlot.GtkSharp
- Improve tracker style (Windows Forms) ([#106])
- Font rendering in OxyPlot.GtkSharp improved by using Pango ([#972])
- Improved LineSeries performance ([#834])
- Fixed bug causing axes titles to not display in OxyPlot.GtkSharp ([#989])

### Changed

- Fixed closing file stream for PdfReportWriter when PdfReportWriter is closed or disposed of. ([#892])
- Renamed OxyPlot.WindowsUniversal to OxyPlot.Windows ([#242])
- Changed OxyPlot.Xamarin.Forms to require OxyPlot.Mobile dependency instead of each separate NuGet. ([#362])
- Renamed OxyPlot.XamarinIOS to OxyPlot.MonoTouch ([#327])
- Renamed OxyPlot.XamarinAndroid to OxyPlot.Xamarin.Android ([#327])
- Renamed OxyPlot.XamarinForms to OxyPlot.Xamarin.Forms ([#327])
- Renamed OxyPlot.XamarinForms.iOS to OxyPlot.Xamarin.Forms.Platform.iOS ([#327])
- Renamed OxyPlot.XamarinFormsIOS to OxyPlot.Xamarin.Forms.Platform.iOS.Classic ([#327])
- Renamed OxyPlot.XamarinFormsAndroid to OxyPlot.Xamarin.Forms.Platform.Android ([#327])
- Renamed OxyPlot.XamarinFormsWinPhone to OxyPlot.Xamarin.Forms.Platform.WP8 ([#327])
- Changed OxyPlot.Xamarin.Android target to Android level 10 ([#223])
- Separated WPF Plot and PlotView ([#252], [#239])
- Current CandleStickSeries renamed to OldCandleStickSeries, replaced by a faster implementation ([#369])
- Invalidate plot when ItemsSource contents change (INotifyCollectionChanged) on WPF only ([#406])
- Xamarin.Forms references updated to 1.5.0.6447 ([#293], [#439])
- Change OxyPlot.Xamarin.Forms.Platform.Android target to Android level 15 ([#439])
- Changed OxyPlot.Xamarin.Forms to portable Profile259 ([#439])
- PlotController should not intercept input per default ([#446])
- Changed DefaultTrackerFormatString for BoxPlotSeries (to include Mean) ([#440])
- Changed Constructor of BoxPlotItem (to include Mean) ([#440])
- Changed Axis, Annotation and Series Render() method (removed model parameter)
- Changed PCL project to profile 259, SL5 is separate now ([#115])
- Extracted CreateReport() and CreateTextReport() from PlotModel ([#517])
- Renamed GetLastUpdateException to GetLastPlotException and added the ability to see render exceptions([#543])
- Move TileMapAnnotation class to example library ([#567])
- Change to semantic versioning ([#595])
- Change GTKSharp3 project to x86 ([#599])
- Change OxyPlot.Xamarin.Android to API Level 15 ([#614])
- Add Xamarin.Forms renderer initialization to PlotViewRenderer ([#632])
- Marked OxyPlot.Xamarin.Forms.Platform.*.Forms.Init() obsolete ([#632])
- Throw exception if Xamarin.Forms renderer is not 'initialized' ([#492])
- Make numeric values of DateTimeAxis compatible with ToOADate ([#660])
- Make struct types immutable ([#692])
- Implement `IEquatable<T>` for struct types ([#692])
- BoxPlotItem changed to reference type ([#692])
- Move Xamarin projects to new repository ([#777])
- Remove CandleStickSeries.Append ([#826])
- Change MinorInterval calculation, add unit test ([#133])
- Rewrite LogarithmicAxis tick calculation ([#820])
- Change Axis methods to protected virtual ([#837])
- Move CalculateMinorInterval and CreateTickValues to AxisUtilities ([#837])
- Change default number format to "g6" in Axis base class ([#841])
- Push packages to myget.org ([#847])
- Change the default format string to `null` for TimeSpanAxis and DateTimeAxis ([#951])

### Removed

- StyleCop tasks ([#556])
- OxyPlot.Metro project (superseded by OxyPlot.WindowsUniversal) ([#241])
- PlotModel.ToSvg method. Use the SvgExporter instead. ([#347])
- Constructors with parameters, use default constructors instead. ([#347])
- Axis.ShowMinorTicks property, use MinorTickSize = 0 instead ([#347])
- ManipulatorBase.GetCursorType method ([#447])
- Model.GetElements() method
- Remove SL4 support ([#115])
- Remove NET35 support ([#115])
- PlotElement.Format method, use StringHelper.Format instead
- EnumerableExtensions.Reverse removed ([#677])
- ListFiller ([#705])

### Fixed

- Added check to LineAnnotation.GetScreenPoints to check if ActualMaximumX==ActualMinimumX for non-curved lines. ([#1029])
- Incorrect placment of axis title of axes with AxisDistance ([#1065])
- SharpDX control not being rendered when loaded
- SharpDX out of viewport scrolling.
- Multiple mouse clicks not being reported in OxyPlot.GtkSharp ([#854])
- StemSeries Tracking to allow tracking on tiny stems ([#809])
- Fixed PDFRenderContext text alignment issues for rotated text ([#723])
- HeatMapSeries.GetValue returns NaN instead of calculating a wrong value in proximity to NaN ([#256])
- Tracker position is wrong when PlotView is offset from origin ([#455])
- CategoryAxis should use StringFormat ([#415])
- Fixed the dependency of OxyPlot.Xamarin.Forms NuGet ([#370])
- Add default ctor for Xamarin.Forms iOS renderer ([#348])
- Windows Phone cursor exception ([#345])
- Bar/ColumSeries tracker format string bug ([#333])
- Fix exception for default tracker format strings ([#265])
- Fix center-aligned legends ([#79])
- Fix Markdown links to tag comparison URL with footnote-style links
- WPF dispatcher issue ([#311], [#309])
- Custom colors for scatters ([#307])
- Rotated axis labels ([#303],[#301])
- Floating point error on axis labels ([#289], [#227])
- Performance of CandleStickSeries ([#290])
- Tracker text for StairStepSeries ([#263])
- XamarinForms/iOS view not updating when model is changed ([#262])
- Improved WPF rendering performance ([#260], [#259])
- Null reference with MVVM binding ([#255])
- WPF PngExporter background ([#234])
- XamlExporter background ([#233])
- .NET 3.5 build ([#229])
- Support WinPhone 8.1 in core NuGet package ([#161])
- Draw legend line with custom pattern ([#356])
- iOS pan/zoom stability ([#336])
- Xamarin.Forms iOS PlotViewRenderer crash ([#458])
- Inaccurate tracker when using LogarithmicAxis ([#443])
- Fix reset of transforms in WinForms render context ([#489])
- Fix StringFormat for TimeSpanAxis not recognizing f, ff, fff, etc ([#330])
- Fix  LineSeries SMOOTH=True will crash WinForms on right click ([#499])
- Fix PlotView leak on iOS ([#503])
- This PlotModel is already in use by some other PlotView control ([#497])
- LegendTextColor not synchronized between wpf.Plot and InternalModel ([#548])
- Legend in CandleStickSeries does not scale correctly ([#554])
- Fix CodeGenerator exception for types without parameterless ctor ([#573])
- Migrate automatic package restore ([#557])
- Fix rendering of rotated 'math' text ([#569], [#448])
- WPF export demo ([#568])
- Fixing a double comparison issue causing infinite loop ([#587])
- Fix null reference exception when ActualPoints was null rendering a StairStepSeries ([#582])
- Background color in the Xamarin.Forms views ([#546])
- IsVisible change in Xamarin.Forms.Platform.iOS ([#546])
- Rendering math text with syntax error gets stuck in an endless loop ([#624])
- Fix issue with MinimumRange not taking Minimum and Maximum values into account ([#550])
- Do not set default Controller in PlotView ctor ([#436])
- Corrected owner type of Wpf.PathAnnotation dependency properties ([#645])
- Fixed partial plot rendering on Xamarin.Android ([#649])
- Default controller should not be shared in WPF PlotViews ([#682])
- PositionAtZeroCrossing adds zero crossing line at wrong position ([#635])
- Implement AreaSeries.ConstantY2 ([#662])
- Null reference exception in ScatterSeries{T} actual points ([#636])
- Code generation for HighLowItem ([#634])
- Axis.MinimumRange did not work correctly ([#711])
- FillColor in ErrorColumnSeries ([#736])
- XAxisKey and YAxisKey added to Wpf.Annotations ([#743])
- Fix HeatMapSeries cannot plot on Universal Windows ([#745])
- Set Resolution in WinForms PngExporter ([#754])
- Axis should never go into infinite loop ([#758])
- Exception in BarSeriesBase ([#790])
- Vertical Axes Title Font Bug ([#474])
- Support string[] as ItemsSource in CategoryAxis ([#825])
- Horizontal RangeColorAxis ([#767])
- LogarithmicAxis sometimes places major ticks outside of the axis range ([#850])
- LineSeries with smoothing raises exception ([#72])
- Exception when legend is outside and plot area is small ([#880])
- Axis alignment with MinimumRange ([#794])
- Fixed strange number formatting when using LogarithmicAxis with very large or very small Series ([#589])
- Fixed LogarithmicAxis to no longer freeze when the axis is reversed ([#925])
- Prevent endless loop in LogarithmicAxis ([#957])
- Fixed WPF series data not refreshed when not visible (included WPF LiveDemo)
- Fixed bug in selection of plot to display in OxyPlot.GtkSharp ExampleBrowser ([#979])
- Fixed non-interpolation of HeatMapSeries in OxyPlot.GtkSharp ([#980])
- Fixed axis min/max calc and axis assignment for CandleStick + VolumeSeries ([#389])
- Fixed drawing of plot backgrounds in OxyPlot.GtkSharp ([#990])

## [0.2014.1.546] - 2014-10-22

### Added

- Support data binding paths ("Point.X") ([#210])
- Support for Xamarin.Forms ([#204])
- Support for Windows Universal apps ([#190])
- Improve TrackerFormatString consistency ([#214])
- Support LineColor.BrokenLineColor
- LabelFormatString for ScatterSeries ([#12])

### Changed

- Changed tracker format strings arguments ([#214])
- Rename OxyPenLineJoin to LineJoin
- Rename LineStyle.Undefined to LineStyle.Automatic

### Fixed

- Improved text rendering for Android and iOS ([#209])
- Custom shape outline for PointAnnotation ([#174])
- Synchronize Wpf.Axis.MinimumRange ([#205])
- TrackerHitResult bug ([#198])
- Position of axis when PositionAtZeroCrossing = true ([#189])
- Expose ScatterSeries.ActualPoints ([#201])
- Add overridable Axis.FormatValueOverride ([#181])
- PngExporter text formatting ([#170])

[Unreleased]: https://github.com/oxyplot/oxyplot/compare/v2.1.0...HEAD
[2.1.0]: https://github.com/oxyplot/oxyplot/compare/v2.0.0...v2.1.0
[2.0.0]: https://github.com/oxyplot/oxyplot/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/oxyplot/oxyplot/compare/v0.2014.1.546...v1.0.0
[0.2014.1.546]: https://github.com/oxyplot/oxyplot/compare/v0.0.1...v0.2014.1.546

[#12]: https://github.com/oxyplot/oxyplot/issues/12
[#72]: https://github.com/oxyplot/oxyplot/issues/72
[#79]: https://github.com/oxyplot/oxyplot/issues/79
[#106]: https://github.com/oxyplot/oxyplot/issues/106
[#115]: https://github.com/oxyplot/oxyplot/issues/115
[#118]: https://github.com/oxyplot/oxyplot/issues/118
[#124]: https://github.com/oxyplot/oxyplot/issues/124
[#133]: https://github.com/oxyplot/oxyplot/issues/133
[#145]: https://github.com/oxyplot/oxyplot/issues/145
[#149]: https://github.com/oxyplot/oxyplot/issues/149
[#161]: https://github.com/oxyplot/oxyplot/issues/161
[#170]: https://github.com/oxyplot/oxyplot/issues/170
[#174]: https://github.com/oxyplot/oxyplot/issues/174
[#181]: https://github.com/oxyplot/oxyplot/issues/181
[#189]: https://github.com/oxyplot/oxyplot/issues/189
[#190]: https://github.com/oxyplot/oxyplot/issues/190
[#198]: https://github.com/oxyplot/oxyplot/issues/198
[#201]: https://github.com/oxyplot/oxyplot/issues/201
[#204]: https://github.com/oxyplot/oxyplot/issues/204
[#205]: https://github.com/oxyplot/oxyplot/issues/205
[#209]: https://github.com/oxyplot/oxyplot/issues/209
[#210]: https://github.com/oxyplot/oxyplot/issues/210
[#214]: https://github.com/oxyplot/oxyplot/issues/214
[#215]: https://github.com/oxyplot/oxyplot/issues/215
[#223]: https://github.com/oxyplot/oxyplot/issues/223
[#227]: https://github.com/oxyplot/oxyplot/issues/227
[#229]: https://github.com/oxyplot/oxyplot/issues/229
[#233]: https://github.com/oxyplot/oxyplot/issues/233
[#234]: https://github.com/oxyplot/oxyplot/issues/234
[#239]: https://github.com/oxyplot/oxyplot/issues/239
[#241]: https://github.com/oxyplot/oxyplot/issues/241
[#242]: https://github.com/oxyplot/oxyplot/issues/242
[#252]: https://github.com/oxyplot/oxyplot/issues/252
[#255]: https://github.com/oxyplot/oxyplot/issues/255
[#256]: https://github.com/oxyplot/oxyplot/issues/256
[#259]: https://github.com/oxyplot/oxyplot/issues/259
[#260]: https://github.com/oxyplot/oxyplot/issues/260
[#262]: https://github.com/oxyplot/oxyplot/issues/262
[#263]: https://github.com/oxyplot/oxyplot/issues/263
[#265]: https://github.com/oxyplot/oxyplot/issues/265
[#266]: https://github.com/oxyplot/oxyplot/issues/266
[#267]: https://github.com/oxyplot/oxyplot/issues/267
[#274]: https://github.com/oxyplot/oxyplot/issues/274
[#276]: https://github.com/oxyplot/oxyplot/issues/276
[#289]: https://github.com/oxyplot/oxyplot/issues/289
[#290]: https://github.com/oxyplot/oxyplot/issues/290
[#293]: https://github.com/oxyplot/oxyplot/issues/293
[#295]: https://github.com/oxyplot/oxyplot/issues/295
[#299]: https://github.com/oxyplot/oxyplot/issues/299
[#301]: https://github.com/oxyplot/oxyplot/issues/301
[#303]: https://github.com/oxyplot/oxyplot/issues/303
[#307]: https://github.com/oxyplot/oxyplot/issues/307
[#309]: https://github.com/oxyplot/oxyplot/issues/309
[#311]: https://github.com/oxyplot/oxyplot/issues/311
[#327]: https://github.com/oxyplot/oxyplot/issues/327
[#330]: https://github.com/oxyplot/oxyplot/issues/330
[#333]: https://github.com/oxyplot/oxyplot/issues/333
[#336]: https://github.com/oxyplot/oxyplot/issues/336
[#345]: https://github.com/oxyplot/oxyplot/issues/345
[#347]: https://github.com/oxyplot/oxyplot/issues/347
[#348]: https://github.com/oxyplot/oxyplot/issues/348
[#356]: https://github.com/oxyplot/oxyplot/issues/356
[#359]: https://github.com/oxyplot/oxyplot/issues/359
[#362]: https://github.com/oxyplot/oxyplot/issues/362
[#369]: https://github.com/oxyplot/oxyplot/issues/369
[#370]: https://github.com/oxyplot/oxyplot/issues/370
[#377]: https://github.com/oxyplot/oxyplot/issues/377
[#378]: https://github.com/oxyplot/oxyplot/issues/378
[#382]: https://github.com/oxyplot/oxyplot/issues/382
[#383]: https://github.com/oxyplot/oxyplot/issues/383
[#389]: https://github.com/oxyplot/oxyplot/issues/389
[#401]: https://github.com/oxyplot/oxyplot/issues/401
[#406]: https://github.com/oxyplot/oxyplot/issues/406
[#415]: https://github.com/oxyplot/oxyplot/issues/415
[#417]: https://github.com/oxyplot/oxyplot/issues/417
[#434]: https://github.com/oxyplot/oxyplot/issues/434
[#436]: https://github.com/oxyplot/oxyplot/issues/436
[#439]: https://github.com/oxyplot/oxyplot/issues/439
[#440]: https://github.com/oxyplot/oxyplot/issues/440
[#443]: https://github.com/oxyplot/oxyplot/issues/443
[#446]: https://github.com/oxyplot/oxyplot/issues/446
[#447]: https://github.com/oxyplot/oxyplot/issues/447
[#448]: https://github.com/oxyplot/oxyplot/issues/448
[#452]: https://github.com/oxyplot/oxyplot/issues/452
[#453]: https://github.com/oxyplot/oxyplot/issues/453
[#455]: https://github.com/oxyplot/oxyplot/issues/455
[#458]: https://github.com/oxyplot/oxyplot/issues/458
[#459]: https://github.com/oxyplot/oxyplot/issues/459
[#474]: https://github.com/oxyplot/oxyplot/issues/474
[#489]: https://github.com/oxyplot/oxyplot/issues/489
[#492]: https://github.com/oxyplot/oxyplot/issues/492
[#494]: https://github.com/oxyplot/oxyplot/issues/494
[#497]: https://github.com/oxyplot/oxyplot/issues/497
[#499]: https://github.com/oxyplot/oxyplot/issues/499
[#503]: https://github.com/oxyplot/oxyplot/issues/503
[#506]: https://github.com/oxyplot/oxyplot/issues/506
[#508]: https://github.com/oxyplot/oxyplot/issues/508
[#517]: https://github.com/oxyplot/oxyplot/issues/517
[#535]: https://github.com/oxyplot/oxyplot/issues/535
[#543]: https://github.com/oxyplot/oxyplot/issues/543
[#546]: https://github.com/oxyplot/oxyplot/issues/546
[#548]: https://github.com/oxyplot/oxyplot/issues/548
[#550]: https://github.com/oxyplot/oxyplot/issues/550
[#554]: https://github.com/oxyplot/oxyplot/issues/554
[#556]: https://github.com/oxyplot/oxyplot/issues/556
[#557]: https://github.com/oxyplot/oxyplot/issues/557
[#567]: https://github.com/oxyplot/oxyplot/issues/567
[#568]: https://github.com/oxyplot/oxyplot/issues/568
[#569]: https://github.com/oxyplot/oxyplot/issues/569
[#573]: https://github.com/oxyplot/oxyplot/issues/573
[#582]: https://github.com/oxyplot/oxyplot/issues/582
[#587]: https://github.com/oxyplot/oxyplot/issues/587
[#589]: https://github.com/oxyplot/oxyplot/issues/589
[#595]: https://github.com/oxyplot/oxyplot/issues/595
[#599]: https://github.com/oxyplot/oxyplot/issues/599
[#614]: https://github.com/oxyplot/oxyplot/issues/614
[#615]: https://github.com/oxyplot/oxyplot/issues/615
[#622]: https://github.com/oxyplot/oxyplot/issues/622
[#623]: https://github.com/oxyplot/oxyplot/issues/623
[#624]: https://github.com/oxyplot/oxyplot/issues/624
[#632]: https://github.com/oxyplot/oxyplot/issues/632
[#634]: https://github.com/oxyplot/oxyplot/issues/634
[#635]: https://github.com/oxyplot/oxyplot/issues/635
[#636]: https://github.com/oxyplot/oxyplot/issues/636
[#644]: https://github.com/oxyplot/oxyplot/issues/644
[#645]: https://github.com/oxyplot/oxyplot/issues/645
[#649]: https://github.com/oxyplot/oxyplot/issues/649
[#660]: https://github.com/oxyplot/oxyplot/issues/660
[#662]: https://github.com/oxyplot/oxyplot/issues/662
[#663]: https://github.com/oxyplot/oxyplot/issues/663
[#668]: https://github.com/oxyplot/oxyplot/issues/668
[#677]: https://github.com/oxyplot/oxyplot/issues/677
[#682]: https://github.com/oxyplot/oxyplot/issues/682
[#692]: https://github.com/oxyplot/oxyplot/issues/692
[#697]: https://github.com/oxyplot/oxyplot/issues/697
[#699]: https://github.com/oxyplot/oxyplot/issues/699
[#705]: https://github.com/oxyplot/oxyplot/issues/705
[#711]: https://github.com/oxyplot/oxyplot/issues/711
[#723]: https://github.com/oxyplot/oxyplot/issues/723
[#729]: https://github.com/oxyplot/oxyplot/issues/729
[#736]: https://github.com/oxyplot/oxyplot/issues/736
[#741]: https://github.com/oxyplot/oxyplot/issues/741
[#743]: https://github.com/oxyplot/oxyplot/issues/743
[#745]: https://github.com/oxyplot/oxyplot/issues/745
[#754]: https://github.com/oxyplot/oxyplot/issues/754
[#758]: https://github.com/oxyplot/oxyplot/issues/758
[#767]: https://github.com/oxyplot/oxyplot/issues/767
[#777]: https://github.com/oxyplot/oxyplot/issues/777
[#787]: https://github.com/oxyplot/oxyplot/issues/787
[#789]: https://github.com/oxyplot/oxyplot/issues/789
[#790]: https://github.com/oxyplot/oxyplot/issues/790
[#794]: https://github.com/oxyplot/oxyplot/issues/794
[#801]: https://github.com/oxyplot/oxyplot/issues/801
[#802]: https://github.com/oxyplot/oxyplot/issues/802
[#809]: https://github.com/oxyplot/oxyplot/issues/809
[#816]: https://github.com/oxyplot/oxyplot/issues/816
[#820]: https://github.com/oxyplot/oxyplot/issues/820
[#825]: https://github.com/oxyplot/oxyplot/issues/825
[#826]: https://github.com/oxyplot/oxyplot/issues/826
[#834]: https://github.com/oxyplot/oxyplot/issues/834
[#837]: https://github.com/oxyplot/oxyplot/issues/837
[#841]: https://github.com/oxyplot/oxyplot/issues/841
[#843]: https://github.com/oxyplot/oxyplot/issues/843
[#845]: https://github.com/oxyplot/oxyplot/issues/845
[#847]: https://github.com/oxyplot/oxyplot/issues/847
[#850]: https://github.com/oxyplot/oxyplot/issues/850
[#854]: https://github.com/oxyplot/oxyplot/issues/854
[#878]: https://github.com/oxyplot/oxyplot/issues/878
[#880]: https://github.com/oxyplot/oxyplot/issues/880
[#892]: https://github.com/oxyplot/oxyplot/issues/892
[#925]: https://github.com/oxyplot/oxyplot/issues/925
[#937]: https://github.com/oxyplot/oxyplot/issues/937
[#946]: https://github.com/oxyplot/oxyplot/issues/946
[#951]: https://github.com/oxyplot/oxyplot/issues/951
[#957]: https://github.com/oxyplot/oxyplot/issues/957
[#960]: https://github.com/oxyplot/oxyplot/issues/960
[#972]: https://github.com/oxyplot/oxyplot/issues/972
[#979]: https://github.com/oxyplot/oxyplot/issues/979
[#980]: https://github.com/oxyplot/oxyplot/issues/980
[#989]: https://github.com/oxyplot/oxyplot/issues/989
[#990]: https://github.com/oxyplot/oxyplot/issues/990
[#996]: https://github.com/oxyplot/oxyplot/issues/996
[#1011]: https://github.com/oxyplot/oxyplot/issues/1011
[#1023]: https://github.com/oxyplot/oxyplot/issues/1023
[#1029]: https://github.com/oxyplot/oxyplot/issues/1029
[#1034]: https://github.com/oxyplot/oxyplot/issues/1034
[#1035]: https://github.com/oxyplot/oxyplot/issues/1035
[#1044]: https://github.com/oxyplot/oxyplot/issues/1044
[#1049]: https://github.com/oxyplot/oxyplot/issues/1049
[#1050]: https://github.com/oxyplot/oxyplot/issues/1050
[#1056]: https://github.com/oxyplot/oxyplot/issues/1056
[#1060]: https://github.com/oxyplot/oxyplot/issues/1060
[#1065]: https://github.com/oxyplot/oxyplot/issues/1065
[#1067]: https://github.com/oxyplot/oxyplot/issues/1067
[#1068]: https://github.com/oxyplot/oxyplot/issues/1068
[#1077]: https://github.com/oxyplot/oxyplot/issues/1077
[#1087]: https://github.com/oxyplot/oxyplot/issues/1087
[#1090]: https://github.com/oxyplot/oxyplot/issues/1090
[#1103]: https://github.com/oxyplot/oxyplot/issues/1103
[#1119]: https://github.com/oxyplot/oxyplot/issues/1119
[#1124]: https://github.com/oxyplot/oxyplot/issues/1124
[#1132]: https://github.com/oxyplot/oxyplot/issues/1132
[#1145]: https://github.com/oxyplot/oxyplot/issues/1145
[#1146]: https://github.com/oxyplot/oxyplot/issues/1146
[#1147]: https://github.com/oxyplot/oxyplot/issues/1147
[#1160]: https://github.com/oxyplot/oxyplot/issues/1160
[#1171]: https://github.com/oxyplot/oxyplot/issues/1171
[#1188]: https://github.com/oxyplot/oxyplot/issues/1188
[#1198]: https://github.com/oxyplot/oxyplot/issues/1198
[#1212]: https://github.com/oxyplot/oxyplot/issues/1212
[#1219]: https://github.com/oxyplot/oxyplot/issues/1219
[#1230]: https://github.com/oxyplot/oxyplot/issues/1230
[#1237]: https://github.com/oxyplot/oxyplot/issues/1237
[#1245]: https://github.com/oxyplot/oxyplot/issues/1245
[#1263]: https://github.com/oxyplot/oxyplot/issues/1263
[#1265]: https://github.com/oxyplot/oxyplot/issues/1265
[#1281]: https://github.com/oxyplot/oxyplot/issues/1281
[#1297]: https://github.com/oxyplot/oxyplot/issues/1297
[#1303]: https://github.com/oxyplot/oxyplot/issues/1303
[#1305]: https://github.com/oxyplot/oxyplot/issues/1305
[#1306]: https://github.com/oxyplot/oxyplot/issues/1306
[#1309]: https://github.com/oxyplot/oxyplot/issues/1309
[#1312]: https://github.com/oxyplot/oxyplot/issues/1312
[#1331]: https://github.com/oxyplot/oxyplot/issues/1331
[#1334]: https://github.com/oxyplot/oxyplot/issues/1334
[#1345]: https://github.com/oxyplot/oxyplot/issues/1345
[#1347]: https://github.com/oxyplot/oxyplot/issues/1347
[#1358]: https://github.com/oxyplot/oxyplot/issues/1358
[#1364]: https://github.com/oxyplot/oxyplot/issues/1364
[#1378]: https://github.com/oxyplot/oxyplot/issues/1378
[#1382]: https://github.com/oxyplot/oxyplot/issues/1382
[#1385]: https://github.com/oxyplot/oxyplot/issues/1385
[#1390]: https://github.com/oxyplot/oxyplot/issues/1390
[#1392]: https://github.com/oxyplot/oxyplot/issues/1392
[#1396]: https://github.com/oxyplot/oxyplot/issues/1396
[#1399]: https://github.com/oxyplot/oxyplot/issues/1399
[#1402]: https://github.com/oxyplot/oxyplot/issues/1402
[#1403]: https://github.com/oxyplot/oxyplot/issues/1403
[#1409]: https://github.com/oxyplot/oxyplot/issues/1409
[#1410]: https://github.com/oxyplot/oxyplot/issues/1410
[#1417]: https://github.com/oxyplot/oxyplot/issues/1417
[#1425]: https://github.com/oxyplot/oxyplot/issues/1425
[#1426]: https://github.com/oxyplot/oxyplot/issues/1426
[#1428]: https://github.com/oxyplot/oxyplot/issues/1428
[#1429]: https://github.com/oxyplot/oxyplot/issues/1429
[#1440]: https://github.com/oxyplot/oxyplot/issues/1440
[#1441]: https://github.com/oxyplot/oxyplot/issues/1441
[#1452]: https://github.com/oxyplot/oxyplot/issues/1452
[#1455]: https://github.com/oxyplot/oxyplot/issues/1455
[#1457]: https://github.com/oxyplot/oxyplot/issues/1457
[#1476]: https://github.com/oxyplot/oxyplot/issues/1476
[#1481]: https://github.com/oxyplot/oxyplot/issues/1481
[#1488]: https://github.com/oxyplot/oxyplot/issues/1488
[#1491]: https://github.com/oxyplot/oxyplot/issues/1491
[#1505]: https://github.com/oxyplot/oxyplot/issues/1505
[#1509]: https://github.com/oxyplot/oxyplot/issues/1509
[#1510]: https://github.com/oxyplot/oxyplot/issues/1510
[#1511]: https://github.com/oxyplot/oxyplot/issues/1511
[#1512]: https://github.com/oxyplot/oxyplot/issues/1512
[#1515]: https://github.com/oxyplot/oxyplot/issues/1515
[#1520]: https://github.com/oxyplot/oxyplot/issues/1520
[#1524]: https://github.com/oxyplot/oxyplot/issues/1524
[#1527]: https://github.com/oxyplot/oxyplot/issues/1527
[#1530]: https://github.com/oxyplot/oxyplot/issues/1530
[#1535]: https://github.com/oxyplot/oxyplot/issues/1535
[#1537]: https://github.com/oxyplot/oxyplot/issues/1537
[#1538]: https://github.com/oxyplot/oxyplot/issues/1538
[#1539]: https://github.com/oxyplot/oxyplot/issues/1539
[#1544]: https://github.com/oxyplot/oxyplot/issues/1544
[#1545]: https://github.com/oxyplot/oxyplot/issues/1545
[#1564]: https://github.com/oxyplot/oxyplot/issues/1564
[#1569]: https://github.com/oxyplot/oxyplot/issues/1569
[#1573]: https://github.com/oxyplot/oxyplot/issues/1573
[#1574]: https://github.com/oxyplot/oxyplot/issues/1574
[#1577]: https://github.com/oxyplot/oxyplot/issues/1577
[#1580]: https://github.com/oxyplot/oxyplot/issues/1580
[#1585]: https://github.com/oxyplot/oxyplot/issues/1585
[#1586]: https://github.com/oxyplot/oxyplot/issues/1586
[#1591]: https://github.com/oxyplot/oxyplot/issues/1591
[#1593]: https://github.com/oxyplot/oxyplot/issues/1593
[#1599]: https://github.com/oxyplot/oxyplot/issues/1599
[#1600]: https://github.com/oxyplot/oxyplot/issues/1600
[#1608]: https://github.com/oxyplot/oxyplot/issues/1608
[#1619]: https://github.com/oxyplot/oxyplot/issues/1619
[#1625]: https://github.com/oxyplot/oxyplot/issues/1625
[#1630]: https://github.com/oxyplot/oxyplot/issues/1630
[#1642]: https://github.com/oxyplot/oxyplot/issues/1642
[#1648]: https://github.com/oxyplot/oxyplot/issues/1648
[#1652]: https://github.com/oxyplot/oxyplot/issues/1652
[#1659]: https://github.com/oxyplot/oxyplot/issues/1659
[#1661]: https://github.com/oxyplot/oxyplot/issues/1661
[#1667]: https://github.com/oxyplot/oxyplot/issues/1667
[#1668]: https://github.com/oxyplot/oxyplot/issues/1668
[#1673]: https://github.com/oxyplot/oxyplot/issues/1673
[#1678]: https://github.com/oxyplot/oxyplot/issues/1678
[#1685]: https://github.com/oxyplot/oxyplot/issues/1685
[#1690]: https://github.com/oxyplot/oxyplot/issues/1690
[#1708]: https://github.com/oxyplot/oxyplot/issues/1708
[#1716]: https://github.com/oxyplot/oxyplot/issues/1716
[#1726]: https://github.com/oxyplot/oxyplot/issues/1726
[#1730]: https://github.com/oxyplot/oxyplot/issues/1730
[#1736]: https://github.com/oxyplot/oxyplot/issues/1736
[#1740]: https://github.com/oxyplot/oxyplot/issues/1740
[#1741]: https://github.com/oxyplot/oxyplot/issues/1741
[#1743]: https://github.com/oxyplot/oxyplot/issues/1743
[#1766]: https://github.com/oxyplot/oxyplot/issues/1766
[#1785]: https://github.com/oxyplot/oxyplot/issues/1785
[#1787]: https://github.com/oxyplot/oxyplot/issues/1787
[#1796]: https://github.com/oxyplot/oxyplot/issues/1796
