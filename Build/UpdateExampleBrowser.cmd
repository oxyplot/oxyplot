echo off
echo HOST: '%MYFTPSERVER%'
echo USER: '%MYFTPUSER%'
echo PW:   '%MYFTPPWD%'
..\Tools\Lynx\FtpUpload.exe %MYFTPSERVER% %MYFTPUSER% %MYFTPPWD% ..\Output\SL4\Examples\Silverlight\ExampleBrowser\ExampleBrowser.xap /objo.net/wwwroot/OxyPlot/ExampleBrowser/ExampleBrowser.xap