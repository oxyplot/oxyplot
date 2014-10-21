copy ..\README.md ..\Output
copy ..\LICENSE ..\Output
copy ..\AUTHORS ..\Output
copy ..\CONTRIBUTORS ..\Output

mkdir ..\Output\lib
mkdir ..\Output\lib\MonoTouch
mkdir ..\Output\lib\MonoAndroid
mkdir ..\Output\lib\Xamarin.iOS10

copy ..\Output\XamarinIOS\OxyPlot.XamarinIOS.??? ..\Output\lib\MonoTouch
copy ..\Output\XamarinAndroid\OxyPlot.XamarinAndroid.??? ..\Output\lib\MonoAndroid
copy ..\Output\Xamarin.iOS\OxyPlot.Xamarin.iOS.??? ..\Output\lib\Xamarin.iOS10

copy ..\Source\OxyPlot.XamarinIOS\*.nuspec ..\Output
copy ..\Source\OxyPlot.XamarinAndroid\*.nuspec ..\Output

nuget pack ..\Output\OxyPlot.XamarinIOS.nuspec -OutputDirectory ..\Output

nuget pack ..\Output\OxyPlot.XamarinAndroid.nuspec -OutputDirectory ..\Output

nuget pack ..\Source\OxyPlot.XamarinForms/OxyPlot.XamarinForms.nuspec -OutputDirectory ..\Output
pause