set EnableNuGetPackageRestore=true

REM === WPF ===
mkdir ..\Packages\OxyPlot.Wpf\lib
mkdir ..\Packages\OxyPlot.Wpf\lib\NET40
mkdir ..\Packages\OxyPlot.Wpf\lib\NET45
copy ..\Output\NET40\OxyPlot.??? ..\Packages\OxyPlot.Wpf\lib\NET40
copy ..\Output\NET40\OxyPlot.Wpf.??? ..\Packages\OxyPlot.Wpf\lib\NET40
copy ..\Output\NET40\OxyPlot.Xps.??? ..\Packages\OxyPlot.Wpf\lib\NET40
copy ..\Output\NET45\OxyPlot.??? ..\Packages\OxyPlot.Wpf\lib\NET45
copy ..\Output\NET45\OxyPlot.Wpf.??? ..\Packages\OxyPlot.Wpf\lib\NET45
copy ..\Output\NET45\OxyPlot.Xps.??? ..\Packages\OxyPlot.Wpf\lib\NET45

copy ..\license.txt ..\Packages\OxyPlot.Wpf
..\Tools\NuGet\NuGet.exe pack ..\Packages\OxyPlot.Wpf\OxyPlot.Wpf.nuspec -OutputDirectory ..\Packages > pack.log

REM === OpenXml ===
mkdir ..\Packages\OxyPlot.OpenXml\lib
mkdir ..\Packages\OxyPlot.OpenXml\lib\NET40
mkdir ..\Packages\OxyPlot.OpenXml\lib\NET45
copy ..\Output\NET40\OxyPlot.OpenXml.??? ..\Packages\OxyPlot.OpenXml\lib\NET40
copy ..\Output\NET45\OxyPlot.OpenXml.??? ..\Packages\OxyPlot.OpenXml\lib\NET45
..\Tools\NuGet\NuGet.exe pack ..\Packages\OxyPlot.OpenXml\OxyPlot.OpenXml.nuspec -OutputDirectory ..\Packages >> pack.log

REM === Pdf ===
mkdir ..\Packages\OxyPlot.Pdf\lib
mkdir ..\Packages\OxyPlot.Pdf\lib\NET40
mkdir ..\Packages\OxyPlot.Pdf\lib\NET45
copy ..\Output\NET40\OxyPlot.Pdf.??? ..\Packages\OxyPlot.Pdf\lib\NET40
copy ..\Output\NET40\PdfSharp.* ..\Packages\OxyPlot.Pdf\lib\NET40
copy ..\Output\NET40\MigraDoc.* ..\Packages\OxyPlot.Pdf\lib\NET40
copy ..\Output\NET45\OxyPlot.Pdf.??? ..\Packages\OxyPlot.Pdf\lib\NET45
copy ..\Output\NET45\PdfSharp.* ..\Packages\OxyPlot.Pdf\lib\NET45
copy ..\Output\NET45\MigraDoc.* ..\Packages\OxyPlot.Pdf\lib\NET45
..\Tools\NuGet\NuGet.exe pack ..\Packages\OxyPlot.Pdf\OxyPlot.Pdf.nuspec -OutputDirectory ..\Packages >> pack.log

REM === METRO ===
mkdir ..\Packages\OxyPlot.Metro\lib
mkdir ..\Packages\OxyPlot.Metro\lib\WINRT
mkdir ..\Packages\OxyPlot.Metro\lib\WINRT\Themes
copy ..\Output\WINRT\OxyPlot.??? ..\Packages\OxyPlot.Metro\lib\WINRT
copy ..\Output\WINRT\OxyPlot.Metro.??? ..\Packages\OxyPlot.Metro\lib\WINRT
copy ..\Output\WINRT\Themes\*.* ..\Packages\OxyPlot.Metro\lib\WINRT\Themes

copy ..\license.txt ..\Packages\OxyPlot.Metro
..\Tools\NuGet\NuGet.exe pack ..\Packages\OxyPlot.Metro\OxyPlot.Metro.nuspec -OutputDirectory ..\Packages >> pack.log

REM === SILVERLIGHT ===
mkdir ..\Packages\OxyPlot.Silverlight\lib
mkdir ..\Packages\OxyPlot.Silverlight\lib\SL4
mkdir ..\Packages\OxyPlot.Silverlight\lib\SL5
copy ..\Output\SL4\OxyPlot.??? ..\Packages\OxyPlot.Silverlight\lib\SL4
copy ..\Output\SL4\OxyPlot.Silverlight.??? ..\Packages\OxyPlot.Silverlight\lib\SL4
copy ..\Output\SL5\OxyPlot.??? ..\Packages\OxyPlot.Silverlight\lib\SL5
copy ..\Output\SL5\OxyPlot.Silverlight.??? ..\Packages\OxyPlot.Silverlight\lib\SL5
copy ..\license.txt ..\Packages\OxyPlot.Silverlight
..\Tools\NuGet\NuGet.exe pack ..\Packages\OxyPlot.Silverlight\OxyPlot.Silverlight.nuspec -OutputDirectory ..\Packages >> pack.log

REM === WINFORMS ===
mkdir ..\Packages\OxyPlot.WindowsForms\lib
mkdir ..\Packages\OxyPlot.WindowsForms\lib\NET40
mkdir ..\Packages\OxyPlot.WindowsForms\lib\NET45
copy ..\Output\NET40\OxyPlot.??? ..\Packages\OxyPlot.WindowsForms\lib\NET40
copy ..\Output\NET40\OxyPlot.WindowsForms.??? ..\Packages\OxyPlot.WindowsForms\lib\NET40
copy ..\Output\NET45\OxyPlot.??? ..\Packages\OxyPlot.WindowsForms\lib\NET45
copy ..\Output\NET45\OxyPlot.WindowsForms.??? ..\Packages\OxyPlot.WindowsForms\lib\NET45
copy ..\license.txt ..\Packages\OxyPlot.WindowsForms
..\Tools\NuGet\NuGet.exe pack ..\Packages\OxyPlot.WindowsForms\OxyPlot.WindowsForms.nuspec -OutputDirectory ..\Packages >> pack.log
