[CmdletBinding()]
param([string]$GameDir, [ValidateSet('Release','Debug')][string]$Configuration = 'Release')
. "$PSScriptRoot/scripts/Common.ps1"
$game = Resolve-GameDir $GameDir
if (Get-Process -Name 'Probably Stolen' -ErrorAction SilentlyContinue) { throw 'Close the game before installation.' }
& "$ProjectRoot/build.ps1" -GameDir $game -Configuration $Configuration
$target = Join-Path $game 'Mods/ascetic_home_storage'
if (Test-Path -LiteralPath $target) { Assert-RuntimeFolder $target -AllowOlderVersion }
New-Item -ItemType Directory -Force $target | Out-Null
foreach ($name in @('manifest.xml','ProbablyStolen.AsceticHomeStorage.dll','preview.png')) {
    Copy-Item -LiteralPath "$ProjectRoot/artifacts/$Configuration/ascetic_home_storage/$name" -Destination $target -Force
}
Assert-RuntimeFolder $target
Write-Host "Installed $target. Enable the mod in-game and restart."
