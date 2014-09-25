for %%G in (..\Output\*.nupkg) do ..\Source\.nuget\NuGet.exe push %%G %NUGET_ACCESS_KEY%
