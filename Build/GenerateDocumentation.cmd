"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" ..\Source\Examples\DocumentationExamples\ExampleGenerator\ExampleGenerator.sln /p:Configuration=Release
mkdir ..\Documentation\Images\Series
..\Source\Examples\DocumentationExamples\ExampleGenerator\bin\Release\ExampleGenerator.exe ..\Documentation\Images\Series

..\Tools\Lynx\WikiT.exe /format=html /template=..\Documentation\Web_Template.html /localLinks={0}.html /version=%1 /input=..\Documentation\*.wiki /output=..\Output\Documentation
..\Tools\Lynx\WikiT.exe /format=html /template=..\Documentation\UserGuide_Template.html /version=%1 /localLinks={0}.html /input=..\Documentation\UserGuide.mwiki /output=..\Output\Documentation

mkdir ..\Output\Documentation\Images
copy ..\Documentation\*.css ..\Output\Documentation
copy ..\Documentation\OxyPlot.hhp ..\Output\Documentation
xcopy ..\Documentation\Images\*.* ..\Output\Documentation\Images /S /Y

..\Tools\Lynx\HhcGen.exe /input=..\Documentation\OxyPlot.hhcgen /output=..\Output\Documentation\OxyPlot.hhc

..\Tools\HtmlHelp\hhc.exe ..\Output\Documentation\OxyPlot.hhp
del ..\Output\Documentation\*.hh?