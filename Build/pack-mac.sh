#!/bin/sh

NUGET=/usr/bin/nuget

rm -f ../Output/*.nupkg

# Create Xamarin.Mac NuGet package
$NUGET pack ../Source/OxyPlot.Xamarin.Mac/OxyPlot.Xamarin.Mac.nuspec -BasePath ../Source/OxyPlot.Xamarin.Mac/ -OutputDirectory ../Output

ls -al ../Output/*.nupkg