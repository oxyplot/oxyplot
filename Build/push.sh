#!/bin/sh

# Pushing packages to NuGet

echo ACCESS KEY: $NUGET_ACCESS_KEY

for f in ../Output/*.nupkg 
do
	mono ../Source/.nuget/NuGet.exe push $f $NUGET_ACCESS_KEY
done
