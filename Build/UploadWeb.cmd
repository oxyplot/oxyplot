xcopy ..\icons\favicon.ico ..\Web /Y
mkdir ..\Web\Examples
xcopy ..\documentation\images\series ..\Web\Examples /Y
..\Tools\Lynx\FtpUpload.exe %OXYPLOT_FTP_SERVER% %OXYPLOT_FTP_USER% %OXYPLOT_FTP_PASSWORD% ..\Web\*.* %OXYPLOT_FTP_FOLDER% /t=1