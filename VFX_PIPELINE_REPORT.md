# ECLIPSE: THE LAST CIVILIZATION
## Environmental VFX Production Pipeline & Particle Systems Report

**Developer:** SVK Creationz  
**Game Engine:** Unity 6 (6000.0.32f1 LTS)  
**Render Pipeline:** Universal Render Pipeline (URP 17.0.3)  
**VFX Systems:** Unity Visual Effect Graph (GPU Compute) + Shuriken Particle System (Hybrid Architecture)  
**Target Platforms:** Android (Vulkan API 26–34), iOS (Metal), PC / Editor  
**Document Classification:** Technical Architecture & Visual Effects Specification  

---

### 1. Executive VFX Strategy & Hybrid Architecture

Visual effects in *ECLIPSE: THE LAST CIVILIZATION* are critical to communicating the bleak, atmospheric horror of Greyfall County. Rather than relying on a single particle engine, the project implements a **hybrid dual-engine architecture**:

```
                         ECLIPSE VFX HYBRID ARCHITECTURE
                                        │
           ┌────────────────────────────┴────────────────────────────┐
           ▼                                                         ▼
   VISUAL EFFECT GRAPH                                    SHURIKEN PARTICLE SYSTEM
   (GPU Compute Driven)                                   (CPU / Threaded Simulation)
   ───────────────────────────                            ───────────────────────────
   • High-Volume Environmental Sim                        • Interactive Gameplay Events
   • 10,000+ Particles on Screen                          • Low-Volume Physical Collisions
   • Airborne Dust & Spore Motes                          • Bullet Impacts & Concrete Debris
   • Persistent Ground Fog Sheets                         • Blood Splatter & Hit Reactions
   • Heavy Rain & Splashing Droplets                      • Muzzle Flashes & Shell Ejections
   • Swirling Burning Embers                              • Explosive Barrel Detonations
```

#### Why Hybrid on Mobile?
- **Unity 6 VFX Graph on Mobile:** Unity 6 officially supports VFX Graph on modern mobile APIs (**Vulkan** on Android and **Metal** on iOS) via GPU compute shaders. GPU particles offload particle positioning, velocity, and turbulence noise from the mobile CPU.
- **Shuriken for Gameplay Events:** Gameplay-triggered events (gunfire, melee impacts, explosive barrels) require tight synchronization with C# combat callbacks, PhysX raycasts, and audio triggers (`Eclipse.Damage`, `Eclipse.Combat`). Shuriken handles these lightweight bursts with minimal latency.

---

### 2. Environmental VFX Profiles for Greyfall County (2038)

#### A. Persistent Ground Fog Sheets (`VFX_Env_GroundFog`)
- **Visual Role:** Low-lying, eerie ground fog that hugs the wet asphalt, alleyways, and drainage curbs, evoking the dread of lurking infected stalkers.
- **Implementation:** VFX Graph quad strip emitter with multi-octave Perlin curl noise vector field.
- **Shader Technique:** Soft particle depth fading (`_CameraDepthTexture`) to eliminate hard geometry intersections against curbs, tires, and barricades.
- **Mobile Budget:** Max 64 simultaneous billboard quads with large radius (`8m` - `12m`), slow drift velocity (`0.15 m/s`), and low opacity (`0.08 - 0.15`).

#### B. Atmospheric Sunbeam Dust & Spore Motes (`VFX_Env_DustMotes`)
- **Visual Role:** Fine airborne dust particles, pollen, and airborne fungal spores suspended in volumetric light shafts filtering between multi-story buildings.
- **Implementation:** GPU point/quad particles driven by low-frequency Brownian turbulence.
- **Particle Count:**
  - High (Mobile) / PC: 4,000 particles in a `20m x 20m x 10m` bounding volume around the player camera.
  - Medium (Mobile): 1,500 particles.
  - Low (Mobile): 500 particles.
- **Color Grading:** Soft warm tint in sunlight shafts; cold sickly green in infected nests.

#### C. Burning Vehicle Plume & Embers (`VFX_Env_VehicleFirePlume`)
- **Visual Role:** Smoldering evacuation wrecks and military roadblocks burning after zombie breaches.
- **Component Decomposition:**
  1. **Fire Core:** 4-frame animated flame flipbook with HDR additive emission, illuminating nearby vehicle geometry.
  2. **Thick Black Oil Smoke:** Dense billowing dark gray/black smoke ascending `15m` before dissipating into overcast cloud cover.
  3. **Floating Burning Embers:** 200–500 micro-particles rising along the smoke thermal updraft with erratic curl noise and fading lifetime.
  4. **Heat Distortion / Refraction:** Screen-space refraction normal quad placed above the fire core simulating thermal air shimmer.

#### D. Dynamic Precipitation & Wetness Ripples (`VFX_Env_RainStorm`)
- **Visual Role:** Heavy autumnal rainfall soaking the abandoned streets of Greyfall.
- **Component Decomposition:**
  1. **Falling Rain Streaks:** Camera-relative particle box emitter. Rain streaks stretch along the velocity vector (`Length Scale: 3.5`).
  2. **Pavement & Car Splashes:** Inverted cone splash bursts emitting when raindrops strike horizontal surfaces (driven by depth collision in VFX Graph).
  3. **Puddle Concentric Ripples:** Animated normal-map decal shader blending circular wave ripples into the asphalt puddle mask.
  4. **Camera Lens Droplet FX:** Screen-space moisture beads that slide down the screen during third-person and first-person exploration.

#### E. Electrical Fault Arc Sparks (`VFX_Env_ElectricalSparks`)
- **Visual Role:** Damaged municipal infrastructure (severed streetlamp cables, damaged transformer boxes) intermittently sparking in the rain.
- **Implementation:** Shuriken burst emitter with gravity, bouncing off concrete surfaces via PhysX plane collision.
- **Light Coupling:** Synchronized point light emitting a `0.05s` cyan/blue flash with shadow casting disabled to prevent mobile draw call spikes.

#### F. Wind-Blown Urban Debris (`VFX_Env_StreetDebris`)
- **Visual Role:** Dry fallen leaves, torn newspapers, and plastic food wrappers skittering across the cracked asphalt in autumn gusts.
- **Implementation:** Low-poly 3D mesh particles (single quad or 6-poly curled leaf) with angular velocity and ground plane clamping.

---

### 3. Particle Shading & Overdraw Mitigation Strategy

Mobile GPUs (Mali, Adreno, PowerVR) are heavily fill-rate and bandwidth constrained. **Overdraw** (rendering multiple transparent particle layers over the same screen pixels) is the primary cause of mobile frame rate drops.

#### Overdraw Prevention Rules
1. **Tight Mesh Particle Geometry:**
   - Instead of rectangular quads with large transparent borders, smoke and fire sprites use tightly trimmed 8-vertex hexagonal particle meshes, eliminating up to **60% of transparent pixel shading**.
2. **Soft Depth Fading:**
   - Every transparent particle shader computes depth distance from the camera depth buffer:
     $$\text{Opacity} = \text{Saturate}\left(\frac{\text{SceneDepth} - \text{ParticleDepth}}{\text{FadeDistance}}\right)$$
   - Prevents sharp cutoffs when particles touch asphalt, buildings, or vehicle hulls.
3. **Half-Resolution Particle Rendering (URP Feature):**
   - On Low and Medium mobile profiles, heavy transparent particle systems (dense smoke and fog) are rendered into a quarter-resolution or half-resolution off-screen buffer before being composited back into the full-resolution frame.
4. **Distance-Based Particle Culling & LODs:**
   - Emitters include explicit `CullingGroup` logic. When an emitter is beyond `40m` from the player camera, particle spawn rates drop by 75%; beyond `75m`, emitters are completely sleeping.

---

### 4. Technical Performance Budgets by Quality Tier

| VFX System | Low Profile (Mobile 30 FPS) | Medium Profile (Mobile 60 FPS) | High Profile (Mobile 60 FPS) | PC / Editor Preset |
| :--- | :---: | :---: | :---: | :---: |
| **Max Active GPU Particles** | 1,000 | 3,500 | 8,000 | 25,000 |
| **Max Active CPU Particles** | 150 | 350 | 750 | 2,000 |
| **Volumetric Ground Fog** | Disabled | 16 Quads (Local) | 48 Quads (Volume) | Full VFX Graph Sheet |
| **Atmospheric Dust Motes** | 250 Points | 1,000 Points | 3,000 Points | 10,000 Points |
| **Rain Emitter Density** | 400 Drops/sec | 1,200 Drops/sec | 2,500 Drops/sec | 6,000 Drops/sec |
| **Heat Distortion Refraction** | Disabled | Disabled | Enabled | Enabled |
| **Transparent Particle Screen Coverage** | < 1.5x Overdraw | < 2.5x Overdraw | < 4.0x Overdraw | < 6.0x Overdraw |

---

### 5. Implementation Roadmap & Package Integration

To activate the GPU Visual Effect Graph pipeline:
1. Include `"com.unity.visualeffectgraph": "17.0.3"` in `Packages/manifest.json`.
2. Configure URP Asset with `VFX Runtime: Enabled` and assign the Default VFX Material.
3. Establish template VFX assets under `Assets/ECLIPSE/Environment/VFX/`:
   - `VFX_Fog_GroundRolling.vfx`
   - `VFX_Atmosphere_DustMotes.vfx`
   - `VFX_Fire_VehiclePlume.vfx`
   - `VFX_Weather_RainStorm.vfx`
   - `VFX_Debris_WindLeaves.vfx`
