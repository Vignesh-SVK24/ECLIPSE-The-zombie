# ECLIPSE: THE LAST CIVILIZATION
## Mobile Build Audit & Platform Readiness Report

**Developer:** SVK Creationz  
**Engine:** Unity 6 (6000.0.32f1 LTS) | Universal Render Pipeline (URP 17.0.3)  
**Primary Target Platforms:** Android (ARM64, Vulkan primary), iOS (ARM64, Metal)  
**Audit Date:** 2026-10-30  
**Project Path:** `C:\Users\selva\.gemini\antigravity\scratch\eclipse-the-last-civilization`

---

### 1. Development Environment & Build Toolchain Audit

| Component | Path / Specification | Status | Notes |
|---|---|:---:|---|
| **Android SDK** | `C:\Users\selva\AppData\Local\Android\Sdk` | **VERIFIED** | SDK platforms, platform-tools, and build-tools available. |
| **Android ADB** | `C:\Users\selva\AppData\Local\Android\Sdk\platform-tools\adb.exe` | **VERIFIED** | Daemon starts cleanly, ready for device connection and APK install. |
| **Java JDK** | `C:\Program Files\Microsoft\jdk-17.0.19.10-hotspot` / `jdk-21` | **VERIFIED** | JDK 17 (Unity recommended) and JDK 21 available. |
| **Android CLI** | `C:\Users\selva\AppData\AndroidCLI\android.exe` | **VERIFIED** | Functional CLI tool for Android management. |
| **iOS Toolchain** | Windows Development Workstation | **DOCUMENTED** | Direct Xcode/IPA compilation requires macOS; Unity generates full Xcode project for macOS deployment. |

---

### 2. Player Settings & Platform Configuration
- **Application Identifier:** `com.svkcreationz.eclipse`
- **Product Name:** `ECLIPSE: THE LAST CIVILIZATION`
- **Company Name:** `SVK Creationz`
- **Orientation:** Landscape Left (3) & Landscape Right (4) autorotation locked.
- **Color Space:** Linear (1)
- **Scripting Backend:** IL2CPP (Required for 64-bit Google Play & iOS App Store compliance).
- **Target Architecture:** ARM64 (`TargetArchitectures.ARM64 = 2`).
- **Minimum Android API:** API Level 26 (Android 8.0 Oreo) — covers 96%+ of active Android devices.
- **Target Android API:** API Level 34 / 35 (Android 14 / 15).
- **Internet Permission:** Enabled (`PlayerSettings.Android.forceInternetPermission = true`) for co-op multiplayer.

---

### 3. Rendering & Graphics Scalability Audit
- **Pipeline:** Universal Render Pipeline (URP 17.0.3).
- **Render Profiles:**
  - `URP_LowQuality.asset`: Render scale 0.75, 25m shadows, 1 cascade, MSAA 1x (30 FPS target on lower-tier devices).
  - `URP_MediumQuality.asset`: Render scale 0.88, 45m shadows, 2 cascades, MSAA 2x (Balanced).
  - `URP_HighQuality.asset`: Render scale 1.0, 75m shadows, 4 cascades, MSAA 4x (Photorealistic).
- **Graphics APIs:**
  - Android: Vulkan (primary for reduced CPU overhead & thermal efficiency), OpenGLES3 (fallback).
  - iOS: Metal (native low-overhead API).

---

### 4. Input & Control Layer Assessment
- **Existing Systems:** Keyboard and mouse input bindings.
- **Required Action:** Implement a unified touch control interface:
  1. Virtual dynamic/fixed left joystick with normalized directional output.
  2. Dedicated right-half camera touch drag area (multitouch isolated from buttons).
  3. Action buttons: Fire, Aim, Reload, Sprint, Weapon Switch, Interact, Inventory, Pause, Revive.
  4. Notch & cutout safe-area adaptation (`SafeAreaFitter`).
  5. Sensitivity and button scale settings.
