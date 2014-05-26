for %%G in (..\Packages\*.nupkg) do ..\Source\.nuget\NuGet.exe push %%G %NUGET_ACCESS_KEY% > push.log
