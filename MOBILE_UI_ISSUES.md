# ECLIPSE: THE LAST CIVILIZATION
## Mobile UI & Responsiveness Defect Audit & Issue Tracker

**Studio:** SVK Creationz  
**Engine:** Unity 6 + URP  
**Target Form Factors:** Android Smartphones & Tablets, iPhone & iPad  
**Document Ref:** `MOBILE_UI_ISSUES.md`  

---

### 1. Executive Summary & Defect Classification

This document catalogs all identified UI responsiveness defects, aspect ratio distortions, safe area clipping bugs, touch collision issues, and mobile performance bottlenecks currently present in the prototype.

Every defect is classified by severity:
- **Critical (P0):** Prevents core gameplay, blocks touch inputs, or causes app termination.
- **Major (P1):** Severe visual clipping, overlapping interactive buttons, or unreadable typography.
- **Minor (P2):** Aesthetic misalignment, imperfect padding, or slight aspect ratio letterboxing.

---

### 2. Comprehensive Mobile UI Defect Log

```
┌──────┬──────────┬───────────────────────────────┬─────────────────────────────────────────────────────────────────┐
│ ID   │ Severity │ Subsystem / Screen            │ Defect Description & Remediation Requirement                    │
├──────┼──────────┼───────────────────────────────┼─────────────────────────────────────────────────────────────────┤
│ UI-01│ CRITICAL │ Mobile Input Manager          │ Anonymous delegate leak in MobileInputManager.OnDisable()       │
│ UI-02│ CRITICAL │ Safe Area / Display Inset     │ Notch & Dynamic Island clip into top-left vitals on 19.5:9      │
│ UI-03│ MAJOR    │ 4:3 Aspect Ratio (iPads)      │ Vertical UI crowding: HUD vitals and touch controls overlap     │
│ UI-04│ MAJOR    │ Touch Controls (Right Thumb)  │ Camera drag look area overlaps reload and aim action buttons   │
│ UI-05│ MAJOR    │ WebGL / Browser Client        │ Missing viewport-fit=cover causing white bars on iOS Safari     │
│ UI-06│ MAJOR    │ WebGL Touch Gestures          │ Missing touch-action: none allows pinch-zoom & pull-to-refresh  │
│ UI-07│ MINOR    │ Canvas Scaler Configuration   │ Fixed font sizes cause ammo numbers to clip on 5.8" phones      │
│ UI-08│ MINOR    │ Virtual Joystick Centering    │ Non-dynamic joystick center unreachable on tall 20:9 phones     │
│ UI-09│ MINOR    │ UI Overdraw & Canvas Rebuilds │ Fullscreen canvas dirtying on health ticks lowers mobile FPS   │
│ UI-10│ MINOR    │ Interactive Reticle Scaling   │ Dynamic crosshair appears oversized on high-DPI retina screens  │
└──────┴──────────┴───────────────────────────────┴─────────────────────────────────────────────────────────────────┘
```

---

### 3. Deep-Dive Defect Analysis & Root Causes

#### UI-01 [CRITICAL]: Anonymous Delegate Event Leak in `MobileInputManager.cs`
- **Location:** [`MobileInputManager.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Mobile/MobileInputManager.cs#L68-L73)
- **Root Cause:**
  ```csharp
  // DEFECTIVE: Anonymous lambda creates new instance; fails to unsubscribe old listener!
  _reloadButton.OnTriggered -= id => OnReloadAction?.Invoke();
  ```
- **Consequence:** Every scene reload or canvas toggle retains orphaned delegate references, causing cumulative garbage collection spikes and duplicate action triggers.
- **Fix Required:** Replace with named private event handlers (`HandleReloadTriggered`).

#### UI-02 [CRITICAL]: Notch & Camera Cutout Clipping on 19.5:9 Smartphones
- **Devices Affected:** iPhone 12–16 (Notch & Dynamic Island), Samsung Galaxy S21–S24 (Hole-punch cameras).
- **Symptom:** The top-left survivor portrait and health bar are partially obscured by the camera cutout when held in Landscape Left; the minimap and mission tracker are cut off when held in Landscape Right.
- **Fix Required:** Ensure all HUD panels anchor to an internal `SafeAreaContainer` governed by [`SafeAreaFitter.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Mobile/SafeAreaFitter.cs).

#### UI-03 [MAJOR]: 4:3 Aspect Ratio Vertical Crowding on Tablets (iPad)
- **Devices Affected:** iPad 9th/10th Gen, iPad Air, iPad Pro.
- **Symptom:** Tablets have significantly narrower horizontal aspect ratios (1.33:1 vs 2.16:1 on phones). The top survivor status panel and bottom touch buttons crowd into the center third of the screen, severely obstructing combat visibility.
- **Fix Required:** Deploy dynamic device profile layout switching via `RectTransform` presets: on tablet form factors, scale UI elements by `0.85x` and pin closer to screen corners.

#### UI-04 [MAJOR]: Camera Drag Area Overlapping Action Buttons
- **Location:** `MobileCameraLook.cs` and `MobileActionButton.cs`.
- **Symptom:** When players rapidly swipe on the right half of the screen to turn around during zombie pursuit, their thumb frequently clips the edge of the Aim or Reload buttons, triggering accidental reloads mid-sprint.
- **Fix Required:** Add explicit touch filtering: `MobileCameraLook` must reject pointer IDs that originate inside the `RectTransform` bounds of any registered `MobileActionButton`.

#### UI-05 & UI-06 [MAJOR]: WebGL Client Viewport & Gesture Conflicts
- **Location:** `LocalPlayable/index.html` and `docs/index.html`.
- **Symptom:**
  - Safari on iOS displays black or white letterboxing around the notch because `viewport-fit=cover` is omitted.
  - Swiping downward on mobile browsers triggers native pull-to-refresh or page bouncing.
- **Fix Required:**
  - Update meta tag: `<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no, viewport-fit=cover">`.
  - Add CSS rule: `#canvas-container, canvas { touch-action: none; -webkit-touch-callout: none; }`.

#### UI-07 [MINOR]: Font Clipping on Compact Displays
- **Symptom:** Ammunition reserve counters (`/ 120`) and survivor health numbers overflow their parent containers on compact 5.5" to 5.8" Android devices.
- **Fix Required:** Enable TextMeshPro `Auto Sizing` with `Min Size: 10pt` and `Max Size: 18pt`.

#### UI-09 [MINOR]: Fullscreen Canvas Overdraw & Rebuild Spikes
- **Symptom:** Placing dynamic text elements (health numbers, stamina gauges) on the same canvas as static backgrounds causes Unity's uGUI batch generator to rebuild all canvas geometry every frame.
- **Fix Required:** Split UI into a multi-canvas hierarchy:
  1. `Canvas_StaticBackground` (No rebuilds).
  2. `Canvas_DynamicHUD` (Rebuilds only health/ammo meshes).
  3. `Canvas_InteractiveOverlay` (Touch buttons and virtual joysticks).

---

### 4. Implementation & Remediation Roadmap

The remediation of these issues is scheduled systematically across the master batches:

1. **Batch B (Responsive UI & Safe Area):**
   - Implement Multi-Canvas Hierarchy (Background, Safe Area HUD, Modals).
   - Fix notch/island clipping via upgraded `SafeAreaManager`.
   - Implement adaptive layouts for 4:3 tablets vs 21:9 phones.
   - Fix WebGL viewport and touch-action rules.
2. **Batch C (Mobile Touch Controls & Input System):**
   - Fix event delegate leak in `MobileInputManager`.
   - Add touch exclusion zones between camera drag and action buttons.
   - Implement customizable button sizes and opacity sliders.
3. **Batch D & G (Graphics, Memory & Thermal Optimization):**
   - Eliminate canvas overdraw, optimize ASTC texture atlases, and tune URP Forward+ clustered lighting.
