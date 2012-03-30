mkdir ..\Packages\OxyPlot.Wpf\lib
copy ..\Output\OxyPlot.dll ..\Packages\OxyPlot.Wpf\lib
copy ..\Output\OxyPlot.xml ..\Packages\OxyPlot.Wpf\lib
copy ..\Output\OxyPlot.Wpf.dll ..\Packages\OxyPlot.Wpf\lib
copy ..\Output\OxyPlot.Wpf.xml ..\Packages\OxyPlot.Wpf\lib
nuget.exe pack ..\Packages\OxyPlot.Wpf\OxyPlot.Wpf.nuspec -OutputDirectory ..\Packages > pack.log

mkdir ..\Packages\OxyPlot.Silverlight\lib
copy ..\Output\OxyPlotSL.dll ..\Packages\OxyPlot.Silverlight\lib
copy ..\Output\OxyPlotSL.xml ..\Packages\OxyPlot.Silverlight\lib
copy ..\Output\OxyPlot.Silverlight.dll ..\Packages\OxyPlot.Silverlight\lib
copy ..\Output\OxyPlot.Silverlight.xml ..\Packages\OxyPlot.Silverlight\lib
nuget.exe pack ..\Packages\OxyPlot.Silverlight\OxyPlot.Silverlight.nuspec -OutputDirectory ..\Packages >> pack.log

mkdir ..\Packages\OxyPlot.WindowsForms\lib
copy ..\Output\OxyPlot.dll ..\Packages\OxyPlot.WindowsForms\lib
copy ..\Output\OxyPlot.xml ..\Packages\OxyPlot.WindowsForms\lib
copy ..\Output\OxyPlot.WindowsForms.dll ..\Packages\OxyPlot.WindowsForms\lib
copy ..\Output\OxyPlot.WindowsForms.xml ..\Packages\OxyPlot.WindowsForms\lib
nuget.exe pack ..\Packages\OxyPlot.WindowsForms\OxyPlot.WindowsForms.nuspec -OutputDirectory ..\Packages >> pack.log
