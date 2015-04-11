#!/bin/sh

NUGET=/usr/local/bin/NuGet.exe

rm -f ../Output/*.nupkg

# Create Xamarin.Mac NuGet package
mono $NUGET pack ../Source/OxyPlot.Xamarin.Mac/OxyPlot.Xamarin.Mac.nuspec -OutputDirectory ../Output

ls -al ../Output/*.nupkg