#!/bin/sh

# Create XamarinIOS NuGet package
mono /usr/local/bin/NuGet.exe pack ../Source/OxyPlot.XamarinIOS/OxyPlot.XamarinIOS.nuspec -OutputDirectory ../Output > pack.log

# Create XamarinAndroid NuGet package
mono /usr/local/bin/NuGet.exe pack ../Source/OxyPlot.XamarinAndroid/OxyPlot.XamarinAndroid.nuspec -OutputDirectory ../Output >> pack.log