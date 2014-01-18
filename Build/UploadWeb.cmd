xcopy ..\icons\favicon.ico ..\Web /Y
mkdir ..\Web\Examples
xcopy ..\documentation\images\series ..\Web\Examples /Y
..\Tools\Lynx\FtpUpload.exe ftp.oxyplot.org %OXYPLOT_FTP_USER% %OXYPLOT_FTP_PWD% ..\Web\*.* %OXYPLOT_FTP_FOLDER% /t=1