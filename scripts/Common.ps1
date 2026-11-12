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
function Get-LocalSetting([string]$Name) {
    $path = Join-Path $ProjectRoot 'config.local.json'
    if (Test-Path -LiteralPath $path) {
        $settings = Get-Content -LiteralPath $path -Raw -Encoding utf8 | ConvertFrom-Json
        if ($settings.PSObject.Properties.Name -contains $Name) { return [string]$settings.$Name }
    }
    return ''
}
function Get-ModVersion {
    [xml]$props = Get-Content -LiteralPath (Join-Path $ProjectRoot 'Directory.Build.props') -Raw -Encoding utf8
    return [string]$props.Project.PropertyGroup.Version
}
function Assert-RuntimeFolder([string]$Path, [switch]$AllowOlderVersion) {
    $expected = @('manifest.xml','ProbablyStolen.AsceticHomeStorage.dll','preview.png')
    $actual = @(Get-ChildItem -LiteralPath $Path -Force)
    if ($AllowOlderVersion -and $actual.Count -eq 2) { $expected = @('manifest.xml','ProbablyStolen.AsceticHomeStorage.dll') }
    if ($actual.Count -ne $expected.Count -or @($actual | Where-Object { $_.PSIsContainer -or $_.Name -notin $expected }).Count) { throw 'Unexpected runtime files.' }
    [xml]$manifest = Get-Content -LiteralPath (Join-Path $Path 'manifest.xml') -Raw -Encoding utf8
    [xml]$props = Get-Content -LiteralPath (Join-Path $ProjectRoot 'Directory.Build.props') -Raw -Encoding utf8
    if ($manifest.Manifest.ID -ne 'ascetic_home_storage' -or (!$AllowOlderVersion -and $manifest.Manifest.ModVersion -ne $props.Project.PropertyGroup.Version)) { throw 'Manifest ID/version mismatch.' }
}
