[CmdletBinding()]
param([string]$GameDir)
. "$PSScriptRoot/scripts/Common.ps1"
& "$ProjectRoot/build.ps1" -GameDir $GameDir
$folder = Join-Path $ProjectRoot 'artifacts/Release/ascetic_home_storage'
Assert-RuntimeFolder $folder
[xml]$props = Get-Content "$ProjectRoot/Directory.Build.props" -Raw -Encoding utf8
$zip = Join-Path $ProjectRoot "artifacts/ProbablyStolen-AsceticHomeStorage-$($props.Project.PropertyGroup.Version).zip"
Compress-Archive -LiteralPath $folder -DestinationPath $zip -Force
$hash = (Get-FileHash -LiteralPath $zip -Algorithm SHA256).Hash
"$hash  $(Split-Path $zip -Leaf)" | Set-Content "$ProjectRoot/artifacts/SHA256SUMS.txt" -Encoding ascii
Write-Host "Packaged $zip"
