cd ..\Output\Examples
del /S *.pdb 
del /S *.vshost.exe 
del /S *.manifest 
del /S *.config
del /S oxyplot.xml
del /S oxyplot.wpf.xml
"C:\Program Files\7-Zip\7z.exe" a -r ..\OxyPlot-Examples.zip *.*
explorer ..
cd ..\Tools