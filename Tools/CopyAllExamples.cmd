cd ..\Source\Examples\WPF
mkdir ..\..\..\Output
mkdir ..\..\..\Output\Examples
mkdir ..\..\..\Output\Examples\WPF
del /S /Q ..\..\..\Output\Examples\WPF\*.*
for /D %%G in (*) DO (
mkdir ..\..\..\Output\Examples\WPF\%%G
xcopy %%G\bin\Release\*.* ..\..\..\Output\Examples\WPF\%%G /S /Y
)
cd ..\..\..\Output\Examples
del /S *.pdb 
del /S *.vshost.exe 
del /S *.manifest 
del /S *.config
del /S oxyplot.xml
del /S oxyplot.wpf.xml
"C:\Program Files\7-Zip\7z.exe" a -r ..\OxyPlot-Examples.zip *.*
explorer ..
cd ..\..\..\Tools