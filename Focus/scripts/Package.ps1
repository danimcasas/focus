param()

$ErrorActionPreference = 'Stop'
$focusRoot = Split-Path -Parent $PSScriptRoot
$version = (Get-Content -LiteralPath (Join-Path $focusRoot 'VERSION') -Raw).Trim()
$releaseName = "Focus-Windows11-EXE-$version"
$releaseRoot = Join-Path $focusRoot 'release'
$packageRoot = Join-Path $releaseRoot $releaseName
$zipPath = Join-Path $releaseRoot "$releaseName.zip"
if ((Test-Path -LiteralPath $packageRoot) -or (Test-Path -LiteralPath $zipPath)) {
    throw 'Ya existe esta entrega. Conserva el paquete y usa una versión nueva para los cambios.'
}
$build = & (Join-Path $PSScriptRoot 'Build.ps1') -Test
$buildRoot = $build.BuildDirectory
New-Item -ItemType Directory -Path (Join-Path $packageRoot 'web') -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $buildRoot 'Focus.exe'),(Join-Path $buildRoot 'AssemblyInfo.cs'),(Join-Path $focusRoot 'src\Focus.cs') -Destination $packageRoot
Copy-Item -LiteralPath (Join-Path $focusRoot 'index.html') -Destination (Join-Path $packageRoot 'web\index.html')
Copy-Item -LiteralPath (Join-Path $focusRoot 'CHANGELOG.md') -Destination $packageRoot
Set-Content -LiteralPath (Join-Path $packageRoot 'VERSION.txt') -Value $version -Encoding utf8
@"
FOCUS $version — APLICACIÓN DE ESCRITORIO PARA WINDOWS
danimcasas.com curso codex 2026

Extrae el ZIP y abre Focus.exe. Requiere .NET Framework 4.x; el paquete no incluye
runtime ni instalador. El ejecutable no tiene firma digital.

Concentración: 25 minutos. Estudio: 50 minutos. Descanso: 10 minutos.
Iniciar empieza o reanuda; Pausar conserva el tiempo; Reiniciar restaura el modo.
La sesión termina en cero y no se guarda al cerrar. La app funciona sin conexión.

La alternativa web está en web/index.html. El código de escritorio está en
Focus.cs y AssemblyInfo.cs. El historial y las instrucciones de compilación están
en el repositorio GitHub danimcasas/focus.

SHA256SUMS.txt permite comprobar la integridad de los archivos del paquete.
"@ | Set-Content -LiteralPath (Join-Path $packageRoot 'LEEME.txt') -Encoding utf8

$manifest = Get-ChildItem -LiteralPath $packageRoot -Recurse -File | Sort-Object FullName | ForEach-Object {
    $relative = [IO.Path]::GetRelativePath($packageRoot, $_.FullName).Replace('\','/')
    '{0}  {1}' -f (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant(),$relative
}
Set-Content -LiteralPath (Join-Path $packageRoot 'SHA256SUMS.txt') -Value $manifest -Encoding utf8
Compress-Archive -LiteralPath $packageRoot -DestinationPath $zipPath -CompressionLevel Optimal
$zipHash = (Get-FileHash -LiteralPath $zipPath -Algorithm SHA256).Hash.ToLowerInvariant()
Set-Content -LiteralPath "$zipPath.sha256" -Value "$zipHash  $releaseName.zip" -Encoding ascii
Write-Output "Paquete: $zipPath"
Write-Output "SHA-256: $zipHash"
