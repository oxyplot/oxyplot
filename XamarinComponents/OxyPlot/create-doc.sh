#!/bin/sh
mono ../../Tools/Xamarin/mdoc.exe update -i ../../Output/PCL/OxyPlot.xml -o doc ../../Output/PCL/OxyPlot.dll
mono ../../Tools/Xamarin/mdoc export-html -o htmldoc doc