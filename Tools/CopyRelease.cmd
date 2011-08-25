cd ..\Source
mkdir ..\Output
mkdir ..\Output\ReleaseWPF
copy ..\Source\OxyPlot.Wpf\bin\Release\*.dll ..\Output\ReleaseWPF
cd ..\Output\ReleaseWPF
7z.exe a -r ..\OxyPlot-WPF-Release.zip *.*
explorer ..
cd ..\..\Tools