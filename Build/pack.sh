#!/bin/sh

NUGET=/usr/local/bin/NuGet.exe

rm -f ../Output/*.nupkg

cp ../README.md ../Output
cp ../LICENSE ../Output
cp ../AUTHORS ../Output
cp ../CONTRIBUTORS ../Output
cp ../README ../Output
mkdir ../Output/lib
mkdir ../Output/lib/MonoTouch
mkdir ../Output/lib/MonoAndroid

cp ../Output/XamarinIOS/OxyPlot.XamarinIOS.??? ../Output/lib/MonoTouch
cp ../Output/XamarinAndroid/OxyPlot.XamarinAndroid.??? ../Output/lib/MonoAndroid

cp ../Source/OxyPlot.XamarinIOS/*.nuspec ../Output
cp ../Source/OxyPlot.XamarinAndroid/*.nuspec ../Output

# Create XamarinIOS NuGet package
mono $NUGET pack ../Output/OxyPlot.XamarinIOS.nuspec -OutputDirectory ../Output

# Create XamarinAndroid NuGet package
mono $NUGET pack ../Output/OxyPlot.XamarinAndroid.nuspec -OutputDirectory ../Output

ls -al ../Output/*.nupkg