echo off
echo HOST: 'ftp.oxyplot.org'
echo USER: '%OXYPLOT_FTP_USER%'
echo PW:   '%OXYPLOT_FTP_PWD%'
..\Tools\Lynx\FtpUpload.exe ftp.oxyplot.org %OXYPLOT_FTP_USER% %OXYPLOT_FTP_PWD% ..\Output\SL4\Examples\Silverlight\ExampleBrowser\ExampleBrowser.xap /oxyplot.org/wwwroot/ExampleBrowser/ExampleBrowser.xap