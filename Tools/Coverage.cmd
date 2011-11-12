set project=OxyPlot.Wpf
set testproject=%project%.Tests
set testdir=..\Source\%testproject%\bin\Release
set output=..\Coverage

set opencoverdir=C:\Program Files (x86)\opencover
set nunitdir=C:\Program Files (x86)\NUnit 2.5.10\bin\net-2.0
set reportgeneratordir=C:\Program Files (x86)\ReportGenerator\bin

"%opencoverdir%\opencover.console.exe" -register:user -target:"%nunitdir%\nunit-console.exe" -targetargs:"%testdir%\%testproject%.dll /noshadow" -filter:"+[%project%]* -[*.Tests]*" -output:"%testdir%\%testproject%.Coverage.xml" > Coverage.OpenCover.log

del /s /q %output%

"%reportgeneratordir%\ReportGenerator.exe" %testdir%\%testproject%.Coverage.xml %output% > Coverage.ReportGenerator.log

rem %output%\index.htm
