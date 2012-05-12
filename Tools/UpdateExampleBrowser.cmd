echo off
echo HOST: '%MYFTPSERVER%'
echo USER: '%MYFTPUSER%'
echo PW:   '%MYFTPPWD%'
call FtpUpload ..\Output\Examples\Silverlight\ExampleBrowser ExampleBrowser.xap /objo.net/wwwroot/OxyPlot/ExampleBrowser