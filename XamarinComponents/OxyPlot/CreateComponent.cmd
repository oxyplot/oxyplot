mkdir bin
copy ..\..\Output\PCL\OxyPlot.dll bin
copy ..\..\Output\XamarinIOS\OxyPlot.XamarinIOS.dll bin
copy ..\..\Output\XamarinAndroid\OxyPlot.XamarinAndroid.dll bin

mkdir icons
copy ..\..\Icons\OxyPlot_128.png icons\OxyPlot_128x128.png
copy ..\..\Icons\OxyPlot_256.png icons\OxyPlot_256x256.png
copy ..\..\Icons\OxyPlot_512.png icons\OxyPlot_512x512.png

..\..\Tools\Xamarin\xamarin-component.exe create-manually "..\OxyPlot-2014.1.xam" ^
    --name="OxyPlot" ^
    --summary="A cross-platform plotting library for .NET" ^
    --publisher="oxyplot.org" ^
    --website="http://oxyplot.org/" ^
    --details="Details.md" ^
    --license="License.md" ^
    --getting-started="GettingStarted.md" ^
    --icon="icons\OxyPlot_128x128.png" ^
    --icon="icons\OxyPlot_256x256.png" ^
    --icon="icons\OxyPlot_512x512.png" ^
    --library="ios":"bin/OxyPlot.dll" ^
    --library="ios":"bin/OxyPlot.XamarinIOS.dll" ^
    --library="android":"bin/OxyPlot.dll" ^
    --library="android":"bin/OxyPlot.XamarinAndroid.dll" ^
    --sample="iOS Sample. Demonstrates OxyPlot on iOS.":"samples/OxyPlotSample.iOS.sln" ^
    --sample="Android Sample. Demonstrates OxyPlot on Android":"samples/OxyPlotSample.Android.sln"

REM --monodoc=doc ^

pause