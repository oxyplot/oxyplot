cd ..\Packages
copy ..\Output\OxyPlot.dll OxyPlot.Wpf\lib
copy ..\Output\OxyPlot.xml OxyPlot.Wpf\lib
copy ..\Output\OxyPlot.Wpf.dll OxyPlot.Wpf\lib
copy ..\Output\OxyPlot.Wpf.xml OxyPlot.Wpf\lib
..\tools\nuget.exe pack OxyPlot.Wpf\OxyPlot.Wpf.nuspec

copy ..\Output\OxyPlotSL.dll OxyPlot.Silverlight\lib
copy ..\Output\OxyPlotSL.xml OxyPlot.Silverlight\lib
copy ..\Output\OxyPlot.Silverlight.dll OxyPlot.Silverlight\lib
copy ..\Output\OxyPlot.Silverlight.xml OxyPlot.Silverlight\lib
..\tools\nuget.exe pack OxyPlot.Silverlight\OxyPlot.Silverlight.nuspec
cd ..\Tools