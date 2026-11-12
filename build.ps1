[CmdletBinding()]
param([string]$GameDir, [ValidateSet('Release','Debug')][string]$Configuration = 'Release')
. "$PSScriptRoot/scripts/Common.ps1"
$game = Resolve-GameDir $GameDir
& dotnet build "$ProjectRoot/src/AsceticHomeStorage/AsceticHomeStorage.csproj" -c $Configuration "-p:GameDir=$game" --nologo
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
$output = Join-Path $ProjectRoot "artifacts/$Configuration/ascetic_home_storage"
New-Item -ItemType Directory -Force $output | Out-Null
Copy-Item -LiteralPath "$ProjectRoot/src/AsceticHomeStorage/bin/$Configuration/netstandard2.1/ProbablyStolen.AsceticHomeStorage.dll" -Destination $output -Force
Copy-Item -LiteralPath "$ProjectRoot/mod/manifest.xml" -Destination $output -Force
Copy-Item -LiteralPath "$ProjectRoot/mod/preview.png" -Destination $output -Force
Assert-RuntimeFolder $output
Write-Host "Built $output"
