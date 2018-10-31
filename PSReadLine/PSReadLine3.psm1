function global:PSConsoleHostReadLine
{
    Microsoft.PowerShell.Core\Set-StrictMode -Off
    [Microsoft.PowerShell.PSConsoleReadLine, Microsoft.PowerShell.PSReadLine3]::ReadLine($host.Runspace, $ExecutionContext)
}
