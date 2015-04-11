#!/bin/sh

NUGET=/usr/local/bin/NuGet.exe

rm -f ../Output/*.nupkg

# Create Xamarin.iOS NuGet package
mono $NUGET pack ../Source/OxyPlot.Xamarin.iOS/OxyPlot.Xamarin.iOS.nuspec -OutputDirectory ../Output

# Create Xamarin.Android NuGet package
mono $NUGET pack ../Source/OxyPlot.Xamarin.Android/OxyPlot.Xamarin.Android.nuspec -OutputDirectory ../Output

ls -al ../Output/*.nupkg