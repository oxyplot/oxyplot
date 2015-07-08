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
mono "$LynxToolkit/UpdateVersionNumbers.exe" /VersionFromNuGet=OxyPlot.Core /Dependency=OxyPlot.Core /ExtractReleaseNotes=../CHANGELOG.md /Directory=..
if [ $? -ne 0 ]; then 
	echo "  FAILED!"
fi

# Clean the output folder
rm -rf $OUTPUT

echo
echo "Building for Xamarin.iOS"
# Build OxyPlot. The output will be created in the $OUTPUT folder.
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.Xamarin.iOS.sln > build-ios.log
if [ $? -ne 0 ]; then 
	echo "  FAILED"
else 
	echo "  OK"
fi
ls -al ../Output/Xamarin.iOS/OxyPlot*

echo
echo "Building for MonoTouch"
# Build OxyPlot. The output will be created in the $OUTPUT folder.
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.MonoTouch.sln > build-monotouch.log
if [ $? -ne 0 ]; then
echo "  FAILED"
else
echo "  OK"
fi
ls -al ../Output/MonoTouch/OxyPlot*

echo
echo "Building for Xamarin.Android"
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.Xamarin.Android.sln > build-android.log
if [ $? -ne 0 ]; then 
	echo "  FAILED"
else 
	echo "  OK"
fi

ls -al ../Output/Xamarin.Android/OxyPlot*
