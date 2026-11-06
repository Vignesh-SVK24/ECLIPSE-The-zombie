# ECLIPSE: THE LAST CIVILIZATION
## iOS & iPadOS Optimization & Build Architecture Plan

**Studio:** SVK Creationz  
**Engine:** Unity 6 (6000.0.32f1 LTS)  
**Render Pipeline:** Universal Render Pipeline (URP 17.0.3)  
**Target Hardware:** iPhone (A13 Bionic to A18 Pro), iPad, iPad Air & iPad Pro (M1–M4 Apple Silicon)  
**Document Ref:** `IOS_OPTIMIZATION_PLAN.md`  

---

### 1. iOS Platform Specifications & Player Settings

Configured via [`EclipseBuildAutomation.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Editor/BuildPipeline/EclipseBuildAutomation.cs):

```
┌─────────────────────────────────┬────────────────────────────────────────────────────────┐
│ Parameter                       │ Configuration & Apple Compliance                       │
├─────────────────────────────────┼────────────────────────────────────────────────────────┤
│ Bundle Identifier               │ com.svkcreationz.eclipse                               │
│ Product Name                    │ ECLIPSE: THE LAST CIVILIZATION                         │
│ Scripting Backend               │ IL2CPP (Required by Apple App Store)                   │
│ Target Architecture             │ ARM64 Only                                             │
│ Target SDK                      │ Device SDK (iOS 15.0+ Minimum Deployment Target)       │
│ Graphics API                    │ Metal (Strictly enforced; OpenGLES deprecated by Apple)│
│ Default Interface Orientation   │ Landscape Left & Landscape Right                       │
│ Auto-Rotation Portrait          │ Disabled (Locks orientation to horizontal gameplay)    │
│ Apple Silicon Support           │ Native execution on M-series iPads                     │
│ Xcode Output Path               │ Builds/iOS/ECLIPSE-iOS-Xcode-Project/                  │
└─────────────────────────────────┴────────────────────────────────────────────────────────┘
```

---

### 2. Apple Metal Graphics Architecture in Unity 6

Apple Silicon GPUs (A-Series and M-Series) utilize high-performance **Tile-Based Deferred Rendering (TBDR)** with massive on-chip tile memory.

1. **Metal Memory Aliasing (Render Graph):**
   - In URP 17, intermediate depth buffers and shadow textures share memory slots on-chip, preventing bandwidth spikes to system unified memory.
2. **Forward+ Clustered Lighting:**
   - Evaluates up to 16 concurrent punctual lights (torches, muzzle flashes, streetlights) in a single pass using Metal compute shaders.
3. **ASTC Texture Decompression:**
   - All textures compress to ASTC natively supported by Apple GPU silicon with zero runtime conversion penalty.

---

### 3. Display Insets & Safe Area Management (HIG Compliance)

Apple devices feature aggressive physical screen cutouts that must be respected per the **Apple Human Interface Guidelines (HIG)**:

```
                            IPHONE NOTCH & DYNAMIC ISLAND INSETS
┌────────────────────────────────────────────────────────────────────────────────────────┐
│ [Dynamic Island / Notch]                                          [Battery / Status]   │
│  <── 59 pt Inset ──>                                               <── 44 pt Inset ──>  │
│                                                                                        │
│                                VISIBLE GAMEPLAY VIEWPORT                               │
│                                                                                        │
│                                  [Home Indicator Bar]                                  │
│                                  <── 34 pt Inset ──>                                   │
└────────────────────────────────────────────────────────────────────────────────────────┘
```

#### Safe Area Protection Rules:
1. **Left Thumb Zone:** The Virtual Joystick center is clamped inward by at least `60pt` from the left edge so fingers never touch the curved display bezel or Dynamic Island.
2. **Right Combat Cluster:** Fire and Aim buttons are offset inward from the right corner cutout.
3. **Bottom Home Indicator:** Bottom HUD elements and touch look drag zones maintain a `34pt` bottom clearance to prevent accidental iOS app-switching gestures during intense zombie encounters.
4. **Fullscreen Backgrounds:** Background sky, fog volumes, and world geometry render edge-to-edge behind the notch (`viewport-fit=cover`).

---

### 4. ProMotion (120Hz) & Thermal Throttling Strategy

#### Frame Rate Modes:
- **iPhone Standard (60Hz Display):** Locked to 60 FPS (Quality) or 30 FPS (Battery Saver).
- **iPad Pro / iPhone Pro (ProMotion 120Hz Display):**
  - Managed by [`FrameRateManager.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Settings/FrameRateManager.cs).
  - Users can select **120 FPS High Performance** mode on M-series iPads.

#### iOS Thermal State Management (`NSProcessInfoThermalState`):
- **Nominal / Fair:** Full graphics settings and target frame rate.
- **Serious:** Automatic stepping: target frame rate drops from 120/60 FPS to 45 FPS; shadow cascades step down from 4 to 2.
- **Critical:** Drops target to 30 FPS, disables SSAO, reduces render scale by 15% to prevent device shutdown.

---

### 5. Application Lifecycle & Interruption Handling

1. **Phone Call & Notification Interruption:**
   - When `OnApplicationPause(true)` or `OnApplicationFocus(false)` triggers:
     - Game auto-pauses and brings up `UI_PauseMenu`.
     - Audio mixer pauses master channels smoothly to prevent audio buzzing.
     - Multiplayer connection sends heartbeat keep-alive or invokes `NetworkReconnectManager`.
2. **Resume Recovery:**
   - Re-queries `Screen.safeArea` in case the device was unlocked in a different orientation.
   - Clears active pointer IDs in `MobileInputManager` to prevent stuck virtual joystick inputs.

---

### 6. Xcode Export & Build Protocol

```
DEVELOPMENT HOST (Windows 11)              MAC WORKSTATION (macOS Sonoma / Sequoia)
─────────────────────────────              ─────────────────────────────────────────
1. Run Unity 6 iOS Export                  3. Open in Xcode 16
   • EclipseBuildAutomation.cs             4. Link Apple Developer Team & Provisioning
2. Generates Xcode Project at:             5. Connect iPhone / iPad via USB-C or Wi-Fi
   Builds/iOS/ECLIPSE-iOS-Xcode-Project/   6. Deploy to TestFlight or On-Device Debug
```

- **Apple Privacy Manifest:** Includes `PrivacyInfo.xcprivacy` declaring UserDefaults and system boot time APIs per Apple 2024 App Store submission rules.
