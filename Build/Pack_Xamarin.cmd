copy ..\README.md ..\Output
copy ..\LICENSE ..\Output
copy ..\AUTHORS ..\Output
copy ..\CONTRIBUTORS ..\Output
copy ..\README ..\Output
mkdir ..\Output\lib
mkdir ..\Output\lib\MonoTouch
mkdir ..\Output\lib\MonoAndroid

copy ..\Output\XamarinIOS\OxyPlot.XamarinIOS.??? ..\Output\lib\MonoTouch
copy ..\Output\XamarinAndroid\OxyPlot.XamarinAndroid.??? ..\Output\lib\MonoAndroid

copy ..\Source\OxyPlot.XamarinIOS\*.nuspec ..\Output
copy ..\Source\OxyPlot.XamarinAndroid\*.nuspec ..\Output

nuget pack ..\Output\OxyPlot.XamarinIOS.nuspec -OutputDirectory ..\Output

nuget pack ..\Output\OxyPlot.XamarinAndroid.nuspec -OutputDirectory ..\Output

nuget pack ..\Source\OxyPlot.XamarinForms/OxyPlot.XamarinForms.nuspec -OutputDirectory ../Output
