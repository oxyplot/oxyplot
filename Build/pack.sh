#!/bin/sh

NUGET=/usr/local/bin/NuGet.exe

# Create XamarinIOS NuGet package
mono $NUGET pack ../Source/OxyPlot.XamarinIOS/OxyPlot.XamarinIOS.nuspec -OutputDirectory ../Output > pack.log

# Create XamarinAndroid NuGet package
mono $NUGET pack ../Source/OxyPlot.XamarinAndroid/OxyPlot.XamarinAndroid.nuspec -OutputDirectory ../Output >> pack.log

ls -al ../Output/*.nupkg