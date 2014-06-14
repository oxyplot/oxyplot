#!/bin/sh

# Clean old output files
rm -rf monodoc
rm -rf htmldoc
rm -rf msxdoc

# Path to the mdoc tool
MDOC=../../Tools/Xamarin/mdoc.exe

# The path to the OxyPlot output files (dll/xml)
OUTPUT=../../Output

# Define the directories for the MonoTouch.dll and Mono.Android.dll assemblies.
# These are needed to generate documentation for OxyPlot.XamarinIOS and OxyPlot.XamarinAndroid@
# and must be specified for mdoc.
# The directories where found by looking at the properties of the references in Xamarin Studio...
MONOTOUCHDIR=/Developer/MonoTouch/usr/lib/mono/2.1
MONOANDROIDDIR=/Library/Frameworks/Xamarin.Android.framework/Versions/Current/lib/mandroid/platforms/android-8

# Create or update documentation from the OxyPlot assembly.
echo "Update doc xml files"
mono $MDOC update -o doc \
	-L $MONOTOUCHDIR \
	-L $MONOANDROIDDIR \
	-i $OUTPUT/PCL/OxyPlot.xml $OUTPUT/PCL/OxyPlot.dll \
	-i $OUTPUT/XamarinIOS/OxyPlot.XamarinIOS.xml $OUTPUT/XamarinIOS/OxyPlot.XamarinIOS.dll \
	-i $OUTPUT/XamarinAndroid/OxyPlot.XamarinAndroid.xml $OUTPUT/XamarinAndroid/OxyPlot.XamarinAndroid.dll \
	> doc-update.log
if [ $? -ne 0 ]; then 
	echo "  FAILED!"
fi

# Export mdoc documentation to HTML.
echo "Export to html (this takes a long time)"
mono $MDOC export-html -o htmldoc doc > doc-export.log
if [ $? -ne 0 ]; then 
	echo "  FAILED!"
fi

# Assemble documentation for use within the monodoc browser (ecma format).
echo "Assemble for monodoc"
mono $MDOC assemble -o doc/OxyPlot doc > doc-assemble.log
if [ $? -ne 0 ]; then 
	echo "  FAILED!"
fi

# Export into Microsoft XML Documentation format files.
echo "Export msxdoc"
mkdir msxdoc
mono $MDOC export-msxdoc doc -o msxdoc/OxyPlot.xml > doc-exportmsxdoc.log
if [ $? -ne 0 ]; then 
	echo "  FAILED!"
fi

# open doc-update.log
# open doc-export.log
# open doc-assemble.log
