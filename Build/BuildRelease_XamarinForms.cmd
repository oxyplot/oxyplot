UpdateVersionNumbers.exe /VersionFromNuGet=OxyPlot.Core /Dependency=OxyPlot.Core /ReleaseNotesFile=..\CHANGELOG.md /Directory=..

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" ..\Source\OxyPlot.XamarinForms.sln /p:Configuration=Release
