param(
    [string]$UnityEditorPath = "",
    [switch]$InstallToConnectedDevice
)

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
if (-not (Test-Path (Join-Path $projectRoot "Assets"))) {
    $projectRoot = (Get-Location).Path
}

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "  ECLIPSE: THE LAST CIVILIZATION - Android APK Builder    " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan

# 1. Resolve Unity Editor Path
if ([string]::IsNullOrEmpty($UnityEditorPath)) {
    $defaultPaths = @(
        "C:\Program Files\Unity\Hub\Editor\6000.0.32f1\Editor\Unity.exe",
        "C:\Program Files\Unity\Hub\Editor\6000.0.0f1\Editor\Unity.exe",
        "C:\Program Files\Unity 6\Editor\Unity.exe"
    )
    foreach ($p in $defaultPaths) {
        if (Test-Path $p) {
            $UnityEditorPath = $p
            break
        }
    }
}

$buildOutputDir = Join-Path $projectRoot "Builds\Android"
if (-not (Test-Path $buildOutputDir)) {
    New-Item -ItemType Directory -Path $buildOutputDir -Force | Out-Null
}

$apkPath = Join-Path $buildOutputDir "ECLIPSE-Android-Development.apk"
$logPath = Join-Path $projectRoot "Logs\AndroidBuild.log"
$logDir = Split-Path -Parent $logPath
if (-not (Test-Path $logDir)) {
    New-Item -ItemType Directory -Path $logDir -Force | Out-Null
}

$adbPath = "C:\Users\selva\AppData\Local\Android\Sdk\platform-tools\adb.exe"

# 2. Check if Unity Editor is available for CLI build
if (-not [string]::IsNullOrEmpty($UnityEditorPath) -and (Test-Path $UnityEditorPath)) {
    Write-Host "Found Unity Editor at: $UnityEditorPath" -ForegroundColor Green
    Write-Host "Starting headless build of Android Development APK..." -ForegroundColor Cyan
    
    $args = @(
        "-quit",
        "-batchmode",
        "-projectPath", "`"$projectRoot`"",
        "-executeMethod", "Eclipse.Build.EclipseBuildAutomation.BuildAndroidDevelopmentAPK",
        "-logFile", "`"$logPath`""
    )
    
    Start-Process -FilePath $UnityEditorPath -ArgumentList $args -Wait -NoNewWindow
    
    if (Test-Path $apkPath) {
        $sizeMB = [math]::Round((Get-Item $apkPath).Length / 1MB, 2)
        Write-Host "BUILD SUCCESSFUL! APK generated at: $apkPath ($sizeMB MB)" -ForegroundColor Green
    } else {
        Write-Host "Build failed. Check log at: $logPath" -ForegroundColor Red
    }
} else {
    Write-Host "NOTE: Unity Editor executable not found in default batch path." -ForegroundColor Yellow
    Write-Host "To build from Unity Editor:" -ForegroundColor White
    Write-Host "  1. Open project in Unity 6." -ForegroundColor Gray
    Write-Host "  2. Click top menu: ECLIPSE > Build > Build Android Development APK." -ForegroundColor Gray
    Write-Host "  3. APK output destination: $apkPath" -ForegroundColor Gray
}

# 3. Handle Device Installation via ADB
if ($InstallToConnectedDevice -or (Test-Path $apkPath)) {
    if (Test-Path $adbPath) {
        Write-Host "`nChecking for connected Android devices via ADB..." -ForegroundColor Cyan
        $devices = & $adbPath devices | Select-String -Pattern "\s+device$"
        if ($devices.Count -gt 0) {
            Write-Host "Found $($devices.Count) connected Android device(s)." -ForegroundColor Green
            if (Test-Path $apkPath) {
                Write-Host "Installing APK to connected device..." -ForegroundColor Cyan
                & $adbPath install -r $apkPath
                Write-Host "Launching ECLIPSE on device..." -ForegroundColor Green
                & $adbPath shell monkey -p com.svkcreationz.eclipse -c android.intent.category.LAUNCHER 1
            }
        } else {
            Write-Host "No authorized physical device detected via ADB. Connect your phone via USB with USB Debugging enabled." -ForegroundColor Gray
        }
    }
}
