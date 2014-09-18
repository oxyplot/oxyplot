#!/bin/sh

# Push all packages to NuGet

for f in ../Output/*.nupkg 
do
	mono /usr/local/bin/NuGet.exe push $f $NUGET_ACCESS_KEY
done
