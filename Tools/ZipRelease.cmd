set dir=..\Output\Examples
del /S /Q %dir%\*.pdb 
del /S /Q %dir%\*.vshost.exe 
del /S /Q %dir%\*.manifest 
del /S /Q %dir%\*.config
del /S %dir%\oxyplot.xml
del /S %dir%\oxyplot.wpf.xml
del /S %dir%\oxyplot.silverlight.xml
del /S %dir%\oxyplot.windowsforms.xml
"C:\Program Files\7-Zip\7z.exe" a -r ..\Output\OxyPlot-%1.zip ..\Output\*.* > ZipRelease.log