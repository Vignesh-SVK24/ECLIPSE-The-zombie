# ECLIPSE: THE LAST CIVILIZATION
## Graphics Toolchain & Production Pipeline Audit

**Developer:** SVK Creationz  
**Game Engine:** Unity 6 (6000.0.32f1 LTS / Host Installed: 6000.6.7f1)  
**Render Pipeline:** Universal Render Pipeline (URP 17.0.3)  
**Target Platforms:** Android (Vulkan API 26–34) & iOS (Metal), with scalable PC/Editor development  
**Audit Date:** October 2026  
**Document Classification:** Technical Architecture & Production Pipeline Specification  

---

### 1. Executive Summary & Engine Target

*ECLIPSE: THE LAST CIVILIZATION* requires an authentic, photorealistic 3D visual presentation representing the post-apocalyptic ruins of Greyfall County (2038). To achieve AAA visual fidelity on mobile hardware without sacrificing 60 FPS performance, the graphics architecture must leverage the modern features of **Unity 6 URP (17.0.3)**:

- **Forward+ Rendering Path:** Allows dozens of localized real-time lights (flickering streetlamps, emergency flashers, campfire barrels, muzzle flashes) with clustered light culling on mobile GPUs.
- **GPU Resident Drawer:** Drastically reduces CPU draw call preparation overhead by retaining static environment meshes and materials in GPU memory.
- **SRP Batcher Compatibility:** Guarantees unified constant buffer layouts (`UnityPerMaterial`), collapsing hundreds of distinct draw calls into single instanced batches on Vulkan and Metal.
- **Render Graph Architecture:** Automates memory aliasing, transient render target pooling, and tile-based deferral on mobile TBDR (Tile-Based Deferred Rendering) GPUs (Adreno, Mali, Apple Silicon).
- **Adaptive Probe Volumes (APV):** Provides high-density baked indirect lighting with seamless dynamic object integration without the memory footprint of traditional dense lightmap atlases.

> [!IMPORTANT]
> In strict accordance with technical constraints, **the project will remain on URP 17.0.3**. High Definition Render Pipeline (HDRP) is strictly incompatible with mobile hardware targets (Vulkan/Metal on smartphone thermal and memory envelopes). URP 17 delivers photorealistic PBR, volumetric fog, post-processing, and compute-driven VFX while respecting mobile performance ceilings.

---

### 2. Unity Package Architecture Audit

An inspection of `Packages/manifest.json` and the Unity 6 package repository was conducted. Below is the comprehensive status of graphics and environment packages:

| Package | Version | Status | Technical Role & Implementation Requirement |
| :--- | :---: | :---: | :--- |
| **Universal Render Pipeline** (`com.unity.render-pipelines.universal`) | `17.0.3` | **Installed** | Core render pipeline, Forward+ path, SRP Batcher, screen-space ambient occlusion, volumetric fog, post-processing. |
| **Shader Graph** (`com.unity.shadergraph`) | `17.0.3` | **Dependency Active / Target Explicit** | Visual authoring of master PBR materials, vertex wind displacement for vegetation, dynamic wet puddle blending, and parallax interior mapping. |
| **Visual Effect Graph** (`com.unity.visualeffectgraph`) | `17.0.3` | **Target to Add** | GPU-accelerated environmental particle simulations (volumetric smoke, embers, rain streaks, dust motes). Fully supported on mobile compute shaders in Unity 6. |
| **Terrain Tools** (`com.unity.terrain-tools`) | `5.0.3` | **Target to Add** | Advanced terrain sculpting, hydraulic/thermal erosion simulation, stamp brushes, holes for underground bunkers/tunnels, and procedural noise brushes. |
| **Cinemachine** (`com.unity.cinemachine`) | `3.1.2` | **Installed** | Multi-camera orchestration, ADS weapon aiming blending, vehicle follow cameras, and cinematic cutscenes. |
| **Splines** (`com.unity.splines`) | `2.6.1` | **Target to Add** | Procedural road extrusion, sidewalk kerbs, highway guardrails, powerlines, and perimeter chainlink fences. |
| **Addressables** (`com.unity.addressables`) | `2.2.2` | **Installed** | Asynchronous asset loading, memory-budgeted chunk streaming for open-world sectors, and downloadable content management. |
| **Adaptive Performance** (`com.unity.adaptiveperformance`) | `5.1.0` | **Target to Add** | Thermal throttling monitoring, dynamic resolution scaling, and battery optimization on mobile devices. |
| **Adaptive Performance Samsung** (`com.unity.adaptiveperformance.samsung.android`) | `5.1.0` | **Target to Add** | Dedicated hardware integration for Samsung Galaxy devices (GameSDK / Vulkan performance profiling). |
| **Burst Compiler** (`com.unity.burst`) | `1.8.18` | **Installed** | High-performance C# job compilation for procedural geometry and physics raycasts. |
| **Input System** (`com.unity.inputsystem`) | `1.11.2` | **Installed** | Unified cross-platform input handling (touch joystick, gamepad, mouse/keyboard). |
| **Netcode for GameObjects** (`com.unity.netcode.gameobjects`) | `2.2.0` | **Installed** | Multiplayer state replication for 2–4 player co-op. |

---

### 3. External DCC Software & Production Ecosystem Verification

A system-level audit of the development host environment was executed. The verified installation status and pipeline protocols are detailed below:

```
HOST ENVIRONMENT AUDIT RESULTS:
-------------------------------------------------------------------------
Host OS:                     Windows 11
Installed Unity Editor:      6000.6.7f1 (C:\Program Files\Unity\Hub\Editor\6000.6.7f1)
Configured Project Editor:   6000.0.32f1 (Unity 6 LTS)
IDE / Code Tools:            Visual Studio Code (Installed), Git 2.45+ (Installed)
Scripting Runtime:           Python 3.11 (Installed), Node.js v24.13.1 (Installed)
Mobile SDK:                  Android Studio (Installed)
2D Creative Tools:           Figma (Installed), CapCut (Installed), Filmora (Installed)
-------------------------------------------------------------------------
```

#### Application Verification Matrix

| Tool / Ecosystem | Local Host Verification | License / Source Type | Production Role & Pipeline Requirement |
| :--- | :---: | :---: | :--- |
| **Blender (4.2+ LTS)** | **Not Installed on Host** | Free & Open Source (GPL) | **Manual Installation Required.** Core DCC for modular architecture, vehicle wrecks, UV unwrapping (non-overlapping Lightmap UVs), LOD generation, and convex collision hull authoring (`UCX_`). |
| **Adobe Substance 3D Painter** | **Not Installed on Host** | Commercial (Adobe / Steam) | **Manual Installation Required.** Multi-channel PBR texturing, curvature-driven edge wear, rust leaking, grime accumulation, and packed mask generation. |
| **Adobe Photoshop** | **Workflow Available (Not in local Program Files)** | Commercial (Creative Cloud) | Integrated via automated Python/JSX pipeline (`Tools/Photoshop/pack_orm_textures.py`). Used for UI graphics, decal transparency maps, and channel-packed ORM masks. |
| **SpeedTree (Games/Cinema v9)** | **Not Installed on Host** | Commercial License | High-end procedural vegetation authoring with wind SDK. *Alternative for zero-cost:* Blender Modular Tree / Geometry Node foliage pipeline with vertex color wind packing. |
| **Houdini (SideFX 20+)** | **Not Installed on Host** | Commercial / Apprentice | Procedural rubble distribution, procedural destruction simulation, and automated LOD baking. *Alternative:* Blender Python procedural geometry scripts. |
| **Gaea (QuadSpinner)** | **Not Installed on Host** | Commercial / Community | Landscape heightmap generation, mountain silhouettes, and hydraulic erosion masks for Greyfall County terrain. |
| **Fab (Epic Games)** | **Cloud / Web / Bridge** | Free / Licensed | High-fidelity photogrammetry scans, modular architecture components, and surface decals. |
| **Poly Haven** | **Cloud / API (Verified CC0)** | Open Source (CC0) | Photorealistic HDRIs for environmental lighting and physically accurate PBR textures (cracked asphalt, weathered concrete, rusted corrugated metal). |
| **ambientCG** | **Cloud / API (Verified CC0)** | Open Source (CC0) | High-resolution PBR surface materials with 16-bit displacement, normal maps, and roughness maps. |

---

### 4. External Toolchain Integration Standards

#### A. Blender to Unity 6 Pipeline
- **Unit Scale:** 1 Blender Unit = 1.0 Meter in Unity. FBX export scale set to `1.0` with `Apply Scalings: FBX Units Scale`.
- **Coordinate System:** Forward = `-Z`, Up = `Y` (Unity standard).
- **Naming Conventions:**
  - Geometry: `SM_<Category>_<AssetName>_<Variant>` (e.g., `SM_Arch_Shopfront_01_Damaged`).
  - Collision Hulls: `UCX_<AssetName>_<Index>` (e.g., `UCX_SM_Arch_Shopfront_01_Damaged_01`).
  - LODs: `<AssetName>_LOD0`, `<AssetName>_LOD1`, `<AssetName>_LOD2`.
- **UV Layout Standards:**
  - UV Channel 0: Tiling and unique texture maps.
  - UV Channel 1: Clean, non-overlapping lightmap UVs (minimum 4-pixel padding at 1024 resolution).

#### B. Substance Painter to Unity URP Export Preset
- **Preset Name:** `Unity_URP_Photoreal_Mobile`
- **Output Channel Maps:**
  1. `_BaseColor`: RGB (sRGB) + Alpha (Transparency or Cutout Mask).
  2. `_Normal`: RGB (Linear) with DirectX format (Y- inverted if authored in OpenGL).
  3. `_MaskMap` (Channel Packed Linear):
     - **R:** Metallic (Grayscale)
     - **G:** Ambient Occlusion (Grayscale)
     - **B:** Detail / Curvature Mask (Grayscale)
     - **A:** Smoothness (Inverted Roughness)
  4. `_Emission`: RGB (sRGB HDR).

#### C. CC0 Cloud Asset Ingestion Pipeline
- Automated downloading and extraction of verified CC0 assets from Poly Haven and ambientCG using Python scripts directly into `Assets/ECLIPSE/Environment/Textures/` and `Assets/ECLIPSE/Environment/Materials/`.

---

### 5. Recommended Toolchain Additions (Pending User Approval)

To transition from architectural planning to photorealistic visual execution, the following package manifest updates are recommended:

```json
{
  "dependencies": {
    "com.unity.shadergraph": "17.0.3",
    "com.unity.visualeffectgraph": "17.0.3",
    "com.unity.splines": "2.6.1",
    "com.unity.terrain-tools": "5.0.3",
    "com.unity.adaptiveperformance": "5.1.0",
    "com.unity.adaptiveperformance.samsung.android": "5.1.0"
  }
}
```

*Status: Awaiting user approval before applying changes to `Packages/manifest.json`.*
