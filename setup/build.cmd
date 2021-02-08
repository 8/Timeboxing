:: build the application
dotnet publish ..\src\TimeBoxing\TimeBoxing.fsproj -f netcoreapp5.0 -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true /p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -o ../setup/in/

:: build the setup.exe
call "%PROGRAMFILES(x86)%\NSIS\makensis.exe" setup.nsi

:: zip the installer
cd out
call 7z a TimeboxingSetup.zip TimeboxingSetup.exe
cd ..
