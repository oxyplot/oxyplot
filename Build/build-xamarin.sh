#!/bin/sh

# Folders
if [ -z "${LYNXTOOLKIT}" ]; then
    LynxToolkit=/usr/local/bin
else
    LynxToolkit=$LYNXTOOLKIT
fi
SOURCE=../Source
OUTPUT=../Output

# Tools
MDTOOL="/Applications/Xamarin Studio.app/Contents/MacOS/mdtool"
NUGET="/usr/bin/nuget"

# Clean the output folder
rm -rf $OUTPUT


# Run the tool that updates the version numbers in all AssemblyInfo.cs files
echo "Updating version numbers..."
mono "$LynxToolkit/UpdateVersionNumbers.exe" /VersionFromNuGet=OxyPlot.Core /Dependency=OxyPlot.Core /ExtractReleaseNotes=../CHANGELOG.md /Directory=..
if [ $? -ne 0 ]; then echo "  FAILED"; else echo "  OK"; fi


# Start the build. The output will be created in the $OUTPUT folder.

# MonoTouch
echo "Building for MonoTouch..."
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.MonoTouch.sln > build-monotouch.log
if [ $? -ne 0 ]; then echo "  FAILED"; else echo "  OK"; fi
ls -al ../Output/MonoTouch/OxyPlot*

# Xamarin.iOS
echo "Building for Xamarin.iOS..."
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.Xamarin.iOS.sln > build-ios.log
if [ $? -ne 0 ]; then echo "  FAILED"; else echo "  OK"; fi
ls -al ../Output/Xamarin.iOS/OxyPlot*

# Xamarin.Android
echo "Building for Xamarin.Android..."
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.Xamarin.Android.sln > build-android.log
if [ $? -ne 0 ]; then echo "  FAILED"; else echo "  OK"; fi
ls -al ../Output/Xamarin.Android/OxyPlot*

# Xamarin.Mac
echo "Building for Xamarin.Mac..."
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.Xamarin.Mac.sln > build-mac.log
if [ $? -ne 0 ]; then echo "  FAILED"; else echo "  OK"; fi
ls -al ../Output/Xamarin.Mac/OxyPlot*


# Start the packaging
$NUGET pack ../Source/OxyPlot.Xamarin.iOS/OxyPlot.Xamarin.iOS.nuspec -OutputDirectory ../Output
$NUGET pack ../Source/OxyPlot.Xamarin.Android/OxyPlot.Xamarin.Android.nuspec -OutputDirectory ../Output
$NUGET pack ../Source/OxyPlot.Xamarin.Mac/OxyPlot.Xamarin.Mac.nuspec -OutputDirectory ../Output
ls -al ../Output/*.nupkg
