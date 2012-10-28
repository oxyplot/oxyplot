call clean ..\Output\NET40\Examples
call clean ..\Output\NET45\Examples

"C:\Program Files\7-Zip\7z.exe" a ..\Output\OxyPlot-NET40-%1.zip ..\Output\NET40\*.* > ZipRelease.log
"C:\Program Files\7-Zip\7z.exe" a ..\Output\OxyPlot-NET45-%1.zip ..\Output\NET45\*.* >> ZipRelease.log
"C:\Program Files\7-Zip\7z.exe" a ..\Output\OxyPlot-SL4-%1.zip ..\Output\SL4\*.* >> ZipRelease.log
"C:\Program Files\7-Zip\7z.exe" a ..\Output\OxyPlot-SL5-%1.zip ..\Output\SL5\*.* >> ZipRelease.log
"C:\Program Files\7-Zip\7z.exe" a -r ..\Output\OxyPlot-NET40-Examples-%1.zip ..\Output\NET40\Examples\*.* >> ZipRelease.log
"C:\Program Files\7-Zip\7z.exe" a -r ..\Output\OxyPlot-NET45-Examples-%1.zip ..\Output\NET45\Examples\*.* >> ZipRelease.log