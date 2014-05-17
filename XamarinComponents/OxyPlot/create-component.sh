#!/bin/sh

# Folders
OUTPUT=../../Output
ICONS=../../Icons

echo Removing old files
rm -v bin/*
rm -v icons/*
rm -v $OUTPUT/*.xam

echo "\nCopying binaries"
if [ ! -d "bin" ]; then
  mkdir bin
fi
cp -v $OUTPUT/PCL/OxyPlot.dll bin
cp -v $OUTPUT/XamarinIOS/OxyPlot.XamarinIOS.dll bin
cp -v $OUTPUT/XamarinAndroid/OxyPlot.XamarinAndroid.dll bin

echo "\nCopying icons"
if [ ! -d "icons" ]; then
  mkdir -v icons
fi
cp -v $ICONS/OxyPlot_128.png icons/OxyPlot_128x128.png
cp -v $ICONS/OxyPlot_256.png icons/OxyPlot_256x256.png
cp -v $ICONS/OxyPlot_512.png icons/OxyPlot_512x512.png

VERSION=${VERSION:=2014.1.0}

OUTPUTPACKAGE=$OUTPUT/OxyPlot-$VERSION.xam

echo "\nCreating Xamarin Component: $OUTPUTPACKAGE"
mono ../../Tools/Xamarin/xamarin-component.exe create-manually "$OUTPUTPACKAGE" \
    --name="OxyPlot" \
    --publisher="oxyplot.org" \
    --website="http://oxyplot.org/" \
    --monodoc="doc" \
    --srcurl="http://hg.codeplex.com/oxyplot" \
    --summary="A cross-platform plotting library for .NET" \
    --screenshot="OxyPlot running on Xamarin.iOS":"content/Screenshot_700x400.png"\
    --popover="content/Popover_320x200.png" \
    --details="content/Details.md" \
    --license="content/License.md" \
    --getting-started="content/GettingStarted.md" \
    --icon="icons/OxyPlot_128x128.png" \
    --icon="icons/OxyPlot_256x256.png" \
    --icon="icons/OxyPlot_512x512.png" \
    --library="ios":"bin/OxyPlot.dll" \
    --library="ios":"bin/OxyPlot.XamarinIOS.dll" \
    --library="android":"bin/OxyPlot.dll" \
    --library="android":"bin/OxyPlot.XamarinAndroid.dll" \
    --sample="iOS Sample. Demonstrates how to create a view with a LineSeries on iOS.":"samples/OxyPlotSample.iOS.sln" \
    --sample="Android Sample. Demonstrates how to create a view with a LineSeries on Android. ":"samples/OxyPlotSample.Android.sln"
