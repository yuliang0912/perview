@echo off
echo YUI Compressor v2.4.2

REM ��� Java ����
if "%JAVA_HOME%" == "" goto NoJavaHome
if not exist "%JAVA_HOME%\bin\java.exe" goto NoJavaHome
if not exist "%JAVA_HOME%\bin\native2ascii.exe" goto NoJavaHome


if "%1" == "" goto exit
pushd "%1"

echo ����ѹ��Css�ļ�
for /r %%i in (*.css) do (
    echo ѹ��Css�ļ� %%i
    "%JAVA_HOME%\bin\java.exe" -jar "%~dp0yuicompressor.jar" --charset UTF-8 -o %%i %%i
)
echo ����ѹ��js�ļ�
for /r %%i in (*.js) do (
    echo ѹ��JS�ļ� %%i
    "%JAVA_HOME%\bin\java.exe" -jar "%~dp0yuicompressor.jar" --charset UTF-8 -o %%i %%i
)

:exit
exit

:NoJavaHome
echo.
echo **** ���Ȱ�װ JDK ������ JAVA_HOME ��������
echo.