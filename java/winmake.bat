@echo off

REM set path for YOUR_JDK to your jdk basic folder, which must contain bin\javac.exe bin\java.exe lib\src.zip
set YOURJDK=%USERPROFILE%\.jdks\openjdk-26

echo "Set YOURJDK currently %YOURJDK% before and add to Path and CLASSPATH"
set  Path=%Path%;%YOURJDK%\bin
set  CLASSPATH=%CLASSPATH%;%YOURJDK%\lib


@call jdkSwitchRun.bat YOURJDK