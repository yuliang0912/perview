@echo off
echo YUI Compressor v2.4.2

REM 检查 Java 环境
if "%JAVA_HOME%" == "" goto NoJavaHome
if not exist "%JAVA_HOME%\bin\java.exe" goto NoJavaHome
if not exist "%JAVA_HOME%\bin\native2ascii.exe" goto NoJavaHome


if "%1" == "" goto exit
pushd "%1"

echo 正在压缩Css文件
for /r %%i in (*.css) do (
    echo 压缩Css文件 %%i
    "%JAVA_HOME%\bin\java.exe" -jar "%~dp0yuicompressor.jar" --charset UTF-8 -o %%i %%i
)
echo 正在压缩js文件
for /r %%i in (*.js) do (
    echo 压缩JS文件 %%i
    "%JAVA_HOME%\bin\java.exe" -jar "%~dp0yuicompressor.jar" --charset UTF-8 -o %%i %%i
)

:exit
exit

:NoJavaHome
echo.
echo **** 请先安装 JDK 并设置 JAVA_HOME 环境变量
echo.