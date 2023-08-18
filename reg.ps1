
Remove-Module PSReadLine -EA SilentlyContinue
Import-Module PSReadLine3
Set-PSReadLine3Option -EditMode VI
iex $(gc PSReadLine\PSReadLine3.psm1 -Raw)
