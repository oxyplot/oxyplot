#!/bin/sh

# Tools
MDTOOL="/Applications/Xamarin Studio.app/Contents/MacOS/mdtool"
UVNTOOL="../../Tools/Lynx/UpdateVersionNumbers.exe"

# Folders
SOURCE=../../Source
OUTPUT=../../Output

echo "Updating version numbers"
# Run the tool that updates the version numbers in all AssemblyInfo.cs files
mono $UVNTOOL /VersionFromNuGet=OxyPlot.Core /Directory=$SOURCE > build-update.log
if [ $? -ne 0 ]; then 
	echo "  FAILED!"
fi

# Clean the output folder
rm -rf $OUTPUT

echo "Building libraries"
# Build OxyPlot. The output will be created in the $OUTPUT folder.
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.XamarinIOS.sln > build-ios.log
if [ $? -ne 0 ]; then 
	echo "  iOS: failed"
else 
	echo "  iOS: OK"
fi
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.XamarinAndroid.sln > build-android.log
if [ $? -ne 0 ]; then 
	echo "  Android: failed"
else 
	echo "  Android: OK"
fi

# Copy the relevant output files to the bin/ folder
if [ ! -d "bin" ]; then
	mkdir bin
fi
cp $OUTPUT/PCL/OxyPlot.dll bin
cp $OUTPUT/XamarinIOS/OxyPlot.XamarinIOS.dll bin
cp $OUTPUT/XamarinAndroid/OxyPlot.XamarinAndroid.dll bin

open build-update.log
open build-ios.log
open build-android.log