#!/bin/sh

NUGET=/usr/bin/nuget

rm -f ../Output/*.nupkg

# Create Xamarin.iOS NuGet package
$NUGET pack ../Source/OxyPlot.Xamarin.iOS/OxyPlot.Xamarin.iOS.nuspec -BasePath ../Source/OxyPlot.Xamarin.iOS/ -OutputDirectory ../Output

# Create Xamarin.Android NuGet package
$NUGET pack ../Source/OxyPlot.Xamarin.Android/OxyPlot.Xamarin.Android.nuspec -BasePath ../Source/OxyPlot.Xamarin.Android/ -OutputDirectory ../Output

ls -al ../Output/*.nupkg