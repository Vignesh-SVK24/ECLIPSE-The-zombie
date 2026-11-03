# ECLIPSE: THE LAST CIVILIZATION
## Master Art Transformation Plan & Staged Implementation Roadmap

**Studio:** SVK Creationz  
**Engine:** Unity 6 + Universal Render Pipeline (URP 17.0.3)  
**Target Visual Benchmark:** Master Concept Key Art (`Assets/ECLIPSE/Art/Concept/ECLIPSE_Master_KeyArt_Stylized.jpg`)  
**Platforms:** Android, iOS, PC  
**Document Ref:** `ECLIPSE_ART_TRANSFORMATION_PLAN.md`  

---

### 1. Vision & Core Creative Objectives

The objective is to elevate *ECLIPSE: THE LAST CIVILIZATION* into a **visually cohesive, premium stylized 3D animated survival game** that matches the character, energy, color, and storytelling of the master concept artwork.

```
                    MASTER VISUAL TRANSFORMATION PILLARS
┌─────────────────────────────────┬─────────────────────────────────┐
│ 1. Stylized Cartoon-Realism     │ 2. Signature Color & Lighting   │
│ • Bold heroic character anatomy │ • Golden hour cinematic sunsets │
│ • Expressive planar facial cuts │ • Warm amber/gold sunbeams      │
│ • Clean, readable silhouettes   │ • Cool purple/violet shadows    │
│ • Hand-painted PBR materials    │ • Toxic green zombie eye glows  │
├─────────────────────────────────┼─────────────────────────────────┤
│ 3. Tactile Fortified World      │ 4. Premium Tactical UI System   │
│ • Rough-hewn timber palisades   │ • Distressed stencil typography │
│ • Sandbag & fuel-drum bunkers   │ • Tactile gunmetal frames       │
│ • "SURVIVE TOGETHER" stencils   │ • Rarity-coded inventory cards  │
│ • Water tower "HOPE LIVES"      │ • Ergonomic mobile touch HUD    │
└─────────────────────────────────┴─────────────────────────────────┘
```

---

### 2. Six-Batch Phased Implementation Roadmap

#### Batch A — Audit, Toolchain Setup & Foundation Prototype (CURRENT BATCH)
- **Scope:**
  - Execute full technical audit of Unity 6, URP, package dependencies, and host tools.
  - Formulate AI tool integration pipelines (Scenario, Meshy, Tripo, Blender).
  - Establish Master Visual Style Guide, UI Design System, and Asset Generation Guide.
  - Build initial stylized UI and graphics prototype verifying shaders, lighting, and HUD.
- **Verification Gate:**
  - Zero gameplay regressions; shaders compile cleanly; local playable client updated.

#### Batch B — Complete UI System Overhaul
- **Scope:**
  - Implement full Unity uGUI / TextMeshPro component library under `Assets/_Project/Scripts/UI/`.
  - Build Cinematic Main Menu with animated Ethan Cross character vignette and logo.
  - Implement tactical In-Game HUD (curved vitals meters, ammo counter, threat compass).
  - Build full Inventory Screen (3D character paperdoll, weapon slots, backpack grid, item cards).
  - Deploy optimized Mobile Touch HUD with tactile buttons, dynamic joysticks, and safe area padding.
- **Verification Gate:**
  - Fully navigable menu transitions in Unity Editor Play Mode and WebGL client.

#### Batch C — Stylized Character & Zombie Roster Overhaul
- **Scope:**
  - Implement Ethan Cross stylized hero model (crimson hoodie, tactical vest, heavy backpack, assault rifle).
  - Implement stylized Walker Zombie with decaying olive-gray flesh and **signature glowing toxic green eyes**.
  - Implement Sarah Mitchell (Medic) survivor archetype.
  - Bind Humanoid Avatars, test animation blend trees (idle, walk, run, aim ADS, reload, hit, die).
- **Verification Gate:**
  - All existing combat colliders, sub-collider hitboxes, and weapon attachment sockets preserve 100% alignment.

#### Batch D — Stylized Environment Prototype: Greyfall Evacuation Street
- **Scope:**
  - Transform Greyfall City Sector 4 into the stylized fortified street depicted in the concept art:
    - Timber palisade barricades with barbed wire and "SURVIVE TOGETHER" red paint.
    - Sandbags stacked on red fuel barrels with ammunition crates and brass kerosene lanterns.
    - Water tower with "HOPE LIVES" slogan and elevated timber watchtower with clan banner.
    - Weathered transit bus overgrown with climbing ivy.
    - Warm sunset directional lighting, volumetric evening fog, and golden sunbeams.
- **Verification Gate:**
  - 60 FPS maintained on mobile quality presets; NavMesh obstacle carving clean for zombie horde navigation.

#### Batch E — Game-Wide World & Asset Integration
- **Scope:**
  - Propagate stylized PBR master materials across remaining game zones:
    - Blackwood Forest (stylized pines, cabins, streams, forest fog).
    - Ironshield Military Base (reinforced bunkers, armored transports, searchlight watchtowers).
    - Ashen Industrial District (brick warehouses, rusted overhead pipes, smokestacks).
    - ECLIPSE Research Complex (clean laboratory corridors, containment vaults, biohazard vats).
  - Finalize remaining survivor models (Marcus Vance, Elena Rostova, Sergeant Jackson) and zombie classes (Runner, Stalker, Brute, Alpha Boss).
- **Verification Gate:**
  - Consistent visual identity across all 5 zones and 100% of characters.

#### Batch F — Mobile Optimization, VFX Polish & Final QA
- **Scope:**
  - Tune Low, Medium, High graphics profiles with Adaptive Performance integration.
  - Optimize draw calls via SRP Batcher and GPU Resident Drawer (< 120 calls on mobile).
  - Final pass on stylized VFX (muzzle flashes, blood splatters, rain splashes, vehicle fire).
  - End-to-end device testing on Android (Vulkan) and iOS (Metal).
- **Verification Gate:**
  - Rock-solid 60 FPS on mainstream devices; zero memory leaks; zero UI blocking glitches.

---

### 3. Strict Gameplay Protection Protocols

1. **Assembly Definition Integrity:** All UI and visual transformations reside strictly within `Eclipse.UI`, `Eclipse.Character`, and `Eclipse.Environment` assemblies without altering `Eclipse.Combat`, `Eclipse.Damage`, `Eclipse.AI`, or `Eclipse.Networking`.
2. **HitBox & Collider Preservation:** Sub-collider bones (`Head`, `Chest`, `Limbs`) retain exact layer tags and collision geometry to ensure combat math and 2.5x headshot critical multipliers remain untouched.
3. **NavMesh Guarantee:** All new environment props automatically incorporate `NavMeshObstacle` components with runtime carving to preserve zombie AI pathfinding.
4. **Reversible Version Control:** Every batch is committed incrementally to Git with explicit validation tags.
