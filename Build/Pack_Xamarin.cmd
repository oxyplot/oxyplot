copy ..\README.md ..\Output
copy ..\LICENSE ..\Output
copy ..\AUTHORS ..\Output
copy ..\CONTRIBUTORS ..\Output

mkdir ..\Output\lib
mkdir ..\Output\lib\MonoTouch
mkdir ..\Output\lib\MonoAndroid
mkdir ..\Output\lib\Xamarin.iOS10

copy ..\Output\MonoTouch\OxyPlot.MonoTouch.??? ..\Output\lib\MonoTouch
copy ..\Output\Xamarin.Android\OxyPlot.Xamarin.Android.??? ..\Output\lib\MonoAndroid
copy ..\Output\Xamarin.iOS\OxyPlot.Xamarin.iOS.??? ..\Output\lib\Xamarin.iOS10

copy ..\Source\OxyPlot.Xamarin.iOS\*.nuspec ..\Output
copy ..\Source\OxyPlot.Xamarin.Android\*.nuspec ..\Output

nuget pack ..\Output\OxyPlot.Xamarin.iOS.nuspec -OutputDirectory ..\Output

nuget pack ..\Output\OxyPlot.Xamarin.Android.nuspec -OutputDirectory ..\Output

nuget pack ..\Source\OxyPlot.Xamarin.Forms/OxyPlot.Xamarin.Forms.nuspec -OutputDirectory ..\Output
pause