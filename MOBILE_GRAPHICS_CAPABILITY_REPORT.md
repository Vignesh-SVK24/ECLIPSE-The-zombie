# ECLIPSE: THE LAST CIVILIZATION
## Mobile Graphics Capability, Device Profiling & Quality Profiles Report

**Developer:** SVK Creationz  
**Game Engine:** Unity 6 (6000.0.32f1 LTS)  
**Render Pipeline:** Universal Render Pipeline (URP 17.0.3)  
**Target Hardware:** Android (Vulkan API 26–34) & iOS (Metal), with scalable PC/Editor support  
**Document Classification:** Mobile Graphics Architecture & Runtime Performance Specification  

---

### 1. Mobile Hardware Landscape & Target Device Tiers

To ensure *ECLIPSE: THE LAST CIVILIZATION* runs smoothly across the fragmented mobile ecosystem, devices are classified into three distinct architectural tiers:

```
                            MOBILE DEVICE PROFILE MATRIX
                                         │
        ┌────────────────────────────────┼────────────────────────────────┐
        ▼                                ▼                                ▼
  TIER 1: ENTRY MOBILE             TIER 2: MAINSTREAM              TIER 3: FLAGSHIP & TABLETS
  (Low Quality Preset)             (Medium Quality Preset)         (High Quality Preset)
  ─────────────────────            ───────────────────────         ──────────────────────────
  • Snapdragon 765G / 778G         • Snapdragon 870 / 888          • Snapdragon 8 Gen 1 / 2 / 3
  • Adreno 620 / 642L              • Adreno 660                    • Adreno 730 / 740 / 750
  • MediaTek Dimensity 800/900     • Dimensity 1200 / 8100         • Dimensity 9000 / 9200 / 9300
  • Mali-G57 / G68                 • Mali-G77 / G710               • Mali-G715 / Immortalis-G720
  • Apple A12 / A13 (iPhone 11)    • Apple A14 / A15 (iPhone 13)   • Apple A16 / A17 Pro / M1-M4
  • 4 GB – 6 GB RAM                • 6 GB – 8 GB RAM               • 8 GB – 16 GB RAM
  • Target: 30 FPS Rock Solid      • Target: 45–60 FPS Stable      • Target: 60–90 FPS Photoreal
```

---

### 2. Unity 6 URP Graphics Pipeline Architecture for Mobile

#### A. Forward+ Rendering Path
- Traditional multi-pass forward rendering requires a separate draw call for every light hitting an object, causing catastrophic draw-call bloat on mobile.
- **Forward+ in URP 17:** Pre-clusters all scene lights (streetlamps, fires, muzzle flashes, searchlights) into camera frustum tiles in a single depth pre-pass. Opaque geometry is shaded in a single pass while evaluating up to **16 concurrent lights per cluster** with zero additional draw calls.

#### B. Tile-Based Deferred Rendering (TBDR) Optimization
- Mobile GPUs (Adreno, Mali, Apple Silicon) render geometry within on-chip **tile memory** (16x16 or 32x32 pixel tiles).
- In URP 17's **Render Graph**, color buffers and depth buffers remain in high-speed on-chip tile SRAM during intermediate passes, completely avoiding bandwidth-heavy flushes to slow external LPDDR RAM.

#### C. GPU Resident Drawer & SRP Batcher
- Static building pieces, sidewalk sections, road segments, and vehicle wrecks are retained permanently in GPU memory.
- All custom environment shaders (`RoadAsphaltPBR.shader`, `WeatheredBuildingPBR.shader`, `CharacterSkinPBR.shader`) implement `CBUFFER_START(UnityPerMaterial)`, ensuring draw calls are collapsed into single instanced draw streams without CPU pipeline stalling.

---

### 3. Detailed Graphics Quality Profiles: Low, Medium, High & Custom

The project features four distinct runtime graphics configurations managed by [`GraphicsSettingsController.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Settings/GraphicsSettingsController.cs):

| Feature / Setting | Low Preset (Entry) | Medium Preset (Mainstream) | High Preset (Flagship) | Custom Preset (User Overrides) |
| :--- | :---: | :---: | :---: | :---: |
| **Target Frame Rate** | 30 FPS | 60 FPS (or 45 FPS) | 60 FPS (90/120 on ProMotion) | Player Selectable: 30 / 45 / 60 / 90 / 120 |
| **Internal Render Scale** | `0.70x` (720p base) | `0.85x` (900p base) | `1.00x` (Native 1080p+) | Player Slider: `0.50x` to `1.20x` |
| **Anti-Aliasing (MSAA)** | Off (Fast FXAA) | 2x MSAA | 4x MSAA | Off / 2x / 4x MSAA / SMAA |
| **Anisotropic Filtering** | Disabled | 2x Forced | 4x / 8x Forced | Disabled / 2x / 4x / 8x |
| **Shadow Casting** | Directional Sun Only | Sun + Key Lights | All Punctual Lights | Off / Directional / All Lights |
| **Shadow Distance** | 25.0 meters | 45.0 meters | 75.0 meters | Slider: 15.0m to 100.0m |
| **Shadow Cascades** | 1 (No Cascades) | 2 Cascades | 4 Cascades | 1 / 2 / 4 Cascades |
| **Shadowmap Resolution** | 512 x 512 | 1024 x 1024 | 2048 x 2048 | 512 / 1024 / 2048 |
| **Shadow Soft Filtering** | Hard Shadows | Soft Shadows (PCF 3x3) | Soft Shadows (PCF 5x5) | Hard / Soft PCF |
| **LOD Bias** | `0.70` (Aggressive LOD) | `1.00` (Balanced) | `1.50` (Extended Range) | Slider: `0.50` to `2.00` |
| **Vegetation Density** | 40% (Cull weeds/grass) | 75% | 100% Full Overgrowth | Slider: 20% to 100% |
| **Particle Budget Multiplier** | 0.30x | 0.70x | 1.00x | Slider: 0.20x to 1.50x |
| **Screen Space Ambient Occlusion (SSAO)** | Disabled | Disabled | Enabled (Half-Res) | Toggle: On / Off |
| **Bloom & Lens Dirt** | Disabled | Enabled (Fast Gaussian) | Enabled (Bicubic High) | Toggle: On / Off |
| **Volumetric Ground Fog** | Disabled | Simple Planar Fog | Volumetric Height Fog | Off / Planar / Volumetric |
| **Texture Quality (Streaming)** | Half-Resolution (Mip 1) | Full-Resolution | Full-Resolution 16x Aniso | Quarter / Half / Full |

---

### 4. Adaptive Performance & Thermal Throttling Mitigation

Mobile devices experience thermal saturation after 10–15 minutes of intensive 3D rendering. When thermal headroom is exceeded, mobile OS kernels aggressively throttle CPU and GPU frequencies by up to 50%, resulting in severe stuttering.

#### The ECLIPSE Adaptive Performance Strategy
By integrating `com.unity.adaptiveperformance` and `com.unity.adaptiveperformance.samsung.android`:

1. **Thermal Headroom Monitoring:**
   - Real-time queries to hardware thermal sensors detect thermal escalation *before* OS throttling occurs.
2. **Dynamic Resolution Scaling (DRS / STP):**
   - If thermal warning level `Level 2 (Throttling Imminent)` is triggered:
     - Render scale dynamically scales down by `0.05` increments (e.g., from `1.0x` down to `0.80x`).
     - Spatial-Temporal Post-Processing (STP / FSR 1.0) upscales the buffer back to screen resolution with zero visual blur.
3. **Graceful Quality Stepping:**
   - Drops shadow cascades from 4 to 2.
   - Reduces particle emitter spawn rates by 35%.
   - Culls decorative micro-debris and distant grass clumps.
   - Returns to full settings automatically once temperature stabilizes below normal threshold.

---

### 5. Mobile Memory, Bandwidth & Poly Budgets

To prevent out-of-memory (OOM) crashes on 4 GB and 6 GB Android devices, the game adheres to the following strict memory ceilings:

```
                      ECLIPSE MOBILE RAM/VRAM ALLOCATION (HIGH PROFILE)
┌──────────────────────────────────────┬────────────────────────┬─────────────┐
│ Subsystem                            │ Memory Cap             │ Percentage  │
├──────────────────────────────────────┼────────────────────────┼─────────────┤
│ Environment Textures (ASTC Compressed│ 320 MB                 │ 42.6%       │
│ Character & Zombie Textures          │ 140 MB                 │ 18.7%       │
│ Weapon & Item Textures               │ 50 MB                  │ 6.7%        │
│ UI Atlases & Font SDFs               │ 40 MB                  │ 5.3%        │
│ Geometry & Vertex Buffers            │ 65 MB                  │ 8.7%        │
│ Audio Clips & Ambiance Loops         │ 45 MB                  │ 6.0%        │
│ Render Targets & Transient Buffers   │ 90 MB                  │ 12.0%       │
├──────────────────────────────────────┼────────────────────────┼─────────────┤
│ TOTAL GRAPHICS & ENGINE FOOTPRINT    │ 750 MB (Max allowable) │ 100.0%      │
└──────────────────────────────────────┴────────────────────────┴─────────────┘
```

#### Frame-Time & Draw Call Ceilings
- **Low Profile:** < 80 Draw Calls, < 80,000 Triangles/frame, < 33.3ms frame time (30 FPS).
- **Medium Profile:** < 120 Draw Calls, < 150,000 Triangles/frame, < 16.6ms frame time (60 FPS).
- **High Profile:** < 180 Draw Calls, < 250,000 Triangles/frame, < 16.6ms frame time (60 FPS).
