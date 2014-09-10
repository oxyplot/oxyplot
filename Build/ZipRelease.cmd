mkdir ..\Output\Release
mkdir ..\Output\Release\NET40
mkdir ..\Output\Release\NET45
mkdir ..\Output\Release\NetCore45
mkdir ..\Output\Release\NetCore45\Themes
mkdir ..\Output\Release\SL5
mkdir ..\Output\Release\WP8

copy ..\Output\NET45\OxyPlot.??? ..\Output\Release

copy ..\Output\NET40\OxyPlot.WindowsForms.??? ..\Output\Release\NET40
copy ..\Output\NET45\OxyPlot.WindowsForms.??? ..\Output\Release\NET45

copy ..\Output\NET40\OxyPlot.WPF.??? ..\Output\Release\NET40
copy ..\Output\NET45\OxyPlot.WPF.??? ..\Output\Release\NET45

copy ..\Output\NET40\OxyPlot.Xps.??? ..\Output\Release\NET40
copy ..\Output\NET45\OxyPlot.Xps.??? ..\Output\Release\NET45

copy ..\Output\SL5\OxyPlot.Silverlight.??? ..\Output\Release\SL5

copy ..\Output\WP8\OxyPlot.WP8.??? ..\Output\Release\WP8

copy ..\Output\NetCore45\OxyPlot.Metro.??? ..\Output\Release\NetCore45
copy ..\Output\NetCore45\Themes\*.* ..\Output\Release\NetCore45\Themes

"C:\Program Files\7-Zip\7z.exe" a -r ..\Output\OxyPlot-%1.zip ..\Output\Release\*.* > ZipRelease.log