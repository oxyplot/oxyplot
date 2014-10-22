#!/bin/sh

NUGET=/usr/local/bin/NuGet.exe

rm -f ../Output/*.nupkg

cp ../README.md ../Output
cp ../LICENSE ../Output
cp ../AUTHORS ../Output
cp ../CONTRIBUTORS ../Output
mkdir ../Output/lib
mkdir ../Output/lib/Xamarin.Mac

cp ../Output/Xamarin.Mac/OxyPlot.Xamarin.Mac.??? ../Output/lib/Xamarin.Mac

cp ../Source/OxyPlot.Xamarin.Mac/*.nuspec ../Output

# Create Xamarin.Mac NuGet package
mono $NUGET pack ../Output/OxyPlot.Xamarin.Mac.nuspec -OutputDirectory ../Output

ls -al ../Output/*.nupkg