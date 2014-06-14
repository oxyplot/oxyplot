#!/bin/sh

# Create XamarinIOS NuGet package
mkdir ../Packages/OxyPlot.XamarinIOS/lib
mkdir ../Packages/OxyPlot.XamarinIOS/lib/MonoTouch
cp ../Output/XamarinIOS/OxyPlot.XamarinIOS.??? ../Packages/OxyPlot.XamarinIOS/lib/MonoTouch
cp ../LICENSE ../Packages/OxyPlot.XamarinIOS
mono ../Source/.nuget/NuGet.exe pack ../Packages/OxyPlot.XamarinIOS/OxyPlot.XamarinIOS.nuspec -OutputDirectory ../Output > pack.log

# Create XamarinAndroid NuGet package
mkdir ../Packages/OxyPlot.XamarinAndroid/lib
mkdir ../Packages/OxyPlot.XamarinAndroid/lib/MonoAndroid
cp ../Output/XamarinAndroid/OxyPlot.XamarinAndroid.??? ../Packages/OxyPlot.XamarinAndroid/lib/MonoAndroid
cp ../LICENSE ../Packages/OxyPlot.XamarinAndroid
mono ../Source/.nuget/NuGet.exe pack ../Packages/OxyPlot.XamarinAndroid/OxyPlot.XamarinAndroid.nuspec -OutputDirectory ../Output >> pack.log