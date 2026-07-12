# Set UTF-8 encoding to handle special characters in paths
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding = [System.Text.Encoding]::UTF8

$sourceDir = "test-results"
$destDir = [System.IO.Path]::Combine($env:USERPROFILE, "OneDrive", "Area de Trabalho", "Gilson DOCS", "videos-candidato")

# Fallback: try the accented version directly
if (!(Test-Path -Path $destDir)) {
    $destDir = "C:\Users\gilso\OneDrive\`u{00C1}rea de Trabalho\Gilson DOCS\videos-candidato"
}

if (!(Test-Path -Path $destDir)) {
    New-Item -ItemType Directory -Force -Path $destDir | Out-Null
}

$webmFiles = Get-ChildItem -Path $sourceDir -Filter "*.webm" -Recurse

foreach ($file in $webmFiles) {
    $folderName = $file.Directory.Name
    $cleanName = $folderName -replace "-chromium$", ""
    $newName = "$cleanName.webm"
    $destPath = Join-Path -Path $destDir -ChildPath $newName

    Copy-Item -Path $file.FullName -Destination $destPath -Force
    Write-Host "Copiado: $newName"
}

Write-Host "Exportacao concluida."
