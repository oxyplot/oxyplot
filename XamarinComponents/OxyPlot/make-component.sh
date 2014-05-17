#!/bin/bash

VERSION=2014.1

# Clean the output folders
. clean.sh

# Build OxyPlot and samples
DIR=$PWD
cd ../../Build
. build.sh
cd $DIR

# Create documentation
. create-doc.sh

# Create the component
. create-component.sh