pushd ..\..\lynx\build
call BuildRelease.cmd
popd
copy ..\..\lynx\output\LynxToolkit.Documents.* ..\Tools\Lynx
copy ..\..\lynx\output\WikiPad.exe ..\Tools\Lynx
copy ..\..\lynx\output\WikiT.exe ..\Tools\Lynx
copy ..\..\lynx\output\XmlDocT.exe ..\Tools\Lynx
copy ..\..\lynx\output\HhcGen.exe ..\Tools\Lynx
copy ..\..\lynx\output\FtpUpload.exe ..\Tools\Lynx