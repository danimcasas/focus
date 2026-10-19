param([switch]$Test)

$ErrorActionPreference = 'Stop'
$focusRoot = Split-Path -Parent $PSScriptRoot
$version = (Get-Content -LiteralPath (Join-Path $focusRoot 'VERSION') -Raw).Trim()
if ($version -notmatch '^\d+\.\d+\.\d+$') { throw 'VERSION debe tener formato X.Y.Z.' }
$webSource = Join-Path $focusRoot 'index.html'
if ((Get-Content -LiteralPath $webSource -Raw) -notmatch ('data-app-version>' + [regex]::Escape($version) + '</span>')) {
    throw 'La versión visible de la web no coincide con VERSION.'
}
$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path -LiteralPath $compiler)) {
    $compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
}
if (-not (Test-Path -LiteralPath $compiler)) { throw 'No se encontró el compilador de .NET Framework 4.x.' }

$buildRoot = Join-Path $focusRoot ('.build\' + $version + '\' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $buildRoot -Force | Out-Null
$assemblyInfo = Join-Path $buildRoot 'AssemblyInfo.cs'
@"
using System.Reflection;
[assembly: AssemblyVersion("$version.0")]
[assembly: AssemblyFileVersion("$version.0")]
[assembly: AssemblyInformationalVersion("$version")]
[assembly: AssemblyCompany("danimcasas.com curso codex 2026")]
"@ | Set-Content -LiteralPath $assemblyInfo -Encoding utf8
$exe = Join-Path $buildRoot 'Focus.exe'
$source = Join-Path $focusRoot 'src\Focus.cs'
& $compiler /nologo /target:winexe /platform:anycpu /optimize+ /codepage:65001 "/out:$exe" /reference:System.Windows.Forms.dll /reference:System.Drawing.dll $source $assemblyInfo
if ($LASTEXITCODE -ne 0) { throw 'Falló la compilación de Focus.' }
if ((Get-Item -LiteralPath $exe).VersionInfo.FileVersion -ne "$version.0") { throw 'Versión incorrecta en el ejecutable.' }

if ($Test) {
    $smokeExe = Join-Path $buildRoot 'FocusSmokeTest.exe'
    & $compiler /nologo /target:exe /platform:anycpu /codepage:65001 /main:FocusDesktop.SmokeTest "/out:$smokeExe" /reference:System.Windows.Forms.dll /reference:System.Drawing.dll $source $assemblyInfo (Join-Path $focusRoot 'tests\SmokeTest.cs')
    if ($LASTEXITCODE -ne 0) { throw 'Falló la compilación de las pruebas.' }
    & $smokeExe $buildRoot | ForEach-Object { Write-Host $_ }
    if ($LASTEXITCODE -ne 0) { throw 'Falló la prueba funcional de Focus.' }
}
Write-Host "Focus $version compilado: $exe"
[pscustomobject]@{ Version = $version; BuildDirectory = $buildRoot; Executable = $exe }
