# ECLIPSE: THE LAST CIVILIZATION
## Android-Specific Optimization & Build Architecture Plan

**Studio:** SVK Creationz  
**Engine:** Unity 6 (6000.0.32f1 LTS)  
**Render Pipeline:** Universal Render Pipeline (URP 17.0.3)  
**Target Hardware:** Android Smartphones & Tablets (Snapdragon, Dimensity, Exynos, Tensor)  
**Document Ref:** `ANDROID_OPTIMIZATION_PLAN.md`  

---

### 1. Android Platform Specifications & Build Settings

All Android player settings are automated via [`EclipseBuildAutomation.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Editor/BuildPipeline/EclipseBuildAutomation.cs) and configured as follows:

```
┌─────────────────────────────────┬────────────────────────────────────────────────────────┐
│ Parameter                       │ Setting & Technical Rationale                          │
├─────────────────────────────────┼────────────────────────────────────────────────────────┤
│ Application Identifier          │ com.svkcreationz.eclipse                               │
│ Product Name                    │ ECLIPSE: THE LAST CIVILIZATION                         │
│ Scripting Backend               │ IL2CPP (High performance native C++ compilation)       │
│ Target Architecture             │ ARM64 Only (Google Play mandatory 64-bit compliance)   │
│ Minimum API Level               │ Android 8.0 'Oreo' (API Level 26)                      │
│ Target API Level                │ Android 14.0 (API Level 34) / Android 15 (API 35)      │
│ Default Screen Orientation      │ Landscape (Auto-rotate between Left and Right)         │
│ Auto-Rotation Portrait          │ Disabled (Prevents accidental vertical flips)          │
│ Internet Permission             │ Enabled (Required for NGO multiplayer co-op)           │
│ Texture Compression Format      │ ASTC (Adaptive Scalable Texture Compression)           │
└─────────────────────────────────┴────────────────────────────────────────────────────────┘
```

#### Verified Local Android Development Environment:
- **Android SDK:** Installed at `C:\Users\selva\AppData\Local\Android\Sdk` (build-tools, platform-tools, emulator).
- **Android Studio & OpenJDK:** Installed at `C:\Program Files\Android\Android Studio\jbr`.
- **Unity Build Runner:** `Tools/Build/build_android_apk.ps1`.

---

### 2. Android Graphics API Strategy (Vulkan vs OpenGLES)

Android devices exhibit substantial GPU driver fragmentation. The graphics API configuration balances peak performance with broad compatibility:

```
                             GRAPHICS API FALLBACK HIERARCHY
                                            │
               ┌────────────────────────────┴────────────────────────────┐
               ▼                                                         ▼
       PRIMARY: VULKAN 1.1+                                   FALLBACK: OPENGLES 3.2
       ────────────────────────                               ────────────────────────
       • Qualcomm Adreno 6xx / 7xx                            • Older MediaTek Helio chipsets
       • ARM Mali-G77 / G710 / G715                           • Legacy Mali-T / G52 GPUs
       • Multi-threaded render passes                         • Fallback when Vulkan driver fails
       • Direct compute shader support (VFX Graph)            • Verified stable OpenGL driver
       • Up to 35% lower CPU overhead                         • Single-threaded graphics submission
```

- **Validation Layers:** `vulkanEnableValidationLayers: 0` in release builds to eliminate diagnostic overhead.

---

### 3. Android Texture Compression & Memory Strategy

To fit within strict mobile memory envelopes (< 650 MB VRAM), all project textures adhere to ASTC block compression:

| Texture Type | Target Format | Bitrate / Block Size | Usage in ECLIPSE |
| :--- | :--- | :---: | :--- |
| **Albedo / Base Color** | `ASTC_RGBA_6x6` | 3.56 bpp | Character clothing, brick facades, wood palisades, asphalt |
| **Normal Maps** | `ASTC_RGB_4x4` | 8.00 bpp | High-fidelity crack normals, vein structures, bullet impacts |
| **Packed Mask Maps** | `ASTC_RGBA_6x6` | 3.56 bpp | R=Metallic, G=AO, B=Detail, A=Smoothness |
| **UI Atlases & Icons** | `ASTC_RGBA_8x8` | 2.00 bpp | Menus, touch buttons, inventory items, HUD gauges |
| **VFX Flipbooks** | `ASTC_RGBA_6x6` | 3.56 bpp | Muzzle flashes, fire cores, blood bursts |

---

### 4. Dynamic Device Profiling & Quality Profiles

The game automatically samples hardware specifications at boot using [`MobileDeviceProfile.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Settings/MobileDeviceProfile.cs):

```
                               DEVICE PROFILE ROUTER
                                         │
        ┌────────────────────────────────┼────────────────────────────────┐
        ▼                                ▼                                ▼
  LOW PROFILE                      MEDIUM PROFILE                   HIGH PROFILE
  (System RAM < 3.5 GB)            (System RAM 3.5 GB – 5.5 GB)     (System RAM > 5.5 GB)
  ─────────────────────            ────────────────────────────     ─────────────────────
  • Render Scale: 0.70x            • Render Scale: 0.85x            • Render Scale: 1.00x Native
  • Target: 30 FPS Rock Solid      • Target: 45–60 FPS Stable       • Target: 60 FPS Photoreal
  • Shadow: 1 Cascade (25m)        • Shadow: 2 Cascades (45m)       • Shadow: 4 Cascades (75m)
  • MSAA: Off (FXAA)               • MSAA: 2x                       • MSAA: 4x
  • Vegetation Density: 40%        • Vegetation Density: 75%        • Vegetation Density: 100%
  • SSAO: Disabled                 • SSAO: Disabled                 • SSAO: Enabled (Half-Res)
```

---

### 5. Thermal Throttling & Adaptive Performance (Samsung GameSDK)

1. **Thermal Headroom Monitoring:** Real-time hardware thermal sampling tracks device temperature escalation.
2. **Dynamic Resolution Scaling (DRS / STP):**
   - When device temperature crosses **Warning Level 2**, render scale gracefully steps down in increments of `0.05` (e.g., from `1.0x` down to `0.80x`).
   - Spatial-Temporal Post-Processing (STP) upscales the buffer back to native screen resolution without noticeable blurring.
3. **Emergency Load Shedding:**
   - Drops shadow cascades from 4 to 2.
   - Throttles background zombie AI tick rates from 30 Hz to 10 Hz for off-screen enemies.
   - Culls minor decorative grass clumps.

---

### 6. Android Build Automation & Release Packaging

#### Development APK Generation:
```powershell
powershell -ExecutionPolicy Bypass -File Tools/Build/build_android_apk.ps1
```
- Output: `Builds/Android/ECLIPSE-Android-Development.apk`.
- Development flags: Profiler connection enabled, script debugging allowed, deep profiling support.

#### Google Play Store Release (AAB + PAD):
- Format: **Android App Bundle (.aab)**.
- Packaging: **Play Asset Delivery (PAD)** splits high-resolution textures into fast-follow asset packs, keeping initial download size under 150 MB.
