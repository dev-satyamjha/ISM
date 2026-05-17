[Setup]
AppName=ISM+
AppVersion=1.0.3
AppVerName=ISM+ Setup
AppPublisher=Satyam Jha
AppPublisherURL=https://github.com/dev-satyamjha
DefaultDirName={autopf}\ISM+
DefaultGroupName=ISM+
UninstallDisplayIcon={app}\ISM+.exe
SetupIconFile=..\..\Assets\Icon1.ico
Compression=lzma2
SolidCompression=yes
OutputDir=.
OutputBaseFilename=ISM_Plus_Setup
LicenseFile=license.rtf
PrivilegesRequired=lowest
WizardStyle=modern
AppId={{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}

[Files]
Source: "..\Portable\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\ISM+"; Filename: "{app}\ISM+.exe"
Name: "{autodesktop}\ISM+"; Filename: "{app}\ISM+.exe"; Tasks: desktopicon
Name: "{group}\Uninstall ISM+"; Filename: "{uninstallexe}"

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop shortcut"; GroupDescription: "Additional icons:"; Flags: unchecked
Name: "startupicon"; Description: "Start ISM+ with Windows"; GroupDescription: "Additional icons:"; Flags: unchecked

[Registry]
Root: HKCU; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "ISM+"; ValueData: """{app}\ISM+.exe"""; Flags: uninsdeletevalue; Tasks: startupicon

[Run]
Filename: "{app}\ISM+.exe"; Description: "Launch ISM+"; Flags: nowait postinstall skipifsilent
