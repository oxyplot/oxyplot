cd ..
git reset --hard HEAD

del /S /Q Output\*.*

UpdateVersionNumbers.exe /VersionFromNuGet=OxyPlot.Core /Dependency=OxyPlot.Core /ExtractReleaseNotes=CHANGELOG.md /Directory=.

msbuild.exe Source\OxyPlot.Mobile.sln /p:Configuration=Release

nuget pack Source\OxyPlot.Mobile.nuspec -OutputDirectory Output
