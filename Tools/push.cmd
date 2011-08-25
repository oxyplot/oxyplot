cd ..\Packages
..\tools\nuget.exe push -source http://packages.nuget.org/v1/ %1 %NUGET_API_KEY%
pause