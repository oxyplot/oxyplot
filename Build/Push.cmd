for %%G in (..\Packages\*.nupkg) do ..\Tools\NuGet\NuGet.exe push %%G %NUGET_ACCESS_KEY% > push.log
