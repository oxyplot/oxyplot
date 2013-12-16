mkdir ..\Output\Documentation
mkdir ..\Output\Documentation\API

..\Tools\Lynx\XmlDocT.exe /format=html /template=..\Documentation\API_Template.html /version=2013.1.1 /input=..\Output\NET45\OxyPlot*.dll /input=..\Output\NetCore45\OxyPlot.Metro.dll /output=..\Output\Documentation\API /helpcontents=..\Output\Documentation\API\OxyPlotAPI.hhc > GenerateAPIDocumentation.log

xcopy ..\Documentation\*.css ..\Output\Documentation\API /S /Y
xcopy ..\Documentation\OxyPlotAPI.hhp ..\Output\Documentation\API /S /Y

..\Tools\HtmlHelp\hhc.exe ..\Output\Documentation\API\OxyPlotAPI.hhp > GenerateAPIDocumentation_HelpCompiler.log

move ..\Output\Documentation\API\OxyPlotAPI.chm ..\Output\Documentation
rem del ..\Output\Documentation\API\*.hh?