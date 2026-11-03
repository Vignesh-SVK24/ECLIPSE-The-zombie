# ECLIPSE: THE LAST CIVILIZATION
## Android APK Build, Device Installation & Testing Guide

**Developer:** SVK Creationz  
**Application ID:** `com.svkcreationz.eclipse`  
**Target Engine:** Unity 6 (6000.0.32f1 LTS) | Universal Render Pipeline (URP 17.0.3)  
**Output Destination:** `Builds/Android/ECLIPSE-Android-Development.apk`

---

### Method A: One-Click Build within Unity Editor (Recommended)
1. Open the project folder in **Unity 6 Hub**:
   `C:\Users\selva\.gemini\antigravity\scratch\eclipse-the-last-civilization`
2. Wait for Unity package resolution (`manifest.json` will restore URP, Input System, Netcode).
3. From the top menu bar, click:  
   **`ECLIPSE > Build > Build Android Development APK`**
4. The automated build pipeline will configure ARM64, IL2CPP, min SDK 26, target SDK 34, landscape autorotation, and compile the development APK directly to:
   `Builds/Android/ECLIPSE-Android-Development.apk`

---

### Method B: Headless Command-Line Build (PowerShell)
Open PowerShell and run:
```powershell
.\Tools\Build\build_android_apk.ps1 -UnityEditorPath "C:\Path\To\Unity.exe"
```

---

### Method C: Installing & Playing on Your Android Phone via USB
1. On your Android phone, enable **Developer Options** and **USB Debugging**:
   - Go to `Settings > About Phone` and tap **Build Number** 7 times.
   - Go to `Settings > System > Developer Options` and turn on **USB Debugging**.
2. Connect your phone to your PC via a USB cable and tap **Allow USB Debugging** on the phone prompt.
3. Verify your device is detected:
   ```powershell
   & "C:\Users\selva\AppData\Local\Android\Sdk\platform-tools\adb.exe" devices
   ```
4. Install the generated APK directly:
   ```powershell
   & "C:\Users\selva\AppData\Local\Android\Sdk\platform-tools\adb.exe" install -r "Builds\Android\ECLIPSE-Android-Development.apk"
   ```
5. Launch the game directly from PC or tap the **ECLIPSE** icon on your phone home screen:
   ```powershell
   & "C:\Users\selva\AppData\Local\Android\Sdk\platform-tools\adb.exe" shell monkey -p com.svkcreationz.eclipse -c android.intent.category.LAUNCHER 1
   ```
