#!/bin/bash

VERSION=2014.1

# Clean the output folders
cd ../XamarinComponents/OxyPlot
rm -rf *.log
rm -rf htmldoc
rm -rf monodoc
rm -rf icons
rm -rf msxdoc
cd ../../Build

# Build OxyPlot and samples
. build.sh
echo

# Create documentation
. create-doc.sh

# Create the component
. create-xamarin-component.sh