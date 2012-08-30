@echo off
echo open %MYFTPSERVER%> ftpcmd.dat
echo user %MYFTPUSER%>> ftpcmd.dat
echo %MYFTPPWD%>> ftpcmd.dat
echo quote pasv>> ftpcmd.dat
echo bin>> ftpcmd.dat
echo hash>>ftpcmd.dat
echo lcd %1>>ftpcmd.dat
echo cd %3>>ftpcmd.dat
echo send %2>> ftpcmd.dat
echo quit>> ftpcmd.dat
ftp -v -d -n -s:ftpcmd.dat
del ftpcmd.dat