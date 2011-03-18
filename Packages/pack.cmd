cd OxyPlot.Wpf
copy ..\..\source\OxyPlot.Wpf\bin\Release\*.dll lib
..\..\tools\nuget.exe pack OxyPlot.Wpf.nuspec
cd ..\OxyPlot.Silverlight
copy ..\..\source\OxyPlot.Silverlight\bin\Release\*.dll lib
..\..\tools\nuget.exe pack OxyPlot.Silverlight.nuspec
cd ..