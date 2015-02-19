cd ..
git reset --hard HEAD

del /S /Q Output\*.*

UpdateVersionNumbers.exe /VersionFromNuGet=OxyPlot.Core /Dependency=OxyPlot.Core /Dependency=OxyPlot.Mobile /ExtractReleaseNotes=CHANGELOG.md /Directory=.

nuget restore Source\OxyPlot.Mobile.sln
msbuild.exe Source\OxyPlot.Mobile.sln /p:Configuration=Release

nuget pack Source\OxyPlot.Mobile.nuspec -OutputDirectory Output
nuget pack Source\OxyPlot.Xamarin.Forms\OxyPlot.Xamarin.Forms.nuspec -OutputDirectory Output

cd build