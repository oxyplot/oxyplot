#!/bin/bash

VERSION=2014.1

# Clean the output folders
. clean.sh

# Build OxyPlot and samples
. build.sh

# Create documentation
. create-doc.sh

# Create the component
. create-component.sh