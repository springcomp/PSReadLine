@{
RootModule = 'PSReadLine3.psm1'
NestedModules = @("Microsoft.PowerShell.PSReadLine3.dll")
ModuleVersion = '3.0'
GUID = '74075365-998E-4EEF-A460-78F3C878116F'
Author = 'Microsoft Corporation'
CompanyName = 'Microsoft Corporation'
Copyright = '(c) Microsoft Corporation. All rights reserved.'
Description = 'Great command line editing in the PowerShell console host'
PowerShellVersion = '5.0'
DotNetFrameworkVersion = '4.6.1'
CLRVersion = '4.0.0'
FormatsToProcess = 'PSReadLine3.format.ps1xml'
AliasesToExport = @()
FunctionsToExport = 'PSConsoleHostReadLine'
CmdletsToExport = 'Get-PSReadLineKeyHandler','Set-PSReadLineKeyHandler','Remove-PSReadLineKeyHandler',
                  'Get-PSReadLine3Option','Set-PSReadLine3Option'
HelpInfoURI = 'https://go.microsoft.com/fwlink/?LinkId=528806'
}
