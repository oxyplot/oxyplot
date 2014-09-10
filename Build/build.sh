#!/bin/sh

# Folders
LynxToolkit=/usr/local/bin

# Tools
MDTOOL="/Applications/Xamarin Studio.app/Contents/MacOS/mdtool"

# Folders
SOURCE=../Source
OUTPUT=../Output

# VERSION=${VERSION:=2014.1.308}

# Run the tool that updates the version numbers in all AssemblyInfo.cs files
mono "$LynxToolkit/UpdateVersionNumbers.exe" /VersionFromNuGet=OxyPlot.Core /Dependency=OxyPlot.Core /Directory=..
if [ $? -ne 0 ]; then 
	echo "  FAILED!"
fi

# Clean the output folder
rm -rf $OUTPUT

echo
echo "Building for Xamarin.iOS"
# Build OxyPlot. The output will be created in the $OUTPUT folder.
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.XamarinIOS.sln > build-ios.log
if [ $? -ne 0 ]; then 
	echo "  FAILED"
else 
	echo "  OK"
fi
ls -al ../Output/XamarinIOS/OxyPlot*
mate build-ios.log

echo
echo "Building for Xamarin.Android"
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.XamarinAndroid.sln > build-android.log
if [ $? -ne 0 ]; then 
	echo "  FAILED"
else 
	echo "  OK"
fi

ls -al ../Output/XamarinAndroid/OxyPlot*
mate build-android.log