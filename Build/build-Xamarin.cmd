cd ..
git reset --hard HEAD

del /S /Q Output\*.*

UpdateVersionNumbers.exe /VersionFromNuGet=OxyPlot.Core /Dependency=OxyPlot.Core /ExtractReleaseNotes=CHANGELOG.md /Directory=.

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" Source\OxyPlot.Xamarin.Forms.sln /p:Configuration=Release

cd Build
call pack-Xamarin.cmd
