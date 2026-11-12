Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$script:ProjectRoot = Split-Path $PSScriptRoot -Parent
function Resolve-GameDir([string]$GameDir) {
    if (!$GameDir) { $GameDir = $env:PROBABLY_STOLEN_GAME_DIR }
    $configPath = Join-Path $ProjectRoot 'config.local.json'
    if (!$GameDir -and (Test-Path -LiteralPath $configPath)) {
        $GameDir = (Get-Content -LiteralPath $configPath -Raw -Encoding utf8 | ConvertFrom-Json).GameDir
    }
    if (!$GameDir) { throw 'Set -GameDir or PROBABLY_STOLEN_GAME_DIR.' }
    $resolved = (Resolve-Path -LiteralPath $GameDir).Path
    foreach ($name in @('Assembly-CSharp.dll','UnityEngine.CoreModule.dll','0Harmony.dll')) {
        if (!(Test-Path -LiteralPath (Join-Path $resolved "Probably Stolen_Data/Managed/$name"))) { throw "Missing reference: $name" }
    }
    return $resolved
}
function Assert-RuntimeFolder([string]$Path) {
    $expected = @('manifest.xml','ProbablyStolen.AsceticHomeStorage.dll')
    $actual = @(Get-ChildItem -LiteralPath $Path -Force)
    if ($actual.Count -ne 2 -or @($actual | Where-Object { $_.PSIsContainer -or $_.Name -notin $expected }).Count) { throw 'Unexpected runtime files.' }
    [xml]$manifest = Get-Content -LiteralPath (Join-Path $Path 'manifest.xml') -Raw -Encoding utf8
    [xml]$props = Get-Content -LiteralPath (Join-Path $ProjectRoot 'Directory.Build.props') -Raw -Encoding utf8
    if ($manifest.Manifest.ID -ne 'ascetic_home_storage' -or $manifest.Manifest.ModVersion -ne $props.Project.PropertyGroup.Version) { throw 'Manifest ID/version mismatch.' }
}
