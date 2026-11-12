[CmdletBinding()]
param()
. "$PSScriptRoot/Common.ps1"
$files = @(Get-ChildItem -LiteralPath $ProjectRoot -Filter '*.ps1' -File)
$files += @(Get-ChildItem -LiteralPath "$ProjectRoot/scripts" -Filter '*.ps1' -File)
foreach ($file in $files) {
    $tokens = $null
    $errors = $null
    [System.Management.Automation.Language.Parser]::ParseFile($file.FullName, [ref]$tokens, [ref]$errors) | Out-Null
    if ($errors.Count) { throw "PowerShell syntax error: $($file.Name): $errors" }
}
[xml]$manifest = Get-Content "$ProjectRoot/mod/manifest.xml" -Raw -Encoding utf8
[xml]$props = Get-Content "$ProjectRoot/Directory.Build.props" -Raw -Encoding utf8
if ($manifest.Manifest.ID -ne 'ascetic_home_storage' -or $manifest.Manifest.ModVersion -ne $props.Project.PropertyGroup.Version) { throw 'Manifest identity/version mismatch.' }
if ($manifest.Manifest.Author -ne 'fuliezi' -or @($manifest.SelectNodes('/Manifest/Capabilities/*')).Count -ne 0) { throw 'Author/capabilities mismatch.' }
$tracked = @(& git -C $ProjectRoot ls-files)
if ($LASTEXITCODE -ne 0) { throw 'git ls-files failed.' }
foreach ($path in $tracked) {
    if ($path -match '(?i)(^|/)(\.local|artifacts|bin|obj)/|\.(dll|exe|zip|pdb|vdf|log|acf)$|config\.local\.json$') { throw "Forbidden tracked file: $path" }
}
& git -C $ProjectRoot diff --check
if ($LASTEXITCODE -ne 0) { throw 'Whitespace check failed.' }
Write-Host 'PASS: scripts, manifest and tracked-file policy.'
