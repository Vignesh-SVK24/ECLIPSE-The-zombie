param(
    [string]$StageTag = "ManualSnapshot",
    [string]$DestinationDir = "Tools\Backup\Archives"
)

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
if (-not (Test-Path (Join-Path $projectRoot "Assets"))) {
    $projectRoot = (Get-Location).Path
}

$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$archiveFolder = Join-Path $projectRoot $DestinationDir

if (-not (Test-Path $archiveFolder)) {
    New-Item -ItemType Directory -Path $archiveFolder -Force | Out-Null
}

$archiveFileName = "ECLIPSE_${StageTag}_${timestamp}.zip"
$archiveFilePath = Join-Path $archiveFolder $archiveFileName

Write-Host "Creating ECLIPSE Project Backup: $archiveFileName..." -ForegroundColor Cyan

$tempBackupDir = Join-Path $env:TEMP "ECLIPSE_Backup_${timestamp}"
New-Item -ItemType Directory -Path $tempBackupDir -Force | Out-Null

try {
    $itemsToBackup = @("Assets", "Packages", "ProjectSettings", "Documentation")
    foreach ($item in $itemsToBackup) {
        $sourcePath = Join-Path $projectRoot $item
        if (Test-Path $sourcePath) {
            $destPath = Join-Path $tempBackupDir $item
            Copy-Item -Path $sourcePath -Destination $destPath -Recurse -Force
        }
    }

    # Compress
    Compress-Archive -Path "$tempBackupDir\*" -DestinationPath $archiveFilePath -CompressionLevel Optimal
    Write-Host "Backup successfully created at: $archiveFilePath" -ForegroundColor Green
    
    $fileSizeMB = [math]::Round((Get-Item $archiveFilePath).Length / 1MB, 2)
    Write-Host "Backup Size: $fileSizeMB MB" -ForegroundColor Gray
}
finally {
    if (Test-Path $tempBackupDir) {
        Remove-Item -Path $tempBackupDir -Recurse -Force -ErrorAction SilentlyContinue
    }
}
