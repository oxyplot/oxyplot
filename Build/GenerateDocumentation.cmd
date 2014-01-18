"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" ..\Source\Examples\DocumentationExamples\ExampleGenerator\ExampleGenerator.sln /p:Configuration=Release
mkdir ..\Documentation\Images\Series
..\Source\Examples\DocumentationExamples\ExampleGenerator\bin\Release\ExampleGenerator.exe ..\Documentation\Images\Series

..\Tools\Lynx\WikiT.exe /format=html /template=..\Documentation\Help_Template.html /localLinks={0}.html /version=2013.1.1 /input=..\Documentation\*.wiki /output=..\Output\Documentation > GenerateDocumentation.log
..\Tools\Lynx\WikiT.exe /format=html /template=..\Documentation\UserGuide_Template.html /version=2013.1.1 /localLinks={0}.html /input=..\Documentation\UserGuide.mwiki /output=..\Output\Documentation  > GenerateDocumentation_UserGuide.log

mkdir ..\Output\Documentation\Images
copy ..\Documentation\*.css ..\Output\Documentation
copy ..\Documentation\OxyPlot.hhp ..\Output\Documentation
xcopy ..\Documentation\Images\*.* ..\Output\Documentation\Images /S /Y

..\Tools\Lynx\HhcGen.exe /input=..\Documentation\OxyPlot.hhcgen /output=..\Output\Documentation\OxyPlot.hhc > GenerateDocumentation_HelpContent.log

..\Tools\HtmlHelp\hhc.exe ..\Output\Documentation\OxyPlot.hhp > GenerateDocumentation_HelpCompiler.log
del ..\Output\Documentation\*.hh?