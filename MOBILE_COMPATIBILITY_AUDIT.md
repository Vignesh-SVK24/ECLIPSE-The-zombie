# ECLIPSE: THE LAST CIVILIZATION
## Phase 1 — Comprehensive Mobile Compatibility & Hardware Audit

**Studio:** SVK Creationz  
**Engine:** Unity 6 (6000.0.32f1 LTS / Host Installed: 6000.6.7f1)  
**Render Pipeline:** Universal Render Pipeline (URP 17.0.3)  
**Target Platforms:** Android Smartphones & Tablets (Vulkan/GLES), iPhone & iPad (Metal)  
**Primary Orientation:** Landscape Left & Landscape Right  
**Document Ref:** `MOBILE_COMPATIBILITY_AUDIT.md`  

---

### 1. Executive Summary & Audit Mandate

*ECLIPSE: THE LAST CIVILIZATION* is undergoing complete mobile adaptation to guarantee a responsive, smooth, and visually stunning survival experience across diverse mobile hardware. This audit evaluates every subsystem of the existing project against mobile engineering constraints:
- Variable aspect ratios (from ultra-wide 21:9 smartphones to 4:3 tablets).
- Display cutouts, notches, dynamic islands, and home indicators.
- Multi-touch responsiveness without input conflicts or drift.
- Thermal saturation, battery consumption, and memory ceilings.
- Unified stylized 3D animated cartoon-realism preservation.

---

### 2. Unity 6 Engine & Rendering Pipeline Audit

```
┌─────────────────────────────────┬────────────────────────────────────────────────────────┐
│ Parameter                       │ Configuration Status & Mobile Evaluation               │
├─────────────────────────────────┼────────────────────────────────────────────────────────┤
│ Unity Editor Version            │ Unity 6 (6000.0.32f1 LTS project config / 6000.6.7f1)  │
│ Render Pipeline                 │ Universal Render Pipeline (URP 17.0.3)                 │
│ Color Space                     │ Linear (Required for physically plausible PBR)        │
│ Rendering Path                  │ Forward+ Clustered Lighting (Scalable on mobile TBDR)  │
│ SRP Batcher                     │ Enabled (All custom shaders use UnityPerMaterial)      │
│ GPU Resident Drawer             │ Enabled (Caches static environment meshes in GPU VRAM) │
│ Default Screen Orientation      │ Landscape (Auto-rotation allowed between Left & Right) │
│ Portrait Allowed                │ False (Locked to prevent accidental vertical flips)   │
└─────────────────────────────────┴────────────────────────────────────────────────────────┘
```

#### Render Quality Profiles Configured:
Managed dynamically by [`GraphicsSettingsController.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Settings/GraphicsSettingsController.cs):
1. **Low Quality Profile:** Render scale `0.70x`, 512 shadowmap, 1 shadow cascade (25m), 0x MSAA, LOD Bias `0.70`, particle multiplier `0.30x`. Target: 30 FPS on entry-tier devices.
2. **Medium Quality Profile:** Render scale `0.85x`, 1024 shadowmap, 2 shadow cascades (45m), 2x MSAA, LOD Bias `1.00`, particle multiplier `0.70x`. Target: 45–60 FPS on mainstream devices.
3. **High Quality Profile:** Render scale `1.00x` native, 2048 shadowmap, 4 shadow cascades (75m), 4x MSAA, LOD Bias `1.50`, SSAO half-res enabled. Target: 60 FPS on flagship devices.
4. **Custom Profile:** User overrides in Settings for fine-grained frame rate (30/45/60/90/120) and graphic toggles.

---

### 3. Screen Dimensions & Aspect Ratio Compatibility

Mobile displays feature extensive aspect ratio diversity. The project must support:

| Form Factor / Ratio | Typical Devices | Resolution Example | Critical Layout Risk |
| :--- | :--- | :--- | :--- |
| **16:9 Standard** | Older phones, budget Android | 1920x1080, 1280x720 | Baseline reference resolution; zero horizontal stretching. |
| **18:9 (2:1)** | Pixel 2 XL, LG G6, Galaxy A-series | 2160x1080, 1440x720 | Extra horizontal space; outer buttons must anchor outward. |
| **19.5:9 Notch / Island** | iPhone 12–16, Galaxy S21–S24 | 2532x1170, 2340x1080 | Hardware notch or Dynamic Island on left/right edges cuts into HUD. |
| **20:9 Ultra-Tall** | Xiaomi, OnePlus, Motorola | 2400x1080 | Severe reachability distance for thumb controls; require inward padding. |
| **21:9 Cinematic** | Sony Xperia 1 / 5 / 10 | 2520x1080 | Extreme aspect ratio; danger of letterbox dead zones or edge clipping. |
| **4:3 / 3:2 Tablet** | iPad 9/10, iPad Air, iPad Pro | 2048x1536, 2388x1668 | **Highest risk of vertical UI compression!** Top HUD and bottom controls crowd center gameplay. |

---

### 4. Canvas Scaler & Safe Area System Audit

#### Current Safe Area Implementation:
- Governed by [`SafeAreaFitter.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Mobile/SafeAreaFitter.cs):
  - Queries `Screen.safeArea` and normalizes against `Screen.width` and `Screen.height`.
  - Sets `anchorMin` and `anchorMax` on the target `RectTransform`.
  - Continuously monitors screen size deltas in `Update()`.

#### Audit Findings:
1. **Strengths:** Dynamically adjusts if device rotates 180 degrees between Landscape Left and Landscape Right.
2. **Deficiencies Identified:**
   - Background graphic elements (sunset sky, vignettes, fullscreen dimmers) must **not** be constrained by the safe area; they must stretch edge-to-edge to prevent black letterbox bars.
   - Interactive UI controls (joystick, action buttons, health gauges) **must** sit inside the safe area container.
   - The project currently lacks a dual-canvas hierarchy separating *Background/VFX Canvas* from *Interactive Safe Area Canvas*.

---

### 5. Mobile Input & Multi-Touch System Audit

#### Subsystem Breakdown:
1. **Virtual Joystick ([`VirtualJoystick.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Mobile/VirtualJoystick.cs)):**
   - Utilizes `IPointerDownHandler`, `IDragHandler`, `IPointerUpHandler`.
   - Tracks `_activePointerId` to isolate multi-touch fingers.
   - Smooth deadzone clamp (`0.10f`) and maximum radius clamp (`75px`).
2. **Camera Drag Look Area ([`MobileCameraLook.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Mobile/MobileCameraLook.cs)):**
   - Dedicated right-side touch area.
   - Calculates pointer delta per frame and resets in `LateUpdate()`.
   - Independent horizontal (`1.8f`) and vertical (`1.4f`) sensitivity with Y-inversion toggle.
3. **Action Buttons ([`MobileActionButton.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Mobile/MobileActionButton.cs)):**
   - Provides Fire, Aim ADS, Sprint, Reload, Swap Weapon, Interact, and Inventory triggers.
4. **Input Router ([`MobileInputManager.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Mobile/MobileInputManager.cs)):**
   - Centralizes touch inputs and feeds `PlayerMovementController` and `PlayerCombatController`.

#### Deficiencies Identified:
- In `MobileInputManager.cs` lines 68–73, anonymous delegate unsubscriptions (`OnTriggered -= id => ...`) fail to unsubscribe, creating subtle event listener leaks.
- On smaller phone screens (< 6.0 inches), the camera look touch surface partially overlaps the reload and aim buttons, risking accidental weapon firing during camera rotation.

---

### 6. Shaders, Materials & Mobile GPU Performance

```
┌──────────────────────────────────────┬────────────────────────┬────────────────────────────────────────┐
│ Shader Asset                         │ Target Pass            │ Mobile SRP Batcher Compatibility       │
├──────────────────────────────────────┼────────────────────────┼────────────────────────────────────────┤
│ StylizedCharacterPBR.shader          │ UniversalForward       │ COMPLIANT (CBUFFER_START UnityPerMat)  │
│ StylizedInfectedPBR.shader           │ UniversalForward       │ COMPLIANT (Includes toxic green glow)  │
│ RoadAsphaltPBR.shader                │ UniversalForward       │ COMPLIANT (Detail normal & puddles)    │
│ WeatheredBuildingPBR.shader          │ UniversalForward       │ COMPLIANT (Vertex color grunge blend)  │
│ CharacterEyePBR.shader               │ UniversalForward       │ COMPLIANT (Cornea refraction)          │
│ CharacterSkinPBR.shader              │ UniversalForward       │ COMPLIANT (Subsurface wrap)            │
└──────────────────────────────────────┴────────────────────────┴────────────────────────────────────────┘
```

- **Draw Call Efficiency:** Because all shaders encapsulate properties inside `CBUFFER_START(UnityPerMaterial)`, the URP SRP Batcher collapses static and dynamic draw calls into single instanced command buffers, eliminating draw-call bottlenecks on Mali and Adreno GPUs.
- **Texture Compression Standard:** Requires ASTC block compression (6x6 for albedos, 4x4 for normal maps) to minimize bandwidth and memory bus saturation.

---

### 7. Audio, Memory & Scene Management

1. **Audio Sources:** Sound effects utilize Vorbis compression with sample rate set to 22,050 Hz or 44,100 Hz. Mobile audio channels should be capped at 24 concurrent voices to preserve CPU audio mixing overhead.
2. **Memory Ceiling:** Total runtime memory allocation on mobile must not exceed **650 MB** on Medium devices (preventing Android OS low-memory killer / LMK daemon eviction).
3. **Garbage Collection:** Zero allocations in `Update()`, `LateUpdate()`, or `FixedUpdate()` across all player, AI, and UI controllers.
