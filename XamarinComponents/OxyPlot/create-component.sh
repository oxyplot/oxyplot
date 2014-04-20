#!/bin/sh

mkdir bin
cp ../../Output/PCL/OxyPlot.dll bin
cp ../../Output/XamarinIOS/OxyPlot.XamarinIOS.dll bin
cp ../../Output/XamarinAndroid/OxyPlot.XamarinAndroid.dll bin

mkdir icons
cp ../../Icons/OxyPlot_128.png icons/OxyPlot_128x128.png
cp ../../Icons/OxyPlot_256.png icons/OxyPlot_256x256.png
cp ../../Icons/OxyPlot_512.png icons/OxyPlot_512x512.png

mono ../../Tools/Xamarin/xamarin-component.exe create-manually "../OxyPlot-2014.1.xam" \
    --name="OxyPlot" \
    --publisher="oxyplot.org" \
    --website="http://oxyplot.org/" \
    --monodoc="doc"
    --srcurl="http://hg.codeplex.com/oxyplot"
    --summary="A cross-platform plotting library for .NET" \
    --screenshot="OxyPlot running on Xamarin.iOS":"Screenshot_700x400.png"\
    --popover="Popover_320x200.png"
    --details="Details.md" \
    --license="License.md" \
    --getting-started="GettingStarted.md" \
    --icon="icons/OxyPlot_128x128.png" \
    --icon="icons/OxyPlot_256x256.png" \
    --icon="icons/OxyPlot_512x512.png" \
    --library="ios":"bin/OxyPlot.dll" \
    --library="ios":"bin/OxyPlot.XamarinIOS.dll" \
    --library="android":"bin/OxyPlot.dll" \
    --library="android":"bin/OxyPlot.XamarinAndroid.dll" \
    --sample="iOS Sample. Demonstrates OxyPlot on iOS.":"samples/OxyPlotSample.iOS.sln" \
    --sample="Android Sample. Demonstrates OxyPlot on Android":"samples/OxyPlotSample.Android.sln"
