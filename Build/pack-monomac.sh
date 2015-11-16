#!/bin/sh

NUGET=/usr/bin/nuget

rm -f ../Output/*.nupkg

# Create MonoMac NuGet package
$NUGET pack ../Source/OxyPlot.MonoMac/OxyPlot.MonoMac.nuspec -BasePath ../Source/OxyPlot.MonoMac/ -OutputDirectory ../Output

ls -al ../Output/*.nupkg
