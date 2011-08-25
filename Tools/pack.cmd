cd ..\Packages
copy ..\source\OxyPlot.Wpf\bin\Release\*.dll OxyPlot.Wpf\lib
copy ..\source\OxyPlot.Wpf\bin\Release\*.xml OxyPlot.Wpf\lib
..\tools\nuget.exe pack OxyPlot.Wpf\OxyPlot.Wpf.nuspec

copy ..\source\OxyPlot.Silverlight\bin\Release\*.dll OxyPlot.Silverlight\lib
copy ..\source\OxyPlot.Silverlight\bin\Release\*.xml OxyPlot.Silverlight\lib
..\tools\nuget.exe pack OxyPlot.Silverlight\OxyPlot.Silverlight.nuspec
cd ..\Tools