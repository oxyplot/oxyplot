del ../Output/*.nupkg

cd ..\Source

UpdateVersionNumbers.exe /VersionFromNuGet=OxyPlot.Core /Dependency=OxyPlot.Core /Directory=..

nuget pack OxyPlot.XamarinForms/OxyPlot.XamarinForms.nuspec -OutputDirectory ../Output

cd ..\Build