; Get rid of warn "7998: ANSI targets are deprecated"
Unicode True

; Name of the installer
Name "Timeboxing"

; Name of the setup file
OutFile "out\TimeboxingSetup.exe"

; Installation directory
InstallDir $PROGRAMFILES\Timeboxing

InstallDirRegKey HKLM "Software\Timeboxing" "Install_Dir"

; Pages
Page components
Page directory
Page instfiles

; Pages for uninstallation
UninstPage uninstConfirm
UninstPage instfiles

; Main Application
Section "!Timeboxing"

  SectionIn RO

  SetOutPath $INSTDIR

  File ".\in\Timeboxing.exe"

  ; Write shortcut link
  CreateShortCut "$SMPROGRAMS\Timeboxing.lnk" "$INSTDIR\Timeboxing.exe" "" "$INSTDIR\Timeboxing.exe" 0

  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\Timeboxing "Install_Dir" "$INSTDIR"

  ; Write the uninstall keys for windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Timeboxing" "DisplayName" "Timeboxing"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Timeboxing" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Timeboxing" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Timeboxing" "NoRepair" 1
  
  ; Write Uninstaller
  WriteUninstaller "uninstall.exe"

SectionEnd


;  Uninstaller
Section "Uninstall"
  
  ; Remove the registry key that stores the installation directory
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Timeboxing"
  DeleteRegKey HKLM SOFTWARE\Timeboxing
  
  ; Delete Shortcut link
  Delete "$SMPROGRAMS\Timeboxing.lnk"

  ; Delete Application Files
  Delete $INSTDIR\Timeboxing.exe
    
  Delete $INSTDIR\uninstall.exe
  RMDir "$INSTDIR"
  
SectionEnd