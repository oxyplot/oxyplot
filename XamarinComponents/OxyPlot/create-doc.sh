#!/bin/sh

# Create or update documentation from ASSEMBLIES.
#mono ../../Tools/Xamarin/mdoc.exe update -i ../../Output/PCL/OxyPlot.xml -o doc ../../Output/PCL/OxyPlot.dll

# Export mdoc documentation within DIRECTORIES to HTML.
#mono ../../Tools/Xamarin/mdoc.exe export-html -o htmldoc doc

# Assemble documentation for use within the monodoc browser (ecma format).
mono ../../Tools/Xamarin/mdoc.exe assemble -o doc/OxyPlot doc

# Export into Microsoft XML Documentation format files.
mkdir msxdoc
mono ../../Tools/Xamarin/mdoc.exe export-msxdoc doc -o msxdoc/OxyPlot.xml
