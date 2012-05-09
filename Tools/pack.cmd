REM === WPF ===
mkdir ..\Packages\OxyPlot.Wpf\lib
copy ..\Output\OxyPlot.??? ..\Packages\OxyPlot.Wpf\lib
copy ..\Output\OxyPlot.Wpf.??? ..\Packages\OxyPlot.Wpf\lib
copy ..\Output\OxyPlot.Xps.??? ..\Packages\OxyPlot.Wpf\lib

copy ..\license.txt ..\Packages\OxyPlot.Wpf
nuget.exe pack ..\Packages\OxyPlot.Wpf\OxyPlot.Wpf.nuspec -OutputDirectory ..\Packages > pack.log

REM === OpenXml ===
mkdir ..\Packages\OxyPlot.OpenXml\lib
copy ..\Output\OxyPlot.OpenXml.??? ..\Packages\OxyPlot.OpenXml\lib
nuget.exe pack ..\Packages\OxyPlot.OpenXml\OxyPlot.OpenXml.nuspec -OutputDirectory ..\Packages >> pack.log

REM === Pdf ===
mkdir ..\Packages\OxyPlot.Pdf\lib
copy ..\Output\OxyPlot.Pdf.??? ..\Packages\OxyPlot.Pdf\lib
copy ..\Output\PdfSharp.* ..\Packages\OxyPlot.Pdf\lib
copy ..\Output\MigraDoc.* ..\Packages\OxyPlot.Pdf\lib
nuget.exe pack ..\Packages\OxyPlot.Pdf\OxyPlot.Pdf.nuspec -OutputDirectory ..\Packages >> pack.log

REM === SILVERLIGHT ===
mkdir ..\Packages\OxyPlot.Silverlight\lib
copy ..\Output\OxyPlotSL.??? ..\Packages\OxyPlot.Silverlight\lib
copy ..\Output\OxyPlot.Silverlight.??? ..\Packages\OxyPlot.Silverlight\lib
copy ..\license.txt ..\Packages\OxyPlot.Silverlight
nuget.exe pack ..\Packages\OxyPlot.Silverlight\OxyPlot.Silverlight.nuspec -OutputDirectory ..\Packages >> pack.log

REM === WINFORMS ===
mkdir ..\Packages\OxyPlot.WindowsForms\lib
copy ..\Output\OxyPlot.??? ..\Packages\OxyPlot.WindowsForms\lib
copy ..\Output\OxyPlot.WindowsForms.??? ..\Packages\OxyPlot.WindowsForms\lib
copy ..\license.txt ..\Packages\OxyPlot.WindowsForms
nuget.exe pack ..\Packages\OxyPlot.WindowsForms\OxyPlot.WindowsForms.nuspec -OutputDirectory ..\Packages >> pack.log
