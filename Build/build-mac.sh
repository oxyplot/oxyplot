#!/bin/sh

# Folders
LynxToolkit=/usr/local/bin

# Tools
MDTOOL="/Applications/Xamarin Studio.app/Contents/MacOS/mdtool"

# Folders
SOURCE=../Source
OUTPUT=../Output

# Run the tool that updates the version numbers in all AssemblyInfo.cs files
mono "$LynxToolkit/UpdateVersionNumbers.exe" /VersionFromNuGet=OxyPlot.Core /Dependency=OxyPlot.Core /Directory=..
if [ $? -ne 0 ]; then 
	echo "  FAILED!"
fi

# Clean the output folder
rm -rf $OUTPUT

echo
echo "Building for Xamarin.Mac"
# Build OxyPlot. The output will be created in the $OUTPUT folder.
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.Xamarin.Mac.sln > build-mac.log
if [ $? -ne 0 ]; then 
	echo "  FAILED"
else 
	echo "  OK"
fi
ls -al ../Output/Xamarin.Mac/OxyPlot*
# mate build-mac.log
